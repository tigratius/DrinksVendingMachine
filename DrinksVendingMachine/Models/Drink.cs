using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinksVendingMachine.Models
{

    //TODO: возможно не нужно
    public class Drink
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CostPrice { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
    }
}