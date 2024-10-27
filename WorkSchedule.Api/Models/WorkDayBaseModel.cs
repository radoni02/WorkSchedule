namespace WorkSchedule.Api.Models
{
    public class WorkDayBaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public WorkDayBaseModel()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
        }
        public WorkDayBaseModel(string name, string surname, string email, DateTime start, DateTime end)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Start = start;
            End = end;
        }
    }
}
