
using Microsoft.AspNetCore.Http;
namespace patient.demography.helpers
{
    public static class utilites
    {
        public static bool getheadervalue(HttpRequest request, string headername, out string value)
        {
            if (request.Headers.ContainsKey(headername))
            {
                value = request.Headers[headername];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
