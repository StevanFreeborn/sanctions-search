namespace SanctionsSearch.Worker.Persistence;

class CommentRepository(
  DbContext dbContext,
  ILogger<CommentRepository> logger
) : EfRepository<Comment>(dbContext, logger), ICommentRepository
{
}