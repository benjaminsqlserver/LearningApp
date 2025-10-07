using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    [Table("Students", Schema = "dbo")]
    public partial class Student
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
        public long StudentID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StudentAdmissionNumber { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StudentSurname { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StudentFirstName { get; set; }

        [ConcurrencyCheck]
        public string StudentOtherNames { get; set; }

        [ConcurrencyCheck]
        public long? StudentSexID { get; set; }

        [Column(TypeName="datetime2")]
        [ConcurrencyCheck]
        public DateTime? StudentDateOfBirth { get; set; }

        [ConcurrencyCheck]
        public long? StudentClassID { get; set; }

        [ConcurrencyCheck]
        public long? StudentNationalityID { get; set; }

        [ConcurrencyCheck]
        public long? StudentStateID { get; set; }

        [ConcurrencyCheck]
        public long? StudentLocalGovernmentAreaID { get; set; }

        [ConcurrencyCheck]
        public string StudentTown { get; set; }

        [ConcurrencyCheck]
        public string StudentResidentialAddress { get; set; }

        [ConcurrencyCheck]
        public string StudentPhoto { get; set; }

        [Column(TypeName="datetime2")]
        [ConcurrencyCheck]
        public DateTime? StudentRegistrationDate { get; set; }

        [Column(TypeName="datetime2")]
        [ConcurrencyCheck]
        public DateTime? StudentGraduationDateOrDateStudentLeftTheSchool { get; set; }

        [ConcurrencyCheck]
        public string FatherSurname { get; set; }

        [ConcurrencyCheck]
        public string FatherFirstName { get; set; }

        [ConcurrencyCheck]
        public string FatherOtherNames { get; set; }

        [ConcurrencyCheck]
        public long? FatherNationalityID { get; set; }

        [ConcurrencyCheck]
        public string FatherTelephone1 { get; set; }

        [ConcurrencyCheck]
        public string FatherTelephone2 { get; set; }

        [ConcurrencyCheck]
        public string FatherEmailAddress { get; set; }

        [ConcurrencyCheck]
        public long? FatherProfessionID { get; set; }

        [ConcurrencyCheck]
        public string FatherResidentialAddress { get; set; }

        [ConcurrencyCheck]
        public string MotherSurname { get; set; }

        [ConcurrencyCheck]
        public string MotherFirstName { get; set; }

        [ConcurrencyCheck]
        public string MotherOtherNames { get; set; }

        [ConcurrencyCheck]
        public long? MotherNationalityID { get; set; }

        [ConcurrencyCheck]
        public string MotherTelephone1 { get; set; }

        [ConcurrencyCheck]
        public string MotherTelephone2 { get; set; }

        [ConcurrencyCheck]
        public string MotherEmailAddress { get; set; }

        [ConcurrencyCheck]
        public long? MotherProfessionID { get; set; }

        [ConcurrencyCheck]
        public string MotherResidentialAddress { get; set; }

        [ConcurrencyCheck]
        public string GuardianSurname { get; set; }

        [ConcurrencyCheck]
        public string GuardianFirstName { get; set; }

        [ConcurrencyCheck]
        public string GuardianOtherNames { get; set; }

        [ConcurrencyCheck]
        public long? GuardianNationalityID { get; set; }

        [ConcurrencyCheck]
        public string GuardianTelephone1 { get; set; }

        [ConcurrencyCheck]
        public string GuardianTelephone2 { get; set; }

        [ConcurrencyCheck]
        public string GuardianEmailAddress { get; set; }

        [ConcurrencyCheck]
        public long? GuardianProfessionID { get; set; }

        [ConcurrencyCheck]
        public string GuardianResidentialAddress { get; set; }
    }
}