namespace Services.Services
{
    public interface IDateService
    {
        DateTimeOffset UtcNow();
    }

    public class DateService : IDateService
    {
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }

}
