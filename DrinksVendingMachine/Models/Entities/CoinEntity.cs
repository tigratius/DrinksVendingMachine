using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Models.Entities
{

    public enum ValueCoins
    { One = 1, Two = 2, Five = 5, Ten = 10}

    public class CoinEntity
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Достоинство монеты
        /// </summary>
        public ValueCoins Value { get; set; }
        /// <summary>
        /// Кол-во
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Блокировка
        /// </summary>
        public bool IsBlocking { get; set; }
    }
}