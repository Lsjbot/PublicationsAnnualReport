using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicationsAnnualReport
{
    public class journalclass
    {
        public string name = "";
        public List<string> altnames = new List<string>();
        public int rank;
        public int citations;
        public double impact;
        public Dictionary<int,int?> norska = new Dictionary<int, int?>();
        public string printISSN = "";
        public string eISSN = "";
        public string NPIfield = "";
        public string NPIdiscipline = "";

        public bool match(string searchname,string ISSN)
        {
            if (!String.IsNullOrEmpty(ISSN))
            {
                if (printISSN == ISSN)
                    return true;
                if (eISSN == ISSN)
                    return true;
            }
            if (!String.IsNullOrEmpty(searchname))
            {
                if (name == searchname)
                    return true;
                if (altnames.Contains(searchname))
                    return true;
            }
            return false;
        }

        public bool match(string searchname, string pISSN, string onlineISSN)
        {
            if (!String.IsNullOrEmpty(pISSN))
            {
                if (this.printISSN == pISSN)
                    return true;
                if (this.eISSN == pISSN)
                    return true;
            }
            if (!String.IsNullOrEmpty(onlineISSN))
            {
                if (this.eISSN == onlineISSN)
                    return true;
                if (this.printISSN == onlineISSN)
                    return true;
            }
            if (!String.IsNullOrEmpty(searchname))
            {
                return matchname(searchname);
            }
            return false;
        }

        public bool matchname(string searchname)
        {
            if (name == searchname)
                return true;
            if (altnames.Contains(searchname))
                return true;

            return false;
        }

        public void addname(string newname)
        {
            if (newname == null)
                return;
            if (newname == name)
                return;
            if (String.IsNullOrEmpty(name))
                name = newname;
            else
            {
                if (!altnames.Contains(newname))
                    altnames.Add(newname);
            }
        }

    }
}
