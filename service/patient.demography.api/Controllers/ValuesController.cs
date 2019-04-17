using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace patient.demography.api.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class patientscontroller : apibase
    {
        public patientscontroller(IOptions<ConnectionStrings> connectionstrings, ILogger<patientscontroller> logger, IHttpContextAccessor httpcontext) : base(connectionstrings, logger, httpcontext)
        {
        }

        [Route("")]
        [HttpGet]
        public IActionResult getpatients([FromQuery(Name = "limit")] int limit = 10, [FromQuery(Name = "offset")] int offset = 0, [FromQuery(Name = "searchkey")] string searchkey = "", [FromQuery(Name = "sort")] string sort = "", [FromQuery(Name = "sortorder")] string sortorder = "", [FromQuery(Name = "otherparams")] string otherparams = "")
        {
            business.patientrepository objpatientrepository = null;
            try
            {
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    objpatientrepository = new business.patientrepository(db, UTCOffset);

                    return Ok(new response()
                    {
                        data = objpatientrepository.getpatients(new griddata()
                        {
                            limit = limit,
                            offset = offset,
                            searchkey = searchkey,
                            sort = sort,
                            sortorder = sortorder,
                            otherparameters = otherparams
                        })
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, buildapierror(logerror(ex)));
            }
            finally
            {
                objpatientrepository = null;
            }
        }

        [Route("{patientid:int}")]
        [HttpGet]
        public IActionResult getcampaign(int patientid)
        {
            business.patientrepository objpatientrepository = null;

            try
            {
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    objpatientrepository = new business.patientrepository(db, UTCOffset);
                    if (objpatientrepository.getpatient(patientid, out patient temp, true))
                    {
                        return Ok(new response() { data = temp });
                    }
                    else
                    {
                        return Ok(new response { errormessage = new List<string>() { $"Invalid campaign type : {patientid}" } });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, buildapierror(logerror(ex)));
            }
            finally
            {
                objpatientrepository = null;
            }
        }
    }
}