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
    public partial class GetStudentExerciseResultsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public GetStudentExerciseResultsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [Route("odata/SvgDB/GetStudentExerciseResultsFunc(StudentID={StudentID},ExerciseID={ExerciseID})")]
        public IActionResult GetStudentExerciseResultsFunc([FromODataUri] long? StudentID, [FromODataUri] long? ExerciseID)
        {
            this.OnGetStudentExerciseResultsDefaultParams(ref StudentID, ref ExerciseID);

            var items = this.context.GetStudentExerciseResults.FromSqlInterpolated($"EXEC [dbo].[GetStudentExerciseResults] {StudentID}, {ExerciseID}").ToList().AsQueryable();

            this.OnGetStudentExerciseResultsInvoke(ref items);

            return Ok(items);
        }

        partial void OnGetStudentExerciseResultsDefaultParams(ref long? StudentID, ref long? ExerciseID);

        partial void OnGetStudentExerciseResultsInvoke(ref IQueryable<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult> items);
    }
}
