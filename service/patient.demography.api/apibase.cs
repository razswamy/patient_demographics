using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace patient.demography
{
    public class apibase : Controller
    {
        public readonly ConnectionStrings _connectionstrings;
        public readonly ILogger<apibase> _logger;
        public readonly IHttpContextAccessor _httpcontext;


        public apibase(IOptions<ConnectionStrings> connectionstrings, ILogger<apibase> logger, IHttpContextAccessor httpcontext)
        {
            _connectionstrings = connectionstrings.Value;
            _logger = logger;
            _httpcontext = httpcontext;
        }

        public string connectionstring
        {
            get
            {
                return _connectionstrings.appconnection;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string logerror(Exception ex, string customdata = "")
        {
            var guid = Guid.NewGuid().ToString();
            _logger.LogError(ex, "{guid}", guid);
            return guid;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public response buildapiresponse(object data)
        {
            return new response() { data = data, };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public response buildapierror(string error)
        {
            return new response() { errormessage = new List<string>() { error } };
        }


        public int UTCOffset
        {
            get
            {
                string utc = "";
                if (helpers.utilites.getheadervalue(_httpcontext.HttpContext.Request, "UTCOffset", out utc))
                {
                    int dd = 0;
                    if (int.TryParse(utc, out dd))
                    {
                        return dd;
                    }
                    else
                    {
                        return Convert.ToInt32(DateTimeOffset.Now.Offset.TotalMinutes);
                    }
                }
                else
                {
                    return Convert.ToInt32(DateTimeOffset.Now.Offset.TotalMinutes);
                }
            }
        }

    }
}