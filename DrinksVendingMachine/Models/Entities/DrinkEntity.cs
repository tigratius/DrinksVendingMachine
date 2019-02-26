using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Models.Entities
{
    public class DrinkEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CostPrice { get; set; }
        public string ImagePath { get; set; }
        public int Count { get; set; }
    }
}