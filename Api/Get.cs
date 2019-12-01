using System.Threading;
using System.Threading.Tasks;
using ASimpleBlog.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASimpleBlog.Features
{
    public class Get
    {
        public class Query : IRequest<Model>
        {
            public string Slug { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
            public string Body { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Model>
        {
            private readonly ApplicationDbContext _dbContext;

            public QueryHandler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var post = await _dbContext.Posts.SingleOrDefaultAsync(x => x.Slug == request.Slug);
                return new Model
                {
                    Id = post.Id,
                    Body = post.Body,
                    Slug = post.Slug,
                    Title = post.Title
                };
            }
        }
    }
}
