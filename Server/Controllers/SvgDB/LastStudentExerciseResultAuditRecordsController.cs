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
    public partial class LastStudentExerciseResultAuditRecordsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public LastStudentExerciseResultAuditRecordsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [Route("odata/SvgDB/LastStudentExerciseResultAuditRecordsFunc(ResultID={ResultID})")]
        public IActionResult LastStudentExerciseResultAuditRecordsFunc([FromODataUri] long? ResultID)
        {
            this.OnLastStudentExerciseResultAuditRecordsDefaultParams(ref ResultID);

            var items = this.context.LastStudentExerciseResultAuditRecords.FromSqlInterpolated($"EXEC [dbo].[LastStudentExerciseResultAuditRecord] {ResultID}").ToList().AsQueryable();

            this.OnLastStudentExerciseResultAuditRecordsInvoke(ref items);

            return Ok(items);
        }

        partial void OnLastStudentExerciseResultAuditRecordsDefaultParams(ref long? ResultID);

        partial void OnLastStudentExerciseResultAuditRecordsInvoke(ref IQueryable<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord> items);
    }
}
