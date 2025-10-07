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
    public partial class InsertStudentExerciseResultWithAuditsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public InsertStudentExerciseResultWithAuditsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [Route("odata/SvgDB/InsertStudentExerciseResultWithAuditsFunc(StudentID={StudentID},ExerciseID={ExerciseID},DateTaken={DateTaken},TimeTaken={TimeTaken},MarkObtainable={MarkObtainable},MarkObtained={MarkObtained},UserID={UserID},ChangedBy={ChangedBy})")]
        public IActionResult InsertStudentExerciseResultWithAuditsFunc([FromODataUri] long? StudentID, [FromODataUri] long? ExerciseID, [FromODataUri] string DateTaken, [FromODataUri] TimeSpan? TimeTaken, [FromODataUri] int? MarkObtainable, [FromODataUri] int? MarkObtained, [FromODataUri] string UserID, [FromODataUri] string ChangedBy)
        {
            this.OnInsertStudentExerciseResultWithAuditsDefaultParams(ref StudentID, ref ExerciseID, ref DateTaken, ref TimeTaken, ref MarkObtainable, ref MarkObtained, ref UserID, ref ChangedBy);


            SqlParameter[] @params =
            {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@StudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = StudentID},
              new SqlParameter("@ExerciseID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = ExerciseID},
              new SqlParameter("@DateTaken", SqlDbType.Date, -1) {Direction = ParameterDirection.Input, Value = DateTaken},
              new SqlParameter("@TimeTaken", SqlDbType.Time, -1) {Direction = ParameterDirection.Input, Value = TimeTaken},
              new SqlParameter("@MarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = MarkObtainable},
              new SqlParameter("@MarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = MarkObtained},
              new SqlParameter("@UserID", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = UserID},
              new SqlParameter("@ChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = ChangedBy},

            };

            foreach(var _p in @params)
            {
                if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
                {
                    _p.Value = DBNull.Value;
                }
            }

            this.context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[InsertStudentExerciseResultWithAudit] @StudentID, @ExerciseID, @DateTaken, @TimeTaken, @MarkObtainable, @MarkObtained, @UserID, @ChangedBy", @params);

            int result = Convert.ToInt32(@params[0].Value);

            this.OnInsertStudentExerciseResultWithAuditsInvoke(ref result);

            return Ok(result);
        }

        partial void OnInsertStudentExerciseResultWithAuditsDefaultParams(ref long? StudentID, ref long? ExerciseID, ref string DateTaken, ref TimeSpan? TimeTaken, ref int? MarkObtainable, ref int? MarkObtained, ref string UserID, ref string ChangedBy);
      partial void OnInsertStudentExerciseResultWithAuditsInvoke(ref int result);
    }
}
