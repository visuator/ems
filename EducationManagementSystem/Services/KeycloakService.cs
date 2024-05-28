using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace EducationManagementSystem.Services;

public class RegisterDto
{
    public string Email { get; set; } = default!;
}
public class KeycloakService(IConfiguration configuration, HttpClient keycloakHttpClient)
{
    private class UserRepresentation
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; } = default!;
        [JsonPropertyName("email")]
        public string Email { get; set; } = default!;
    }
    private class AuthorizationResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;
    }
    public async Task<Guid> Register(RegisterDto dto)
    {
        await Authorize();
        var generatedId = Guid.NewGuid();
        var response = await keycloakHttpClient.PostAsJsonAsync(configuration.GetSection("Keycloak:CreateUserEndpoint").Get<string>(), new UserRepresentation()
        {
            Id = generatedId,
            Username = dto.Email,
            Email = dto.Email
        });
        response.EnsureSuccessStatusCode();
        return generatedId;
    }
    private async Task Authorize()
    {
        var response =
            await keycloakHttpClient.PostAsync(configuration.GetSection("Keycloak:TokenEndpoint").Get<string>(),
                new FormUrlEncodedContent([
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", configuration.GetSection("Keycloak:ClientId").Get<string>()),
                    new KeyValuePair<string, string>("client_secret", configuration.GetSection("Keycloak:ClientSecret").Get<string>()),
                ]));
        response.EnsureSuccessStatusCode();
        var dto = await response.Content.ReadFromJsonAsync<AuthorizationResponse>();
        keycloakHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",dto.AccessToken);
    }
}