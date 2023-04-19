namespace RealEstate.Data.Repositories.Base
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using RealEstate.Data.Interfaces;

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> 
        where TEntity : class
    {
        protected readonly ApplicationDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public void Add(TEntity entity) => this.dbSet.Add(entity);

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
            => this.dbSet.Where(expression).AsNoTracking();

        public IQueryable<TEntity> All() => this.dbSet;

        public void Remove(TEntity entity) => this.dbSet.Remove(entity);

        public void Update(TEntity entity) => this.dbSet.Update(entity);

        public void SaveChanges() => this.context.SaveChanges();
    }
}
