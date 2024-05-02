using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using System.Text;

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
                    <li><a href="/keys">List Data Protection Keys</a></li>
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

app.MapGet("/keys", (IKeyRingProvider keyRingProvider, IKeyManager keyManager) => {
    var defaultKeyId = keyRingProvider.GetCurrentKeyRing().DefaultKeyId;
    var allKeys = keyManager.GetAllKeys();
    var defaultKey = allKeys.Single(k => k.KeyId == defaultKeyId);

    var builder = new StringBuilder();
    builder.AppendLine($"Machine Name: {Environment.MachineName}");
    builder.AppendLine();

    builder.AppendLine($"**Default Key**");
    AppendKey(builder, defaultKey);
    builder.AppendLine();

    builder.AppendLine($"**Other Keys**");
    foreach (var key in allKeys)
    {
        if (key.KeyId == defaultKeyId)
        {
            continue;
        }
        AppendKey(builder, key);
    }

    return builder.ToString();

    static void AppendKey(StringBuilder builder, IKey key)
    {
        builder.AppendLine($"Key ID: {key.KeyId:B}");
        builder.AppendLine($"Creation Date (PDT): {key.CreationDate.ToOffset(TimeSpan.FromHours(-7))}");
        builder.AppendLine($"Activation Date (PDT): {key.ActivationDate.ToOffset(TimeSpan.FromHours(-7))}");
        builder.AppendLine($"Expiration Date (PDT): {key.ExpirationDate.ToOffset(TimeSpan.FromHours(-7))}");
        builder.AppendLine();
    }
});


app.Run();


record SecretAndInstance(string Secret, string Id);