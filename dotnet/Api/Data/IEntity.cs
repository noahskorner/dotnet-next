using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data
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
