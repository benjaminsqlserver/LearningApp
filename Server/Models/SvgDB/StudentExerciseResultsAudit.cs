using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    [Table("StudentExerciseResultsAudit", Schema = "dbo")]
    public partial class StudentExerciseResultsAudit
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
        public long AuditID { get; set; }

        [ConcurrencyCheck]
        public long? ResultID { get; set; }

        [ConcurrencyCheck]
        public long? StudentID { get; set; }

        [ConcurrencyCheck]
        public long? ExerciseID { get; set; }

        [ConcurrencyCheck]
        public DateTime? DateTaken { get; set; }

        [ConcurrencyCheck]
        public TimeOnly? TimeTaken { get; set; }

        [ConcurrencyCheck]
        public int? MarkObtainable { get; set; }

        [ConcurrencyCheck]
        public int? MarkObtained { get; set; }

        [ConcurrencyCheck]
        public string UserID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ActionType { get; set; }

        [Column(TypeName="datetime2")]
        [ConcurrencyCheck]
        public DateTime ChangedOn { get; set; }

        [ConcurrencyCheck]
        public string ChangedBy { get; set; }
    }
}