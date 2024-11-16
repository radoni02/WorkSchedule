namespace WorkSchedule.Api.Models
{
    public class WorkDayModel : WorkDayBaseModel
    {
        public WorkDayModel() : base() { }
        public WorkDayModel(string name, string surname, Guid Id, DateTime start, DateTime end) : base(name, surname, Id, start, end) { }
    }
}
