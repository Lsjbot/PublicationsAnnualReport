using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using KnowledgePicker.WordCloud;
using KnowledgePicker.WordCloud.Collections;
using KnowledgePicker.WordCloud.Coloring;
using KnowledgePicker.WordCloud.Drawing;
using KnowledgePicker.WordCloud.Layouts;
using KnowledgePicker.WordCloud.Primitives;
using KnowledgePicker.WordCloud.Sizers;
using SkiaSharp;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using WordcloudLsjbot;


namespace PublicationsAnnualReport
{
    public partial class FormWordcloud : Form
    {
        List<pubclass> publist;
        string title = "";
        string folder = "";
        Color backgroundcolor = Color.FromArgb(240, 240, 200); //ljusgul //Color.FromArgb(254,243,237); //ljusrosa
        Color word1color = Color.FromArgb(27, 69, 145);
        Color word2color = Color.FromArgb(214, 33, 40);
        Color? wordcolor = null;// Color.FromArgb(225, 182, 191); //ljusrosa
        public FormWordcloud(List<pubclass> plpar, string titpar, string folderpar)
        {
            InitializeComponent();
            publist = plpar;
            title = titpar;
            folder = folderpar;
            
            
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private SKColor colorconvert(Color c)
        {
            return new SKColor(c.R, c.G, c.B);
        }

        private void MakeCloud(Dictionary<string, int> frequencies, string imagefile, string title)
        {
            int xsize = util.tryconvert(TBwidth.Text);
            int ysize = util.tryconvert(TBheight.Text);
            var wcc = new WordcloudLsjbot.WordCloudClass(xsize, ysize,CBbackpage.Checked,util.tryconvert(TBcontrast.Text),CBtitspecial.Checked);
            wcc.SetFontSize(util.tryconvert(TBminfont.Text), util.tryconvert(TBmaxfont.Text));
            wcc.SetBackgroundColor(colorconvert(backgroundcolor));
            wcc.SetSpecialColors(colorconvert(word1color),colorconvert(word2color));
            wcc.SetSpacing(util.tryconvert(TBxspace.Text), util.tryconvert(TByspace.Text));
            string shape = "rectangle";
            if (RBcircle.Checked)
                shape = "circle";
            else if (RBellipse.Checked)
                shape = "ellipse";
            else if (RBtriangle.Checked)
                shape = "triangle";
            wcc.SetShape(shape);
            if (wordcolor != null)
                wcc.SetSpecificWordColor(colorconvert((Color)wordcolor));
            else
            {
                wcc.SetColorRange(
                util.tryconvert(TBminh.Text), util.tryconvert(TBmaxh.Text),
                util.tryconvert(TBmins.Text), util.tryconvert(TBmaxs.Text),
                util.tryconvert(TBminv.Text), util.tryconvert(TBmaxv.Text));
            }
            int nw = 0;
            int skip = CBbackpage.Checked ? 2 : 0;
            foreach (string w in frequencies.Keys)
            {
                nw++;
                if (nw > skip)
                    wcc.Add(w, frequencies[w]);
            }
            int nfail = wcc.Arrange();
            memo("nfail = " + nfail);
            SKBitmap bitmap = wcc.Draw(title);
            wcc.DrawToFile(title, imagefile, bitmap);
            pictureBox1.Width = xsize;
            pictureBox1.Height = ysize;
            using (var stream = new MemoryStream())
            {
                bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
                stream.Seek(0, SeekOrigin.Begin);
                var image = Image.FromStream(stream);
                pictureBox1.Image = image;// new Bitmap(new System.IO.MemoryStream(data.ToArray()));
                //image.Save(imagefile);
            }
        }

        private void MakeCloudKnowledgePicker(Dictionary<string, int> frequencies, string imagefile, string title)
        {
            //Dictionary<string, int> freqorder = new Dictionary<string, int>();
            //foreach (KeyValuePair<string,int> kp in frequencies.OrderBy(key => key.Value))
            //    freqorder.Add(kp.Key,kp.Value);
            IEnumerable<WordCloudEntry> wordEntries = frequencies.Select(p => new WordCloudEntry(p.Key, p.Value));

            var wordCloud = new WordCloudInput(wordEntries)
            {
                Width = 1024,
                Height = 768,
                MinFontSize = 8,
                MaxFontSize = 48
            };
            var sizer = new LogSizer(wordCloud);
            using (var engine = new SkGraphicEngine(sizer, wordCloud))
            {
                var layout = new SpiralLayout(wordCloud);
                var colorizer = new RandomColorizer(); // optional
                var wcg = new WordCloudGenerator<SKBitmap>(wordCloud, engine, layout, colorizer);

                IEnumerable<(LayoutItem Item, double FontSize)> items = wcg.Arrange();

                using (var bitmap = new SKBitmap(wordCloud.Width, wordCloud.Height))
                using (var canvas = new SKCanvas(bitmap))
                {

                    // Draw on white background.
                    canvas.Clear(SKColors.White);
                    canvas.DrawBitmap(wcg.Draw(), 0, 0);

                    // Add title.
                    SKPaint paint = new SKPaint();
                    paint.TextSize = 12.0f;
                    paint.IsAntialias = true;
                    paint.Color = new SKColor(0x9C, 0xAF, 0xB7);
                    paint.IsStroke = false;
                    paint.StrokeWidth = 3;
                    paint.TextAlign = SKTextAlign.Left;
                    string label = title;
                    if (CBkeywords.Checked)
                        label += " keywords";
                    if (CBabstract.Checked)
                        label += " abstracts";
                    if (CBtitle.Checked)
                        label += " titles";

                    canvas.DrawText(label, 30, 30, paint);

                    

                    // Save to PNG.
                    using (var data = bitmap.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        pictureBox1.Image = new Bitmap(new System.IO.MemoryStream(data.ToArray()));
                        using (var writer = File.Create(imagefile))
                        {
                            data.SaveTo(writer);
                        }
                    }
                }
            }

        }

        char[] trimchars = " .-,;:'?!0123456789()".ToCharArray();
        private string cookkey(string rawkey)
        {
            return rawkey.Trim(trimchars).ToLower();//.Replace(" ","_");
        }



        private void GatherWords(List<string> divafields, out Dictionary<string,Dictionary<string,int>> fdict, out Dictionary<string,Dictionary<string,Dictionary<string,double>>> linkdict)
        {
            fdict = new Dictionary<string, Dictionary<string, int>>();
            linkdict = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            string allword = "all";
            fdict.Add(allword, new Dictionary<string, int>());
            linkdict.Add(allword, new Dictionary<string, Dictionary<string, double>>());

            string tagrex = @"\<[^<>]+\>";

            if (CBfile.Checked)
            {
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(of.FileName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] words = line.Split('\t');
                            if (!fdict[allword].ContainsKey(words[0]))
                                fdict[allword].Add(words[0], util.tryconvert(words[1]));
                        }
                    }
                }
            }
            else if (CBtext.Checked)
            {
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader sr = new StreamReader(of.FileName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            foreach (string rawkey in line.Split())
                            {
                                string key = cookkey(rawkey);
                                if (String.IsNullOrEmpty(key))
                                    continue;
                                if (blacklist.Contains(key))
                                    continue;
                                if (fdict[allword].ContainsKey(key))
                                {
                                    fdict[allword][key]++;
                                }
                                else
                                {
                                    fdict[allword].Add(key, 1);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (pubclass pc in publist)
                {
                    foreach (string divafield in divafields)
                    {
                        if (pc.dict.ContainsKey(divafield))
                        {
                            string[] keys;
                            if (divafield == pubclass.keywordstring)
                                keys = pc.dict[divafield].ToLower().Split(';');
                            else if (divafield == pubclass.titstring)
                                keys = pc.dict[divafield].ToLower().Split();
                            else if (divafield == pubclass.abstractstring)
                                keys = Regex.Replace(pc.dict[divafield], tagrex, "").ToLower().Split();
                            else
                                keys = new string[0];
                            foreach (string rawkey in keys)
                            {
                                string key = cookkey(rawkey);
                                if (blacklist.Contains(key))
                                    continue;
                                if (fdict[allword].ContainsKey(key))
                                {
                                    fdict[allword][key]++;
                                }
                                else
                                {
                                    fdict[allword].Add(key, 1);
                                }
                                if (!linkdict[allword].ContainsKey(key))
                                    linkdict[allword].Add(key, new Dictionary<string, double>());
                                foreach (string rawkey2 in keys)
                                {
                                    string key2 = cookkey(rawkey2);
                                    if (key2 == key)
                                        continue;
                                    if (blacklist.Contains(key2))
                                        continue;
                                    if (linkdict[allword][key].ContainsKey(key2))
                                        linkdict[allword][key][key2]++;
                                    else
                                        linkdict[allword][key].Add(key2, 1);
                                }
                            }
                            if (RB_inst.Checked)
                            {
                                foreach (string inst in pc.get_authorinstitution(false,true))
                                {
                                    if (!fdict.ContainsKey(inst))
                                        fdict.Add(inst, new Dictionary<string, int>());
                                    if (!linkdict.ContainsKey(inst))
                                        linkdict.Add(inst, new Dictionary<string, Dictionary<string, double>>());
                                    foreach (string rawkey in keys)
                                    {
                                        string key = cookkey(rawkey);
                                        if (blacklist.Contains(key))
                                            continue;
                                        if (fdict[inst].ContainsKey(key))
                                            fdict[inst][key]++;
                                        else
                                            fdict[inst].Add(key, 1);
                                        if (!linkdict[inst].ContainsKey(key))
                                            linkdict[inst].Add(key, new Dictionary<string, double>());
                                        foreach (string rawkey2 in keys)
                                        {
                                            string key2 = cookkey(rawkey2);
                                            if (key2 == key)
                                                continue;
                                            if (blacklist.Contains(key2))
                                                continue;
                                            if (linkdict[inst][key].ContainsKey(key2))
                                                linkdict[inst][key][key2]++;
                                            else
                                                linkdict[inst][key].Add(key2, 1);
                                        }
                                    }
                                }
                            }
                            if (RB_subject.Checked)
                            {
                                foreach (string inst in pc.get_authorsubject())
                                {
                                    if (!fdict.ContainsKey(inst))
                                        fdict.Add(inst, new Dictionary<string, int>());
                                    foreach (string key in keys)
                                    {
                                        if (fdict[inst].ContainsKey(key))
                                            fdict[inst][key]++;
                                        else
                                            fdict[inst].Add(key, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void Wordcloudbutton_Click(object sender, EventArgs e)
        {
            fill_blacklist();
            //var frequencies = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, int>> fdict;
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> linkdict;
            List<string> divastrings = new List<string>();

            string source = "";
            if (CBkeywords.Checked)
            {
                divastrings.Add(pubclass.keywordstring);
                source += " key";
            }
            if (CBabstract.Checked)
            {
                divastrings.Add(pubclass.abstractstring);
                source += " abs";
            }
            if (CBtitle.Checked)
            {
                divastrings.Add(pubclass.titstring);
                source += " tit";
            }

            GatherWords(divastrings, out fdict, out linkdict);

            //foreach (string s in fdict["all"].Keys)
            //    if (fdict["all"][s] > 50)
            //        memo(s + "\t" + fdict["all"][s]);
            //MakeCloud(fdict["all"],util.unusedfn(folder+"wordcloud_all.png"),title);
            foreach (string s in fdict.Keys)
            {
                string titstring = TBcloudtitle.Text;
                if (String.IsNullOrEmpty(titstring))
                    titstring = s + " " + source + " " + title;
                if (CBfile.Checked)
                    titstring = "";
                MakeCloud(fdict[s], util.unusedfn(folder + "cloud_" + s + source + "_.png"), titstring);
            }

        }

        public List<string> blacklist = new List<string>();

        public void fill_blacklist()
        {
            if (blacklist.Count > 0)
                return;

            blacklist.Add("the");
            blacklist.Add("and");
            blacklist.Add("of");
            blacklist.Add("in");
            blacklist.Add("to");
            blacklist.Add("a");
            blacklist.Add("for");
            blacklist.Add("with");
            blacklist.Add("is");
            blacklist.Add("as");
            blacklist.Add("that");
            blacklist.Add("on");
            blacklist.Add("this");
            blacklist.Add("were");
            blacklist.Add("was");
            blacklist.Add("are");
            blacklist.Add("by");
            blacklist.Add("from");
            blacklist.Add("study");
            blacklist.Add("an");
            blacklist.Add("be");
            blacklist.Add("i");
            blacklist.Add("at");
            blacklist.Add("their");
            blacklist.Add("we");
            blacklist.Add("between");
            blacklist.Add("data");

            blacklist.Add("care");
            blacklist.Add("or");
            blacklist.Add("it");
            blacklist.Add("how");
            blacklist.Add("have");
            blacklist.Add("can");
            blacklist.Add("used");

            blacklist.Add("which");
            blacklist.Add("using");
            blacklist.Add("results");
            blacklist.Add("research");

            blacklist.Add("has");
            blacklist.Add("more");
            blacklist.Add("also");
            blacklist.Add("not");
            blacklist.Add("these");

            blacklist.Add("use");
            blacklist.Add("analysis");



            blacklist.Add("during");

            blacklist.Add("=");

            blacklist.Add("all");
            blacklist.Add("but");
            blacklist.Add("they");

            blacklist.Add("both");
            blacklist.Add("two");

            blacklist.Add("been");
            blacklist.Add("one");
            blacklist.Add("such");


            blacklist.Add("when");

            blacklist.Add("about");





            blacklist.Add("there");
            blacklist.Add("time");

            blacklist.Add("well");
            blacklist.Add("three");
            blacklist.Add("than");

            blacklist.Add("among");
            blacklist.Add("other");

            blacklist.Add("into");

            blacklist.Add("what");

            blacklist.Add("aim");
            blacklist.Add("however");


            blacklist.Add("through");
            blacklist.Add("had");


            blacklist.Add("new");
            blacklist.Add("after");
            blacklist.Add("important");
            blacklist.Add("our");

            blacklist.Add("higher");

            blacklist.Add("findings");
            blacklist.Add("most");


            blacklist.Add("while");


            blacklist.Add("who");
            blacklist.Add("its");






            blacklist.Add("found");
            blacklist.Add("within");
            blacklist.Add("where");
            blacklist.Add("may");
            blacklist.Add("no");
            blacklist.Add("show");
            blacklist.Add("further");
            blacklist.Add("over");
            blacklist.Add("some");

            blacklist.Add("–");
            blacklist.Add("&amp");
            blacklist.Add("across");
            blacklist.Add("är");
            blacklist.Add("att");
            blacklist.Add("av");
            blacklist.Add("being");
            blacklist.Add("could");
            blacklist.Add("de");
            blacklist.Add("den");
            blacklist.Add("det");
            blacklist.Add("different");
            blacklist.Add("do");
            blacklist.Add("due");
            blacklist.Add("each");
            blacklist.Add("en");
            blacklist.Add("et");
            blacklist.Add("ett");
            blacklist.Add("first");
            blacklist.Add("för");
            blacklist.Add("four");
            blacklist.Add("från");
            blacklist.Add("har");
            blacklist.Add("high");
            blacklist.Add("hur");
            blacklist.Add("if");
            blacklist.Add("kan");
            blacklist.Add("key");
            blacklist.Add("la");
            blacklist.Add("large");
            blacklist.Add("less");
            blacklist.Add("made");
            blacklist.Add("main");
            blacklist.Add("many");
            blacklist.Add("med");
            blacklist.Add("men");
            blacklist.Add("needs");
            blacklist.Add("och");
            blacklist.Add("often");
            blacklist.Add("olika");
            blacklist.Add("om");
            blacklist.Add("only");
            blacklist.Add("out");
            blacklist.Add("own");
            blacklist.Add("på");
            blacklist.Add("per");
            blacklist.Add("several");
            blacklist.Add("should");
            blacklist.Add("showed");
            blacklist.Add("shows");
            blacklist.Add("som");
            blacklist.Add("them");
            blacklist.Add("those");
            blacklist.Add("thus");
            blacklist.Add("till");
            blacklist.Add("under");
            blacklist.Add("way");
            blacklist.Add("will");
            blacklist.Add("inte");
            blacklist.Add("också");
            blacklist.Add("även");
            blacklist.Add("dock");
            blacklist.Add("så");
            blacklist.Add("därför");
            blacklist.Add("endast");
            blacklist.Add("särskilt");
            blacklist.Add("t.ex.");
            blacklist.Add("bl.a.");
            blacklist.Add("mer");
            blacklist.Add("det");
            blacklist.Add("detta");
            blacklist.Add("ett");
            blacklist.Add("Ett");
            blacklist.Add("den");
            blacklist.Add("denna");
            blacklist.Add("Den");
            blacklist.Add("en");
            blacklist.Add("En");
            blacklist.Add("någon");
            blacklist.Add("de");
            blacklist.Add("dessa");
            blacklist.Add("De");
            blacklist.Add("alla");
            blacklist.Add("samma");
            blacklist.Add("när");
            blacklist.Add("där");
            blacklist.Add("hur");
            blacklist.Add("som");
            blacklist.Add("vad");
            blacklist.Add("vilket");
            blacklist.Add("att");
            blacklist.Add("annat");
            blacklist.Add("sådant");
            blacklist.Add("sådan");
            blacklist.Add("annan");
            blacklist.Add("sådana");
            blacklist.Add("andra");
            blacklist.Add("olika");
            blacklist.Add("vissa");
            blacklist.Add("nya");
            blacklist.Add("nya");
            blacklist.Add("och");
            blacklist.Add("eller");
            blacklist.Add("som");
            blacklist.Add("samt");
            blacklist.Add("än");
            blacklist.Add("men");
            blacklist.Add("fall");
            blacklist.Add("år");
            blacklist.Add("krav");
            blacklist.Add("barn");
            blacklist.Add("stycket");
            blacklist.Add("år");
            blacklist.Add("beslut");
            blacklist.Add("sätt");
            blacklist.Add("a");
            blacklist.Add("avsnitt");
            blacklist.Add("förslag");
            blacklist.Add("arbete");
            blacklist.Add("uppgifter");
            blacklist.Add("bestämmelser");
            blacklist.Add("åtgärder");
            blacklist.Add("personer");
            blacklist.Add("procent");
            blacklist.Add("regler");
            blacklist.Add("myndigheter");
            blacklist.Add("frågor");
            blacklist.Add("lagen");
            blacklist.Add("regeringen");
            blacklist.Add("artikel");
            blacklist.Add("lag");
            blacklist.Add("verksamhet");
            blacklist.Add("information");
            blacklist.Add("rätt");
            blacklist.Add("del");
            blacklist.Add("tid");
            blacklist.Add("if");
            blacklist.Add("myndighet");
            blacklist.Add("möjlighet");
            blacklist.Add("Ds");
            blacklist.Add("Sverige");
            blacklist.Add("the");
            blacklist.Add("det");
            blacklist.Add("Det");
            blacklist.Add("detta");
            blacklist.Add("Detta");
            blacklist.Add("den");
            blacklist.Add("man");
            blacklist.Add("de");
            blacklist.Add("sig");
            blacklist.Add("i");
            blacklist.Add("av");
            blacklist.Add("för");
            blacklist.Add("till");
            blacklist.Add("om");
            blacklist.Add("på");
            blacklist.Add("med");
            blacklist.Add("enligt");
            blacklist.Add("från");
            blacklist.Add("I");
            blacklist.Add("vid");
            blacklist.Add("inom");
            blacklist.Add("under");
            blacklist.Add("genom");
            blacklist.Add("mot");
            blacklist.Add("mellan");
            blacklist.Add("För");
            blacklist.Add("efter");
            blacklist.Add("över");
            blacklist.Add("Enligt");
            blacklist.Add("hos");
            blacklist.Add("utan");
            blacklist.Add("sin");
            blacklist.Add("sina");
            blacklist.Add("två");
            blacklist.Add("första");
            blacklist.Add("andra");
            blacklist.Add("att");
            blacklist.Add("om");
            blacklist.Add("Om");
            blacklist.Add("se");
            blacklist.Add("vara");
            blacklist.Add("kunna");
            blacklist.Add("ha");
            blacklist.Add("få");
            blacklist.Add("är");
            blacklist.Add("har");
            blacklist.Add("kan");
            blacklist.Add("ska");
            blacklist.Add("skall");
            blacklist.Add("får");
            blacklist.Add("bör");
            blacklist.Add("gäller");
            blacklist.Add("innebär");
            blacklist.Add("kommer");
            blacklist.Add("måste");
            blacklist.Add("avser");
            blacklist.Add("blir");
            blacklist.Add("finns");
            blacklist.Add("avses");
            blacklist.Add("anges");
            blacklist.Add("skulle");
            blacklist.Add("var");
            blacklist.Add("vi");
            blacklist.Add("fick");
            blacklist.Add("ganska");
            blacklist.Add("kanske");
            blacklist.Add("dem");
            blacklist.Add("många");
            blacklist.Add("bara");
            blacklist.Add("våra");
            blacklist.Add("tredje");
            blacklist.Add("blivit");
            blacklist.Add("ju");
            blacklist.Add("nog");
            blacklist.Add("något");
            blacklist.Add("mycket");
            blacklist.Add("gör");
            blacklist.Add("om");
            blacklist.Add("g");
            blacklist.Add("n");
            blacklist.Add("f");
            blacklist.Add("lite");
            blacklist.Add("bra");
            blacklist.Add("här");
            blacklist.Add("o");
            blacklist.Add("vårt");
            blacklist.Add("något");

        }

        private void Gephibutton_Click(object sender, EventArgs e)
        {
            //var frequencies = new Dictionary<string, int>();
            fill_blacklist();
            Dictionary<string, Dictionary<string, int>> fdict;
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> linkdict;
            List<string> divastrings = new List<string>();

            if (CBkeywords.Checked)
                divastrings.Add(pubclass.keywordstring);
            if (CBabstract.Checked)
                divastrings.Add(pubclass.abstractstring);
            if (CBtitle.Checked)
                divastrings.Add(pubclass.titstring);

            GatherWords(divastrings, out fdict, out linkdict);

            foreach (string s in fdict.Keys)
            {
                WriteGephi(fdict[s], linkdict[s], s);
            }

        }

        private void WriteGephi(Dictionary<string, int> nodedict, Dictionary<string, Dictionary<string, double>> linkdict, string institution)
        {
            Dictionary<string, int> nodeiddict = new Dictionary<string, int>();

            string selectstring = "Wordlink-" + institution;
            string folder = util.timestampfolder(@"C:\Temp\Gephi\", "Word-Gephi per institution");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            //string folder = openFileDialog1.InitialDirectory;
            string fn = util.unusedfn(folder + selectstring + @"nodes.csv");
            Dictionary<int, KnownColor> colordict = new Dictionary<int, KnownColor>();
            KnownColor[] knowncol = util.getallcolors();
            int nextcolor = 0;

            //Dictionary<string, bool> nodeused = new Dictionary<string, bool>();
            //if (String.IsNullOrEmpty(institution))
            //{
            //    foreach (string s in nodedict.Keys)
            //        nodeused[s] = true;
            //}
            //else
            //{
            //    foreach (string s in nodedict.Keys)
            //        nodeused[s] = false;
            //    nodeused[institution] = true;
            //    foreach (string s in linkdict.Keys)
            //        nodeused[s] = true;
            //}

            int minlink = 5;
            List<string> linkednodes = new List<string>();
            foreach (string s0 in linkdict.Keys)
            {
                double nlink = linkdict[s0].Values.Sum();
                if (nlink < minlink)
                    continue;
                if (!linkednodes.Contains(s0))
                    linkednodes.Add(s0);
                foreach (string s in linkdict[s0].Keys)
                {
                    if (!linkednodes.Contains(s))
                        linkednodes.Add(s);
                }
            }

            int id = 0;
            memo("Writing nodes to " + fn);
            using (StreamWriter sw = new StreamWriter(fn))
            {
                sw.WriteLine("Id;Label;Color");
                foreach (string nodename in nodedict.Keys)
                {
                    if (!linkednodes.Contains(nodename))
                        continue;
                    id++;
                    nodeiddict.Add(nodename, id);
                    if (!colordict.ContainsKey(nodedict[nodename]))
                    {
                        memo("Color " + knowncol[nextcolor].ToString());
                        colordict.Add(nodedict[nodename], knowncol[nextcolor]);
                        nextcolor++;
                    }
                    sw.WriteLine(nodeiddict[nodename] + ";" + nodename + ";" + String.Format("#{0:X6}", Color.FromKnownColor(colordict[nodedict[nodename]]).ToArgb() & 0x00ffffff));   //";#0000FF");
                }
            }

            string fn2 = fn.Replace("nodes", "edges");//unusedfn(folder + @"energiedges.csv");
            memo("Writing edges to " + fn2);
            using (StreamWriter sw = new StreamWriter(fn2))
            {
                sw.WriteLine("Source;Target;Weight");
                foreach (string s0 in linkdict.Keys)
                {
                    if (!nodeiddict.ContainsKey(s0))
                        continue;
                    foreach (string s in linkdict[s0].Keys)
                    {
                        if (!nodeiddict.ContainsKey(s))
                            continue;
                        sw.WriteLine(nodeiddict[s0] + ";" + nodeiddict[s] + ";" + linkdict[s0][s].ToString(CultureInfo.InvariantCulture));
                    }
                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((sender as Button).Text.Contains("background"))
                    backgroundcolor = colorDialog1.Color;
                else if ((sender as Button).Text.Contains("title"))
                    word1color = colorDialog1.Color;
                else if ((sender as Button).Text.Contains("author"))
                    word2color = colorDialog1.Color;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
