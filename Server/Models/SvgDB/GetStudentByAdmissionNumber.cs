using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LearningApp.Server.Models.SvgDB
{
    public partial class GetStudentByAdmissionNumber
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        public long StudentID { get; set; }

        public string StudentAdmissionNumber { get; set; }

        public string StudentSurname { get; set; }

        public string StudentFirstName { get; set; }

        public string StudentOtherNames { get; set; }

        public long? StudentSexID { get; set; }

        public DateTime? StudentDateOfBirth { get; set; }

        public long? StudentClassID { get; set; }

        public long? StudentNationalityID { get; set; }

        public long? StudentStateID { get; set; }

        public long? StudentLocalGovernmentAreaID { get; set; }

        public string StudentTown { get; set; }

        public string StudentResidentialAddress { get; set; }

        public string StudentPhoto { get; set; }

        public DateTime? StudentRegistrationDate { get; set; }

        public DateTime? StudentGraduationDateOrDateStudentLeftTheSchool { get; set; }

        public string FatherSurname { get; set; }

        public string FatherFirstName { get; set; }

        public string FatherOtherNames { get; set; }

        public long? FatherNationalityID { get; set; }

        public string FatherTelephone1 { get; set; }

        public string FatherTelephone2 { get; set; }

        public string FatherEmailAddress { get; set; }

        public long? FatherProfessionID { get; set; }

        public string FatherResidentialAddress { get; set; }

        public string MotherSurname { get; set; }

        public string MotherFirstName { get; set; }

        public string MotherOtherNames { get; set; }

        public long? MotherNationalityID { get; set; }

        public string MotherTelephone1 { get; set; }

        public string MotherTelephone2 { get; set; }

        public string MotherEmailAddress { get; set; }

        public long? MotherProfessionID { get; set; }

        public string MotherResidentialAddress { get; set; }

        public string GuardianSurname { get; set; }

        public string GuardianFirstName { get; set; }

        public string GuardianOtherNames { get; set; }

        public long? GuardianNationalityID { get; set; }

        public string GuardianTelephone1 { get; set; }

        public string GuardianTelephone2 { get; set; }

        public string GuardianEmailAddress { get; set; }

        public long? GuardianProfessionID { get; set; }

        public string GuardianResidentialAddress { get; set; }
    }
}