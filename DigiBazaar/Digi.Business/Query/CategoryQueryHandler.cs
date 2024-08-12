using AutoMapper;
using MediatR;
using Digi.Base;
using Digi.Base.Response;
using Digi.Business.Cqrs;
using Digi.Data.Domain;
using Digi.Data.UnitOfWork;
using Digi.Schema;

namespace Digi.Business.Query;

public class CategoryQueryHandler : 
    IRequestHandler<GetAllCategoryQuery, ApiResponse<List<CategoryResponse>>>

{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<List<CategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var entityList = await unitOfWork.CategoryRepository.GetAll();
        var mappedList = mapper.Map<List<CategoryResponse>>(entityList);
        
        return new ApiResponse<List<CategoryResponse>>(mappedList);
    }
}