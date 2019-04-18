using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace patient.demography.api.Controllers
{
    [Route("api/patients/phonenumber")]
    public class patientsphonenumbercontroller : apibase
    {
        public patientsphonenumbercontroller(IOptions<ConnectionStrings> connectionstrings, ILogger<patientsphonenumbercontroller> logger, IHttpContextAccessor httpcontext) : base(connectionstrings, logger, httpcontext)
        {
        }

        [Route("{patientid:int}")]
        [HttpGet]
        public IActionResult getpatientsphonenumber(int patientid)
        {
            business.patientphonenumberrepository objpatientphonenumberrepository = null;

            try
            {
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    objpatientphonenumberrepository = new business.patientphonenumberrepository(db, UTCOffset);
                    var xx = new response() { data = objpatientphonenumberrepository.getallpatientphonenumbers(patientid) };
                    return Ok(xx);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, buildapierror(logerror(ex)));
            }
            finally
            {
                objpatientphonenumberrepository = null;
            }
        }

        [Route("delete/{patientphonenumberid:int}")]
        [HttpPost]
        public IActionResult deletepatientphonenumber(int patientphonenumberid)
        {
            business.patientphonenumberrepository objpatientphonenumberrepository = null;

            try
            {
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    db.BeginTransaction();
                    objpatientphonenumberrepository = new business.patientphonenumberrepository(db, UTCOffset);
                    if (objpatientphonenumberrepository.deletepatientphonenumber(patientphonenumberid))
                    {
                        db.Commit();
                        return Ok(new response { data = true });
                    }
                    else
                    {
                        return Ok(new response { errormessage = new List<string>() { $"Invalid patient phone number : {patientphonenumberid}" } });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, buildapierror(logerror(ex)));
            }
            finally
            {
                objpatientphonenumberrepository = null;
            }
        }

        [Route("add")]
        [HttpPost]
        public IActionResult addpatientphonenumber([FromBody]patientphonenumber patientphonenumber)
        {
            business.patientphonenumberrepository objpatientphonenumberrepository = null;
            try
            {
                List<string> error = new List<string>();
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    db.BeginTransaction();
                    objpatientphonenumberrepository = new business.patientphonenumberrepository(db, UTCOffset);
                    if (!objpatientphonenumberrepository.validatepatientphonenumber(patientphonenumber, out error))
                    {
                        if (objpatientphonenumberrepository.addpatientphonenumber(patientphonenumber, out patientphonenumber outdata))
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
                objpatientphonenumberrepository = null;
            }
        }

        [Route("update")]
        [HttpPost]
        public IActionResult updatepatientphonenumber([FromBody]patientphonenumber patientphonenumber)
        {
            business.patientphonenumberrepository objpatientphonenumberrepository = null;
            try
            {
                List<string> error = new List<string>();
                using (var db = new helpers.dbcontext(connectionstring))
                {
                    objpatientphonenumberrepository = new business.patientphonenumberrepository(db, UTCOffset);
                    if (!objpatientphonenumberrepository.validatepatientphonenumber(patientphonenumber, out error))
                    {
                        db.BeginTransaction();
                        if (objpatientphonenumberrepository.updatepatientphonenumber(patientphonenumber))
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
                objpatientphonenumberrepository = null;
            }
        }
    }
}