using AutoMapper;
using MediatR;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Command;

public class CouponCommandHandler :
    IRequestHandler<CreateCouponCommand, ApiResponse<CouponResponse>>,
    IRequestHandler<GenerateCouponCommand, ApiResponse<CouponResponse>>,
    IRequestHandler<DeleteCouponCommand, ApiResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CouponCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<CouponResponse>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var existingCoupon = unitOfWork.CouponRepository.FirstOrDefault(x => x.CouponCode == request.Request.CouponCode);
        if (existingCoupon != null)
        {
            throw new ApiResponse("Coupon code already exists");
        }
        
        var mapped = mapper.Map<CouponRequest, Coupon>(request.Request);
        var entity = await unitOfWork.CouponRepository.Insert(mapped);
        await unitOfWork.Complete();
        
        var response = mapper.Map<CouponResponse>(entity);
        
        return new ApiResponse<CouponResponse>(response);
    }
    
    public async Task<ApiResponse<CouponResponse>> Handle(GenerateCouponCommand request, CancellationToken cancellationToken)
    {
        var coupon = new Coupon
        {
            CouponCode = Guid.NewGuid().ToString("N").Substring(0, 10),
            CouponAmount = request.Request.CouponAmount,
            ExpireDate = request.Request.ExpireDate
        };

        var entity= await unitOfWork.CouponRepository.Insert(coupon);
        await unitOfWork.Complete();
        
        var response = mapper.Map<CouponResponse>(entity);
        
        return new ApiResponse<CouponResponse>(response);
    }

    public async Task<ApiResponse> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var entity = unitOfWork.CouponRepository.FirstOrDefault(x => x.Id == request.CouponId);
        
        unitOfWork.CouponRepository.Delete(entity);
        await unitOfWork.Complete();
        
        return new ApiResponse();
    }

    
}