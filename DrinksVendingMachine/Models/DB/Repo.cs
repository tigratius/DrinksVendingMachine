using System;
using System.Data.Entity;
using System.Linq;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.DB
{
    public class DrinkRepo : Repo, IRepository<DrinkEntity>
    {
        private bool _isDisposed = false;

        public DrinkRepo(VengingMachineDbContext context) : base(context)
        {
        }

        public void Add(DrinkEntity drink)
        {
             Db.DrinkEntities.Add(drink);
        }

        public void Remove(DrinkEntity drink)
        {
            Db.DrinkEntities.Remove(drink);
        }
        
        public DrinkEntity Get(Guid id)
        {
            return Db.DrinkEntities.First(c => c.Id == id);
        }

        public IQueryable<DrinkEntity> Queryable()
        {
            return Db.DrinkEntities;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            _isDisposed = true;

            base.Dispose(disposing);
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }
    }

    public class CoinsRepo : Repo, IRepository<CoinEntity>
    {
        private bool _isDisposed = false;

        public CoinsRepo(VengingMachineDbContext context) : base(context)
        {
        }

        public void Add(CoinEntity coin)
        {
            Db.CoinsEntities.Add(coin);
        }

        public void Remove(CoinEntity coin)
        {
            Db.CoinsEntities.Remove(coin);
        }

        public CoinEntity Get(Guid id)
        {
            return Db.CoinsEntities.First(c => c.Id == id);
        }

        public IQueryable<CoinEntity> Queryable()
        {
            return Db.CoinsEntities;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            _isDisposed = true;

            base.Dispose(disposing);
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }
    }

    public class StateRepo : Repo, ISimpleRepo<CurrentStateEntity>
    {
        private bool _isDisposed = false;

        public StateRepo(VengingMachineDbContext context) : base(context)
        {
        }

        public CurrentStateEntity GetFirst()
        {
            return Db.CurrentStateEntities.First();
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            _isDisposed = true;

            base.Dispose(disposing);
        }

        public void SaveChanges()
        {
            Db.SaveChanges();
        }
    }

    public abstract class Repo : IDisposable
    {
        protected readonly VengingMachineDbContext Db;

        private bool _isDisposed = false;

        protected Repo(VengingMachineDbContext context)
        {
            Db = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                Db?.Dispose();
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repo()
        {
            Dispose(false);
        }
    }
}