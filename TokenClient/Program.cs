using Duende.IdentityModel.OidcClient;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var options = new OidcClientOptions
        {
    Authority = "https://localhost:5001",
    ClientId = "interactive",
    ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0", // if required
    Scope = "openid profile scope2",
    RedirectUri = "http://127.0.0.1:7890/",
    Browser = new SystemBrowser("http://127.0.0.1:7890/"),
    DisablePushedAuthorization = true // try this if you still get errors
};

        var oidcClient = new OidcClient(options);
        var result = await oidcClient.LoginAsync(new LoginRequest());

        if (result.IsError)
        {
            Console.WriteLine($"Error: {result.Error}");
            return;
        }

        Console.WriteLine($"Access Token: {result.AccessToken}");
    }
}
