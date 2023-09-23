namespace PublicationsAnnualReport
{
    partial class FormProfileSubject
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.Quitbutton = new System.Windows.Forms.Button();
            this.LBprofsubj = new System.Windows.Forms.ListBox();
            this.Authorlistbutton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.TBdir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Clearauthorbutton = new System.Windows.Forms.Button();
            this.CBname = new System.Windows.Forms.CheckBox();
            this.CBcas = new System.Windows.Forms.CheckBox();
            this.CBprofile = new System.Windows.Forms.CheckBox();
            this.CBsubject = new System.Windows.Forms.CheckBox();
            this.CBpeer = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CB_profilecolor = new System.Windows.Forms.CheckBox();
            this.CB_inst_color = new System.Windows.Forms.CheckBox();
            this.CB_namelist_color = new System.Windows.Forms.CheckBox();
            this.clusterbutton = new System.Windows.Forms.Button();
            this.TBcluster = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RB_subject = new System.Windows.Forms.RadioButton();
            this.RB_individual = new System.Windows.Forms.RadioButton();
            this.CB_external = new System.Windows.Forms.CheckBox();
            this.CB_otherHDa = new System.Windows.Forms.CheckBox();
            this.CB_publess = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.TBcluster)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(465, 392);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // Quitbutton
            // 
            this.Quitbutton.Location = new System.Drawing.Point(599, 502);
            this.Quitbutton.Name = "Quitbutton";
            this.Quitbutton.Size = new System.Drawing.Size(122, 38);
            this.Quitbutton.TabIndex = 3;
            this.Quitbutton.Text = "Close";
            this.Quitbutton.UseVisualStyleBackColor = true;
            this.Quitbutton.Click += new System.EventHandler(this.Quitbutton_Click);
            // 
            // LBprofsubj
            // 
            this.LBprofsubj.FormattingEnabled = true;
            this.LBprofsubj.Location = new System.Drawing.Point(493, 2);
            this.LBprofsubj.Name = "LBprofsubj";
            this.LBprofsubj.Size = new System.Drawing.Size(238, 134);
            this.LBprofsubj.TabIndex = 4;
            this.LBprofsubj.SelectedIndexChanged += new System.EventHandler(this.LBprofsubj_SelectedIndexChanged);
            // 
            // Authorlistbutton
            // 
            this.Authorlistbutton.Location = new System.Drawing.Point(599, 142);
            this.Authorlistbutton.Name = "Authorlistbutton";
            this.Authorlistbutton.Size = new System.Drawing.Size(122, 34);
            this.Authorlistbutton.TabIndex = 5;
            this.Authorlistbutton.Text = "Read author list";
            this.Authorlistbutton.UseVisualStyleBackColor = true;
            this.Authorlistbutton.Click += new System.EventHandler(this.Authorlistbutton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(601, 211);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 36);
            this.button1.TabIndex = 6;
            this.button1.Text = "Make GEPHI code";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TBdir
            // 
            this.TBdir.Location = new System.Drawing.Point(134, 512);
            this.TBdir.Name = "TBdir";
            this.TBdir.Size = new System.Drawing.Size(284, 20);
            this.TBdir.TabIndex = 7;
            this.TBdir.Text = "C:\\Temp\\Gephi\\";
            this.TBdir.TextChanged += new System.EventHandler(this.TBdir_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 515);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Initial directory";
            // 
            // Clearauthorbutton
            // 
            this.Clearauthorbutton.Location = new System.Drawing.Point(599, 182);
            this.Clearauthorbutton.Name = "Clearauthorbutton";
            this.Clearauthorbutton.Size = new System.Drawing.Size(120, 23);
            this.Clearauthorbutton.TabIndex = 9;
            this.Clearauthorbutton.Text = "Clear author list";
            this.Clearauthorbutton.UseVisualStyleBackColor = true;
            this.Clearauthorbutton.Click += new System.EventHandler(this.Clearauthorbutton_Click);
            // 
            // CBname
            // 
            this.CBname.AutoSize = true;
            this.CBname.Location = new System.Drawing.Point(601, 330);
            this.CBname.Name = "CBname";
            this.CBname.Size = new System.Drawing.Size(54, 17);
            this.CBname.TabIndex = 10;
            this.CBname.Text = "Name";
            this.CBname.UseVisualStyleBackColor = true;
            // 
            // CBcas
            // 
            this.CBcas.AutoSize = true;
            this.CBcas.Location = new System.Drawing.Point(601, 353);
            this.CBcas.Name = "CBcas";
            this.CBcas.Size = new System.Drawing.Size(93, 17);
            this.CBcas.TabIndex = 11;
            this.CBcas.Text = "CAS signature";
            this.CBcas.UseVisualStyleBackColor = true;
            // 
            // CBprofile
            // 
            this.CBprofile.AutoSize = true;
            this.CBprofile.Location = new System.Drawing.Point(601, 376);
            this.CBprofile.Name = "CBprofile";
            this.CBprofile.Size = new System.Drawing.Size(55, 17);
            this.CBprofile.TabIndex = 12;
            this.CBprofile.Text = "Profile";
            this.CBprofile.UseVisualStyleBackColor = true;
            // 
            // CBsubject
            // 
            this.CBsubject.AutoSize = true;
            this.CBsubject.Location = new System.Drawing.Point(601, 399);
            this.CBsubject.Name = "CBsubject";
            this.CBsubject.Size = new System.Drawing.Size(62, 17);
            this.CBsubject.TabIndex = 13;
            this.CBsubject.Text = "Subject";
            this.CBsubject.UseVisualStyleBackColor = true;
            // 
            // CBpeer
            // 
            this.CBpeer.AutoSize = true;
            this.CBpeer.Checked = true;
            this.CBpeer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBpeer.Location = new System.Drawing.Point(125, 482);
            this.CBpeer.Name = "CBpeer";
            this.CBpeer.Size = new System.Drawing.Size(116, 17);
            this.CBpeer.TabIndex = 14;
            this.CBpeer.Text = "Peer reviewed only";
            this.CBpeer.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(598, 314);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "GEPHI label:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(601, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "GEPHI color code";
            // 
            // CB_profilecolor
            // 
            this.CB_profilecolor.AutoSize = true;
            this.CB_profilecolor.Location = new System.Drawing.Point(601, 435);
            this.CB_profilecolor.Name = "CB_profilecolor";
            this.CB_profilecolor.Size = new System.Drawing.Size(69, 17);
            this.CB_profilecolor.TabIndex = 17;
            this.CB_profilecolor.Text = "By profile";
            this.CB_profilecolor.UseVisualStyleBackColor = true;
            // 
            // CB_inst_color
            // 
            this.CB_inst_color.AutoSize = true;
            this.CB_inst_color.Location = new System.Drawing.Point(601, 458);
            this.CB_inst_color.Name = "CB_inst_color";
            this.CB_inst_color.Size = new System.Drawing.Size(85, 17);
            this.CB_inst_color.TabIndex = 18;
            this.CB_inst_color.Text = "By institution";
            this.CB_inst_color.UseVisualStyleBackColor = true;
            // 
            // CB_namelist_color
            // 
            this.CB_namelist_color.AutoSize = true;
            this.CB_namelist_color.Location = new System.Drawing.Point(601, 481);
            this.CB_namelist_color.Name = "CB_namelist_color";
            this.CB_namelist_color.Size = new System.Drawing.Size(82, 17);
            this.CB_namelist_color.TabIndex = 19;
            this.CB_namelist_color.Text = "By name list";
            this.CB_namelist_color.UseVisualStyleBackColor = true;
            // 
            // clusterbutton
            // 
            this.clusterbutton.Location = new System.Drawing.Point(479, 467);
            this.clusterbutton.Name = "clusterbutton";
            this.clusterbutton.Size = new System.Drawing.Size(114, 45);
            this.clusterbutton.TabIndex = 20;
            this.clusterbutton.Text = "Cluster from Gephi links";
            this.clusterbutton.UseVisualStyleBackColor = true;
            this.clusterbutton.Click += new System.EventHandler(this.clusterbutton_Click);
            // 
            // TBcluster
            // 
            this.TBcluster.Location = new System.Drawing.Point(313, 422);
            this.TBcluster.Maximum = 100;
            this.TBcluster.Name = "TBcluster";
            this.TBcluster.Size = new System.Drawing.Size(256, 45);
            this.TBcluster.TabIndex = 21;
            this.TBcluster.Value = 40;
            this.TBcluster.Scroll += new System.EventHandler(this.TBcluster_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(363, 411);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Cluster threshold";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RB_subject);
            this.groupBox1.Controls.Add(this.RB_individual);
            this.groupBox1.Location = new System.Drawing.Point(599, 252);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(122, 59);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GEPHI unit";
            // 
            // RB_subject
            // 
            this.RB_subject.AutoSize = true;
            this.RB_subject.Location = new System.Drawing.Point(9, 36);
            this.RB_subject.Name = "RB_subject";
            this.RB_subject.Size = new System.Drawing.Size(61, 17);
            this.RB_subject.TabIndex = 1;
            this.RB_subject.TabStop = true;
            this.RB_subject.Text = "Subject";
            this.RB_subject.UseVisualStyleBackColor = true;
            // 
            // RB_individual
            // 
            this.RB_individual.AutoSize = true;
            this.RB_individual.Location = new System.Drawing.Point(9, 18);
            this.RB_individual.Name = "RB_individual";
            this.RB_individual.Size = new System.Drawing.Size(70, 17);
            this.RB_individual.TabIndex = 0;
            this.RB_individual.TabStop = true;
            this.RB_individual.Text = "Individual";
            this.RB_individual.UseVisualStyleBackColor = true;
            // 
            // CB_external
            // 
            this.CB_external.AutoSize = true;
            this.CB_external.Enabled = false;
            this.CB_external.Location = new System.Drawing.Point(125, 459);
            this.CB_external.Name = "CB_external";
            this.CB_external.Size = new System.Drawing.Size(151, 17);
            this.CB_external.TabIndex = 24;
            this.CB_external.Text = "Include external coauthors";
            this.CB_external.UseVisualStyleBackColor = true;
            // 
            // CB_otherHDa
            // 
            this.CB_otherHDa.AutoSize = true;
            this.CB_otherHDa.Location = new System.Drawing.Point(125, 436);
            this.CB_otherHDa.Name = "CB_otherHDa";
            this.CB_otherHDa.Size = new System.Drawing.Size(151, 17);
            this.CB_otherHDa.TabIndex = 25;
            this.CB_otherHDa.Text = "Include other HDa authors";
            this.CB_otherHDa.UseVisualStyleBackColor = true;
            // 
            // CB_publess
            // 
            this.CB_publess.AutoSize = true;
            this.CB_publess.Location = new System.Drawing.Point(125, 411);
            this.CB_publess.Name = "CB_publess";
            this.CB_publess.Size = new System.Drawing.Size(195, 17);
            this.CB_publess.TabIndex = 26;
            this.CB_publess.Text = "Include authors with no publications";
            this.CB_publess.UseVisualStyleBackColor = true;
            // 
            // FormProfileSubject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 544);
            this.Controls.Add(this.CB_publess);
            this.Controls.Add(this.CB_otherHDa);
            this.Controls.Add(this.CB_external);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBcluster);
            this.Controls.Add(this.clusterbutton);
            this.Controls.Add(this.CB_namelist_color);
            this.Controls.Add(this.CB_inst_color);
            this.Controls.Add(this.CB_profilecolor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CBpeer);
            this.Controls.Add(this.CBsubject);
            this.Controls.Add(this.CBprofile);
            this.Controls.Add(this.CBcas);
            this.Controls.Add(this.CBname);
            this.Controls.Add(this.Clearauthorbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBdir);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Authorlistbutton);
            this.Controls.Add(this.LBprofsubj);
            this.Controls.Add(this.Quitbutton);
            this.Controls.Add(this.richTextBox1);
            this.Name = "FormProfileSubject";
            this.Text = "FormProfileSubject";
            ((System.ComponentModel.ISupportInitialize)(this.TBcluster)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button Quitbutton;
        private System.Windows.Forms.ListBox LBprofsubj;
        private System.Windows.Forms.Button Authorlistbutton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TBdir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Clearauthorbutton;
        private System.Windows.Forms.CheckBox CBname;
        private System.Windows.Forms.CheckBox CBcas;
        private System.Windows.Forms.CheckBox CBprofile;
        private System.Windows.Forms.CheckBox CBsubject;
        private System.Windows.Forms.CheckBox CBpeer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CB_profilecolor;
        private System.Windows.Forms.CheckBox CB_inst_color;
        private System.Windows.Forms.CheckBox CB_namelist_color;
        private System.Windows.Forms.Button clusterbutton;
        private System.Windows.Forms.TrackBar TBcluster;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RB_subject;
        private System.Windows.Forms.RadioButton RB_individual;
        private System.Windows.Forms.CheckBox CB_external;
        private System.Windows.Forms.CheckBox CB_otherHDa;
        private System.Windows.Forms.CheckBox CB_publess;
    }
}