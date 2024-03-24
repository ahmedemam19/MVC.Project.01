using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.DAL.Data.Configurations
{
    internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.Salary).HasColumnType("decimal(12, 2)");


            builder.Property(e => e.Gender).HasConversion(
                                            (Gender) => Gender.ToString(),
                                            (GenderAsString) => (Gender) Enum.Parse(typeof(Gender), GenderAsString, true));

            builder.Property(e => e.EmpType).HasConversion(
                                            (EmpType) => EmpType.ToString(),
                                            (EmpTypeAsString) => (EmpType)Enum.Parse(typeof(EmpType), EmpTypeAsString, true));

        }
    }
}
