using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    [Table("StudentExerciseResults", Schema = "dbo")]
    public partial class StudentExerciseResult
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ResultID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long StudentID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long ExerciseID { get; set; }

        public StudentExercise StudentExercise { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime DateTaken { get; set; }

        [Required]
        [ConcurrencyCheck]
        public TimeOnly TimeTaken { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int MarkObtainable { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int MarkObtained { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string UserID { get; set; }
    }
}