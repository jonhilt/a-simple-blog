using ASimpleBlog.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ASimpleBlog.Api
{

    public class Edit
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Slug { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Model>
        {
            ApplicationDbContext _dbContext;

            public QueryHandler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var existing = await _dbContext.Posts.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                return new Model
                {
                    Id = existing.Id,
                    Body = existing.Body,
                    Slug = existing.Slug,
                    Title = existing.Title
                };
            }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            ApplicationDbContext _dbContext;

            public CommandHandler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            protected override async Task Handle(Command command, CancellationToken cancellationToken)
            {
                var existing = await _dbContext.Posts.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
                existing.Body = command.Body;
                existing.Title = command.Title;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
