namespace WorkSchedule.Api.Models
{
    public class PreferenceModel : WorkDayBaseModel
    {
        public PreferenceModel() : base() { }
        public PreferenceModel(string name, string surname, Guid id, DateTime start, DateTime end) : base(name, surname, id, start, end) { }
    }
}
