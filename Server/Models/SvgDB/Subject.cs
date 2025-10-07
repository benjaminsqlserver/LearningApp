using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    [Table("Subjects", Schema = "dbo")]
    public partial class Subject
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
        public long SubjectID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string SubjectName { get; set; }

        public ICollection<StudentExercise> StudentExercises { get; set; }
    }
}