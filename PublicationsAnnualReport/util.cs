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
            return splitcsv(line, ';', '"', '"');
        }
        public static string[] splitcsv(string line, char splitchar,char groupstart,char groupend)
        {
            string rex = "(\\"+groupstart+"[^\\"+groupend+"]*\\"+groupend+")" + splitchar;
            string matchsub = @"¤¤¤MATCH£";
            string matchrex = matchsub + @"(\d+)";
            //string splitstring =  "\"" + splitchar + "\"" ;
            MatchCollection matches = Regex.Matches(line+splitchar, rex);
            if (matches.Count > 0)
            {
                //int imatch = 0;
                for (int imatch=0; imatch < matches.Count;imatch++)
                {
                    line = line.Replace(matches[imatch].Groups[1].Value,matchsub+imatch);
                    //imatch++;
                }
                string[] words = line.Split(splitchar);
                for (int i=0;i<words.Length;i++)
                {
                    //if (words[i].Trim('\"').StartsWith("%%%MATCH£"))
                    //{
                    //    int imatch = util.tryconvert(words[i].Split('£')[1].Trim(groupend));
                    //    words[i] = matches[imatch].Groups[1].Value;
                    //}
                    foreach (Match m in Regex.Matches(words[i],matchrex))
                    {
                        int imatch = util.tryconvert(m.Groups[1].Value);
                        words[i] = words[i].Replace(m.Value, matches[imatch].Groups[1].Value);
                    }
                    //if (words[i].Contains("%%%MATCH£"))
                    //{
                    //    int imatch = util.tryconvert(words[i].Split('£')[1].Trim(groupend));
                    //    words[i] = words[i].Replace("%%%MATCH£"+imatch,matches[imatch].Groups[1].Value);
                    //}
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

        public static double Median(List<double> numbers)
        {
            int numberCount = numbers.Count();
            if (numberCount == 0)
                return 0;
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) +
                    sortedNumbers.ElementAt(halfIndex-1)) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }
            return median;
        }

        public static string mergestringvector(string[] sv)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in sv)
                sb.Append(s);
            return sb.ToString();
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

        public static string freefilename(string fnbase)
        {
            if (!File.Exists(fnbase))
                return fnbase;

            int n = 0;
            string fn = fnbase;
            do
            {
                n++;
                fn = fnbase.Replace(".", n.ToString() + ".");
            }
            while (File.Exists(fn));
            return fn;
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
        public static string remove_disambig(string title)
        {
            string tit = title;
            if (tit.IndexOf("(") > 0)
                tit = tit.Remove(tit.IndexOf("(")).Trim();
            else if (tit.IndexOf(",") > 0)
                tit = tit.Remove(tit.IndexOf(",")).Trim();
            //if (tit != title)
            //    Console.WriteLine(title + " |" + tit + "|");
            return tit;
        }

        public static bool is_disambig(string title)
        {
            return (title != remove_disambig(title));
        }

        static Dictionary<char, int> romanNumbersDictionary = new Dictionary<char, int>() {
    {
        'I',
        1
    }, {
        'V',
        5
    }, {
        'X',
        10
    }, {
        'L',
        50
    }, {
        'C',
        100
    }, {
        'D',
        500
    }, {
        'M',
        1000
    }
};



        public static int RomanToInt(string s)
        {
            int sum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char currentRomanChar = s[i];
                romanNumbersDictionary.TryGetValue(currentRomanChar, out int num);
                if (i + 1 < s.Length && romanNumbersDictionary[s[i + 1]] > romanNumbersDictionary[currentRomanChar])
                {
                    sum -= num;
                }
                else
                {
                    sum += num;
                }
            }
            return sum;
        }
        public static bool is_roman(string name) //check for roman numeral
        {
            if (String.IsNullOrEmpty(name))
                return false;
            for (int i = 0; i < name.Length; i++)
            {
                char currentRomanChar = name.ToUpper()[i];
                if (!romanNumbersDictionary.ContainsKey(currentRomanChar))
                    return false;
            }
            return true;
        }

        public static bool onlydigits(string s)
        {
            string rex = @"\D";
            foreach (Match m in Regex.Matches(s, rex))
                return false;
            return true;
        }

        public static bool is_latin(string name)
        {
            return (get_alphabet(name) == "latin");
        }

        public static string get_alphabet(string name)
        {
            char[] letters = remove_disambig(name).ToCharArray();
            int n = 0;
            int sum = 0;
            //int nlatin = 0;
            Dictionary<string, int> alphdir = new Dictionary<string, int>();
            foreach (char c in letters)
            {
                int uc = Convert.ToInt32(c);
                sum += uc;
                string alphabet = "none";
                if (uc <= 0x0040) alphabet = "none";
                //else if ((uc >= 0x0030) && (uc <= 0x0039)) alphabet = "number";
                //else if ((uc >= 0x0020) && (uc <= 0x0040)) alphabet = "punctuation";
                else if ((uc >= 0x0041) && (uc <= 0x007F)) alphabet = "latin";
                else if ((uc >= 0x00A0) && (uc <= 0x00FF)) alphabet = "latin";
                else if ((uc >= 0x0100) && (uc <= 0x017F)) alphabet = "latin";
                else if ((uc >= 0x0180) && (uc <= 0x024F)) alphabet = "latin";
                else if ((uc >= 0x0250) && (uc <= 0x02AF)) alphabet = "phonetic";
                else if ((uc >= 0x02B0) && (uc <= 0x02FF)) alphabet = "spacing modifier letters";
                else if ((uc >= 0x0300) && (uc <= 0x036F)) alphabet = "combining diacritical marks";
                else if ((uc >= 0x0370) && (uc <= 0x03FF)) alphabet = "greek and coptic";
                else if ((uc >= 0x0400) && (uc <= 0x04FF)) alphabet = "cyrillic";
                else if ((uc >= 0x0500) && (uc <= 0x052F)) alphabet = "cyrillic";
                else if ((uc >= 0x0530) && (uc <= 0x058F)) alphabet = "armenian";
                else if ((uc >= 0x0590) && (uc <= 0x05FF)) alphabet = "hebrew";
                else if ((uc >= 0x0600) && (uc <= 0x06FF)) alphabet = "arabic";
                else if ((uc >= 0x0700) && (uc <= 0x074F)) alphabet = "syriac";
                else if ((uc >= 0x0780) && (uc <= 0x07BF)) alphabet = "thaana";
                else if ((uc >= 0x0900) && (uc <= 0x097F)) alphabet = "devanagari";
                else if ((uc >= 0x0980) && (uc <= 0x09FF)) alphabet = "bengali";
                else if ((uc >= 0x0A00) && (uc <= 0x0A7F)) alphabet = "gurmukhi";
                else if ((uc >= 0x0A80) && (uc <= 0x0AFF)) alphabet = "gujarati";
                else if ((uc >= 0x0B00) && (uc <= 0x0B7F)) alphabet = "oriya";
                else if ((uc >= 0x0B80) && (uc <= 0x0BFF)) alphabet = "tamil";
                else if ((uc >= 0x0C00) && (uc <= 0x0C7F)) alphabet = "telugu";
                else if ((uc >= 0x0C80) && (uc <= 0x0CFF)) alphabet = "kannada";
                else if ((uc >= 0x0D00) && (uc <= 0x0D7F)) alphabet = "malayalam";
                else if ((uc >= 0x0D80) && (uc <= 0x0DFF)) alphabet = "sinhala";
                else if ((uc >= 0x0E00) && (uc <= 0x0E7F)) alphabet = "thai";
                else if ((uc >= 0x0E80) && (uc <= 0x0EFF)) alphabet = "lao";
                else if ((uc >= 0x0F00) && (uc <= 0x0FFF)) alphabet = "tibetan";
                else if ((uc >= 0x1000) && (uc <= 0x109F)) alphabet = "myanmar";
                else if ((uc >= 0x10A0) && (uc <= 0x10FF)) alphabet = "georgian";
                else if ((uc >= 0x1100) && (uc <= 0x11FF)) alphabet = "korean";
                else if ((uc >= 0x1200) && (uc <= 0x137F)) alphabet = "ethiopic";
                else if ((uc >= 0x13A0) && (uc <= 0x13FF)) alphabet = "cherokee";
                else if ((uc >= 0x1400) && (uc <= 0x167F)) alphabet = "unified canadian aboriginal syllabics";
                else if ((uc >= 0x1680) && (uc <= 0x169F)) alphabet = "ogham";
                else if ((uc >= 0x16A0) && (uc <= 0x16FF)) alphabet = "runic";
                else if ((uc >= 0x1700) && (uc <= 0x171F)) alphabet = "tagalog";
                else if ((uc >= 0x1720) && (uc <= 0x173F)) alphabet = "hanunoo";
                else if ((uc >= 0x1740) && (uc <= 0x175F)) alphabet = "buhid";
                else if ((uc >= 0x1760) && (uc <= 0x177F)) alphabet = "tagbanwa";
                else if ((uc >= 0x1780) && (uc <= 0x17FF)) alphabet = "khmer";
                else if ((uc >= 0x1800) && (uc <= 0x18AF)) alphabet = "mongolian";
                else if ((uc >= 0x1900) && (uc <= 0x194F)) alphabet = "limbu";
                else if ((uc >= 0x1950) && (uc <= 0x197F)) alphabet = "tai le";
                else if ((uc >= 0x19E0) && (uc <= 0x19FF)) alphabet = "khmer";
                else if ((uc >= 0x1D00) && (uc <= 0x1D7F)) alphabet = "phonetic";
                else if ((uc >= 0x1E00) && (uc <= 0x1EFF)) alphabet = "latin";
                else if ((uc >= 0x1F00) && (uc <= 0x1FFF)) alphabet = "greek and coptic";
                else if ((uc >= 0x2000) && (uc <= 0x206F)) alphabet = "none";
                else if ((uc >= 0x2070) && (uc <= 0x209F)) alphabet = "none";
                else if ((uc >= 0x20A0) && (uc <= 0x20CF)) alphabet = "none";
                else if ((uc >= 0x20D0) && (uc <= 0x20FF)) alphabet = "combining diacritical marks for symbols";
                else if ((uc >= 0x2100) && (uc <= 0x214F)) alphabet = "letterlike symbols";
                else if ((uc >= 0x2150) && (uc <= 0x218F)) alphabet = "none";
                else if ((uc >= 0x2190) && (uc <= 0x21FF)) alphabet = "none";
                else if ((uc >= 0x2200) && (uc <= 0x22FF)) alphabet = "none";
                else if ((uc >= 0x2300) && (uc <= 0x23FF)) alphabet = "none";
                else if ((uc >= 0x2400) && (uc <= 0x243F)) alphabet = "none";
                else if ((uc >= 0x2440) && (uc <= 0x245F)) alphabet = "optical character recognition";
                else if ((uc >= 0x2460) && (uc <= 0x24FF)) alphabet = "enclosed alphanumerics";
                else if ((uc >= 0x2500) && (uc <= 0x257F)) alphabet = "none";
                else if ((uc >= 0x2580) && (uc <= 0x259F)) alphabet = "none";
                else if ((uc >= 0x25A0) && (uc <= 0x25FF)) alphabet = "none";
                else if ((uc >= 0x2600) && (uc <= 0x26FF)) alphabet = "none";
                else if ((uc >= 0x2700) && (uc <= 0x27BF)) alphabet = "none";
                else if ((uc >= 0x27C0) && (uc <= 0x27EF)) alphabet = "none";
                else if ((uc >= 0x27F0) && (uc <= 0x27FF)) alphabet = "none";
                else if ((uc >= 0x2800) && (uc <= 0x28FF)) alphabet = "braille";
                else if ((uc >= 0x2900) && (uc <= 0x297F)) alphabet = "none";
                else if ((uc >= 0x2980) && (uc <= 0x29FF)) alphabet = "none";
                else if ((uc >= 0x2A00) && (uc <= 0x2AFF)) alphabet = "none";
                else if ((uc >= 0x2B00) && (uc <= 0x2BFF)) alphabet = "none";
                else if ((uc >= 0x2E80) && (uc <= 0x2EFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F00) && (uc <= 0x2FDF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2FF0) && (uc <= 0x2FFF)) alphabet = "none";
                else if ((uc >= 0x3000) && (uc <= 0x303F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3040) && (uc <= 0x309F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x30A0) && (uc <= 0x30FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3100) && (uc <= 0x312F)) alphabet = "bopomofo";
                else if ((uc >= 0x3130) && (uc <= 0x318F)) alphabet = "korean";
                else if ((uc >= 0x3190) && (uc <= 0x319F)) alphabet = "chinese/japanese";
                else if ((uc >= 0x31A0) && (uc <= 0x31BF)) alphabet = "bopomofo";
                else if ((uc >= 0x31F0) && (uc <= 0x31FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3200) && (uc <= 0x32FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3300) && (uc <= 0x33FF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x3400) && (uc <= 0x4DBF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x4DC0) && (uc <= 0x4DFF)) alphabet = "none";
                else if ((uc >= 0x4E00) && (uc <= 0x9FFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA000) && (uc <= 0xA48F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xA490) && (uc <= 0xA4CF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xAC00) && (uc <= 0xD7AF)) alphabet = "korean";
                else if ((uc >= 0xD800) && (uc <= 0xDB7F)) alphabet = "high surrogates";
                else if ((uc >= 0xDB80) && (uc <= 0xDBFF)) alphabet = "high private use surrogates";
                else if ((uc >= 0xDC00) && (uc <= 0xDFFF)) alphabet = "low surrogates";
                else if ((uc >= 0xE000) && (uc <= 0xF8FF)) alphabet = "private use area";
                else if ((uc >= 0xF900) && (uc <= 0xFAFF)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFB00) && (uc <= 0xFB4F)) alphabet = "alphabetic presentation forms";
                else if ((uc >= 0xFB50) && (uc <= 0xFDFF)) alphabet = "arabic";
                else if ((uc >= 0xFE00) && (uc <= 0xFE0F)) alphabet = "variation selectors";
                else if ((uc >= 0xFE20) && (uc <= 0xFE2F)) alphabet = "combining half marks";
                else if ((uc >= 0xFE30) && (uc <= 0xFE4F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xFE50) && (uc <= 0xFE6F)) alphabet = "small form variants";
                else if ((uc >= 0xFE70) && (uc <= 0xFEFF)) alphabet = "arabic";
                else if ((uc >= 0xFF00) && (uc <= 0xFFEF)) alphabet = "halfwidth and fullwidth forms";
                else if ((uc >= 0xFFF0) && (uc <= 0xFFFF)) alphabet = "specials";
                else if ((uc >= 0x10000) && (uc <= 0x1007F)) alphabet = "linear b";
                else if ((uc >= 0x10080) && (uc <= 0x100FF)) alphabet = "linear b";
                else if ((uc >= 0x10100) && (uc <= 0x1013F)) alphabet = "aegean numbers";
                else if ((uc >= 0x10300) && (uc <= 0x1032F)) alphabet = "old italic";
                else if ((uc >= 0x10330) && (uc <= 0x1034F)) alphabet = "gothic";
                else if ((uc >= 0x10380) && (uc <= 0x1039F)) alphabet = "ugaritic";
                else if ((uc >= 0x10400) && (uc <= 0x1044F)) alphabet = "deseret";
                else if ((uc >= 0x10450) && (uc <= 0x1047F)) alphabet = "shavian";
                else if ((uc >= 0x10480) && (uc <= 0x104AF)) alphabet = "osmanya";
                else if ((uc >= 0x10800) && (uc <= 0x1083F)) alphabet = "cypriot syllabary";
                else if ((uc >= 0x1D000) && (uc <= 0x1D0FF)) alphabet = "byzantine musical symbols";
                else if ((uc >= 0x1D100) && (uc <= 0x1D1FF)) alphabet = "musical symbols";
                else if ((uc >= 0x1D300) && (uc <= 0x1D35F)) alphabet = "tai xuan jing symbols";
                else if ((uc >= 0x1D400) && (uc <= 0x1D7FF)) alphabet = "none";
                else if ((uc >= 0x20000) && (uc <= 0x2A6DF)) alphabet = "chinese/japanese";
                else if ((uc >= 0x2F800) && (uc <= 0x2FA1F)) alphabet = "chinese/japanese";
                else if ((uc >= 0xE0000) && (uc <= 0xE007F)) alphabet = "none";

                bool ucprint = false;
                if (alphabet != "none")
                {
                    n++;
                    if (!alphdir.ContainsKey(alphabet))
                        alphdir.Add(alphabet, 0);
                    alphdir[alphabet]++;
                }
                else if (uc != 0x0020)
                {
                    //Console.Write("c=" + c.ToString() + ", uc=0x" + uc.ToString("x5") + "|");
                    //ucprint = true;
                }
                if (ucprint)
                    Console.WriteLine();
            }

            int nmax = 0;
            string alphmax = "none";
            foreach (string alph in alphdir.Keys)
            {
                //Console.WriteLine("ga:" + alph + " " + alphdir[alph].ToString());
                if (alphdir[alph] > nmax)
                {
                    nmax = alphdir[alph];
                    alphmax = alph;
                }
            }

            if (letters.Length > 2 * n) //mostly non-alphabetic
                return "none";
            else if (nmax > n / 2) //mostly same alphabet
                return alphmax;
            else
                return "mixed"; //mixed alphabets
        }

        public static Dictionary<string, string> alphabet_sv = new Dictionary<string, string>();//from English name to Swedish name of alphabets

        public static string get_alphabet_sv(string alph_en)
        {
            Console.WriteLine("gas:" + alph_en);
            if (alphabet_sv.Count == 0)
            {
                alphabet_sv.Add("bopomofo", "zhuyin");
                alphabet_sv.Add("halfwidth and fullwidth forms", "");
                alphabet_sv.Add("syriac", "syriska alfabetet");
                alphabet_sv.Add("thaana", "tāna");
                alphabet_sv.Add("lao", "laotisk skrift");
                alphabet_sv.Add("khmer", "khmerisk skrift");
                alphabet_sv.Add("gurmukhi", "gurmukhi");
                alphabet_sv.Add("myanmar", "burmesisk skrift");
                alphabet_sv.Add("tibetan", "tibetansk skrift");
                alphabet_sv.Add("sinhala", "singalesisk skrift");
                alphabet_sv.Add("ethiopic", "etiopisk skrift");
                alphabet_sv.Add("oriya", "oriya-skrift");
                alphabet_sv.Add("kannada", "kannada");
                alphabet_sv.Add("malayalam", "malayalam");
                alphabet_sv.Add("telugu", "teluguskrift");
                alphabet_sv.Add("tamil", "tamilska alfabetet");
                alphabet_sv.Add("gujarati", "gujarati");
                alphabet_sv.Add("bengali", "bengalisk skrift");
                alphabet_sv.Add("armenian", "armeniska alfabetet");
                alphabet_sv.Add("georgian", "georgiska alfabetet");
                alphabet_sv.Add("devanagari", "devanāgarī");
                alphabet_sv.Add("korean", "hangul");
                alphabet_sv.Add("hebrew", "hebreiska alfabetet");
                alphabet_sv.Add("greek and coptic", "grekiska alfabetet");
                alphabet_sv.Add("chinese/japanese", "kinesiska tecken");
                alphabet_sv.Add("thai", "thailändska alfabetet");
                alphabet_sv.Add("cyrillic", "kyrilliska alfabetet");
                alphabet_sv.Add("arabic", "arabiska alfabetet");
                alphabet_sv.Add("latin", "latinska alfabetet");
            }

            if (alphabet_sv.ContainsKey(alph_en))
                return alphabet_sv[alph_en];
            else
                return "okänd skrift";
        }




    }
}
