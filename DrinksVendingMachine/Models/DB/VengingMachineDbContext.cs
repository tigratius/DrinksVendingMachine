using System.Data.Entity;
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.DB
{
    public class VengingMachineDbContext : DbContext
    {
        public DbSet<DrinkEntity> DrinkEntities { get; set; }
        public DbSet<CoinEntity> CoinsEntities { get; set; }
        public DbSet<CurrentStateEntity> CurrentStateEntities { get; set; }
    }
}