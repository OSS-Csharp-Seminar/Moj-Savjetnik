using Microsoft.JSInterop;
using MyAdvisor.Client.Models.Auth;
using MyAdvisor.Client.Models.Common;
using System.Net.Http.Json;

namespace MyAdvisor.Client.Services;

public class AuthService(HttpClient http, IJSRuntime js)
{
    public async Task RegisterAsync(string firstName, string lastName, string email, string password)
    {
        var res = await http.PostAsJsonAsync("/api/auth/register", new { firstName, lastName, email, password });
        if (!res.IsSuccessStatusCode)
        {
            var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(data?.Error ?? "Registration failed.");
        }
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var res = await http.PostAsJsonAsync("/api/auth/login", new { email, password });
        if (!res.IsSuccessStatusCode)
        {
            var data = await res.Content.ReadFromJsonAsync<ErrorResponse>();
            throw new Exception(data?.Error ?? "Invalid email or password.");
        }
        return (await res.Content.ReadFromJsonAsync<AuthResponse>())!;
    }

    public async Task SaveTokensAsync(AuthResponse tokens)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "accessToken", tokens.AccessToken);
        await js.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokens.RefreshToken);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await js.InvokeAsync<string?>("localStorage.getItem", "accessToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task LogoutAsync()
    {
        var refreshToken = await js.InvokeAsync<string?>("localStorage.getItem", "refreshToken");
        if (!string.IsNullOrEmpty(refreshToken))
        {
            try { await http.PostAsJsonAsync("/api/auth/revoke", new { refreshToken }); }
            catch { }
        }

        await js.InvokeVoidAsync("localStorage.removeItem", "accessToken");
        await js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
    }
}
