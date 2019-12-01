using ASimpleBlog.Data;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ASimpleBlog.Api
{

    public class Add
    {
        public class Command : IRequest
        {          
            public string Title { get; set; }            
            public string Slug { get; set; }            
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
                await _dbContext.Posts.AddAsync(new Post
                {
                    Body = command.Body,
                    Slug = command.Slug,
                    Title = command.Title
                }, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
