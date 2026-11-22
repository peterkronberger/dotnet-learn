using Microsoft.AspNetCore.Http.HttpResults;

// The handler answers 200 with the value, or 404.
static Results<Ok<string>, ___> Find(string? name) =>
    name is null
        ? TypedResults.NotFound()
        : TypedResults.Ok(name);
