using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicationsAnnualReport
{
    class WordCloudClass
    {
        private Dictionary<string, double> words = new Dictionary<string, double>();
        private List<WordEntryClass> items = new List<WordEntryClass>();

        public static double minfont = 12;
        public static double maxfont = 64;
        double minweight = Double.MaxValue;
        double maxweight = 0;
        double xsize;
        double ysize;
        double aspectratio;
        bool[,] taken;
        static double cellratio = 4;
        double cellsize = minfont / cellratio;
        int xcells;
        int ycells;
        int colorcontrast = 100000;
        float minh = 0;
        float maxh = 250;
        float mins = 0;
        float maxs = 99;
        float minv = 0;
        float maxv = 99;
        string shape = "circle";
        bool skipcentralsquare = false;
        bool titlespecial = false;
        SKColor backgroundcolor = new SKColor(225,182,191); //ljusrosa
        SKColor word1color = new SKColor(27,69,145);
        SKColor word2color = new SKColor(214,33,40);
        SKColor? wordcolor = null;

        public WordCloudClass(int xsizepar, int ysizepar, bool skipcentral, int contrast, bool titspecial)
        {
            xsize = xsizepar;
            ysize = ysizepar;
            aspectratio = ysize / xsize;
            colorcontrast = contrast;
            skipcentralsquare = skipcentral;
            titlespecial = titspecial;
            //if (skipcentralsquare)
            //    maxfont = maxfont / 2;

            xcells = (int)(xsize / cellsize);
            ycells = (int)(ysize / cellsize);
            taken = new bool[xcells, ycells];
        }

        public void SetShape(string shapepar)
        {
            shape = shapepar;
        }

        public void SetFontSize(int mnfont, int mxfont)
        {
            minfont = mnfont;
            maxfont = mxfont;
            cellsize = minfont / 2;
            xcells = (int)(xsize / cellsize);
            ycells = (int)(ysize / cellsize);
            taken = new bool[xcells, ycells];
        }


        public void SetSpecialColors(SKColor skc, SKColor w1c, SKColor w2c)
        {
            backgroundcolor = skc;
            word1color = w1c;
            word2color = w2c;
        }

        public void SetSpecificWordColor(SKColor wc)
        {
            wordcolor = wc;
        }

        public void SetColorRange(float minhue, float maxhue, float minsat, float maxsat, float minval, float maxval)
        {
            minh = minhue;
            maxh = maxhue;
            mins = minsat;
            maxs = maxsat;
            minv = minval;
            maxv = maxval;
        }

        public bool ColorOK(float h,float s, float v)
        {
            if (h < minh)
                return false;
            if (h > maxh)
                return false;
            if (s < mins)
                return false;
            if (s > maxs)
                return false;
            if (v < minv)
                return false;
            if (v > maxv)
                return false;
            return true;
        }

        public void Add(string word, double weight)
        {
            if (words.ContainsKey(word))
                words[word] += weight;
            else
                words.Add(word, weight);
        }

        public int Arrange()
        {
            foreach (string w in words.Keys)
            {
                if (words[w] < minweight)
                    minweight = words[w];
                if (words[w] > maxweight)
                    maxweight = words[w];
            }

            foreach (string w in words.Keys)
            {
                WordEntryClass we = new WordEntryClass();
                we.word = w;
                we.fontsize = getfont(words[w]);
                if (we.fontsize > maxfont)
                {
                    int dummy = 0;
                }
                items.Add(we);
            }

            int xmid = xcells / 2;
            int ymid = ycells / 2;
            if (shape == "ellipse")
            {
                int r2 = (int)((aspectratio * xmid * aspectratio * xmid) + (ymid * ymid)) / 2;
                for (int x = 0; x < xcells; x++)
                    for (int y = 0; y < ycells; y++)
                    {
                        if (aspectratio * (x - xmid) * aspectratio * (x - xmid) + (y - ymid) * (y - ymid) > r2)
                            taken[x, y] = true;
                        else
                            taken[x, y] = false;
                    }
            }
            else if (shape == "circle")
            {
                int r2 = (int)(Math.Min(xmid,ymid)*Math.Min(xmid,ymid));
                for (int x = 0; x < xcells; x++)
                    for (int y = 0; y < ycells; y++)
                    {
                        if ((x - xmid) * (x - xmid) + (y - ymid) * (y - ymid) > r2)
                            taken[x, y] = true;
                        else
                            taken[x, y] = false;
                    }

            }

            if (skipcentralsquare)
            {
                for (int x = xmid - xcells / 5; x < xmid + xcells / 5; x++)
                    for (int y = ymid - ycells / 5; y < ymid + ycells / 5; y++)
                        taken[x, y] = true;
            }



            int nfail = 0;
            int n = 0;
            foreach (WordEntryClass we in items.OrderByDescending(we => we.fontsize))
            {
                if (we.x > 0)
                    continue;
                n++;
                Tuple<int, int> pos = null;
                if (n <= 3 && titlespecial)
                {
                    if (skipcentralsquare)
                        continue;
                    else if (n == 1)
                    {
                        //pos = findspace(we, +1);
                        string w2 = we.word.Split().Last();
                        we.word = we.word.Replace(w2, "").Trim();
                        WordEntryClass we2 = we.Clone();
                        we2.word = w2;
                        
                        pos = new Tuple<int, int>(xcells / 4, ymid + ycells / 6);
                        var ws = wsize(we, n);
                        var pos2 = new Tuple<int, int>(pos.Item1 + ws.Item1 / 2,pos.Item2-ws.Item2);
                        we2.x = (int)(pos2.Item1 * cellsize);
                        we2.y = (int)(ysize - pos2.Item2 * cellsize);
                        set_taken(pos, ws);
                        var ws2 = wsize(we2, n);
                        set_taken(pos2,ws2);
                        items.Add(we2);
                    }
                    else if (n == 2)
                    {
                        string w2 = we.word.Split().Last();
                        we.word = we.word.Replace(w2, "").Trim();
                        WordEntryClass we2 = we.Clone();
                        we2.word = w2;
                        //pos = findspace(we, -1);
                        pos = new Tuple<int, int>(xcells / 3, ymid - ycells / 6);
                        var ws = wsize(we, n);
                        var pos2 = new Tuple<int, int>(pos.Item1 + ws.Item1 / 2, pos.Item2 - ws.Item2);
                        we2.x = (int)(pos2.Item1 * cellsize);
                        we2.y = (int)(ysize-pos2.Item2 * cellsize);
                        set_taken(pos, ws);
                        var ws2 = wsize(we2, n);
                        set_taken(pos2,ws2);
                        items.Add(we2);
                    }
                    else if (n == 3)
                    {
                        string w2 = we.word.Split().Last();
                        we.word = we.word.Replace(w2, "").Trim();
                        WordEntryClass we2 = we.Clone();
                        we2.word = w2;
                        //pos = findspace(we, -1);
                        pos = new Tuple<int, int>(xcells / 4, ymid);
                        var ws = wsize(we, n);
                        var pos2 = new Tuple<int, int>(pos.Item1, pos.Item2 - ws.Item2);
                        we2.x = (int)(pos2.Item1 * cellsize);
                        we2.y = (int)(ysize-pos2.Item2 * cellsize);
                        set_taken(pos, ws);
                        var ws2 = wsize(we2, n);
                        set_taken(pos2,ws2);
                        items.Add(we2);
                    }
                }
                else
                    pos = findspace(we, 0);
                if (pos == null)
                {
                    nfail++;
                    continue;
                }
                we.x = (int)(pos.Item1*cellsize);
                we.y = (int)(ysize-pos.Item2*cellsize);
            }
            return nfail;
        }

        //public int colordiff(SKColor c1, SKColor c2)
        //{
        //    int db = c1.Blue - c2.Blue;
        //    int dg = c1.Green - c2.Green;
        //    int dr = c1.Red - c2.Red;

        //    return db * db + dr * dr + dg * dg;
        //}

        public int colordiff(SKColor c1, byte blue, byte red, byte green)
        {
            int db = c1.Blue - blue;
            int dg = c1.Green - green;
            int dr = c1.Red - red;

            return db * db + dr * dr + dg * dg;
        }

        public int colordiff(SKColor c1,SKColor c2)
        {
            float h1;
            float s1;
            float v1;
            float h2;
            float s2;
            float v2;
            c1.ToHsv(out h1, out s1, out v1);
            c2.ToHsv(out h2, out s2, out v2);

            return (int)(2 * (h1 - h2) * (h1 - h2) + (s1 - s2) * (s1 - s2) + 3 * (v1 - v2) * (v1 - v2));
        }

        Random rnd = new Random();

        public SKColor getcolor()
        {
            if (wordcolor != null)
                return (SKColor)wordcolor;

            byte blue;
            byte red;
            byte green;
            int n = 0;
            SKColor newcolor;
            float h2;
            float s2;
            float v2;
            do
            {
                blue = (byte)rnd.Next(255);
                red = (byte)rnd.Next(254);
                green = (byte)rnd.Next(253);
                newcolor = new SKColor(red, green, blue);
                newcolor.ToHsl(out h2, out s2, out v2);
                n++;
            }
            while (n < 10000 && !ColorOK(h2, s2, v2));// && colordiff(backgroundcolor, newcolor) < colorcontrast);

            //Console.WriteLine("h,s,l:\t" + (int)h2 + "\t" + (int)s2 + "\t" + (int)v2 + "\t"+(int)n);
            return newcolor;
        }

        public SKPaint getpaint(WordEntryClass we,int n)
        {
            if (we.paint != null)
                return we.paint;

            SKPaint paint = new SKPaint();

            // the font manager finds fonts
            var fontManager = SKFontManager.Default;



            // or, we can try and make sure that we have the character, and hope it has the others
            char c = ' ';
            if (!String.IsNullOrEmpty(we.word))
                c = we.word[0];
            using (var roman = fontManager.MatchCharacter("Times New Roman", c))
            {
                paint.Typeface = roman;
                paint.TextSize = we.fontsize;
                paint.IsAntialias = true;
                paint.Color = getcolor();//new SKColor(ranbytes[colorbase], ranbytes[colorbase+1], ranbytes[colorbase+2]);
                if (!skipcentralsquare)
                {
                    if (n <= 3)
                    {
                        paint.Typeface = fontManager.MatchCharacter("Arial", SKFontStyle.Bold, null, c);

                    }
                    if (n == 1)
                    {
                        paint.Color = word1color;
                    }
                    else if (n == 2)
                        paint.Color = word2color;
                    else if (n == 3)
                    {
                        paint.Color = word1color;
                        paint.TextSize = paint.TextSize / 2;
                    }
                }

                //colorbase += 3;
                paint.IsStroke = false;
                //paint.StrokeWidth = 3;
                paint.TextAlign = SKTextAlign.Left;
                //textPaint.TextSize = (float)Sizer.GetFontSize(count);
                //SKRect rect = new SKRect();
                //paint.MeasureText(we.word, ref rect);
                //if (we.paint != null)
                we.paint = paint;
                return paint;
            }

        }

        public SKData Draw(string title)
        {
            using (var bitmap = new SKBitmap((int)xsize, (int)ysize))
            using (var canvas = new SKCanvas(bitmap))
            {

                // Draw on white background.
                canvas.Clear(backgroundcolor);
                //canvas.DrawBitmap(wcg.Draw(), 0, 0);

                //for (int x = 0; x < xcells; x++)
                //    for (int y = 0; y < ycells; y++)
                //    {
                //        if (taken[x, y])
                //            bitmap.SetPixel(x, y, SKColors.Red);
                //        else
                //            bitmap.SetPixel(x, y, SKColors.Green);

                //    }

                //byte[] ranbytes = new byte[3 * items.Count];
                //rnd.NextBytes(ranbytes);

                //int colorbase = 0;
                int n = 0;
                foreach (WordEntryClass we in items)
                {
                    n++;
                    if (we.x < 0)
                        continue;



                    SKPaint paint = getpaint(we,n);
                    canvas.DrawText(we.word, we.x, we.y, paint);
                    
                }

                // Add title.
                SKPaint titpaint = new SKPaint();
                titpaint.TextSize = 12.0f;
                titpaint.IsAntialias = true;
                titpaint.Color = new SKColor(0x9C, 0xAF, 0xB7);
                titpaint.IsStroke = false;
                titpaint.StrokeWidth = 3;
                titpaint.TextAlign = SKTextAlign.Left;
                string label = title;

                canvas.DrawText(label, 30, 30, titpaint);



                // Save to PNG.
                return bitmap.Encode(SKEncodedImageFormat.Png, 100);

            }
        }

        public Tuple<int,int> findspace(WordEntryClass we,int sign)
        {
            Random rnd = new Random();

            bool spacefound = false;
            int ntry = 0;
            int nmax = 4000;
            int x;
            int y;
            //SKPaint paint = new SKPaint();
            //SKRect rect = new SKRect();
            //// the font manager finds fonts
            //var fontManager = SKFontManager.Default;
            //// or, we can try and make sure that we have the character, and hope it has the others
            //char c = ' ';
            //if (!String.IsNullOrEmpty(we.word))
            //    c = we.word[0];
            //using (var arial = fontManager.MatchCharacter("Arial", c))
            //{
            //    paint.Typeface = arial;
            //    paint.TextSize = we.fontsize;
            //    paint.IsAntialias = true;
            //    paint.IsStroke = false;
            //    paint.TextAlign = SKTextAlign.Left;
            //    paint.MeasureText(we.word, ref rect);

            //}

            int wsizex;
            int wsizey;

            do
            {
                var ws = wsize(we, 999);
                wsizex = ws.Item1;// (int)(rect.Width / cellsize)+1;
                wsizey = ws.Item2;//(int)(rect.Height / cellsize)+1;

                if (wsizex >= xcells)
                    we.word = we.word.Substring(0, we.word.Length / 2) + "...";
            }
            while (wsizex >= xcells);

            do
            {
                ntry++;
                x = rnd.Next(xcells-wsizex);
                y = rnd.Next(ycells-wsizey);
                if (sign != 0)
                {
                    if ((y - ycells / 2) * sign < 0)
                        continue;
                }
                if (taken[x, y])
                    continue;

                bool free = true;
                for (int i = x; i < x + wsizex; i++)
                    for (int j = y; j < y + wsizey; j++)
                    {
                        if (taken[i, j])
                        {
                            free = false;
                            break;
                        }
                    }
                if (free)
                    spacefound = true;
            }
            while (!spacefound && ntry < nmax);

            if (spacefound)
            {
                if (sign != 0)
                    Console.WriteLine(we.word+" sign, y, ycells/2 " + sign + " " + y + " " + (ycells / 2));
                set_taken(x, y, wsizex, wsizey);
                return new Tuple<int, int>(x, y);
            }
            else
                return null;
        }
        private void set_taken(Tuple<int,int> pos, Tuple<int,int> ws)
        {
            set_taken(pos.Item1, pos.Item2, ws.Item1,ws.Item2);
        }

        private void set_taken(int x,int y, int wsizex, int wsizey)
        {
            for (int i = x; i < x + wsizex; i++)
                for (int j = y; j < y + wsizey; j++)
                    taken[i, j] = true;
        }

        private Tuple<int,int> wsize(WordEntryClass we, int n)
        {
            SKPaint paint = getpaint(we, n);// new SKPaint();
            SKRect rect = new SKRect();
            paint.MeasureText(we.word, ref rect);

            int wsizey = (int)(rect.Height / cellsize) + 1;
            int wsizex = (int)(rect.Width / cellsize) + 2;

            return new Tuple<int, int>(wsizex, wsizey);
        }

        public float getfont(double weight)
        {
            if (maxweight == minweight)
                return (float)(2*minfont);
            else
                return (float)(minfont + (weight - minweight) * (maxfont - minfont) / (maxweight - minweight));
        }


    }
}
