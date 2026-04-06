using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MyAdvisor.Client.Models.Auth;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyAdvisor.Client.Handlers;

public class AuthHandler(IJSRuntime js, NavigationManager nav, Uri baseAddress) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Buffer body so it can be replayed after a token refresh retry
        if (request.Content is not null)
            await request.Content.LoadIntoBufferAsync();

        var token = await js.InvokeAsync<string?>("localStorage.getItem", "accessToken");
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var newToken = await TryRefreshAsync(cancellationToken);
            if (newToken is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                response = await base.SendAsync(request, cancellationToken);
            }
        }

        return response;
    }

    private async Task<string?> TryRefreshAsync(CancellationToken cancellationToken)
    {
        var refreshToken = await js.InvokeAsync<string?>("localStorage.getItem", "refreshToken");
        if (string.IsNullOrEmpty(refreshToken))
        {
            nav.NavigateTo("/login");
            return null;
        }

        using var req = new HttpRequestMessage(HttpMethod.Post, new Uri(baseAddress, "api/auth/refresh"));
        req.Content = JsonContent.Create(new { refreshToken });

        HttpResponseMessage res;
        try
        {
            res = await base.SendAsync(req, cancellationToken);
        }
        catch
        {
            return null;
        }

        if (!res.IsSuccessStatusCode)
        {
            await js.InvokeVoidAsync("localStorage.removeItem", "accessToken");
            await js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            nav.NavigateTo("/login");
            return null;
        }

        var tokens = await res.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken: cancellationToken);
        if (tokens is null) return null;

        await js.InvokeVoidAsync("localStorage.setItem", "accessToken", tokens.AccessToken);
        await js.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokens.RefreshToken);
        return tokens.AccessToken;
    }
}
