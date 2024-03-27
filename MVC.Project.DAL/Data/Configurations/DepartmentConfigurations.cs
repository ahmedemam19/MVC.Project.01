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
    internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            // fluent APIs for " Department " Domain

            builder.Property(d => d.Id).UseIdentityColumn(10, 10);
            builder.Property(d => d.Code).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(d => d.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();

            builder.HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
