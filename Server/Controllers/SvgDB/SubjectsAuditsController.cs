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
    [Route("odata/SvgDB/SubjectsAudits")]
    public partial class SubjectsAuditsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public SubjectsAuditsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.SubjectsAudit> GetSubjectsAudits()
        {
            var items = this.context.SubjectsAudits.AsQueryable<LearningApp.Server.Models.SvgDB.SubjectsAudit>();
            this.OnSubjectsAuditsRead(ref items);

            return items;
        }

        partial void OnSubjectsAuditsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.SubjectsAudit> items);

        partial void OnSubjectsAuditGet(ref SingleResult<LearningApp.Server.Models.SvgDB.SubjectsAudit> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/SubjectsAudits(AuditID={AuditID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.SubjectsAudit> GetSubjectsAudit(long key)
        {
            var items = this.context.SubjectsAudits.Where(i => i.AuditID == key);
            var result = SingleResult.Create(items);

            OnSubjectsAuditGet(ref result);

            return result;
        }
        partial void OnSubjectsAuditDeleted(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnAfterSubjectsAuditDeleted(LearningApp.Server.Models.SvgDB.SubjectsAudit item);

        [HttpDelete("/odata/SvgDB/SubjectsAudits(AuditID={AuditID})")]
        public IActionResult DeleteSubjectsAudit(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SubjectsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.SubjectsAudit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectsAuditDeleted(item);
                this.context.SubjectsAudits.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSubjectsAuditDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectsAuditUpdated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnAfterSubjectsAuditUpdated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);

        [HttpPut("/odata/SvgDB/SubjectsAudits(AuditID={AuditID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSubjectsAudit(long key, [FromBody]LearningApp.Server.Models.SvgDB.SubjectsAudit item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SubjectsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.SubjectsAudit>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectsAuditUpdated(item);
                this.context.SubjectsAudits.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SubjectsAudits.Where(i => i.AuditID == key);
                
                this.OnAfterSubjectsAuditUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/SubjectsAudits(AuditID={AuditID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSubjectsAudit(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.SubjectsAudit> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SubjectsAudits
                    .Where(i => i.AuditID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.SubjectsAudit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSubjectsAuditUpdated(item);
                this.context.SubjectsAudits.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SubjectsAudits.Where(i => i.AuditID == key);
                
                this.OnAfterSubjectsAuditUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectsAuditCreated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);
        partial void OnAfterSubjectsAuditCreated(LearningApp.Server.Models.SvgDB.SubjectsAudit item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.SubjectsAudit item)
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

                this.OnSubjectsAuditCreated(item);
                this.context.SubjectsAudits.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SubjectsAudits.Where(i => i.AuditID == item.AuditID);

                

                this.OnAfterSubjectsAuditCreated(item);

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
