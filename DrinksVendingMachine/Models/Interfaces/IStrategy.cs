using System.Collections.Generic;
using System.IO;
using DrinksVendingMachine.Models.Classes;

namespace DrinksVendingMachine.Models.Interfaces
{
    public interface IStrategy
    {
        List<Drink> Import(Stream stream);

        bool IsAllowedExtension(string extension);
    }
}
