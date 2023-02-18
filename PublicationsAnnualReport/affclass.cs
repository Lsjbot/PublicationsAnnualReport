using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PublicationsAnnualReport
{
    public class affclass
    {
        public int id = 0;
        public string name = "";
        public int orgtype = 0;
        public int partof = -1;
        public int aliasof = -1;

        public const int sweuni = 1;
        public const int otheruni = 2;
        public const int other = 3;
        public const int unknown = 4;
        public const int reslab = 5;

        public static List<affclass> afflist = new List<affclass>();
        public static Dictionary<int, string> unidict = new Dictionary<int, string>();
        public static Dictionary<int, string> unitypedict = new Dictionary<int, string>();
        public static Dictionary<string, string> typenamedict = new Dictionary<string, string>() {
            {"nu","Nya universitet" },
            {"gu","Gamla universitet" },
            {"hh","Högskolor med bredd" },
            {"sh","Fackhögskolor" },
            {"su","Fackuniversitet" },
        };
        public static Dictionary<int, string> shortnamedict = new Dictionary<int, string>(); //abbreviated names for (Swedish) unis
        public static Dictionary<int, Tuple<double, double>> unilatlong = new Dictionary<int, Tuple<double, double>>(); //latitude/longitude for (Swedish) unis


        public static string list_sweuni()
        {
            StringBuilder sb = new StringBuilder();
            foreach (affclass ac in afflist)
            {
                if (ac.aliasof > 0)
                    continue;
                if (ac.orgtype == sweuni)
                    sb.Append(ac.name + "\t"+ac.id+"\n");
            }
            return sb.ToString();
        }
        public static affclass getaff(string namepar)
        {
            affclass aff = (from c in afflist where c.name == namepar select c).FirstOrDefault();
            if (aff != null)
            {
                if (aff.partof > 0)
                {
                    aff = (from c in afflist where c.id == aff.partof select c).FirstOrDefault();
                }
                else if (aff.aliasof > 0)
                {
                    aff = (from c in afflist where c.id == aff.aliasof select c).FirstOrDefault();
                }
                return aff;
            }

            return null;

        }

        public static void readaffs(string fn)
        {
            Console.WriteLine("Reading affiliations from " + fn);
            int n = 0;
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    affclass aff = new affclass();
                    aff.id = util.tryconvert(words[0]);
                    if (aff.id < 0)
                        continue;
                    aff.name = words[1];
                    aff.orgtype = util.tryconvert(words[3]);
                    if (words.Length > 4)
                        if (!String.IsNullOrEmpty(words[4]))
                            aff.partof = util.tryconvert(words[4]);
                    if (words.Length > 5)
                        if (!String.IsNullOrEmpty(words[5]))
                            aff.aliasof = util.tryconvert(words[5]);
                    afflist.Add(aff);
                    n++;
                }
            }
            Console.WriteLine(n + " affiliations read.");

        }

        public static void readuni(string fn)
        {
            Console.WriteLine("Reading universities from " + fn);
            int n = 0;
            using (StreamReader sr = new StreamReader(fn))
            {
                //sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 3)
                        continue;
                    unidict.Add(util.tryconvert(words[1]), words[0]);
                    unitypedict.Add(util.tryconvert(words[1]), words[2]);

                    n++;
                }
            }
            Console.WriteLine(n + " universities read.");
        }

        public static void readuni_latlong(string fn)
        {
            Console.WriteLine("Reading university positions from " + fn);
            int n = 0;
            using (StreamReader sr = new StreamReader(fn))
            {
                sr.ReadLine(); //throw away header
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split('\t');
                    if (words.Length < 5)
                        continue;
                    int id = util.tryconvert(words[2]);
                    shortnamedict.Add(id, words[1]);
                    double lat = util.tryconvertdouble(words[3]);
                    double lon = util.tryconvertdouble(words[4]);
                    unilatlong.Add(id, new Tuple<double, double>(lat,lon));
                    n++;
                }
            }
            Console.WriteLine(n + " universities read.");
        }



    }
}
