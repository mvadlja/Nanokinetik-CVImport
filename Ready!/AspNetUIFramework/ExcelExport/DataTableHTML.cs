using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AspNetUIFramework.ExcelExport
{
    public class DataTableHTML
    {
        private StringBuilder StrBuilder;

        public DataTableHTML()
        {
            StrBuilder = new StringBuilder();
        }

        public StringBuilder HtmlTable
        {
            get { return StrBuilder; }
        }

        /// <summary>
        /// Exportira DataTable u Html fajl
        /// </summary>
        /// <param name="DataTableSource">DataTable izvor</param>
        /// <param name="cssID">Class tag</param>
        /// <returns></returns>
        public bool Export(DataTable DataTableSource, string query, string mainTitle)
        {
            try
            {
                StrBuilder.Append("<Table border=\"1px\">");

                // datum
                StrBuilder.Append("<TR>");
                StrBuilder.Append("<TD colspan='26'>Export date: " + DateTime.Now.ToString("HH:mm - yyyy-MM-dd") + "</TD>");
                StrBuilder.Append("</TR>");

                // main title
                StrBuilder.Append("<TR>");
                StrBuilder.Append("<TD colspan='26'><B>" + mainTitle + "</B></TD>");
                StrBuilder.Append("</TR>");

                //header
                StrBuilder.Append("<TR>");
                foreach (DataColumn Column in DataTableSource.Columns)
                {
                    switch (Column.ColumnName)
                    {
                        case "recId":

                            break;

                    }

                    StrBuilder.Append("<TD bgcolor=\"#99CCFF\">" + Column.ColumnName + "</TD>");
                }
                StrBuilder.Append("</TR>");

                // rows
                foreach (DataRow Row in DataTableSource.Rows)//*****************************************************
                {
                    StrBuilder.Append("<TR>");
                    for (int i = 0; i < Row.ItemArray.Length; i++)
                    {
                        string Item = Row.ItemArray[i].ToString().Trim();
                        if (Item.Length == 0) Item = "&nbsp;";
                        StrBuilder.Append("<TD>" + Item + "</TD>");
                    }
                    StrBuilder.Append("</TR>");

                }//*************************************************************************************************


                StrBuilder.Append("</Table>");
                StrBuilder.Append("<br><br>");
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool ExportDetails(DataTable DataTableSource, string query, string mainTitle)
        {
            try
            {
                StrBuilder.Append("<Table border=\"1px\">");

                // datum
                StrBuilder.Append("<TR>");
                StrBuilder.Append("<TD colspan='26'>Export date: " + DateTime.Now.ToString("HH:mm - yyyy-MM-dd") + "</TD>");
                StrBuilder.Append("</TR>");

                // main title
                StrBuilder.Append("<TR>");
                StrBuilder.Append("<TD colspan='26'><B>" + mainTitle + "</B></TD>");
                StrBuilder.Append("</TR>");

                int i = 0;
                foreach (DataColumn Column in DataTableSource.Columns)
                {
                    switch (Column.ColumnName)
                    {
                        case "recId":

                            break;

                    }

                    StrBuilder.Append("<TR>");
                    StrBuilder.Append("<TD bgcolor=\"#99CCFF\">" + Column.ColumnName + "</TD>");
                    StrBuilder.Append("<TD>" + DataTableSource.Rows[0][i] + "</TD>");
                    StrBuilder.Append("</TR>");

                    i++;
                }

                StrBuilder.Append("</Table>");
                StrBuilder.Append("<br><br>");
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Otvara html fajl u IExploreru
        /// </summary>
        /// <param name="SourceFile">Fajl koji prikazuješ</param>
        public void PrintPreview(string SourceFile)
        {
            Process myProcess = new Process();
            myProcess.EnableRaisingEvents = false;
            myProcess.StartInfo.FileName = "iexplore";
            myProcess.StartInfo.Arguments = SourceFile;
            myProcess.Start();
        }

        /// <summary>
        /// Apenduje html u aktivnom html fajlu
        /// </summary>
        /// <param name="Html"></param>
        public void Append(string Html)
        {
            StrBuilder.Append(Html);
        }
    }
}
