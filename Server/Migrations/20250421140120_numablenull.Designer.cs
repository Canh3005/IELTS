﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyMvcBackend.Data;

#nullable disable

namespace MyMvcBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250421140120_numablenull")]
    partial class numablenull
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyMvcBackend.Models.ListeningAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnswerText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("ListeningAnswers");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningPart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ListOfQuestions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecordingId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RecordingId");

                    b.ToTable("ListeningParts");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PartId")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PartId");

                    b.ToTable("ListeningQuestions");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningRecording", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AudioUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Passage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("ListeningRecordings");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfQuestions")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ListeningTests");
                });

            modelBuilder.Entity("MyMvcBackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyMvcBackend.Models.UserAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("TestResultId")
                        .HasColumnType("int");

                    b.Property<string>("UAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TestResultId");

                    b.ToTable("UserAnswers");
                });

            modelBuilder.Entity("MyMvcBackend.Models.UserTestResults", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Accuracy")
                        .HasColumnType("real");

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.Property<DateTime>("TestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("TestType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeTaken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTestResults");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningAnswer", b =>
                {
                    b.HasOne("MyMvcBackend.Models.ListeningQuestion", "ListeningQuestion")
                        .WithMany("ListeningAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListeningQuestion");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningPart", b =>
                {
                    b.HasOne("MyMvcBackend.Models.ListeningRecording", "ListeningRecording")
                        .WithMany("ListeningParts")
                        .HasForeignKey("RecordingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListeningRecording");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningQuestion", b =>
                {
                    b.HasOne("MyMvcBackend.Models.ListeningPart", "ListeningPart")
                        .WithMany("ListeningQuestions")
                        .HasForeignKey("PartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListeningPart");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningRecording", b =>
                {
                    b.HasOne("MyMvcBackend.Models.ListeningTest", "ListeningTest")
                        .WithMany("ListeningRecordings")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListeningTest");
                });

            modelBuilder.Entity("MyMvcBackend.Models.UserAnswer", b =>
                {
                    b.HasOne("MyMvcBackend.Models.UserTestResults", "UserTestResult")
                        .WithMany("UserAnswers")
                        .HasForeignKey("TestResultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserTestResult");
                });

            modelBuilder.Entity("MyMvcBackend.Models.UserTestResults", b =>
                {
                    b.HasOne("MyMvcBackend.Models.ListeningTest", "ListeningTest")
                        .WithMany("UserTestResults")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyMvcBackend.Models.User", "User")
                        .WithMany("UserTestResults")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ListeningTest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningPart", b =>
                {
                    b.Navigation("ListeningQuestions");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningQuestion", b =>
                {
                    b.Navigation("ListeningAnswers");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningRecording", b =>
                {
                    b.Navigation("ListeningParts");
                });

            modelBuilder.Entity("MyMvcBackend.Models.ListeningTest", b =>
                {
                    b.Navigation("ListeningRecordings");

                    b.Navigation("UserTestResults");
                });

            modelBuilder.Entity("MyMvcBackend.Models.User", b =>
                {
                    b.Navigation("UserTestResults");
                });

            modelBuilder.Entity("MyMvcBackend.Models.UserTestResults", b =>
                {
                    b.Navigation("UserAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
