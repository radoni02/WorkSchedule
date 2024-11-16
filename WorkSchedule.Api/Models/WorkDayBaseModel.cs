namespace WorkSchedule.Api.Models
{
    public class WorkDayBaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid UserId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public WorkDayBaseModel()
        {
            Name = string.Empty;
            Surname = string.Empty;
            UserId = Guid.Empty;
        }
        public WorkDayBaseModel(string name, string surname, Guid ID, DateTime start, DateTime end)
        {
            Name = name;
            Surname = surname;
            UserId = ID;
            Start = start;
            End = end;
        }
    }
}
