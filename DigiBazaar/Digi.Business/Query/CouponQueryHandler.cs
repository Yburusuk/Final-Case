using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Query;

public class CouponQueryHandler : 
    IRequestHandler<GetAllCouponQuery, ApiResponse<List<CouponResponse>>>

{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CouponQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CouponResponse>>> Handle(GetAllCouponQuery request, CancellationToken cancellationToken)
    {
        var entityList = await unitOfWork.CouponRepository.GetAll();
        var mappedList = mapper.Map<List<CouponResponse>>(entityList);
        
        return new ApiResponse<List<CouponResponse>>(mappedList);
    }
}