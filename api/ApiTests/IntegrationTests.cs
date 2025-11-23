using System.Net;
using System.Net.Http.Json;
using Api.Dtos.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ApiTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Api.Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Api.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_WithValidAdminCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Username = "admin",
                Password = "adminPassword123" // Ensure this user exists in your seeded DB
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Auth", loginRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result.Token));
            Assert.Equal("Admin", result.Role);
        }

        [Fact]
        public async Task GetFiles_PublicEndpoint_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/Files/list");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UploadFile_WithoutToken_ReturnsUnauthorized()
        {
            // Arrange
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("1"), "CourseId");
            content.Add(new ByteArrayContent(new byte[10]), "File", "test.txt");

            // Act
            var response = await _client.PostAsync("/api/Files/upload", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        // Helper to get token
        private async Task<string> AuthenticateAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("/Auth", new LoginRequest { Username = username, Password = password });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result!.Token;
        }

        [Fact]
        public async Task CreateMajor_AsAdmin_ReturnsCreated()
        {
            // Arrange
            var token = await AuthenticateAsync("admin", "adminPassword123");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var majorDto = new 
            { 
                MajorName = "Integration Test Major " + Guid.NewGuid(), 
                UniversityID = 1 
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Majors", majorDto);

            // Assert
            // NOTE: If MajorsController is unsecured, this might pass even without token!
            // But we expect it to be secured or at least work.
            response.EnsureSuccessStatusCode();
        }
    }

    // DTOs for Tests
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
