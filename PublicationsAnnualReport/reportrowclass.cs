using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicationsAnnualReport
{
    class reportrowclass
    {
        public string rowtitle = "";
        public List<string> pubtypes = new List<string>();
        public List<string> contenttypes = new List<string>();
        //public int refstatus = 0; //0 = all, 1 = ref only, -1 = nonref only, -2 = pop-sci only
        public bool pubonly = false; //only published or everything
        public bool nosubacc = true; //skip submitted and accepted
    }
}
