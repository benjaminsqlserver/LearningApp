using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using LearningApp.Server.Data;

namespace LearningApp.Server
{
    public partial class SvgDBService
    {
        SvgDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly SvgDBContext context;
        private readonly NavigationManager navigationManager;

        public SvgDBService(SvgDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportStudentExerciseResultsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresults/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresults/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentExerciseResultsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresults/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresults/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentExerciseResultsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResult> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResult>> GetStudentExerciseResults(Query query = null)
        {
            var items = Context.StudentExerciseResults.AsQueryable();

            items = items.Include(i => i.StudentExercise);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStudentExerciseResultsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentExerciseResultGet(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnGetStudentExerciseResultByResultId(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResult> items);


        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> GetStudentExerciseResultByResultId(long resultid)
        {
            var items = Context.StudentExerciseResults
                              .AsNoTracking()
                              .Where(i => i.ResultID == resultid);

            items = items.Include(i => i.StudentExercise);
 
            OnGetStudentExerciseResultByResultId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStudentExerciseResultGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStudentExerciseResultCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnAfterStudentExerciseResultCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> CreateStudentExerciseResult(LearningApp.Server.Models.SvgDB.StudentExerciseResult studentexerciseresult)
        {
            OnStudentExerciseResultCreated(studentexerciseresult);

            var existingItem = Context.StudentExerciseResults
                              .Where(i => i.ResultID == studentexerciseresult.ResultID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StudentExerciseResults.Add(studentexerciseresult);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(studentexerciseresult).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentExerciseResultCreated(studentexerciseresult);

            return studentexerciseresult;
        }

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> CancelStudentExerciseResultChanges(LearningApp.Server.Models.SvgDB.StudentExerciseResult item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentExerciseResultUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnAfterStudentExerciseResultUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> UpdateStudentExerciseResult(long resultid, LearningApp.Server.Models.SvgDB.StudentExerciseResult studentexerciseresult)
        {
            OnStudentExerciseResultUpdated(studentexerciseresult);

            var itemToUpdate = Context.StudentExerciseResults
                              .Where(i => i.ResultID == studentexerciseresult.ResultID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(studentexerciseresult);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStudentExerciseResultUpdated(studentexerciseresult);

            return studentexerciseresult;
        }

        partial void OnStudentExerciseResultDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnAfterStudentExerciseResultDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResult> DeleteStudentExerciseResult(long resultid)
        {
            var itemToDelete = Context.StudentExerciseResults
                              .Where(i => i.ResultID == resultid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentExerciseResultDeleted(itemToDelete);


            Context.StudentExerciseResults.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentExerciseResultDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStudentExerciseResultsAuditsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresultsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresultsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentExerciseResultsAuditsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexerciseresultsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexerciseresultsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentExerciseResultsAuditsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>> GetStudentExerciseResultsAudits(Query query = null)
        {
            var items = Context.StudentExerciseResultsAudits.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStudentExerciseResultsAuditsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentExerciseResultsAuditGet(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnGetStudentExerciseResultsAuditByAuditId(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> items);


        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> GetStudentExerciseResultsAuditByAuditId(long auditid)
        {
            var items = Context.StudentExerciseResultsAudits
                              .AsNoTracking()
                              .Where(i => i.AuditID == auditid);

 
            OnGetStudentExerciseResultsAuditByAuditId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStudentExerciseResultsAuditGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStudentExerciseResultsAuditCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnAfterStudentExerciseResultsAuditCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> CreateStudentExerciseResultsAudit(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit studentexerciseresultsaudit)
        {
            OnStudentExerciseResultsAuditCreated(studentexerciseresultsaudit);

            var existingItem = Context.StudentExerciseResultsAudits
                              .Where(i => i.AuditID == studentexerciseresultsaudit.AuditID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StudentExerciseResultsAudits.Add(studentexerciseresultsaudit);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(studentexerciseresultsaudit).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentExerciseResultsAuditCreated(studentexerciseresultsaudit);

            return studentexerciseresultsaudit;
        }

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> CancelStudentExerciseResultsAuditChanges(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentExerciseResultsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnAfterStudentExerciseResultsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> UpdateStudentExerciseResultsAudit(long auditid, LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit studentexerciseresultsaudit)
        {
            OnStudentExerciseResultsAuditUpdated(studentexerciseresultsaudit);

            var itemToUpdate = Context.StudentExerciseResultsAudits
                              .Where(i => i.AuditID == studentexerciseresultsaudit.AuditID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(studentexerciseresultsaudit);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStudentExerciseResultsAuditUpdated(studentexerciseresultsaudit);

            return studentexerciseresultsaudit;
        }

        partial void OnStudentExerciseResultsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnAfterStudentExerciseResultsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> DeleteStudentExerciseResultsAudit(long auditid)
        {
            var itemToDelete = Context.StudentExerciseResultsAudits
                              .Where(i => i.AuditID == auditid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentExerciseResultsAuditDeleted(itemToDelete);


            Context.StudentExerciseResultsAudits.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentExerciseResultsAuditDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStudentExercisesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexercises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexercises/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentExercisesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentexercises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentexercises/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentExercisesRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExercise> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.StudentExercise>> GetStudentExercises(Query query = null)
        {
            var items = Context.StudentExercises.AsQueryable();

            items = items.Include(i => i.Subject);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStudentExercisesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentExerciseGet(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnGetStudentExerciseByExerciseId(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExercise> items);


        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> GetStudentExerciseByExerciseId(long exerciseid)
        {
            var items = Context.StudentExercises
                              .AsNoTracking()
                              .Where(i => i.ExerciseID == exerciseid);

            items = items.Include(i => i.Subject);
 
            OnGetStudentExerciseByExerciseId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStudentExerciseGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStudentExerciseCreated(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnAfterStudentExerciseCreated(LearningApp.Server.Models.SvgDB.StudentExercise item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> CreateStudentExercise(LearningApp.Server.Models.SvgDB.StudentExercise studentexercise)
        {
            OnStudentExerciseCreated(studentexercise);

            var existingItem = Context.StudentExercises
                              .Where(i => i.ExerciseID == studentexercise.ExerciseID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StudentExercises.Add(studentexercise);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(studentexercise).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentExerciseCreated(studentexercise);

            return studentexercise;
        }

        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> CancelStudentExerciseChanges(LearningApp.Server.Models.SvgDB.StudentExercise item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentExerciseUpdated(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnAfterStudentExerciseUpdated(LearningApp.Server.Models.SvgDB.StudentExercise item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> UpdateStudentExercise(long exerciseid, LearningApp.Server.Models.SvgDB.StudentExercise studentexercise)
        {
            OnStudentExerciseUpdated(studentexercise);

            var itemToUpdate = Context.StudentExercises
                              .Where(i => i.ExerciseID == studentexercise.ExerciseID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(studentexercise);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStudentExerciseUpdated(studentexercise);

            return studentexercise;
        }

        partial void OnStudentExerciseDeleted(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnAfterStudentExerciseDeleted(LearningApp.Server.Models.SvgDB.StudentExercise item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentExercise> DeleteStudentExercise(long exerciseid)
        {
            var itemToDelete = Context.StudentExercises
                              .Where(i => i.ExerciseID == exerciseid)
                              .Include(i => i.StudentExerciseResults)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentExerciseDeleted(itemToDelete);


            Context.StudentExercises.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentExerciseDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStudentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/students/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/students/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.Student> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.Student>> GetStudents(Query query = null)
        {
            var items = Context.Students.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStudentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentGet(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnGetStudentByStudentId(ref IQueryable<LearningApp.Server.Models.SvgDB.Student> items);


        public async Task<LearningApp.Server.Models.SvgDB.Student> GetStudentByStudentId(long studentid)
        {
            var items = Context.Students
                              .AsNoTracking()
                              .Where(i => i.StudentID == studentid);

 
            OnGetStudentByStudentId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStudentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStudentCreated(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnAfterStudentCreated(LearningApp.Server.Models.SvgDB.Student item);

        public async Task<LearningApp.Server.Models.SvgDB.Student> CreateStudent(LearningApp.Server.Models.SvgDB.Student student)
        {
            OnStudentCreated(student);

            var existingItem = Context.Students
                              .Where(i => i.StudentID == student.StudentID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Students.Add(student);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(student).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentCreated(student);

            return student;
        }

        public async Task<LearningApp.Server.Models.SvgDB.Student> CancelStudentChanges(LearningApp.Server.Models.SvgDB.Student item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentUpdated(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnAfterStudentUpdated(LearningApp.Server.Models.SvgDB.Student item);

        public async Task<LearningApp.Server.Models.SvgDB.Student> UpdateStudent(long studentid, LearningApp.Server.Models.SvgDB.Student student)
        {
            OnStudentUpdated(student);

            var itemToUpdate = Context.Students
                              .Where(i => i.StudentID == student.StudentID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(student);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStudentUpdated(student);

            return student;
        }

        partial void OnStudentDeleted(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnAfterStudentDeleted(LearningApp.Server.Models.SvgDB.Student item);

        public async Task<LearningApp.Server.Models.SvgDB.Student> DeleteStudent(long studentid)
        {
            var itemToDelete = Context.Students
                              .Where(i => i.StudentID == studentid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentDeleted(itemToDelete);


            Context.Students.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStudentsAuditsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStudentsAuditsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/studentsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/studentsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStudentsAuditsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentsAudit> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.StudentsAudit>> GetStudentsAudits(Query query = null)
        {
            var items = Context.StudentsAudits.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStudentsAuditsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStudentsAuditGet(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnGetStudentsAuditByAuditId(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentsAudit> items);


        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> GetStudentsAuditByAuditId(long auditid)
        {
            var items = Context.StudentsAudits
                              .AsNoTracking()
                              .Where(i => i.AuditID == auditid);

 
            OnGetStudentsAuditByAuditId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStudentsAuditGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStudentsAuditCreated(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnAfterStudentsAuditCreated(LearningApp.Server.Models.SvgDB.StudentsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> CreateStudentsAudit(LearningApp.Server.Models.SvgDB.StudentsAudit studentsaudit)
        {
            OnStudentsAuditCreated(studentsaudit);

            var existingItem = Context.StudentsAudits
                              .Where(i => i.AuditID == studentsaudit.AuditID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.StudentsAudits.Add(studentsaudit);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(studentsaudit).State = EntityState.Detached;
                throw;
            }

            OnAfterStudentsAuditCreated(studentsaudit);

            return studentsaudit;
        }

        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> CancelStudentsAuditChanges(LearningApp.Server.Models.SvgDB.StudentsAudit item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStudentsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnAfterStudentsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> UpdateStudentsAudit(long auditid, LearningApp.Server.Models.SvgDB.StudentsAudit studentsaudit)
        {
            OnStudentsAuditUpdated(studentsaudit);

            var itemToUpdate = Context.StudentsAudits
                              .Where(i => i.AuditID == studentsaudit.AuditID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(studentsaudit);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStudentsAuditUpdated(studentsaudit);

            return studentsaudit;
        }

        partial void OnStudentsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnAfterStudentsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.StudentsAudit> DeleteStudentsAudit(long auditid)
        {
            var itemToDelete = Context.StudentsAudits
                              .Where(i => i.AuditID == auditid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStudentsAuditDeleted(itemToDelete);


            Context.StudentsAudits.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStudentsAuditDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSubjectsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjects/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSubjectsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjects/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSubjectsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.Subject> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.Subject>> GetSubjects(Query query = null)
        {
            var items = Context.Subjects.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSubjectsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSubjectGet(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnGetSubjectBySubjectId(ref IQueryable<LearningApp.Server.Models.SvgDB.Subject> items);


        public async Task<LearningApp.Server.Models.SvgDB.Subject> GetSubjectBySubjectId(long subjectid)
        {
            var items = Context.Subjects
                              .AsNoTracking()
                              .Where(i => i.SubjectID == subjectid);

 
            OnGetSubjectBySubjectId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSubjectGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSubjectCreated(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnAfterSubjectCreated(LearningApp.Server.Models.SvgDB.Subject item);

        public async Task<LearningApp.Server.Models.SvgDB.Subject> CreateSubject(LearningApp.Server.Models.SvgDB.Subject subject)
        {
            OnSubjectCreated(subject);

            var existingItem = Context.Subjects
                              .Where(i => i.SubjectID == subject.SubjectID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Subjects.Add(subject);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(subject).State = EntityState.Detached;
                throw;
            }

            OnAfterSubjectCreated(subject);

            return subject;
        }

        public async Task<LearningApp.Server.Models.SvgDB.Subject> CancelSubjectChanges(LearningApp.Server.Models.SvgDB.Subject item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSubjectUpdated(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnAfterSubjectUpdated(LearningApp.Server.Models.SvgDB.Subject item);

        public async Task<LearningApp.Server.Models.SvgDB.Subject> UpdateSubject(long subjectid, LearningApp.Server.Models.SvgDB.Subject subject)
        {
            OnSubjectUpdated(subject);

            var itemToUpdate = Context.Subjects
                              .Where(i => i.SubjectID == subject.SubjectID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(subject);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSubjectUpdated(subject);

            return subject;
        }

        partial void OnSubjectDeleted(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnAfterSubjectDeleted(LearningApp.Server.Models.SvgDB.Subject item);

        public async Task<LearningApp.Server.Models.SvgDB.Subject> DeleteSubject(long subjectid)
        {
            var itemToDelete = Context.Subjects
                              .Where(i => i.SubjectID == subjectid)
                              .Include(i => i.StudentExercises)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSubjectDeleted(itemToDelete);


            Context.Subjects.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSubjectDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSubjectsAuditsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjectsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjectsaudits/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSubjectsAuditsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/subjectsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/subjectsaudits/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSubjectsAuditsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.SubjectsAudit> items);

        public async Task<IQueryable<LearningApp.Server.Models.SvgDB.SubjectsAudit>> GetSubjectsAudits(Query query = null)
        {
            var items = Context.SubjectsAudits.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSubjectsAuditsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSubjectsAuditGet(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnGetSubjectsAuditByAuditId(ref IQueryable<LearningApp.Server.Models.SvgDB.SubjectsAudit> items);


        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> GetSubjectsAuditByAuditId(long auditid)
        {
            var items = Context.SubjectsAudits
                              .AsNoTracking()
                              .Where(i => i.AuditID == auditid);

 
            OnGetSubjectsAuditByAuditId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSubjectsAuditGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSubjectsAuditCreated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnAfterSubjectsAuditCreated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> CreateSubjectsAudit(LearningApp.Server.Models.SvgDB.SubjectsAudit subjectsaudit)
        {
            OnSubjectsAuditCreated(subjectsaudit);

            var existingItem = Context.SubjectsAudits
                              .Where(i => i.AuditID == subjectsaudit.AuditID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SubjectsAudits.Add(subjectsaudit);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(subjectsaudit).State = EntityState.Detached;
                throw;
            }

            OnAfterSubjectsAuditCreated(subjectsaudit);

            return subjectsaudit;
        }

        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> CancelSubjectsAuditChanges(LearningApp.Server.Models.SvgDB.SubjectsAudit item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSubjectsAuditUpdated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnAfterSubjectsAuditUpdated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> UpdateSubjectsAudit(long auditid, LearningApp.Server.Models.SvgDB.SubjectsAudit subjectsaudit)
        {
            OnSubjectsAuditUpdated(subjectsaudit);

            var itemToUpdate = Context.SubjectsAudits
                              .Where(i => i.AuditID == subjectsaudit.AuditID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(subjectsaudit);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSubjectsAuditUpdated(subjectsaudit);

            return subjectsaudit;
        }

        partial void OnSubjectsAuditDeleted(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnAfterSubjectsAuditDeleted(LearningApp.Server.Models.SvgDB.SubjectsAudit item);

        public async Task<LearningApp.Server.Models.SvgDB.SubjectsAudit> DeleteSubjectsAudit(long auditid)
        {
            var itemToDelete = Context.SubjectsAudits
                              .Where(i => i.AuditID == auditid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSubjectsAuditDeleted(itemToDelete);


            Context.SubjectsAudits.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSubjectsAuditDeleted(itemToDelete);

            return itemToDelete;
        }
    
      public async Task ExportGetStudentByAdmissionNumbersToExcel(string StudentAdmissionNumber, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentbyadmissionnumbers/excel(StudentAdmissionNumber='{StudentAdmissionNumber}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentbyadmissionnumbers/excel(StudentAdmissionNumber='{StudentAdmissionNumber}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task ExportGetStudentByAdmissionNumbersToCSV(string StudentAdmissionNumber, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentbyadmissionnumbers/csv(StudentAdmissionNumber='{StudentAdmissionNumber}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentbyadmissionnumbers/csv(StudentAdmissionNumber='{StudentAdmissionNumber}', fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task<IQueryable<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>> GetGetStudentByAdmissionNumbers(string StudentAdmissionNumber, Query query = null)
      {
          OnGetStudentByAdmissionNumbersDefaultParams(ref StudentAdmissionNumber);

          var items = Context.GetStudentByAdmissionNumbers.FromSqlInterpolated($"EXEC [dbo].[GetStudentByAdmissionNumber] {StudentAdmissionNumber}").ToList().AsQueryable();

          if (query != null)
          {
              if (!string.IsNullOrEmpty(query.Expand))
              {
                  var propertiesToExpand = query.Expand.Split(',');
                  foreach(var p in propertiesToExpand)
                  {
                      items = items.Include(p.Trim());
                  }
              }

              ApplyQuery(ref items, query);
          }
          
          OnGetStudentByAdmissionNumbersInvoke(ref items);

          return await Task.FromResult(items);
      }

      partial void OnGetStudentByAdmissionNumbersDefaultParams(ref string StudentAdmissionNumber);

      partial void OnGetStudentByAdmissionNumbersInvoke(ref IQueryable<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber> items);  

      public async Task ExportGetStudentExerciseResultsToExcel(long? StudentID, long? ExerciseID, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentexerciseresults/excel(StudentID={StudentID}, ExerciseID={ExerciseID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentexerciseresults/excel(StudentID={StudentID}, ExerciseID={ExerciseID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task ExportGetStudentExerciseResultsToCSV(long? StudentID, long? ExerciseID, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/getstudentexerciseresults/csv(StudentID={StudentID}, ExerciseID={ExerciseID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/getstudentexerciseresults/csv(StudentID={StudentID}, ExerciseID={ExerciseID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task<IQueryable<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult>> GetGetStudentExerciseResults(long? StudentID, long? ExerciseID, Query query = null)
      {
          OnGetStudentExerciseResultsDefaultParams(ref StudentID, ref ExerciseID);

          var items = Context.GetStudentExerciseResults.FromSqlInterpolated($"EXEC [dbo].[GetStudentExerciseResults] {StudentID}, {ExerciseID}").ToList().AsQueryable();

          if (query != null)
          {
              if (!string.IsNullOrEmpty(query.Expand))
              {
                  var propertiesToExpand = query.Expand.Split(',');
                  foreach(var p in propertiesToExpand)
                  {
                      items = items.Include(p.Trim());
                  }
              }

              ApplyQuery(ref items, query);
          }
          
          OnGetStudentExerciseResultsInvoke(ref items);

          return await Task.FromResult(items);
      }

      partial void OnGetStudentExerciseResultsDefaultParams(ref long? StudentID, ref long? ExerciseID);

      partial void OnGetStudentExerciseResultsInvoke(ref IQueryable<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult> items);  

      public async Task ExportLastStudentExerciseResultAuditRecordsToExcel(long? ResultID, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/laststudentexerciseresultauditrecords/excel(ResultID={ResultID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/laststudentexerciseresultauditrecords/excel(ResultID={ResultID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task ExportLastStudentExerciseResultAuditRecordsToCSV(long? ResultID, Query query = null, string fileName = null)
      {
          navigationManager.NavigateTo(query != null ? query.ToUrl($"export/svgdb/laststudentexerciseresultauditrecords/csv(ResultID={ResultID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/svgdb/laststudentexerciseresultauditrecords/csv(ResultID={ResultID}, fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
      }

      public async Task<IQueryable<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord>> GetLastStudentExerciseResultAuditRecords(long? ResultID, Query query = null)
      {
          OnLastStudentExerciseResultAuditRecordsDefaultParams(ref ResultID);

          var items = Context.LastStudentExerciseResultAuditRecords.FromSqlInterpolated($"EXEC [dbo].[LastStudentExerciseResultAuditRecord] {ResultID}").ToList().AsQueryable();

          if (query != null)
          {
              if (!string.IsNullOrEmpty(query.Expand))
              {
                  var propertiesToExpand = query.Expand.Split(',');
                  foreach(var p in propertiesToExpand)
                  {
                      items = items.Include(p.Trim());
                  }
              }

              ApplyQuery(ref items, query);
          }
          
          OnLastStudentExerciseResultAuditRecordsInvoke(ref items);

          return await Task.FromResult(items);
      }

      partial void OnLastStudentExerciseResultAuditRecordsDefaultParams(ref long? ResultID);

      partial void OnLastStudentExerciseResultAuditRecordsInvoke(ref IQueryable<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord> items);  
      public async Task<int> DeleteStudentExerciseResultWithAudits(long? ResultID, string ChangedBy)
      {
          OnDeleteStudentExerciseResultWithAuditsDefaultParams(ref ResultID, ref ChangedBy);

          SqlParameter[] @params =
          {
              new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@ResultID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = ResultID},
              new SqlParameter("@ChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = ChangedBy},

          };

          foreach(var _p in @params)
          {
              if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
              {
                  _p.Value = DBNull.Value;
              }
          }

          Context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[DeleteStudentExerciseResultWithAudit] @ResultID, @ChangedBy", @params);

          int result = Convert.ToInt32(@params[0].Value);


          OnDeleteStudentExerciseResultWithAuditsInvoke(ref result);

          return await Task.FromResult(result);
      }

      partial void OnDeleteStudentExerciseResultWithAuditsDefaultParams(ref long? ResultID, ref string ChangedBy);
      partial void OnDeleteStudentExerciseResultWithAuditsInvoke(ref int result);
      public async Task<int> InsertStudentExerciseResultWithAudits(long? StudentID, long? ExerciseID, string DateTaken, TimeSpan? TimeTaken, int? MarkObtainable, int? MarkObtained, string UserID, string ChangedBy)
      {
          OnInsertStudentExerciseResultWithAuditsDefaultParams(ref StudentID, ref ExerciseID, ref DateTaken, ref TimeTaken, ref MarkObtainable, ref MarkObtained, ref UserID, ref ChangedBy);

          SqlParameter[] @params =
          {
              new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@StudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = StudentID},
              new SqlParameter("@ExerciseID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = ExerciseID},
              new SqlParameter("@DateTaken", SqlDbType.Date, -1) {Direction = ParameterDirection.Input, Value = DateTaken},
              new SqlParameter("@TimeTaken", SqlDbType.Time, -1) {Direction = ParameterDirection.Input, Value = TimeTaken},
              new SqlParameter("@MarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = MarkObtainable},
              new SqlParameter("@MarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = MarkObtained},
              new SqlParameter("@UserID", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = UserID},
              new SqlParameter("@ChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = ChangedBy},

          };

          foreach(var _p in @params)
          {
              if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
              {
                  _p.Value = DBNull.Value;
              }
          }

          Context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[InsertStudentExerciseResultWithAudit] @StudentID, @ExerciseID, @DateTaken, @TimeTaken, @MarkObtainable, @MarkObtained, @UserID, @ChangedBy", @params);

          int result = Convert.ToInt32(@params[0].Value);


          OnInsertStudentExerciseResultWithAuditsInvoke(ref result);

          return await Task.FromResult(result);
      }

      partial void OnInsertStudentExerciseResultWithAuditsDefaultParams(ref long? StudentID, ref long? ExerciseID, ref string DateTaken, ref TimeSpan? TimeTaken, ref int? MarkObtainable, ref int? MarkObtained, ref string UserID, ref string ChangedBy);
      partial void OnInsertStudentExerciseResultWithAuditsInvoke(ref int result);
      public async Task<int> UpdateStudentExerciseResultWithAudits(long? ResultID, long? OldStudentID, long? OldExerciseID, string OldDateTaken, TimeSpan? OldTimeTaken, int? OldMarkObtainable, int? OldMarkObtained, string OldUserID, string OldChangedBy, long? NewStudentID, long? NewExerciseID, string NewDateTaken, TimeSpan? NewTimeTaken, int? NewMarkObtainable, int? NewMarkObtained, string NewUserID, string NewChangedBy)
      {
          OnUpdateStudentExerciseResultWithAuditsDefaultParams(ref ResultID, ref OldStudentID, ref OldExerciseID, ref OldDateTaken, ref OldTimeTaken, ref OldMarkObtainable, ref OldMarkObtained, ref OldUserID, ref OldChangedBy, ref NewStudentID, ref NewExerciseID, ref NewDateTaken, ref NewTimeTaken, ref NewMarkObtainable, ref NewMarkObtained, ref NewUserID, ref NewChangedBy);

          SqlParameter[] @params =
          {
              new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@ResultID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = ResultID},
              new SqlParameter("@OldStudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = OldStudentID},
              new SqlParameter("@OldExerciseID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = OldExerciseID},
              new SqlParameter("@OldDateTaken", SqlDbType.Date, -1) {Direction = ParameterDirection.Input, Value = OldDateTaken},
              new SqlParameter("@OldTimeTaken", SqlDbType.Time, -1) {Direction = ParameterDirection.Input, Value = OldTimeTaken},
              new SqlParameter("@OldMarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = OldMarkObtainable},
              new SqlParameter("@OldMarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = OldMarkObtained},
              new SqlParameter("@OldUserID", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = OldUserID},
              new SqlParameter("@OldChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = OldChangedBy},
              new SqlParameter("@NewStudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = NewStudentID},
              new SqlParameter("@NewExerciseID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = NewExerciseID},
              new SqlParameter("@NewDateTaken", SqlDbType.Date, -1) {Direction = ParameterDirection.Input, Value = NewDateTaken},
              new SqlParameter("@NewTimeTaken", SqlDbType.Time, -1) {Direction = ParameterDirection.Input, Value = NewTimeTaken},
              new SqlParameter("@NewMarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = NewMarkObtainable},
              new SqlParameter("@NewMarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = NewMarkObtained},
              new SqlParameter("@NewUserID", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = NewUserID},
              new SqlParameter("@NewChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = NewChangedBy},

          };

          foreach(var _p in @params)
          {
              if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
              {
                  _p.Value = DBNull.Value;
              }
          }

          Context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[UpdateStudentExerciseResultWithAudit] @ResultID, @OldStudentID, @OldExerciseID, @OldDateTaken, @OldTimeTaken, @OldMarkObtainable, @OldMarkObtained, @OldUserID, @OldChangedBy, @NewStudentID, @NewExerciseID, @NewDateTaken, @NewTimeTaken, @NewMarkObtainable, @NewMarkObtained, @NewUserID, @NewChangedBy", @params);

          int result = Convert.ToInt32(@params[0].Value);


          OnUpdateStudentExerciseResultWithAuditsInvoke(ref result);

          return await Task.FromResult(result);
      }

      partial void OnUpdateStudentExerciseResultWithAuditsDefaultParams(ref long? ResultID, ref long? OldStudentID, ref long? OldExerciseID, ref string OldDateTaken, ref TimeSpan? OldTimeTaken, ref int? OldMarkObtainable, ref int? OldMarkObtained, ref string OldUserID, ref string OldChangedBy, ref long? NewStudentID, ref long? NewExerciseID, ref string NewDateTaken, ref TimeSpan? NewTimeTaken, ref int? NewMarkObtainable, ref int? NewMarkObtained, ref string NewUserID, ref string NewChangedBy);
      partial void OnUpdateStudentExerciseResultWithAuditsInvoke(ref int result);
    }
}