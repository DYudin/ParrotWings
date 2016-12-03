using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TransactionSubsystem.Repositories.Abstract
{
    public interface IEntityRepository<T> where T : class, new()
    {
        //IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> GetAll();

        //T GetSingle(int id);
        T GetSingle(Expression<Func<T, bool>> predicate);
        //T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        //Task<T> GetSingleAsync(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Commit();
    }
}
