namespace WorkSchedule.Api.Models
{
    public class WorkDayModel : WorkDayBaseModel
    {
        public WorkDayModel() : base() { }
        public WorkDayModel(string name, string surname, string email, DateTime start, DateTime end) : base(name, surname, email, start, end) { }
    }
}
