﻿namespace PdfNotetaker
{
    partial class PdfViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            treeView1 = new TreeView();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.BackColor = SystemColors.Control;
            treeView1.BorderStyle = BorderStyle.None;
            treeView1.Location = new Point(2, 12);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(164, 828);
            treeView1.TabIndex = 0;
            // 
            // PdfViewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1062, 841);
            Controls.Add(treeView1);
            Name = "PdfViewForm";
            Text = "PdfViewForm";
            ResumeLayout(false);
        }

        #endregion

        private TreeView treeView1;
    }
}