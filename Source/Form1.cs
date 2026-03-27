using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFtab
{
    public partial class MainForm : Form
    {
        private List<string> _documents = new List<string>();
        private string _outputPath = "";
        private CancellationTokenSource _cancellationTokenSource;
        private readonly object _logLock = new object();
        private readonly List<string> _logMessages = new List<string>();
        private readonly string _logFilePath;
        private int _processedCount = 0;

        public MainForm()
        {
            InitializeComponent();
            QuestPDF.Settings.License = LicenseType.Community;

            _logFilePath = Path.Combine(Application.StartupPath, "PDFtab.log");

            InitializeUI();
            LoadLog();
        }

        private void InitializeUI()
        {
            btnLoad.Click += btnLoad_Click;
            btnExecuteCSV.Click += btnExecuteCSV_Click;
            btnCancel.Click += btnCancel_Click;
            btnDestination.Click += btnDestination_Click;
            btnOpenFolder.Click += btnOpenFolder_Click;
            btnLog.Click += btnLog_Click;
            btnClear.Click += btnClear_Click;

            chkDestination.CheckedChanged += chkDestination_CheckedChanged;
            chkFrame.CheckedChanged += chkFrame_CheckedChanged;  // Neu: Event für Checkbox

            lvDoc.View = View.Details;
            lvDoc.Columns.Add("Dateiname", 350);
            lvDoc.Columns.Add("Status", 100);
            lvDoc.FullRowSelect = true;
            lvDoc.AllowDrop = true;

            lvDoc.DragEnter += lvDoc_DragEnter;
            lvDoc.DragDrop += lvDoc_DragDrop;

            lblStatus.Text = "Bereit";
            pbStatus.Value = 0;

            btnDestination.Enabled = chkDestination.Checked;
            btnCancel.Enabled = false;
        }

        // Neu: Event-Handler für chkFrame
        private void chkFrame_CheckedChanged(object sender, EventArgs e)
        {
            // Optional: UI-Feedback
            lblStatus.Text = chkFrame.Checked ? "Rahmen für Tabellen aktiviert" : "Rahmen deaktiviert";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _documents.Clear();
            lvDoc.Items.Clear();
            _processedCount = 0;
            UpdateProgress(0);
            lblStatus.Text = "Liste geleert";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "CSV Dateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var path in dialog.FileNames)
                    {
                        if (Directory.Exists(path))
                            LoadFolder(path);
                        else if (File.Exists(path) &&
                                 path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                            AddDocument(path);
                    }
                }
            }
        }

        private void LoadFolder(string folder)
        {
            var files = Directory.GetFiles(folder, "*.csv", SearchOption.AllDirectories);
            foreach (var file in files)
                AddDocument(file);

            UpdateStatusSafe($"{files.Length} CSV Dateien aus Ordner geladen");
        }

        private void AddDocument(string file)
        {
            if (!_documents.Contains(file))
            {
                _documents.Add(file);
                AddToListView(file, "Geladen");
            }
        }

        private void lvDoc_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void lvDoc_DragDrop(object sender, DragEventArgs e)
        {
            var paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                    LoadFolder(path);
                else if (File.Exists(path) &&
                         path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    AddDocument(path);
            }
        }

        private void AddToListView(string filePath, string status)
        {
            var item = new ListViewItem(Path.GetFileName(filePath));
            item.SubItems.Add(status);
            item.Tag = filePath;
            lvDoc.Items.Add(item);
        }

        private async void btnExecuteCSV_Click(object sender, EventArgs e)
        {
            if (_documents.Count == 0)
            {
                lblStatus.Text = "Keine CSV Dateien geladen!";
                return;
            }

            _processedCount = 0;
            UpdateProgress(0);

            btnExecuteCSV.Enabled = false;
            btnCancel.Enabled = true;

            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await ConvertAllCSVsAsync();
            }
            catch (OperationCanceledException)
            {
                UpdateStatusSafe("Abgebrochen");
            }
            finally
            {
                btnExecuteCSV.Enabled = true;
                btnCancel.Enabled = false;

                if (chkOpen.Checked)
                    OpenOutputFolder();
            }
        }

        private async Task ConvertAllCSVsAsync()
        {
            int total = _documents.Count;

            await Task.Run(() =>
            {
                Parallel.ForEach(_documents, new ParallelOptions
                {
                    CancellationToken = _cancellationTokenSource.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                csvFile =>
                {
                    try
                    {
                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                        UpdateListViewStatus(csvFile, "Konvertiere...");

                        string outputFile = GetOutputFilePath(csvFile);
                        Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

                        ConvertCSVToPDF(csvFile, outputFile);

                        int done = Interlocked.Increment(ref _processedCount);

                        UpdateListViewStatus(csvFile, "✓ Fertig");
                        LogMessage($"✓ {Path.GetFileName(csvFile)}");

                        int percent = (int)((done * 100.0) / total);
                        UpdateProgress(percent);
                        UpdateStatusSafe($"Fortschritt: {done}/{total}");
                    }
                    catch (Exception ex)
                    {
                        UpdateListViewStatus(csvFile, "✗ Fehler");
                        LogMessage($"✗ {csvFile}: {ex.Message}");
                    }
                });
            });

            UpdateProgress(100);
            UpdateStatusSafe("Fertig");
        }

        private List<string[]> ParseCsv(string file)
        {
            var result = new List<string[]>();
            foreach (var line in File.ReadLines(file))
            {
                var row = new List<string>();
                bool inQuotes = false;
                string current = "";
                foreach (char c in line)
                {
                    if (c == '"')
                        inQuotes = !inQuotes;
                    else if ((c == ';' || c == ',') && !inQuotes)
                    {
                        row.Add(current);
                        current = "";
                    }
                    else
                        current += c;
                }
                row.Add(current);
                result.Add(row.ToArray());
            }
            return result;
        }

        private void ConvertCSVToPDF(string csvFile, string pdfFile)
        {
            var rows = ParseCsv(csvFile);
            bool drawFrame = chkFrame.Checked;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);

                    if (chkTitle.Checked)
                    {
                        page.Header()
                            .Container()
                            .Padding(10f)
                            .AlignCenter()
                            .Text(Path.GetFileNameWithoutExtension(csvFile))
                            .Bold()
                            .FontSize(16);
                    }

                    page.Content().Table(table =>
                    {
                        int columnCount = rows.Max(r => r.Length);

                        table.ColumnsDefinition(columns =>
                        {
                            for (int i = 0; i < columnCount; i++)
                                columns.RelativeColumn(1u); // uint statt int
                        });

                        if (rows.Count > 0)
                        {
                            // Header mit Rahmen und Farbe, wenn aktiviert
                            table.Header(header =>
                            {
                                foreach (var cellText in rows[0])
                                {
                                    header.Cell().Container()
                                        .Padding(4f)
                                        .Border(drawFrame ? 1f : 0f)
                                        .BorderColor(drawFrame ? Colors.Grey.Medium : Colors.Transparent)
                                        .Background(drawFrame ? Colors.Grey.Lighten4 : Colors.Transparent)
                                        .Text(cellText)
                                        .SemiBold()
                                        .FontSize(11);
                                }
                            });

                            // Datenzeilen mit feinen Trennlinien
                            for (int i = 1; i < rows.Count; i++)
                            {
                                var rowIndex = i; // Für Closure
                                foreach (var cellText in rows[i])
                                {
                                    table.Cell().Container()
                                        .Padding(4f)
                                        .BorderBottom(drawFrame ? 0.5f : 0f)
                                        .BorderRight(drawFrame ? 0.5f : 0f)
                                        .BorderColor(drawFrame ? Colors.Grey.Lighten2 : Colors.Transparent)
                                        .Text(cellText)
                                        .FontSize(10);
                                }
                            }
                        }
                        else
                        {
                            table.Cell().ColumnSpan((uint)columnCount)
                                .AlignCenter()
                                .Text("Keine Daten gefunden");
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
            }).GeneratePdf(pdfFile);
        }

        private string GetOutputFilePath(string csvFile)
        {
            string dir = chkDestination.Checked && !string.IsNullOrEmpty(_outputPath)
                ? _outputPath
                : Path.GetDirectoryName(csvFile);

            return Path.Combine(dir, Path.GetFileNameWithoutExtension(csvFile) + ".pdf");
        }

        private string GetOutputFolder()
        {
            return chkDestination.Checked && !string.IsNullOrEmpty(_outputPath)
                ? _outputPath
                : Path.GetDirectoryName(_documents.FirstOrDefault() ?? "");
        }

        private void UpdateListViewStatus(string filePath, string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateListViewStatus(filePath, status)));
                return;
            }

            foreach (ListViewItem item in lvDoc.Items)
            {
                if ((string)item.Tag == filePath)
                    item.SubItems[1].Text = status;
            }
        }

        private void UpdateProgress(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProgress(value)));
                return;
            }

            pbStatus.Value = Math.Max(0, Math.Min(100, value));
        }

        private void UpdateStatusSafe(string text)
        {
            if (InvokeRequired)
                Invoke(new Action(() => lblStatus.Text = text));
            else
                lblStatus.Text = text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            lblStatus.Text = "Abbruch...";
        }

        private void chkDestination_CheckedChanged(object sender, EventArgs e)
        {
            btnDestination.Enabled = chkDestination.Checked;
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _outputPath = fbd.SelectedPath;
                    lblStatus.Text = $"Ziel: {Path.GetFileName(_outputPath)}";
                }
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenOutputFolder();
        }

        private void OpenOutputFolder()
        {
            string folder = GetOutputFolder();
            if (Directory.Exists(folder))
                Process.Start("explorer.exe", folder);
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", _logFilePath);
        }

        private void LogMessage(string message)
        {
            lock (_logLock)
            {
                string entry = $"{DateTime.Now:HH:mm:ss} - {message}";
                _logMessages.Add(entry);
                File.WriteAllLines(_logFilePath, _logMessages);
            }
        }

        private void LoadLog()
        {
            if (File.Exists(_logFilePath))
                _logMessages.AddRange(File.ReadAllLines(_logFilePath));
        }
    }
}
