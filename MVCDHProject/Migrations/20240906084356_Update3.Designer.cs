﻿// <auto-generated />
using MVCDHProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVCDHProject.Migrations
{
    [DbContext(typeof(MVCCoreDBContext))]
    [Migration("20240906084356_Update3")]
    partial class Update3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MVCDHProject.Models.Customer", b =>
                {
                    b.Property<int>("Custid")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("Money");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("Varchar");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("Varchar");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Custid");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Custid = 101,
                            Balance = 50000m,
                            City = "Delhi",
                            Name = "Sai",
                            Status = true
                        },
                        new
                        {
                            Custid = 102,
                            Balance = 40000m,
                            City = "Mumbai",
                            Name = "Sonia",
                            Status = true
                        },
                        new
                        {
                            Custid = 103,
                            Balance = 30000m,
                            City = "Chennai",
                            Name = "Pankaj",
                            Status = true
                        },
                        new
                        {
                            Custid = 104,
                            Balance = 25000m,
                            City = "Bengaluru",
                            Name = "Samuels",
                            Status = true
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
