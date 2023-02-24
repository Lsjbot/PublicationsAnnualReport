using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PublicationsAnnualReport
{
    public class authorclass
    {
        public static List<string> profiles = new List<string>();
        public static List<string> subjects = new List<string>();
        public static List<string> institutions = new List<string>();
        public static List<string> departments = new List<string>();
        public static Dictionary<string, string> subjinst = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> instsubj = new Dictionary<string, List<string>>();
        public static string nosubject = "(inget/annat ämne)";
        public static string noinstitution = "(ingen institution)";

        public static Dictionary<string, string> aliasdict = new Dictionary<string, string>() //from scopus/Wos to CAS
        {
            {"Paulsrud, B.","Yoximer Paulsrud, BethAnne" },
            {"von Ahlefeld Nisser, D.","Ahlefeld Nisser, Desirée von" },
            {"Paulsrud, B","Yoximer Paulsrud, BethAnne" },
            {"Paulsrud, BethAnne","Yoximer Paulsrud, BethAnne" },
            {"Nisser, Desiree von Ahlefeld","Ahlefeld Nisser, Desirée von" },
            {"von Ahlefeld Nisser, D","Ahlefeld Nisser, Desirée von" },
            {"Nisser, DV","Ahlefeld Nisser, Desirée von" },
            {"Aho, E.B.","Bomström Aho,Erika" },
            {"Aho, B.","Bomström Aho,Erika" },
            {"Aho, EB","Bomström Aho,Erika" },
            {"Aho, Erika","Bomström Aho,Erika" },
            {"Berge, L.F.","Berge, Lars" },
            {"Berge, LF","Berge, Lars" },
            {"Johansson, A.-M.","Johansson, Annie-Maj" },
            {"Johansson, AM","Johansson, Annie-Maj" },
            {"Äa&rnlöv, J.","Ärnlöv, Johan" },
            {"Äarnlöv, J.","Ärnlöv, Johan" },
            {"Arnlov, J.","Ärnlöv, Johan" },
            {"Arnlov, J","Ärnlöv, Johan" },
            {"Arnlov, Johan","Ärnlöv, Johan" },
            {"arnlov, Johan","Ärnlöv, Johan" },
            {"AErnloev, Johan","Ärnlöv, Johan" },
            {"Aernloev, Johan","Ärnlöv, Johan" },
            {"Hu, L.-L.","Hu, Lung-Lung" },
            {"Hu, L.L.","Hu, Lung-Lung" },
            {"Hu, LL","Hu, Lung-Lung" },
            {"Llena, C.Z.","Zamorano Llena, Carmen" },
            {"Llena, CZ","Zamorano Llena, Carmen" },
            {"Llena, Carmen Zamorano","Zamorano Llena, Carmen" },
            {"Klingberg Allvin, M.","Klingberg-Allvin, Marie" },
            {"Klingberg Allvin, M","Klingberg-Allvin, Marie" },
            {"Allvin, MK","Klingberg-Allvin, Marie" },
            {"Allvin, Marie Klingberg","Klingberg-Allvin, Marie" },
            {"De Bernardi, C.","de Bernardi, Cecilia" },
            {"De Bernardi, C","de Bernardi, Cecilia" },
            {"Höög, C.N.","Nyström Höög, Catharina" },
            {"Höög, CN","Nyström Höög, Catharina" },
            {"Hoog, CN","Nyström Höög, Catharina" },
            {"Hoog, Catharina Nystrom","Nyström Höög, Catharina" },
            {"Vegas, C.L.","Leon Vegas, Carolina" },
            {"Vegas, CL","Leon Vegas, Carolina" },
            {"Hauksson-Tresch, N.","Hauksson Tresch, Nathalie" },
            {"Hauksson-Tresch, N","Hauksson Tresch, Nathalie" },
            {"Velasco, M.H.","Hernandez Velasco, Marco" },
            {"Velasco, MH","Hernandez Velasco, Marco" },
            {"Nordlander, M.C.","Cortas Nordlander, Maria" },
            {"Nordlander, MC","Cortas Nordlander, Maria" },
            {"Adel, Annelie","Ädel, Annelie" },
            {"Cassel, SH","Heldt Cassel, Susanna" },
            {"Cassel, S.H.","Heldt Cassel, Susanna" },
            {"Cassel, Susanna Heldt","Heldt Cassel, Susanna" },
            {"Grunden, H","Grundén, Helena" },
            {"Grunden, H.","Grundén, Helena" },
            {"Fahlstrom, M","Fahlström, Magnus" },
            {"Fahlstrom, Magnus","Fahlström, Magnus" },
            {"Badiella, CM","Magrinyà Badiella, Carles" },
            {"Badiella, C.M.","Magrinyà Badiella, Carles" },
            {"Molnar, D","Molnár, David" },
            {"Molnar, David","Molnár, David" },
            {"Molnar, D.","Molnár, David" },
            {"Ronnegard, L","Rönnegård, Lars" },
            {"Ronnegard, Lars","Rönnegård, Lars" },
            {"Roennegard, L","Rönnegård, Lars" },
            {"Roennegard, Lars","Rönnegård, Lars" },
            {"Engstrom, MS","Svedbo Engström, Maria" },
            {"Stromsoe, Anneli","Strömsöe, Anneli" },
            {"Stromsoe, A","Strömsöe, Anneli" },
            {"Stromsoe, Annelie","Strömsöe, Anneli" },
            {"Klimplova, Lenka","Klimplová, Lenka" },
            {"Ohrn, K","Öhrn, Kerstin" },
            {"Hakansson, Johan","Håkansson, Johan" },
            {"Hakansson, Jan","Håkansson, Jan" },
            {"Zhang, XX","Zhang, Xingxing" },
            {"Xingxing, Z.","Zhang, Xingxing" },
            {"Aberg, Jenny","Åberg, Jenny" },
            {"Wallin, AE","Eilegård Wallin, Alexandra" },
            {"Gal, CV","Gál, Csilla" },
            {"Ostman, JO","Östman, Jan-Ola" },
            {"Palsson, Ale","Pålsson, Ale" },
            {"Goransson, KE","Göransson, Katarina" },
            {"Nordstroem, J","Nordström, Jonas" },
            {"Michaelsson, Madeleine","Michaëlsson, Madeleine" },
            {"Moller, Peter","Möller, Peter" },
            {"Hulten, Peter","Hultén, Peter" },
            {"Jon-and, Anna","Jon-And, Anna" },
            {"Jader, J","Jäder, Jonas" },
            {"Jonsson, Birgitta","Jönsson, Birgitta" },
            {"Lindstrom, Anders","Lindström, Anders" },
            {"Swaren, Mikael","Swarén, Mikael" },
            {"Powar, S","Satvasheel Powar" },
            {"Powar, S.","Satvasheel Powar" },
            {"Wikstrom, Daniel","Wikström, Daniel" },
            {"Osterling, SR","Reyier Österling, Sofia" },
            {"Gunilla, L","Lindqvist, Gunilla" },
            {"Gunilla, L.","Lindqvist, Gunilla" },
            {"Aberg, Anna Cristina","Åberg, Anna Cristina" },
            {"Nilsson, JE","Nilsson, Johnny" },
            {"Engstrom, G","Engström, Gabriella" },
            {"Nordstrom, J","Nordström, Jonas" },
            {"Selven, Sebastian","Selvén, Sebastian" },
            {"Jerden, L","Jerdén, Lars" },
            {"Mevludin, M","Memedi, Mevludin" },
            {"Mevludin, M.","Memedi, Mevludin" },
            {"Wallin, A.E.","Eilegård Wallin, Alexandra" },
            {"Swenberg, Thorbjorn","Swenberg, Thorbjörn" },
            {"Wedin, Asa","Wedin, Åsa" },
            {"Rosen, Jenny","Rosén, Jenny" },
            {"Elbe, Jorgen","Elbe, Jörgen" },
            {"Aho, Erika Bomstrom","Bomström Aho,Erika" },
            {"Grunden, Helena","Grundén, Helena" },
            {"Hu Lung-Lung","Hu, Lung-Lung" },
            {"Hu Lung Lung","Hu, Lung-Lung" },
            {"Hu, Lung Lung","Hu, Lung-Lung" },
            {"Nordlander, Maria Cortas","Cortas Nordlander, Maria" },
            {"Hauksson-Tresch, Nathalie","Hauksson Tresch, Nathalie" },
            {"Ohrn, Kerstin","Öhrn, Kerstin" },
            {"Velasco, Marco Hernandez","Hernandez Velasco, Marco" },
            {"De Majo, Veronica","de Majo, Veronica" },
            {"Yachin, Jonathan Moshe","Yachin, Jonathan M" },
            {"Jansson, Peter Martin","Jansson, Peter M" },
            {"Ronnelid, Mats","Rönnelid, Mats" },
            {"Zhang Xingxing","Zhang, Xingxing" },
            {"Back, Jessica Gyhlesten","Gyhlesten Back, Jessica" },
            {"Hammar, Lena Marmstal","Marmstål Hammar, Lena" },
            {"Rocher Hahlin, Celine","Rocher Hahlin, Céline" },
            {"Flacking, Renee","Flacking, Renée" },
            {"Byrskog, Ulrika","Byrskog, Ulrica" },
            {"Engstrom, Maria Svedbo","Svedbo Engström, Maria" },
            //{"","" },
            //{"","" },
            //{"","" },
            //{"","" },


            //{"","" },
        };


        public string name=""; //Last, First
        public string CAS="";
        public string orcid="";
        public List<string> affiliation=new List<string>();
        public string subject = "";
        public string subjectalias = "";
        public string profile = "";
        public string institution = "";
        public string originst = ""; //save original institution if virtual institution created
        public string department = "";
        public string specialgroup = "";
        public char gender = 'X';
        public string notes = "";
        public bool HDa_in_publication = false;

        public Dictionary<int, int> pubyear = new Dictionary<int, int>() { { 2010, 0 }, { 2011, 0 }, { 2012, 0 }, { 2013, 0 }, { 2014, 0 }, { 2015, 0 }, { 2016, 0 }, { 2017, 0 }, { 2018, 0 }, { 2019, 0 }, { 2020, 0 }, { 2021, 0 }, { 2022, 0 }, { 2023, 0 } }; 

        public static List<string> fill_instsubj(List<authorclass> caslist)
        {
            List<string> ls = new List<string>();
            if (instsubj.Count > 0)
                return ls;

            Dictionary<string, Dictionary<string, int>> isdict = new Dictionary<string, Dictionary<string, int>>();
            foreach (authorclass ac in caslist)
            {
                string inst = ac.getinstitution(false);
                if (!instsubj.ContainsKey(inst))
                    instsubj.Add(inst, new List<string>() { nosubject });
                string subj = ac.getsubjectalias();
                if (subj == "kultur och samhälle")
                {
                    int dummy = 0;
                }

                if (!isdict.ContainsKey(subj))
                    isdict.Add(subj, new Dictionary<string, int>());
                if (!isdict[subj].ContainsKey(inst))
                    isdict[subj].Add(inst, 1);
                else
                    isdict[subj][inst]++;
            }
            foreach (string subj in isdict.Keys)
            {
                string inst;
                if (isdict[subj].Count == 1)
                    inst = isdict[subj].Keys.First();
                else
                {
                    int max = -1;
                    string imax = "";
                    foreach (string ins in isdict[subj].Keys)
                    {
                        if (isdict[subj][ins] > max)
                        {
                            max = isdict[subj][ins];
                            imax = ins;
                        }
                    }
                    inst = imax;
                }
                subjinst.Add(subj, inst);
                if (!instsubj[inst].Contains(subj))
                    instsubj[inst].Add(subj);
                if (pubclass.ausubjalias.ContainsKey(subj))
                {
                    if (!instsubj[inst].Contains(pubclass.ausubjalias[subj]))
                        instsubj[inst].Add(pubclass.ausubjalias[subj]);
                }
            }

            foreach (authorclass ac in caslist)
            {
                string inst = ac.getinstitution(false);
                string subj = ac.getsubjectalias();
                if (subj == nosubject)
                    continue;
                if (inst != subjinst[subj])
                {
                    Console.WriteLine(ac.name + "\t" + inst + "\t" + subj);
                    //ls.Add(ac.name + "\t" + inst + "\t" + subj);
                    //ac.subject = nosubject;
                    //ac.subjectalias = nosubject;
                }
            }
            return ls;
        }
        
        public static string namefromCAS(string cas, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.CAS == cas
                    select c.reversename();
            if (q.Count() == 1)
                return q.First();
            else
                return cas;

        }

        public static authorclass findbyname(string name,List<authorclass> authorlist)
        {
            if (aliasdict.ContainsKey(name))
                return findbyname(aliasdict[name], authorlist);
            var q1 = from c in authorlist where c.name == name select c;
            if (q1.Count() == 1)
                return q1.First();
            var q2 = from c in authorlist where c.matchinitials(name) select c;
            if (q2.Count() == 1)
                return q2.First();
            //var q3 = from c in authorlist where c.name.StartsWith(name.Split(',')[0]) select c;
            //if (q3.Count() == 1)
            //    return q3.First();
            return null;
        }

        public string getinstitution(bool trueinst) //trueinst return "originst" if true, "institution" if false.
        {
            if (trueinst)
            {
                if (!String.IsNullOrEmpty(originst))
                    return originst;
                else
                    return getinstitution(false);
            }
            else
            {
                if (!String.IsNullOrEmpty(institution))
                    return institution;
                foreach (string aff in affiliation)
                    if (institutions.Contains(aff))
                        return aff.ToLower();
            }
            return noinstitution;
        }

        public void changeinstitution(string newinst)
        {
            institution = newinst;
            for (int i=0;i<affiliation.Count;i++)
                if (institutions.Contains(affiliation[i]))
                {
                    affiliation[i] = newinst;
                    return;
                }
        }

        public static authorclass authorfromname(string name, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.name == name
                    select c;
            if (q.Count() == 1)
                return q.First();
            else
                return null;

        }

        public static string profilefromCAS(string cas, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.CAS == cas
                    select c;
            if (q.Count() == 1)
            {
                foreach (string aff in q.First().affiliation)
                {
                    if (profiles.Contains(aff))
                        return aff;
                }
                return "no profile";
            }
            else
                return "not found";

        }

        public static string institutionfromCAS(string cas, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.CAS == cas
                    select c;
            if (q.Count() == 1)
            {
                foreach (string aff in q.First().affiliation)
                {
                    if (institutions.Contains(aff))
                        return aff;
                }
                return "no institution";
            }
            else
                return "not found";

        }

        public static authorclass findbyCAS(string cas, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.CAS == cas
                    select c;
            if (q.Count() == 1)
            {
                return q.First();
                //foreach (string aff in q.First().affiliation)
                //{
                //    if (subjects.Contains(aff))
                //        return aff;
                //}
                //return "no subject";
            }
            else
                return null;

        }


        public static string subjectfromCAS(string cas, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.CAS == cas
                    select c;
            if (q.Count() == 1)
            {
                return q.First().getsubject();
                //foreach (string aff in q.First().affiliation)
                //{
                //    if (subjects.Contains(aff))
                //        return aff;
                //}
                //return "no subject";
            }
            else
                return "no subject";

        }

        public string getsubject()
        {
            if (!String.IsNullOrEmpty(subject))
                return subject;
            foreach (string aff in affiliation)
            {
                if (subjects.Contains(aff))
                    return aff;
            }
            return "no subject";
        }

        public string getgroup()
        {
            return this.specialgroup;
        }

        public string getsubjectalias()
        {
            if (!String.IsNullOrEmpty(subjectalias))
                return subjectalias;
            string s = getsubject();
            if (pubclass.ausubjalias.ContainsKey(s))
                s = pubclass.ausubjalias[s];
            return s;
        }

        public void addgroup(string group, bool makevirtinst)
        {
            this.specialgroup = group;
            this.affiliation.Add(group);
            if (makevirtinst)
            {
                this.originst = this.institution;
                this.institution = group;
                if (!institutions.Contains(group))
                    institutions.Add(group);
                if (!instsubj.ContainsKey(group))
                {
                    instsubj.Add(group, new List<string>());
                }
                if (!instsubj[group].Contains(this.getsubjectalias()))
                    instsubj[group].Add(this.getsubjectalias());

            }
        }

        public static List<authorclass> getbysubject(string subject, List<authorclass> aulist)
        {
            var q = from c in aulist
                    where c.getsubjectalias() == subject
                    select c;
            return q.ToList();
        }


        public static List<authorclass> parseauthors(string s)
        {
            List<authorclass> ls = new List<authorclass>();

            //"Aartsen, Marja;Valtorta, Nicole;Dahlberg, Lena [ldh] [0000-0002-7685-3216] (Högskolan Dalarna [7306], Akademin Utbildning, hälsa och samhälle [15900], Socialt arbete [9427]);van Regenmortel, Sofie;Waldegrave, Charles;Corrigan, Trudy"
            string[] aa = s.Split(';');

            string casrex = @"\[([a-z]{3,4})\]";
            string affrex = @"\((.+?)\)";

            foreach (string au in aa)
            {
                authorclass ac = new authorclass();
                ac.name = au.Split(new char[] { '(', '[' })[0];
                foreach (Match m in Regex.Matches(au, casrex))
                {
                    ac.CAS = m.Groups[1].Value;
                }
                foreach (Match m in Regex.Matches(au, affrex))
                {
                    ac.affiliation.Add(m.Groups[1].Value.Trim());
                }
                ls.Add(ac);
            }

            return ls;
        }

        public void mergeaffiliations(authorclass ac2)
        {
            foreach (string aff in ac2.affiliation)
                if (!this.affiliation.Contains(aff))
                    this.affiliation.Add(aff);
            if (String.IsNullOrEmpty(this.subject))
                this.subject = ac2.subject;
            if (String.IsNullOrEmpty(this.subjectalias))
                this.subjectalias = ac2.subjectalias;
            if (String.IsNullOrEmpty(this.profile))
                this.profile = ac2.profile;
            if (String.IsNullOrEmpty(this.institution))
                this.institution = ac2.institution;
        }

        public string reversename() //Xxx, Yyy -> Yyy Xxx
        {
            if (!this.name.Contains(","))
                return this.name;
            string[] ss = this.name.Split(',');
            string lastname = ss[0];
            string firstname = ss[1].Trim();
            return firstname + " " + lastname;
        }

        public string initialname()
        {
            return initialname("Y. Z.");
        }

        public string initialname(string itype) // "Xxx, Yyy Zzz" -> "Xxx, Y. Z." formatted according to itype
        {
            if (!this.name.Contains(","))
                return this.name;

            string[] ss = this.name.Split(',');
            string lastname = ss[0];
            string initials = "";
            string afterchar = "";
            if (itype.Contains("."))
                afterchar = ".";
            string betweenchar = " ";
            if (!itype.Contains(" "))
            {
                if (itype.Contains("-"))
                    betweenchar = "-";
                else
                    betweenchar = "";
            }
            for (int i = 1; i < ss.Length; i++)
            {
                if (String.IsNullOrEmpty(ss[i]))
                    continue;
                string firstname = ss[i].Trim();
                string[] fn = firstname.Split();
                foreach (string fnn in fn)
                    if (!String.IsNullOrEmpty(fnn))
                        initials += fnn.Substring(0, 1) + afterchar + betweenchar;
            }
            if (!String.IsNullOrEmpty(betweenchar))
                initials = initials.Trim(betweenchar.ToCharArray());
            return lastname + ", " + initials;
        }

        public bool matchinitials(string iname) //match name against "Xxx, Y Z", "Xxx, Y.Z.", "Xxx, YZ", "Xxx, Y. Z."
        {
            string[] initialtypes = new string[] { "Y. Z.", "Y.Z.", "Y Z", "YZ" };
            foreach (string itype in initialtypes)
            {
                if (iname == initialname(itype))
                    return true;
            }
            return false;
        }

        public static authorclass getfirstHDa(List<authorclass> ls)
        {
            foreach (authorclass ac in ls)
            {
                if (!String.IsNullOrEmpty(ac.CAS) && ac.HDa_in_publication)
                    return ac;
                //if (!String.IsNullOrEmpty(ac.CAS) && ac.affiliation.Count > 0)
                //{
                //    foreach (string aff in ac.affiliation)
                //        if (aff.Contains("Dalarna"))
                //            return ac;
                //}
            }
            return null;
        }

        public bool isHDa()
        {
            if (!String.IsNullOrEmpty(this.CAS) && this.affiliation.Count > 0)
            {
                foreach (string aff in this.affiliation)
                    if (aff.Contains("Dalarna"))
                        return true;
            }
            return false;

        }

        public bool CASbutnotHDa()
        {
            if (string.IsNullOrEmpty(CAS))
                return false;
            if (isHDa())
                return false;
            return true;
        }
    }

}
