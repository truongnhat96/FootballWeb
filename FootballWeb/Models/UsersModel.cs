using Microsoft.AspNetCore.Identity;

namespace FootballWeb.Models
{
    public class UsersModel
    {
        public string UserName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];
    }
}
