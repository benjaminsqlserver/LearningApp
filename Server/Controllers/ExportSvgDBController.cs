using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using LearningApp.Server.Data;

namespace LearningApp.Server.Controllers
{
    public partial class ExportSvgDBController : ExportController
    {
        private readonly SvgDBContext context;
        private readonly SvgDBService service;

        public ExportSvgDBController(SvgDBContext context, SvgDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/SvgDB/studentexerciseresults/csv")]
        [HttpGet("/export/SvgDB/studentexerciseresults/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentExerciseResultsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudentExerciseResults(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentexerciseresults/excel")]
        [HttpGet("/export/SvgDB/studentexerciseresults/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentExerciseResultsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudentExerciseResults(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentexerciseresultsaudits/csv")]
        [HttpGet("/export/SvgDB/studentexerciseresultsaudits/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentExerciseResultsAuditsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudentExerciseResultsAudits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentexerciseresultsaudits/excel")]
        [HttpGet("/export/SvgDB/studentexerciseresultsaudits/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentExerciseResultsAuditsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudentExerciseResultsAudits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentexercises/csv")]
        [HttpGet("/export/SvgDB/studentexercises/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentExercisesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudentExercises(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentexercises/excel")]
        [HttpGet("/export/SvgDB/studentexercises/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentExercisesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudentExercises(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/students/csv")]
        [HttpGet("/export/SvgDB/students/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudents(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/students/excel")]
        [HttpGet("/export/SvgDB/students/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudents(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentsaudits/csv")]
        [HttpGet("/export/SvgDB/studentsaudits/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentsAuditsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStudentsAudits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/studentsaudits/excel")]
        [HttpGet("/export/SvgDB/studentsaudits/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStudentsAuditsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStudentsAudits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/subjects/csv")]
        [HttpGet("/export/SvgDB/subjects/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSubjects(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/subjects/excel")]
        [HttpGet("/export/SvgDB/subjects/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSubjects(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/subjectsaudits/csv")]
        [HttpGet("/export/SvgDB/subjectsaudits/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectsAuditsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSubjectsAudits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/subjectsaudits/excel")]
        [HttpGet("/export/SvgDB/subjectsaudits/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSubjectsAuditsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSubjectsAudits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/getstudentbyadmissionnumbers/csv(StudentAdmissionNumber='{StudentAdmissionNumber}', fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGetStudentByAdmissionNumbersToCSV(string StudentAdmissionNumber, string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGetStudentByAdmissionNumbers(StudentAdmissionNumber), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/getstudentbyadmissionnumbers/excel(StudentAdmissionNumber='{StudentAdmissionNumber}', fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGetStudentByAdmissionNumbersToExcel(string StudentAdmissionNumber, string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGetStudentByAdmissionNumbers(StudentAdmissionNumber), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/getstudentexerciseresults/csv(StudentID={StudentID}, ExerciseID={ExerciseID}, fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGetStudentExerciseResultsToCSV(long? StudentID, long? ExerciseID, string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGetStudentExerciseResults(StudentID, ExerciseID), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/getstudentexerciseresults/excel(StudentID={StudentID}, ExerciseID={ExerciseID}, fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGetStudentExerciseResultsToExcel(long? StudentID, long? ExerciseID, string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGetStudentExerciseResults(StudentID, ExerciseID), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/laststudentexerciseresultauditrecords/csv(ResultID={ResultID}, fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLastStudentExerciseResultAuditRecordsToCSV(long? ResultID, string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLastStudentExerciseResultAuditRecords(ResultID), Request.Query, false), fileName);
        }

        [HttpGet("/export/SvgDB/laststudentexerciseresultauditrecords/excel(ResultID={ResultID}, fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLastStudentExerciseResultAuditRecordsToExcel(long? ResultID, string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLastStudentExerciseResultAuditRecords(ResultID), Request.Query, false), fileName);
        }
    }
}
