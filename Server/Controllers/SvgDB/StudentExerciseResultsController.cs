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
    [Route("odata/SvgDB/StudentExerciseResults")]
    public partial class StudentExerciseResultsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public StudentExerciseResultsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.StudentExerciseResult> GetStudentExerciseResults()
        {
            var items = this.context.StudentExerciseResults.AsQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResult>();
            this.OnStudentExerciseResultsRead(ref items);

            return items;
        }

        partial void OnStudentExerciseResultsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExerciseResult> items);

        partial void OnStudentExerciseResultGet(ref SingleResult<LearningApp.Server.Models.SvgDB.StudentExerciseResult> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/StudentExerciseResults(ResultID={ResultID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.StudentExerciseResult> GetStudentExerciseResult(long key)
        {
            var items = this.context.StudentExerciseResults.Where(i => i.ResultID == key);
            var result = SingleResult.Create(items);

            OnStudentExerciseResultGet(ref result);

            return result;
        }
        partial void OnStudentExerciseResultDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnAfterStudentExerciseResultDeleted(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);

        [HttpDelete("/odata/SvgDB/StudentExerciseResults(ResultID={ResultID})")]
        public IActionResult DeleteStudentExerciseResult(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.StudentExerciseResults
                    .Where(i => i.ResultID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExerciseResult>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentExerciseResultDeleted(item);
                this.context.StudentExerciseResults.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStudentExerciseResultDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentExerciseResultUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnAfterStudentExerciseResultUpdated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);

        [HttpPut("/odata/SvgDB/StudentExerciseResults(ResultID={ResultID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStudentExerciseResult(long key, [FromBody]LearningApp.Server.Models.SvgDB.StudentExerciseResult item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentExerciseResults
                    .Where(i => i.ResultID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExerciseResult>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentExerciseResultUpdated(item);
                this.context.StudentExerciseResults.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExerciseResults.Where(i => i.ResultID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "StudentExercise");
                this.OnAfterStudentExerciseResultUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/StudentExerciseResults(ResultID={ResultID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStudentExerciseResult(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.StudentExerciseResult> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentExerciseResults
                    .Where(i => i.ResultID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExerciseResult>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStudentExerciseResultUpdated(item);
                this.context.StudentExerciseResults.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExerciseResults.Where(i => i.ResultID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "StudentExercise");
                this.OnAfterStudentExerciseResultUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentExerciseResultCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);
        partial void OnAfterStudentExerciseResultCreated(LearningApp.Server.Models.SvgDB.StudentExerciseResult item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.StudentExerciseResult item)
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

                this.OnStudentExerciseResultCreated(item);
                this.context.StudentExerciseResults.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExerciseResults.Where(i => i.ResultID == item.ResultID);

                Request.QueryString = Request.QueryString.Add("$expand", "StudentExercise");

                this.OnAfterStudentExerciseResultCreated(item);

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
