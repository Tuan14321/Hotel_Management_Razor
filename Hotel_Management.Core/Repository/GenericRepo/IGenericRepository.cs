namespace Hotel_Management.Core.Repository.GenericRepo
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Create TEntity
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        void CreateRange(TEntity entity);

        void Update(TEntity entity);

        void UpdateRange(TEntity entity);

        void Delete(TEntity entity);

        void Delete(int id);

        IEnumerable<TEntity> GetAll();

        TEntity Find(int id);
        IEnumerable<TEntity> SearchByName(Func<TEntity, string> nameSelector, string name);
    }
}
