using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace PublicationsAnnualReport
{
    class WordEntryClass
    {
        public string word;
        public float fontsize;
        public int x = -1;
        public int y = -1;
        public SKPaint paint = null;

        public WordEntryClass Clone()
        {
            WordEntryClass we = new WordEntryClass();
            we.word = this.word;
            we.fontsize = this.fontsize;
            we.x = this.x;
            we.y = this.y;
            return we;
        }

    }
}
