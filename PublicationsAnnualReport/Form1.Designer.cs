namespace PublicationsAnnualReport
{
    partial class Form1
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
            this.Quitbutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ReadDIVAbutton = new System.Windows.Forms.Button();
            this.PubStatsButton = new System.Windows.Forms.Button();
            this.whopublishedbutton = new System.Windows.Forms.Button();
            this.AnnrepTableButton = new System.Windows.Forms.Button();
            this.CB_publists = new System.Windows.Forms.CheckBox();
            this.TB_startyear = new System.Windows.Forms.TextBox();
            this.TB_endyear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_worddir = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TBupprattad = new System.Windows.Forms.TextBox();
            this.TBkontrollerad = new System.Windows.Forms.TextBox();
            this.ProfileSubjectButton = new System.Windows.Forms.Button();
            this.institutionbutton = new System.Windows.Forms.Button();
            this.scopusbutton = new System.Windows.Forms.Button();
            this.corpusbutton = new System.Windows.Forms.Button();
            this.CB_perinst = new System.Windows.Forms.CheckBox();
            this.Journalbutton = new System.Windows.Forms.Button();
            this.CBanyauthorpos = new System.Windows.Forms.CheckBox();
            this.CBfraction = new System.Windows.Forms.CheckBox();
            this.SubjectTabButton = new System.Windows.Forms.Button();
            this.RISbutton = new System.Windows.Forms.Button();
            this.CB_gephi = new System.Windows.Forms.CheckBox();
            this.CB_csvR = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbHelg = new System.Windows.Forms.ComboBox();
            this.Wordcloudbutton = new System.Windows.Forms.Button();
            this.SearchStringbutton = new System.Windows.Forms.Button();
            this.CBannrep = new System.Windows.Forms.CheckBox();
            this.Authorlistbutton = new System.Windows.Forms.Button();
            this.KTHbutton = new System.Windows.Forms.Button();
            this.Primulabutton = new System.Windows.Forms.Button();
            this.Topauthorbutton = new System.Windows.Forms.Button();
            this.Citebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(639, 436);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(138, 66);
            this.Quitbutton.TabIndex = 0;
            this.Quitbutton.Text = "Avsluta";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(22, 25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(509, 393);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ReadDIVAbutton
            // 
            this.ReadDIVAbutton.Location = new System.Drawing.Point(583, 42);
            this.ReadDIVAbutton.Name = "ReadDIVAbutton";
            this.ReadDIVAbutton.Size = new System.Drawing.Size(194, 46);
            this.ReadDIVAbutton.TabIndex = 2;
            this.ReadDIVAbutton.Text = "Läs CAS- och DIVA-fil";
            this.ReadDIVAbutton.UseVisualStyleBackColor = true;
            this.ReadDIVAbutton.Click += new System.EventHandler(this.ReadDIVAbutton_Click);
            // 
            // PubStatsButton
            // 
            this.PubStatsButton.Enabled = false;
            this.PubStatsButton.Location = new System.Drawing.Point(582, 94);
            this.PubStatsButton.Name = "PubStatsButton";
            this.PubStatsButton.Size = new System.Drawing.Size(195, 21);
            this.PubStatsButton.TabIndex = 3;
            this.PubStatsButton.Text = "Publikationsstatistik";
            this.PubStatsButton.UseVisualStyleBackColor = true;
            this.PubStatsButton.Click += new System.EventHandler(this.PubStatsButton_Click);
            // 
            // whopublishedbutton
            // 
            this.whopublishedbutton.Enabled = false;
            this.whopublishedbutton.Location = new System.Drawing.Point(197, 436);
            this.whopublishedbutton.Name = "whopublishedbutton";
            this.whopublishedbutton.Size = new System.Drawing.Size(138, 34);
            this.whopublishedbutton.TabIndex = 4;
            this.whopublishedbutton.Text = "Vem publicerade 2017-2019?";
            this.whopublishedbutton.UseVisualStyleBackColor = true;
            this.whopublishedbutton.Click += new System.EventHandler(this.whopublishedbutton_Click);
            // 
            // AnnrepTableButton
            // 
            this.AnnrepTableButton.Enabled = false;
            this.AnnrepTableButton.Location = new System.Drawing.Point(583, 178);
            this.AnnrepTableButton.Name = "AnnrepTableButton";
            this.AnnrepTableButton.Size = new System.Drawing.Size(196, 30);
            this.AnnrepTableButton.TabIndex = 5;
            this.AnnrepTableButton.Text = "Tabell till årsredovisningen";
            this.AnnrepTableButton.UseVisualStyleBackColor = true;
            this.AnnrepTableButton.Click += new System.EventHandler(this.AnnrepTableButton_Click);
            // 
            // CB_publists
            // 
            this.CB_publists.AutoSize = true;
            this.CB_publists.Location = new System.Drawing.Point(690, 281);
            this.CB_publists.Name = "CB_publists";
            this.CB_publists.Size = new System.Drawing.Size(102, 17);
            this.CB_publists.TabIndex = 6;
            this.CB_publists.Text = "Skapa word-filer";
            this.CB_publists.UseVisualStyleBackColor = true;
            this.CB_publists.CheckedChanged += new System.EventHandler(this.CB_publists_CheckedChanged);
            // 
            // TB_startyear
            // 
            this.TB_startyear.Location = new System.Drawing.Point(611, 17);
            this.TB_startyear.Name = "TB_startyear";
            this.TB_startyear.Size = new System.Drawing.Size(39, 20);
            this.TB_startyear.TabIndex = 7;
            this.TB_startyear.Text = "2018";
            // 
            // TB_endyear
            // 
            this.TB_endyear.Location = new System.Drawing.Point(720, 17);
            this.TB_endyear.Name = "TB_endyear";
            this.TB_endyear.Size = new System.Drawing.Size(47, 20);
            this.TB_endyear.TabIndex = 8;
            this.TB_endyear.Text = "2022";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(559, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Startår";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(671, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Slutår";
            // 
            // TB_worddir
            // 
            this.TB_worddir.Location = new System.Drawing.Point(639, 344);
            this.TB_worddir.Name = "TB_worddir";
            this.TB_worddir.Size = new System.Drawing.Size(138, 20);
            this.TB_worddir.TabIndex = 11;
            this.TB_worddir.Text = "C:\\temp\\";
            this.TB_worddir.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 347);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Mapp för Word-filer:";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(537, 371);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Upprättat av";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(537, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Kontrollerat av";
            // 
            // TBupprattad
            // 
            this.TBupprattad.Location = new System.Drawing.Point(639, 369);
            this.TBupprattad.Name = "TBupprattad";
            this.TBupprattad.Size = new System.Drawing.Size(138, 20);
            this.TBupprattad.TabIndex = 15;
            this.TBupprattad.Text = "Sverker Johansson";
            // 
            // TBkontrollerad
            // 
            this.TBkontrollerad.Location = new System.Drawing.Point(639, 391);
            this.TBkontrollerad.Name = "TBkontrollerad";
            this.TBkontrollerad.Size = new System.Drawing.Size(138, 20);
            this.TBkontrollerad.TabIndex = 16;
            this.TBkontrollerad.Text = "Lisa Grönborg";
            // 
            // ProfileSubjectButton
            // 
            this.ProfileSubjectButton.Location = new System.Drawing.Point(582, 146);
            this.ProfileSubjectButton.Name = "ProfileSubjectButton";
            this.ProfileSubjectButton.Size = new System.Drawing.Size(197, 26);
            this.ProfileSubjectButton.TabIndex = 17;
            this.ProfileSubjectButton.Text = "Kluster profil eller ämne";
            this.ProfileSubjectButton.UseVisualStyleBackColor = true;
            this.ProfileSubjectButton.Click += new System.EventHandler(this.ProfileSubjectButton_Click);
            // 
            // institutionbutton
            // 
            this.institutionbutton.Location = new System.Drawing.Point(474, 436);
            this.institutionbutton.Name = "institutionbutton";
            this.institutionbutton.Size = new System.Drawing.Size(121, 34);
            this.institutionbutton.TabIndex = 18;
            this.institutionbutton.Text = "Fixa institutionslista";
            this.institutionbutton.UseVisualStyleBackColor = true;
            this.institutionbutton.Click += new System.EventHandler(this.institutionbutton_Click);
            // 
            // scopusbutton
            // 
            this.scopusbutton.Location = new System.Drawing.Point(474, 476);
            this.scopusbutton.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.scopusbutton.Name = "scopusbutton";
            this.scopusbutton.Size = new System.Drawing.Size(121, 27);
            this.scopusbutton.TabIndex = 19;
            this.scopusbutton.Text = "Matcha SCOPUS";
            this.scopusbutton.UseVisualStyleBackColor = true;
            this.scopusbutton.Click += new System.EventHandler(this.scopusbutton_Click);
            // 
            // corpusbutton
            // 
            this.corpusbutton.Location = new System.Drawing.Point(340, 476);
            this.corpusbutton.Name = "corpusbutton";
            this.corpusbutton.Size = new System.Drawing.Size(129, 27);
            this.corpusbutton.TabIndex = 20;
            this.corpusbutton.Text = "Skapa författarkorpus";
            this.corpusbutton.UseVisualStyleBackColor = true;
            this.corpusbutton.Click += new System.EventHandler(this.corpusbutton_Click);
            // 
            // CB_perinst
            // 
            this.CB_perinst.AutoSize = true;
            this.CB_perinst.Location = new System.Drawing.Point(690, 264);
            this.CB_perinst.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.CB_perinst.Name = "CB_perinst";
            this.CB_perinst.Size = new System.Drawing.Size(89, 17);
            this.CB_perinst.TabIndex = 21;
            this.CB_perinst.Text = "Per institution";
            this.CB_perinst.UseVisualStyleBackColor = true;
            // 
            // Journalbutton
            // 
            this.Journalbutton.Location = new System.Drawing.Point(340, 436);
            this.Journalbutton.Name = "Journalbutton";
            this.Journalbutton.Size = new System.Drawing.Size(128, 34);
            this.Journalbutton.TabIndex = 22;
            this.Journalbutton.Text = "Tidskriftsdata";
            this.Journalbutton.UseVisualStyleBackColor = true;
            this.Journalbutton.Click += new System.EventHandler(this.Journalbutton_Click);
            // 
            // CBanyauthorpos
            // 
            this.CBanyauthorpos.AutoSize = true;
            this.CBanyauthorpos.Checked = true;
            this.CBanyauthorpos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBanyauthorpos.Location = new System.Drawing.Point(539, 264);
            this.CBanyauthorpos.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CBanyauthorpos.Name = "CBanyauthorpos";
            this.CBanyauthorpos.Size = new System.Drawing.Size(119, 17);
            this.CBanyauthorpos.TabIndex = 23;
            this.CBanyauthorpos.Text = "Oavsett författarpos";
            this.CBanyauthorpos.UseVisualStyleBackColor = true;
            // 
            // CBfraction
            // 
            this.CBfraction.AutoSize = true;
            this.CBfraction.Location = new System.Drawing.Point(539, 283);
            this.CBfraction.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CBfraction.Name = "CBfraction";
            this.CBfraction.Size = new System.Drawing.Size(107, 17);
            this.CBfraction.TabIndex = 24;
            this.CBfraction.Text = "Även fraktionerat";
            this.CBfraction.UseVisualStyleBackColor = true;
            // 
            // SubjectTabButton
            // 
            this.SubjectTabButton.Enabled = false;
            this.SubjectTabButton.Location = new System.Drawing.Point(583, 120);
            this.SubjectTabButton.Margin = new System.Windows.Forms.Padding(2);
            this.SubjectTabButton.Name = "SubjectTabButton";
            this.SubjectTabButton.Size = new System.Drawing.Size(196, 21);
            this.SubjectTabButton.TabIndex = 25;
            this.SubjectTabButton.Text = "Skapa excelflikar per ämne";
            this.SubjectTabButton.UseVisualStyleBackColor = true;
            this.SubjectTabButton.Click += new System.EventHandler(this.SubjectTabButton_Click);
            // 
            // RISbutton
            // 
            this.RISbutton.Location = new System.Drawing.Point(197, 476);
            this.RISbutton.Margin = new System.Windows.Forms.Padding(2);
            this.RISbutton.Name = "RISbutton";
            this.RISbutton.Size = new System.Drawing.Size(138, 26);
            this.RISbutton.TabIndex = 26;
            this.RISbutton.Text = "Läs RIS-filer";
            this.RISbutton.UseVisualStyleBackColor = true;
            this.RISbutton.Click += new System.EventHandler(this.RISbutton_Click);
            // 
            // CB_gephi
            // 
            this.CB_gephi.AutoSize = true;
            this.CB_gephi.Location = new System.Drawing.Point(211, 512);
            this.CB_gephi.Margin = new System.Windows.Forms.Padding(2);
            this.CB_gephi.Name = "CB_gephi";
            this.CB_gephi.Size = new System.Drawing.Size(107, 17);
            this.CB_gephi.TabIndex = 27;
            this.CB_gephi.Text = "Skapa Gephi-filer";
            this.CB_gephi.UseVisualStyleBackColor = true;
            // 
            // CB_csvR
            // 
            this.CB_csvR.AutoSize = true;
            this.CB_csvR.Location = new System.Drawing.Point(211, 536);
            this.CB_csvR.Margin = new System.Windows.Forms.Padding(2);
            this.CB_csvR.Name = "CB_csvR";
            this.CB_csvR.Size = new System.Drawing.Size(123, 17);
            this.CB_csvR.TabIndex = 28;
            this.CB_csvR.Text = "Skapa CSV-filer till R";
            this.CB_csvR.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 480);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmbHelg
            // 
            this.cmbHelg.FormattingEnabled = true;
            this.cmbHelg.Location = new System.Drawing.Point(57, 437);
            this.cmbHelg.Margin = new System.Windows.Forms.Padding(2);
            this.cmbHelg.Name = "cmbHelg";
            this.cmbHelg.Size = new System.Drawing.Size(82, 21);
            this.cmbHelg.TabIndex = 30;
            // 
            // Wordcloudbutton
            // 
            this.Wordcloudbutton.Location = new System.Drawing.Point(583, 214);
            this.Wordcloudbutton.Name = "Wordcloudbutton";
            this.Wordcloudbutton.Size = new System.Drawing.Size(194, 23);
            this.Wordcloudbutton.TabIndex = 31;
            this.Wordcloudbutton.Text = "Skapa ordmoln";
            this.Wordcloudbutton.UseVisualStyleBackColor = true;
            this.Wordcloudbutton.Click += new System.EventHandler(this.Wordcloudbutton_Click);
            // 
            // SearchStringbutton
            // 
            this.SearchStringbutton.Location = new System.Drawing.Point(340, 508);
            this.SearchStringbutton.Margin = new System.Windows.Forms.Padding(2);
            this.SearchStringbutton.Name = "SearchStringbutton";
            this.SearchStringbutton.Size = new System.Drawing.Size(128, 31);
            this.SearchStringbutton.TabIndex = 32;
            this.SearchStringbutton.Text = "Skapa söksträngar WoS/Scopus";
            this.SearchStringbutton.UseVisualStyleBackColor = true;
            this.SearchStringbutton.Click += new System.EventHandler(this.SearchStringbutton_Click);
            // 
            // CBannrep
            // 
            this.CBannrep.AutoSize = true;
            this.CBannrep.Checked = true;
            this.CBannrep.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBannrep.Location = new System.Drawing.Point(690, 304);
            this.CBannrep.Name = "CBannrep";
            this.CBannrep.Size = new System.Drawing.Size(73, 17);
            this.CBannrep.TabIndex = 33;
            this.CBannrep.Text = "ÅR-format";
            this.CBannrep.UseVisualStyleBackColor = true;
            // 
            // Authorlistbutton
            // 
            this.Authorlistbutton.Location = new System.Drawing.Point(473, 508);
            this.Authorlistbutton.Name = "Authorlistbutton";
            this.Authorlistbutton.Size = new System.Drawing.Size(122, 31);
            this.Authorlistbutton.TabIndex = 34;
            this.Authorlistbutton.Text = "Skapa författarlista";
            this.Authorlistbutton.UseVisualStyleBackColor = true;
            this.Authorlistbutton.Click += new System.EventHandler(this.Authorlistbutton_Click);
            // 
            // KTHbutton
            // 
            this.KTHbutton.Location = new System.Drawing.Point(474, 545);
            this.KTHbutton.Name = "KTHbutton";
            this.KTHbutton.Size = new System.Drawing.Size(121, 23);
            this.KTHbutton.TabIndex = 35;
            this.KTHbutton.Text = "Läs KTH-fil";
            this.KTHbutton.UseVisualStyleBackColor = true;
            this.KTHbutton.Click += new System.EventHandler(this.KTHbutton_Click);
            // 
            // Primulabutton
            // 
            this.Primulabutton.Location = new System.Drawing.Point(340, 544);
            this.Primulabutton.Name = "Primulabutton";
            this.Primulabutton.Size = new System.Drawing.Size(128, 24);
            this.Primulabutton.TabIndex = 36;
            this.Primulabutton.Text = "Läs Primula-fil";
            this.Primulabutton.UseVisualStyleBackColor = true;
            this.Primulabutton.Click += new System.EventHandler(this.Primulabutton_Click);
            // 
            // Topauthorbutton
            // 
            this.Topauthorbutton.Location = new System.Drawing.Point(474, 574);
            this.Topauthorbutton.Name = "Topauthorbutton";
            this.Topauthorbutton.Size = new System.Drawing.Size(121, 23);
            this.Topauthorbutton.TabIndex = 37;
            this.Topauthorbutton.Text = "Toppförfattare";
            this.Topauthorbutton.UseVisualStyleBackColor = true;
            this.Topauthorbutton.Click += new System.EventHandler(this.Topauthorbutton_Click);
            // 
            // Citebutton
            // 
            this.Citebutton.Location = new System.Drawing.Point(340, 574);
            this.Citebutton.Name = "Citebutton";
            this.Citebutton.Size = new System.Drawing.Size(129, 23);
            this.Citebutton.TabIndex = 38;
            this.Citebutton.Text = "Citeringsstatistik";
            this.Citebutton.UseVisualStyleBackColor = true;
            this.Citebutton.Click += new System.EventHandler(this.Citebutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 604);
            this.Controls.Add(this.Citebutton);
            this.Controls.Add(this.Topauthorbutton);
            this.Controls.Add(this.Primulabutton);
            this.Controls.Add(this.KTHbutton);
            this.Controls.Add(this.Authorlistbutton);
            this.Controls.Add(this.CBannrep);
            this.Controls.Add(this.SearchStringbutton);
            this.Controls.Add(this.Wordcloudbutton);
            this.Controls.Add(this.cmbHelg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CB_csvR);
            this.Controls.Add(this.CB_gephi);
            this.Controls.Add(this.RISbutton);
            this.Controls.Add(this.SubjectTabButton);
            this.Controls.Add(this.CBfraction);
            this.Controls.Add(this.CBanyauthorpos);
            this.Controls.Add(this.Journalbutton);
            this.Controls.Add(this.CB_perinst);
            this.Controls.Add(this.corpusbutton);
            this.Controls.Add(this.scopusbutton);
            this.Controls.Add(this.institutionbutton);
            this.Controls.Add(this.ProfileSubjectButton);
            this.Controls.Add(this.TBkontrollerad);
            this.Controls.Add(this.TBupprattad);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_worddir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_endyear);
            this.Controls.Add(this.TB_startyear);
            this.Controls.Add(this.CB_publists);
            this.Controls.Add(this.AnnrepTableButton);
            this.Controls.Add(this.whopublishedbutton);
            this.Controls.Add(this.PubStatsButton);
            this.Controls.Add(this.ReadDIVAbutton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Quitbutton);
            this.Name = "Form1";
            this.Text = " ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button ReadDIVAbutton;
        private System.Windows.Forms.Button PubStatsButton;
        private System.Windows.Forms.Button whopublishedbutton;
        private System.Windows.Forms.Button AnnrepTableButton;
        private System.Windows.Forms.CheckBox CB_publists;
        private System.Windows.Forms.TextBox TB_startyear;
        private System.Windows.Forms.TextBox TB_endyear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_worddir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBupprattad;
        private System.Windows.Forms.TextBox TBkontrollerad;
        private System.Windows.Forms.Button ProfileSubjectButton;
        private System.Windows.Forms.Button institutionbutton;
        private System.Windows.Forms.Button scopusbutton;
        private System.Windows.Forms.Button corpusbutton;
        private System.Windows.Forms.CheckBox CB_perinst;
        private System.Windows.Forms.Button Journalbutton;
        private System.Windows.Forms.CheckBox CBanyauthorpos;
        private System.Windows.Forms.CheckBox CBfraction;
        private System.Windows.Forms.Button SubjectTabButton;
        private System.Windows.Forms.Button RISbutton;
        private System.Windows.Forms.CheckBox CB_gephi;
        private System.Windows.Forms.CheckBox CB_csvR;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cmbHelg;
        private System.Windows.Forms.Button Wordcloudbutton;
        private System.Windows.Forms.Button SearchStringbutton;
        private System.Windows.Forms.CheckBox CBannrep;
        private System.Windows.Forms.Button Authorlistbutton;
        private System.Windows.Forms.Button KTHbutton;
        private System.Windows.Forms.Button Primulabutton;
        private System.Windows.Forms.Button Topauthorbutton;
        private System.Windows.Forms.Button Citebutton;
    }
}

