namespace Data.Repositories
{
    public interface IEntity
    {
        long Id { get; set; }
    }

    public class Entity : IEntity
    {
        public long Id { get; set; }
    }
}
