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
    [Route("odata/SvgDB/Students")]
    public partial class StudentsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public StudentsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<LearningApp.Server.Models.SvgDB.Student> GetStudents()
        {
            var items = this.context.Students.AsQueryable<LearningApp.Server.Models.SvgDB.Student>();
            this.OnStudentsRead(ref items);

            return items;
        }

        partial void OnStudentsRead(ref IQueryable<LearningApp.Server.Models.SvgDB.Student> items);

        partial void OnStudentGet(ref SingleResult<LearningApp.Server.Models.SvgDB.Student> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/SvgDB/Students(StudentID={StudentID})")]
        public SingleResult<LearningApp.Server.Models.SvgDB.Student> GetStudent(long key)
        {
            var items = this.context.Students.Where(i => i.StudentID == key);
            var result = SingleResult.Create(items);

            OnStudentGet(ref result);

            return result;
        }
        partial void OnStudentDeleted(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnAfterStudentDeleted(LearningApp.Server.Models.SvgDB.Student item);

        [HttpDelete("/odata/SvgDB/Students(StudentID={StudentID})")]
        public IActionResult DeleteStudent(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Students
                    .Where(i => i.StudentID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.Student>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentDeleted(item);
                this.context.Students.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStudentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentUpdated(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnAfterStudentUpdated(LearningApp.Server.Models.SvgDB.Student item);

        [HttpPut("/odata/SvgDB/Students(StudentID={StudentID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStudent(long key, [FromBody]LearningApp.Server.Models.SvgDB.Student item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Students
                    .Where(i => i.StudentID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.Student>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStudentUpdated(item);
                this.context.Students.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Students.Where(i => i.StudentID == key);
                
                this.OnAfterStudentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/SvgDB/Students(StudentID={StudentID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStudent(long key, [FromBody]Delta<LearningApp.Server.Models.SvgDB.Student> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Students
                    .Where(i => i.StudentID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<LearningApp.Server.Models.SvgDB.Student>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStudentUpdated(item);
                this.context.Students.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Students.Where(i => i.StudentID == key);
                
                this.OnAfterStudentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStudentCreated(LearningApp.Server.Models.SvgDB.Student item);
        partial void OnAfterStudentCreated(LearningApp.Server.Models.SvgDB.Student item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] LearningApp.Server.Models.SvgDB.Student item)
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

                this.OnStudentCreated(item);
                this.context.Students.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Students.Where(i => i.StudentID == item.StudentID);

                

                this.OnAfterStudentCreated(item);

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
