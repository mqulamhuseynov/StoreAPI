using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(256);

            builder.Property(p => p.Price).
                HasPrecision(18, 2);

            builder.Property(p => p.Description)
                .HasMaxLength(1000)
                .IsRequired();
        }
    }
}
