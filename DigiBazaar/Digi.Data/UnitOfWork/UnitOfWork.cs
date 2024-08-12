using Digi.Data.Context;
using Digi.Data.Domain;
using Digi.Data.GenericRepository;

namespace Digi.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DataContext dbContext;
    
    public IGenericRepository<Product> ProductRepository { get; }
    public IGenericRepository<Category> CategoryRepository { get; }
    public IGenericRepository<Order> OrderRepository { get; }
    public IGenericRepository<OrderDetail> OrderDetailRepository { get; }
    public IGenericRepository<Coupon> CouponRepository { get; }
    public IGenericRepository<ProductOrderDetail> ProductOrderDetailRepository { get; }
    
    public UnitOfWork(DataContext dbContext)
    {
        this.dbContext = dbContext;
        
        ProductRepository = new GenericRepository<Product>(this.dbContext);
        CategoryRepository = new GenericRepository<Category>(this.dbContext);
        OrderRepository = new GenericRepository<Order>(this.dbContext);
        OrderDetailRepository = new GenericRepository<OrderDetail>(this.dbContext);
        CouponRepository = new GenericRepository<Coupon>(this.dbContext);
        ProductOrderDetailRepository = new GenericRepository<ProductOrderDetail>(this.dbContext);
    }

    public void Dispose()
    {
    }
    public async Task Complete()
    {
        using (var dbTransaction = await dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                await dbContext.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}