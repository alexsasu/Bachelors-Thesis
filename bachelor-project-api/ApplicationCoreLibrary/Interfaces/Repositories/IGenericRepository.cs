namespace ApplicationCoreLibrary.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        void Create(TEntity entity);

        void CreateRange(IEnumerable<TEntity> entities);

        Task<TEntity?> GetByIdAsync(int? id);

        IQueryable<TEntity> GetAll();

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);

        Task<bool> SaveAsync();
    }
}
