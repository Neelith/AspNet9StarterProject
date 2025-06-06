﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using YourProjectName.Infrastructure.Persistence;

#nullable disable

namespace YourProjectName.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("YourProjectName.Domain.WeatherForecast.WeatherForecastAggregate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Forecasts", (string)null);
                });

            modelBuilder.Entity("YourProjectName.Domain.WeatherForecast.WeatherForecastAggregate", b =>
                {
                    b.OwnsOne("YourProjectName.Domain.WeatherForecast.Summary", "Summary", b1 =>
                        {
                            b1.Property<int>("WeatherForecastAggregateId")
                                .HasColumnType("integer");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("character varying(256)")
                                .HasColumnName("Summary");

                            b1.HasKey("WeatherForecastAggregateId");

                            b1.ToTable("Forecasts", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("WeatherForecastAggregateId");
                        });

                    b.Navigation("Summary");
                });
#pragma warning restore 612, 618
        }
    }
}
