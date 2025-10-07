using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    public partial class LastStudentExerciseResultAuditRecord
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        public long AuditID { get; set; }

        public long? ResultID { get; set; }

        public long? StudentID { get; set; }

        public long? ExerciseID { get; set; }

        public DateTime? DateTaken { get; set; }

        public TimeSpan? TimeTaken { get; set; }

        public int? MarkObtainable { get; set; }

        public int? MarkObtained { get; set; }

        public string UserID { get; set; }

        public string ActionType { get; set; }

        public DateTime ChangedOn { get; set; }

        public string ChangedBy { get; set; }
    }
}