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
    [Route("odata/SvgDB/StudentsAudits")]
    public partial class StudentsAuditsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public StudentsAuditsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.StudentsAudit> GetStudentsAudits()
        {
            var items = this.context.StudentsAudits.AsQueryable<LearningApp.Server.Models.SvgDB.StudentsAudit>();
            this.OnStudentsAuditsRead(ref items);

            return items;
        }

        partial void OnStudentsAuditsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentsAudit> items);

        partial void OnStudentsAuditGet(ref SingleResult<LearningApp.Server.Models.SvgDB.StudentsAudit> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/StudentsAudits(AuditID={AuditID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.StudentsAudit> GetStudentsAudit(long key)
        {
            var items = this.context.StudentsAudits.Where(i => i.AuditID == key);
            var result = SingleResult.Create(items);

            OnStudentsAuditGet(ref result);

            return result;
        }
        partial void OnStudentsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnAfterStudentsAuditDeleted(LearningApp.Server.Models.SvgDB.StudentsAudit item);

        [HttpDelete("/odata/SvgDB/StudentsAudits(AuditID={AuditID})")]
        public IActionResult DeleteStudentsAudit(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.StudentsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentsAudit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentsAuditDeleted(item);
                this.context.StudentsAudits.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStudentsAuditDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnAfterStudentsAuditUpdated(LearningApp.Server.Models.SvgDB.StudentsAudit item);

        [HttpPut("/odata/SvgDB/StudentsAudits(AuditID={AuditID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStudentsAudit(long key, [FromBody]LearningApp.Server.Models.SvgDB.StudentsAudit item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentsAudit>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentsAuditUpdated(item);
                this.context.StudentsAudits.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentsAudits.Where(i => i.AuditID == key);
                
                this.OnAfterStudentsAuditUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/StudentsAudits(AuditID={AuditID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStudentsAudit(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.StudentsAudit> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentsAudit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStudentsAuditUpdated(item);
                this.context.StudentsAudits.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentsAudits.Where(i => i.AuditID == key);
                
                this.OnAfterStudentsAuditUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentsAuditCreated(LearningApp.Server.Models.SvgDB.StudentsAudit item);
        partial void OnAfterStudentsAuditCreated(LearningApp.Server.Models.SvgDB.StudentsAudit item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.StudentsAudit item)
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

                this.OnStudentsAuditCreated(item);
                this.context.StudentsAudits.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentsAudits.Where(i => i.AuditID == item.AuditID);

                

                this.OnAfterStudentsAuditCreated(item);

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
