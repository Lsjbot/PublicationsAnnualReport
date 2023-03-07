
namespace PublicationsAnnualReport
{
    partial class FormWordcloud
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
            this.Wordcloudbutton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RB_subject = new System.Windows.Forms.RadioButton();
            this.RB_inst = new System.Windows.Forms.RadioButton();
            this.RB_all = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Gephibutton = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.CBkeywords = new System.Windows.Forms.CheckBox();
            this.CBabstract = new System.Windows.Forms.CheckBox();
            this.CBtitle = new System.Windows.Forms.CheckBox();
            this.CBfile = new System.Windows.Forms.CheckBox();
            this.CBbackpage = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.TBcontrast = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TBminh = new System.Windows.Forms.TextBox();
            this.TBmaxh = new System.Windows.Forms.TextBox();
            this.TBmaxs = new System.Windows.Forms.TextBox();
            this.TBmins = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBmaxv = new System.Windows.Forms.TextBox();
            this.TBminv = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Titlecolorbutton = new System.Windows.Forms.Button();
            this.Authorcolorbutton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.CBtitspecial = new System.Windows.Forms.CheckBox();
            this.TBwidth = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TBheight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TBminfont = new System.Windows.Forms.TextBox();
            this.TBmaxfont = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RBrectangle = new System.Windows.Forms.RadioButton();
            this.RBellipse = new System.Windows.Forms.RadioButton();
            this.RBcircle = new System.Windows.Forms.RadioButton();
            this.CBtext = new System.Windows.Forms.CheckBox();
            this.TBcloudtitle = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.RBtriangle = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.TBxspace = new System.Windows.Forms.TextBox();
            this.TByspace = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Wordcloudbutton
            // 
            this.Wordcloudbutton.Location = new System.Drawing.Point(1085, 64);
            this.Wordcloudbutton.Name = "Wordcloudbutton";
            this.Wordcloudbutton.Size = new System.Drawing.Size(93, 48);
            this.Wordcloudbutton.TabIndex = 0;
            this.Wordcloudbutton.Text = "Skapa ordmoln";
            this.Wordcloudbutton.UseVisualStyleBackColor = true;
            this.Wordcloudbutton.Click += new System.EventHandler(this.Wordcloudbutton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RB_subject);
            this.panel1.Controls.Add(this.RB_inst);
            this.panel1.Controls.Add(this.RB_all);
            this.panel1.Location = new System.Drawing.Point(1034, 182);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(111, 100);
            this.panel1.TabIndex = 1;
            // 
            // RB_subject
            // 
            this.RB_subject.AutoSize = true;
            this.RB_subject.Location = new System.Drawing.Point(9, 58);
            this.RB_subject.Name = "RB_subject";
            this.RB_subject.Size = new System.Drawing.Size(70, 17);
            this.RB_subject.TabIndex = 2;
            this.RB_subject.TabStop = true;
            this.RB_subject.Text = "Per ämne";
            this.RB_subject.UseVisualStyleBackColor = true;
            // 
            // RB_inst
            // 
            this.RB_inst.AutoSize = true;
            this.RB_inst.Location = new System.Drawing.Point(9, 35);
            this.RB_inst.Name = "RB_inst";
            this.RB_inst.Size = new System.Drawing.Size(88, 17);
            this.RB_inst.TabIndex = 1;
            this.RB_inst.TabStop = true;
            this.RB_inst.Text = "Per institution";
            this.RB_inst.UseVisualStyleBackColor = true;
            // 
            // RB_all
            // 
            this.RB_all.AutoSize = true;
            this.RB_all.Checked = true;
            this.RB_all.Location = new System.Drawing.Point(9, 12);
            this.RB_all.Name = "RB_all";
            this.RB_all.Size = new System.Drawing.Size(98, 17);
            this.RB_all.TabIndex = 0;
            this.RB_all.TabStop = true;
            this.RB_all.Text = "Alla tillsammans";
            this.RB_all.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(933, 958);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // Gephibutton
            // 
            this.Gephibutton.Location = new System.Drawing.Point(1085, 118);
            this.Gephibutton.Name = "Gephibutton";
            this.Gephibutton.Size = new System.Drawing.Size(93, 41);
            this.Gephibutton.TabIndex = 3;
            this.Gephibutton.Text = "Skapa Gephi";
            this.Gephibutton.UseVisualStyleBackColor = true;
            this.Gephibutton.Click += new System.EventHandler(this.Gephibutton_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1045, 766);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(189, 198);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // CBkeywords
            // 
            this.CBkeywords.AutoSize = true;
            this.CBkeywords.Checked = true;
            this.CBkeywords.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBkeywords.Location = new System.Drawing.Point(1067, 303);
            this.CBkeywords.Name = "CBkeywords";
            this.CBkeywords.Size = new System.Drawing.Size(72, 17);
            this.CBkeywords.TabIndex = 5;
            this.CBkeywords.Text = "Keywords";
            this.CBkeywords.UseVisualStyleBackColor = true;
            // 
            // CBabstract
            // 
            this.CBabstract.AutoSize = true;
            this.CBabstract.Location = new System.Drawing.Point(1067, 329);
            this.CBabstract.Name = "CBabstract";
            this.CBabstract.Size = new System.Drawing.Size(65, 17);
            this.CBabstract.TabIndex = 6;
            this.CBabstract.Text = "Abstract";
            this.CBabstract.UseVisualStyleBackColor = true;
            // 
            // CBtitle
            // 
            this.CBtitle.AutoSize = true;
            this.CBtitle.Location = new System.Drawing.Point(1067, 352);
            this.CBtitle.Name = "CBtitle";
            this.CBtitle.Size = new System.Drawing.Size(46, 17);
            this.CBtitle.TabIndex = 7;
            this.CBtitle.Text = "Title";
            this.CBtitle.UseVisualStyleBackColor = true;
            // 
            // CBfile
            // 
            this.CBfile.AutoSize = true;
            this.CBfile.Location = new System.Drawing.Point(1034, 375);
            this.CBfile.Name = "CBfile";
            this.CBfile.Size = new System.Drawing.Size(104, 17);
            this.CBfile.TabIndex = 8;
            this.CBfile.Text = "Extern fil viktlista";
            this.CBfile.UseVisualStyleBackColor = true;
            // 
            // CBbackpage
            // 
            this.CBbackpage.AutoSize = true;
            this.CBbackpage.Location = new System.Drawing.Point(1054, 423);
            this.CBbackpage.Name = "CBbackpage";
            this.CBbackpage.Size = new System.Drawing.Size(106, 17);
            this.CBbackpage.TabIndex = 9;
            this.CBbackpage.Text = "Backpage layout";
            this.CBbackpage.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1146, 288);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 29);
            this.button1.TabIndex = 10;
            this.button1.Text = "Set background color";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TBcontrast
            // 
            this.TBcontrast.Location = new System.Drawing.Point(1173, 397);
            this.TBcontrast.Name = "TBcontrast";
            this.TBcontrast.Size = new System.Drawing.Size(100, 20);
            this.TBcontrast.TabIndex = 11;
            this.TBcontrast.Text = "100000";
            this.TBcontrast.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1142, 380);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Color contrast (0-150000)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1042, 453);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Hue range";
            // 
            // TBminh
            // 
            this.TBminh.Location = new System.Drawing.Point(1120, 450);
            this.TBminh.Name = "TBminh";
            this.TBminh.Size = new System.Drawing.Size(44, 20);
            this.TBminh.TabIndex = 14;
            this.TBminh.Text = "0";
            // 
            // TBmaxh
            // 
            this.TBmaxh.Location = new System.Drawing.Point(1189, 450);
            this.TBmaxh.Name = "TBmaxh";
            this.TBmaxh.Size = new System.Drawing.Size(44, 20);
            this.TBmaxh.TabIndex = 15;
            this.TBmaxh.Text = "359";
            // 
            // TBmaxs
            // 
            this.TBmaxs.Location = new System.Drawing.Point(1189, 475);
            this.TBmaxs.Name = "TBmaxs";
            this.TBmaxs.Size = new System.Drawing.Size(44, 20);
            this.TBmaxs.TabIndex = 18;
            this.TBmaxs.Text = "99";
            // 
            // TBmins
            // 
            this.TBmins.Location = new System.Drawing.Point(1120, 476);
            this.TBmins.Name = "TBmins";
            this.TBmins.Size = new System.Drawing.Size(44, 20);
            this.TBmins.TabIndex = 17;
            this.TBmins.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1021, 478);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Saturation range";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // TBmaxv
            // 
            this.TBmaxv.Location = new System.Drawing.Point(1189, 500);
            this.TBmaxv.Name = "TBmaxv";
            this.TBmaxv.Size = new System.Drawing.Size(44, 20);
            this.TBmaxv.TabIndex = 21;
            this.TBmaxv.Text = "99";
            // 
            // TBminv
            // 
            this.TBminv.Location = new System.Drawing.Point(1120, 500);
            this.TBminv.Name = "TBminv";
            this.TBminv.Size = new System.Drawing.Size(44, 20);
            this.TBminv.TabIndex = 20;
            this.TBminv.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1042, 503);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Brightness range";
            // 
            // Titlecolorbutton
            // 
            this.Titlecolorbutton.Location = new System.Drawing.Point(1145, 323);
            this.Titlecolorbutton.Name = "Titlecolorbutton";
            this.Titlecolorbutton.Size = new System.Drawing.Size(122, 23);
            this.Titlecolorbutton.TabIndex = 22;
            this.Titlecolorbutton.Text = "Set title color";
            this.Titlecolorbutton.UseVisualStyleBackColor = true;
            this.Titlecolorbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Authorcolorbutton
            // 
            this.Authorcolorbutton.Location = new System.Drawing.Point(1145, 346);
            this.Authorcolorbutton.Name = "Authorcolorbutton";
            this.Authorcolorbutton.Size = new System.Drawing.Size(122, 23);
            this.Authorcolorbutton.TabIndex = 23;
            this.Authorcolorbutton.Text = "Set author color";
            this.Authorcolorbutton.UseVisualStyleBackColor = true;
            this.Authorcolorbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1010, 1000);
            this.panel2.TabIndex = 24;
            // 
            // CBtitspecial
            // 
            this.CBtitspecial.AutoSize = true;
            this.CBtitspecial.Location = new System.Drawing.Point(1166, 423);
            this.CBtitspecial.Name = "CBtitspecial";
            this.CBtitspecial.Size = new System.Drawing.Size(82, 17);
            this.CBtitspecial.TabIndex = 25;
            this.CBtitspecial.Text = "Title special";
            this.CBtitspecial.UseVisualStyleBackColor = true;
            // 
            // TBwidth
            // 
            this.TBwidth.Location = new System.Drawing.Point(1076, 531);
            this.TBwidth.Name = "TBwidth";
            this.TBwidth.Size = new System.Drawing.Size(45, 20);
            this.TBwidth.TabIndex = 26;
            this.TBwidth.TabStop = false;
            this.TBwidth.Text = "1400";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1031, 538);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1031, 557);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Height";
            // 
            // TBheight
            // 
            this.TBheight.Location = new System.Drawing.Point(1076, 557);
            this.TBheight.Name = "TBheight";
            this.TBheight.Size = new System.Drawing.Size(45, 20);
            this.TBheight.TabIndex = 29;
            this.TBheight.Text = "1000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1143, 538);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Minfont";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1143, 557);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Maxfont";
            // 
            // TBminfont
            // 
            this.TBminfont.Location = new System.Drawing.Point(1191, 531);
            this.TBminfont.Name = "TBminfont";
            this.TBminfont.Size = new System.Drawing.Size(42, 20);
            this.TBminfont.TabIndex = 32;
            this.TBminfont.Text = "12";
            // 
            // TBmaxfont
            // 
            this.TBmaxfont.Location = new System.Drawing.Point(1189, 557);
            this.TBmaxfont.Name = "TBmaxfont";
            this.TBmaxfont.Size = new System.Drawing.Size(44, 20);
            this.TBmaxfont.TabIndex = 33;
            this.TBmaxfont.Text = "64";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RBtriangle);
            this.groupBox1.Controls.Add(this.RBrectangle);
            this.groupBox1.Controls.Add(this.RBellipse);
            this.groupBox1.Controls.Add(this.RBcircle);
            this.groupBox1.Location = new System.Drawing.Point(1166, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(107, 117);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Molnets form";
            // 
            // RBrectangle
            // 
            this.RBrectangle.AutoSize = true;
            this.RBrectangle.Checked = true;
            this.RBrectangle.Location = new System.Drawing.Point(9, 67);
            this.RBrectangle.Name = "RBrectangle";
            this.RBrectangle.Size = new System.Drawing.Size(74, 17);
            this.RBrectangle.TabIndex = 2;
            this.RBrectangle.TabStop = true;
            this.RBrectangle.Text = "Rektangel";
            this.RBrectangle.UseVisualStyleBackColor = true;
            // 
            // RBellipse
            // 
            this.RBellipse.AutoSize = true;
            this.RBellipse.Location = new System.Drawing.Point(9, 44);
            this.RBellipse.Name = "RBellipse";
            this.RBellipse.Size = new System.Drawing.Size(49, 17);
            this.RBellipse.TabIndex = 1;
            this.RBellipse.Text = "Ellips";
            this.RBellipse.UseVisualStyleBackColor = true;
            // 
            // RBcircle
            // 
            this.RBcircle.AutoSize = true;
            this.RBcircle.Location = new System.Drawing.Point(9, 21);
            this.RBcircle.Name = "RBcircle";
            this.RBcircle.Size = new System.Drawing.Size(51, 17);
            this.RBcircle.TabIndex = 0;
            this.RBcircle.Text = "Cirkel";
            this.RBcircle.UseVisualStyleBackColor = true;
            // 
            // CBtext
            // 
            this.CBtext.AutoSize = true;
            this.CBtext.Location = new System.Drawing.Point(1034, 399);
            this.CBtext.Name = "CBtext";
            this.CBtext.Size = new System.Drawing.Size(86, 17);
            this.CBtext.TabIndex = 35;
            this.CBtext.Text = "Extern fil text";
            this.CBtext.UseVisualStyleBackColor = true;
            // 
            // TBcloudtitle
            // 
            this.TBcloudtitle.Location = new System.Drawing.Point(1099, 19);
            this.TBcloudtitle.Name = "TBcloudtitle";
            this.TBcloudtitle.Size = new System.Drawing.Size(168, 20);
            this.TBcloudtitle.TabIndex = 36;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1037, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Cloud title:";
            // 
            // RBtriangle
            // 
            this.RBtriangle.AutoSize = true;
            this.RBtriangle.Location = new System.Drawing.Point(7, 91);
            this.RBtriangle.Name = "RBtriangle";
            this.RBtriangle.Size = new System.Drawing.Size(63, 17);
            this.RBtriangle.TabIndex = 3;
            this.RBtriangle.TabStop = true;
            this.RBtriangle.Text = "Triangel";
            this.RBtriangle.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1040, 602);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Horizontal spacing";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1040, 623);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "Vertical spacing";
            // 
            // TBxspace
            // 
            this.TBxspace.Location = new System.Drawing.Point(1140, 599);
            this.TBxspace.Name = "TBxspace";
            this.TBxspace.Size = new System.Drawing.Size(48, 20);
            this.TBxspace.TabIndex = 40;
            this.TBxspace.Text = "2";
            // 
            // TByspace
            // 
            this.TByspace.Location = new System.Drawing.Point(1140, 623);
            this.TByspace.Name = "TByspace";
            this.TByspace.Size = new System.Drawing.Size(48, 20);
            this.TByspace.TabIndex = 41;
            this.TByspace.Text = "3";
            // 
            // FormWordcloud
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 1023);
            this.Controls.Add(this.TByspace);
            this.Controls.Add(this.TBxspace);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.TBcloudtitle);
            this.Controls.Add(this.CBtext);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.TBmaxfont);
            this.Controls.Add(this.TBminfont);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TBheight);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TBwidth);
            this.Controls.Add(this.CBtitspecial);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Authorcolorbutton);
            this.Controls.Add(this.Titlecolorbutton);
            this.Controls.Add(this.TBmaxv);
            this.Controls.Add(this.TBminv);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBmaxs);
            this.Controls.Add(this.TBmins);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBmaxh);
            this.Controls.Add(this.TBminh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TBcontrast);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CBbackpage);
            this.Controls.Add(this.CBfile);
            this.Controls.Add(this.CBtitle);
            this.Controls.Add(this.CBabstract);
            this.Controls.Add(this.CBkeywords);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.Gephibutton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Wordcloudbutton);
            this.Name = "FormWordcloud";
            this.Text = "FormWordcloud";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Wordcloudbutton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton RB_subject;
        private System.Windows.Forms.RadioButton RB_inst;
        private System.Windows.Forms.RadioButton RB_all;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Gephibutton;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox CBkeywords;
        private System.Windows.Forms.CheckBox CBabstract;
        private System.Windows.Forms.CheckBox CBtitle;
        private System.Windows.Forms.CheckBox CBfile;
        private System.Windows.Forms.CheckBox CBbackpage;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TBcontrast;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBminh;
        private System.Windows.Forms.TextBox TBmaxh;
        private System.Windows.Forms.TextBox TBmaxs;
        private System.Windows.Forms.TextBox TBmins;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBmaxv;
        private System.Windows.Forms.TextBox TBminv;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Titlecolorbutton;
        private System.Windows.Forms.Button Authorcolorbutton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox CBtitspecial;
        private System.Windows.Forms.TextBox TBwidth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TBheight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TBminfont;
        private System.Windows.Forms.TextBox TBmaxfont;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RBrectangle;
        private System.Windows.Forms.RadioButton RBellipse;
        private System.Windows.Forms.RadioButton RBcircle;
        private System.Windows.Forms.CheckBox CBtext;
        private System.Windows.Forms.TextBox TBcloudtitle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton RBtriangle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TBxspace;
        private System.Windows.Forms.TextBox TByspace;
    }
}