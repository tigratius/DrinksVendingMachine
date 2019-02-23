using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Classes.Entities
{
    public class CurrentStateEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int Deposit { get; set; }
        public int Change { get; set; }
    }
}