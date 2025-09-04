using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(80);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(80);

            builder.HasOne(s => s.User)
                .WithMany() 
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict); // silerken user silinmesin

            builder.Property(x => x.BirthDate)
                .HasColumnType("date");
        }
    }
}
