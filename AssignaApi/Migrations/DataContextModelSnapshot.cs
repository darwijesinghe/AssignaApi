﻿// <auto-generated />
using System;
using AssignaApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssignaApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AssignaApi.Models.Category", b =>
                {
                    b.Property<int>("cat_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("cat_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("insertdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("cat_id");

                    b.ToTable("category");
                });

            modelBuilder.Entity("AssignaApi.Models.Priority", b =>
                {
                    b.Property<int>("pri_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("insertdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("pri_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("pri_id");

                    b.ToTable("priority");
                });

            modelBuilder.Entity("AssignaApi.Models.Task", b =>
                {
                    b.Property<int>("tsk_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("cat_id")
                        .HasColumnType("int");

                    b.Property<bool>("complete")
                        .HasColumnType("bit");

                    b.Property<DateTime>("deadline")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("insertdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("pending")
                        .HasColumnType("bit");

                    b.Property<bool>("pri_high")
                        .HasColumnType("bit");

                    b.Property<bool>("pri_low")
                        .HasColumnType("bit");

                    b.Property<bool>("pri_medium")
                        .HasColumnType("bit");

                    b.Property<string>("tsk_note")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("tsk_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<string>("user_note")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.HasKey("tsk_id");

                    b.HasIndex("cat_id");

                    b.HasIndex("user_id");

                    b.ToTable("task");
                });

            modelBuilder.Entity("AssignaApi.Models.Users", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("email_verified")
                        .HasColumnType("bit");

                    b.Property<DateTime>("expires_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("family_name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("first_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("given_name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("insertdate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<bool>("is_admin")
                        .HasColumnType("bit");

                    b.Property<string>("locale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("password_hash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("password_salt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("refresh_expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("reset_expires")
                        .HasColumnType("datetime2");

                    b.Property<string>("reset_token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("verify_token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("AssignaApi.Models.Task", b =>
                {
                    b.HasOne("AssignaApi.Models.Category", "category")
                        .WithMany("task")
                        .HasForeignKey("cat_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AssignaApi.Models.Users", "users")
                        .WithMany("task")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
