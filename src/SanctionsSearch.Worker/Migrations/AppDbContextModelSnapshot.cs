﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SanctionsSearch.Worker.Persistence;

#nullable disable

namespace SanctionsSearch.Worker.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CityProvincePostal")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SdnId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SdnId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Alias", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SdnId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SdnId");

                    b.ToTable("Aliases");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SdnId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SdnId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Sdn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CallSign")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("GrossRegisteredTonnage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Program")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tonnage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("VesselFlag")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VesselOwner")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VesselType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sdns");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Address", b =>
                {
                    b.HasOne("SanctionsSearch.Worker.Models.Sdn", "Sdn")
                        .WithMany("Addresses")
                        .HasForeignKey("SdnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sdn");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Alias", b =>
                {
                    b.HasOne("SanctionsSearch.Worker.Models.Sdn", "Sdn")
                        .WithMany("Aliases")
                        .HasForeignKey("SdnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sdn");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Comment", b =>
                {
                    b.HasOne("SanctionsSearch.Worker.Models.Sdn", "Sdn")
                        .WithMany("Comments")
                        .HasForeignKey("SdnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sdn");
                });

            modelBuilder.Entity("SanctionsSearch.Worker.Models.Sdn", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Aliases");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
