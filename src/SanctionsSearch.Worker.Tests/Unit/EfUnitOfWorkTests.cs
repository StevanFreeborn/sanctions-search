namespace SanctionsSearch.Worker.Tests.Unit;

public class EfUnitOfWorkTests
{
  private readonly Mock<DbContext> _context = new();
  private readonly Mock<ILoggerFactory> _loggerFactory = new();
  private readonly EfUnitOfWork _unitOfWork;

  public EfUnitOfWorkTests()
  {
    _unitOfWork = new(_context.Object, _loggerFactory.Object);
  }

  [Fact]
  public void Sdns_WhenCalled_ShouldReturnSdnRepository()
  {
    _unitOfWork.Sdns.Should().BeOfType<SdnRepository>();
  }

  [Fact]
  public void Addresses_WhenCalled_ShouldReturnAddressRepository()
  {
    _unitOfWork.Addresses.Should().BeOfType<AddressRepository>();
  }

  [Fact]
  public void Aliases_WhenCalled_ShouldReturnAliasRepository()
  {
    _unitOfWork.Aliases.Should().BeOfType<AliasRepository>();
  }

  [Fact]
  public void Comments_WhenCalled_ShouldReturnCommentRepository()
  {
    _unitOfWork.Comments.Should().BeOfType<CommentRepository>();
  }
}