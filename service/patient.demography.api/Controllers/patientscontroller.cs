using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace patient.demography.api.Controllers
{
    [Route("api/patients")]
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
        public IActionResult getpatient(int patientid)
        {
            business.patientrepository objpatientrepository = null;

            try
            {
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    objpatientrepository = new business.patientrepository(db, UTCOffset);
                    if (objpatientrepository.getpatient(patientid, out patient temp, true))
                    {
                        var xx = new response() { data = temp };
                        return Ok(xx);
                    }
                    else
                    {
                        return Ok(new response { errormessage = new List<string>() { $"Invalid patient : {patientid}" } });
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

        [Route("delete/{patientid:int}")]
        [HttpPost]
        public IActionResult deletepatient(int patientid)
        {
            business.patientrepository objpatientrepository = null;

            try
            {
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    db.BeginTransaction();
                    objpatientrepository = new business.patientrepository(db, UTCOffset);
                    if (objpatientrepository.deletepatient(patientid))
                    {
                        db.Commit();
                        return Ok(new response { data = true });
                    }
                    else
                    {
                        return Ok(new response { errormessage = new List<string>() { $"Invalid patient : {patientid}" } });
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

        [Route("add")]
        [HttpPost]
        public IActionResult addpatient([FromBody]patient patient)
        {
            business.patientrepository objpatientrepository = null;
            business.patientphonenumberrepository objpatientphonenumberrepository = null;
            try
            {
                List<string> error = new List<string>();
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    db.BeginTransaction();
                    objpatientrepository = new business.patientrepository(db, UTCOffset);
                    if (!objpatientrepository.validatepatient(patient, out error))
                    {
                        if (objpatientrepository.addpatient(patient, out patient outdata))
                        {
                            objpatientphonenumberrepository = new business.patientphonenumberrepository(db, UTCOffset);
                            for (int i = 0; i < patient.patientphonenumbers.Count; i++)
                            {
                                patient.patientphonenumbers[i].isdeleted = false;
                                patient.patientphonenumbers[i].patientid = outdata.patientid;
                            }
                            objpatientphonenumberrepository.addpatientphonenumber(patient.patientphonenumbers);
                            db.Commit();
                            return Ok(new response { data = true });
                        }
                        else
                        {
                            return Ok(new response { errormessage = new List<string>() { "Please try after sometime" } });
                        }
                    }
                    else
                    {
                        return Ok(new response { errormessage = error });
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

        [Route("update")]
        [HttpPost]
        public IActionResult updatepatient([FromBody]patient patient)
        {
            business.patientrepository objpatientrepository = null;
            try
            {
                List<string> error = new List<string>();
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    objpatientrepository = new business.patientrepository(db, UTCOffset);
                    if (!objpatientrepository.validatepatient(patient, out error))
                    {
                        db.BeginTransaction();
                        if (objpatientrepository.updatepatient(patient))
                        {
                            db.Commit();
                            return Ok(new response { data = true });
                        }
                        else
                        {
                            return Ok(new response { errormessage = new List<string>() { "Please try after sometime" } });
                        }
                    }
                    else
                    {
                        return Ok(new response { errormessage = error });
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