﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using src.Models.Wz;

#nullable disable

namespace src.Migrations
{
    [DbContext(typeof(WzDbContext))]
    [Migration("20241128112359_addingWzV021")]
    partial class addingWzV021
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("src.Models.Wz.DtoWzEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Aktenzeichen")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RowVersion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Weiserzeichen")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WzEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
