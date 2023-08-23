using Hotel_Management_Razor.UI.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Core.Repository.GenericRepo
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly HotelManagementContext context;
        protected DbSet<TEntity> dbSet;

        public GenericRepository(HotelManagementContext context = null)
        {
            this.context = context ?? new HotelManagementContext();
            dbSet = this.context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void CreateRange(TEntity entity)
        {
            dbSet.AddRange(entity);
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(int id)
        {
            dbSet.Remove(dbSet.Find(id));
        }

        public TEntity Find(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(TEntity entity)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<TEntity> SearchByName(Func<TEntity, string> nameSelector, string name)
        {
            var entities = dbSet.ToList();
            return entities.Where(entity => nameSelector(entity).Contains(name)).ToList();
        }
    }
}
