using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Classes.Entities
{
    public enum ValueCoins
    { One = 1, Two = 2, Five = 5, Ten = 10}

    public class CoinEntity
    {
        [Key]
        public Guid Id { get; set; }
        public ValueCoins Value { get; set; }
        public int Count { get; set; }
        public bool IsBlocking { get; set; }
    }
}