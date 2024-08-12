using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Business.TotalAmount;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Command;

public class OrderDetailCommandHandler :
    IRequestHandler<AddProductToOrderDetailCommand, ApiResponse<OrderDetailResponse>>,
    IRequestHandler<RemoveProductFromOrderDetailCommand, ApiResponse<OrderDetailResponse>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public OrderDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<OrderDetailResponse>> Handle(AddProductToOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var product = unitOfWork.ProductRepository.FirstOrDefault(x=> x.Id == request.Request.ProductId && x.IsActive == true);
        
        var orderDetail = unitOfWork.OrderDetailRepository.FirstOrDefault(x=> x.UserName == request.Request.UserName && x.IsActive == true, "Products", "Order");
        
        var productOrderDetail = new ProductOrderDetail
        {
            ProductId = product.Id,
            OrderDetailId = orderDetail.Id
        };
        
        await unitOfWork.ProductOrderDetailRepository.Insert(productOrderDetail);
        
        await unitOfWork.Complete();

        var order = unitOfWork.OrderRepository.FirstOrDefault(x => x.Id == orderDetail.OrderId && x.IsActive == true, "OrderDetail");
        
        var calculate = new CalculateTotalAmount(unitOfWork);

        await calculate.Calculate(order.Id);

        var response = mapper.Map<OrderDetailResponse>(orderDetail);
        
        return new ApiResponse<OrderDetailResponse>(response);
    }

    public async Task<ApiResponse<OrderDetailResponse>> Handle(RemoveProductFromOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var product = unitOfWork.ProductRepository.FirstOrDefault(x=> x.Id == request.Request.ProductId && x.IsActive == true);
        
        var orderDetail = unitOfWork.OrderDetailRepository.FirstOrDefault(x=> x.UserName == request.Request.UserName && x.IsActive == true, "Products");

        var productOrderDetail = unitOfWork.ProductOrderDetailRepository.FirstOrDefault(x => x.OrderDetailId == orderDetail.Id && x.ProductId == product.Id);
        
        await unitOfWork.ProductOrderDetailRepository.Delete(productOrderDetail.Id);
        
        await unitOfWork.Complete();

        var order = unitOfWork.OrderRepository.FirstOrDefault(x => x.Id == orderDetail.OrderId && x.IsActive == true, "OrderDetail");
        
        var calculate = new CalculateTotalAmount(unitOfWork);
        
        await calculate.Calculate(order.Id);

        var response = mapper.Map<OrderDetailResponse>(orderDetail);
        
        return new ApiResponse<OrderDetailResponse>(response);
    }
}