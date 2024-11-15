namespace WorkSchedule.Api.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Role { get; set; }
        public UserModel()
        {
            Name = string.Empty;
            Lastname = string.Empty;
            Role = string.Empty;
        }
        public UserModel(string name, string lastname, string role)
        {
            Name = name;
            Lastname = lastname;
            Role = role;
        }
    }
}
