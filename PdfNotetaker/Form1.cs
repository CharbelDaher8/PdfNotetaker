using Microsoft.Web.WebView2.WinForms;
using System.Windows.Forms;
using System.Xml;

namespace PdfNotetaker
{
    public partial class Form1 : Form
    {
        string _folderPath = "";
        private bool isFormatting;
        private bool isDarkMode;

        public Form1()
        {
            InitializeComponent();
            webView21.Source = new Uri("C:\\Users\\Work 3\\Downloads\\Lasch_Christopher_The_Culture_of_Narcissism.pdf");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileNameWithoutExtension(selectedFilePath);

                // Get the currently selected TabPage
                TabPage currentTabPage = tabControl1.SelectedTab;
                if (currentTabPage != null)
                {
                    // Find the WebView2 control in the selected TabPage
                    WebView2 webView2 = FindWebView2(currentTabPage);
                    if (webView2 != null)
                    {
                        // Set the PDF file path
                        webView2.Source = new Uri(selectedFilePath);
                        CreateFolderForPdf(selectedFilePath);

                        currentTabPage.Text = fileName;
                    }
                    else
                    {
                        MessageBox.Show("WebView2 control not found in the selected tab.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("No tab is currently selected.", "Error");
                }
            }
        }

        private WebView2 FindWebView2(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is WebView2 webView2)
                {
                    return webView2;
                }

                if (control.HasChildren)
                {
                    WebView2 found = FindWebView2(control);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
            return null;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTextToFile();
        }
        private void SaveTextToFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
            saveFileDialog.InitialDirectory = _folderPath;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllText(filePath, richTextBox1.Text);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void CreateFolderForPdf(string pdfFilePath)
        {
            try
            {
                string pdfFileName = Path.GetFileNameWithoutExtension(pdfFilePath);

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string pdfViewerFolder = Path.Combine(documentsPath, "PdfViewer");

                string pdfFolderPath = Path.Combine(pdfViewerFolder, pdfFileName);

                if (!Directory.Exists(pdfViewerFolder))
                {
                    Directory.CreateDirectory(pdfViewerFolder);
                }

                if (!Directory.Exists(pdfFolderPath))
                {
                    Directory.CreateDirectory(pdfFolderPath);
                    MessageBox.Show("Folder created: " + pdfFolderPath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Folder already exists: " + pdfFolderPath, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                _folderPath = pdfFolderPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewTabWithSplitContainer();
        }

        private void AddNewTabWithSplitContainer()
        {
            // Create a new TabPage
            TabPage newTabPage = new TabPage($"Tab {tabControl1.TabPages.Count + 1}");

            // Create a SplitContainer
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical // Vertical split
            };

            // Create and configure RichTextBox for the first panel
            RichTextBox richTextBox = new RichTextBox
            {
                Name = $"RichTextBox_{tabControl1.TabPages.Count + 1}", // Name incremented
                Dock = DockStyle.Fill
            };
            splitContainer.Panel1.Controls.Add(richTextBox);

            // Create and configure WebBrowser for the second panel
            WebView2 webBrowser = new WebView2
            {
                Name = $"WebView2_{tabControl1.TabPages.Count + 1}", // Name incremented
                Dock = DockStyle.Fill
            };
            splitContainer.Panel2.Controls.Add(webBrowser);

            // Add SplitContainer to the TabPage
            newTabPage.Controls.Add(splitContainer);

            // Add the TabPage to the TabControl
            tabControl1.TabPages.Add(newTabPage);
        }

        private void ToggleDarkMode()
        {
            // Toggle the dark mode flag
            isDarkMode = !isDarkMode;

            // Set colors based on the dark mode flag
            Color backgroundColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            Color foregroundColor = isDarkMode ? Color.White : Color.Black;

            // Apply colors to the form
            BackColor = backgroundColor;
            ForeColor = foregroundColor;

            // Apply colors to the tab control and tabs
            tabControl1.BackColor = backgroundColor;
            tabControl1.ForeColor = foregroundColor;

            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                tabPage.BackColor = backgroundColor;
                tabPage.ForeColor = foregroundColor;

                // Apply colors to controls within each tab
                ApplyDarkModeToControls(tabPage.Controls, backgroundColor, foregroundColor);
            }
        }

        private void ApplyDarkModeToControls(Control.ControlCollection controls, Color backgroundColor, Color foregroundColor)
        {
            foreach (Control control in controls)
            {
                if (control is RichTextBox richTextBox)
                {
                    richTextBox.BackColor = backgroundColor;
                    richTextBox.ForeColor = foregroundColor;
                }
                else if (control is WebView2 webView2)
                {
                    webView2.BackColor = backgroundColor; // WebView2 may need custom styling
                    webView2.ForeColor = foregroundColor; // WebView2 may need custom styling
                }
                else if (control is SplitContainer splitContainer)
                {
                    splitContainer.BackColor = backgroundColor;
                    splitContainer.Panel1.BackColor = backgroundColor;
                    splitContainer.Panel2.BackColor = backgroundColor;

                    ApplyDarkModeToControls(splitContainer.Panel1.Controls, backgroundColor, foregroundColor);
                    ApplyDarkModeToControls(splitContainer.Panel2.Controls, backgroundColor, foregroundColor);
                }
                else
                {
                    control.BackColor = backgroundColor;
                    control.ForeColor = foregroundColor;

                    if (control.HasChildren)
                    {
                        ApplyDarkModeToControls(control.Controls, backgroundColor, foregroundColor);
                    }
                }
            }

        }

        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleDarkMode(); 
        }
    }
}
