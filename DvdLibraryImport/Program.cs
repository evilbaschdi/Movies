using System.Xml.Linq;
using EvilBaschdi.Core.Extensions;
using Movie.Core.Models;

namespace DvdLibraryImport;

/// <summary>
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class Program
{
    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        const string fileName = "shelf_export.txt";
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
            foreach (var list in items.Select(item => item.Element("data")?.Elements().ToList()))
            {
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        var movie = new MovieRecord
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        Format = "Blu-ray"
                                    };

                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (item.Element("field")?.Value)
                        {
                            case "11":
                                //todo: barcode implementation
                                //Console.WriteLine(datum.Element("value")?.Value);
                                break;
                            case "2":
                                var name = item.Element("value")?.Value;
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