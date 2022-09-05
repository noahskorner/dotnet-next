namespace Infrastructure.Services
{
    public interface IDateService
    {
        DateTimeOffset Now();
        DateTimeOffset UtcNow();
    }

    public class DateService : IDateService
    {
        public DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }

}
