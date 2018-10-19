namespace OneClickHealth.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExerciseProgress")]
    public partial class ExerciseProgress
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProgressId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExerciseId { get; set; }

        [Required]
        public string UserId { get; set; }

        public int HoursSpent { get; set; }

        [Column(TypeName = "date")]
        public DateTime EntryDate { get; set; }

        public virtual Exercise Exercise { get; set; }

        public virtual ProgressDiary ProgressDiary { get; set; }
    }
}
