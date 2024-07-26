using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.DAL.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Data.Configyrations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
           builder.HasKey(x => x.Id);
           builder.Property(x => x.Id).UseIdentityColumn(10, 10);
           builder.Property(x=>x.Code).HasColumnType("varchar").HasMaxLength(10).IsRequired();
           builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired(); 
            builder.HasMany(d => d.Employees)
                .WithOne(e=>e.Department)
                .HasForeignKey(e=>e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
