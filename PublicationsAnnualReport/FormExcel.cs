using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PublicationsAnnualReport
{
    public partial class FormExcel : Form
    {
        Dictionary<string, int> hd = new Dictionary<string, int>(); //header of Diva file
        Dictionary<string, CheckBox> cbdict = new Dictionary<string, CheckBox>();

        public FormExcel(Dictionary<string, int> hdpar)
        {
            InitializeComponent();
            hd = hdpar;

            int x0 = 5;// GBinst.Location.X;
            int y0 = 5;// GBinst.Location.Y;
            int pitch = 25;

            int n = 1;
            foreach (string inst in authorclass.institutions)
            {
                CheckBox cb = new CheckBox();
                cb.Location = new Point(x0 + 5, y0 + n * pitch);
                cb.Width = GBinst.Width - 10;
                cb.Text = inst;
                cb.Name = "CB" + inst;
                cb.Visible = true;
                cb.Enabled = true;
                cb.Checked = true;
                cb.Show();
                GBinst.Controls.Add(cb);
                cbdict.Add(inst, cb);
                n++;
            }
        }

        public void memo(string s)
        {
            richTextBox1.AppendText(s + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void SheetWithHeader(Excel.Worksheet sheet, int datarows)
        {
            for (int i = 0; i <= datarows; i++)
                sheet.Rows.EntireRow.Insert(Excel.XlInsertShiftDirection.xlShiftDown, false);
            foreach (string hh in hd.Keys)
            {
                sheet.Cells[1, hd[hh] + 1] = hh;
            }
            Excel.Range qa = sheet.Columns[hd[pubclass.authorstring] + 1];
            qa.ColumnWidth = 50;
            Excel.Range qt = sheet.Columns[hd[pubclass.titstring] + 1];
            qt.ColumnWidth = 50;
            //sheet.Columns[pubclass.titstring + 1].ColumnWidth = 300;
            //sheet.Cells[1, pubclass.titstring + 1].EntireColumn.ColumnWidth = 300;
            //Excel.Range titcol = ((Excel.Range)sheet.Cells[1, pubclass.titstring+1]).EntireColumn;
            //titcol.ColumnWidth = 300;
            //Excel.Range aucol = ((Excel.Range)sheet.Cells[1, pubclass.authorstring + 1]).EntireColumn;
            //titcol.ColumnWidth = 400;
        }

        private string validsheetname(string catpar, List<string> sheetnames)
        {
            char[] nono = new char[] { ':', '\\', '/', '?', '*', '[', ']' };
            string cat = catpar;
            foreach (char c in nono)
                cat = cat.Replace(c, '-');

            if (cat == "History")
                cat = "History.";

            if (cat.Length > 30)
            {
                string[] w = cat.Split('(');
                if (w.Length == 2 && w[0].Length > 24 && w[1].Length < 24)
                {
                    cat = w[0].Substring(0, 24 - w[1].Length) + "... (" + w[1];
                }
                else
                {
                    cat = cat.Substring(0, 27) + "...";
                }
            }

            int i = 1;
            while (sheetnames.Contains(cat))
            {
                cat = cat.Substring(0, cat.Length - i.ToString().Length) + i.ToString();
                i++;
            }
            return cat;
        }

        private void AddExcelTabDiva(Excel.Workbook xl, Dictionary<string, Excel.Worksheet> sheetdict, SortedDictionary<string, List<pubclass>> dict, string dictkey, List<string> sheetnames, int maxcount, string inst)
        {
            memo(dictkey);
            if (!dict.ContainsKey(dictkey))
            {
                memo("no entry for " + dictkey);
                return;
            }
            var q = from c in dict[dictkey] select c;
            if (inst != hdainst)
            {
                q = from c in q
                        where c.get_authorinstitution(RB_originstfile.Checked).Contains(inst)
                        select c;
            }
            if (q.Count() == 0)
            {
                memo("no publications from "+inst+" for " + dictkey);
                return;
            }
            Excel.Worksheet sheet = (Excel.Worksheet)xl.Sheets.Add();
            sheet.Name = validsheetname(dictkey, sheetnames);
            sheetnames.Add(sheet.Name);
            SheetWithHeader(sheet, q.Count());
            sheetdict.Add(dictkey, sheet);
            int publine = 1;
            foreach (pubclass pc in q)
            {
                if (dictkey == authorclass.nosubject && !pc.get_authorinstitution(RB_originstfile.Checked).Contains(inst))
                    continue;
                publine++;
                pc.write_excelrow(sheet, publine, hd);
                if (publine > maxcount)
                    break;
            }

        }

        string hdainst = "HDa";

        private void Excelbutton_Click(object sender, EventArgs e)
        {
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();

            //DialogResult dr = openFileDialog1.ShowDialog();
            //if (dr != DialogResult.OK)
            //    return;

            //string folder = @"C:\Users\sja\OneDrive - Högskolan Dalarna\Dokument\Invärld\";
            string folderbase = "DIVA per institution";
            if (RB_HDafile.Checked)
                folderbase = "DIVA HDa";
            string folder = util.timestampfolder(@"C:\Temp\", folderbase);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            //string folder = @"C:\users\Lsj\Documents\Invärld\Könsfördelning publikationer\";
            //memo("Reading " + openFileDialog1.FileName);

            string fncat = util.freefilename(folder + "Diva by Category Scopus.xlsx");
            //Excel.Workbook xlWcat = xlApp.Workbooks.Add();
            //Dictionary<string, Excel.Worksheet> catsheetdict = new Dictionary<string, Excel.Worksheet>();
            SortedDictionary<string, List<pubclass>> catpubdict = new SortedDictionary<string, List<pubclass>>();

            string fnsubj = util.freefilename(folder + "Diva by ResearchSubject Scopus.xlsx");
            //Excel.Workbook xlWsubj = xlApp.Workbooks.Add();
            //Dictionary<string, Excel.Worksheet> subjsheetdict = new Dictionary<string, Excel.Worksheet>();
            SortedDictionary<string, List<pubclass>> subjpubdict = new SortedDictionary<string, List<pubclass>>();
            //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

            string fnausubj = util.freefilename(folder + "Diva by AuthorSubject.xlsx");
            //Excel.Workbook xlWausubj = xlApp.Workbooks.Add();
            //Dictionary<string, Excel.Worksheet> ausubjsheetdict = new Dictionary<string, Excel.Worksheet>();
            SortedDictionary<string, List<pubclass>> ausubjpubdict = new SortedDictionary<string, List<pubclass>>();
            //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

            SortedDictionary<string, List<pubclass>> augrouppubdict = new SortedDictionary<string, List<pubclass>>();

            string fnauinst = util.freefilename(folder + "Diva by AuthorInstitution.xlsx");
            Excel.Workbook xlWauinst = xlApp.Workbooks.Add();
            Dictionary<string, Excel.Worksheet> auinstsheetdict = new Dictionary<string, Excel.Worksheet>();
            SortedDictionary<string, List<pubclass>> auinstpubdict = new SortedDictionary<string, List<pubclass>>();
            //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];

            Dictionary<string, string> fninst = new Dictionary<string, string>();
            Dictionary<string, Excel.Workbook> xldict = new Dictionary<string, Excel.Workbook>();
            Dictionary<string, Dictionary<string, Excel.Worksheet>> sheetdictdict = new Dictionary<string, Dictionary<string, Excel.Worksheet>>();

            if (RB_HDafile.Checked)
            {
                //string inst = "HDa";
                fninst.Add(hdainst, util.freefilename(folder + "DIVA " + hdainst + ".xlsx"));
                Excel.Workbook xl = xlApp.Workbooks.Add();
                xldict.Add(hdainst, xl);
                sheetdictdict.Add(hdainst, new Dictionary<string, Excel.Worksheet>());

            }
            else
            {
                foreach (string inst in authorclass.institutions)
                {
                    if (!cbdict[inst].Checked)
                        continue;
                    fninst.Add(inst, util.freefilename(folder + "DIVA " + inst + ".xlsx"));
                    Excel.Workbook xl = xlApp.Workbooks.Add();
                    xldict.Add(inst, xl);
                    sheetdictdict.Add(inst, new Dictionary<string, Excel.Worksheet>());
                }
            }


            foreach (pubclass pc in Form1.publist)
            {
                if (pc.has(pubclass.scopusstring))
                {
                    //    continue;

                    foreach (string cat in pc.parse_categories())
                    {
                        if (!catpubdict.ContainsKey(cat))
                            catpubdict.Add(cat, new List<pubclass>());
                        catpubdict[cat].Add(pc);
                    }
                    foreach (string rs in pc.parse_researchsubject())
                    {
                        if (!subjpubdict.ContainsKey(rs))
                            subjpubdict.Add(rs, new List<pubclass>());
                        subjpubdict[rs].Add(pc);
                    }
                }

                foreach (string aus in pc.get_authorsubjectalias())
                {
                    if (!ausubjpubdict.ContainsKey(aus))
                        ausubjpubdict.Add(aus, new List<pubclass>());
                    ausubjpubdict[aus].Add(pc);
                }

                foreach (string aus in pc.get_authorgroup())
                {
                    if (!augrouppubdict.ContainsKey(aus))
                        augrouppubdict.Add(aus, new List<pubclass>());
                    augrouppubdict[aus].Add(pc);
                }

                foreach (string aus in pc.get_authorinstitution(RB_originstfile.Checked))
                {
                    if (!cbdict[aus].Checked)
                        continue;
                    if (!auinstpubdict.ContainsKey(aus))
                        auinstpubdict.Add(aus, new List<pubclass>());
                    auinstpubdict[aus].Add(pc);

                    //Take care of authors with subject/inst mismatch:
                    bool foundsubj = false;
                    foreach (string ausubj in pc.get_authorsubjectalias())
                        if (authorclass.instsubj[aus].Contains(ausubj))
                            foundsubj = true;
                    if (!foundsubj)
                    {
                        if (!ausubjpubdict.ContainsKey(authorclass.nosubject))
                            ausubjpubdict.Add(authorclass.nosubject, new List<pubclass>());
                        ausubjpubdict[authorclass.nosubject].Add(pc);
                    }
                }
                if (!auinstpubdict.ContainsKey(hdainst))
                    auinstpubdict.Add(hdainst, new List<pubclass>());
                auinstpubdict[hdainst].Add(pc);

            }
            memo("Sorted publications.");
            memo(catpubdict.Count + " categories");
            memo(subjpubdict.Count + " research subjects");
            memo(ausubjpubdict.Count + " author subjects");
            memo(auinstpubdict.Count + " author institutions");

            foreach (string s in ausubjpubdict.Keys)
                memo(s + "\t" + ausubjpubdict[s].Count);
            foreach (string s in auinstpubdict.Keys)
                memo(s + "\t" + auinstpubdict[s].Count);

            //return;

            int ncat = 0;
            int maxcount = 333333;

            List<string> sheetnames = new List<string>();


            //foreach (string cat in catpubdict.Keys.Reverse())
            //{
            //    memo(cat);
            //    Excel.Worksheet sheet = (Excel.Worksheet)xlWcat.Sheets.Add();

            //    sheet.Name = validsheetname(cat, sheetnames);
            //    sheetnames.Add(sheet.Name);
            //    SheetWithHeader(sheet, catpubdict.Count);
            //    catsheetdict.Add(cat, sheet);
            //    int publine = 1;
            //    foreach (pubclass pc in catpubdict[cat])
            //    {
            //        publine++;
            //        pc.write_excelrow(sheet, publine, hd);
            //        if (publine > maxcount)
            //            break;
            //    }
            //    ncat++;
            //    if (ncat > maxcount)
            //        break;
            //}
            //memo("Saving to " + fncat);
            //xlWcat.SaveAs(fncat);


            //int nsubj = 0;
            //foreach (string subj in subjpubdict.Keys.Reverse())
            //{
            //    memo(subj);
            //    Excel.Worksheet sheet = (Excel.Worksheet)xlWsubj.Sheets.Add();
            //    sheet.Name = validsheetname(subj, sheetnames);
            //    sheetnames.Add(sheet.Name);
            //    SheetWithHeader(sheet, subjpubdict.Count);
            //    subjsheetdict.Add(subj, sheet);
            //    int publine = 1;
            //    foreach (pubclass pc in subjpubdict[subj])
            //    {
            //        publine++;
            //        pc.write_excelrow(sheet, publine, hd);
            //        if (publine > maxcount)
            //            break;
            //    }
            //    nsubj++;
            //    if (nsubj > maxcount)
            //        break;
            //}
            //memo("Saving to " + fnsubj);
            //xlWsubj.SaveAs(fnsubj);


            //int nausubj = 0;
            //foreach (string ausubj in ausubjpubdict.Keys.Reverse())
            //{
            //    memo(ausubj);
            //    Excel.Worksheet sheet = (Excel.Worksheet)xlWausubj.Sheets.Add();
            //    sheet.Name = validsheetname(ausubj, sheetnames);
            //    sheetnames.Add(sheet.Name);
            //    SheetWithHeader(sheet, ausubjpubdict.Count);
            //    ausubjsheetdict.Add(ausubj, sheet);
            //    int publine = 1;
            //    foreach (pubclass pc in ausubjpubdict[ausubj])
            //    {
            //        publine++;
            //        pc.write_excelrow(sheet, publine, hd);
            //        if (publine > maxcount)
            //            break;
            //    }
            //    nausubj++;
            //    if (nausubj > maxcount)
            //        break;
            //}

            //memo("Saving to " + fnausubj);
            //xlWausubj.SaveAs(fnausubj);


            //int nauinst = 0;
            //foreach (string auinst in auinstpubdict.Keys.Reverse())
            //{
            //    memo(auinst);
            //    Excel.Worksheet sheet = (Excel.Worksheet)xlWauinst.Sheets.Add();
            //    sheet.Name = validsheetname(auinst, sheetnames);
            //    sheetnames.Add(sheet.Name);
            //    SheetWithHeader(sheet, auinstpubdict.Count);
            //    auinstsheetdict.Add(auinst, sheet);
            //    int publine = 1;
            //    foreach (pubclass pc in auinstpubdict[auinst])
            //    {
            //        publine++;
            //        pc.write_excelrow(sheet, publine, hd);
            //        if (publine > maxcount)
            //            break;
            //    }
            //    nauinst++;
            //    if (nauinst > maxcount)
            //        break;
            //}



            //int nauinst = 0;
            foreach (string auinst in fninst.Keys)
            {
                memo(auinst);
                //private void AddExcelTab(Excel.Workbook xl, Dictionary<string, Excel.Worksheet> sheetdict, SortedDictionary<string, List<pubclass>> dict, string dictkey, List<string> sheetnames, int maxcount)
                if (RB_authorsubject.Checked)
                {
                    if (authorclass.instsubj.ContainsKey(auinst))
                        foreach (string subj in authorclass.instsubj[auinst])
                        {
                            AddExcelTabDiva(xldict[auinst], sheetdictdict[auinst], ausubjpubdict, subj, sheetnames, maxcount, auinst);
                        }
                }
                else if (RB_scopuscategory.Checked)
                {
                    //if (authorclass.instsubj.ContainsKey(auinst))
                    foreach (string subj in catpubdict.Keys)
                    {
                        AddExcelTabDiva(xldict[auinst], sheetdictdict[auinst], catpubdict, subj, sheetnames, maxcount, auinst);
                    }
                }
                else if (RB_scopussubject.Checked)
                {
                    //if (authorclass.instsubj.ContainsKey(auinst))
                    foreach (string subj in subjpubdict.Keys)
                    {
                        AddExcelTabDiva(xldict[auinst], sheetdictdict[auinst], subjpubdict, subj, sheetnames, maxcount, auinst);
                    }
                }
                else if (RB_groupfile.Checked)
                {
                    //if (authorclass.instsubj.ContainsKey(auinst))
                    foreach (string subj in augrouppubdict.Keys)
                    {
                        AddExcelTabDiva(xldict[auinst], sheetdictdict[auinst], augrouppubdict, subj, sheetnames, maxcount, auinst);
                    }
                }


                AddExcelTabDiva(xldict[auinst], sheetdictdict[auinst], auinstpubdict, auinst, sheetnames, maxcount, auinst);
                memo("Saving to " + fninst[auinst]);
                xldict[auinst].SaveAs(fninst[auinst]);

                //Excel.Worksheet sheet = (Excel.Worksheet)xldict[auinst].Sheets.Add();
                //sheet.Name = validsheetname(auinst, sheetnames);
                //sheetnames.Add(sheet.Name);
                //SheetWithHeader(sheet, auinstpubdict.Count);
                //auinstsheetdict.Add(auinst, sheet);
                //int publine = 1;
                //foreach (pubclass pc in auinstpubdict[auinst])
                //{
                //    publine++;
                //    pc.write_excelrow(sheet, publine, hd);
                //    if (publine > maxcount)
                //        break;
                //}
                //nauinst++;
                //if (nauinst > maxcount)
                //    break;
            }

            //memo("Saving to " + fnauinst);
            //xlWauinst.SaveAs(fnauinst);

            //foreach (string sc in catsheetdict.Keys)
            //{
            //    Marshal.ReleaseComObject(catsheetdict[sc]);
            //}
            //foreach (string sc in subjsheetdict.Keys)
            //{
            //    Marshal.ReleaseComObject(subjsheetdict[sc]);
            //}
            //foreach (string sc in ausubjsheetdict.Keys)
            //{
            //    Marshal.ReleaseComObject(ausubjsheetdict[sc]);
            //}
            foreach (string sc in auinstsheetdict.Keys)
            {
                Marshal.ReleaseComObject(auinstsheetdict[sc]);
            }
            //Then you can read from the sheet, keeping in mind that indexing in Excel is not 0 based. This just reads the cells and prints them back just as they were in the file.

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            //for (int i = 1; i <= rowCount; i++)
            //{
            //    for (int j = 1; j <= colCount; j++)
            //    {
            //        //new line
            //        if (j == 1)
            //            Console.Write("\r\n");

            //        //write the value to the console
            //        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
            //            Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");

            //        //add useful things here!   
            //    }
            //}

            //Lastly, the references to the unmanaged memory must be released. If this is not properly done, then there will be lingering processes that will hold the file access writes to your Excel workbook.

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background

            //close and release
            //xlWcat.Close();
            //Marshal.ReleaseComObject(xlWcat);

            //xlWsubj.Close();
            //Marshal.ReleaseComObject(xlWsubj);

            //xlWausubj.Close();
            //Marshal.ReleaseComObject(xlWausubj);

            xlWauinst.Close();
            Marshal.ReleaseComObject(xlWauinst);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            memo("==== DONE ====");

        }
    }
}
