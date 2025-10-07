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
    public partial class UpdateStudentExerciseResultWithAuditsController : ODataController
    {
        private LearningApp.Server.Data.SvgDBContext context;

        public UpdateStudentExerciseResultWithAuditsController(LearningApp.Server.Data.SvgDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [Route("odata/SvgDB/UpdateStudentExerciseResultWithAuditsFunc(ResultID={ResultID},OldStudentID={OldStudentID},OldExerciseID={OldExerciseID},OldDateTaken={OldDateTaken},OldTimeTaken={OldTimeTaken},OldMarkObtainable={OldMarkObtainable},OldMarkObtained={OldMarkObtained},OldUserID={OldUserID},OldChangedBy={OldChangedBy},NewStudentID={NewStudentID},NewExerciseID={NewExerciseID},NewDateTaken={NewDateTaken},NewTimeTaken={NewTimeTaken},NewMarkObtainable={NewMarkObtainable},NewMarkObtained={NewMarkObtained},NewUserID={NewUserID},NewChangedBy={NewChangedBy})")]
        public IActionResult UpdateStudentExerciseResultWithAuditsFunc([FromODataUri] long? ResultID, [FromODataUri] long? OldStudentID, [FromODataUri] long? OldExerciseID, [FromODataUri] string OldDateTaken, [FromODataUri] TimeSpan? OldTimeTaken, [FromODataUri] int? OldMarkObtainable, [FromODataUri] int? OldMarkObtained, [FromODataUri] string OldUserID, [FromODataUri] string OldChangedBy, [FromODataUri] long? NewStudentID, [FromODataUri] long? NewExerciseID, [FromODataUri] string NewDateTaken, [FromODataUri] TimeSpan? NewTimeTaken, [FromODataUri] int? NewMarkObtainable, [FromODataUri] int? NewMarkObtained, [FromODataUri] string NewUserID, [FromODataUri] string NewChangedBy)
        {
            this.OnUpdateStudentExerciseResultWithAuditsDefaultParams(ref ResultID, ref OldStudentID, ref OldExerciseID, ref OldDateTaken, ref OldTimeTaken, ref OldMarkObtainable, ref OldMarkObtained, ref OldUserID, ref OldChangedBy, ref NewStudentID, ref NewExerciseID, ref NewDateTaken, ref NewTimeTaken, ref NewMarkObtainable, ref NewMarkObtained, ref NewUserID, ref NewChangedBy);


            SqlParameter[] @params =
            {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
              new SqlParameter("@ResultID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = ResultID},
              new SqlParameter("@OldStudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = OldStudentID},
              new SqlParameter("@OldExerciseID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = OldExerciseID},
              new SqlParameter("@OldDateTaken", SqlDbType.Date, -1) {Direction = ParameterDirection.Input, Value = OldDateTaken},
              new SqlParameter("@OldTimeTaken", SqlDbType.Time, -1) {Direction = ParameterDirection.Input, Value = OldTimeTaken},
              new SqlParameter("@OldMarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = OldMarkObtainable},
              new SqlParameter("@OldMarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = OldMarkObtained},
              new SqlParameter("@OldUserID", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = OldUserID},
              new SqlParameter("@OldChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = OldChangedBy},
              new SqlParameter("@NewStudentID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = NewStudentID},
              new SqlParameter("@NewExerciseID", SqlDbType.BigInt, -1) {Direction = ParameterDirection.Input, Value = NewExerciseID},
              new SqlParameter("@NewDateTaken", SqlDbType.Date, -1) {Direction = ParameterDirection.Input, Value = NewDateTaken},
              new SqlParameter("@NewTimeTaken", SqlDbType.Time, -1) {Direction = ParameterDirection.Input, Value = NewTimeTaken},
              new SqlParameter("@NewMarkObtainable", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = NewMarkObtainable},
              new SqlParameter("@NewMarkObtained", SqlDbType.Int, -1) {Direction = ParameterDirection.Input, Value = NewMarkObtained},
              new SqlParameter("@NewUserID", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = NewUserID},
              new SqlParameter("@NewChangedBy", SqlDbType.NVarChar, 450) {Direction = ParameterDirection.Input, Value = NewChangedBy},

            };

            foreach(var _p in @params)
            {
                if((_p.Direction == ParameterDirection.Input || _p.Direction == ParameterDirection.InputOutput) && _p.Value == null)
                {
                    _p.Value = DBNull.Value;
                }
            }

            this.context.Database.ExecuteSqlRaw("EXEC @returnVal=[dbo].[UpdateStudentExerciseResultWithAudit] @ResultID, @OldStudentID, @OldExerciseID, @OldDateTaken, @OldTimeTaken, @OldMarkObtainable, @OldMarkObtained, @OldUserID, @OldChangedBy, @NewStudentID, @NewExerciseID, @NewDateTaken, @NewTimeTaken, @NewMarkObtainable, @NewMarkObtained, @NewUserID, @NewChangedBy", @params);

            int result = Convert.ToInt32(@params[0].Value);

            this.OnUpdateStudentExerciseResultWithAuditsInvoke(ref result);

            return Ok(result);
        }

        partial void OnUpdateStudentExerciseResultWithAuditsDefaultParams(ref long? ResultID, ref long? OldStudentID, ref long? OldExerciseID, ref string OldDateTaken, ref TimeSpan? OldTimeTaken, ref int? OldMarkObtainable, ref int? OldMarkObtained, ref string OldUserID, ref string OldChangedBy, ref long? NewStudentID, ref long? NewExerciseID, ref string NewDateTaken, ref TimeSpan? NewTimeTaken, ref int? NewMarkObtainable, ref int? NewMarkObtained, ref string NewUserID, ref string NewChangedBy);
      partial void OnUpdateStudentExerciseResultWithAuditsInvoke(ref int result);
    }
}
