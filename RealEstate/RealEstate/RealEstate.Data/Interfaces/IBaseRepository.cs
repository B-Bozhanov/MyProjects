namespace RealEstate.Data.Interfaces
{
    using System.Linq.Expressions;

    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void SaveChanges();
    }
}
