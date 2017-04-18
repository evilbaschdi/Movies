using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Movie.Core;

namespace DvdLibraryImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\swalter\Documents\";
            var fileName = "shelf_export.txt";
            var shelfExport = Path.Combine(path, fileName);
            if (!File.Exists(shelfExport))
            {
                Console.WriteLine("File does not exist");
                Console.ReadLine();
                return;
            }


            var shelfExportXml = XDocument.Load(shelfExport).Root;
            var items = shelfExportXml?.Element("items")?.Elements().ToList();

            //11 == Barcode
            //2 == Title
            //4 == Date Released (DVD not movie)

            if (items != null)
            {
                foreach (var item in items)
                {
                    var data = item.Element("data")?.Elements().ToList();
                    if (data != null && data.Any())
                    {
                        foreach (var datum in data)
                        {
                            var movie = new MovieRecord();
                            movie.Id = Guid.NewGuid().ToString();
                            movie.Format = "Blu-ray";

                            switch (datum.Element("field")?.Value)
                            {
                                case "11":
                                    //todo: barcode aufnehmen
                                    //Console.WriteLine(datum.Element("value")?.Value);
                                    break;
                                case "2":
                                    var name = datum.Element("value")?.Value;
                                    movie.Name = name.DecodeString().Trim();
                                    break;
                                case "4":
                                    //Console.WriteLine(datum.Element("value")?.Value);
                                    break;
                            }
                        }
                    }
                    Console.WriteLine(Environment.NewLine);
                }
            }

            Console.WriteLine();
            Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        public static string DecodeString(this string input)
        {
            var bytes = Encoding.Default.GetBytes(input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}