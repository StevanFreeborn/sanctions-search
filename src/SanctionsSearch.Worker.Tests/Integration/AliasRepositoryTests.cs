namespace SanctionsSearch.Worker.Tests.Integration;

public class AliasRepositoryTests : RepositoryTest
{
  private readonly AliasRepository _repository;
  private readonly AliasFaker _faker = new();

  public AliasRepositoryTests()
  {
    var loggerFactory = LoggerFactory.Create(builder => builder.ClearProviders());
    _repository = new(_context, new Logger<AliasRepository>(loggerFactory));
  }

  [Fact]
  public async Task Upsert_WithNewEntity_ShouldAddEntityToDatabase()
  {
    var now = DateTimeOffset.UtcNow;
    _timeProvider.Setup(x => x.GetUtcNow()).Returns(now);

    var entity = _faker.Generate();

    await _repository.Upsert(entity);
    await _context.SaveChangesAsync();

    var result = await _context.Set<Alias>().FindAsync(entity.Id);

    result.Should().BeEquivalentTo(entity);
    result!.CreatedAt.Should().Be(now.DateTime);
    result.UpdatedAt.Should().Be(now.DateTime);
  }

  [Fact]
  public async Task Upsert_WithExistingEntity_ShouldUpdateEntityInDatabase()
  {
    var createdTimeStamp = DateTimeOffset.UtcNow;
    var updatedTimeStamp = createdTimeStamp.AddSeconds(2);

    _timeProvider.Setup(x => x.GetUtcNow()).Returns(createdTimeStamp);

    var entity = _faker.Generate();

    // Insert the entity
    await _repository.Upsert(entity);
    await _context.SaveChangesAsync();

    // Assert the entity was created
    var createdEntity = await _context.Set<Alias>().FindAsync(entity.Id);

    createdEntity.Should().BeEquivalentTo(entity);
    createdEntity!.CreatedAt.Should().Be(createdTimeStamp.DateTime);
    createdEntity.UpdatedAt.Should().Be(createdTimeStamp.DateTime);

    _timeProvider.Setup(x => x.GetUtcNow()).Returns(updatedTimeStamp);

    // Update the entity
    entity.Remarks = "Updated";
    await _repository.Upsert(entity);
    await _context.SaveChangesAsync();

    // Assert the entity was updated
    var result = await _context.Set<Alias>().FindAsync(entity.Id);

    result.Should().BeEquivalentTo(entity);
    result!.CreatedAt.Should().BeSameDateAs(createdTimeStamp.DateTime);
    result.UpdatedAt.Should().BeSameDateAs(updatedTimeStamp.DateTime);
  }

  [Fact]
  public async Task Find_WithPredicate_ShouldReturnEntitiesMatchingPredicate()
  {
    var entities = _faker.Generate(3);

    foreach (var entity in entities)
    {
      await _repository.Upsert(entity);
    }

    await _context.SaveChangesAsync();

    var target = entities[0];
    var result = await _repository.Find(x => x.Id == target.Id);

    result.Should().HaveCount(1);
    result.First().Should().BeEquivalentTo(target);
  }
}