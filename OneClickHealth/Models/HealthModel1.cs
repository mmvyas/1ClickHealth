namespace OneClickHealth.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web.Mvc;
    using System.ComponentModel.DataAnnotations;

    public partial class HealthModel1 : DbContext
    {
        public ProgressDiary progressDiary { get; set; }
        public Exercise exercise { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public ExerciseProgress exerciseProgress { get; set; }

        [Required]
        public string EntryName { get; set; }


        [Required]        
        public int[] ExerciseId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter only positive whole numbers")]
        [RegularExpression (@"^[0-9]+$" , ErrorMessage = "Hours Spent can only be whole numbers")]
        public int HoursSpent1{ get; set; }
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Hours Spent can only be whole numbers")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter only positive whole numbers")]
        public int HoursSpent2 { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Hours Spent can only be whole numbers")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter only positive whole numbers")]
        public int HoursSpent3 { get; set; }

        public HealthModel1(): base("name=HealthModel1")
        {
        }

        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<ExerciseProgress> ExerciseProgresses { get; set; }
        public virtual DbSet<ProgressDiary> ProgressDiaries { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.ExerciseProgresses)
                .WithRequired(e => e.Exercise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProgressDiary>()
                .HasMany(e => e.ExerciseProgresses)
                .WithRequired(e => e.ProgressDiary)
                .WillCascadeOnDelete(false);
        }
    }
}
