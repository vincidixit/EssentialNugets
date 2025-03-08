using System.Text.Json.Serialization;

namespace UserService.Services.Types
{
    public class User
    {
        public string Name { get; set; }

        public string Location { get; set; }
    }
}
