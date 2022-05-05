using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities
{
    public class WordHelper
    {
        public string filepath { get; set; }
        public WordHelper(string path)
        {
            this.filepath = path;
        }
        public class Docx_Text
        {
            public string Id { get; set; }
            public string Value { get; set; }
            public Docx_Text() { }
            public Docx_Text(string id,string value)
            {
                this.Id = id;
                this.Value = value;
            }
        }
        public class Docx_Table_Cell
        {
            public string value { get; set; }
            public Docx_Table_Cell() { }
            public Docx_Table_Cell(string val)
            {
                this.value = val;
            }
        }
        public class Docx_Table_Row
        {
            public List<Docx_Table_Cell> cells { get; set; }
            public Docx_Table_Row()
            {
                cells = new List<Docx_Table_Cell>();
            }
            public List<Docx_Table_Cell> getCells()
            {
                return cells;
            }
            public Docx_Table_Cell  Cell(int index)
            {
                return cells[index];
            }
            public void AddCell(Docx_Table_Cell cell)
            {
                cells.Add(cell);
            }
        }

        public class Docx_Table
        {
            public int tableindex { get; set; }
            public List<Docx_Table_Row> rows { get; set; }
            public Docx_Table(int index)
            {
                this.tableindex = index;
                rows = new List<Docx_Table_Row>();
            }
        }

        string GetTempDir()
        {
            return Path.Combine(filepath, "temp");
        }
        string GetTemplateDir()
        {
            return Path.Combine(filepath, "template");
        }

        public string GenerateWordFile(string templateFile, List<Docx_Text> texts, List<Docx_Table> tables, string picture)
        {
            string generatedFile = "";

            try
            {                
                string outputFileName = Path.GetFileNameWithoutExtension(templateFile);
                string tmpFile = outputFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".docx";
                string tmpFullFilePath = Path.Combine(GetTempDir(), tmpFile);

                string templateFullFilePath = Path.Combine(GetTemplateDir(), templateFile);

                if (System.IO.File.Exists(templateFullFilePath))
                {
                    System.IO.File.Copy(templateFullFilePath, tmpFullFilePath);
                    string fileName = tmpFullFilePath;
                    fileName = "D:\\ACCM\\Reports\\temp\\CaseForm_20200228114327525.docx";
                    Xceed.Words.NET.DocX aDoc = null;

                    try
                    {
                        aDoc = Xceed.Words.NET.DocX.Load(fileName);

                        CreateWordDocFromTemplate(aDoc, texts, tables, picture);
                        generatedFile = tmpFullFilePath;
                        aDoc.Save();
                        aDoc = null;


                    }
                    catch (Exception innerEx)
                    {
                        string msg = innerEx.Message;
                        if (aDoc != null)
                        {
                            aDoc = null;

                        }
                        generatedFile = "";

                    }
                }

            }

            catch (Exception ex)
            {
                generatedFile = "";
            }

            return generatedFile;
        }
        void FindAndReplace(Xceed.Words.NET.DocX aDoc, string findText, string replaceWithText)
        {
            
            aDoc.ReplaceText(findText, replaceWithText);
        }
        void CreateWordDocFromTemplate(Xceed.Words.NET.DocX aDoc, List<Docx_Text> texts, List<Docx_Table> tables, string picture)
        {
            foreach (Docx_Text txt in texts)
            {
                FindAndReplace(aDoc, txt.Id, txt.Value);
            }
            if (!string.IsNullOrEmpty(picture))
            {
                if (File.Exists(picture))
                {
                  //  AddPicture(aDoc, picture);
                }
            }


            foreach (Docx_Table table in tables)
            {
                int i = 0;
                foreach (Docx_Table_Row row in table.rows)
                {

                    int rIndx = aDoc.Tables[table.tableindex - 1].Rows.Count - 1;
                    int cIndx = 0;
                    foreach (Docx_Table_Cell cell in row.getCells())
                    {
                        aDoc.Tables[table.tableindex - 1].Rows[rIndx].Cells[cIndx].Paragraphs[0].Append(cell.value);
                      //  aDoc.Tables[table.tableindex - 1].Rows[rIndx].Cells[cIndx].VerticalAlignment = Xceed.Words.NET.DocX.v.Center;
                        aDoc.Tables[table.tableindex - 1].Rows[rIndx].Cells[cIndx].Paragraphs[0].Font("Pyidaungsu");
                        aDoc.Tables[table.tableindex - 1].Rows[rIndx].Cells[cIndx].Paragraphs[0].FontSize(13);
                        cIndx = cIndx + 1;
                    }

                    i = i + 1;
                    if (i < table.rows.Count)
                    {
                        aDoc.Tables[table.tableindex - 1].InsertRow();
                    }
                   
                }
            }
        }
    }
}
