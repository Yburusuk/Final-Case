using AutoMapper;
using MediatR;
using Digi.Base;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Query;

public class OrderDetailQueryHandler : 
    IRequestHandler<GetOrderDetailQuery, ApiResponse<List<OrderDetailResponse>>>,
    IRequestHandler<GetAllOrderDetailQuery, ApiResponse<List<OrderDetailResponse>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public OrderDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<OrderDetailResponse>>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.OrderDetailRepository.Where(x => x.UserName == request.Username && x.IsActive == true, "Order","Products");
        
        var response = mapper.Map<List<OrderDetailResponse>>(entity);
        
        return new ApiResponse<List<OrderDetailResponse>>(response);
    }

    public async Task<ApiResponse<List<OrderDetailResponse>>> Handle(GetAllOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await unitOfWork.OrderDetailRepository.GetAll("Order", "Products");
        
        var response = mapper.Map<List<OrderDetailResponse>>(entity);
        
        return new ApiResponse<List<OrderDetailResponse>>(response);
    }
}