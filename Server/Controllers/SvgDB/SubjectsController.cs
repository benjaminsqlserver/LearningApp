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
    [Route("odata/SvgDB/Subjects")]
    public partial class SubjectsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public SubjectsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.Subject> GetSubjects()
        {
            var items = this.context.Subjects.AsQueryable<LearningApp.Server.Models.SvgDB.Subject>();
            this.OnSubjectsRead(ref items);

            return items;
        }

        partial void OnSubjectsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.Subject> items);

        partial void OnSubjectGet(ref SingleResult<LearningApp.Server.Models.SvgDB.Subject> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/Subjects(SubjectID={SubjectID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.Subject> GetSubject(long key)
        {
            var items = this.context.Subjects.Where(i => i.SubjectID == key);
            var result = SingleResult.Create(items);

            OnSubjectGet(ref result);

            return result;
        }
        partial void OnSubjectDeleted(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnAfterSubjectDeleted(LearningApp.Server.Models.SvgDB.Subject item);

        [HttpDelete("/odata/SvgDB/Subjects(SubjectID={SubjectID})")]
        public IActionResult DeleteSubject(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Subjects
                    .Where(i => i.SubjectID == key)
                    .Include(i => i.StudentExercises)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.Subject>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectDeleted(item);
                this.context.Subjects.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSubjectDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectUpdated(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnAfterSubjectUpdated(LearningApp.Server.Models.SvgDB.Subject item);

        [HttpPut("/odata/SvgDB/Subjects(SubjectID={SubjectID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSubject(long key, [FromBody]LearningApp.Server.Models.SvgDB.Subject item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Subjects
                    .Where(i => i.SubjectID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.Subject>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSubjectUpdated(item);
                this.context.Subjects.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Subjects.Where(i => i.SubjectID == key);
                
                this.OnAfterSubjectUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/Subjects(SubjectID={SubjectID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSubject(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.Subject> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Subjects
                    .Where(i => i.SubjectID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.Subject>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSubjectUpdated(item);
                this.context.Subjects.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Subjects.Where(i => i.SubjectID == key);
                
                this.OnAfterSubjectUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSubjectCreated(LearningApp.Server.Models.SvgDB.Subject item);
        partial void OnAfterSubjectCreated(LearningApp.Server.Models.SvgDB.Subject item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.Subject item)
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

                this.OnSubjectCreated(item);
                this.context.Subjects.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Subjects.Where(i => i.SubjectID == item.SubjectID);

                

                this.OnAfterSubjectCreated(item);

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
