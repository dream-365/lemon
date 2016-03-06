using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using HtmlAgilityPack;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Data
{
    public class SpreadsheetCollection : IReadOnlyCollection<BsonDocument>
    {
        private string _source;
        private string _sheetName;

        public SpreadsheetCollection(string source, string sheetName = "My Active Cases")
        {
            _source = source;
            _sheetName = sheetName;
        }

        public Task ForEachAsync(Action<BsonDocument> processor)
        {
            var task = new Task(() =>
            {

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(_source, true))
                {

                    IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == _sheetName);
                    if (sheets.Count() == 0)
                    {
                        new Exception("The specified worksheet does not exist.");
                    }

                    WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);

                    var headers = ParseCells(worksheetPart.Worksheet.Descendants<Row>().FirstOrDefault(), document);

                    foreach (Row row in worksheetPart.Worksheet.Descendants<Row>().Skip(1))
                    {

                        var bson = new BsonDocument();

                        var values = ParseCells(row, document);

                        for (int i = 0; i < headers.Length; i++)
                        {
                            bson.Set(headers[i], values[i]);
                        }

                        processor(bson);

                    }
                }

            });

            task.RunSynchronously();

            return task;
        }

        private string[] ParseCells(Row row, SpreadsheetDocument document)
        {

            var values = new List<string>();

            foreach (var cell in row.Descendants<Cell>())
            {

                values.Add(GetCellValue(cell, document));
            }

            return values.ToArray();
        }

        private string GetCellValue(Cell cell, SpreadsheetDocument document)
        {

            string value = cell.InnerText;

            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:

                        // For shared strings, look up the value in the
                        // shared strings table.
                        var stringTable =
                            document.WorkbookPart.GetPartsOfType<SharedStringTablePart>()
                            .FirstOrDefault();

                        // If the shared string table is missing, something 
                        // is wrong. Return the index that is in
                        // the cell. Otherwise, look up the correct text in 
                        // the table.
                        if (stringTable != null)
                        {
                            value =
                                stringTable.SharedStringTable
                                .ElementAt(int.Parse(value)).InnerText;
                        }
                        break;

                    case CellValues.Boolean:
                        switch (value)
                        {
                            case "0":
                                value = "FALSE";
                                break;
                            default:
                                value = "TRUE";
                                break;
                        }
                        break;
                }
            }
            return value;
        }


        public Task ForEachAsync(Func<BsonDocument, Task> processor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex">start from 1</param>
        /// <returns></returns>
        public string[] GetRow(int rowIndex)
        {

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(_source, true))
            {

                IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == _sheetName);
                if (sheets.Count() == 0)
                {
                    new Exception("The specified worksheet does not exist.");
                }

                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);

                Row row = worksheetPart.Worksheet.Descendants<Row>().ElementAt(rowIndex-1);
                return ParseCells(row, document);
              
            }
            throw new Exception("no row found!");

        }
    }
}
