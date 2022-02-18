namespace QLess.Core.Interface
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<bool> CreateAsync(TEntity entity);
		bool Create(TEntity entity, out long id);
		Task<bool> DeleteAsync(long id);
		Task<IQueryable<TEntity>> FindAllAsync();
		Task<TEntity> FindByIdAsync(long id);
		Task<bool> UpdateAsync(TEntity entity);
	}
}
