using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace PublicationsAnnualReport
{
    public class pubclass
    {
        public static string titstring = "Title";
        public static string titstringscopus = "Document Title";
        public static string pubtypestring = "PublicationType";
        public static string authorstring = "Name";
        public static string authorstringscopus = "Authors";
        public static string pidstring = "PID";
        public static string contenttypestring = "ContentType";
        public static string yearstring = "Year";
        public static string yearstringscopus = "Publication Year";
        public static string statusstring = "Status";
        public static string journalstring = "Journal";
        public static string journalstringscopus = "Journal Title";
        public static string volumestring = "Volume";
        public static string issuestring = "Issue";
        public static string startpagestring = "StartPage";
        public static string endpagestring = "EndPage";
        public static string hostpubstring = "HostPublication";
        public static string publisherstring = "Publisher";
        public static string seriesstring = "Series";
        public static string doistring = "DOI";
        public static string confstring = "Conference";
        public static string scopusstring = "ScopusId";
        public static string eidstring = "EID";
        public static string isistring = "ISI";
        public static string abstractstring = "Abstract";
        public static string categorystring = "Categories";
        public static string researchsubjectstring = "ResearchSubjects";
        public static string keywordstring = "Keywords";
        public static string pissnstring = "JournalISSN";
        public static string eissnstring = "JournalEISSN";
        public static string issnstringscopus = "ISSN";
        public static string impactstring = "Journal impact";
        public static string norskastring = "Norska listan";
        public static string isbnstring = "ISBN";
        public static string contributorstring = "Contributors";


        public static string ctrefstring = "Refereegranskat";
        public static string ctpopstring = "Övrig (populärvetenskap, debatt, mm)";
        public static string ctscistring = "Övrigt vetenskapligt";

        public static string ptartstring = "Artikel i tidskrift";
        public static string ptkonfstring = "Konferensbidrag";
        public static string ptkapitelstring = "Kapitel i bok, del av antologi";
        public static string ptrecensionstring = "Artikel, recension";
        public static string ptthesis2string = "Doktorsavhandling, sammanläggning";
        public static string ptrapportstring = "Rapport";
        public static string ptbokstring = "Bok";
        public static string ptreviewstring = "Artikel, forskningsöversikt";
        public static string ptlic2string = "Licentiatavhandling, sammanläggning";
        public static string ptotherstring = "Övrigt";
        public static string ptlic1string = "Licentiatavhandling, monografi";
        public static string pteditorstring = "Samlingsverk (redaktörskap)";
        public static string ptthesis1string = "Doktorsavhandling, monografi";
        public static string ptprocstring = "Proceedings (redaktörskap)";
        public static string ptpreprintstring = "Manuskript (preprint)";

        public static string kthpidstring = "KTH_Doc_id";
        public static string kthdoctypestring = "KTH_Doc_type_code_rev";
        public static string kthyearstring = "KTH_Publication_year";
        public static string kthno_authorsstring = "KTH_No_authors";
        public static string kthUTstring = "KTH_UT";
        public static string kthsubjectstring = "KTH_Subject_category";
        public static string kthC_sciwostring = "KTH_C_sciwo";
        public static string kthC_scxwostring = "KTH_C_scxwo";
        public static string kthCf_sciwostring = "KTH_Cf_sciwo";
        public static string kthCf_scxwostring = "KTH_Cf_scxwo";
        public static string kthjcfstring = "KTH_Jcf";
        public static string kthjcf2string = "WoS JCF";
        public static string kthtop1string = "KTH_Top1_scxwo";
        public static string kthtop5string = "KTH_Top5_scxwo";
        public static string kthtop10string = "KTH_Top10_scxwo";
        public static string kthtop25string = "KTH_Top25_scxwo";

        public static string scopuscitationstring = "Scopus citations";
        public static string kthtopxstring = "Top X% in field";
        public static string kthcitationstring = "Web of science citations";
        public static string kthfieldnormstring = "Web of science field normalized citations";

        public static Dictionary<int, string> kthtopdict = new Dictionary<int, string>() { { 1, kthtop1string }, { 5, kthtop5string }, { 10, kthtop10string }, { 25, kthtop25string }};


        //public static string pubtypestring = "";
        //public static string pubtypestring = "";
        //public static string pubtypestring = "";
        //public static string pubtypestring = "";

        public static Dictionary<string, string> ausubjalias = new Dictionary<string, string>()
        {
            //{"Arabiska","Språk övrigt"},
            {"Arbetsvetenskap","Arbetsvetenskap"},
            {"Arbetsvetenskap -2019/NGL-C 2020-","Arbetsvetenskap"},
            {"Bild","Bild"},
            {"Bildproduktion","Media"},
            {"Byggteknik","Byggteknik"},
            {"Byggteknik; Energiteknik","Byggteknik; Energiteknik"},
            {"Datateknik","Datateknik"},
            {"Energi- och miljöteknik","Energiteknik"},
            {"Energiteknik","Energiteknik"},
            {"Energiteknik/Mikrodataanalys","Energiteknik;Mikrodataanalys"},
            {"Engelska","Engelska"},
            {"Engelska + Pedagogiskt arbete (2020-)","Pedagogiskt arbete"},
            {"Företagsekonomi","Företagsekonomi"},
            //{"Franska","Språk övrigt"},
            {"Fysik","Fysik och matematik"},
            {"Företagekonomi","Företagsekonomi" },
            {"Historia","Historia"},
            {"Idrotts- och hälsovetenskap","Idrotts- och hälsovetenskap"},
            {"Idrotts- och hälsovetenskap; Medicinsk vetenskap","Idrotts- och hälsovetenskap;Medicinsk vetenskap"},
            {"Industriell ekonomi","Industriell ekonomi"},
            {"Informatik","Informatik"},
            //{"Italienska","Språk övrigt"},
            //{"Japanska","Språk övrigt"},
            //{"Kinesiska","Språk övrigt"},
            {"Kulturgeografi","Turismvetenskap"},
            {"Kulturgeografi; Turismvetenskap","Turismvetenskap"},
            {"Ljud- och musikproduktion","Media"},
            {"Maskinteknik","Maskin och material"},
            {"Matematik","Fysik och matematik"},
            {"Matematikdidaktik","Matematikdidaktik"},
            {"Materialteknik","Maskin och material"},
            {"Materialvetenskap","Maskin och material"},
            {"Medicinsk vetenskap","Medicinsk vetenskap"},
            {"Mikrodataanalys","Mikrodataanalys"},
            {"Mikrodataanalys/Datateknik 2022-","Mikrodataanalys;Datateknik"},
            {"Nationalekonomi","Nationalekonomi"},
            {"Naturvetenskap","Naturvetenskap"},
            {"no subject",authorclass.nosubject},
            {"",authorclass.nosubject},
            {"Omvårdnad","Omvårdnad"},
            {"Pedagogik","Pedagogik"},
            {"Pedagogisk arbete","Pedagogiskt arbete"},
            {"Pedagogiskt arbete","Pedagogiskt arbete"},
            //{"Portugisiska","Språk övrigt"},
            {"Religionsvetenskap","Religionsvetenskap"},
            //{"Ryska","Språk övrigt"},
            {"Sexuell reproduktiv perinatal hälsa","Sexuell reproduktiv perinatal hälsa"},
            {"Sexuell, reproduktiv och perinatal hälsa","Sexuell reproduktiv perinatal hälsa"},
            {"Skog- och träteknik","Skog- och träteknik"},
            {"Socialt arbete","Socialt arbete"},
            {"Sociologi","Sociologi"},
            //{"Spanska","Språk övrigt"},
            {"Statistik","Statistik"},
            {"Statsvetenskap","Statsvetenskap"},
            {"Svenska som andraspråk","Svenska som andraspråk"},
            {"Svenska språket","Svenska"},
            {"Turismvetenskap","Turismvetenskap"},
            {"Vårdvetenskap","Vårdvetenskap"},

        };

        public static Dictionary<string, string> titcleandict = new Dictionary<string, string>()
        {{"<em>","" },
        {"</em>","" },
        {"<sub>","" },
        {"</sub>","" },
            {" : ",": " }
        };

        public Dictionary<string, string> dict = new Dictionary<string, string>();
        public List<authorclass> authors;
        public List<string> casauthors = new List<string>();
        public List<authorclass> contributors;
        public List<string> cascontributors = new List<string>();
        public authorclass firstHDa = new authorclass();
        public Dictionary<string, int> affgroupdict = new Dictionary<string, int>();
        public Dictionary<string, int> affgrouptypedict = new Dictionary<string, int>();
        public int year = -1;
        public string lowtitle = "";
        //public int? norska = null;
        //public double? impact = null;

        public void cleantitle()
        {
            string s;
            if (this.has(titstring))
                s = this.dict[titstring];
            else if (this.has(titstringscopus))
                s = this.dict[titstringscopus];
            else
                s = "(no title)";

            foreach (string bad in titcleandict.Keys)
                if (s.Contains(bad))
                    s = s.Replace(bad, titcleandict[bad]);
            s = s.Trim(new char[] { '.', ' ' });
            if (dict.ContainsKey(titstring))
                this.dict[titstring] = s;
            else
                this.dict.Add(titstring, s);
            this.lowtitle = s.ToLower();
        }

        public void fill_casauthors(List<authorclass> caslist)
        {
            //casauthors = new List<string>();
            foreach (authorclass ac in authors)
            {
                if (!String.IsNullOrEmpty(ac.CAS))
                {
                    if (!casauthors.Contains(ac.CAS))
                    {
                        casauthors.Add(ac.CAS);
                        authorclass casauth = (from c in caslist
                                               where c.CAS == ac.CAS
                                               select c).FirstOrDefault();
                        if (casauth != null)
                        {
                            ac.mergeaffiliations(casauth);
                            ac.name = casauth.name;
                        }
                    }
                }
            }
            if (contributors != null)
                foreach (authorclass ac in contributors)
                {
                    if (!String.IsNullOrEmpty(ac.CAS))
                    {
                        if (!cascontributors.Contains(ac.CAS))
                        {
                            cascontributors.Add(ac.CAS);
                            authorclass casauth = (from c in caslist
                                                   where c.CAS == ac.CAS
                                                   select c).FirstOrDefault();
                            if (casauth != null)
                            {
                                ac.mergeaffiliations(casauth);
                                ac.name = casauth.name;
                            }
                        }
                    }
                }
        }

        public void merge(pubclass pc2)
        {
            if (this.authors == null)
                this.authors = pc2.authors;
            else
                foreach (authorclass ac2 in pc2.authors)
                {
                    if (!this.authors.Contains(ac2))
                    {
                        this.authors.Add(ac2);
                        if (!String.IsNullOrEmpty(ac2.CAS))
                            this.casauthors.Add(ac2.CAS);
                    }
                }
            if (this.contributors == null)
                this.contributors = pc2.contributors;
            else
                foreach (authorclass ac2 in pc2.contributors)
                {
                    if (!this.contributors.Contains(ac2))
                    {
                        this.contributors.Add(ac2);
                        if (!String.IsNullOrEmpty(ac2.CAS))
                            this.cascontributors.Add(ac2.CAS);
                    }
                }
            foreach (string aff in pc2.affgroupdict.Keys)
            {
                if (!this.affgroupdict.ContainsKey(aff))
                {
                    this.affgroupdict.Add(aff, pc2.affgroupdict[aff]);
                    this.affgrouptypedict.Add(aff, pc2.affgrouptypedict[aff]);
                }
                else
                {
                    this.affgroupdict[aff] += pc2.affgroupdict[aff];
                    //Debug.Assert(this.affgrouptypedict[aff] == pc2.affgrouptypedict[aff]);
                }
            }

        }

        public void setyear()
        {
            if (has(yearstring))
            {
                try
                {
                    year = Convert.ToInt32(dict[yearstring]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else if (has(yearstringscopus))
            {
                try
                {
                    year = Convert.ToInt32(dict[yearstringscopus]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public string[] makeapa()
        {
            string[] returnstring = new string[3]; //string[0] before italic part, string[1] italic, string[2] after italic part

            StringBuilder apa = new StringBuilder();

            string separator = "";
            foreach (authorclass au in authors)
            {
                apa.Append(separator + au.initialname());
                separator = ", ";
            }
            apa.Append(" (" + year + "). ");
            //apa.Append(dict[pubclass.titstring]);
            //if (!dict[pubclass.titstring].EndsWith("."))
            //    apa.Append(".");

            if (dict.ContainsKey(journalstring) && !string.IsNullOrEmpty(dict[journalstring])) //italic journal
            {
                apa.Append(dict[pubclass.titstring]);
                if (!dict[pubclass.titstring].EndsWith("."))
                    apa.Append(".");
                returnstring[0] = apa.ToString();
                apa.Clear();
                string vol = dict.ContainsKey(volumestring) ? dict[volumestring] : "";
                //apa.Append(" "+dict[journalstring] + " " + vol);
                returnstring[1] = " " + dict[journalstring] + " " + vol;
                if (dict.ContainsKey(issuestring) && !string.IsNullOrEmpty(dict[issuestring]))
                    apa.Append("(" + dict[issuestring] + ")");
                else
                    apa.Append(", ");
                if (dict.ContainsKey(startpagestring) && !string.IsNullOrEmpty(dict[startpagestring]))
                    apa.Append(dict[startpagestring]);
                if (dict.ContainsKey(endpagestring) && !string.IsNullOrEmpty(dict[endpagestring]))
                    apa.Append("-" + dict[endpagestring]);

            }
            else if (dict.ContainsKey(hostpubstring) && !string.IsNullOrEmpty(dict[hostpubstring])) //italic host pub
            {
                apa.Append(dict[pubclass.titstring]);
                if (!dict[pubclass.titstring].EndsWith("."))
                    apa.Append(".");
                apa.Append(" In:");
                returnstring[0] = apa.ToString();
                apa.Clear();
                returnstring[1] = " "+dict[hostpubstring] + ".";
                //apa.Append(" In: " + dict[hostpubstring] + ".");
            }
            else //italic title
            {
                returnstring[0] = apa.ToString();
                apa.Clear();
                returnstring[1] = " "+dict[pubclass.titstring];
                if (!dict[pubclass.titstring].EndsWith("."))
                    returnstring[1]+= ".";
            }
            if (dict.ContainsKey(publisherstring) && !string.IsNullOrEmpty(dict[publisherstring]))
                apa.Append(" "+dict[publisherstring]+".");
            if (dict.ContainsKey(seriesstring) && !string.IsNullOrEmpty(dict[seriesstring]))
                apa.Append(" " + dict[seriesstring]+".");
            if (dict.ContainsKey(confstring) && !string.IsNullOrEmpty(dict[confstring]))
                apa.Append(" " + dict[confstring] + ".");
            if (dict.ContainsKey(doistring) && !string.IsNullOrEmpty(dict[doistring]))
                apa.Append(" DOI: " + dict[doistring]);
            if (this.has(pubclass.impactstring))
                apa.Append(" Journal impact (JCR) " + dict[pubclass.impactstring]);
            if (this.has(pubclass.norskastring))
                apa.Append(" Norska listan nivå " + dict[pubclass.norskastring]);

            returnstring[2] = apa.ToString();

            //return apa.ToString();
            return returnstring;
        }

        //public bool hasaffiliation(string aff)
        //{
        //    foreach (authorclass au in authors)
        //    {
        //        if (au.affiliation.Contains(aff))
        //            return true;
        //    }
        //    return false;
        //}

        public bool hasaffiliation(string aff, bool requirehda)
        {
            foreach (authorclass au in authors)
            {
                if (!requirehda || au.HDa_in_publication)
                    if (au.affiliation.Contains(aff))
                        return true;
            }
            return false;
        }

        public double fraction(string aff)
        {
            int nwith = 0;
            int ntot = 0;
            foreach (authorclass au in authors)
            {
                ntot++;
                foreach (string affil in au.affiliation)
                {
                    if (affil.Contains(aff))
                    {
                        nwith++;
                        break;
                    }
                }
            }
            if (nwith == 0)
                return 0;
            if (nwith == ntot)
                return 1;
            double frac = nwith / (double)ntot;
            if (frac < 0.1)
                frac = 0.1;
            return frac;
        }

        public bool has(string colid)
        {
            if (!dict.ContainsKey(colid))
                return false;
            if (String.IsNullOrEmpty(dict[colid]))
                return false;
            return true;
        }

        public List<string> parse_categories()
        {
            if (!has(categorystring))
                return new List<string>() { "(no category)" };
            string[] cats = dict[categorystring].Split(';');
            return cats.ToList();
        }

        public List<string> parse_researchsubject()
        {
            if (!has(researchsubjectstring))
                return new List<string>() { "(no category)" };
            string[] cats = dict[researchsubjectstring].Split(';');
            List<string> ls = new List<string>();
            foreach (string s in cats)
            {
                string[] w2 = s.Split(',');
                //string subj = "";
                List<string> lls = new List<string>();
                if (w2.Length == 1)
                    lls.Add(s);
                else
                {
                    for (int i = 0; i < w2.Length - 1; i++)
                    {
                        if (w2[i] == "Research Profiles 2009-2020")
                            continue;
                        int j = i + 1;
                        while (j < w2.Length && w2[j].StartsWith(" "))
                        {
                            w2[i] += w2[j];
                            w2[j] = "";
                            j++;
                        }
                        if (!String.IsNullOrEmpty(w2[i]))
                            lls.Add(w2[i]);
                    }
                }
                foreach (string ll in lls)
                {
                    string ll2 = translate(ll.Trim());
                    if (!ls.Contains(ll2))
                        ls.Add(ll2);
                }
            }
            return ls;

        }

        public List<string> get_authorsubject()
        {
            List<string> ls = new List<string>();
            foreach (authorclass aulocal in authors)
            {
                if (!aulocal.HDa_in_publication)
                    continue;
                if (!aulocal.isHDa())
                    continue;
                authorclass au = authorclass.findbyCAS(aulocal.CAS, Form1.caslist);
                if (au == null)
                    continue;
                string subj = au.getsubject();
                if (!ls.Contains(subj))
                    ls.Add(subj);
            }
            return ls;
        }

        public List<string> get_authorgroup()
        {
            List<string> ls = new List<string>();
            foreach (authorclass aulocal in authors)
            {
                if (!aulocal.HDa_in_publication)
                    continue;
                if (!aulocal.isHDa())
                    continue;
                authorclass au = authorclass.findbyCAS(aulocal.CAS, Form1.caslist);
                if (au == null)
                    continue;
                string group = au.getgroup();
                if (String.IsNullOrEmpty(group))
                    continue;
                if (!ls.Contains(group))
                    ls.Add(group);
            }
            return ls;
        }

        public List<string> get_authorinstitution(bool trueinst)
        {
            List<string> ls = new List<string>();
            foreach (authorclass aulocal in authors)
            {
                if (!aulocal.HDa_in_publication)
                    continue;
                if (!aulocal.isHDa())
                    continue;
                authorclass au = authorclass.findbyCAS(aulocal.CAS, Form1.caslist);
                if (au != null)
                {
                    string subj = au.getinstitution(trueinst);
                    if (!ls.Contains(subj))
                        ls.Add(subj);
                }
            }
            return ls;
        }

        public List<string> get_authorsubjectalias()
        {
            List<string> laus = get_authorsubject();
            List<string> ls = new List<string>();
            foreach (string s in laus)
            {
                string au = s;
                if (ausubjalias.ContainsKey(s))
                    au = ausubjalias[s];
                if (au == "x")
                    continue;
                //if (s == "Vårdvetenskap" && laus.Count > 1)
                //    continue;
                if (au.Contains(';'))
                {
                    foreach (string au2 in au.Split(';'))
                        if (!ls.Contains(au2.Trim()))
                            ls.Add(au2.Trim());
                }
                else
                {
                    if (!ls.Contains(au))
                        ls.Add(au);
                }
            }
            return ls;
        }

        public string get_issn()
        {
            if (has(pissnstring))
                return dict[pissnstring];
            else if (has(eissnstring))
                return dict[eissnstring];
            return "";
        }

        public string translate(string s)
        {
            switch (s)
            {
                case "Hälsa och välfärd":
                    return "Health and Welfare";
                case "Energi och samhällsbyggnad":
                    return "Energy and Built Environments";
                case "Interkulturella studier":
                    return "Intercultural Studies";
                case "Komplexa system - mikrodataanalys":
                    return "Complex Systems – Microdata Analysis";
                case "Stålformning och ytteknik":
                    return "Steel Forming and Surface Engineering";
                default:
                    return s;

            }
        }

        public void write_excelrow(Worksheet sheet, int row, Dictionary<string,int> hd)
        {
            foreach (string hh in dict.Keys)
            {
                if (hd.ContainsKey(hh))
                {
                    if (hh == pubclass.kthfieldnormstring)
                    {
                        double cf = util.tryconvertdouble(dict[hh]);
                        if (cf > 0)
                            sheet.Cells[row, hd[hh] + 1] = cf;
                    }
                    else
                        sheet.Cells[row, hd[hh] + 1] = dict[hh];
                }
            }

        }

    }

}
