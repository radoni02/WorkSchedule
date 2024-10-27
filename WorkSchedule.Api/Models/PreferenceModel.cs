namespace WorkSchedule.Api.Models
{
    public class PreferenceModel : WorkDayBaseModel
    {
        public PreferenceModel() : base() { }
        public PreferenceModel(string name, string surname, string email, DateTime start, DateTime end) : base(name, surname, email, start, end) { }
    }
}
