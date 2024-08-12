using AutoMapper;
using Digi.Base.Response;
using Digi.Base.Sessions;
using Digi.Business.Cqrs;
using Digi.Data.UnitOfWork;
using Digi.Schema;
using MediatR;

namespace Digi.Business.Query;

public class WalletBalanceQueryHandler : 
    IRequestHandler<GetWalletBalanceQuery, ApiResponse<decimal>>

{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly ISessionContext sessionContext;

    public WalletBalanceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ISessionContext sessionContext)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.sessionContext = sessionContext;
    }

    public async Task<ApiResponse<decimal>> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
    {
        var walletBalance = sessionContext.Session.WalletBalance;
        
        return new ApiResponse<decimal>(walletBalance);
    }
}