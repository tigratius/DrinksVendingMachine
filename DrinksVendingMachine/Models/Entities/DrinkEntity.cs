using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Models.Entities
{
    public class DrinkEntity
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Название напитка
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public int CostPrice { get; set; }
        /// <summary>
        /// Виртуальный путь до файла
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Кол-во
        /// </summary>
        public int Count { get; set; }
    }
}