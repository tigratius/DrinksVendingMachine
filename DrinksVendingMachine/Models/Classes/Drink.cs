using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.Classes
{
    public class Drink
    {
        public string ImgPath { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }
        public string Name { get; set; }
    }

    public class DrinkOperationInfo
    {
        public DrinkEntity Drink { get; set; }
        public string Msg { get; set; }
        public bool Success { get; set; }
        public int Change { get; set; }
    }
}