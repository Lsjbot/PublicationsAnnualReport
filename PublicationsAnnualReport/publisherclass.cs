using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicationsAnnualReport
{
    public class publisherclass
    {
        public string name = "";
        public List<string> altnames = new List<string>();
        public Dictionary<int, int?> norska = new Dictionary<int, int?>();
        public List<string> ISBNprefix = new List<string>();
        public string country = "";


        public bool match(string searchname, string ISBN)
        {
            if (!String.IsNullOrEmpty(ISBN))
            {
                foreach (string prefix in ISBNprefix)
                    if (ISBN.StartsWith(prefix))
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

        public bool match(string searchname, List<string> ISBNlist)
        {
            foreach (string ISBN in ISBNlist)
            {
                if (!String.IsNullOrEmpty(ISBN))
                {
                    foreach (string prefix in ISBNprefix)
                        if (ISBN.StartsWith(prefix))
                            return true;
                }
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
