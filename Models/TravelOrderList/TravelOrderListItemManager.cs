using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;

namespace TravelManagementApi.Models.TravelOrderList
{
    public class TravelOrderListItemManager
    {
        readonly IFormFile _travelOrderListItem;
        readonly string _listName;

        public string ListName
        {
            get { return _listName; }
        }

        public TravelOrderListItemManager(IFormFile travelOrderList)
        {
            _travelOrderListItem = travelOrderList;
            _listName = _travelOrderListItem.FileName;
        }

        public async Task<TravelOrderListItem> SaveListAsync(string filePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await _travelOrderListItem.CopyToAsync(fileStream);
            }

            return new TravelOrderListItem
            {
                Name = _listName,
                Path = filePath
            };
        }

        public List<TravelOrderData> GetExtractedListData()
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Open(_travelOrderListItem.OpenReadStream(), false);
            WorkbookPart workbookPart = document.WorkbookPart;
            WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

            List<TravelOrderData> travelOrderDataItems = new List<TravelOrderData>();

            for (var rowIndex = 1; rowIndex < sheetData.Elements<Row>().Count(); rowIndex++)
            {
                var row = sheetData.Elements<Row>().ElementAt(rowIndex);
                var travelOrderDataItem = new TravelOrderData();

                for (var cellIndex = 1; cellIndex < row.Elements<Cell>().Count() - 1; cellIndex++)
                {
                    var cell = row.Elements<Cell>().ElementAt(cellIndex);

                    if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                    {
                        var stringId = Convert.ToInt32(cell.InnerText); // Do some error checking here
                        var cellData = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;

                        switch (cellIndex)
                        {
                            case 2:
                                travelOrderDataItem.Employee = cellData;
                                break;
                            case 3:
                                travelOrderDataItem.Initials = cellData;
                                break;
                            case 4:
                                travelOrderDataItem.InitialsShorthand = cellData;
                                break;
                            case 6:
                                travelOrderDataItem.Role = cellData;
                                break;
                            case 7:
                                travelOrderDataItem.City = cellData;
                                break;

                        }
                    }
                    else
                    {
                        if (cellIndex == 8 || cellIndex == 9 || cellIndex == 10)
                        {
                            var cellValue = DateTime.FromOADate(double.Parse(cell.InnerText)).ToString("dd.MM.yyyy");

                            switch (cellIndex)
                            {
                                case 8:
                                    travelOrderDataItem.DateStart = cellValue;
                                    break;
                                case 9:
                                    travelOrderDataItem.DateEnd = cellValue;
                                    break;
                                case 10:
                                    travelOrderDataItem.DatePaid = cellValue;
                                    break;
                            }

                        }
                        else
                        {
                            switch (cellIndex)
                            {
                                case 1:
                                    travelOrderDataItem.FileName = cell.CellValue.Text;
                                    break;
                                case 5:
                                    travelOrderDataItem.OrderNumber = cell.CellValue.Text;
                                    break;
                                case 11:
                                    travelOrderDataItem.NumberOfDays = cell.CellValue.Text;
                                    break;
                                case 12:
                                    travelOrderDataItem.AmountPerDay = cell.CellValue.Text;
                                    break;
                                case 13:
                                    travelOrderDataItem.AmountSumForDays = cell.CellValue.Text;
                                    break;
                            }
                        }
                    }
                }
                travelOrderDataItems.Add(travelOrderDataItem);
            }
            return travelOrderDataItems;
        }
    }
}
