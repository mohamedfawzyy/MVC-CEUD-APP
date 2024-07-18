using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Data.Configyrations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired(true);
            builder.Property(E=>E.Age).IsRequired(false);
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");
            builder.Property(E=>E.Gender).IsRequired(true).
                HasConversion(
                (Gender)=>Gender.ToString(),
                (StringGender)=>(Gender) Enum.Parse(typeof(Gender),StringGender));
            builder.Property(E => E.EmpType).IsRequired(true)
                .HasConversion(
                (EmpType) => EmpType.ToString(),
                (StringEmpType) => (EmpType)Enum.Parse(typeof(EmpType), StringEmpType));
        }
    }
}
