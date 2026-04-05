using Microsoft.JSInterop;
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
            throw new Exception(data?.Error ?? "Registration failed");
        }
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var res = await http.PostAsJsonAsync("/api/auth/login", new { email, password });
        if (!res.IsSuccessStatusCode)
            throw new Exception("Invalid email or password");
        return (await res.Content.ReadFromJsonAsync<AuthResponse>())!;
    }

    public async Task SaveTokensAsync(AuthResponse tokens)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "accessToken", tokens.AccessToken);
        await js.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokens.RefreshToken);
    }
}

public record AuthResponse(string AccessToken, string RefreshToken);

public record ErrorResponse(string Error);
