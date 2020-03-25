using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using TravelManagementApi.Models.TravelOrder;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace TravelManagementApi.Models.TravelOrderDocument
{
    public class TravelOrderDocumentManager
    {
        private string _documentTemplatePath, _generatedDocumentsPath;
        public long _listId;
        private List<TravelOrderData> _travelOrderDataItems;

        public TravelOrderDocumentManager( string documentTemplatePath, List<TravelOrderData> travelOrderDataItems, long listId,  string generatedDocumentsPath)
        {
            _documentTemplatePath = documentTemplatePath;
            _travelOrderDataItems = travelOrderDataItems;
            _listId = listId;
            _generatedDocumentsPath = generatedDocumentsPath;
        }

        public List<TravelOrderDocumentItem> GenerateDocumentsFromData()
        {
            List<TravelOrderDocumentItem> travelOrderDocumentItems = new List<TravelOrderDocumentItem>();

            using (var mainDoc = WordprocessingDocument.Open(_documentTemplatePath, false))
            {

                foreach (var travelOrderDataItem in _travelOrderDataItems)
                {
                    var newDocumentPath = Path.Combine(_generatedDocumentsPath, _listId.ToString(), travelOrderDataItem.FileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(newDocumentPath));
                    using var generatedDocument = WordprocessingDocument.Create(newDocumentPath, WordprocessingDocumentType.Document);
                    // copy parts from source document to new document
                    foreach (var part in mainDoc.Parts)
                        generatedDocument.AddPart(part.OpenXmlPart, part.RelationshipId);

                    IDictionary<String, BookmarkStart> bookmarkMap = new Dictionary<String, BookmarkStart>();

                    foreach (BookmarkStart bookmarkStart in generatedDocument.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                    {
                        bookmarkMap[bookmarkStart.Name] = bookmarkStart;
                    }

                    foreach (BookmarkStart bookmarkStart in bookmarkMap.Values)
                    {
                        OpenXmlElement elem = bookmarkStart.NextSibling();

                        while (elem != null && !(elem is BookmarkEnd))
                        {
                            OpenXmlElement nextElem = elem.NextSibling();
                            elem.Remove();
                            elem = nextElem;
                        }

                        switch (bookmarkStart.Name.Value)
                        {
                            case "Zaposleni":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.Employee)), bookmarkStart);
                                break;
                            case "Skracenica":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.InitialsShorthand)), bookmarkStart);
                                break;
                            case "RedniBroj":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.OrderNumber)), bookmarkStart);
                                break;
                            case "RadnoMesto":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.Role)), bookmarkStart);
                                break;
                            case "Mesto":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.City)), bookmarkStart);
                                break;
                            case "DateStart":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.DateStart)), bookmarkStart);
                                break;
                            case "DateEnd":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.DateEnd)), bookmarkStart);
                                break;
                            case "DatePaid":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.DatePaid)), bookmarkStart);
                                break;
                            case "NumberOfDays":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.NumberOfDays)), bookmarkStart);
                                break;
                            case "AmountPerDay":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.AmountPerDay)), bookmarkStart);
                                break;
                            case "AmountSumForDays":
                                bookmarkStart.Parent.InsertAfter<Run>(new Run(new Text(travelOrderDataItem.AmountSumForDays)), bookmarkStart);
                                break;
                        }
                    }

                    var travelOrderDocumentItem = new TravelOrderDocumentItem
                    {
                        ListId = _listId,
                        Name = travelOrderDataItem.FileName,
                        Path = newDocumentPath
                    };

                    travelOrderDocumentItems.Add(travelOrderDocumentItem);
                }
            }

            return travelOrderDocumentItems;
        }
    }
}
