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

namespace PublicationsAnnualReport
{
    public partial class FormProfileSubject : Form
    {
        private Form1 parent;
        private List<authorclass> authorlist = new List<authorclass>();
        public List<string> usecaslist = new List<string>();
        public static string allstring = "--ALL--";
        public string selectstring = "sampub-";

        public static Dictionary<string, string> profileshort = new Dictionary<string, string>()
        {
            {"Interkulturella studier","ISTUD" },
            {"Energi och samhällsbyggnad","ESB" },
            {"Komplexa system - mikrodataanalys","MD" },
            {"Utbildning och lärande","UL" },
            {"Hälsa och välfärd","HV" },
            {"Stålformning och ytteknik","SY" },
            {"no profile","-" }
        };


        public FormProfileSubject(Form1 parentpar)
        {
            InitializeComponent();
            parent = parentpar;

            LBprofsubj.Items.Add(allstring);
            foreach (string s in authorclass.institutions)
                LBprofsubj.Items.Add(s);
            foreach (string s in authorclass.departments)
                LBprofsubj.Items.Add(s);
            foreach (string s in authorclass.profiles)
                LBprofsubj.Items.Add(s);
            foreach (string s in authorclass.subjects)
                LBprofsubj.Items.Add(s);
            label4.Text = "Cluster threshold " + TBcluster.Value;

            openFileDialog1.InitialDirectory = TBdir.Text;
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

        private void Authorlistbutton_Click(object sender, EventArgs e)
        {
            authorlist = read_namelist();
            foreach (authorclass ac in authorlist)
                usecaslist.Add(ac.CAS);
            memo(usecaslist.Count + " authors in list.");

        }

        private List<authorclass> read_namelist()
        {
            List<authorclass> aclist = new List<authorclass>();
            openFileDialog1.Title = "Författarlista:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return aclist;
            }

            string fncas = openFileDialog1.FileName;
            memo("Reading " + fncas);

            using (StreamReader sr = new StreamReader(fncas))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    var q = from authorclass c in Form1.caslist where c.name == line.Trim() select c;
                    if (q.Count() != 1)
                        q = from authorclass c in Form1.caslist where c.reversename() == line.Trim() select c;
                    if (q.Count() != 1)
                        memo(line + " not found!");
                    else
                    {
                        aclist.Add(q.First());
                    }
                }
            }
            return aclist;
        }


        private void DoGephiIndividual(List<string> aulist)
        {
            string folder = openFileDialog1.InitialDirectory;
            string fn = util.unusedfn(folder + selectstring+@"nodes.csv");
            Dictionary<string, int> nodedict = new Dictionary<string, int>();
            Dictionary<int, Dictionary<int, int>> edgedict = new Dictionary<int, Dictionary<int, int>>();
            Dictionary<string, KnownColor> colordict = new Dictionary<string, KnownColor>();
            List<string> hasedge = new List<string>();
            KnownColor[] knowncol = util.getallcolors();
            int nextcolor = 0;

            List<authorclass> namecolorlist = new List<authorclass>();
            if (CB_namelist_color.Checked)
            {
                namecolorlist = read_namelist();
            }

            int j = 1;
            memo("Finding nodes");
            foreach (string au in aulist)
            {
                if (nodedict.ContainsKey(au))
                    continue;
                var q = from c in Form1.publist
                        where c.casauthors.Contains(au)
                        select c;
                if (q.Count() == 0) //skip unpublished
                    continue;
                nodedict.Add(au, j);
                j++;
            }

            memo("Finding edges");
            foreach (string au in aulist)
            {
                var q = from c in Form1.publist
                        where c.casauthors.Contains(au)
                        select c;
                if (CBpeer.Checked)
                    q = from c in q where c.dict[pubclass.contenttypestring] == "Refereegranskat" select c;
                foreach (pubclass pc in q)
                {
                    //bool beyondau = false;
                    foreach (string au2 in pc.casauthors)
                    {
                        if (!aulist.Contains(au2))
                            continue;
                        if (nodedict[au2] <= nodedict[au])
                            continue;
                        if (!edgedict.ContainsKey(nodedict[au]))
                        {
                            edgedict.Add(nodedict[au], new Dictionary<int, int>());
                            if (!hasedge.Contains(au))
                                hasedge.Add(au);
                        }
                        if (!edgedict[nodedict[au]].ContainsKey(nodedict[au2]))
                        {
                            edgedict[nodedict[au]].Add(nodedict[au2], 0);
                            if (!hasedge.Contains(au2))
                                hasedge.Add(au2);
                        }
                        edgedict[nodedict[au]][nodedict[au2]]++;
                    }
                }
            }

            memo("Writing nodes to " + fn);
            using (StreamWriter sw = new StreamWriter(fn))
            {
                sw.WriteLine("Id;Label;Color");
                int i = 1;
                foreach (string au in aulist)
                {
                    if (!nodedict.ContainsKey(au))
                        continue;

                    i = nodedict[au];
                    string label = "";
                    string profile = authorclass.profilefromCAS(au, authorlist);
                    string institution = authorclass.institutionfromCAS(au, authorlist);
                    if (profileshort.ContainsKey(profile))
                        profile = profileshort[profile];
                    memo(authorclass.namefromCAS(au, authorlist) + "\t" + au);
                    //memo(profile);
                    if (CBname.Checked)
                        label += " " + authorclass.namefromCAS(au, authorlist);
                    if (CBcas.Checked)
                        label += " " + au;
                    if (CBprofile.Checked)
                        label += " " + profile;
                    if (CBsubject.Checked)
                        label += " " + authorclass.subjectfromCAS(au, authorlist);

                    if (label.Contains(";"))
                        label = label.Replace(";", ",");
                    string colorkey = "none";
                    //Color col = Color.Black;
                    if (CB_profilecolor.Checked)
                        colorkey = profile;
                    else if (CB_inst_color.Checked)
                        colorkey = institution;
                    else if (CB_namelist_color.Checked)
                    {
                        var qq = from c in namecolorlist where c.CAS == au select c;
                        if (qq.Count() == 1)
                            colorkey = "yes";
                        else
                            colorkey = "no";
                    }
                    if (!colordict.ContainsKey(colorkey))
                    {
                        memo("Color " + knowncol[nextcolor].ToString());
                        colordict.Add(colorkey, knowncol[nextcolor]);
                        nextcolor++;
                    }
                    if (CB_publess.Checked || hasedge.Contains(au))
                        sw.WriteLine(i + ";" + label + ";" + String.Format("#{0:X6}", Color.FromKnownColor(colordict[colorkey]).ToArgb() & 0x00ffffff));   //";#0000FF");
                    //nodedict.Add(au, i);
                    i++;
                }
            }

            string fn2 = fn.Replace("nodes", "edges");//unusedfn(folder + @"energiedges.csv");
            memo("Writing edges to " + fn2);
            using (StreamWriter sw = new StreamWriter(fn2))
            {
                sw.WriteLine("Source;Target;Weight;Label");
                foreach (int iau in edgedict.Keys)
                {
                    foreach (int iau2 in edgedict[iau].Keys )
                    {
                        sw.WriteLine(iau+";"+iau2+";"+edgedict[iau][iau2] + ";" + edgedict[iau][iau2]);
                    }
                }
            }


            memo("Done");

        }

        private void DoGephiSubject(List<string> aulist)
        {
            string folder = openFileDialog1.InitialDirectory;
            string fn = util.unusedfn(folder + selectstring + @"nodes.csv");
            Dictionary<string, int> nodedict = new Dictionary<string, int>();
            Dictionary<int, Dictionary<int, int>> edgedict = new Dictionary<int, Dictionary<int, int>>();
            Dictionary<string, KnownColor> colordict = new Dictionary<string, KnownColor>();
            KnownColor[] knowncol = util.getallcolors();
            int nextcolor = 0;

            List<authorclass> namecolorlist = new List<authorclass>();
            if (CB_namelist_color.Checked)
            {
                namecolorlist = read_namelist();
            }

            memo("Writing nodes to " + fn);

            try
            {
                if (File.Exists(fn))
                {
                    memo("File already exists: " + fn);
                    return;
                }
            }
            catch (Exception e)
            {
                memo("IO error " + e.Message);
            }

            using (StreamWriter sw = new StreamWriter(fn))
            {
                sw.WriteLine("Id;Label;Color");
                int i = 1;
                foreach (string au in aulist)
                {
                    string nodekey = authorclass.subjectfromCAS(au, authorlist);
                    if (nodekey == "no subject")
                        memo(au + " no subject");
                    if (nodedict.ContainsKey(nodekey))
                        continue;
                    var q = from c in Form1.publist
                            where c.casauthors.Contains(au)
                            select c;
                    if (q.Count() == 0) //skip unpublished
                        continue;
                    string label = "";
                    string profile = authorclass.profilefromCAS(au, authorlist);
                    string institution = authorclass.institutionfromCAS(au, authorlist);
                    if (profileshort.ContainsKey(profile))
                        profile = profileshort[profile];
                    //memo(profile);
                    memo(authorclass.namefromCAS(au, authorlist) + "\t" + au);
                    if (CBname.Checked)
                        label += " " + authorclass.namefromCAS(au, authorlist);
                    if (CBcas.Checked)
                        label += " " + au;
                    if (CBprofile.Checked)
                        label += " " + profile;
                    if (CBsubject.Checked)
                        label += " " + authorclass.subjectfromCAS(au, authorlist);

                    if (label.Contains(";"))
                        label = label.Replace(";", ",");
                    string colorkey = "none";
                    //Color col = Color.Black;
                    if (CB_profilecolor.Checked)
                        colorkey = profile;
                    else if (CB_inst_color.Checked)
                        colorkey = institution;
                    else if (CB_namelist_color.Checked)
                    {
                        var qq = from c in namecolorlist where c.CAS == au select c;
                        if (qq.Count() == 1)
                            colorkey = "yes";
                        else
                            colorkey = "no";
                    }
                    if (!colordict.ContainsKey(colorkey))
                    {
                        memo("Color " + knowncol[nextcolor].ToString());
                        colordict.Add(colorkey, knowncol[nextcolor]);
                        nextcolor++;
                    }
                    sw.WriteLine(i + ";" + label + ";" + String.Format("#{0:X6}", Color.FromKnownColor(colordict[colorkey]).ToArgb() & 0x00ffffff));   //";#0000FF");
                    nodedict.Add(nodekey, i);
                    i++;
                }
            }

            memo("Finding edges");
            foreach (string au in aulist)
            {
                string nodekey = authorclass.subjectfromCAS(au, authorlist);

                var q = from c in Form1.publist
                        where c.casauthors.Contains(au)
                        select c;
                if (CBpeer.Checked)
                    q = from c in q where c.dict[pubclass.contenttypestring] == "Refereegranskat" select c;
                foreach (pubclass pc in q)
                {
                    //bool beyondau = false;
                    foreach (string au2 in pc.casauthors)
                    {
                        if (!aulist.Contains(au2))
                            continue;
                        string nodekey2 = authorclass.subjectfromCAS(au2, authorlist);
                        if (nodedict[nodekey2] <= nodedict[nodekey])
                            continue;
                        if (!edgedict.ContainsKey(nodedict[nodekey]))
                        {
                            edgedict.Add(nodedict[nodekey], new Dictionary<int, int>());
                        }
                        if (!edgedict[nodedict[nodekey]].ContainsKey(nodedict[nodekey2]))
                            edgedict[nodedict[nodekey]].Add(nodedict[nodekey2], 0);
                        edgedict[nodedict[nodekey]][nodedict[nodekey2]]++;
                    }
                }
            }

            string fn2 = fn.Replace("nodes", "edges");//unusedfn(folder + @"energiedges.csv");
            memo("Writing edges to " + fn2);
            using (StreamWriter sw = new StreamWriter(fn2))
            {
                sw.WriteLine("Source;Target;Weight");
                foreach (int iau in edgedict.Keys)
                {
                    foreach (int iau2 in edgedict[iau].Keys)
                    {
                        sw.WriteLine(iau + ";" + iau2 + ";" + edgedict[iau][iau2]);
                    }
                }
            }


            memo("Done");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (usecaslist.Count == 0)
            {
                memo("No authors selected");
                return;
            }

            if (RB_subject.Checked)
                DoGephiSubject(usecaslist);
            else if (RB_individual.Checked)
                DoGephiIndividual(usecaslist);

        }

        private void Clearauthorbutton_Click(object sender, EventArgs e)
        {
            authorlist.Clear();
            usecaslist.Clear();
        }

        private void LBprofsubj_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = LBprofsubj.SelectedItem.ToString();
            authorlist.Clear();
            usecaslist.Clear();
            if ( s == allstring)
            {
                var q = from c in Form1.caslist
                        select c;
                foreach (authorclass ac in q)
                {
                    authorlist.Add(ac);
                    usecaslist.Add(ac.CAS);
                }
                selectstring = "sampub-all-";
            }
            else 
            { 
                var q = from c in Form1.caslist
                    where c.affiliation.Contains(s)
                    select c;
                foreach (authorclass ac in q)
                {
                    if (!usecaslist.Contains(ac.CAS))
                    {
                        authorlist.Add(ac);
                        usecaslist.Add(ac.CAS);
                        if (CB_otherHDa.Checked)
                        {
                            var qq = from c in Form1.publist
                                     where c.casauthors.Contains(ac.CAS)
                                     select c;
                            foreach (var pub in qq)
                            {
                                foreach (string cocas in pub.casauthors)
                                {
                                    if (!usecaslist.Contains(cocas))
                                    {
                                        usecaslist.Add(cocas);
                                        authorlist.Add(authorclass.findbyCAS(cocas, Form1.caslist));
                                    }
                                }
                            }
                        }
                    }
                }
                if (profileshort.ContainsKey(s))
                    selectstring = profileshort[s] + "-";
                else
                    selectstring = s + "-";
            }
        }


        private int tryconvert(string s)
        {
            int i = -1;
            try
            {
                i = Convert.ToInt32(s);
            }
            catch (Exception e)
            {
                memo(e.Message);
            }
            return i;
        }

        private Dictionary<int, int> mergedicts(Dictionary<int, int> d1, Dictionary<int, int> d2, List<int> skiplist)
        {
            Dictionary<int, int> d = new Dictionary<int, int>();
            foreach (int i in d1.Keys)
            {
                if (skiplist.Contains(i))
                    continue;

                d.Add(i, d1[i]);
            }
            foreach (int i in d2.Keys)
            {
                if (skiplist.Contains(i))
                    continue;
                if (!d.ContainsKey(i))
                    d.Add(i, d2[i]);
                else
                    d[i] += d2[i];
            }
            return d;
        }

        private void clusterbutton_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> nodedict = new Dictionary<int, string>();
            Dictionary<int, Dictionary<int, int>> edgedict = new Dictionary<int, Dictionary<int, int>>();
            Dictionary<int,int> nodesumdict = new Dictionary<int,int>();


            openFileDialog1.Title = "Nodes file:";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr != DialogResult.OK)
            {
                memo("File not selected");
                return;
            }

            string fnnodes = openFileDialog1.FileName;
            memo("Reading " + fnnodes);
            int nnode = 0;
            using (StreamReader sr = new StreamReader(fnnodes))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split(';');
                    if (words.Length < 2)
                        continue;
                    int id = tryconvert(words[0]);
                    nodedict.Add(id, words[1]);
                    nodesumdict.Add(id, 0);
                    nnode++;
                }
            }
            memo("nnodes = " + nnode);

            string fnedges = fnnodes.Replace("node", "edge");
            memo("Reading " + fnedges);
            int nedge = 0;
            using (StreamReader sr = new StreamReader(fnedges))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split(';');
                    if (words.Length < 3)
                        continue;
                    int[] id = new int[2];
                    for (int i = 0; i < 2; i++)
                    {
                        id[i] = tryconvert(words[i]);
                    }
                    int w = tryconvert(words[2]);
                    for (int i = 0; i < 2; i++)
                    {
                        if (!edgedict.ContainsKey(id[i]))
                            edgedict.Add(id[i], new Dictionary<int, int>());
                        edgedict[id[i]].Add(id[1 - i], w);
                        nodesumdict[id[i]] += w;
                    }
                    nedge++;
                }
            }
            memo("nedges = " + nedge);

            int njoin = 0;
            double nfactor = 0.01*TBcluster.Value;
            do
            {
                njoin = 0;
                List<int> dummy = nodedict.Keys.ToList();
                int idmax = dummy.Max();
                foreach (int id1 in dummy)
                {
                    if (!nodedict.ContainsKey(id1))
                        continue;
                    if (!edgedict.ContainsKey(id1))
                        continue;
                    if (edgedict[id1].Count == 0)
                        continue;
                    int max = (from x in edgedict[id1] where x.Value == edgedict[id1].Max(v => v.Value) select x.Key).First();
                    if (edgedict[id1][max] > nfactor*nodesumdict[id1])
                    {
                        idmax++;
                        nodedict.Add(idmax, nodedict[id1] + "+" + nodedict[max]);
                        nodesumdict.Add(idmax, nodesumdict[id1] + nodesumdict[max]);// - 2 * edgedict[id1][max]);
                        Console.WriteLine("Joining "+nodedict[idmax]);
                        List<int> skiplist = new List<int>() { id1, max };
                        Dictionary<int, int> maxdict = mergedicts(edgedict[id1], edgedict[max], skiplist);
                        edgedict.Add(idmax, maxdict);
                        //fixa alla andra noder i edgedict!
                        List<int> dummy2 = maxdict.Keys.ToList();
                        foreach (int ii in dummy2)
                        {
                            if (edgedict[ii].ContainsKey(id1))
                                edgedict[ii][id1] = 0;
                            if (edgedict[ii].ContainsKey(max))
                                edgedict[ii][max] = 0;
                            if (skiplist.Contains(ii))
                                continue;
                            edgedict[ii].Add(idmax, maxdict[ii]);
                        }
                        nodedict.Remove(id1);
                        nodedict.Remove(max);
                        //edgedict.Remove(id1);
                        //edgedict.Remove(max);
                        njoin++;
                    }
                }
            }
            while (njoin > 0);

            memo("Done");
            Console.WriteLine("Done!");

            memo(nodedict.Count + " clusters:");
            int singles = 0;
            int nmax = 0;
            foreach (int i in nodedict.Keys)
            {
                if (nodedict[i].Contains("+"))
                {
                    int nc = nodedict[i].Split('+').Length;
                    if (nc > nmax)
                        nmax = nc;
                    memo(nc+"\t" + nodedict[i]);
                }
                else
                    singles++;
            }
            memo(nodedict.Count + " clusters, of which "+singles+" singles and "+(nodedict.Count-singles)+" multiples.");
            memo("Largest cluster: "+nmax);
        }

        private void TBcluster_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Cluster threshold " + TBcluster.Value;
        }

        private void TBdir_TextChanged(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = TBdir.Text;
        }
    }
}
