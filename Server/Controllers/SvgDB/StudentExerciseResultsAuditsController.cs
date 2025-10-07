using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LearningApp.Server.Controllers.SvgDB
{
    [Route("odata/SvgDB/StudentExerciseResultsAudits")]
    public partial class StudentExerciseResultsAuditsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public StudentExerciseResultsAuditsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> GetStudentExerciseResultsAudits()
        {
            var items = this.context.StudentExerciseResultsAudits.AsQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>();
            this.OnStudentExerciseResultsAuditsRead(ref items);

            return items;
        }

        partial void OnStudentExerciseResultsAuditsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> items);

        partial void OnStudentExerciseResultsAuditGet(ref SingleResult<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/StudentExerciseResultsAudits(AuditID={AuditID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> GetStudentExerciseResultsAudit(long key)
        {
            var items = this.context.StudentExerciseResultsAudits.Where(i => i.AuditID == key);
            var result = SingleResult.Create(items);

            OnStudentExerciseResultsAuditGet(ref result);

            return result;
        }
        partial void OnStudentExerciseResultsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnAfterStudentExerciseResultsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);

        [HttpDelete("/odata/SvgDB/StudentExerciseResultsAudits(AuditID={AuditID})")]
        public IActionResult DeleteStudentExerciseResultsAudit(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.StudentExerciseResultsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentExerciseResultsAuditDeleted(item);
                this.context.StudentExerciseResultsAudits.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStudentExerciseResultsAuditDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentExerciseResultsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnAfterStudentExerciseResultsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);

        [HttpPut("/odata/SvgDB/StudentExerciseResultsAudits(AuditID={AuditID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStudentExerciseResultsAudit(long key, [FromBody]LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentExerciseResultsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentExerciseResultsAuditUpdated(item);
                this.context.StudentExerciseResultsAudits.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExerciseResultsAudits.Where(i => i.AuditID == key);
                
                this.OnAfterStudentExerciseResultsAuditUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/StudentExerciseResultsAudits(AuditID={AuditID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStudentExerciseResultsAudit(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentExerciseResultsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStudentExerciseResultsAuditUpdated(item);
                this.context.StudentExerciseResultsAudits.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExerciseResultsAudits.Where(i => i.AuditID == key);
                
                this.OnAfterStudentExerciseResultsAuditUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentExerciseResultsAuditCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);
        partial void OnAfterStudentExerciseResultsAuditCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnStudentExerciseResultsAuditCreated(item);
                this.context.StudentExerciseResultsAudits.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExerciseResultsAudits.Where(i => i.AuditID == item.AuditID);

                

                this.OnAfterStudentExerciseResultsAuditCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
