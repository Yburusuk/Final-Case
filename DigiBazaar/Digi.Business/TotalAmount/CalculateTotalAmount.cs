using Digi.Data.Domain;
using Digi.Data.UnitOfWork;

namespace Digi.Business.TotalAmount;

public class CalculateTotalAmount : ICalculateTotalAmount
{
    private readonly IUnitOfWork unitOfWork;

    public CalculateTotalAmount(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task Calculate(long orderId)
    {
        var order = unitOfWork.OrderRepository.FirstOrDefault(x => x.Id == orderId && x.IsActive == true, "OrderDetail");
        
        var productOrderDetails = await unitOfWork.ProductOrderDetailRepository.Where(x=> x.OrderDetailId == order.OrderDetail.Id);
        
        if (productOrderDetails is null) order.TotalAmount = 0;
        else
        {
            order.TotalAmount = 0;
            foreach (var productOrderDetail in productOrderDetails)
            {
                var productId = productOrderDetail.ProductId;
                var productPrice = unitOfWork.ProductRepository.FirstOrDefault(x => x.Id == productId).Price;
                order.TotalAmount += productPrice;
            }    
        }
        
        unitOfWork.OrderRepository.Update(order);
        await unitOfWork.Complete();
    }
}