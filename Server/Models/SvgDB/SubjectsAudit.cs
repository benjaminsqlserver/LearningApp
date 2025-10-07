using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    [Table("SubjectsAudit", Schema = "dbo")]
    public partial class SubjectsAudit
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
        public long? SubjectID { get; set; }

        [ConcurrencyCheck]
        public string SubjectName { get; set; }

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