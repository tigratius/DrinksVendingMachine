using System.Collections.Generic;
using System.IO;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Classes.Managers
{
    //Todo: подумать над названием
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

        public bool IsAllowedExtension(string ext)
        {
            return _contextStrategy.IsAllowedExtension(ext);
        }
    }
}