using System.Collections.Generic;
using System.IO;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Classes.Importers
{
    /// <summary>
    /// Релизация алгоритма простого имопрта csv файла, имеющего след структуру
    /// Название; Стоимость; Количество; Путь до картинки 
    /// Пример первых 2 строк
    /// Mir;5;10;D:\user\downloads\mirinda.jpg 
    //  Spr;10;10;D:\user\downloads\sprite.jpg
    /// </summary>
    public class CsvImport : IStrategy
    {
        private const string ExtensionsAllowed = "csv";

        public List<Drink> Import(Stream stream)
        {
            var drinks = new List<Drink>();

            using (StreamReader streamReader =
                new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    string row = streamReader.ReadLine();
                    if (!string.IsNullOrEmpty(row))
                    {
                        var cell = row.Split(';');
                        drinks.Add(new Drink()
                        {
                            Name = cell[0],
                            Cost = int.Parse(cell[1]),
                            Count = int.Parse(cell[2]),
                            ImgPath = cell[3]
                        });
                    }
                }
            }

            return drinks;
        }

        public bool IsAllowedExtension(string extension)
        {
            return extension.Contains(ExtensionsAllowed) ;
        }
    }
}