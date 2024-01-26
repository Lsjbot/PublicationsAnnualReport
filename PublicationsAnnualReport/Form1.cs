using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Xceed.Words.NET;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PublicationsAnnualReport
{
    public partial class Form1 : Form
    {

        public static List<authorclass> caslist = new List<authorclass>();

        public static List<pubclass> publist = new List<pubclass>();

        public static List<journalclass> journallist = new List<journalclass>();

        public static List<publisherclass> publisherlist = new List<publisherclass>();

        public Dictionary<char, int> genderlist = new Dictionary<char, int>()
        { { 'M', 0 }, {'F',0 }, {'X',0 } };

        public Dictionary<string, int> pubtypelist = new Dictionary<string, int>();
        public Dictionary<string, int> contenttypelist = new Dictionary<string, int>();
        public Dictionary<string, int> statuslist = new Dictionary<string, int>();
        public List<string> apalist = new List<string>();

        Dictionary<string, int> hd = new Dictionary<string, int>(); //header of Diva file


        public DateTime divadate = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void Quitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public char getgender(string cas)
        {
            var q = from c in caslist where c.CAS == cas select c.gender;
            if (q.Count() == 1)
                return q.First();
            else if (q.Count() > 1)
            {
                memo("Duplicate CAS " + cas);
                return 'X';
            }
            else
                return 'X';
        }

        private void ReadAPAlist()
        {
            openFileDialog1.Title = "APA-fil:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fnapa = openFileDialog1.FileName;
            memo("Reading " + fnapa);

            using (StreamReader sr = new StreamReader(fnapa))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                        apalist.Add(line);
                }
            }

            memo("APAlist entries: " + apalist.Count);

        }

        private void ReadCAS()
        {
            openFileDialog1.Title = "CAS-fil:";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fncas = openFileDialog1.FileName;
            memo("Reading " + fncas);

            using (StreamReader sr = new StreamReader(fncas))
            {
                string header = sr.ReadLine();
                string[] headsplit = header.Split('\t');

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 3)
                        continue;
                    authorclass ac = new authorclass();
                    ac.name = words[0];
                    ac.CAS = words[1];
                    if (words[2] == "1")
                        ac.gender = 'F';
                    else
                        ac.gender = 'M';
                    if (!String.IsNullOrEmpty(words[3]))
                    {
                        string prof = words[3].Trim(new char[] { ' ', '"' });
                        ac.affiliation.Add(prof);
                        ac.profile = prof;
                        if (!authorclass.profiles.Contains(prof))
                            authorclass.profiles.Add(prof);
                    }
                    if (!String.IsNullOrEmpty(words[4]))
                    {
                        //if (words[4].Contains("2020"))
                        //    memo(words[4]);
                        string sub = words[4].Split('|')[0].Trim(new char[] { ' ', '"' });
                        if (String.IsNullOrEmpty(sub))
                            sub = authorclass.nosubject;
                        //if (ac.name.Contains("Aho"))
                        //{
                        //    int dummy = 0;
                        //}
                        ac.affiliation.Add(sub);
                        ac.subject = sub;
                        if (pubclass.ausubjalias.ContainsKey(ac.subject))
                            ac.subjectalias = pubclass.ausubjalias[ac.subject];
                        else
                            ac.subjectalias = ac.subject;
                        if (!authorclass.subjects.Contains(sub))
                            authorclass.subjects.Add(sub);
                    }
                    string inst = authorclass.noinstitution;
                    if (!String.IsNullOrEmpty(words[5]))
                    {
                        inst = words[5].ToLower().Trim(new char[] { ' ', '"' });
                    }
                    ac.affiliation.Add(inst);
                    ac.institution = inst;
                    if (!authorclass.institutions.Contains(inst))
                        authorclass.institutions.Add(inst);
                    
                    ac.HDa_in_publication = true;

                    if (words.Length >= 7)
                    {
                        ac.notes = words[6];
                        //if (!String.IsNullOrEmpty(words[7]))
                        //{
                        //    ac.affiliation.Add(words[7].Trim(new char[] { ' ', '"' }));
                        //    if (!authorclass.departments.Contains(words[7].Trim(new char[] { ' ', '"' })))
                        //        authorclass.departments.Add(words[7].Trim(new char[] { ' ', '"' }));
                        //}
                    }
                    //if (ac.subjectalias == "kultur och samhälle")
                    //{
                    //    int dummy = 0;
                    //}
                    caslist.Add(ac);
                }
            }

            memo("caslist entries: " + caslist.Count);
            int nm = (from c in caslist where c.gender == 'M' select c).Count();
            int nf = (from c in caslist where c.gender == 'F' select c).Count();
            memo("Men:   " + nm);
            memo("Women: " + nf);

            List<string> changedauthors = authorclass.fill_instsubj(caslist);
            memo("Authors changed in fill_instsubj:");
            foreach (string s in changedauthors)
                memo(s);
        }

        private void ReadDIVAbutton_Click(object sender, EventArgs e)
        {
            ReadCAS();

            openFileDialog1.Title = "DIVA-fil:";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            divadate = File.GetCreationTime(fn);

            string[] separator = { "\",\"" };
            using (StreamReader sr = new StreamReader(fn))
            {
                string header = sr.ReadLine();
                string[] headsplit;
                if (header.Contains('\t'))
                {
                    headsplit = header.Split('\t');
                    separator[0] = "\t";
                }
                else if (header.Contains(";"))
                {
                    headsplit = header.Split(';');
                    separator[0] =  "\";\"" ;
                }
                else
                    headsplit = header.Split(',');
                for (int i = 0; i < headsplit.Length; i++)
                {
                    hd.Add(headsplit[i], i);
                }

                //kluge för att få in dem i output-filer senare:
                hd.Add(pubclass.impactstring, headsplit.Length);
                hd.Add(pubclass.norskastring, headsplit.Length+1);
                hd.Add(pubclass.scopuscitationstring, headsplit.Length + 2);
                hd.Add(pubclass.kthcitationstring, headsplit.Length + 3);
                hd.Add(pubclass.kthfieldnormstring, headsplit.Length + 4);
                hd.Add(pubclass.kthtopxstring, headsplit.Length + 5);
                hd.Add(pubclass.kthjcf2string, headsplit.Length + 6);
                hd.Add(pubclass.firsthdagenderstring, headsplit.Length + 7);
                hd.Add(pubclass.pubgroupstring, headsplit.Length + 8);

                int npub = 0;

                while (!sr.EndOfStream)
                {
                    npub++;

                    //if (npub > 10)
                    //    break;

                    string line = sr.ReadLine();
                    if (String.IsNullOrEmpty(line))
                        continue;
                    string[] words = line.Split(separator, StringSplitOptions.None);
                    pubclass pc = new pubclass();
                    for (int i = 0; i < Math.Min(words.Length, headsplit.Length); i++)
                    {
                        string w = words[i].Trim(new char[] { ' ', '"' });
                        if (!String.IsNullOrEmpty(w))
                            pc.dict.Add(headsplit[i], w);
                    }
                    if (!pc.dict.ContainsKey("Name"))
                        continue;
                    pc.authors = authorclass.parseauthors(pc.dict["Name"]);
                    if (pc.has(pubclass.contributorstring))
                        pc.contributors = authorclass.parseauthors(pc.dict[pubclass.contributorstring]);
                    List<string> pcinstlist = new List<string>();
                    List<string> pcsubjlist = new List<string>();
                    List<string> CASwithoutHDa = new List<string>();
                    List<string> CASwithHDa = new List<string>();
                    foreach (authorclass ac in pc.authors)
                    {
                        ac.HDa_in_publication = ac.isHDa();
                        if (ac.isHDa())
                        {
                            CASwithHDa.Add(ac.CAS);
                        }
                        else if (ac.CASbutnotHDa())
                            CASwithoutHDa.Add(ac.CAS);
                    }

                    pc.fill_casauthors(caslist);
                    
                    foreach (authorclass ac in pc.authors)
                    {
                        if (CASwithHDa.Contains(ac.CAS))
                        {
                            if (!pcinstlist.Contains(ac.getinstitution(false)))
                                pcinstlist.Add(ac.getinstitution(false));
                            if (!pcsubjlist.Contains(ac.getsubjectalias()))
                                pcsubjlist.Add(ac.getsubjectalias());
                        }
                    }
                    foreach (authorclass ac in pc.authors)
                    {

                        if (CASwithoutHDa.Contains(ac.CAS))
                        {
                            if (!pcinstlist.Contains(ac.getinstitution(false)))
                                memo(pc.dict[pubclass.pidstring]+" Institution level missing for " + ac.name);
                            else if (!pcsubjlist.Contains(ac.getsubjectalias()))
                                memo(pc.dict[pubclass.pidstring]+ " Subject level missing for " + ac.name);
                        }
                    }
                    //memo(pc.authors.Count + "\t" + pc.dict["Name"]);
                    //foreach (authorclass ac in pc.authors)
                    //    memo(ac.name + "\t" + ac.CAS + "\t" + ac.affiliation);
                    pc.firstHDa = authorclass.getfirstHDa(pc.authors);
                    if (pc.firstHDa != null)
                    {
                        pc.firstHDa.gender = getgender(pc.firstHDa.CAS);
                        if (pc.firstHDa.gender == 'X')
                            memo(pc.dict[pubclass.pidstring] + "\t" + pc.firstHDa.name + " has no gender");
                    }
                    else
                    {
                        memo(pc.dict[pubclass.pidstring] + "\tNo HDa author!" + "\t" + pc.dict[pubclass.authorstring]);
                        //foreach (authorclass ac in pc.authors)
                        //    memo(ac.name + "\t" + ac.CAS + "\t" + ac.affiliation);
                        continue;
                    }
                    pc.cleantitle();
                    pc.setyear();

                    if (!pubtypelist.ContainsKey(pc.dict[pubclass.pubtypestring]))
                        pubtypelist.Add(pc.dict[pubclass.pubtypestring], 0);
                    if (!contenttypelist.ContainsKey(pc.dict[pubclass.contenttypestring]))
                        contenttypelist.Add(pc.dict[pubclass.contenttypestring], 0);
                    if (pc.dict.ContainsKey(pubclass.statusstring) && !statuslist.ContainsKey(pc.dict[pubclass.statusstring]))
                        statuslist.Add(pc.dict[pubclass.statusstring], 0);

                    pubtypelist[pc.dict[pubclass.pubtypestring]]++;
                    contenttypelist[pc.dict[pubclass.contenttypestring]]++;
                    if (pc.dict.ContainsKey(pubclass.statusstring))
                        statuslist[pc.dict[pubclass.statusstring]]++;
                    genderlist[pc.firstHDa.gender]++;


                    publist.Add(pc);


                }
            }

            memo("Publications: " + publist.Count);

            PubStatsButton.Enabled = true;
            whopublishedbutton.Enabled = true;
            AnnrepTableButton.Enabled = true;
            ProfileSubjectButton.Enabled = true;
            SubjectTabButton.Enabled = true;
        }



        //private int tryconvert(string s)
        //{
        //    int i = -1;
        //    try
        //    {
        //        i = Convert.ToInt32(s.Replace(" ", ""));
        //    }
        //    catch (Exception e)
        //    {
        //        memo("tryconvert(" + s + ") " + e.Message);
        //    }
        //    return i;
        //}


        private void PubStatsButton_Click(object sender, EventArgs e)
        {
            memo("Publikationstyp:");

            int startyear = util.tryconvert(TB_startyear.Text);
            if (startyear < 0)
            {
                memo("Invalid startyear " + TB_startyear.Text);
                return;
            }

            int endyear = util.tryconvert(TB_endyear.Text);
            if (endyear < 0)
            {
                memo("Invalid endyear " + TB_endyear.Text);
                return;
            }

            for (int year = startyear; year <= endyear; year++)
            {
                int yearsum = 0;
                memo(year.ToString());
                foreach (string pubtype in pubtypelist.Keys)
                {
                    int pubtypesum = 0;
                    memo(pubtype);
                    foreach (string contenttype in contenttypelist.Keys)
                    {
                        int contenttypesum = 0;
                        memo(contenttype);
                        foreach (string status in statuslist.Keys)
                        {
                            int statussum = 0;
                            memo(status);
                            foreach (char gender in genderlist.Keys)
                            {
                                var q = from c in publist
                                        where c.dict[pubclass.yearstring] == year.ToString()
                                        where c.dict[pubclass.pubtypestring] == pubtype
                                        where c.dict[pubclass.contenttypestring] == contenttype
                                        where (c.dict.ContainsKey(pubclass.statusstring) && c.dict[pubclass.statusstring] == status)
                                        where c.firstHDa.gender == gender
                                        select c;
                                memo(gender + "\t" + q.Count());
                                statussum += q.Count();
                            }
                            memo(status + "\t" + statussum);
                            contenttypesum += statussum;
                        }
                        memo(contenttype + "\t" + contenttypesum);
                        pubtypesum += contenttypesum;
                    }
                    memo(pubtype + "\t" + pubtypesum);
                    yearsum += pubtypesum;

                }
                memo(year + "\t" + yearsum);
            }

            foreach (string pubtype in pubtypelist.Keys)
                memo(pubtype + "\t" + pubtypelist[pubtype]);
            foreach (string contenttype in contenttypelist.Keys)
                memo(contenttype + "\t" + contenttypelist[contenttype]);
            foreach (string status in statuslist.Keys)
                memo(status + "\t" + statuslist[status]);
            foreach (char gender in genderlist.Keys)
                memo(gender + "\t" + genderlist[gender]);

        }

        private void whopublishedbutton_Click(object sender, EventArgs e)
        {
            foreach (pubclass pub in publist)
            {
                foreach (authorclass aut in pub.authors)
                {
                    if (!String.IsNullOrEmpty(aut.CAS))
                    {
                        authorclass casaut = (from c in caslist where c.CAS == aut.CAS select c).FirstOrDefault();
                        if (casaut == null)
                        {
                            memo(aut.name + " not found in caslist");
                            continue;
                        }

                        if (pub.year > 2009)
                            casaut.pubyear[pub.year]++;
                    }
                }
            }

            foreach (authorclass aut in caslist)
            {
                bool pub1719 = false;
                int sum1719 = 0;
                string s = "\t" + aut.name + "\t\t\t";
                for (int i = 2017; i < 2020; i++)
                {
                    sum1719 += aut.pubyear[i];
                    if (aut.pubyear[i] > 0)
                        pub1719 = true;
                }
                if (pub1719)
                    s += "Ja";
                else
                    s += "Nej";
                s += "\t" + sum1719;
                memo(s);
            }
        }

        private void MakeWordfile(IEnumerable<pubclass> q, int year, reportrowclass rr, char gender, string dir, string title,string footer)
        {
            string fn = dir;

            fn += "Publikationer " + title + " " + rr.rowtitle + " "+ year;

            //if (rr.pubtypes.Count < 3)
            //{
            //    foreach (string s in rr.pubtypes)
            //        fn += " " + s;
            //}
            //else
            //{
            //    foreach (string s in rr.pubtypes)
            //        fn += " " + s.Substring(0,3);

            //}

            string styp = " alla";
            if (rr.contenttypes.Count == 1)
                styp = " "+rr.contenttypes.First().ToLower();
            else if (rr.contenttypes.Count == 2)
            {
                if (!rr.contenttypes.Contains(pubclass.ctrefstring))
                    styp = " ej refereegranskat";
                else if (!rr.contenttypes.Contains(pubclass.ctpopstring))
                    styp = " vetenskapligt";
                else if (!rr.contenttypes.Contains(pubclass.ctscistring))
                    styp = " refereegranskat eller populärvetenskapligt";
            }
            //if (rr.refstatus == 1)
            //    styp = " refereegranskat";
            //else if (rr.refstatus == -1)
            //    styp = " ej refereegranskat";
            //fn += styp;

            fn += " " + gender + ".docx";

            Console.WriteLine(fn);

            using (DocX document = DocX.Create(fn))
            {
                // Add a new Paragraph to the document.
                Paragraph p1 = document.InsertParagraph();
                p1.Append(title);
                p1.StyleName = "Heading1";

                Paragraph p2 = document.InsertParagraph();
                string s = "Försteförfattare ";
                if (gender == 'M')
                    s += "man";
                else if (gender == 'K' || gender == 'F')
                    s += "kvinna";
                else
                    s = "";
                p2.Append(s).Bold();

                Paragraph p3 = document.InsertParagraph();
                string sp = "Publikationstyp: ";
                string separator = "";
                foreach (string spt in rr.pubtypes)
                {
                    sp += separator + spt;
                    separator = "; ";
                }
                p3.Append(sp);

                Paragraph p4 = document.InsertParagraph();
                p4.Append("Typ av innehåll: " + styp);

                Paragraph p4b = document.InsertParagraph();
                string bb = "Published";
                if (!rr.pubonly)
                    bb = "Published; Epub ahead of print; In press";
                p4b.Append("Status: " + bb);

                Paragraph p5 = document.InsertParagraph();
                p5.Append("Utgivningsår: " + year);

                Paragraph p6 = document.InsertParagraph();
                p6.Append("Antal: " + q.Count());

                Paragraph p7 = document.InsertParagraph();
                p7.Append("Källa: DiVA");

                Paragraph p8 = document.InsertParagraph();
                p8.Append("Datum utsökning DiVA: " + divadate.Date);

                document.InsertParagraph();

                Xceed.Words.NET.List listpub = null;

                SortedList<string, string[]> sortlist = new SortedList<string, string[]>();
                Dictionary<string, string> doublecheckdict = new Dictionary<string, string>();

                foreach (pubclass pub in q)
                {
                    string[] apaparts = pub.makeapa();
                    //if (apaparts[0].Contains("Giedraitis"))
                    //    memo(apaparts[0]);
                    //var qapa = from c in apalist where c.Contains(pub.dict[pubclass.titstring]) select c;
                    //if (qapa.Count() == 0)
                    //    memo("Title not found " + pub.dict[pubclass.titstring]);
                    //else if (qapa.Count() > 0)
                    //    memo("Title duplicate " + pub.dict[pubclass.titstring]);
                    //else
                    //if (qapa.Count() == 1)
                    //{
                    //    apa = qapa.First();
                    //    //memo("APA found!");
                    //}
                    //if (!sortlist.ContainsKey(apa))
                    //    sortlist.Add(apa, apa);
                    string fullapa = util.mergestringvector(apaparts);
                    if (doublecheckdict.ContainsKey(fullapa))
                        memo("DUBLETT: " + pub.dict[pubclass.pidstring] + " " + doublecheckdict[fullapa] + " " + fullapa);
                    else
                    {
                        doublecheckdict.Add(fullapa, pub.dict[pubclass.pidstring]);
                        if (!sortlist.ContainsKey(apaparts[0]))
                            sortlist.Add(apaparts[0], apaparts);
                        else
                        {
                            while (sortlist.ContainsKey(apaparts[0]))
                                apaparts[0] += " ";
                            sortlist.Add(apaparts[0], apaparts);
                        }
                    }
                }
                foreach (string apa in sortlist.Keys)
                {
                    if (listpub == null)
                        listpub = document.AddList(apa.Trim(), 0, ListItemType.Numbered, 1);
                    else
                        document.AddListItem(listpub, apa.Trim());
                    if (util.is_latin(sortlist[apa][1]))
                        listpub.Items.Last().Append(sortlist[apa][1]).Italic();
                    else
                        listpub.Items.Last().Append(sortlist[apa][1]);
                    listpub.Items.Last().Append(sortlist[apa][2]);

                }
                //memo("Listpub: "+listpub.ToString());
                if (listpub != null)
                    document.InsertList(listpub);

                document.InsertParagraph();

                var pp = document.InsertParagraph();

                var t = document.AddTable(4, 2);

                t.SetColumnWidth(0, 4500);
                t.SetColumnWidth(1, 4500);

                // Create a large blue border.
                Border b = new Border(Xceed.Words.NET.BorderStyle.Tcbs_single, BorderSize.two, 0, Color.DarkBlue);

                // Set the tables Top, Bottom, Left and Right Borders to b.
                t.SetBorder(TableBorderType.Top, b);
                t.SetBorder(TableBorderType.Bottom, b);
                t.SetBorder(TableBorderType.Left, b);
                t.SetBorder(TableBorderType.Right, b);
                t.SetBorder(TableBorderType.InsideV, b);

                t.Rows[0].Cells[0].Paragraphs[0].Append("Underlag upprättat av");
                t.Rows[1].Cells[0].Paragraphs[0].Append(TBupprattad.Text);
                if (TBupprattad.Text.Contains("Sverker"))
                    t.Rows[2].Cells[0].Paragraphs[0].Append("Avd stöd till ledning och styrning");
                else
                    t.Rows[2].Cells[0].Paragraphs[0].Append("Biblioteket");
                t.Rows[3].Cells[0].Paragraphs[0].Append("Högskolan Dalarna");

                t.Rows[0].Cells[1].Paragraphs[0].Append("Underlag kontrollerat av");
                t.Rows[1].Cells[1].Paragraphs[0].Append(TBkontrollerad.Text);
                t.Rows[2].Cells[1].Paragraphs[0].Append("Biblioteket");
                t.Rows[3].Cells[1].Paragraphs[0].Append("Högskolan Dalarna");

                pp.InsertTableAfterSelf(t);

                document.AddFooters();
                document.DifferentFirstPage = false;
                document.DifferentOddAndEvenPages = false;
                //document.Footers.First.PageNumbers = true;
                //document.Footers.Even.PageNumbers = true;
                //document.Footers.Odd.PageNumbers = true;
                //document.Footers.Odd.InsertParagraph("Page ").AppendPageNumber(PageNumberFormat.normal).Append(" of ").AppendPageCount(PageNumberFormat.normal);

                Paragraph pf = document.Footers.Odd.InsertParagraph("\t\t"+footer+" ");
                pf.AppendPageNumber(PageNumberFormat.normal);
                pf.Append(" av ");
                pf.AppendPageCount(PageNumberFormat.normal);

                // .AppendPageNumber(PageNumberFormat.normal);
                // InsertParagraph("Page ").AppendPageNumber(PageNumberFormat.normal).Append(" of ").AppendPageCount(PageNumberFormat.normal);


                // Save the document.
                document.Save();
            }


        }

        private void AnnrepTableButton_Click(object sender, EventArgs e)
        {
            if (CB_publists.Checked && apalist.Count == 0)
                ReadAPAlist();

            if (CB_perinst.Checked)
            {
                foreach (string inst in authorclass.institutions)
                    AnnrepTable(inst);
            }
            else if (CBpersubject.Checked)
            {
                foreach (string inst in authorclass.institutions)
                {
                    memo("");
                    memo("#################################################");
                    memo("======== " + inst + " ===========");
                    memo("#################################################");
                    memo("");
                    var dum = authorclass.instsubj[inst];
                    dum.Sort();
                    foreach (string subj in dum)
                    {
                        AnnrepTable(subj);
                    }
                }
            }
            else
                AnnrepTable(""); //everything
        }

        private List<reportrowclass> ReportRows(int reporttype) //reporttype = 0 for annual report, reporttype = 1 for UFN-granskning
        {
            List<reportrowclass> rowlist = new List<reportrowclass>();

            if (reporttype == 0)
            {
                reportrowclass r1 = new reportrowclass();
                r1.rowtitle = "Rapporter";
                r1.pubtypes.Add("Rapport");
                //r1.refstatus = 0;
                r1.contenttypes = contenttypelist.Keys.ToList();
                rowlist.Add(r1);

                reportrowclass r2 = new reportrowclass();
                r2.rowtitle = "Böcker";
                r2.pubtypes.Add("Bok");
                //r1.refstatus = 0;
                r2.contenttypes = contenttypelist.Keys.ToList();
                rowlist.Add(r2);

                reportrowclass r3 = new reportrowclass();
                r3.rowtitle = "Kapitel";
                r3.pubtypes.Add("Kapitel i bok, del av antologi");
                //r1.refstatus = 0;
                r3.contenttypes = contenttypelist.Keys.ToList();
                rowlist.Add(r3);

                reportrowclass r4 = new reportrowclass();
                r4.rowtitle = "Konferens (ref)";
                r4.pubtypes.Add("Konferensbidrag");
                //r4.refstatus = 1;
                r4.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r4);

                reportrowclass r5 = new reportrowclass();
                r5.rowtitle = "Konferens (övr)";
                r5.pubtypes.Add("Konferensbidrag");
                //r5.refstatus = -1;
                r5.contenttypes.Add(pubclass.ctscistring);
                r5.contenttypes.Add(pubclass.ctpopstring);
                rowlist.Add(r5);

                reportrowclass r6 = new reportrowclass();
                r6.rowtitle = "Artiklar (ref)";
                r6.pubtypes.Add("Artikel i tidskrift");
                r6.pubtypes.Add("Artikel, forskningsöversikt");
                //r6.refstatus = 1;
                r6.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r6);

                reportrowclass r7 = new reportrowclass();
                r7.rowtitle = "Artiklar (övr)";
                r7.pubtypes.Add("Artikel i tidskrift");
                r7.pubtypes.Add("Artikel, forskningsöversikt");
                r7.pubtypes.Add("Artikel, recension");
                //r7.refstatus = -1;
                r7.contenttypes.Add(pubclass.ctscistring);
                r7.contenttypes.Add(pubclass.ctpopstring);
                rowlist.Add(r7);
            }
            else if (reporttype == 1)
            {
                reportrowclass r2 = new reportrowclass();
                r2.rowtitle = "Böcker (ref)";
                r2.pubtypes.Add("Bok");
                //r1.refstatus = 0;
                r2.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r2);

                reportrowclass r2b = new reportrowclass();
                r2b.rowtitle = "Böcker (övr)";
                r2b.pubtypes.Add("Bok");
                //r1.refstatus = 0;
                r2b.contenttypes.Add(pubclass.ctpopstring);
                r2b.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r2b);

                reportrowclass r3 = new reportrowclass();
                r3.rowtitle = "Kapitel (ref)";
                r3.pubtypes.Add("Kapitel i bok, del av antologi");
                //r1.refstatus = 0;
                r3.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r3);

                reportrowclass r3b = new reportrowclass();
                r3b.rowtitle = "Kapitel (övr)";
                r3b.pubtypes.Add("Kapitel i bok, del av antologi");
                //r1.refstatus = 0;
                r3b.contenttypes.Add(pubclass.ctpopstring);
                r3b.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r3b);

                reportrowclass r4 = new reportrowclass();
                r4.rowtitle = "Konferens (ref)";
                r4.pubtypes.Add("Konferensbidrag");
                //r4.refstatus = 1;
                r4.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r4);

                reportrowclass r5 = new reportrowclass();
                r5.rowtitle = "Konferens (övr)";
                r5.pubtypes.Add("Konferensbidrag");
                //r5.refstatus = -1;
                r5.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r5);

                reportrowclass r6 = new reportrowclass();
                r6.rowtitle = "Artiklar (ref)";
                r6.pubtypes.Add("Artikel i tidskrift");
                r6.pubtypes.Add("Artikel, forskningsöversikt");
                //r6.refstatus = 1;
                r6.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r6);

                reportrowclass r7 = new reportrowclass();
                r7.rowtitle = "Artiklar (övr)";
                r7.pubtypes.Add("Artikel i tidskrift");
                r7.pubtypes.Add("Artikel, forskningsöversikt");
                r7.pubtypes.Add("Artikel, recension");
                //r7.refstatus = -1;
                r7.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r7);

                reportrowclass r9 = new reportrowclass();
                r9.rowtitle = "Redaktörskap samlingsverk+proceedings";
                r9.pubtypes.Add("Samlingsverk (redaktörskap)");
                r9.pubtypes.Add("Proceedings (redaktörskap)");
                //r9.refstatus = -1;
                r9.contenttypes.Add(pubclass.ctrefstring);
                r9.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r9);

                reportrowclass r1 = new reportrowclass();
                r1.rowtitle = "Rapporter m.m. vetenskapligt";
                r1.pubtypes.Add("Rapport");
                r1.pubtypes.Add("Övrigt");
                r1.pubtypes.Add("Manuskript (preprint)");
                //r1.refstatus = 0;
                r1.contenttypes.Add(pubclass.ctrefstring);
                r1.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r1);

                reportrowclass r8 = new reportrowclass();
                r8.rowtitle = pubclass.ctpopstring;
                r8.pubtypes = pubtypelist.Keys.ToList();
                //r8.refstatus = -1;
                r8.contenttypes.Add(pubclass.ctpopstring);
                rowlist.Add(r8);

                reportrowclass r11 = new reportrowclass();
                r11.rowtitle = "Avhandlingar (PhD+Lic)";
                r11.pubtypes.Add("Doktorsavhandling, sammanläggning");
                r11.pubtypes.Add("Doktorsavhandling, monografi");
                r11.pubtypes.Add("Licentiatavhandling, sammanläggning");
                r11.pubtypes.Add("Licentiatavhandling, monografi");
                //r8.refstatus = -1;
                r11.contenttypes.Add(pubclass.ctscistring);
                rowlist.Add(r11);

                reportrowclass r10 = new reportrowclass();
                r10.rowtitle = "Totalt";
                r10.pubtypes = pubtypelist.Keys.ToList();
                //r8.refstatus = -1;
                r10.contenttypes = contenttypelist.Keys.ToList();
                rowlist.Add(r10);

                reportrowclass r13 = new reportrowclass();
                r13.rowtitle = "(varav refereegranskat)";
                r13.pubtypes.Add("Artikel i tidskrift");
                r13.pubtypes.Add("Artikel, forskningsöversikt");
                r13.pubtypes.Add("Konferensbidrag");
                r13.pubtypes.Add("Bok");
                r13.pubtypes.Add("Kapitel i bok, del av antologi");
                r13.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r13);

                reportrowclass r12 = new reportrowclass();
                r12.rowtitle = "Ref + 3*böcker, wconf = 1";
                r12.pubtypes.Add("Artikel i tidskrift");
                r12.pubtypes.Add("Artikel, forskningsöversikt");
                r12.pubtypes.Add("Konferensbidrag");
                r12.pubtypes.Add("Bok");
                r12.pubtypes.Add("Kapitel i bok, del av antologi");
                r12.weighted = true;
                r12.weights.Add("Artikel i tidskrift", 1);
                r12.weights.Add("Artikel, forskningsöversikt", 1);
                //r12.weights.Add("Konferensbidrag", 1/(double)3);
                r12.weights.Add("Konferensbidrag", 1);
                r12.weights.Add("Bok", 3);
                r12.weights.Add("Kapitel i bok, del av antologi", 1);
                //r12.refstatus = 1;
                r12.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r12);

                reportrowclass r14 = new reportrowclass();
                r14.rowtitle = "Ref + 3*böcker, wconf=1/3";
                r14.pubtypes.Add("Artikel i tidskrift");
                r14.pubtypes.Add("Artikel, forskningsöversikt");
                r14.pubtypes.Add("Konferensbidrag");
                r14.pubtypes.Add("Bok");
                r14.pubtypes.Add("Kapitel i bok, del av antologi");
                r14.weighted = true;
                r14.weights.Add("Artikel i tidskrift", 1);
                r14.weights.Add("Artikel, forskningsöversikt", 1);
                r14.weights.Add("Konferensbidrag", 1/(double)3);
                //r14.weights.Add("Konferensbidrag", 1);
                r14.weights.Add("Bok", 3);
                r14.weights.Add("Kapitel i bok, del av antologi", 1);
                //r14.refstatus = 1;
                r14.contenttypes.Add(pubclass.ctrefstring);
                rowlist.Add(r14);

                foreach (var rr in rowlist)
                    rr.perinst = !CBauthorpin.Checked;
            }

            return rowlist;

        }

        private double pubsum(IEnumerable<pubclass> q,reportrowclass rr, char gender, int year, string aff, bool frac, bool perinst,string inst)
        {
            double sum = 0;
            IEnumerable<pubclass> qm;
            bool instorsubj = authorclass.institutions.Contains(inst);
            if (gender == ' ')
            {
                qm = from c in q where c.year == year select c;
            }
            else
            {
                qm = from c in q where c.year == year where c.firstHDa.gender == gender select c;
            }
            if (frac)
            {
                if (rr.weighted)
                {
                    foreach (pubclass pc in qm)
                    {
                        sum += pc.fraction(aff,util.tryconvert(TBfractak.Text))*rr.weights[pc.dict[pubclass.pubtypestring]];
                    }
                }
                else
                {
                    foreach (pubclass pc in qm)
                    {
                        sum += pc.fraction(aff, util.tryconvert(TBfractak.Text));
                    }
                }
            }
            else
            {
                if (rr.weighted)
                {
                    if (perinst)
                    {
                        foreach (pubclass pc in qm)
                        {
                            sum += rr.weights[pc.dict[pubclass.pubtypestring]];
                        }
                    }
                    else
                    {
                        foreach (pubclass pc in qm)
                        {
                            List<string> afflist = instorsubj ? pc.get_authorinstitution(true, false) : pc.get_authorsubjectalias();
                            foreach (string ii in afflist)
                            {
                                if (ii==inst)
                                    sum += rr.weights[pc.dict[pubclass.pubtypestring]];
                            }
                        }
                    }
                }
                else if (perinst)
                {
                    sum = qm.Count();
                }
                else
                {
                    foreach (pubclass pc in qm)
                    {
                        List<string> afflist = instorsubj ? pc.get_authorinstitution(true, false) : pc.get_authorsubjectalias();
                        foreach (string ii in afflist)
                        {
                            if (ii == inst)
                                sum++;
                        }    
                    }
                }
            }

            return sum;
        }

        private void AnnrepTable(string inst)
        {
            string header = "Publikationstyp";

            int startyear = util.tryconvert(TB_startyear.Text);
            if (startyear < 0)
            {
                memo("Invalid startyear " + TB_startyear.Text);
                return;
            }

            int endyear = util.tryconvert(TB_endyear.Text);
            if (endyear < startyear)
            {
                memo("Invalid endyear " + TB_endyear.Text);
                return;
            }

            for (int year = endyear; year >= startyear; year--)
            {
                header += "\t" + year;
            }
            if (CBfraction.Checked)
            {
                header += "\t Fraktionerat (tak="+TBfractak.Text+"):";
                for (int year = endyear; year >= startyear; year--)
                {
                    header += "\t" + year;
                }
            }
            if (!String.IsNullOrEmpty(inst)) 
            {
                memo("");
                memo("======== " + inst + " ===========");
            }
            memo(header);
            memo("");

            int reporttype = 0;
            if (!CBannrep.Checked)
                reporttype = 1;
            List<reportrowclass> rowlist = ReportRows(reporttype);
            string dir = "";
            if (CB_publists.Checked)
            {
                dir = TB_worddir.Text;
                if (!dir.EndsWith(@"\"))
                    dir += @"\";

                //DateTime now = DateTime.Now;
                //dir += now.Year + "-" + now.Month + "-" + now.Day + " " + now.Hour + "-" + now.Minute + @"\";
                dir = util.timestampfolder(dir,"Word pub-lists ");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            string aff = inst;
            if (String.IsNullOrEmpty(aff))
                aff = "Dalarna";

            List<string> rpid = new List<string>();
            foreach (reportrowclass rr in rowlist)
            {
                memo(rr.rowtitle);
                bool totalrow = rr.rowtitle == "Totalt";
                IEnumerable<pubclass> q;
                q = from c in publist
                    where rr.pubtypes.Contains(c.dict[pubclass.pubtypestring])
                    where c.firstHDa != null
                    select c;
                //memo("q1 "+q.Count().ToString());
                if (rr.pubonly)
                    q = from c in q where (c.dict.ContainsKey(pubclass.statusstring) && c.dict[pubclass.statusstring] == "published") select c;
                if (rr.nosubacc)
                    q = from c in q where (!c.dict.ContainsKey(pubclass.statusstring) || (c.dict[pubclass.statusstring] != "submitted" && c.dict[pubclass.statusstring] != "accepted")) select c;
                //memo("q2 " + q.Count().ToString());
                if (rr.contenttypes.Count < contenttypelist.Count)
                    q = from c in q where rr.contenttypes.Contains(c.dict[pubclass.contenttypestring]) select c;
                //if (rr.refstatus == 1)
                //    q = from c in q where c.dict[pubclass.contenttypestring] == "Refereegranskat" select c;
                //else if (rr.refstatus == -1)
                //    q = from c in q where c.dict[pubclass.contenttypestring] != "Refereegranskat" select c;
                //memo("q3 " + q.Count().ToString());
                if (!String.IsNullOrEmpty(inst))
                {
                    if (authorclass.institutions.Contains(inst))
                    {
                        if (CBanyauthorpos.Checked)
                        {
                            q = from c in q
                                where c.get_authorinstitution(false, true).Contains(inst)
                                select c;
                        }
                        else
                        {
                            q = from c in q
                                where c.firstHDa.getinstitution(false).Contains(inst)
                                select c;
                        }
                    }
                    else
                    {
                        if (CBanyauthorpos.Checked)
                        {
                            q = from c in q
                                where c.get_authorsubjectalias().Contains(inst)
                                select c;
                        }
                        else
                        {
                            q = from c in q
                                where c.firstHDa.getsubjectalias().Contains(inst)
                                select c;
                        }
                    }
                    if (CB_publists.Checked && q.Count() > 0)
                    {
                        int fakeyear = 100 * (startyear % 100) + (endyear % 100);
                        MakeWordfile(q, fakeyear, rr, ' ', dir, inst,"Publikationer "+inst);
                    }
                }
                string ms = "manlig förf.";
                char gender = 'M';
                string footerstring = "ÅR HDa " + (DateTime.Now.Year - 1);
                for (int year = endyear; year >= startyear; year--)
                {
                    //ms += "\t" + qm.Count();
                    ms += "\t" + pubsum(q, rr, gender, year, aff,false,rr.perinst,inst);
                    if (CB_publists.Checked && String.IsNullOrEmpty(inst))
                    {
                        var qm = from c in q where c.year == year where c.firstHDa.gender == gender select c;
                        MakeWordfile(qm, year, rr, gender, dir, "Högskolan Dalarna", footerstring);
                    }
                    //return;
                }
                if (CBfraction.Checked)
                {
                    ms += "\t"; //fraktionerat:
                    for (int year = endyear; year >= startyear; year--)
                    {
                        //var qm = from c in q where c.year == year where c.firstHDa.gender == gender select c.fraction(aff);
                        //ms += "\t" + qm.Sum();
                        ms += "\t" + pubsum(q, rr, gender, year, aff,true, rr.perinst, inst).ToString("F2");
                        //return;
                    }
                }
                memo(ms);
                string ks = "kvinnlig förf.";
                gender = 'F';
                for (int year = endyear; year >= startyear; year--)
                {
                    //ks += "\t" + qk.Count();
                    ks += "\t" + pubsum(q, rr, gender, year, aff,false, rr.perinst, inst);
                    if (CB_publists.Checked && String.IsNullOrEmpty(inst))
                    {
                        var qk = from c in q where c.year == year where c.firstHDa.gender == gender select c;
                        MakeWordfile(qk, year, rr, gender, dir, "Högskolan Dalarna", footerstring);
                    }
                }
                if (CBfraction.Checked)
                {
                    ks += "\t"; //fraktionerat:
                    for (int year = endyear; year >= startyear; year--)
                    {
                        //var qk = from c in q where c.year == year where c.firstHDa.gender == gender select c.fraction(aff);
                        //ks += "\t" + qk.Sum();
                        ks += "\t" + pubsum(q, rr, gender, year, aff, true, rr.perinst, inst).ToString("F2");
                    }
                }
                memo(ks);

                foreach (pubclass pp in q)
                {
                    if (!pp.has(pubclass.firsthdagenderstring))
                        pp.dict.Add(pubclass.firsthdagenderstring, pp.firstHDa.gender.ToString());
                    if (!pp.has(pubclass.pubgroupstring))
                        pp.dict.Add(pubclass.pubgroupstring, rr.rowtitle);
                }
                string ss = "summa";
                for (int year = endyear; year >= startyear; year--)
                {
                    //ss += "\t" + qs.Count();
                    ss += "\t" + pubsum(q, rr, ' ', year, aff,false, rr.perinst, inst);
                    var qs = from c in q where c.year == year select c;
                    if (!totalrow)
                    {
                        foreach (pubclass pc in qs)
                        {
                            rpid.Add(pc.dict[pubclass.pidstring]);
                        }
                    }
                    else
                    {
                        foreach (pubclass pc in qs)
                        {
                            if (!rpid.Contains(pc.dict[pubclass.pidstring]))
                            {
                                memo(pc.dict[pubclass.pidstring] + "\t" + pc.dict[pubclass.contenttypestring] + "\t" + pc.dict[pubclass.pubtypestring]);
                            }
                            
                        }

                    }
                }
                if (CBfraction.Checked)
                {
                    ss += "\t"; //fraktionerat:
                    for (int year = endyear; year >= startyear; year--)
                    {
                        //var qs = from c in q where c.year == year select c.fraction(aff);
                        //ss += "\t" + qs.Sum();
                        ss += "\t" + pubsum(q, rr, ' ', year, aff, true, rr.perinst, inst).ToString("F2");
                    }
                }
                memo(ss);

                //break;
            }


        }

        private void CB_publists_CheckedChanged(object sender, EventArgs e)
        {
            label3.Visible = CB_publists.Checked;
            TB_worddir.Visible = CB_publists.Checked;
        }

        private void ProfileSubjectButton_Click(object sender, EventArgs e)
        {
            FormProfileSubject fps = new FormProfileSubject(this);
            fps.Show();
        }

        private string insertcomma(string name, int n) //insert comma in n:th gap
        {
            string[] parts = name.Split();
            string s = "";
            for (int i = 0; i < parts.Length; i++)
            {
                s += parts[i];
                if (i == n - 1)
                    s += ", ";
                else
                    s += " ";
            }
            return s.Trim();

        }

        private void institutionbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Institutionsfil:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fncas = openFileDialog1.FileName;
            memo("Reading " + fncas);

            using (StreamReader sr = new StreamReader(fncas))
            {
                string inst = "";
                string dep = "";
                int ngood = 0;
                int n = 0;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim('\t');
                    string[] words = line.Split('\t');
                    if (words.Length > 1) //inst/dep name
                    {
                        if (!String.IsNullOrEmpty(words[1]))
                            inst = words[1].Trim(':');
                        else if (!String.IsNullOrEmpty(words[2]))
                            dep = words[2];
                    }
                    else //teacher name
                    {
                        if (String.IsNullOrEmpty(words[0]))
                            continue;
                        if (words[0].Contains("("))
                            continue;
                        n++;
                        authorclass ac = authorclass.authorfromname(words[0], caslist);
                        int k = 1;
                        while ((ac == null) && (k < 4))
                        {
                            ac = authorclass.authorfromname(insertcomma(words[0], k), caslist);
                            k++;
                        }
                        if (ac == null)
                            memo(words[0] + " not found");
                        else
                        {
                            ac.affiliation.Add(inst);
                            if (!String.IsNullOrEmpty(dep))
                                ac.affiliation.Add("Avdelning " + dep);
                            ngood++;
                        }
                    }
                }
                memo("institutionlist entries: " + n);
                memo("match inst/cas entries: " + ngood);
            }


            using (StreamWriter sw = new StreamWriter("casinst.txt"))
            {
                sw.WriteLine("Namn\tCAS\tKön\tProfil\tÄmne\tAnteckningar\tInstitution\tAvdelning");
                foreach (authorclass ac in caslist)
                {
                    sw.Write(ac.name + "\t" + ac.CAS + "\t");
                    if (ac.gender == 'F')
                        sw.Write("1\t");
                    else
                        sw.Write("2\t");
                    bool first = true;
                    foreach (string aff in ac.affiliation)
                        if (authorclass.profiles.Contains(aff))
                        {
                            if (!first)
                                sw.Write(",");
                            first = false;
                            sw.Write(aff);
                        }
                    first = true;
                    sw.Write("\t");
                    foreach (string aff in ac.affiliation)
                        if (authorclass.subjects.Contains(aff))
                        {
                            if (!first)
                                sw.Write(",");
                            first = false;
                            sw.Write(aff);
                        }
                    sw.Write("\t" + ac.notes + "\t");
                    foreach (string aff in ac.affiliation)
                        if (aff.Contains("Institution"))
                            sw.Write(aff);
                    sw.Write("\t");
                    foreach (string aff in ac.affiliation)
                        if (aff.Contains("Avdelning"))
                            sw.Write(aff);
                    sw.WriteLine();
                }
            }

        }

        private void scopusbutton_OLD(object sender, EventArgs e)
        {
            List<pubclass> scopuslist = new List<pubclass>();

            openFileDialog1.Title = "SCOPUS-fil:";

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            //divadate = File.GetCreationTime(fn);

            string[] separator = { "\t" };
            using (StreamReader sr = new StreamReader(fn))
            {

                string header = sr.ReadLine();
                string[] headsplit = header.Split('\t');
                Dictionary<string, int> hd = new Dictionary<string, int>();
                for (int i = 0; i < headsplit.Length; i++)
                {
                    hd.Add(headsplit[i], i);
                }
                int npub = 0;

                while (!sr.EndOfStream)
                {
                    npub++;

                    //if (npub > 10)
                    //    break;

                    string line = sr.ReadLine();
                    if (String.IsNullOrEmpty(line))
                        continue;
                    string[] words = line.Split(separator, StringSplitOptions.None);
                    pubclass pc = new pubclass();
                    for (int i = 0; i < Math.Min(words.Length, headsplit.Length); i++)
                    {
                        pc.dict.Add(headsplit[i], words[i].Trim(new char[] { ' ', '"' }));
                    }
                    if (!pc.dict.ContainsKey(pubclass.authorstringscopus))
                        continue;
                    pc.authors = authorclass.parseauthors(pc.dict[pubclass.authorstringscopus]);
                    pc.fill_casauthors(caslist);
                    pc.cleantitle();
                    pc.setyear();



                    scopuslist.Add(pc);


                }
            }

            memo("Scopus Publications: " + scopuslist.Count);

            Dictionary<string, List<string>> psdict = new Dictionary<string, List<string>>();

            foreach (pubclass scopub in scopuslist)
            {
                string doi = scopub.dict[pubclass.doistring];
                if (!string.IsNullOrEmpty(doi))
                {
                    var q = from c in publist where c.dict[pubclass.doistring] == doi select c;
                    if (q.Count() > 1)
                    {
                        memo("Multiple DIVA PID for same DOI " + doi);
                    }
                    else
                    {
                        foreach (pubclass pc in q)
                        {
                            if (String.IsNullOrEmpty(pc.dict[pubclass.scopusstring]))
                            {
                                //memo(pc.dict[pubclass.pidstring] + "\t" + doi + "\t" + scopub.dict[pubclass.eidstring] + "\t" + pc.dict[pubclass.titstring] + "\t" + scopub.dict[pubclass.titstring]);
                                if (!psdict.ContainsKey(pc.dict[pubclass.pidstring]))
                                    psdict.Add(pc.dict[pubclass.pidstring], new List<string>());
                                psdict[pc.dict[pubclass.pidstring]].Add(scopub.dict[pubclass.eidstring]);
                            }
                        }
                    }
                }
            }
            foreach (string pid in psdict.Keys)
            {
                if (psdict[pid].Count == 1)
                    memo("\"" + pid + "\",\"" + psdict[pid].First() + "\"");
                else
                    memo("Multiple scopus ID for PID " + pid);
            }

        }

        private void scopusbutton_Click(object sender, EventArgs e)
        {
            List<pubclass> scopuslist = new List<pubclass>();

            openFileDialog1.Title = "SCOPUS-fil:";

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            //divadate = File.GetCreationTime(fn);

            string[] separator = { "\t" };
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                string yearheader = sr.ReadLine();
                string header = sr.ReadLine();
                string[] headsplit = header.Split('\t');
                string[] yearheadsplit = yearheader.Split('\t');
                Dictionary<string, int> hd = new Dictionary<string, int>();
                for (int i = 0; i < headsplit.Length; i++)
                {
                    hd.Add(headsplit[i], i);
                }
                for (int i = 0; i < yearheadsplit.Length; i++)
                {
                    if (!String.IsNullOrEmpty(yearheadsplit[i]))
                        hd.Add(yearheadsplit[i], i);
                }


                int npub = 0;

                while (!sr.EndOfStream)
                {
                    npub++;

                    //if (npub > 10)
                    //    break;

                    string line = sr.ReadLine();
                    if (String.IsNullOrEmpty(line))
                        continue;
                    string[] words = line.Split(separator, StringSplitOptions.None);
                    pubclass pc = new pubclass();
                    for (int i = 0; i < Math.Min(words.Length, headsplit.Length); i++)
                    {
                        if (!String.IsNullOrEmpty(yearheadsplit[i]))
                            pc.dict.Add(yearheadsplit[i], words[i].Trim(new char[] { ' ', '"' }));
                        else
                            pc.dict.Add(headsplit[i], words[i].Trim(new char[] { ' ', '"' }));
                    }
                    if (!pc.dict.ContainsKey(pubclass.authorstringscopus))
                        continue;
                    pc.authors = authorclass.parseauthors(pc.dict[pubclass.authorstringscopus]);
                    pc.fill_casauthors(caslist);
                    pc.cleantitle();
                    pc.setyear();



                    scopuslist.Add(pc);


                }
            }



            memo("Scopus Publications: " + scopuslist.Count);

            Dictionary<string, List<string>> psdict = new Dictionary<string, List<string>>();

            int nmatch = 0;

            var qissn = from c in publist where c.has(pubclass.pissnstring) || c.has(pubclass.eissnstring) select c;

            foreach (pubclass scopub in scopuslist)
            {
                //string doi = scopub.dict[pubclass.doistring];
                string tit = scopub.lowtitle;
                string issn = scopub.dict[pubclass.issnstringscopus].Insert(4, "-");
                pubclass pc = null;
                if (!string.IsNullOrEmpty(tit))
                {
                    var q = from c in qissn where c.lowtitle == tit select c;
                    if (q.Count() > 1)
                    {
                        //memo("Multiple DIVA PID for same title " + tit);
                        var qm = from c in q where c.get_issn() == issn select c;
                        if (qm.Count() == 1)
                            pc = qm.First();
                        else
                            memo("Multiple DIVA PID mut no matching ISSN for title " + tit);
                    }
                    else if (q.Count() == 0)
                    {
                        //memo("No DIVA PID for title " + tit);
                        var q3 = from c in qissn where c.get_issn() == issn select c;
                        var q2 = from c in q3 where util.EditDistance(c.lowtitle, tit) < 4 select c;
                        if (q2.Count() == 1)
                            pc = q2.First();
                        else if (q2.Count() > 1)
                        {
                            int dist = 999;
                            foreach (pubclass pcq in q2)
                            {
                                if (util.EditDistance(pcq.lowtitle, tit) < dist)
                                {
                                    dist = util.EditDistance(pcq.lowtitle, tit);
                                    pc = pcq;
                                }
                            }
                        }
                        else
                        {
                            foreach (pubclass pcq in q3)
                            {
                                if (tit.Contains(pcq.lowtitle))
                                {
                                    pc = pcq;
                                    break;
                                }
                            }
                            if (pc == null)
                                memo("No DIVA PID for title " + tit);
                        }
                    }
                    else
                    {
                        pc = q.First();
                        //nmatch++;
                    }
                }
                if (pc != null)
                {
                    nmatch++;
                    if (scopub.has("total") && !pc.has(pubclass.scopuscitationstring))
                        pc.dict.Add(pubclass.scopuscitationstring, scopub.dict["total"]);
                }    
            }

            memo("nmatch Scopus = " + nmatch);

            //foreach (string pid in psdict.Keys)
            //{
            //    if (psdict[pid].Count == 1)
            //        memo("\"" + pid + "\",\"" + psdict[pid].First() + "\"");
            //    else
            //        memo("Multiple scopus ID for PID " + pid);
            //}



        }

        private void corpusbutton_Click(object sender, EventArgs e)
        {
            //sig_00_20xx.txt
            Dictionary<string, List<pubclass>> audict = new Dictionary<string, List<pubclass>>();

            foreach (pubclass pc in publist)
            {
                foreach (string cas in pc.casauthors)
                {
                    if (!audict.ContainsKey(cas))
                        audict.Add(cas, new List<pubclass>());
                    audict[cas].Add(pc);
                }

            }
            int titfactor = 3;
            string dir = TB_worddir.Text;
            foreach (string cas in audict.Keys)
            {
                string fn = dir + cas + "_00_2021.txt";
                using (StreamWriter sw = new StreamWriter(fn))
                {
                    foreach (pubclass pc in audict[cas])
                    {
                        for (int i = 0; i < titfactor; i++)
                            sw.WriteLine(pc.dict[pubclass.titstring]);
                        if (pc.dict.ContainsKey(pubclass.abstractstring))
                            sw.WriteLine(pc.dict[pubclass.abstractstring]);
                    }
                }
            }
        }

        private void read_journalimpact()
        {
            if (journallist.Count > 0)
                return;

            openFileDialog1.Title = "Journal-fil:"; //JCR2021.txt i Invärld
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            using (StreamReader sr = new StreamReader(fn))
            {
                int ngood = 0;
                int n = 0;
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim('\t');
                    string[] words = line.Split('\t');
                    n++;
                    if (words.Length < 4)
                        continue;
                    string jname = cleanjournaltitle(words[1]);
                    var q = from c in journallist
                            where c.match(jname, null)
                            select c;
                    if (q.Count() > 0)
                        continue;

                    journalclass jc = new journalclass();
                    jc.rank = util.tryconvert(words[0]);
                    jc.name = jname;
                    jc.citations = util.tryconvert(words[2].Replace(" ",""));
                    jc.impact = util.tryconvertdouble(words[3]);
                    journallist.Add(jc);
                }
                memo("journallist entries: " + n);
            }

        }

        private string cleanjournaltitle(string original)
        {
            string s = original.ToLower().Trim(new char[] { ' ', '"' });
            if (s.StartsWith("the "))
            {
                s = s.Substring(4);
            }
            if (s.Contains(" and "))
                s = s.Replace(" and ", " & ");
            if (s.Contains(" och "))
                s = s.Replace(" och ", " & ");
            if (s.StartsWith("The "))
                s = s.Replace("The ", "");

            return s.Trim();
        }

        private void read_journal_norska()
        {
            var q0 = from c in journallist where c.norska.Count > 0 select c;
            if (q0.Count() > 0)
                return;

            openFileDialog1.Title = "Norska-listan-fil tidskrifter:"; //Norska-listan-221110.... i Invärld
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            using (StreamReader sr = new StreamReader(fn))
            {
                int ngood = 0;
                int n = 0;
                string header = sr.ReadLine();
                char splitchar = header.Contains(';') ? ';' : ',';
                string[] hwords = header.Split(splitchar);
                Dictionary<string, int> hdict = new Dictionary<string, int>();
                for (int i = 0; i < hwords.Length; i++)
                    hdict.Add(hwords[i], i);

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = util.splitcsv(line,splitchar);
                    n++;
                    if (words == null)
                        continue;
                    if (words.Length < 4)
                        continue;
                    string jname = cleanjournaltitle(words[hdict["International title"]]);
                    if (n % 1000 == 0)
                        memo(n + " " + jname);
                    string pissn = words[hdict["Print ISSN"]];
                    string eissn = words[hdict["Online ISSN"]];
                    string issn = String.IsNullOrEmpty(pissn) ? eissn : pissn;
                    journalclass jc;
                    var q = from c in journallist
                            where c.match(jname, issn)
                            select c;
                    bool newjournal = false;
                    if (q.Count() > 0)
                        jc = q.First();
                    else
                    {
                        jc = new journalclass();
                        newjournal = true;
                    }
                    jc.addname(jname);
                    jc.addname(words[hdict["Original title"]]);
                    jc.printISSN = pissn;
                    jc.eISSN = eissn;
                    jc.NPIdiscipline = words[hdict["NPI Academic Discipline"]];
                    jc.NPIfield = words[hdict["NPI Scientific Field"]];

                    if (jc.norska.Count == 0)
                    {
                        string levelstring = "Level ";
                        foreach (string h in hdict.Keys)
                        {
                            if (h.StartsWith(levelstring))
                            {
                                int year = util.tryconvert(h.Replace(levelstring, "").Trim());
                                int? level = 0;
                                if (words[hdict[h]] == "X")
                                    level = null;
                                else
                                    level = util.tryconvert(words[hdict[h]]);
                                if (level < 0)
                                    level = null;
                                jc.norska.Add(year, level);
                            }
                        }
                    }

                    if (newjournal)
                        journallist.Add(jc);
                }
                memo("journallist entries with Norska listan: " + journallist.Count);
            }


        }

        private void Journalbutton_Click(object sender, EventArgs e)
        {
            read_journalimpact();
            read_publisher_norska();
            read_journal_norska();

            int startyear = util.tryconvert(TB_startyear.Text);
            if (startyear < 0)
            {
                memo("Invalid startyear " + TB_startyear.Text);
                return;
            }

            int endyear = util.tryconvert(TB_endyear.Text);
            if (endyear < 0)
            {
                memo("Invalid endyear " + TB_endyear.Text);
                return;
            }



            //List<string> summarystrings = new List<string>();
            //summarystrings.AddRange(publevelsummary("", startyear,endyear));

            string header = "\tPub totalt\tArtiklar ref\tMed impact\tMed ScopusID\tScopus visibility\tMed WoS\tWoS visibility\tNorska 1\tNorska 2\tBöcker/bokkapitel\tNorska 1\tNorska 2\tScopus, WoS & Norska\tScopus & WoS\tScopus & Norska\tWoS & Norska\tBara Scopus\tBara WoS\tBara Norska\tIngen";
            memo("HDa"+header);                                                                                                                                                        //"\t"+n3+"\t"+nsw+"\t"+nsn+"\t"+nwn+"\t"+ns+"\t"+nw+"\t"+nn+"\t"+n0;
            foreach (string ss in publevelsummary("", startyear, endyear,false))
                memo(ss);


            if (CB_perinst.Checked)
            {
                foreach (string inst in authorclass.institutions)
                {
                    memo(inst+header);
                    foreach (string ss in publevelsummary(inst, startyear, endyear,false))
                        memo(ss);
                }
            }
            //memo("");
            //foreach (string s in summarystrings)
            //    memo(s);

        }

        private List<string> publevelsummary(string inst, int startyear, int endyear,bool printoutlets)
        {
            List<string> ls = new List<string>();
            for (int year = startyear; year <= endyear; year++)
                ls.Add(publevelsummary(inst, year,printoutlets));
            return ls;
        }

        private string publevelsummary(string inst, int year,bool printoutlets)
        {
            Dictionary<string, double> journaldict = new Dictionary<string, double>();
            Dictionary<string, double> publisherdict = new Dictionary<string, double>();

            string aff = String.IsNullOrEmpty(inst) ? "Dalarna" : inst;
            if (printoutlets)
            {
                memo("=====================================================");
                memo("Publikationsklassning för " + aff + " " + year);
                memo("-----------------------------------------------------");
            }

            var qpub = from c in publist where c.firstHDa != null select c;
            //if (rr.nosubacc)
            qpub = from c in qpub where (!c.dict.ContainsKey(pubclass.statusstring) || (c.dict[pubclass.statusstring] != "submitted" && c.dict[pubclass.statusstring] != "accepted")) select c;
            if (year > 0)
                qpub = from c in qpub where c.year == year select c;

            if (!String.IsNullOrEmpty(inst))
            {
                if (CBanyauthorpos.Checked)
                {
                    qpub = from c in qpub
                           where c.get_authorinstitution(false,true).Contains(inst)//hasaffiliation(inst,true)
                        select c;
                }
                else
                {
                    qpub = from c in qpub
                           where c.firstHDa.getinstitution(false) == inst
                        select c;
                }
            }

            double njpub = 0;
            double nbpub = 0;
            double npubtotal = 0;
            double nscopus = 0;
            double nisi = 0;

            double n3 = 0;
            double nsw = 0;
            double nsn = 0;
            double nwn = 0;
            double ns = 0;
            double nw = 0;
            double nn = 0;
            double n0 = 0;

            foreach (pubclass pub in qpub)
            {
                
                double w = 1;
                if (CBfraction.Checked)
                    w = pub.fraction(aff,util.tryconvert(TBfractak.Text));
                npubtotal += w;

                if (pub.dict[pubclass.pidstring] == "941186")
                {
                    int ii = 0;
                }
                if (pub.dict.ContainsKey(pubclass.journalstring) && pub.dict[pubclass.contenttypestring] == "Refereegranskat")
                {
                    njpub+=w;
                    if (pub.dict.ContainsKey(pubclass.scopusstring))
                        nscopus += w;
                    if (pub.dict.ContainsKey(pubclass.isistring))
                        nisi += w;
                    string j = cleanjournaltitle(pub.dict[pubclass.journalstring]);
                    string pissn = pub.dict.ContainsKey(pubclass.pissnstring) ? pub.dict[pubclass.pissnstring] : "";
                    string eissn = pub.dict.ContainsKey(pubclass.eissnstring) ? pub.dict[pubclass.eissnstring] : "";

                    var q = from c in journallist where c.match(j, pissn, eissn) select c;
                    if (q.Count() > 0)
                    {
                        journalclass jc = q.First();
                        j = jc.name;
                        if ((jc.impact > 0)&& !pub.dict.ContainsKey(pubclass.impactstring))
                            pub.dict.Add(pubclass.impactstring, jc.impact.ToString("N1"));
                        if (jc.norska.ContainsKey(pub.year) && !pub.dict.ContainsKey(pubclass.norskastring))
                            pub.dict.Add(pubclass.norskastring, jc.norska[pub.year].ToString());
                    }


                    if (!String.IsNullOrEmpty(j))
                    {
                        if (!journaldict.ContainsKey(j))
                            journaldict.Add(j, w);
                        else
                            journaldict[j]+=w;
                    }

                    if (pub.has(pubclass.scopusstring))
                    {
                        if (pub.has(pubclass.isistring))
                        {
                            if (pub.has(pubclass.norskastring))
                            {
                                n3 += w;
                            }
                            else
                            {
                                nsw += w;
                            }
                        }
                        else
                        {
                            if (pub.has(pubclass.norskastring))
                            {
                                nsn += w;
                            }
                            else
                            {
                                ns += w;
                            }

                        }

                    }
                    else
                    {
                        if (pub.has(pubclass.isistring))
                        {
                            if (pub.has(pubclass.norskastring))
                            {
                                nwn += w;
                            }
                            else
                            {
                                nw += w;
                            }
                        }
                        else
                        {
                            if (pub.has(pubclass.norskastring))
                            {
                                nn += w;
                            }
                            else
                            {
                                n0 += w;
                            }

                        }
                    }

                }
                else if ((pub.dict[pubclass.pubtypestring] == "Bok")||(pub.dict[pubclass.pubtypestring] == "Kapitel i bok, del av antologi"))
                {
                    nbpub+=w;
                    string j = cleanjournaltitle(pub.dict[pubclass.publisherstring]);
                    List<string> isbnlist;
                    if (pub.dict.ContainsKey(pubclass.isbnstring))
                        isbnlist = util.parseISBN(pub.dict[pubclass.isbnstring]);
                    else
                        isbnlist = new List<string>();
                    var q = from c in publisherlist where c.match(j, isbnlist) select c;
                    if (q.Count() > 0)
                    {
                        publisherclass pc = q.First();
                        j = pc.name;
                        if (pc.norska.ContainsKey(pub.year) && !pub.dict.ContainsKey(pubclass.norskastring))
                            pub.dict.Add(pubclass.norskastring, pc.norska[pub.year].ToString());
                    }
                    //if (j.StartsWith("sage "))
                    //    memo("Sage: "+pub.dict[pubclass.isbnstring]+pub.dict[pubclass.titstring]);
                    if (!string.IsNullOrEmpty(j))
                    {
                        if (!publisherdict.ContainsKey(j))
                            publisherdict.Add(j, w);
                        else
                            publisherdict[j]+=w;
                    }
                }
                //else //double-check
                //{
                //    if (pub.dict.ContainsKey(pubclass.scopusstring))
                //        memo("Scopus non-ref "+pub.dict[pubclass.titstring]);
                //    if (pub.dict.ContainsKey(pubclass.isistring))
                //        memo("ISI non-ref " + pub.dict[pubclass.titstring]);

                //}
            }

            //memo("=== " + inst + " ref-artiklars synlighet ===");
            //memo("Ingen synlighet\t"+n0);
            //memo("Synliga endast i Scopus\t" + ns);
            //memo("Synliga endast i WoS\t" + nw);
            //memo("Synliga endast i Norska Listan\t" + nn);
            //memo("Synliga i Scopus och WoS\t" + nsw);
            //memo("Synliga i WoS och Norska Listan\t" + nwn);
            //memo("Synliga i Scopus och Norska Listan\t" + nsn);
            //memo("Synliga i Scopus, WoS och Norska\t" + n3);

            if (printoutlets)
            {
                memo("===== Tidskrifter ===========");
                memo("Namn\t# pub\tImpact\tNorska listan");
            }
            double nimpact = 0;
            //int npub = 0;
            Dictionary<int, double> njnorskdict = new Dictionary<int, double>() { { 0, 0 }, { 1, 0 }, { 2, 0 } };

            foreach (string j in journaldict.Keys)
            {
                //npub += journaldict[j];
                var q = from c in journallist where c.match(j, null) select c;
                if (q.Count() > 0)
                {
                    journalclass jc = q.First();
                    if (jc.impact > 0)
                    {
                        nimpact += journaldict[j];
                    }
                    string norskmax = "";
                    if (jc.norska.Count > 0)
                    {
                        int? inmax = jc.norska.Values.Max();
                        if (inmax != null && njnorskdict.ContainsKey((int)inmax))
                            njnorskdict[(int)inmax] += journaldict[j];
                        norskmax = jc.norska.Values.Max().ToString();
                    }
                    if (printoutlets)
                        memo(j + "\t" + journaldict[j] + "\t"
                        + (jc.impact > 0 ? jc.impact.ToString("N1") : "")
                        + "\t" + norskmax);
                }
                else if (printoutlets)
                    memo(j + "\t" + journaldict[j]);
            }

            if (printoutlets)
            {
                memo("");
                memo("===== Förlag ===========");
                memo("Namn\t# pub\tNorska listan");
            }
            Dictionary<int, double> nbnorskdict = new Dictionary<int, double>() { { 0, 0 }, { 1, 0 }, { 2, 0 } };

            foreach (string j in publisherdict.Keys)
            {
                //npub += publisherdict[j];
                var q = from c in publisherlist where c.match(j, "") select c;
                if (q.Count() > 0)
                {
                    publisherclass jc = q.First();
                    string norskmax = "";
                    if (jc.norska.Count > 0)
                    {
                        int? inmax = jc.norska.Values.Max();
                        if (inmax != null && nbnorskdict.ContainsKey((int)inmax))
                            nbnorskdict[(int)inmax] += publisherdict[j];
                        norskmax = jc.norska.Values.Max().ToString();
                    }
                    if (printoutlets)
                        memo(j + "\t" + publisherdict[j] + "\t" + norskmax);
                }
                else if (printoutlets)
                    memo(j + "\t" + publisherdict[j]);
            }

            if (printoutlets)
            {
                memo("#Tidskrifter = " + publisherdict.Count);
                memo("#Tidskriftsartiklar = " + njpub);
                memo(" med impact factor: " + nimpact);
                foreach (int nnn in njnorskdict.Keys)
                    memo(" with level " + nnn + " in Norska listan: " + njnorskdict[nnn]);

                memo("#Förlag = " + publisherdict.Count);
                memo("#Böcker/bokkapitel = " + nbpub);
                foreach (int nnn in nbnorskdict.Keys)
                    memo(" with level " + nnn + " in Norska listan: " + nbnorskdict[nnn]);
            }
            //string header = "\tPub totalt\tArtiklar ref\tMed impact\tMed ScopusID\tMed ISI\tNorska 1\tNorska 2\tBöcker/bokkapitel\tNorska 1\tNorska 2";
            string scopusvis = CBfraction.Checked ? "" : (100 * nscopus / njpub).ToString("N1") + "%";
            string isivis = CBfraction.Checked ? "" : (100 * nisi / njpub).ToString("N1") + "%";
            return year + "\t" + npubtotal + "\t" + njpub + "\t" + nimpact + "\t"+nscopus+"\t"+scopusvis+"\t"+nisi + "\t" + isivis + "\t" + njnorskdict[1] + "\t" + njnorskdict[2] + "\t" + nbpub + "\t" + nbnorskdict[1] + "\t" + nbnorskdict[2]+"\t"+n3+"\t"+nsw+"\t"+nsn+"\t"+nwn+"\t"+ns+"\t"+nw+"\t"+nn+"\t"+n0;
        }

        private void read_publisher_norska()
        {
            if (publisherlist.Count > 0)
                return;

            openFileDialog1.Title = "Norska-listan-fil förlag:"; //Norska-listan-221110.... i Invärld
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            using (StreamReader sr = new StreamReader(fn))
            {
                int ngood = 0;
                int n = 0;
                string header = sr.ReadLine();
                char splitchar = header.Contains(';') ? ';' : ',';
                string[] hwords = header.Split(splitchar);
                Dictionary<string, int> hdict = new Dictionary<string, int>();
                for (int i = 0; i < hwords.Length; i++)
                    hdict.Add(hwords[i], i);

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = util.splitcsv(line,splitchar);
                    n++;
                    if (words == null)
                        continue;
                    if (words.Length < 4)
                        continue;
                    string jname = cleanjournaltitle(words[hdict["International title"]]);
                    if (n % 1000 == 0)
                        memo(n + " " + jname);

                    List<string> isbn = string.IsNullOrEmpty(words[hdict["ISBN-Prefix"]]) ? new List<string>() : util.parseISBN(words[hdict["ISBN-Prefix"]]);
                    publisherclass jc = new publisherclass();
                    jc.addname(jname);
                    jc.addname(words[hdict["Original title"]]);
                    jc.ISBNprefix = isbn;

                    if (jc.norska.Count == 0)
                    {
                        string levelstring = "Level ";
                        foreach (string h in hdict.Keys)
                        {
                            if (h.StartsWith(levelstring))
                            {
                                int year = util.tryconvert(h.Replace(levelstring, "").Trim());
                                int? level = util.tryconvert(words[hdict[h]]);
                                if (level < 0)
                                    level = null;
                                jc.norska.Add(year, level);
                            }
                        }
                    }

                    publisherlist.Add(jc);
                }
                memo("publisherlist entries with Norska listan: " + publisherlist.Count);
            }


        }


    

        private void SubjectTabButton_Click(object sender, EventArgs e)
        {
            int startyear = util.tryconvert(TB_startyear.Text);
            if (startyear < 0)
            {
                memo("Invalid startyear " + TB_startyear.Text);
                return;
            }

            int endyear = util.tryconvert(TB_endyear.Text);
            if (endyear < startyear)
            {
                memo("Invalid endyear " + TB_endyear.Text);
                return;
            }

            FormExcel fx = new FormExcel(hd,startyear,endyear);
            fx.Show();
        }

        public static double getdouble(dynamic cell)
        {
            //Type unknown = ((ObjectHandle)cell.Value).Unwrap().GetType();
            //return unknown.ToString();

            double i = 0;

            if (cell == null)
                return 0;

            if (cell.Value.GetType() != i.GetType())
                return 0;

            return cell.Value;

            //try
            //{
            //    i = cell.Value;
            //}
            //catch (Exception e)
            //{
            //    return 0;
            //}

            //return i;
        }

        private List<string> FillCountrylist()
        {
            List<string> countries = new List<string>();
            CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo cult in allCultures)
            {
                RegionInfo reg = new RegionInfo(cult.LCID);
                if (!countries.Contains(reg.EnglishName))
                    countries.Add(reg.EnglishName.Replace("_", " "));
            }
            countries.Add("Angola");
            countries.Add("Anguilla");
            countries.Add("Barbados");
            countries.Add("Benin");
            countries.Add("Brunei Darussalam");
            countries.Add("Burundi");
            countries.Add("Chad");
            countries.Add("Congo");
            countries.Add("Cyprus");
            countries.Add("Cote d'Ivoire");
            countries.Add("Czech Republic");
            countries.Add("Fiji");
            countries.Add("Gambia");
            countries.Add("Ghana");
            countries.Add("Hong Kong");
            countries.Add("Liberia");
            countries.Add("Macau");
            countries.Add("Malawi");
            countries.Add("Mauritania");
            countries.Add("Mauritius");
            countries.Add("Mozambique");
            countries.Add("Namibia");
            countries.Add("New Caledonia");
            countries.Add("Palestine");
            countries.Add("Papua New Guinea");
            countries.Add("Russian Federation");
            countries.Add("Seychelles");
            countries.Add("Solomon Islands");
            countries.Add("South Korea");
            countries.Add("Suriname");
            countries.Add("Syrian Arab Republic");
            countries.Add("Tanzania");
            countries.Add("Togo");
            countries.Add("Tonga");
            countries.Add("Trinidad and Tobago");
            countries.Add("Trinidad Tobago");
            countries.Add("Uganda");
            countries.Add("Viet Nam");
            countries.Add("Zambia");
            countries.Add("Democratic Republic Congo");
            countries.Add("Libyan Arab Jamahiriya");

            //countries.Sort();

            countries.Add("England");
            countries.Add("Peoples R China");
            countries.Add("U Arab Emirates");
            countries.Add("Cote Ivoire");
            countries.Add("Scotland");
            countries.Add("Bosnia & Herceg");
            countries.Add("Guyana");
            countries.Add("Wales");
            countries.Add("North Ireland");
            countries.Add("Micronesia");
            countries.Add("DEM REP CONGO");
            countries.Add("Papua N Guinea");
            countries.Add("Sudan");
            //countries.Add("");
            //countries.Add("");

            return countries;
        }

        public Dictionary<string, string> FillCountryaliasList()
        {
            Dictionary<string, string> countryalias = new Dictionary<string, string>();
            countryalias.Add("England", "United Kingdom");
            countryalias.Add("Peoples R China", "China");
            countryalias.Add("U Arab Emirates", "United Arab Emirates");
            countryalias.Add("Cote Ivoire", "Cote d'Ivoire");
            countryalias.Add("Scotland", "United Kingdom");
            countryalias.Add("Bosnia & Herceg", "Bosnia and Herzegovina");
            countryalias.Add("Wales", "United Kingdom");
            countryalias.Add("North Ireland", "United Kingdom");
            countryalias.Add("DEM REP CONGO", "Congo");
            countryalias.Add("Democratic Republic Congo", "Congo");
            countryalias.Add("Papua N Guinea", "Papua New Guinea");
            countryalias.Add("Libyan Arab Jamahiriya", "Libya");
            countryalias.Add("Trinidad and Tobago", "Trinidad Tobago");
            countryalias.Add("Viet Nam", "Vietnam");

            return countryalias;

        }

        public Dictionary<string, pubclass> risdict = new Dictionary<string, pubclass>();

        private void read_RIS(string filetype, int focusuni, int round)
        {
            openFileDialog1.Title = filetype;
            openFileDialog1.Filter = "RIS files (*.ris)|*.ris|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            char[] trimchars = new char[] { ' ', '-' };
            string bracketrex = @"\[(.*)\]";

            List<string> countries = FillCountrylist();
            Dictionary<string, string> countryalias = FillCountryaliasList();

            memo("Reading " + openFileDialog1.FileName);

            using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
            {
                hbookclass affhist = new hbookclass("All affiliations");
                hbookclass affmatchhist = new hbookclass("Matched affiliations");
                hbookclass affnomatchhist = new hbookclass("Unmatched affiliations");
                hbookclass countryhist = new hbookclass("Countries");
                hbookclass notcountryhist = new hbookclass("Non-countries in country position");
                hbookclass naffhist = new hbookclass("# affiliations");
                hbookclass ncountryhist = new hbookclass("# countries per article");
                hbookclass chinahist = new hbookclass("Chinese coauthor affiliations");


                int nline = 0;
                int nrecord = 0;
                int nrecord2 = 0;
                int ndoi0 = 0;
                int ndoi1 = 0;
                int ndoi2 = 0;

                while (!sr.EndOfStream)
                {
                    List<string> affs = new List<string>();
                    bool endofrecord = false;

                    List<authorclass> authorsHDa = new List<authorclass>();
                    List<string> namelist = new List<string>();
                    string title = "";
                    string dtype = "";
                    string doi = "";

                    pubclass pc = new pubclass();

                    //int ncountry = 0;
                    do
                    {
                        string line = sr.ReadLine();
                        nline++;
                        endofrecord = String.IsNullOrEmpty(line.Trim());
                        if (!endofrecord)
                        {
                            string newtype = line.Substring(0, 2);
                            if (newtype != "  ")
                                dtype = newtype;
                            if (dtype == "AD" || dtype == "C1")
                            {
                                string aff = line.Substring(2).Trim(trimchars).Trim('.');
                                string nameaff = "";
                                if (dtype == "C1")
                                {
                                    foreach (Match m in Regex.Matches(aff, bracketrex))
                                    {
                                        aff = aff.Replace(m.Value, "");
                                        nameaff = m.Groups[1].Value;
                                    }
                                    foreach (string name in nameaff.Split(';'))
                                    {
                                        namelist.Add(name.Trim());
                                        if (aff.Contains("Dalarna"))
                                        {
                                            authorclass auau = authorclass.findbyname(name.Trim(), caslist);
                                            if (auau != null)
                                                authorsHDa.Add(auau);
                                        }
                                    }
                                }

                                string[] affparts = aff.Split(',');
                                string country = affparts.Last().Trim();
                                if (country.EndsWith("USA"))
                                    country = "United States";
                                if (countries.Contains(country))
                                    countryhist.Add(country);
                                else
                                {
                                    bool found = false;
                                    foreach (string ap in affparts)
                                    {
                                        if (countries.Contains(ap))
                                        {
                                            country = ap;
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                    {
                                        notcountryhist.Add(line);
                                        country = "(unknown country)";
                                    }
                                    else
                                        countryhist.Add(country);
                                }
                                if (countryalias.ContainsKey(country))
                                    country = countryalias[country];
                                //if (country == "China")
                                //{
                                //    //memo(aff);
                                //    bool founduni = false;
                                //    string[] tofindlist = new string[] { "niversity", "nstitute", "ospital", "cent","cademy","ffice","ivision","aboratory" };
                                //    string mainaff = aff;
                                //    foreach (string tofind in tofindlist)
                                //    {
                                //        foreach (string aa in affparts.Reverse())
                                //        {
                                //            string ap = aa.Trim().Replace("Univ ", "University ").Replace("&"," and ").Replace("  "," ").ToLower();
                                //            if (ap.EndsWith("Univ"))
                                //                ap += "ersity";
                                //            if (ap.Contains(tofind))
                                //            {
                                //                founduni = true;
                                //                mainaff = ap;
                                //            }
                                //            if (ap.Contains("army"))
                                //                memo(title);
                                //        }
                                //        if (founduni)
                                //            break;
                                //    }
                                //    chinahist.Add(mainaff);
                                //}
                                //if (!country.Contains("Sweden"))
                                //    continue;
                                bool matchfound = false;
                                int afftype = affclass.unknown;
                                string affname = "";
                                foreach (string aa in affparts)
                                {
                                    string a = aa.Trim().Replace("Univ ", "University ");
                                    if (a.EndsWith("Univ"))
                                        a += "ersity";
                                    affhist.Add(a);
                                    affclass ac = affclass.getaff(a);
                                    if (ac != null)
                                    {
                                        affmatchhist.Add(ac.name);
                                        matchfound = true;
                                        affname = ac.name;
                                        if (ac.id == focusuni)
                                            afftype = -1; //don't make links to our own uni
                                        else
                                            afftype = ac.orgtype;
                                    }
                                }
                                string affstring = country;
                                if (country.Contains("Sweden"))
                                {
                                    if (afftype == affclass.sweuni)
                                    {
                                        affstring = affname;
                                    }
                                    else
                                    {
                                        affstring = "Sweden non-uni";
                                    }

                                    if (!matchfound)
                                    {
                                        foreach (string aa in affparts)
                                        {
                                            string a = aa.Trim().Replace("Univ ", "University ");
                                            if (a.EndsWith("Univ"))
                                                a += "ersity";
                                            affclass ac = affclass.getaff(a);
                                            if (ac == null)
                                            {
                                                affnomatchhist.Add(a);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    afftype = 0;
                                    //ncountry++;
                                }
                                if (afftype != -1)
                                {
                                    if (pc.affgroupdict.ContainsKey(affstring))
                                        pc.affgroupdict[affstring]++;
                                    else
                                    {
                                        pc.affgroupdict.Add(affstring, 1);
                                        pc.affgrouptypedict.Add(affstring, afftype);
                                    }
                                }


                            }
                            //else if (dtype == "AU" || dtype == "AF")
                            //{
                            //    string name = line.Substring(2).Trim(trimchars);
                            //    namelist.Add(name);
                            //    //if (!name.EndsWith("."))
                            //    //    name += ".";
                            //    authorclass ac = authorclass.findbyname(name, caslist);
                            //    if (ac != null && !authorsHDa.Contains(ac))
                            //        authorsHDa.Add(ac);
                            //    //var q = from c in caslist where c.matchinitials(name) select c;
                            //    //if (q.Count() > 1)
                            //    //{
                            //    //    memo("Author ambiguous: " + name + " " + q.Count());
                            //    //    authorsHDa.Add(new authorclass()); //dummy author without institution
                            //    //}
                            //    //else if (q.Count() == 1)
                            //    //{
                            //    //    authorsHDa.Add(q.First());
                            //    //}
                            //}
                            else if (dtype == "TI")
                                title += " " + line.Substring(2).Trim(trimchars);
                            else if (dtype == "ER")
                                nrecord2++;
                            else if (dtype == "DO" || dtype == "DI")
                            {
                                doi = line.Substring(2).Trim(trimchars);
                            }
                        }
                    }
                    while (!sr.EndOfStream && !endofrecord);
                    nrecord++;

                    title = title.Trim();

                    if (authorsHDa.Count == 0)
                    {
                        if (!String.IsNullOrEmpty(doi))
                        {
                            var q = from c in publist where c.has(pubclass.doistring) && c.dict[pubclass.doistring] == doi select c;
                            if (q.Count() == 1)
                            {
                                pc.merge(q.First());
                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder("");
                                foreach (string name in namelist)
                                    sb.Append("; " + name);
                                memo("DOI not found in Diva \tDOI" + doi + "\t" + sb.ToString());
                                continue;
                            }
                        }
                        //foreach (string name in namelist)
                        //{
                        //    //match last name only
                        //    var ql = from c in caslist where c.name.StartsWith(name.Split(',')[0]) select c;
                        //    if (ql.Count() == 1)
                        //    {
                        //        authorsHDa.Add(ql.First());
                        //    }
                        //}
                        else
                        {
                            StringBuilder sb = new StringBuilder("");
                            foreach (string name in namelist)
                                sb.Append("; " + name);
                            memo("No HDa author \tDOI" + doi + "\t" + sb.ToString());
                            continue;
                        }
                    }
                    //pc.authors = authorsHDa;
                    pc.dict.Add(pubclass.titstring, title);
                    pc.cleantitle();

                    if (!String.IsNullOrEmpty(doi))
                    {
                        if (!risdict.ContainsKey(doi))
                        {
                            risdict.Add(doi, pc);
                            ndoi1++;
                        }
                        else
                        {
                            if (round < 2)
                                memo("duplicate doi " + doi + "\t" + title);
                            else
                                risdict[doi].merge(pc);
                            ndoi2++;
                        }
                    }
                    else
                    {
                        memo("missing doi\t" + title);
                        var q = from c in risdict where c.Value.lowtitle == title.ToLower() select c;
                        if (q.Count() == 1)
                        {
                            if (round < 2)
                                memo("duplicate title " + doi + "\t" + title);
                            else
                                risdict[q.First().Key].merge(pc);
                            ndoi2++;
                        }
                        else
                        {
                            ndoi0++;
                            doi = "nodoi" + round + "-" + ndoi0;
                            risdict.Add(doi, pc);
                        }
                    }


                    naffhist.Add(pc.affgroupdict.Count);
                    int ncountry = (from c in pc.affgroupdict.Keys where countries.Contains(c) select c).Count() + 1; //Add 1 for Sweden
                    ncountryhist.Add(ncountry);
                    if (ncountry > 95)
                        memo("ncountry = " + ncountry + " for " + title);


                    //authorsHDa.Clear();
                    //affgroupdict.Clear();
                    //affgrouptypedict.Clear();
                }

                //memo(affnomatchhist.GetSHist());
                //memo(affmatchhist.GetSHist());
                //memo(countryhist.GetSHist());
                //memo(notcountryhist.GetSHist());
                //memo(naffhist.GetIHist());
                //memo(ncountryhist.GetIHist());

                memo("nline = " + nline);
                memo("nrecord = " + nrecord);
                memo("nrecord2 = " + nrecord2);
                memo("ndoi0 = " + ndoi0);
                memo("ndoi1 = " + ndoi1);
                memo("ndoi2 = " + ndoi2);
                memo("===========================");

                memo(chinahist.GetSHist());

            }


        }
        private void RISbutton_Click(object sender, EventArgs e)
        {
            int focusuni = 2736; //HDa
            if (affclass.afflist.Count == 0)
            {
                string fnaff = @"D:\Downloads\Swepub\afflist.txt";
                affclass.readaffs(fnaff);

                string fnuni = @"D:\Downloads\Swepub\lärosäten.txt";
                affclass.readuni(fnuni);

                string fnuni2 = @"D:\Downloads\Swepub\lärosäten-latlong.txt";
                affclass.readuni_latlong(fnuni2);
            }

            if (caslist.Count == 0)
            {
                ReadCAS();
            }

            read_RIS("Scopus-fil:", focusuni, 1);

            //read_RIS("WoS-fil:", focusuni, 2);

            memo("Total unique DOIs: " + risdict.Count);

            Dictionary<string, int> nodedict = new Dictionary<string, int>();
            Dictionary<string, int> nodeiddict = new Dictionary<string, int>();
            int nodeid = 1;

            Dictionary<string, Dictionary<string, double>> linkdict = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> crosslinkdict = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();

            Dictionary<string, Dictionary<string, List<string>>> affpubdict = new Dictionary<string, Dictionary<string, List<string>>>();


            if (CB_gephi.Checked || CB_csvR.Checked)
            {

                foreach (string doi in risdict.Keys)
                {

                    double affsum = 0;
                    foreach (string s in risdict[doi].affgroupdict.Keys)
                        affsum += risdict[doi].affgroupdict[s];

                    double weight = 1;
                    if (affsum * risdict[doi].authors.Count > 0)
                        weight = 1 / (affsum * risdict[doi].authors.Count);

                    //if (affgroupdict.Count > 40)
                    //{
                    //    memo(authorsHDa.First().getinstitution() + " " + affgroupdict.Count+" "+title);
                    //}

                    foreach (authorclass auc in risdict[doi].authors)
                    {
                        string inst = auc.getinstitution(false).ToLower();
                        if (!nodedict.ContainsKey(inst))
                        {
                            nodedict.Add(inst, -1);
                            nodeiddict.Add(inst, nodeid);
                            nodeid++;
                        }
                        if (!linkdict.ContainsKey(inst))
                            linkdict.Add(inst, new Dictionary<string, double>());
                        if (!affpubdict.ContainsKey(inst))
                            affpubdict.Add(inst, new Dictionary<string, List<string>>());
                        if (!crosslinkdict.ContainsKey(inst))
                            crosslinkdict.Add(inst, new Dictionary<string, Dictionary<string, double>>());
                        foreach (string s in risdict[doi].affgroupdict.Keys)
                        {
                            if (!nodedict.ContainsKey(s))
                            {
                                nodedict.Add(s, risdict[doi].affgrouptypedict[s]);
                                nodeiddict.Add(s, nodeid);
                                nodeid++;
                            }
                            if (!linkdict[inst].ContainsKey(s))
                                linkdict[inst].Add(s, weight * risdict[doi].affgroupdict[s]);
                            else
                                linkdict[inst][s] += weight * risdict[doi].affgroupdict[s];
                            if (!crosslinkdict[inst].ContainsKey(s))
                            {
                                crosslinkdict[inst].Add(s, new Dictionary<string, double>());
                            }
                            if (!affpubdict[inst].ContainsKey(s))
                            {
                                affpubdict[inst].Add(s, new List<string>());
                            }
                            affpubdict[inst][s].Add(auc.name+": "+risdict[doi].dict[pubclass.titstring]);
                            if (risdict[doi].affgroupdict.Count < 20) //skip large counts, connects everything with everything
                            {
                                foreach (string s2 in risdict[doi].affgroupdict.Keys)
                                {
                                    if (s2 == s)
                                        continue;
                                    if (s2.CompareTo(s) < 0)
                                        continue;
                                    if (!crosslinkdict[inst][s].ContainsKey(s2))
                                        crosslinkdict[inst][s].Add(s2, weight * risdict[doi].affgroupdict[s]);
                                    else
                                        crosslinkdict[inst][s][s2] += weight * risdict[doi].affgroupdict[s];
                                }
                            }
                        }
                    }
                }

                if (CB_gephi.Checked)
                {
                    //Make Gephi files:

                    WriteGephi(nodedict, nodeiddict, linkdict, crosslinkdict, "");
                    foreach (string inst in linkdict.Keys)
                        WriteGephi(nodedict, nodeiddict, linkdict, crosslinkdict, inst);
                }

                if (CB_csvR.Checked)
                {
                    //Make CSV files:

                    WriteCSV(nodedict, nodeiddict, linkdict, crosslinkdict, "");
                    foreach (string inst in linkdict.Keys)
                        WriteCSV(nodedict, nodeiddict, linkdict, crosslinkdict, inst);

                }
            }
            memo("Done");

        }

        private void WriteGephi(Dictionary<string, int> nodedict, Dictionary<string, int> nodeiddict, Dictionary<string, Dictionary<string, double>> linkdict, Dictionary<string, Dictionary<string, Dictionary<string, double>>> crosslinkdict, string institution)
        {
            string selectstring = "W+S-" + institution;
            string folder = util.timestampfolder(@"C:\Temp\Gephi\", "Gephi per institution");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            //string folder = openFileDialog1.InitialDirectory;
            string fn = util.unusedfn(folder + selectstring + @"nodes.csv");
            Dictionary<int, KnownColor> colordict = new Dictionary<int, KnownColor>();
            KnownColor[] knowncol = util.getallcolors();
            int nextcolor = 0;

            Dictionary<string, bool> nodeused = new Dictionary<string, bool>();
            if (String.IsNullOrEmpty(institution))
            {
                foreach (string s in nodedict.Keys)
                    nodeused[s] = true;
            }
            else
            {
                foreach (string s in nodedict.Keys)
                    nodeused[s] = false;
                nodeused[institution] = true;
                foreach (string s in linkdict[institution].Keys)
                    nodeused[s] = true;
            }


            memo("Writing nodes to " + fn);
            using (StreamWriter sw = new StreamWriter(fn))
            {
                sw.WriteLine("Id;Label;Color");
                foreach (string nodename in nodedict.Keys)
                {
                    if (!nodeused[nodename])
                        continue;
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
                if (String.IsNullOrEmpty(institution))
                {
                    foreach (string inst in linkdict.Keys)
                    {
                        foreach (string s in linkdict[inst].Keys)
                        {
                            sw.WriteLine(nodeiddict[inst] + ";" + nodeiddict[s] + ";" + linkdict[inst][s].ToString(CultureInfo.InvariantCulture));
                        }
                        foreach (string s in crosslinkdict[inst].Keys)
                            foreach (string s2 in crosslinkdict[inst][s].Keys)
                            {
                                sw.WriteLine(nodeiddict[s2] + ";" + nodeiddict[s] + ";" + crosslinkdict[inst][s][s2].ToString(CultureInfo.InvariantCulture));
                            }
                    }
                }
                else
                {
                    foreach (string s in linkdict[institution].Keys)
                    {
                        sw.WriteLine(nodeiddict[institution] + ";" + nodeiddict[s] + ";" + linkdict[institution][s].ToString(CultureInfo.InvariantCulture));
                    }
                    foreach (string s in crosslinkdict[institution].Keys)
                        foreach (string s2 in crosslinkdict[institution][s].Keys)
                        {
                            sw.WriteLine(nodeiddict[s2] + ";" + nodeiddict[s] + ";" + crosslinkdict[institution][s][s2].ToString(CultureInfo.InvariantCulture));
                        }
                }

            }

        }

        private void WriteCSV(Dictionary<string, int> nodedict, Dictionary<string, int> nodeiddict, Dictionary<string, Dictionary<string, double>> linkdict, Dictionary<string, Dictionary<string, Dictionary<string, double>>> crosslinkdict, string institution)
        {
            Dictionary<string, string> csvalias = new Dictionary<string, string>()
            {
                {"United States", "USA" },
                {"United Kingdom","UK" },
                {"South Korea", "Korea South" },
                {"North Korea", "Korea North" },
                {"Russian Federation","Russia" },
            };
            string selectstring = "W+S-" + institution;
            string folder = util.timestampfolder(@"C:\Temp\", "R per institution");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string fn = util.unusedfn(folder + selectstring + @"-R.csv");
            string fnswe = util.unusedfn(folder + selectstring + @"-sweuni-R.csv");

            Dictionary<string, bool> nodeused = new Dictionary<string, bool>();
            if (String.IsNullOrEmpty(institution))
            {
                foreach (string s in nodedict.Keys)
                    nodeused[s] = true;
            }
            else
            {
                foreach (string s in nodedict.Keys)
                    nodeused[s] = false;
                nodeused[institution] = true;
                foreach (string s in linkdict[institution].Keys)
                    nodeused[s] = true;
            }

            Dictionary<string, double> nodeweightdict = new Dictionary<string, double>();

            if (String.IsNullOrEmpty(institution))
            {
                foreach (string inst in linkdict.Keys)
                {
                    foreach (string s in linkdict[inst].Keys)
                    {
                        if (!nodeweightdict.ContainsKey(s))
                            nodeweightdict.Add(s, 0);
                        nodeweightdict[s] += linkdict[inst][s];
                    }
                    foreach (string s in crosslinkdict[inst].Keys)
                        foreach (string s2 in crosslinkdict[inst][s].Keys)
                        {
                            if (!nodeweightdict.ContainsKey(s))
                                nodeweightdict.Add(s, 0);
                            nodeweightdict[s] += crosslinkdict[inst][s][s2];
                        }
                }
            }
            else
            {
                foreach (string s in linkdict[institution].Keys)
                {
                    if (!nodeweightdict.ContainsKey(s))
                        nodeweightdict.Add(s, 0);
                    nodeweightdict[s] += linkdict[institution][s];
                }
                foreach (string s in crosslinkdict[institution].Keys)
                    foreach (string s2 in crosslinkdict[institution][s].Keys)
                    {
                        if (!nodeweightdict.ContainsKey(s))
                            nodeweightdict.Add(s, 0);
                        nodeweightdict[s] += crosslinkdict[institution][s][s2];
                    }
            }

            memo("Writing nodes to " + fn);
            using (StreamWriter sw = new StreamWriter(fn))
            using (StreamWriter swswe = new StreamWriter(fnswe))
            {
                sw.WriteLine("\"\",\"country.etc\",\"type\",\"weight\"");
                swswe.WriteLine("\"\",\"name\",\"type\",\"weight\",\"lat\",\"long\"");
                foreach (string nodename in nodedict.Keys)
                {
                    if (!nodeused[nodename])
                        continue;
                    if (!nodeweightdict.ContainsKey(nodename))
                        continue;
                    sw.WriteLine(nodeiddict[nodename] + ",\"" + (csvalias.ContainsKey(nodename) ? csvalias[nodename] : nodename) + "\"," + nodedict[nodename] + "," + nodeweightdict[nodename].ToString(CultureInfo.InvariantCulture));   //";#0000FF");
                    if (nodedict[nodename] == affclass.sweuni)
                    {
                        affclass ac = affclass.getaff(nodename);
                        if (ac != null)
                        {
                            string label = affclass.shortnamedict[ac.id];
                            swswe.WriteLine(nodeiddict[nodename] + ",\"" + label + "\"," + nodedict[nodename] + "," + nodeweightdict[nodename].ToString(CultureInfo.InvariantCulture) + "," + affclass.unilatlong[ac.id].Item1.ToString(CultureInfo.InvariantCulture) + "," + affclass.unilatlong[ac.id].Item2.ToString(CultureInfo.InvariantCulture));   //";#0000FF");

                        }
                    }

                }
            }

        }

        private Dictionary<string, string> readPrimula()
        {
            Dictionary<string, string> pdict = new Dictionary<string, string>();
            openFileDialog1.Title = "Primula-fil:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return pdict;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine();
                sr.ReadLine();

                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length > 4)
                        if (!pdict.ContainsKey(words[4]))
                            pdict.Add(words[4], words[0]);
                }
            }
            return pdict;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (string inst in authorclass.institutions)
            {
                int n = (from c in caslist where c.getinstitution(false) == inst select c).Count();
                memo(inst + "\t" + n);
            }

            //hbookclass th = new hbookclass("Test");
            //th.SetBins(new List<double>() { 0, 1, 2, 3 });

            //th.Add(0.0);
            //th.Add(2.0);
            //memo(th.GetDHist());
            //if (affclass.afflist.Count == 0)
            //{
            //    string fnaff = @"D:\Downloads\Swepub\afflist.txt";
            //    affclass.readaffs(fnaff);
            //}

            //memo(affclass.list_sweuni());

            //var primuladict = readPrimula();
            //if (caslist.Count == 0)
            //    ReadCAS();

            //int n = 0;
            //foreach (string pname in primuladict.Keys)
            //{
            //    n++;
            //    var q = from c in caslist where c.name.Replace(",", "") == pname select c;
            //    if (q.Count() == 1)
            //    {
            //        string[] instparts = primuladict[pname].Split();
            //        if (instparts.Length < 3 || !q.First().getinstitution().StartsWith(instparts[2]))
            //        memo(pname + "\t" + primuladict[pname] + "\t" + q.First().getinstitution());
            //    }
            //    else
            //    {
            //        if (primuladict[pname].StartsWith("inst")) //skippa stödet
            //        {
            //            if (q.Count() == 0)
            //            {
            //                string candidates = "";
            //                foreach (string ppart in pname.Split())
            //                {
            //                    var q2 = from c in caslist where c.name.Contains(ppart) select c;
            //                    foreach (authorclass ac in q2)
            //                        candidates += "; " + ac.name;
            //                }
            //                memo(pname + "\t" + primuladict[pname] + "\t***not found in caslist*** "+candidates);
            //            }
            //            else
            //                memo(pname + "\t" + primuladict[pname] + "\t***found " + q.Count() + " in caslist***");
            //        }
            //    }
            //}
        }

        private void Wordcloudbutton_Click(object sender, EventArgs e)
        {
            string dir = TB_worddir.Text;
            if (!dir.EndsWith(@"\"))
                dir += @"\";

            //DateTime now = DateTime.Now;
            //dir += now.Year + "-" + now.Month + "-" + now.Day + " " + now.Hour + "-" + now.Minute + @"\";
            dir = util.timestampfolder(dir, "Wordclouds ");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            FormWordcloud fwc = new FormWordcloud(publist,"DIVA wordcloud "+divadate,dir);
            fwc.Show();
        }

        private void SearchStringbutton_Click(object sender, EventArgs e)
        {
            StringBuilder wos = new StringBuilder("UT=(");
            StringBuilder scopus = new StringBuilder("DOI");
            bool firstwos = true;
            bool firstscopus = true;

            foreach (pubclass pc in publist)
            {
                if (pc.dict.ContainsKey(pubclass.isistring))
                {
                    if (!firstwos)
                        wos.Append(" OR ");
                    else
                        firstwos = false;
                    wos.Append(pc.dict[pubclass.isistring]);
                }
                if (pc.dict.ContainsKey(pubclass.doistring))
                {
                    if (!firstscopus)
                        scopus.Append(" OR ");
                    else
                        firstscopus = false;
                    scopus.Append("("+pc.dict[pubclass.doistring]+")");
                }
            }

            wos.Append(")");
            memo(wos.ToString());
            memo("");
            memo(scopus.ToString());

        }

        private void Authorlistbutton_Click(object sender, EventArgs e)
        {
            Dictionary<string, SortedDictionary<string, SortedDictionary<string, int>>> audict = new Dictionary<string, SortedDictionary<string, SortedDictionary<string, int>>>();

            authorclass.fill_instsubj(caslist);

            foreach (string inst in authorclass.institutions)
            {
                audict.Add(inst, new SortedDictionary<string, SortedDictionary<string, int>>());
                audict[inst].Add(authorclass.nosubject, new SortedDictionary<string, int>());
            }

            foreach (authorclass ac in caslist)
            {
                string inst = ac.getinstitution(false);
                string subj = ac.getsubjectalias();
                if (!audict.ContainsKey(inst))
                    audict.Add(inst, new SortedDictionary<string, SortedDictionary<string, int>>());
                if (!audict[inst].ContainsKey(subj))
                {
                    if (authorclass.instsubj[inst].Contains(subj))
                        audict[inst].Add(subj, new SortedDictionary<string, int>());
                }
            }

            foreach (pubclass pc in publist)
            {
                foreach (authorclass ac in pc.authors)
                {
                    if (String.IsNullOrEmpty(ac.CAS))
                        continue;
                    else if (ac.CAS == "mte")
                    {
                        int dummy = 0; //Tegmark
                    }
                    string inst = ac.getinstitution(false);
                    string subj = ac.getsubjectalias();
                    if (!audict[inst].ContainsKey(subj))
                        subj = authorclass.nosubject;
                    if (!audict[inst][subj].ContainsKey(ac.name))
                        audict[inst][subj].Add(ac.name, 0);
                    audict[inst][subj][ac.name]++;
                }
            }

            foreach (string inst in audict.Keys)
            {
                memo(inst);
                foreach (string subj in audict[inst].Keys)
                {
                    if (audict[inst][subj].Count > 0)
                    {
                        memo("\t" + subj);
                        foreach (string au in audict[inst][subj].Keys)
                            memo("\t\t" + au + "\t" + audict[inst][subj][au]);
                    }
                }
            }
        }

        private void KTHbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "KTH-fil:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            int nline = 0;
            using (StreamReader sr = new StreamReader(fn))
            {
                string header = sr.ReadLine();
                string[] hwords = header.Split('\t');
                int iut = -1;
                for (int i = 0; i < hwords.Length; i++)
                    if (hwords[i] == "UT")
                        iut = i;

                var qisi = from c in publist where c.has(pubclass.isistring) select c;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    nline++;
                    string[] words = line.Split('\t');
                    string ut = words[iut];
                    var q = from c in qisi where c.dict[pubclass.isistring] == ut select c;
                    if (q.Count() == 0)
                        memo("UT not found for " + line);
                    else if (q.Count() > 1)
                        memo(q.Count() + " hits for " + line);
                    else
                    {
                        pubclass pc = q.First();
                        for (int i = 0; i < words.Length; i++)
                        {
                            pc.dict.Add("KTH_" + hwords[i], words[i]);
                        }
                    }
                }
            }

            memo(nline + " lines read.");


        }

        private void Primulabutton_Click(object sender, EventArgs e)
        {
            ReadPrimula();
        }

        private void ReadPrimula()
        {

            List<string> primulainst = new List<string>();
            List<string> primulasubj = new List<string>();
            List<string> primuladept = new List<string>();

            Dictionary<string, string> primulainstkey = new Dictionary<string, string>(){
{"institutionen för inform","information och teknik"},
{"institutionen för kultur","kultur och samhälle"},
{"institutionen för läraru","lärarutbildning"},
{"avdelningen för stöd i u","verksamhetsstödet"},
{"institutionen för språk,","språk, litteratur och lärande"},
{"institutionen för hälsa","hälsa och välfärd"},
{"avdelningen för stöd til","verksamhetsstödet"},
{"avdelningen för ekonomi","verksamhetsstödet"},
{"biblioteket","verksamhetsstödet"},
{"avdelningen för students","verksamhetsstödet"},
{"rektorsfunktion","verksamhetsstödet"},
{"avdelningen för fastighe","verksamhetsstödet"},
{"avdelningen för it och d","verksamhetsstödet"},
{"avdelningen för hr","verksamhetsstödet"},
{"verksamhetsstödet","verksamhetsstödet"},
{"avdelningen för kommunik","verksamhetsstödet"},
{"(ingen institution)","(ingen institution)"}
};


            openFileDialog1.Title = "Primula-fil:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            int nline = 0;
            int nmatch = 0;
            int nmissing = 0;
            int nmulti = 0;
            int nmismatchinst = 0;
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine();
                string header = sr.ReadLine();
                string[] hwords = header.Split('\t');
                int icas = -1;
                for (int i = 0; i < hwords.Length; i++)
                    if (hwords[i] == "CAS-id")
                        icas = i;
                int iinst = 0;
                int isubj = 10;
                int idept = 1;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    nline++;
                    string[] words = line.Split('\t');
                    string cas = words[icas];
                    string inst = words[iinst].ToLower();
                    if (!String.IsNullOrEmpty(inst))
                    {
                        if (!primulainst.Contains(inst))
                            primulainst.Add(inst);
                    }
                    if (!String.IsNullOrEmpty(words[isubj]))
                    {
                        if (!primulasubj.Contains(words[isubj]))
                            primulasubj.Add(words[isubj]);
                    }
                    if (!String.IsNullOrEmpty(words[idept]))
                    {
                        if (!primuladept.Contains(words[idept]))
                            primuladept.Add(words[idept]);
                    }
                    //if (!inst.StartsWith("institutionen"))
                    //    continue;
                    var q = from c in caslist where c.CAS == cas select c;
                    if (q.Count() == 0)
                    {
                        //memo("CAS not found for " + line);
                        nmissing++;
                    }
                    else if (q.Count() > 1)
                    {
                        memo(q.Count() + " hits for " + line);
                        nmulti++;
                    }
                    else
                    {
                        authorclass ac = q.First();
                        ac.department = words[idept];
                        nmatch++;
                        if (ac.getinstitution(true) != primulainstkey[inst])
                        {
                            memo("Institution mismatch for \t" + ac.name + "\t" + ac.getinstitution(true) + "\t" + primulainstkey[inst]);
                            ac.changeinstitution(primulainstkey[inst]);
                            nmismatchinst++;
                        }
                    }
                }
            }

            memo(nline + " lines read.");

            memo("\n=======Ämnen i Primula:==========");
            foreach (string s in primulasubj)
                memo(s);
            memo("\n=======Avdelningar i Primula:==========");
            foreach (string s in primuladept)
                memo(s);
            memo("\n=======Institutioner i Primula:==========");
            foreach (string s in primulainst)
                memo(s);
            memo("\n=======Institutioner i CAS:==========");
            foreach (string s in authorclass.institutions)
                memo(s);

            memo("\n=======Institutioner i Caslist:==========");
            foreach (string s in authorclass.institutions)
                memo(s);

            memo("");
            memo("nmatch = " + nmatch);
            memo("nmissing = " + nmissing);
            memo("nmulti = " + nmulti);
            memo("nmismatchinst = " + nmismatchinst);
        }

        private void Topauthorbutton_Click(object sender, EventArgs e)
        {
            Dictionary<string, Dictionary<string, int>> instaudict = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> instrefdict = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> instcitedict = new Dictionary<string, Dictionary<string, int>>();
            foreach (string s in authorclass.institutions)
            {
                instaudict.Add(s, new Dictionary<string, int>());
                instrefdict.Add(s, new Dictionary<string, int>());
                instcitedict.Add(s, new Dictionary<string, int>());
            }
            instaudict.Add("HDa", new Dictionary<string, int>());
            instrefdict.Add("HDa", new Dictionary<string, int>());
            instcitedict.Add("HDa", new Dictionary<string, int>());


            foreach (pubclass pc in publist)
            {
                int cites = 0;
                //r6.pubtypes.Add("Artikel i tidskrift");
                //r6.pubtypes.Add("Artikel, forskningsöversikt");
                ////r6.refstatus = 1;
                //r6.contenttypes.Add(pubclass.ctrefstring);
                bool artref = (pc.dict[pubclass.pubtypestring] == pubclass.ptartstring) && (pc.dict[pubclass.contenttypestring] == pubclass.ctrefstring);
                if (pc.has(pubclass.kthC_scxwostring))
                    cites = util.tryconvert(pc.dict[pubclass.kthC_scxwostring]);
                //else if (pc.has(pubclass.scopuscitationstring))
                //    cites = util.tryconvert(pc.dict[pubclass.scopuscitationstring]);
                foreach (authorclass aclocal in pc.authors)
                {
                    if (!aclocal.isHDa())
                        continue;

                    authorclass ac = authorclass.findbyCAS(aclocal.CAS, caslist);
                    if (ac == null)
                        continue;
                    string inst = ac.getinstitution(false);
                    if (!instaudict[inst].ContainsKey(ac.name))
                    {
                        instaudict[inst].Add(ac.name, 1);
                        instrefdict[inst].Add(ac.name, artref ? 1 : 0);
                        instcitedict[inst].Add(ac.name, cites);
                    }
                    else
                    {
                        instaudict[inst][ac.name]++;
                        if (artref)
                            instrefdict[inst][ac.name]++;
                        instcitedict[inst][ac.name] += cites;
                    }
                    inst = "HDa";
                    if (!instaudict[inst].ContainsKey(ac.name))
                    {
                        instaudict[inst].Add(ac.name, 1);
                        instrefdict[inst].Add(ac.name, artref ? 1 : 0);
                        instcitedict[inst].Add(ac.name, cites);
                    }
                    else
                    {
                        instaudict[inst][ac.name]++;
                        if (artref)
                            instrefdict[inst][ac.name]++;
                        instcitedict[inst][ac.name] += cites;
                    }
                }
            }

            int ntop = 10;

            foreach (string inst in instaudict.Keys)
            {
                var toppubdict = util.getTopValues(instaudict[inst], ntop);
                int toppub = toppubdict.Values.Sum();
                int otherpub = instaudict[inst].Values.Sum()-toppub;

                var toprefdict = util.getTopValues(instrefdict[inst], ntop);
                int topref = toprefdict.Values.Sum();
                int otherref = instrefdict[inst].Values.Sum() - topref;


                var topcitedict = util.getTopValues(instcitedict[inst], ntop);
                int topcite = topcitedict.Values.Sum();
                int othercites = instcitedict[inst].Values.Sum()-topcite;

                memo("=== " + inst + " ===");
                int itop = 0;
                foreach (string name in toppubdict.Keys)
                {
                    itop++;
                    memo(name + "\tTopp " + itop+"\t" + toppubdict[name]);
                }
                memo("\tÖvriga\t" + otherpub);
                memo(" ");

                itop = 0;
                foreach (string name in toprefdict.Keys)
                {
                    itop++;
                    memo(name + "\tTopp " + itop + "\t" + toprefdict[name]);
                }
                memo("\tÖvriga\t" + otherref);
                memo(" ");

                itop = 0;
                foreach (string name in topcitedict.Keys)
                {
                    itop++;
                    memo(name + "\tTopp " + itop + "\t" + topcitedict[name]);
                }
                memo("\tÖvriga\t" + othercites);
                memo(" ");

            }

        }

        private void Citebutton_Click(object sender, EventArgs e)
        {
            //Dictionary<string, int> kthsubjectdict = new Dictionary<string, int>();

            hbookclass citehist = new hbookclass("Citations per publication (self excluded)");
            hbookclass kthsubjecthist = new hbookclass("Pubs per subject");
            hbookclass topxhist = new hbookclass("Publikation in Top xx%");

            Dictionary<string, hbookclass> citehistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> citeselfhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> cfhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> cfselfhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> topxhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> subjhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> nsubjhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> jcfhistdict = new Dictionary<string, hbookclass>();
            Dictionary<string, hbookclass> scopushistdict = new Dictionary<string, hbookclass>();
            //Dictionary<string, hbookclass> citemeanhistdict = new Dictionary<string, hbookclass>();
            //Dictionary<string, hbookclass> cfmeanhistdict = new Dictionary<string, hbookclass>();


            Dictionary<string, int> pubcount = new Dictionary<string, int>();
            Dictionary<string, int> pubcountwithKTH = new Dictionary<string, int>();
            Dictionary<string, int> pubcountwithScopus = new Dictionary<string, int>();
            Dictionary<string, int> pubcountwithboth = new Dictionary<string, int>();
            Dictionary<string, int> pubcountwithnone = new Dictionary<string, int>();
            Dictionary<string, Dictionary<int, double>> yearpubcount = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubcountmissing = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubcountzeroKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubcountKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearcitecountKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubcfcountKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearcitecfcountKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubjcfcountKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearcitejcfcountKTH = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubcountzeroScopus = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearpubcountScopus = new Dictionary<string, Dictionary<int, double>>();
            Dictionary<string, Dictionary<int, double>> yearcitecountScopus = new Dictionary<string, Dictionary<int, double>>();

            Dictionary<string, Dictionary<int, List<double>>> yearcitelistKTH = new Dictionary<string, Dictionary<int, List<double>>>();
            Dictionary<string, Dictionary<int, List<double>>> yearcitelistScopus = new Dictionary<string, Dictionary<int, List<double>>>();


            Dictionary<string, int> subjnodeiddict = new Dictionary<string, int>();
            int nsubj = 0;
            Dictionary<string, Dictionary<string, int>> subjnodedict = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<int, Dictionary<int, double>>> subjedgedict = new Dictionary<string, Dictionary<int, Dictionary<int, double>>>();

            List<string> instlist = authorclass.institutions.ToList();
            instlist.Add("HDa");

            List<double> citebins = new List<double>() { 0, 2, 5, 10, 15, 20, 30, 40, 50, 100 };
            List<double> cfbins = new List<double>() { 0, 0.5, 0.8, 1, 1.5, 2, 3, 5, 10, 20 };

            foreach (string inst in instlist)
            {
                pubcount.Add(inst, 0);
                pubcountwithKTH.Add(inst, 0);
                pubcountwithScopus.Add(inst, 0);
                pubcountwithboth.Add(inst, 0);
                pubcountwithnone.Add(inst, 0);

                yearpubcount.Add(inst, new Dictionary<int, double>());
                yearpubcountmissing.Add(inst, new Dictionary<int, double>());
                yearpubcountzeroKTH.Add(inst, new Dictionary<int, double>());
                yearpubcountKTH.Add(inst, new Dictionary<int, double>());
                yearcitecountKTH.Add(inst, new Dictionary<int, double>());
                yearpubcfcountKTH.Add(inst, new Dictionary<int, double>());
                yearcitecfcountKTH.Add(inst, new Dictionary<int, double>());
                yearpubjcfcountKTH.Add(inst, new Dictionary<int, double>());
                yearcitejcfcountKTH.Add(inst, new Dictionary<int, double>());
                yearpubcountzeroScopus.Add(inst, new Dictionary<int, double>());
                yearpubcountScopus.Add(inst, new Dictionary<int, double>());
                yearcitecountScopus.Add(inst, new Dictionary<int, double>());
                yearcitelistKTH.Add(inst, new Dictionary<int, List<double>>());
                yearcitelistScopus.Add(inst, new Dictionary<int, List<double>>());
                for (int i=2018;i<=2022;i++)
                {
                    yearpubcount[inst].Add(i, 0);
                    yearpubcountmissing[inst].Add(i, 0);
                    yearpubcountzeroKTH[inst].Add(i, 0);
                    yearpubcountKTH[inst].Add(i, 0);
                    yearcitecountKTH[inst].Add(i, 0);
                    yearpubcfcountKTH[inst].Add(i, 0);
                    yearcitecfcountKTH[inst].Add(i, 0);
                    yearpubjcfcountKTH[inst].Add(i, 0);
                    yearcitejcfcountKTH[inst].Add(i, 0);
                    yearpubcountzeroScopus[inst].Add(i, 0);
                    yearpubcountScopus[inst].Add(i, 0);
                    yearcitecountScopus[inst].Add(i, 0);
                    yearcitelistKTH[inst].Add(i, new List<double>());
                    yearcitelistScopus[inst].Add(i, new List<double>());
                }



                citehistdict.Add(inst, new hbookclass(inst + ": citeringar per publikation (WoS, utan självcitering)"));
                citeselfhistdict.Add(inst, new hbookclass(inst + ": citeringar per publikation (WoS, med självcitering)"));
                cfhistdict.Add(inst, new hbookclass(inst + ": citeringar (fältnormaliserat) per publikation, WoS"));
                cfselfhistdict.Add(inst, new hbookclass(inst + ": citeringar (fältnormaliserat, med självcitering) per publikation, WoS"));
                citehistdict[inst].SetBins(citebins);
                cfhistdict[inst].SetBins(cfbins);
                citeselfhistdict[inst].SetBins(citebins);
                cfselfhistdict[inst].SetBins(cfbins);
                //citehistdict[inst].SetBins(0, 50, 25);
                //cfhistdict[inst].SetBins(0, 5, 25);
                //citeselfhistdict[inst].SetBins(0, 50, 25);
                //cfselfhistdict[inst].SetBins(0, 5, 25);
                subjhistdict.Add(inst, new hbookclass(inst + ": forskningsfält"));
                nsubjhistdict.Add(inst, new hbookclass(inst + ": antal forskningsfält per publikation"));
                topxhistdict.Add(inst, new hbookclass(inst + ": publikationer i topp-X mest citerade"));
                jcfhistdict.Add(inst, new hbookclass(inst + ": fältnormaliserad citeringsnivå för tidskriften (JCF)"));
                jcfhistdict[inst].SetBins(cfbins);
                scopushistdict.Add(inst, new hbookclass(inst + ": citeringar per publikation (Scopus 2018-2022)"));
                scopushistdict[inst].SetBins(citebins);

                subjnodedict.Add(inst, new Dictionary<string, int>());
                subjedgedict.Add(inst, new Dictionary<int, Dictionary<int, double>>());
            }


            foreach (pubclass pc in publist)
            {
                if (pc.firstHDa == null)
                    continue;
                if (!(!pc.dict.ContainsKey(pubclass.statusstring) || (pc.dict[pubclass.statusstring] != "submitted" && pc.dict[pubclass.statusstring] != "accepted")))
                    continue;

                List<string> pubinstlist = pc.get_authorinstitution(false,true);
                pubinstlist.Add("HDa");
                foreach (string inst in pubinstlist)
                {
                    pubcount[inst]++;
                    yearpubcount[inst][pc.year]++;
                }
                if (pc.has(pubclass.kthpidstring))
                {

                    string[] subjects = pc.dict[pubclass.kthsubjectstring].Trim('"').Split(';');
                    foreach (string sub in subjects)
                    {
                        kthsubjecthist.Add(sub.Trim('"').Trim());
                    }
                    int cites = util.tryconvert(pc.dict[pubclass.kthC_scxwostring]);
                    double cf = util.tryconvertdouble(pc.dict[pubclass.kthCf_scxwostring]);
                    int citeself = util.tryconvert(pc.dict[pubclass.kthC_sciwostring]);
                    double cfself = util.tryconvertdouble(pc.dict[pubclass.kthCf_sciwostring]);
                    citehist.Add(cites);

                    if (!pc.dict.ContainsKey(pubclass.kthcitationstring))
                    {
                        pc.dict.Add(pubclass.kthcitationstring, pc.dict[pubclass.kthC_scxwostring]);
                        pc.dict.Add(pubclass.kthfieldnormstring, pc.dict[pubclass.kthCf_scxwostring]);
                    }

                    //if (cf > 50)
                    //    memo(pc.dict[pubclass.kthfieldnormstring]);
                    string topx = "Inte i topp";
                    foreach (int tx in pubclass.kthtopdict.Keys)
                    {
                        if (!pc.has(pubclass.kthtopdict[tx]))
                        {
                            topx = "Saknas";
                            break;
                        }
                        if (util.tryconvertdouble(pc.dict[pubclass.kthtopdict[tx]]) > 0)
                        {
                            topx = "Topp " + tx.ToString().PadLeft(2) + " %";
                            break;
                        }
                    }
                    topxhist.Add(topx);
                    if (topx != "Saknas")
                        pc.dict.Add(pubclass.kthtopxstring, topx);

                    foreach (string inst in pubinstlist)
                    {
                        citehistdict[inst].Add((double)cites);
                        citeselfhistdict[inst].Add((double)citeself);
                        yearpubcountKTH[inst][pc.year]++;
                        if (cites == 0)
                            yearpubcountzeroKTH[inst][pc.year]++;
                        yearcitecountKTH[inst][pc.year]+= cites;
                        yearcitelistKTH[inst][pc.year].Add(cites);
                        if (pc.has(pubclass.kthCf_scxwostring))
                        {
                            cfhistdict[inst].Add(cf);
                            cfselfhistdict[inst].Add(cfself);
                            yearpubcfcountKTH[inst][pc.year]++;
                            yearcitecfcountKTH[inst][pc.year] += cf;
                        }
                        topxhistdict[inst].Add(topx);
                        foreach (string subraw in subjects)
                        {
                            string sub = subraw.Trim('"').Trim();
                            subjhistdict[inst].Add(sub);

                            if (!subjnodeiddict.ContainsKey(sub))
                            {
                                nsubj++;
                                subjnodeiddict.Add(sub, nsubj);
                            }
                            int isub = subjnodeiddict[sub];
                            if (!subjnodedict[inst].ContainsKey(sub))
                                subjnodedict[inst].Add(sub, 1);
                            else
                                subjnodedict[inst][sub]++;
                            if (subjects.Length > 1)
                            {
                                foreach (string sub2raw in subjects)
                                {
                                    string sub2 = sub2raw.Trim('"').Trim();
                                    if (sub2 == sub)
                                        continue;
                                    if (!subjnodeiddict.ContainsKey(sub2))
                                    {
                                        nsubj++;
                                        subjnodeiddict.Add(sub2, nsubj);
                                    }
                                    int isub2 = subjnodeiddict[sub2];
                                    if (!subjedgedict[inst].ContainsKey(isub))
                                        subjedgedict[inst].Add(isub, new Dictionary<int, double>());
                                    if (!subjedgedict[inst][isub].ContainsKey(isub2))
                                        subjedgedict[inst][isub].Add(isub2, 1);
                                    else
                                        subjedgedict[inst][isub][isub2]++;
                                }
                            }
                        }
                        nsubjhistdict[inst].Add(subjects.Length);
                    }

                    if (pc.has(pubclass.kthjcfstring))
                    {
                        foreach (string inst in pubinstlist)
                        {
                            jcfhistdict[inst].Add(util.tryconvertdouble(pc.dict[pubclass.kthjcfstring]));
                        }
                        if (!pc.dict.ContainsKey(pubclass.kthjcf2string))
                            pc.dict.Add(pubclass.kthjcf2string, pc.dict[pubclass.kthjcfstring]);
                        foreach (string inst in pubinstlist)
                        {
                            yearpubjcfcountKTH[inst][pc.year]++;
                            yearcitejcfcountKTH[inst][pc.year]+= util.tryconvertdouble(pc.dict[pubclass.kthjcfstring]);
                        }
                    }
                }

                if (pc.has(pubclass.scopuscitationstring))
                {
                    int nscopus = util.tryconvert(pc.dict[pubclass.scopuscitationstring]);
                    foreach (string inst in pubinstlist)
                    {
                        scopushistdict[inst].Add(util.tryconvertdouble(pc.dict[pubclass.scopuscitationstring]));
                        if (pc.has(pubclass.kthpidstring))
                            pubcountwithboth[inst]++;
                        else
                            pubcountwithScopus[inst]++;
                        //yearpubcountzeroScopus[inst].Add(i, 0);
                        //yearpubcountScopus[inst].Add(i, 0);
                        //yearcitecountScopus[inst].Add(i, 0);
                        yearpubcountScopus[inst][pc.year]++;
                        yearcitecountScopus[inst][pc.year] += nscopus;
                        yearcitelistScopus[inst][pc.year].Add(nscopus);
                        if (nscopus == 0)
                            yearpubcountzeroScopus[inst][pc.year]++;
                    }
                }
                else
                {
                    if (pc.has(pubclass.kthpidstring))
                        foreach (string inst in pubinstlist)
                            pubcountwithKTH[inst]++;
                    else
                        foreach (string inst in pubinstlist)
                        {
                            pubcountwithnone[inst]++;
                            yearpubcountmissing[inst][pc.year]++;
                        }


                }
            }

            memo(citehist.GetIHist());
            memo(kthsubjecthist.GetSHist());
            memo(topxhist.GetSHist());

            foreach (string inst in instlist)
            {
                memo("==========================================");
                memo(inst);
                memo("==========================================");
                memo("Publ med citeringsdata från både WoS och Scopus\t" + pubcountwithboth[inst]);
                memo("Publ med citeringsdata endast från WoS\t" + pubcountwithKTH[inst]);
                memo("Publ med citeringsdata endast från Scopus\t" + pubcountwithScopus[inst]);
                memo("Publ helt utan citeringsdata\t" + pubcountwithnone[inst]);
                memo("Publ totalt\t" + pubcount[inst]);

                memo("================= WoS tidsserier citeringar (utan självcitering) =========================");
                StringBuilder sby = new StringBuilder("");
                StringBuilder sbtotal = new StringBuilder("Totala publikationer:");
                StringBuilder sbwith = new StringBuilder("Andel med citeringsdata (WoS eller Scopus):");
                StringBuilder sbcite = new StringBuilder("WoS medelantal citeringar (ej självcitering):");
                StringBuilder sbmedian = new StringBuilder("WoS medianantal citeringar (ej självcitering):");
                StringBuilder sbzero = new StringBuilder("WoS andel ociterade publ (ej självcitering)");
                StringBuilder sbcf = new StringBuilder("WoS medel fältnormaliserade citeringar (ej självcitering):");
                StringBuilder sbjcf = new StringBuilder("WoS medel-JCF:");
                StringBuilder sbscite = new StringBuilder("Scopus medelantal citeringar (ej självcitering):");
                StringBuilder sbsmedian = new StringBuilder("Scopus medianantal citeringar (ej självcitering):");
                StringBuilder sbszero = new StringBuilder("Scopus andel ociterade publ (ej självcitering):");
                for (int i = 2022; i >= 2018; i--)
                {
                    sby.Append("\t" + i);
                    sbtotal.Append("\t" + yearpubcount[inst][i]);
                    sbwith.Append("\t" + (100*(yearpubcount[inst][i]-yearpubcountmissing[inst][i])/ yearpubcount[inst][i]).ToString("N1")+" %");
                    sbcite.Append("\t" + (yearcitecountKTH[inst][i] / yearpubcountKTH[inst][i]).ToString("N2"));
                    sbmedian.Append("\t" + util.Median(yearcitelistKTH[inst][i]));
                    sbzero.Append("\t" + (100*yearpubcountzeroKTH[inst][i] / yearpubcountKTH[inst][i]).ToString("N1")+" %");
                    if (yearpubcfcountKTH[inst][i] > 0)
                        sbcf.Append("\t" + (yearcitecfcountKTH[inst][i] / yearpubcfcountKTH[inst][i]).ToString("N2"));
                    else 
                        sbcf.Append("\t" + "(-)");
                    sbjcf.Append("\t" +  (yearcitejcfcountKTH[inst][i] / yearpubjcfcountKTH[inst][i]).ToString("N2"));
                    sbscite.Append("\t" + (yearcitecountScopus[inst][i] / yearpubcountScopus[inst][i]).ToString("N2"));
                    sbsmedian.Append("\t" + util.Median(yearcitelistScopus[inst][i]));
                    sbszero.Append("\t" + (100 * yearpubcountzeroScopus[inst][i] / yearpubcountScopus[inst][i]).ToString("N1") + " %");
                }
                memo(sby.ToString());
                memo(sbtotal.ToString());
                memo(sbwith.ToString());
                memo(sbcite.ToString());
                memo(sbmedian.ToString());
                memo(sbzero.ToString());
                memo(sbcf.ToString());
                memo(sbjcf.ToString());
                memo(sbscite.ToString());
                memo(sbsmedian.ToString());
                memo(sbszero.ToString());

                memo("=================citehist=========================");
                memo(citehistdict[inst].GetDHist());
                memo("=================citeself=========================");
                memo(citeselfhistdict[inst].GetDHist());
                memo("=================scopus=========================");
                memo(scopushistdict[inst].GetDHist());
                memo("=================cfhist=========================");
                memo(cfhistdict[inst].GetDHist());
                memo("=================cfself=========================");
                memo(cfselfhistdict[inst].GetDHist());
                memo("=================jcf=========================");
                memo(jcfhistdict[inst].GetDHist());
                memo("=================topx=========================");
                memo(topxhistdict[inst].GetSHist());
                memo("=================subjhist=========================");
                memo(subjhistdict[inst].GetSHist());
                memo("=================nsubjhist========================");
                memo(nsubjhistdict[inst].GetIHist());

                if (CB_gephi.Checked)
                {
                    util.WriteGephi(subjnodedict[inst], subjnodeiddict, subjedgedict[inst], inst, "WoS subjects ", false);
                }

            }

        }

        private void Virtinstbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Gruppfil:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fngrupp = openFileDialog1.FileName;
            memo("Reading " + fngrupp);

            int n = 0;
            using (StreamReader sr = new StreamReader(fngrupp))
            {
                string hline = sr.ReadLine();
                string[] hwords = hline.Split('\t');
                int cascol = -1;
                int subjectcol = -1;
                for (int i = 0; i < hwords.Length; i++)
                {
                    if (hwords[i].ToUpper() == "CAS")
                        cascol = i;
                    if (hwords[i].ToUpper() == "ÄMNE")
                        subjectcol = i;
                }
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 2)
                        continue;
                    string cas = "";
                    string subject = "";
                    if (cascol >= 0)
                        cas = words[cascol];
                    if (subjectcol >= 0)
                        subject = words[subjectcol];
                    if (cas + subject == "")
                    {
                        for (int i = 0; i < words.Length; i++)
                        {
                            if (words[i].Length == 3 || words[i].Length == 4)
                                cas = words[i].ToLower();
                        }
                        if (cas == "")
                            continue;
                    }
                    int igroup = words.Length - 1;
                    string group = "";
                    do
                    {
                        group = words[igroup]; // group name in last column
                        igroup--;
                    }
                    while (String.IsNullOrEmpty(group));
                    group = group.Trim(); 
                    if (String.IsNullOrEmpty(group))
                        continue;
                    n++;
                    if (!String.IsNullOrEmpty(cas))
                    {
                        authorclass ac = authorclass.findbyCAS(cas, caslist);
                        if (ac == null)
                            continue;
                        if (String.IsNullOrEmpty(group))
                            continue;
                        ac.addgroup(group, true);
                    }
                    else if (!String.IsNullOrEmpty(subject))
                    {
                        List<authorclass> aclist = authorclass.getbysubject(subject, caslist);
                        foreach (authorclass ac in aclist)
                            ac.addgroup(group, true);
                    }
                }
            }

            memo("Group entries: " + n);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void swepubbutton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Swepub-fil:";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            Dictionary<int, string> idoa = new Dictionary<int, string>();
            Dictionary<string, int> truecount = new Dictionary<string, int>();
            Dictionary<string, int> falsecount = new Dictionary<string, int>();


            string key = "duplicate_ids";
            string oa = "open_access";

            string fn = openFileDialog1.FileName;
            memo("Reading " + fn);

            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine(); //throw away query header

                string hline = sr.ReadLine();

                int ikey = -1;
                int ioa = -1;
                string[] hwords = hline.Split('\t');
                for (int i = 0; i < hwords.Length; i++)
                {
                    if (hwords[i] == key)
                        ikey = i;
                    if (hwords[i] == oa)
                        ioa = i;

                }

                string rex = @"org:du\-(\d+)";

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = util.splitcsv(line, '\t');//line.Split('\t');

                    string ids = words[ikey];
                    int id = -1;
                    foreach (Match m in Regex.Matches(ids, rex))
                    {
                        memo(m.Groups[1].Value);
                        id = util.tryconvert(m.Groups[1].Value);
                    }
                    if (id < 0)
                        memo("Fail for " + ids);

                    idoa.Add(id, words[ioa].ToUpper());
                }

                int startyear = 2021;
                int endyear = 2022;
                for (int year = startyear; year <= endyear; year++)
                {
                    truecount.Clear();
                    falsecount.Clear();
                    truecount.Add("HDa", 0);
                    foreach (string inst in authorclass.institutions)
                        truecount.Add(inst, 0);
                    falsecount.Add("HDa", 0);
                    foreach (string inst in authorclass.institutions)
                        falsecount.Add(inst, 0);
                    foreach (pubclass pc in publist)
                    {
                        if (!pc.has(pubclass.nbnstring))
                            continue;
                        if (pc.year != year)
                            continue;
                        int id = util.tryconvert(pc.dict[pubclass.nbnstring].Split('-')[1]);
                        if (id < 0)
                            continue;
                        if (!idoa.ContainsKey(id))
                            continue;
                        if (idoa[id] == "TRUE")
                        {
                            truecount["HDa"]++;
                            foreach (string inst in pc.get_authorinstitution(true,true))
                                truecount[inst]++;
                        }
                        else
                        {
                            falsecount["HDa"]++;
                            foreach (string inst in pc.get_authorinstitution(true,true))
                                falsecount[inst]++;
                        }
                    }
                    memo("År " + year);
                    memo("Inst\tOpen access\tNot open access");
                    foreach (string inst in truecount.Keys)
                    {
                        memo(inst + "\t" + truecount[inst] + "\t" + falsecount[inst]);
                    }
                }


            }

        }
    }
}
