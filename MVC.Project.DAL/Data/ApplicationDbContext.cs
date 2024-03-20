﻿using Microsoft.EntityFrameworkCore;
using MVC.Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Project.DAL.Data
{
    internal class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        


        public DbSet<Department> departments { get; set; }

    }
}
