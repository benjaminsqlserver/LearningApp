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
    public partial class DeleteStudentExerciseResultWithAuditsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public DeleteStudentExerciseResultWithAuditsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [Route("odata/SvgDB/DeleteStudentExerciseResultWithAuditsFunc(ResultID={ResultID},ChangedBy={ChangedBy})")]
        public IActionResult DeleteStudentExerciseResultWithAuditsFunc([FromODataUri] long? ResultID, [FromODataUri] string ChangedBy)
        {
            this.OnDeleteStudentExerciseResultWithAuditsDefaultParams(ref ResultID, ref ChangedBy);


            SqlParameter[] @params =
            {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@ResultID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = ResultID},
              new SqlParameter("@ChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = ChangedBy},

            };

            foreach(var _p in @params)
            {
                if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
                {
                    _p.Value = DBNull.Value;
                }
            }

            this.context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[DeleteStudentExerciseResultWithAudit] @ResultID, @ChangedBy", @params);

            int result = Convert.ToInt32(@params[0].Value);

            this.OnDeleteStudentExerciseResultWithAuditsInvoke(ref result);

            return Ok(result);
        }

        partial void OnDeleteStudentExerciseResultWithAuditsDefaultParams(ref long? ResultID, ref string ChangedBy);
      partial void OnDeleteStudentExerciseResultWithAuditsInvoke(ref int result);
    }
}
