﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UserApi.Context;

#nullable disable

namespace UserApi.Migrations
{
    [DbContext(typeof(ContextApp))]
    partial class ContextAppModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ModelsLibrary.MessageModels.Entity.MessageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ModelsLibrary.UserModels.Entity.RoleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ModelsLibrary.UserModels.Entity.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("bytea");

                    b.Property<Guid>("RoleTypeId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleTypeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ModelsLibrary.MessageModels.Entity.MessageEntity", b =>
                {
                    b.HasOne("ModelsLibrary.UserModels.Entity.UserEntity", "Recipient")
                        .WithMany("ReceiveMessages")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ModelsLibrary.UserModels.Entity.UserEntity", "Sender")
                        .WithMany("SendMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ModelsLibrary.UserModels.Entity.UserEntity", b =>
                {
                    b.HasOne("ModelsLibrary.UserModels.Entity.RoleEntity", "RoleType")
                        .WithMany("Users")
                        .HasForeignKey("RoleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleType");
                });

            modelBuilder.Entity("ModelsLibrary.UserModels.Entity.RoleEntity", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ModelsLibrary.UserModels.Entity.UserEntity", b =>
                {
                    b.Navigation("ReceiveMessages");

                    b.Navigation("SendMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
