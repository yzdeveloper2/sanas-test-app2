using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using PrismApp.Models;
using PrismApp.Services.Interfaces;

namespace PrismApp.Services
{
    public class UserService : IUserService
    {
        private string _url;

        public UserService(string url)
        {
            _url = url;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var json = await LoadFromUrl(_url);
            return ToUsers(json);
        }

        private static async Task<string> LoadFromUrl(string url)
        {
            var client = new WebClient();
            return await client.DownloadStringTaskAsync(url);
        }

        private IEnumerable<User> ToUsers(string json)
        {
            var jo = System.Text.Json.JsonDocument.Parse(json);
            foreach (var user in jo.RootElement.EnumerateArray())
            {
                yield return new User
                {
                    FirstName = GetFirstName(user),
                    LastName = GetLastName(user),
                    Title = GetTitle(user),
                    Id = GetId(user),
                    SupervisorId = GetSupervisorId(user)
                };
            }
        }

        private int? GetSupervisorId(JsonElement user)
        {
            return GetIntProperty(user, "supervisorId");
        }

        private int? GetId(JsonElement user)
        {
            return GetIntProperty(user, "id");
        }

        private string? GetTitle(JsonElement user)
        {
            return GetStringProperty(user, "title");
        }

        private string? GetFirstName(JsonElement user)
        {
            return GetStringProperty(user, "firstName");
        }

        private string? GetLastName(JsonElement user)
        { 
            return GetStringProperty(user, "lastName");
        }

        private string? GetStringProperty(JsonElement element, string name)
        {
            return element.TryGetProperty(name, out var value) ? value.ToString() : null;
        }

        private int? GetIntProperty(JsonElement element, string name)
        {
            return element.TryGetProperty(name, out var value) ? 
                value.ValueKind == JsonValueKind.Number ? value.GetInt32() : (int?)null : null;
        }
    }
}
