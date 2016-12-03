using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TransactionSubsystem.Repositories.Abstract;

namespace TransactionSubsystem.Repositories.Implementation
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, new()
    {
        private TransactionSubsystemContext _context;

       public EntityRepository(TransactionSubsystemContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsEnumerable();
        }
        //public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    return query.AsEnumerable();
        //}

        //public virtual async Task<IEnumerable<T>> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }
        //    return await query.ToListAsync();
        //}
        //public T GetSingle(int id)
        //{
        //    return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        //}

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        //public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        //{
        //    IQueryable<T> query = _context.Set<T>();
        //    foreach (var includeProperty in includeProperties)
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    return query.Where(predicate).FirstOrDefault();
        //}

        //public async Task<T> GetSingleAsync(int id)
        //{
        //    return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        //}
        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry<T>(entity);
            _context.Set<T>().Add(entity);
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
        }

    }
}
