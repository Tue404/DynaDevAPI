﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VIDUUUUU.Data;

#nullable disable

namespace VIDUUUUU.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20250119213259_sdf")]
    partial class sdf
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("VIDUUUUU.Models.NguoiDung", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NguoiDungs");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Email = "admin@example.com",
                            HoTen = "Admin User",
                            Password = "admin123",
                            UserName = "admin"
                        },
                        new
                        {
                            ID = 2,
                            Email = "user1@example.com",
                            HoTen = "User One",
                            Password = "password1",
                            UserName = "user1"
                        },
                        new
                        {
                            ID = 3,
                            Email = "user2@example.com",
                            HoTen = "User Two",
                            Password = "password2",
                            UserName = "user2"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
