using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PdfNotetaker
{
    public partial class PdfViewForm : Form
    {
        string folderPath = @"C:\Users\Work 3\Documents\PdfViewer";
        public PdfViewForm()
        {
            InitializeComponent();
            LoadDirectory(folderPath); 
        }


        private void LoadDirectory(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);

            // Create the root directory node
            TreeNode rootNode = new TreeNode(di.Name);
            rootNode.Tag = di;
            treeView1.Nodes.Add(rootNode);

            // Load all subdirectories and files
            LoadSubDirectories(di, rootNode);
        }

        private void LoadSubDirectories(DirectoryInfo directoryInfo, TreeNode parentNode)
        {
            try
            {
                // Load subdirectories
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    TreeNode dirNode = new TreeNode(dir.Name);
                    dirNode.Tag = dir;
                    parentNode.Nodes.Add(dirNode);

                    // Recursively load subdirectories
                    LoadSubDirectories(dir, dirNode);
                }

                // Load files
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    TreeNode fileNode = new TreeNode(file.Name);
                    fileNode.Tag = file;
                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle cases where access to certain folders is denied
            }
        }
    }


}
