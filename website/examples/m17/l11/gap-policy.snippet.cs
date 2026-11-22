builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ChangeIncidentStatus", policy =>
        policy.___("responder"));
