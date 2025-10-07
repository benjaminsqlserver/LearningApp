
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace LearningApp.Client
{
    public partial class SvgDBService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public SvgDBService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/SvgDB/");
        }


        public async System.Threading.Tasks.Task ExportStudentExerciseResultsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresults/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresults/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentExerciseResultsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresults/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresults/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudentExerciseResults(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExerciseResult>> GetStudentExerciseResults(Query query)
        {
            return await GetStudentExerciseResults(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExerciseResult>> GetStudentExerciseResults(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResults");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentExerciseResults(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExerciseResult>>(response);
        }

        partial void OnCreateStudentExerciseResult(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> CreateStudentExerciseResult(LearningApp.Server.Models.SvgDB.StudentExerciseResult studentExerciseResult = default(LearningApp.Server.Models.SvgDB.StudentExerciseResult))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResults");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentExerciseResult), Encoding.UTF8, "application/json");

            OnCreateStudentExerciseResult(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentExerciseResult>(response);
        }

        partial void OnDeleteStudentExerciseResult(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudentExerciseResult(long resultId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResults({resultId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStudentExerciseResult(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStudentExerciseResultByResultId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> GetStudentExerciseResultByResultId(string expand = default(string), long resultId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResults({resultId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentExerciseResultByResultId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentExerciseResult>(response);
        }

        partial void OnUpdateStudentExerciseResult(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStudentExerciseResult(long resultId = default(long), LearningApp.Server.Models.SvgDB.StudentExerciseResult studentExerciseResult = default(LearningApp.Server.Models.SvgDB.StudentExerciseResult))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResults({resultId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", studentExerciseResult.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentExerciseResult), Encoding.UTF8, "application/json");

            OnUpdateStudentExerciseResult(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStudentExerciseResultsAuditsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresultsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresultsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentExerciseResultsAuditsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresultsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresultsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudentExerciseResultsAudits(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>> GetStudentExerciseResultsAudits(Query query)
        {
            return await GetStudentExerciseResultsAudits(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>> GetStudentExerciseResultsAudits(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResultsAudits");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentExerciseResultsAudits(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>>(response);
        }

        partial void OnCreateStudentExerciseResultsAudit(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> CreateStudentExerciseResultsAudit(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit studentExerciseResultsAudit = default(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResultsAudits");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentExerciseResultsAudit), Encoding.UTF8, "application/json");

            OnCreateStudentExerciseResultsAudit(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>(response);
        }

        partial void OnDeleteStudentExerciseResultsAudit(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudentExerciseResultsAudit(long auditId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResultsAudits({auditId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStudentExerciseResultsAudit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStudentExerciseResultsAuditByAuditId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> GetStudentExerciseResultsAuditByAuditId(string expand = default(string), long auditId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResultsAudits({auditId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentExerciseResultsAuditByAuditId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>(response);
        }

        partial void OnUpdateStudentExerciseResultsAudit(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStudentExerciseResultsAudit(long auditId = default(long), LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit studentExerciseResultsAudit = default(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit))
        {
            var uri = new Uri(baseUri, $"StudentExerciseResultsAudits({auditId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", studentExerciseResultsAudit.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentExerciseResultsAudit), Encoding.UTF8, "application/json");

            OnUpdateStudentExerciseResultsAudit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStudentExercisesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexercises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexercises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentExercisesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexercises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexercises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudentExercises(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExercise>> GetStudentExercises(Query query)
        {
            return await GetStudentExercises(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExercise>> GetStudentExercises(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StudentExercises");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentExercises(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentExercise>>(response);
        }

        partial void OnCreateStudentExercise(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> CreateStudentExercise(LearningApp.Server.Models.SvgDB.StudentExercise studentExercise = default(LearningApp.Server.Models.SvgDB.StudentExercise))
        {
            var uri = new Uri(baseUri, $"StudentExercises");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentExercise), Encoding.UTF8, "application/json");

            OnCreateStudentExercise(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentExercise>(response);
        }

        partial void OnDeleteStudentExercise(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudentExercise(long exerciseId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentExercises({exerciseId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStudentExercise(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStudentExerciseByExerciseId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> GetStudentExerciseByExerciseId(string expand = default(string), long exerciseId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentExercises({exerciseId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentExerciseByExerciseId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentExercise>(response);
        }

        partial void OnUpdateStudentExercise(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStudentExercise(long exerciseId = default(long), LearningApp.Server.Models.SvgDB.StudentExercise studentExercise = default(LearningApp.Server.Models.SvgDB.StudentExercise))
        {
            var uri = new Uri(baseUri, $"StudentExercises({exerciseId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", studentExercise.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentExercise), Encoding.UTF8, "application/json");

            OnUpdateStudentExercise(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudents(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.Student>> GetStudents(Query query)
        {
            return await GetStudents(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.Student>> GetStudents(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Students");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudents(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.Student>>(response);
        }

        partial void OnCreateStudent(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.Student> CreateStudent(LearningApp.Server.Models.SvgDB.Student student = default(LearningApp.Server.Models.SvgDB.Student))
        {
            var uri = new Uri(baseUri, $"Students");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

            OnCreateStudent(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.Student>(response);
        }

        partial void OnDeleteStudent(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudent(long studentId = default(long))
        {
            var uri = new Uri(baseUri, $"Students({studentId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStudent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStudentByStudentId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.Student> GetStudentByStudentId(string expand = default(string), long studentId = default(long))
        {
            var uri = new Uri(baseUri, $"Students({studentId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentByStudentId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.Student>(response);
        }

        partial void OnUpdateStudent(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStudent(long studentId = default(long), LearningApp.Server.Models.SvgDB.Student student = default(LearningApp.Server.Models.SvgDB.Student))
        {
            var uri = new Uri(baseUri, $"Students({studentId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", student.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(student), Encoding.UTF8, "application/json");

            OnUpdateStudent(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStudentsAuditsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStudentsAuditsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStudentsAudits(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentsAudit>> GetStudentsAudits(Query query)
        {
            return await GetStudentsAudits(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentsAudit>> GetStudentsAudits(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"StudentsAudits");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentsAudits(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.StudentsAudit>>(response);
        }

        partial void OnCreateStudentsAudit(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> CreateStudentsAudit(LearningApp.Server.Models.SvgDB.StudentsAudit studentsAudit = default(LearningApp.Server.Models.SvgDB.StudentsAudit))
        {
            var uri = new Uri(baseUri, $"StudentsAudits");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentsAudit), Encoding.UTF8, "application/json");

            OnCreateStudentsAudit(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentsAudit>(response);
        }

        partial void OnDeleteStudentsAudit(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudentsAudit(long auditId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentsAudits({auditId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStudentsAudit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStudentsAuditByAuditId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> GetStudentsAuditByAuditId(string expand = default(string), long auditId = default(long))
        {
            var uri = new Uri(baseUri, $"StudentsAudits({auditId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStudentsAuditByAuditId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.StudentsAudit>(response);
        }

        partial void OnUpdateStudentsAudit(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStudentsAudit(long auditId = default(long), LearningApp.Server.Models.SvgDB.StudentsAudit studentsAudit = default(LearningApp.Server.Models.SvgDB.StudentsAudit))
        {
            var uri = new Uri(baseUri, $"StudentsAudits({auditId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", studentsAudit.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(studentsAudit), Encoding.UTF8, "application/json");

            OnUpdateStudentsAudit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSubjectsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSubjectsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSubjects(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.Subject>> GetSubjects(Query query)
        {
            return await GetSubjects(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.Subject>> GetSubjects(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Subjects");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjects(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.Subject>>(response);
        }

        partial void OnCreateSubject(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.Subject> CreateSubject(LearningApp.Server.Models.SvgDB.Subject subject = default(LearningApp.Server.Models.SvgDB.Subject))
        {
            var uri = new Uri(baseUri, $"Subjects");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subject), Encoding.UTF8, "application/json");

            OnCreateSubject(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.Subject>(response);
        }

        partial void OnDeleteSubject(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSubject(long subjectId = default(long))
        {
            var uri = new Uri(baseUri, $"Subjects({subjectId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSubject(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSubjectBySubjectId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.Subject> GetSubjectBySubjectId(string expand = default(string), long subjectId = default(long))
        {
            var uri = new Uri(baseUri, $"Subjects({subjectId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjectBySubjectId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.Subject>(response);
        }

        partial void OnUpdateSubject(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSubject(long subjectId = default(long), LearningApp.Server.Models.SvgDB.Subject subject = default(LearningApp.Server.Models.SvgDB.Subject))
        {
            var uri = new Uri(baseUri, $"Subjects({subjectId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", subject.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subject), Encoding.UTF8, "application/json");

            OnUpdateSubject(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSubjectsAuditsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjectsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjectsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSubjectsAuditsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjectsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjectsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSubjectsAudits(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.SubjectsAudit>> GetSubjectsAudits(Query query)
        {
            return await GetSubjectsAudits(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.SubjectsAudit>> GetSubjectsAudits(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"SubjectsAudits");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjectsAudits(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.SubjectsAudit>>(response);
        }

        partial void OnCreateSubjectsAudit(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> CreateSubjectsAudit(LearningApp.Server.Models.SvgDB.SubjectsAudit subjectsAudit = default(LearningApp.Server.Models.SvgDB.SubjectsAudit))
        {
            var uri = new Uri(baseUri, $"SubjectsAudits");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subjectsAudit), Encoding.UTF8, "application/json");

            OnCreateSubjectsAudit(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.SubjectsAudit>(response);
        }

        partial void OnDeleteSubjectsAudit(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSubjectsAudit(long auditId = default(long))
        {
            var uri = new Uri(baseUri, $"SubjectsAudits({auditId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSubjectsAudit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSubjectsAuditByAuditId(HttpRequestMessage requestMessage);

        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> GetSubjectsAuditByAuditId(string expand = default(string), long auditId = default(long))
        {
            var uri = new Uri(baseUri, $"SubjectsAudits({auditId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSubjectsAuditByAuditId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<LearningApp.Server.Models.SvgDB.SubjectsAudit>(response);
        }

        partial void OnUpdateSubjectsAudit(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSubjectsAudit(long auditId = default(long), LearningApp.Server.Models.SvgDB.SubjectsAudit subjectsAudit = default(LearningApp.Server.Models.SvgDB.SubjectsAudit))
        {
            var uri = new Uri(baseUri, $"SubjectsAudits({auditId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", subjectsAudit.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(subjectsAudit), Encoding.UTF8, "application/json");

            OnUpdateSubjectsAudit(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportGetStudentByAdmissionNumbersToExcel(string StudentAdmissionNumber, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentbyadmissionnumbers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentbyadmissionnumbers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportGetStudentByAdmissionNumbersToCSV(string StudentAdmissionNumber, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentbyadmissionnumbers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentbyadmissionnumbers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetGetStudentByAdmissionNumbers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>> GetGetStudentByAdmissionNumbers(string studentAdmissionNumber = default(string), string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"GetStudentByAdmissionNumbersFunc(StudentAdmissionNumber='{Uri.EscapeDataString(studentAdmissionNumber.Trim().Replace("'", "''"))}')");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGetStudentByAdmissionNumbers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>>(response);
        }

        public async System.Threading.Tasks.Task ExportGetStudentExerciseResultsToExcel(long? StudentID, long? ExerciseID, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentexerciseresults/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentexerciseresults/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportGetStudentExerciseResultsToCSV(long? StudentID, long? ExerciseID, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentexerciseresults/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentexerciseresults/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetGetStudentExerciseResults(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult>> GetGetStudentExerciseResults(long? studentId = default(long?), long? exerciseId = default(long?), string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"GetStudentExerciseResultsFunc(StudentID={(studentId != null ? studentId : "null")},ExerciseID={(exerciseId != null ? exerciseId : "null")})");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGetStudentExerciseResults(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult>>(response);
        }

        public async System.Threading.Tasks.Task ExportLastStudentExerciseResultAuditRecordsToExcel(long? ResultID, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/laststudentexerciseresultauditrecords/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/laststudentexerciseresultauditrecords/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportLastStudentExerciseResultAuditRecordsToCSV(long? ResultID, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/laststudentexerciseresultauditrecords/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/laststudentexerciseresultauditrecords/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetLastStudentExerciseResultAuditRecords(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord>> GetLastStudentExerciseResultAuditRecords(long? resultId = default(long?), string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"LastStudentExerciseResultAuditRecordsFunc(ResultID={(resultId != null ? resultId : "null")})");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetLastStudentExerciseResultAuditRecords(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord>>(response);
        }

        partial void OnDeleteStudentExerciseResultWithAudits(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStudentExerciseResultWithAudits(long? resultId = default(long?), string changedBy = default(string))
        {
            var uri = new Uri(baseUri, $"DeleteStudentExerciseResultWithAuditsFunc(ResultID={(resultId != null ? resultId : "null")},ChangedBy='{Uri.EscapeDataString(changedBy.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnDeleteStudentExerciseResultWithAudits(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnInsertStudentExerciseResultWithAudits(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> InsertStudentExerciseResultWithAudits(long? studentId = default(long?), long? exerciseId = default(long?), string dateTaken = default(string), TimeSpan? timeTaken = default(TimeSpan?), int? markObtainable = default(int?), int? markObtained = default(int?), string userId = default(string), string changedBy = default(string))
        {
            var uri = new Uri(baseUri, $"InsertStudentExerciseResultWithAuditsFunc(StudentID={(studentId != null ? studentId : "null")},ExerciseID={(exerciseId != null ? exerciseId : "null")},DateTaken='{Uri.EscapeDataString(dateTaken.Trim().Replace("'", "''"))}',TimeTaken={(timeTaken != null ? timeTaken : "null")},MarkObtainable={(markObtainable != null ? markObtainable : "null")},MarkObtained={(markObtained != null ? markObtained : "null")},UserID='{Uri.EscapeDataString(userId.Trim().Replace("'", "''"))}',ChangedBy='{Uri.EscapeDataString(changedBy.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnInsertStudentExerciseResultWithAudits(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnUpdateStudentExerciseResultWithAudits(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> UpdateStudentExerciseResultWithAudits(long? resultId = default(long?), long? oldStudentId = default(long?), long? oldExerciseId = default(long?), string oldDateTaken = default(string), TimeSpan? oldTimeTaken = default(TimeSpan?), int? oldMarkObtainable = default(int?), int? oldMarkObtained = default(int?), string oldUserId = default(string), string oldChangedBy = default(string), long? newStudentId = default(long?), long? newExerciseId = default(long?), string newDateTaken = default(string), TimeSpan? newTimeTaken = default(TimeSpan?), int? newMarkObtainable = default(int?), int? newMarkObtained = default(int?), string newUserId = default(string), string newChangedBy = default(string))
        {
            var uri = new Uri(baseUri, $"UpdateStudentExerciseResultWithAuditsFunc(ResultID={(resultId != null ? resultId : "null")},OldStudentID={(oldStudentId != null ? oldStudentId : "null")},OldExerciseID={(oldExerciseId != null ? oldExerciseId : "null")},OldDateTaken='{Uri.EscapeDataString(oldDateTaken.Trim().Replace("'", "''"))}',OldTimeTaken={(oldTimeTaken != null ? oldTimeTaken : "null")},OldMarkObtainable={(oldMarkObtainable != null ? oldMarkObtainable : "null")},OldMarkObtained={(oldMarkObtained != null ? oldMarkObtained : "null")},OldUserID='{Uri.EscapeDataString(oldUserId.Trim().Replace("'", "''"))}',OldChangedBy='{Uri.EscapeDataString(oldChangedBy.Trim().Replace("'", "''"))}',NewStudentID={(newStudentId != null ? newStudentId : "null")},NewExerciseID={(newExerciseId != null ? newExerciseId : "null")},NewDateTaken='{Uri.EscapeDataString(newDateTaken.Trim().Replace("'", "''"))}',NewTimeTaken={(newTimeTaken != null ? newTimeTaken : "null")},NewMarkObtainable={(newMarkObtainable != null ? newMarkObtainable : "null")},NewMarkObtained={(newMarkObtained != null ? newMarkObtained : "null")},NewUserID='{Uri.EscapeDataString(newUserId.Trim().Replace("'", "''"))}',NewChangedBy='{Uri.EscapeDataString(newChangedBy.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnUpdateStudentExerciseResultWithAudits(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}