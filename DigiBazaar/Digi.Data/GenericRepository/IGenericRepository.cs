using System.Linq.Expressions;

namespace Digi.Data.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task Save();
    Task<TEntity?> GetById(long id,params string[] includes);
    Task<TEntity> Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task Delete(long id);
    Task<List<TEntity>> GetAll(params string[] includes);
    Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression,params string[] includes);
    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression,params string[] includes);
}