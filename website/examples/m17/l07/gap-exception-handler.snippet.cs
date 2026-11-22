builder.Services.AddProblemDetails();

var app = builder.Build();

// An unhandled exception becomes a 500 problem response.
app.___();
// An empty 404 response gets a problem body.
app.UseStatusCodePages();
