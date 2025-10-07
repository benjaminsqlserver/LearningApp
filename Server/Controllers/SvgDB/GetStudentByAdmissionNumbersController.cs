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
    public partial class GetStudentByAdmissionNumbersController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public GetStudentByAdmissionNumbersController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [Route("odata/SvgDB/GetStudentByAdmissionNumbersFunc(StudentAdmissionNumber={StudentAdmissionNumber})")]
        public IActionResult GetStudentByAdmissionNumbersFunc([FromODataUri] string StudentAdmissionNumber)
        {
            this.OnGetStudentByAdmissionNumbersDefaultParams(ref StudentAdmissionNumber);

            var items = this.context.GetStudentByAdmissionNumbers.FromSqlInterpolated($"EXEC [dbo].[GetStudentByAdmissionNumber] {StudentAdmissionNumber}").ToList().AsQueryable();

            this.OnGetStudentByAdmissionNumbersInvoke(ref items);

            return Ok(items);
        }

        partial void OnGetStudentByAdmissionNumbersDefaultParams(ref string StudentAdmissionNumber);

        partial void OnGetStudentByAdmissionNumbersInvoke(ref IQueryable<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber> items);
    }
}
