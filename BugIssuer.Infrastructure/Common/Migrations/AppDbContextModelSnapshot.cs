﻿// <auto-generated />
using System;

using BugIssuer.Domain;
using BugIssuer.Domain.Enums;
using BugIssuer.Infrastructure.Common.Persistance;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CleanArchitecture.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("BugIssuer.Domain.Issue", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("TEXT");

                b.Property<int>("IssueId")
                    .HasColumnType("INTEGER");

                b.Property<string>("Description")
                    .HasColumnType("TEXT");

                b.Property<string>("Category")
                    .HasColumnType("TEXT");

                b.Property<string>("AuthorId")
                    .HasColumnType("TEXT");

                b.Property<string>("Author")
                    .HasColumnType("TEXT");

                b.Property<int>("Urgency")
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("DateTime")
                    .HasColumnType("TEXT");

                b.Property<DateTime>("LastUpdate")
                    .HasColumnType("TEXT");

                b.Property<string>("Assignee")
                    .HasColumnType("TEXT");

                b.Property<string>("Status")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("IssueId");

                b.ToTable("Issue");
            });

            modelBuilder.Entity("BugIssuer.Domain.User", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("TEXT");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("UserName")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("Department")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("UserId");

                b.ToTable("Users");
            });

            modelBuilder.Entity("BugIssuer.Domain.Issue", b =>
            {
                b.OwnsMany("BugIssuer.Domain.Comment", "Comment", b1 =>
                {
                    b1.Property<int>("IssueId")
                        .HasColumnType("INTEGER");

                    b1.Property<int>("CommentId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("CommentId");

                    b1.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b1.Property<string>("AuthorId")
                        .HasColumnType("TEXT");

                    b1.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b1.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b1.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b1.HasKey("IssueId");

                    b1.ToTable("Issue");

                    b1.WithOwner()
                        .HasForeignKey("IssueId");
                });

                b.Navigation("Comment")
                    .IsRequired();
            });
#pragma warning restore 612, 618
        }
    }
}