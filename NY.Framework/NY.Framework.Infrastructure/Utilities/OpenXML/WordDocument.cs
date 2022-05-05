using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NY.Framework.Infrastructure.Utilities.OpenXML.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;

namespace NY.Framework.Infrastructure.Utilities.OpenXML
{
    public class WordDocument
    {
        string _outputDir;

        string GetTemplateDir()
        {
            return System.IO.Path.Combine(_outputDir, "template");
        }

        string GetTempDir()
        {
            return System.IO.Path.Combine(_outputDir, "temp");
        }

        public WordDocument(string outputDir)
        {
            _outputDir = outputDir;
        }

        void CreateTemplateFiles()
        {
            string tmpDir = GetTempDir();
            string templDir = GetTemplateDir();

            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }

            if (!Directory.Exists(templDir))
            {
                Directory.CreateDirectory(templDir);
            }

        }

        public void CleanTempFiles(string tempfile)
        {            
            if(System.IO.File.Exists(tempfile))
            {
                System.IO.File.Delete(tempfile);
            }
        }

        public string Generate(string templateFile, List<DOCX_TEXT> texts, List<DOCX_TABLE> tables, DOCX_PICTURE picture)
        {
            string generatedFile = "";

            try
            {
                CreateTemplateFiles();
                string outputFileName = System.IO.Path.GetFileNameWithoutExtension(templateFile);
                string tmpFile = outputFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".docx";
                string tmpFullFilePath = System.IO.Path.Combine(GetTempDir(), tmpFile);

                string templateFullFilePath = System.IO.Path.Combine(GetTemplateDir(), templateFile);

                if (System.IO.File.Exists(templateFullFilePath))
                {
                    System.IO.File.Copy(templateFullFilePath, tmpFullFilePath);
                    string fileName = tmpFullFilePath;


                    WordprocessingDocument wDoc = null;

                    try
                    {
                        wDoc = WordprocessingDocument.Open(fileName, true);
                        CreateWordDocFromTemplate(wDoc, texts, tables, picture);

                        generatedFile = tmpFullFilePath;

                        wDoc.MainDocumentPart.Document.Save();
                        wDoc.Save();
                        wDoc.Close();
                        wDoc.Dispose();


                    }
                    catch (Exception innerEx)
                    {

                        if (wDoc != null)
                        {
                            wDoc.Close();

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


        void CreateWordDocFromTemplate(WordprocessingDocument wDoc, List<DOCX_TEXT> texts, List<DOCX_TABLE> tables, DOCX_PICTURE picture)
        {
            if (texts != null)
            {
                string docText = "";
                using (StreamReader sr = new StreamReader(wDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                foreach (DOCX_TEXT txt in texts)
                {
                    Regex regexText = new Regex(txt.ID);
                    if (txt.Value == null)
                    {
                        docText = regexText.Replace(docText, "");
                    }
                    else
                    {
                        docText = regexText.Replace(docText, txt.Value);
                    }

                }

                using (StreamWriter sw = new StreamWriter(wDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }
            }


            //if(pictures != null)
            //{
            //    foreach (DOCX_PICTURE pic in pictures)
            //    {
            //        AddPictures(wDoc, pic);
            //    }
            //}

            if (picture != null)
            {
                if (!string.IsNullOrEmpty(picture.FilePath))
                {
                    AddPictures(wDoc, picture);
                }
            }


            if (tables != null)
            {
                foreach (DOCX_TABLE table in tables)
                {
                    AddTableData(wDoc, table);
                }
            }

        }

        void AddTableData(WordprocessingDocument wDoc, DOCX_TABLE table)
        {
            try
            {
                TableProperties wtable =
wDoc.MainDocumentPart.Document.Body.Descendants
<TableProperties>().Where(t =>t.TableCaption!=null && t.TableCaption.Val == table.Caption).FirstOrDefault();
                if (wtable != null)
                {
                    Table tmpTable = (Table)wtable.Parent;

                    TableRow rowCopy = null;
                    //TableRow theRow = tmpTable.Elements<TableRow>().Last();

                    bool firstRow = true;
                    foreach (DOCX_TABLE_ROW row in table.Rows)
                    {

                        if (firstRow)
                        {
                            firstRow = false;
                            rowCopy = tmpTable.Elements<TableRow>().Last();
                        }
                        else
                        {
                            rowCopy = (TableRow)tmpTable.Elements<TableRow>().Last().CloneNode(true);
                            tmpTable.Append(rowCopy);
                        }

                        for (int i = 0; i < row.getCells().Count; i++)
                        {
                            TableCell cell = rowCopy.Elements<TableCell>().ElementAt(i);
                            //Paragraph p = cell.Elements<Paragraph>().First();
                            //var runProperties = new RunProperties();
                            //runProperties.AppendChild(new RunFonts() { Ascii = table.FontName });
                            //runProperties.AppendChild(new FontSize() { Val = table.FontSize });
                            //Run r = new Run();
                            //r.RunProperties = runProperties;
                            //// Set the text for the run.
                            //Text t = new Text();
                            //t.Text = row.Cell(i).Value;
                            //r.Append(t);
                            //p.Append(r);

                            Paragraph p = cell.Elements<Paragraph>().First();
                            var runProperties = new RunProperties();
                            runProperties.AppendChild(new RunFonts() { Ascii = table.FontName });
                            runProperties.AppendChild(new FontSize() { Val = table.FontSize });
                            Run r = p.Elements<Run>().First();

                            // Set the text for the run.
                            Text t = r.Elements<Text>().First();
                            t.Text = row.Cell(i).Value;

                        }
                    }




                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


        }

        void AddPictures(WordprocessingDocument wDoc, DOCX_PICTURE pic)
        {
            try
            {
                //ImagePart imgp = (ImagePart)wDoc.MainDocumentPart.GetPartById(pic.ID);
                ImagePart imgp = wDoc.MainDocumentPart.GetPartsOfType<ImagePart>().FirstOrDefault();
                if (imgp != null)
                {
                    var newImageBytes = File.ReadAllBytes(pic.FilePath);

                    if (newImageBytes != null)
                    {
                        using (var wrirter = new BinaryWriter(imgp.GetStream()))
                        {
                            wrirter.Write(newImageBytes);
                            wrirter.Close();
                        }
                    }


                }

            }
            catch (Exception ex)
            {

            }

        }
    }
}
