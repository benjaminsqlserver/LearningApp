using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    [Table("StudentExercises", Schema = "dbo")]
    public partial class StudentExercise
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
        public long ExerciseID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ExerciseName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public long SubjectID { get; set; }

        public Subject Subject { get; set; }

        public ICollection<StudentExerciseResult> StudentExerciseResults { get; set; }
    }
}