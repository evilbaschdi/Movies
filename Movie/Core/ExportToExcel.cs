//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.InteropServices;
//using System.Windows;
//using System.Windows.Input;
//using Microsoft.Office.Interop.Excel;
//using Application = Microsoft.Office.Interop.Excel.Application;

//namespace Movie.Core
//{
//    public class ExportToExcel<T>
//        where T : class
//    {
//        private readonly object _optionalValue = Missing.Value;
//        public List<T> DataToPrint;
//        private _Workbook _book;
//        private Workbooks _books;
//        private Application _excelApp;
//        private Font _font;
//        private Range _range;
//        private _Worksheet _sheet;
//        private Sheets _sheets;
//        // Optional argument variable

//        /// <summary>
//        ///     Generate report and sub functions
//        /// </summary>
//        public void GenerateReport()
//        {
//            try
//            {
//                if (DataToPrint != null)
//                {
//                    if (DataToPrint.Count != 0)
//                    {
//                        Mouse.SetCursor(Cursors.Wait);
//                        CreateExcelRef();
//                        FillSheet();
//                        OpenReport();
//                        Mouse.SetCursor(Cursors.Arrow);
//                    }
//                }
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(string.Format("Error while generating Excel report: {0}", exception));
//            }
//            finally
//            {
//                ReleaseObject(_sheet);
//                ReleaseObject(_sheets);
//                ReleaseObject(_book);
//                ReleaseObject(_books);
//                ReleaseObject(_excelApp);
//            }
//        }

//        /// <summary>
//        ///     Make Microsoft Excel application visible
//        /// </summary>
//        private void OpenReport()
//        {
//            _excelApp.Visible = true;
//        }

//        /// <summary>
//        ///     Populate the Excel sheet
//        /// </summary>
//        private void FillSheet()
//        {
//            object[] header = CreateHeader();
//            WriteData(header);
//        }

//        /// <summary>
//        ///     Write data into the Excel sheet
//        /// </summary>
//        /// <param name="header"></param>
//        private void WriteData(object[] header)
//        {
//            var objData = new object[DataToPrint.Count, header.Length];

//            for (int j = 0; j < DataToPrint.Count; j++)
//            {
//                T item = DataToPrint[j];
//                for (int i = 0; i < header.Length; i++)
//                {
//                    object y = typeof (T).InvokeMember
//                        (header[i].ToString(), BindingFlags.GetProperty, null, item, null);
//                    objData[j, i] = (y == null) ? "" : y.ToString();
//                }
//            }
//            AddExcelRows("A2", DataToPrint.Count, header.Length, objData);
//            AutoFitColumns("A1", DataToPrint.Count + 1, header.Length);
//        }

//        /// <summary>
//        ///     Method to make columns auto fit according to data
//        /// </summary>
//        /// <param name="startRange"></param>
//        /// <param name="rowCount"></param>
//        /// <param name="colCount"></param>
//        private void AutoFitColumns(string startRange, int rowCount, int colCount)
//        {
//            _range = _sheet.Range[startRange, _optionalValue];
//            _range = _range.Resize[rowCount, colCount];
//            _range.Columns.AutoFit();
//        }

//        /// <summary>
//        ///     Create header from the properties
//        /// </summary>
//        /// <returns></returns>
//        private object[] CreateHeader()
//        {
//            PropertyInfo[] headerInfo = typeof (T).GetProperties();

//            // Create an array for the headers and add it to the
//            // worksheet starting at cell A1.

//            object[] headerToAdd = headerInfo.Select(t => t.Name).Cast<object>().ToArray();
//            AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
//            SetHeaderStyle();

//            return headerToAdd;
//        }

//        /// <summary>
//        ///     Set Header style as bold
//        /// </summary>
//        private void SetHeaderStyle()
//        {
//            _font = _range.Font;
//            _font.Bold = true;
//        }

//        /// <summary>
//        ///     Method to add an excel rows
//        /// </summary>
//        /// <param name="startRange"></param>
//        /// <param name="rowCount"></param>
//        /// <param name="colCount"></param>
//        /// <param name="values"></param>
//        private void AddExcelRows
//            (string startRange, int rowCount, int colCount, object values)
//        {
//            _range = _sheet.Range[startRange, _optionalValue];
//            _range = _range.Resize[rowCount, colCount];
//            _range.set_Value(_optionalValue, values);
//        }

//        /// <summary>
//        ///     Create Excel application parameters instances
//        /// </summary>
//        private void CreateExcelRef()
//        {
//            _excelApp = new Application();
//            _books = _excelApp.Workbooks;
//            _book = _books.Add(_optionalValue);
//            _sheets = _book.Worksheets;
//            _sheet = (_Worksheet) (_sheets.Item[1]);
//        }

//        /// <summary>
//        ///     Release unused COM objects
//        /// </summary>
//        /// <param name="obj"></param>
//        private void ReleaseObject(object obj)
//        {
//            try
//            {
//                Marshal.ReleaseComObject(obj);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//            finally
//            {
//                GC.Collect();
//            }
//        }
//    }
//}