using Microsoft.Web.WebView2.WinForms;
using System.Windows.Forms;
using System.Xml;
using System.Security.AccessControl;


namespace PdfNotetaker
{
    public partial class Form1 : Form
    {
        string _folderPath = "";
        string _noteSavePath = "";
        private bool isDarkMode;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFolderSelector();
        }

        private void openFolderSelector()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                string fileName = Path.GetFileNameWithoutExtension(selectedFilePath);

                TabPage currentTabPage = tabControl1.SelectedTab;
                if (currentTabPage != null)
                {
                    WebView2 webView2 = FindWebView2(currentTabPage);
                    if (webView2 != null)
                    {
                        webView2.Source = new Uri(selectedFilePath);
                        CreateFolderForPdf(selectedFilePath, currentTabPage);

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

        private RichTextBox FindRichTextBox(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is RichTextBox richTextBox)
                {
                    return richTextBox;
                }

                if (control.HasChildren)
                {
                    RichTextBox found = FindRichTextBox(control);
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

            if (_noteSavePath.Length == 0)
            {
                string filePath = _folderPath + "Notes.txt";
                _noteSavePath = filePath;

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
            else
            {
                GrantFileAccess(_noteSavePath, Environment.UserName);
                File.WriteAllText(_noteSavePath, richTextBox1.Text);
            }
        }

        private void CreateFolderForPdf(string pdfFilePath, TabPage currentTabPage)
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
                    string notesFilePath = Path.Combine(pdfFolderPath, "Notes.txt");

                    if (File.Exists(notesFilePath))
                    {
                        var textbox = FindRichTextBox(currentTabPage);
                        // Read all the content of the Notes.txt file
                        string fileContent = File.ReadAllText(notesFilePath);

                        if (textbox != null)
                        {
                            textbox.Text = fileContent;
                        }

                    }
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
            TabPage newTabPage = new TabPage($"Tab {tabControl1.TabPages.Count + 1}");

            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };

            RichTextBox richTextBox = new RichTextBox
            {
                Name = $"RichTextBox_{tabControl1.TabPages.Count + 1}",
                Dock = DockStyle.Fill
            };
            splitContainer.Panel1.Controls.Add(richTextBox);

            WebView2 webBrowser = new WebView2
            {
                Name = $"WebView2_{tabControl1.TabPages.Count + 1}",
                Dock = DockStyle.Fill
            };
            splitContainer.Panel2.Controls.Add(webBrowser);

            newTabPage.Controls.Add(splitContainer);

            tabControl1.TabPages.Add(newTabPage);
        }

        private void ToggleDarkMode()
        {
            isDarkMode = !isDarkMode;

            Color backgroundColor = isDarkMode ? Color.FromArgb(30, 30, 30) : Color.White;
            Color foregroundColor = isDarkMode ? Color.White : Color.Black;

            BackColor = backgroundColor;
            ForeColor = foregroundColor;

            tabControl1.BackColor = backgroundColor;
            tabControl1.ForeColor = foregroundColor;

            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                tabPage.BackColor = backgroundColor;
                tabPage.ForeColor = foregroundColor;

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
                    webView2.BackColor = backgroundColor;
                    webView2.ForeColor = foregroundColor;
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (!commandLine.Visible)
                {
                    commandLine.Visible = true;
                    commandLine.Focus();
                }
            }
            else if (e.Control && e.KeyCode == Keys.T)
            {
                AddNewTabWithSplitContainer();
            }
            else if (e.Control && e.KeyCode == Keys.W)
            {
                if (tabControl1.SelectedIndex >= 0)
                    tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }

        }

        private void CommandLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox commandLine = (TextBox)sender;
                string command = commandLine.Text;

                HandleCommand(command);

                commandLine.Clear();
                commandLine.Visible = false;
            }
        }

        private void HandleCommand(string command)
        {
            if (command == ":w")
            {
                SaveTextToFile();
            }
            else if (command == ":q")
            {
                this.Close();
            }
            else if (command == ":wq")
            {
                SaveTextToFile();
                this.Close();
            }
            else if (command == ":o")
            {
                openFolderSelector();
            }
            else
            {
                ;
            }
        }

        public static void GrantFileAccess(string filePath, string user)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            FileSecurity fileSecurity = fileInfo.GetAccessControl();

            FileSystemAccessRule accessRule = new FileSystemAccessRule(user,
                FileSystemRights.FullControl, AccessControlType.Allow);

            fileSecurity.AddAccessRule(accessRule);
            fileInfo.SetAccessControl(fileSecurity);
        }

        private void viewPDFsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PdfViewForm pdfViewForm = new PdfViewForm();
            pdfViewForm.ShowDialog();
        }
    }
}
