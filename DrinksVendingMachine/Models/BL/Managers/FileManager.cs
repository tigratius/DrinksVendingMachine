using System.Collections.Generic;
using System.IO;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.BL.Managers
{
    public class FileManager
    {
        private readonly IStrategy _contextStrategy;

        public FileManager(IStrategy strategy)
        {
            _contextStrategy = strategy;
            
        }

        public List<Drink> Import(Stream stream)
        {
            return _contextStrategy.Import(stream);
        }
    }
}