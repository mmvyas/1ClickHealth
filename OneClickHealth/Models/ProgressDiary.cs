namespace OneClickHealth.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProgressDiary")]
    public partial class ProgressDiary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProgressDiary()
        {
            ExerciseProgresses = new HashSet<ExerciseProgress>();
        }

        [Key]
        public int ProgressId { get; set; }

        [Required]
        public string EntryName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExerciseProgress> ExerciseProgresses { get; set; }
    }
}
