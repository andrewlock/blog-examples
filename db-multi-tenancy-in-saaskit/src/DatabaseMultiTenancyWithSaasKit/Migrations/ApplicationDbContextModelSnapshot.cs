using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DatabaseMultiTenancyWithSaasKit.Models;

namespace DatabaseMultiTenancyWithSaasKit.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("DatabaseMultiTenancyWithSaasKit.Models.AppTenant", b =>
                {
                    b.Property<int>("AppTenantId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hostname");

                    b.Property<string>("Name");

                    b.HasKey("AppTenantId");

                    b.ToTable("AppTenants");
                });
        }
    }
}
