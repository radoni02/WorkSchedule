namespace WorkSchedule.Api.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Role { get; set; }
        public UserModel()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Lastname = string.Empty;
            Role = string.Empty;
        }
        public UserModel(string name, string lastname, string role, Guid id)
        {
            Id = id;
            Name = name;
            Lastname = lastname;
            Role = role;
        }
    }
}
