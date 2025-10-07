using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LearningApp.Server.Models.SvgDB;

namespace LearningApp.Server.Data
{
    public partial class SvgDBContext : DbContext
    {
        public SvgDBContext()
        {
        }

        public SvgDBContext(DbContextOptions<SvgDBContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LearningApp.Server.Models.SvgDB.DeleteStudentExerciseResultWithAudit>().HasNoKey();

            builder.Entity<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>().HasNoKey();

            builder.Entity<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult>().HasNoKey();

            builder.Entity<LearningApp.Server.Models.SvgDB.InsertStudentExerciseResultWithAudit>().HasNoKey();

            builder.Entity<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord>().HasNoKey();

            builder.Entity<LearningApp.Server.Models.SvgDB.UpdateStudentExerciseResultWithAudit>().HasNoKey();

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentExerciseResult>()
              .HasOne(i => i.StudentExercise)
              .WithMany(i => i.StudentExerciseResults)
              .HasForeignKey(i => i.ExerciseID)
              .HasPrincipalKey(i => i.ExerciseID);

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentExercise>()
              .HasOne(i => i.Subject)
              .WithMany(i => i.StudentExercises)
              .HasForeignKey(i => i.SubjectID)
              .HasPrincipalKey(i => i.SubjectID);

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>()
              .Property(p => p.ChangedOn)
              .HasDefaultValueSql(@"(getdate())");

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentsAudit>()
              .Property(p => p.ChangedOn)
              .HasDefaultValueSql(@"(getdate())");

            builder.Entity<LearningApp.Server.Models.SvgDB.SubjectsAudit>()
              .Property(p => p.ChangedOn)
              .HasDefaultValueSql(@"(getdate())");

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>()
              .Property(p => p.ChangedOn)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.Student>()
              .Property(p => p.StudentDateOfBirth)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.Student>()
              .Property(p => p.StudentRegistrationDate)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.Student>()
              .Property(p => p.StudentGraduationDateOrDateStudentLeftTheSchool)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentsAudit>()
              .Property(p => p.StudentDateOfBirth)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentsAudit>()
              .Property(p => p.StudentRegistrationDate)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentsAudit>()
              .Property(p => p.StudentGraduationDateOrDateStudentLeftTheSchool)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.StudentsAudit>()
              .Property(p => p.ChangedOn)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.SubjectsAudit>()
              .Property(p => p.ChangedOn)
              .HasColumnType("datetime2");

            builder.Entity<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>()
              .Property(p => p.StudentDateOfBirth)
              .HasColumnType("datetime2(7)");

            builder.Entity<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>()
              .Property(p => p.StudentRegistrationDate)
              .HasColumnType("datetime2(7)");

            builder.Entity<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>()
              .Property(p => p.StudentGraduationDateOrDateStudentLeftTheSchool)
              .HasColumnType("datetime2(7)");

            builder.Entity<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord>()
              .Property(p => p.ChangedOn)
              .HasColumnType("datetime2(7)");
            this.OnModelBuilding(builder);
        }

        public DbSet<LearningApp.Server.Models.SvgDB.StudentExerciseResult> StudentExerciseResults { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> StudentExerciseResultsAudits { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.StudentExercise> StudentExercises { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.Student> Students { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.StudentsAudit> StudentsAudits { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.Subject> Subjects { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.SubjectsAudit> SubjectsAudits { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.DeleteStudentExerciseResultWithAudit> DeleteStudentExerciseResultWithAudits { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber> GetStudentByAdmissionNumbers { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult> GetStudentExerciseResults { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.InsertStudentExerciseResultWithAudit> InsertStudentExerciseResultWithAudits { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord> LastStudentExerciseResultAuditRecords { get; set; }

        public DbSet<LearningApp.Server.Models.SvgDB.UpdateStudentExerciseResultWithAudit> UpdateStudentExerciseResultWithAudits { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}