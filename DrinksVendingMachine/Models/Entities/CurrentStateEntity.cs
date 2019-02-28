using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksVendingMachine.Models.Entities
{
    /// <summary>
    /// Тыблица текущего состояния автомата
    /// насчет это таблицы не уверен возможно, нужно было добавить признак в поле Coins  и по нему считать
    /// </summary>
    public class CurrentStateEntity
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Внесенные средства
        /// </summary>
        public int Deposit { get; set; }
        /// <summary>
        /// сдача
        /// </summary>
        public int Change { get; set; }
    }
}