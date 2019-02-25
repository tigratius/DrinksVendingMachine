using System;
using System.Collections.Generic;
using System.IO;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.BL.Importers
{
    public class CsvImport : IStrategy
    {
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
                            Cost = Int32.Parse(cell[1]),
                            Count = Int32.Parse(cell[2]),
                            ImgPath = cell[3]
                        });
                    }
                }
            }

            return drinks;
        }
    }

    
}