using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Business;
using ClosedXML.Excel;

namespace Architecture
{
    public static class GanerateExcel
    {
        public static string CreateExcel<T>(ExcelConfigurations configurations, IList<T> domains) where T : BaseDomain
        {
            var path = configurations.DirectoryPath + $"{DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss")}.xlsx";

            File.Copy(configurations.Template, path);

            using (var workbook = new XLWorkbook(path))
            {
                var worksheet = workbook.Worksheets.Worksheet(1);

                for (int i = 0; i < domains.Count; i++)
                {
                    var domain = domains[i];
                    worksheet.Cell("A" + (2 + i)).Value = domain;
                }
                workbook.Save();
            }

            return path;
        }

        public static void WriteCollectionToExcelTable<T>(ExcelConfigurations configurations,/*string sheetName, */params T[] elements)
        {
            var table = new DataTable();

            foreach (var p in (typeof(T).GetProperties()))
            {
                table.Columns.Add(p.Name, typeof(string));
            }

            foreach (T trd in elements)
            {
                table.Rows.Add(trd.GetType().GetProperties().Select(p => p.GetValue(trd)).ToArray());
            }

            var path = configurations.DirectoryPath + $"{DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss")}.xlsx";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var workbook = new XLWorkbook())
            {
                var currSheet = workbook.Worksheets.Add(table/*, sheetName*/);

                if (workbook.Worksheets.Count > 0)
                {
                    workbook.SaveAs($"{path + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss")}.xlsx");
                }
            }
        }

    }
}
