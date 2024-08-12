using Digi.Data.Domain;
using Digi.Data.GenericRepository;

namespace Digi.Data.UnitOfWork;

public interface IUnitOfWork
{
    Task Complete(); 
    IGenericRepository<Product> ProductRepository { get; }
    IGenericRepository<Category> CategoryRepository { get; }
    IGenericRepository<Order> OrderRepository { get; }
    IGenericRepository<OrderDetail> OrderDetailRepository { get; }
    IGenericRepository<Coupon> CouponRepository { get; }
    IGenericRepository<ProductOrderDetail> ProductOrderDetailRepository { get; }
}