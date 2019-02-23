using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Classes.Entities
{
    public class DrinkEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CostPrice { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
    }
}