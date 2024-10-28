namespace WorkSchedule.Api.Models
{
    public class RoleModel
    {
        public string Name { get; set; }
        public RoleModel()
        {
            Name = string.Empty;
        }
        public RoleModel(string name)
        {
            Name = name;
        }
    }
}
