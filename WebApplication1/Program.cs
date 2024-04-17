using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Content(
    """
        <html>
            <body>
                <h1>ASP.NET Core 6.0</h1>
                <p>Protect and Unprotect Data</p>
                <ul>
                    <li><a href="/protect">Protect Data</a></li>
                    <li><a href="/unprotect">Unprotect Data</a></li>
                </ul>
            </body>
    """, 
    "text/html");
});

app.MapGet("/protect", (IDataProtectionProvider protectionProvider) =>
{
    var protector = protectionProvider.CreateProtector("MyApp");
    var protectedData = protector.Protect("Hello, world!");
    return new SecretAndInstance(protectedData, Environment.MachineName);
});

app.MapGet("/unprotect/{secret}", (string secret, IDataProtectionProvider protectionProvider) =>
{
    var protector = protectionProvider.CreateProtector("MyApp");
    var unprotectedData = protector.Unprotect(secret);
    return new SecretAndInstance(unprotectedData, Environment.MachineName);
});


app.Run();


record SecretAndInstance(string Secret, string Id);