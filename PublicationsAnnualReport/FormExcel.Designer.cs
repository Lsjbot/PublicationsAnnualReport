
namespace PublicationsAnnualReport
{
    partial class FormExcel
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
            this.Excelbutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RB_originstfile = new System.Windows.Forms.RadioButton();
            this.RB_HDafile = new System.Windows.Forms.RadioButton();
            this.RB_instfile = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RB_scopussubject = new System.Windows.Forms.RadioButton();
            this.RB_scopuscategory = new System.Windows.Forms.RadioButton();
            this.RB_groupfile = new System.Windows.Forms.RadioButton();
            this.RB_authorsubject = new System.Windows.Forms.RadioButton();
            this.GBinst = new System.Windows.Forms.GroupBox();
            this.RBnotabs = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Excelbutton
            // 
            this.Excelbutton.Location = new System.Drawing.Point(568, 30);
            this.Excelbutton.Name = "Excelbutton";
            this.Excelbutton.Size = new System.Drawing.Size(206, 62);
            this.Excelbutton.TabIndex = 0;
            this.Excelbutton.Text = "Skapa Excel";
            this.Excelbutton.UseVisualStyleBackColor = true;
            this.Excelbutton.Click += new System.EventHandler(this.Excelbutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(32, 30);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(396, 368);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_originstfile);
            this.groupBox1.Controls.Add(this.RB_HDafile);
            this.groupBox1.Controls.Add(this.RB_instfile);
            this.groupBox1.Location = new System.Drawing.Point(568, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(206, 103);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Delas i filer:";
            // 
            // RB_originstfile
            // 
            this.RB_originstfile.AutoSize = true;
            this.RB_originstfile.Location = new System.Drawing.Point(6, 44);
            this.RB_originstfile.Name = "RB_originstfile";
            this.RB_originstfile.Size = new System.Drawing.Size(127, 17);
            this.RB_originstfile.TabIndex = 2;
            this.RB_originstfile.TabStop = true;
            this.RB_originstfile.Text = "Efter originalinstitution";
            this.RB_originstfile.UseVisualStyleBackColor = true;
            // 
            // RB_HDafile
            // 
            this.RB_HDafile.AutoSize = true;
            this.RB_HDafile.Location = new System.Drawing.Point(6, 70);
            this.RB_HDafile.Name = "RB_HDafile";
            this.RB_HDafile.Size = new System.Drawing.Size(102, 17);
            this.RB_HDafile.TabIndex = 1;
            this.RB_HDafile.TabStop = true;
            this.RB_HDafile.Text = "Hela HDa i en fil";
            this.RB_HDafile.UseVisualStyleBackColor = true;
            // 
            // RB_instfile
            // 
            this.RB_instfile.AutoSize = true;
            this.RB_instfile.Checked = true;
            this.RB_instfile.Location = new System.Drawing.Point(7, 20);
            this.RB_instfile.Name = "RB_instfile";
            this.RB_instfile.Size = new System.Drawing.Size(94, 17);
            this.RB_instfile.TabIndex = 0;
            this.RB_instfile.TabStop = true;
            this.RB_instfile.Text = "Efter institution";
            this.RB_instfile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RBnotabs);
            this.groupBox2.Controls.Add(this.RB_scopussubject);
            this.groupBox2.Controls.Add(this.RB_scopuscategory);
            this.groupBox2.Controls.Add(this.RB_groupfile);
            this.groupBox2.Controls.Add(this.RB_authorsubject);
            this.groupBox2.Location = new System.Drawing.Point(568, 233);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 141);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Delas i flikar:";
            // 
            // RB_scopussubject
            // 
            this.RB_scopussubject.AutoSize = true;
            this.RB_scopussubject.Location = new System.Drawing.Point(6, 92);
            this.RB_scopussubject.Name = "RB_scopussubject";
            this.RB_scopussubject.Size = new System.Drawing.Size(115, 17);
            this.RB_scopussubject.TabIndex = 3;
            this.RB_scopussubject.TabStop = true;
            this.RB_scopussubject.Text = "Efter Scopus-ämne";
            this.RB_scopussubject.UseVisualStyleBackColor = true;
            // 
            // RB_scopuscategory
            // 
            this.RB_scopuscategory.AutoSize = true;
            this.RB_scopuscategory.Location = new System.Drawing.Point(7, 68);
            this.RB_scopuscategory.Name = "RB_scopuscategory";
            this.RB_scopuscategory.Size = new System.Drawing.Size(127, 17);
            this.RB_scopuscategory.TabIndex = 2;
            this.RB_scopuscategory.TabStop = true;
            this.RB_scopuscategory.Text = "Efter Scopus-kategori";
            this.RB_scopuscategory.UseVisualStyleBackColor = true;
            // 
            // RB_groupfile
            // 
            this.RB_groupfile.AutoSize = true;
            this.RB_groupfile.Checked = true;
            this.RB_groupfile.Location = new System.Drawing.Point(7, 44);
            this.RB_groupfile.Name = "RB_groupfile";
            this.RB_groupfile.Size = new System.Drawing.Size(88, 17);
            this.RB_groupfile.TabIndex = 1;
            this.RB_groupfile.TabStop = true;
            this.RB_groupfile.Text = "Enligt gruppfil";
            this.RB_groupfile.UseVisualStyleBackColor = true;
            // 
            // RB_authorsubject
            // 
            this.RB_authorsubject.AutoSize = true;
            this.RB_authorsubject.Location = new System.Drawing.Point(7, 20);
            this.RB_authorsubject.Name = "RB_authorsubject";
            this.RB_authorsubject.Size = new System.Drawing.Size(112, 17);
            this.RB_authorsubject.TabIndex = 0;
            this.RB_authorsubject.Text = "Efter författarämne";
            this.RB_authorsubject.UseVisualStyleBackColor = true;
            // 
            // GBinst
            // 
            this.GBinst.Location = new System.Drawing.Point(568, 383);
            this.GBinst.Name = "GBinst";
            this.GBinst.Size = new System.Drawing.Size(220, 304);
            this.GBinst.TabIndex = 4;
            this.GBinst.TabStop = false;
            this.GBinst.Text = "Institutioner";
            // 
            // RBnotabs
            // 
            this.RBnotabs.AutoSize = true;
            this.RBnotabs.Location = new System.Drawing.Point(7, 115);
            this.RBnotabs.Name = "RBnotabs";
            this.RBnotabs.Size = new System.Drawing.Size(76, 17);
            this.RBnotabs.TabIndex = 4;
            this.RBnotabs.TabStop = true;
            this.RBnotabs.Text = "(inga flikar)";
            this.RBnotabs.UseVisualStyleBackColor = true;
            // 
            // FormExcel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 699);
            this.Controls.Add(this.GBinst);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Excelbutton);
            this.Name = "FormExcel";
            this.Text = "FormExcel";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Excelbutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_HDafile;
        private System.Windows.Forms.RadioButton RB_instfile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton RB_groupfile;
        private System.Windows.Forms.RadioButton RB_authorsubject;
        private System.Windows.Forms.RadioButton RB_scopuscategory;
        private System.Windows.Forms.GroupBox GBinst;
        private System.Windows.Forms.RadioButton RB_originstfile;
        private System.Windows.Forms.RadioButton RB_scopussubject;
        private System.Windows.Forms.RadioButton RBnotabs;
    }
}