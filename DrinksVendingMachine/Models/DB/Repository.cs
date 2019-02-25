using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DrinksVendingMachine.Models.DB
{
    public abstract class Repository<T>
    {
        public abstract T Add(T add);
        public abstract T Remove(T add);
        public abstract IQueryable<T> Queryable();
    }

    public class DbSetRepository<T> : Repository<T> where T : class
    {
        public DbSetRepository(IDbSet<T> dbSet)
        {
            _dbSet = dbSet;
        }

        private readonly IDbSet<T> _dbSet;

        public override T Add(T obj)
        {
            _dbSet.Add(obj);
            return obj;
        }

        public override T Remove(T obj)
        {
            _dbSet.Remove(obj);
            return obj;
        }

        public override IQueryable<T> Queryable()
        {
            return _dbSet;
        }
    }

    public class FakeRepository<T> : Repository<T>
    {
        public FakeRepository(List<T> list)
        {
            _list = list;
            _listNew = new List<T>(list);
        }

        public FakeRepository()
            : this(new List<T>())
        {
        }

        private readonly List<T> _list;
        private readonly List<T> _listNew;

        public override T Add(T obj)
        {
            _listNew.Add(obj);
            return obj;
        }

        public override T Remove(T obj)
        {
            _listNew.Remove(obj);
            return obj;
        }

        public override IQueryable<T> Queryable()
        {
            return GetList().AsQueryable();
        }

        public List<T> GetList()
        {
            return _list.ToArray().ToList();
        }

        public void SaveChanges()
        {
            lock (this)
            {
                _list.Clear();
                _list.AddRange(_listNew);
            }
        }
    }
}