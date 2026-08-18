---
title: Controllers
---

# UseCase Controller

## UseCaseDTO
The data structure for a Use-Case.
```tsx title="UseCaseDTO.cs"
[Table("usecase")]
public class UseCaseDTO
{
    public string Guid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
```

## API Endpoints
### GET api/UseCases
Gets all UseCaseDTO's out of the Postgres DB.

### GET api/UseCases/(guid)
Gets a UseCaseDTO by Guid out of the Postgres DB.

### POST api/UseCases
Creates a new UseCaseDTO in the Postgres DB.

### PUT api/UseCases/(guid)
Updates a UseCaseDTO by Guid in the Postgres DB.

### DELETE api/UseCases/(guid)
Deletes a UseCaseDTO by Guid out of the Postgres DB.

## Patterns
- Dependency Injection
- Repository Pattern
- Command Pattern

## General functionality
When an API call to retrieve all UseCaseDTO's is being made, the UseCaseController calls the interface IUseCaseRepository.
```tsx title="UseCaseController.cs"
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UseCaseDTO>))]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[HttpGet]
public async Task<IActionResult> GetAllUseCasesAsync()
{
    try
    {
        var useCases = await _repository.GetAllUseCasesAsync();
        return Ok(useCases);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred while retrieving all use cases.");
        return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
    }
}
```

In the UseCaseRepository a GetAllUseCaseCommand is being created and executed.
The results of the command will be returned.


```tsx title="IUseCaseRepository.cs"
Task<IEnumerable<UseCaseDTO>> GetAllUseCasesAsync();
```

```tsx title="UseCaseRepository.cs"
public async Task<IEnumerable<UseCaseDTO>> GetAllUseCasesAsync()
{
    var getAllUseCasesCommand = new GetAllUseCasesCommand(_context);
    await getAllUseCasesCommand.ExecuteAsync();

    return getAllUseCasesCommand.Result;
}
```

The GetAllUseCasesCommand uses the DbContext (EntityFramework) to retrieve the UseCaseDTO's from the database.

UseCaseDbContext has a DbSet of UseCases and uses it to execute the query.
