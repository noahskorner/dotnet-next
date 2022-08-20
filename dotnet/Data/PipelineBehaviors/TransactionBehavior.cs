using Data;
using MediatR;

namespace Data.PipelineBehaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ApiContext _context;

        public TransactionBehavior(ApiContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var result = await next();

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
    }
}
