using MediatR;
using Digi.Base.Response;
using Digi.Schema;

namespace Digi.Business.Cqrs;

public record GetWalletBalanceQuery(string Username) : IRequest<ApiResponse<decimal>>;