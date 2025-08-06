using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Duende.IdentityModel.OidcClient.Browser;

public class SystemBrowser : IBrowser
{
    private readonly string _redirectUri;
    public SystemBrowser(string redirectUri)
    {
        _redirectUri = redirectUri;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, System.Threading.CancellationToken cancellationToken = default)
    {
        using (var listener = new HttpListener())
        {
            listener.Prefixes.Add(_redirectUri);
            listener.Start();

            Process.Start(new ProcessStartInfo(options.StartUrl) { UseShellExecute = true });

            var context = await listener.GetContextAsync();

            var response = context.Response;
            string responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://localhost'></head><body>Please return to the app.</body></html>";
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            responseOutput.Close();

            return new BrowserResult
            {
                ResultType = BrowserResultType.Success,
                Response = context.Request.Url.ToString()
            };
        }
    }
}