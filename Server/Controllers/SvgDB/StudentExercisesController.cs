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
    [Route("odata/SvgDB/StudentExercises")]
    public partial class StudentExercisesController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public StudentExercisesController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.StudentExercise> GetStudentExercises()
        {
            var items = this.context.StudentExercises.AsQueryable<LearningApp.Server.Models.SvgDB.StudentExercise>();
            this.OnStudentExercisesRead(ref items);

            return items;
        }

        partial void OnStudentExercisesRead(ref IQueryable<LearningApp.Server.Models.SvgDB.StudentExercise> items);

        partial void OnStudentExerciseGet(ref SingleResult<LearningApp.Server.Models.SvgDB.StudentExercise> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/StudentExercises(ExerciseID={ExerciseID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.StudentExercise> GetStudentExercise(long key)
        {
            var items = this.context.StudentExercises.Where(i => i.ExerciseID == key);
            var result = SingleResult.Create(items);

            OnStudentExerciseGet(ref result);

            return result;
        }
        partial void OnStudentExerciseDeleted(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnAfterStudentExerciseDeleted(LearningApp.Server.Models.SvgDB.StudentExercise item);

        [HttpDelete("/odata/SvgDB/StudentExercises(ExerciseID={ExerciseID})")]
        public IActionResult DeleteStudentExercise(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.StudentExercises
                    .Where(i => i.ExerciseID == key)
                    .Include(i => i.StudentExerciseResults)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExercise>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentExerciseDeleted(item);
                this.context.StudentExercises.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStudentExerciseDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentExerciseUpdated(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnAfterStudentExerciseUpdated(LearningApp.Server.Models.SvgDB.StudentExercise item);

        [HttpPut("/odata/SvgDB/StudentExercises(ExerciseID={ExerciseID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStudentExercise(long key, [FromBody]LearningApp.Server.Models.SvgDB.StudentExercise item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentExercises
                    .Where(i => i.ExerciseID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExercise>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentExerciseUpdated(item);
                this.context.StudentExercises.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExercises.Where(i => i.ExerciseID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Subject");
                this.OnAfterStudentExerciseUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/StudentExercises(ExerciseID={ExerciseID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStudentExercise(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.StudentExercise> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StudentExercises
                    .Where(i => i.ExerciseID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.StudentExercise>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStudentExerciseUpdated(item);
                this.context.StudentExercises.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExercises.Where(i => i.ExerciseID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Subject");
                this.OnAfterStudentExerciseUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentExerciseCreated(LearningApp.Server.Models.SvgDB.StudentExercise item);
        partial void OnAfterStudentExerciseCreated(LearningApp.Server.Models.SvgDB.StudentExercise item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.StudentExercise item)
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

                this.OnStudentExerciseCreated(item);
                this.context.StudentExercises.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StudentExercises.Where(i => i.ExerciseID == item.ExerciseID);

                Request.QueryString = Request.QueryString.Add("$expand", "Subject");

                this.OnAfterStudentExerciseCreated(item);

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
