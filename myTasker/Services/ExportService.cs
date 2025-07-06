using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myTasker.Services
{
    public class ExportService
    {
        public async Task ExportAsync<T>(IEnumerable<T> data, ExportFormat format, string filePath)
        {
            switch (format)
            {
                case ExportFormat.Xlsx:
                    await ExportToExcelAsync(data, filePath);
                    break;

                case ExportFormat.Csv:
                    await ExportToCsvAsync(data, filePath);
                    break;

                default:
                    throw new NotSupportedException($"Format {format} non supporté.");
            }
        }

        private async Task ExportToExcelAsync<T>(IEnumerable<T> data, string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Export");

            var properties = typeof(T).GetProperties();

            // En-têtes
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cells[1, col + 1].Value = properties[col].Name;
            }

            // Données
            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var value = properties[col].GetValue(item)?.ToString();
                    worksheet.Cells[row, col + 1].Value = value;
                }
                row++;
            }

            await package.SaveAsAsync(new FileInfo(filePath));
        }

        private async Task ExportToCsvAsync<T>(IEnumerable<T> data, string filePath)
        {
            var properties = typeof(T).GetProperties();
            var sb = new StringBuilder();

            // En-têtes
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Lignes
            foreach (var item in data)
            {
                var line = string.Join(",", properties.Select(p =>
                {
                    var val = p.GetValue(item)?.ToString()?.Replace(",", " ");
                    return $"\"{val}\"";
                }));
                sb.AppendLine(line);
            }

            await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
    public enum ExportFormat
    {
        Xlsx,
        Csv,
        PDF,
        JSON,
        WORD,
        XML
    }
}
