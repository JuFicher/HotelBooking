using Entities = Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data
{
    public class GuestConfiguration : IEntityTypeConfiguration<Entities.Guest>
    {
        public void Configure(EntityTypeBuilder<Entities.Guest> builder)
        {
            builder.HasKey( guest => guest.Id );
            builder.OwnsOne( guest => guest.DocumentId)
                .Property( guest => guest.IdNumber);

            builder.OwnsOne( guest => guest.DocumentId)
                .Property( guest => guest.DocumentType);    
        }        
    }
}