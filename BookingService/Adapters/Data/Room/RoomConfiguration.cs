using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data
{
    public class RoomConfiguration : IEntityTypeConfiguration<Entities.Room>
    {
        public void Configure(EntityTypeBuilder<Entities.Room> builder)
        {
            builder.HasKey( room => room.Id );
            builder.OwnsOne( room => room.Price)
                .Property( room => room.Currency);

            builder.OwnsOne( room => room.Price)
                .Property( room => room.Value);    
        }        
    }
}