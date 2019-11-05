using ClosedXML.Excel;
using GenericParsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace WebUI.Helpers
{
    public static class ExcelHelper
    {
        public struct ExcelDownloadFile
        {
            public byte[] FileContents { get; set; }

            public string ContentType { get; set; }

            public string FileDownLoadName { get; set; }
        }

        public static ExcelDownloadFile ExcelIndir(List<DataTable> dataTables, string excelName)
        {
            ExcelDownloadFile file;

            var fileName = $"{excelName}-{DateTime.Now.ToString().Replace("/", ".").Replace(" ", "-")}.xlsx";

            using (XLWorkbook wb = new XLWorkbook())
            {
                for (int i = 0; i < dataTables.Count; i++)
                {
                    wb.Worksheets.Add(dataTables[i], dataTables[i].TableName);
                }
              
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    file = new ExcelDownloadFile
                    {
                        FileContents = stream.ToArray(),
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileDownLoadName = fileName
                    };
                }
            }

            return file;
        }

        public static Tuple<DataSet, string> ExcelToDataSet(HttpPostedFileBase file, string yol, string excelName)
        {
            DataSet ds = new DataSet();

            try
            {
                if (file.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".csv")
                    {
                        var fileName = $"{excelName}-{DateTime.Now.ToString().Replace("/", ".").Replace(" ", "-")}{fileExtension}";

                        string fileLocation = $"{yol}{excelName}{Path.GetExtension(file.FileName)}";

                        if (File.Exists(fileLocation))
                        {
                            File.Delete(fileLocation);
                        }

                        file.SaveAs(fileLocation);

                        string excelConnectionString = string.Empty;
                        excelConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileLocation};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                        if (fileExtension == ".csv")
                        {
                            using (GenericParserAdapter parser = new GenericParserAdapter(fileLocation))
                            {
                                parser.ColumnDelimiter = ';';
                                ds = parser.GetDataSet();
                            }
                        }
                        else
                        {
                            if (fileExtension == ".xls")
                            {
                                excelConnectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fileLocation};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            }
                            else if (fileExtension == ".xlsx")
                            {
                                excelConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileLocation};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            }

                            var excelConnection = new OleDbConnection(excelConnectionString);
                            excelConnection.Open();

                            var dt = new DataTable();

                            dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                            if (dt == null)
                            {
                                return null;
                            }

                            var excelSheets = new string[dt.Rows.Count];
                            int t = 0;

                            foreach (DataRow row in dt.Rows)
                            {
                                excelSheets[t] = row["TABLE_NAME"].ToString();
                                t++;
                            }

                            OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                            string query = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }
                        }
                    }
                }

                return new Tuple<DataSet, string>(ds, string.Empty);
            }
            catch (Exception ex)
            {
                return new Tuple<DataSet, string>(ds, ex.Message);
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            var indicies = new List<int>();

            var columnList = new List<DataColumn>();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                if (prop.Name.IndexOf("Id") != -1)
                {
                    indicies.Add(i);
                    continue;
                }

                columnList.Add(new DataColumn(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
            }

            foreach (var column in columnList)
            {
                table.Columns.Add(column);
            }

            var values = new List<object>();

            //object[] values = new object[columnList.Count];

            foreach (T item in data)
            {
                for (int i = 0; i < props.Count; i++)
                {
                    var exists = indicies.Count(x => x == i) > 0;

                    if (exists)
                        continue;

                    values.Add(props[i].GetValue(item));
                }

                object[] tableValues = new object[values.Count];

                for (var j = 0; j < values.Count; j++)
                {
                    tableValues[j] = values[j];
                }

                table.Rows.Add(tableValues);

                values.Clear();
            }

            return table;
        }
    }
}