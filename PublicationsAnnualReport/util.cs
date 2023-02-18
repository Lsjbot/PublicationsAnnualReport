using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PublicationsAnnualReport
{
    class util
    {

        public static string addid(string s, int id, int pad)
        {
            return s.PadRight(pad) + "|" + id.ToString() + "|";
        }

        public static Dictionary<string,int> getTopValues(Dictionary<string,int> dict, int ntop)
        {
            var topItems = (from entry in dict
                               orderby entry.Value
                               descending
                               select entry).ToDictionary
                   (
                    pair => pair.Key,
                    pair => pair.Value
                   ).Take(ntop).ToDictionary
                   (
                    pair => pair.Key,
                    pair => pair.Value
                   );
            return topItems;
        }

        //public static int getid(string s)
        //{
        //    string rex = @"\|(\d+)\|";
        //    foreach (Match match in Regex.Matches(s, rex))
        //    {
        //        return tryconvert(match.Groups[1].Value);
        //    }

        //    return -1;

        //}

        public static string[] splitcsv(string line)
        {
            if (line.Contains("\";\""))
                return splitcsv(line, ';');
            else
                return splitcsv(line, ',');
        }

        public static string[] splitcsv(string line, char splitchar)
        {
            string rex = "\\\"([^\"]*)\\\"" + splitchar;
            //string splitstring =  "\"" + splitchar + "\"" ;
            MatchCollection matches = Regex.Matches(line+splitchar, rex);
            if (matches.Count > 0)
            {
                //int imatch = 0;
                for (int imatch=0; imatch < matches.Count;imatch++)
                {
                    line = line.Replace(matches[imatch].Groups[1].Value,"%%%MATCH£"+imatch);
                    imatch++;
                }
                string[] words = line.Split(splitchar);
                for (int i=0;i<words.Length;i++)
                {
                    if (words[i].StartsWith("%%%MATCH£"))
                    {
                        int imatch = util.tryconvert(words[i].Split('£')[1]);
                        words[i] = matches[imatch].Groups[1].Value;
                    }
                }
                //string[] words = new string[matches.Count];
                //int imatch = 0;
                //foreach (Match match in matches)
                //{
                //    words[imatch] = match.Groups[1].Value;
                //    imatch++;
                //}
                return words;
            }
            else
                return line.Split(splitchar);
            //string[] words = line.Split(splitstring, 99, System.StringSplitOptions.None);
        }

        public static List<string> parseISBN(string isbn)
        {
            List<string> ls = new List<string>();
            foreach (string s in isbn.Split(new char[]{ ';',','}))
                ls.Add(cleanISBN(s));
            return ls;
        }

        public static string cleanISBN(string isbn)
        {
            return isbn.Replace("-", "").Replace(" ", "").Trim();
        }

        public static string unusedfn(string fnbase)
        {
            string suffix = ".txt";
            if (!fnbase.Contains(suffix))
            {
                suffix = "." + fnbase.Split('.').Last();
            }
            //memo(fnbase + " suffix:" + suffix);
            int i = 1;
            string fn = fnbase.Replace(suffix, i + suffix);
            while (File.Exists(fn))
            {
                i++;
                fn = fnbase.Replace(suffix, i + suffix);
            }
            return fn;
        }

        public static KnownColor[] getallcolors()
        {
            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            return colors;
            //foreach(KnownColor knowColor in colors)
            //{
            //  Color color = Color.FromKnownColor(knowColor);
        }



        public static int tryconvert(string word)
        {
            int i = -1;

            if (String.IsNullOrEmpty(word)) 
                return -1;

            try
            {
                i = Convert.ToInt32(word);
            }
            catch (OverflowException)
            {
                //memo("i Outside the range of the Int32 type: " + word);
            }
            catch (FormatException)
            {
                //if ( !String.IsNullOrEmpty(word))
                //    Console.WriteLine("i Not in a recognizable format: " + word);
            }

            return i;

        }

        public static double tryconvertdouble(string word)
        {
            double i = -1;

            if (String.IsNullOrEmpty(word))
                return i;

            try
            {
                i = Convert.ToDouble(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Double type: " + word);
            }
            catch (FormatException)
            {
                try
                {
                    i = Convert.ToDouble(word.Replace(".", ","));
                }
                catch (FormatException)
                {
                    //Console.WriteLine("i Not in a recognizable double format: " + word.Replace(".", ","));
                }
                //Console.WriteLine("i Not in a recognizable double format: " + word);
            }

            return i;

        }

        public static string timestampfolder(string folder)
        {
            return timestampfolder(folder, "");
        }

        public static string timestampfolder(string folder,string prefix)
        {
            DateTime now = DateTime.Now;
            string separator = @"\";
            if (folder.EndsWith(separator))
                separator = "";
            return folder + separator + prefix + now.Year + "-" + now.Month + "-" + now.Day + " " + now.Hour + "-" + now.Minute + @"\";

        }

        public static string format_authorlist(List<string> ls)
        {

            if (ls.Count == 0)
                return "";
            else if (ls.Count == 1)
                return ls[0];
            else if (ls.Count < 4)
            {
                string s = ls[0];
                for (int i = 1; i < ls.Count; i++)
                    s += " & " + ls[i];
                return s;
            }
            else
            {
                return ls[0] + " et al.";
            }

        }

        public static string addcentury(int? year)
        {
            if (year == null)
                return "(no year)";
            if (year <= (DateTime.Now.Year % 100))
                return "20" + year.ToString();
            else if (year < 100)
                return "19" + year.ToString();
            else
                return year.ToString();
        }

        // From https://gist.github.com/wickedshimmy/449595/a17ab0d689623f5e6730eeb1c8606ab771149819
        public static int EditDistance(string original, string modified)
        {
            if (original == modified)
                return 0;

            int len_orig = original.Length;
            int len_diff = modified.Length;
            if (len_orig == 0 || len_diff == 0)

                return len_orig == 0 ? len_diff : len_orig;

            var matrix = new int[len_orig + 1, len_diff + 1];

            for (int i = 1; i <= len_orig; i++)
            {
                matrix[i, 0] = i;
                for (int j = 1; j <= len_diff; j++)
                {
                    int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                    if (i == 1)
                        matrix[0, j] = j;

                    var vals = new int[] {
                    matrix[i - 1, j] + 1,
                    matrix[i, j - 1] + 1,
                    matrix[i - 1, j - 1] + cost
                };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }

        public static double WeightedDistance(string original, string modified, double[,] weights)
        {
            //Weighted Levenhstein distance. Assumes each char in the strings is an index to the weight array

            if (original == modified)
                return 0;

            int len_orig = original.Length;
            int len_diff = modified.Length;
            if (len_orig == 0 || len_diff == 0)

                return len_orig == 0 ? len_diff : len_orig;

            var matrix = new double[len_orig + 1, len_diff + 1];

            for (int i = 1; i <= len_orig; i++)
            {
                matrix[i, 0] = i;
                for (int j = 1; j <= len_diff; j++)
                {
                    double cost = modified[j - 1] == original[i - 1] ? 0 : weights[(int)modified[j - 1], (int)original[i - 1]];
                    if (i == 1)
                        matrix[0, j] = j;

                    var vals = new double[] {
                        matrix[i - 1, j] + 1,
                        matrix[i, j - 1] + 1,
                        matrix[i - 1, j - 1] + cost
                    };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }


        public static void WriteGephi(Dictionary<string, int> nodedict, Dictionary<string, int> nodeiddict, Dictionary<int, Dictionary<int, double>> linkdict, string institution, string gephiname, bool skipunlinked)
        {
            //Dictionary<string, int> nodeiddict = new Dictionary<string, int>();

            string selectstring = gephiname+"-" + institution+"-";
            string folder = util.timestampfolder(@"C:\Temp\Gephi\", gephiname);
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
            List<int> linkednodes = new List<int>();
            foreach (int s0 in linkdict.Keys)
            {
                double nlink = linkdict[s0].Values.Sum();
                if (nlink < minlink)
                    continue;
                if (!linkednodes.Contains(s0))
                    linkednodes.Add(s0);
                foreach (int s in linkdict[s0].Keys)
                {
                    if (!linkednodes.Contains(s))
                        linkednodes.Add(s);
                }
            }

            int id = 0;
            Console.WriteLine("Writing nodes to " + fn);
            using (StreamWriter sw = new StreamWriter(fn))
            {
                sw.WriteLine("Id;Label;Color");
                foreach (string nodename in nodedict.Keys)
                {
                    if (skipunlinked && !linkednodes.Contains(nodeiddict[nodename]))
                        continue;
                    if (!colordict.ContainsKey(nodedict[nodename]))
                    {
                        //memo("Color " + knowncol[nextcolor].ToString());
                        colordict.Add(nodedict[nodename], knowncol[nextcolor]);
                        nextcolor++;
                    }
                    sw.WriteLine(nodeiddict[nodename] + ";" + nodename + ";" + String.Format("#{0:X6}", Color.FromKnownColor(colordict[nodedict[nodename]]).ToArgb() & 0x00ffffff));   //";#0000FF");
                }
            }

            string fn2 = fn.Replace("nodes", "edges");//unusedfn(folder + @"energiedges.csv");
            Console.WriteLine("Writing edges to " + fn2);
            using (StreamWriter sw = new StreamWriter(fn2))
            {
                sw.WriteLine("Source;Target;Weight");
                foreach (int s0 in linkdict.Keys)
                {
                    if (skipunlinked && !linkednodes.Contains(s0))
                        continue;
                    foreach (int s in linkdict[s0].Keys)
                    {
                        if (skipunlinked && !linkednodes.Contains(s))
                            continue;
                        sw.WriteLine(s0 + ";" + s + ";" + linkdict[s0][s].ToString(CultureInfo.InvariantCulture));
                    }
                }

            }

        }


    }
}
