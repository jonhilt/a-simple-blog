using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASimpleBlog.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASimpleBlog.Features
{

    public class ListAll
    {
        public class Query : IRequest<Model>
        {
        }

        public class Model
        {
            public List<Post> Posts { get; set; } = new List<Post>();

            public class Post
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public string Slug { get; set; }
            }
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
                var posts = await _dbContext.Posts
                    .Select(x => new Model.Post
                    {
                        Id = x.Id,
                        Slug = x.Slug,
                        Title = x.Title
                    })
                    .ToListAsync(cancellationToken: cancellationToken);

                return new Model { Posts = posts };
            }
        }
    }
}
