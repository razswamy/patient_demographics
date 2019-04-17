using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace patient.demography.api
{
    public class XMLInputFormatter : TextInputFormatter
    {
        public readonly IHttpContextAccessor _httpcontext;
        public XMLInputFormatter(IHttpContextAccessor httpcontext)
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            _httpcontext = httpcontext;
        }

        protected override bool CanReadType(Type type)
        {
            return base.CanReadType(type);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;

            using (var reader = new StreamReader(request.Body, encoding))
            {
                try
                {
                    Type[] types = new Type[] { typeof(response), typeof(patient), typeof(patientphonenumber), typeof(griddata), typeof(List<string>), typeof(gendertype), typeof(phonenumbertype) };
                    var xmldeserializer = new XmlSerializer(typeof(T), types);
                    var stringWriter = new StringWriter();
                    using (var writer = XmlWriter.Create(stringWriter))
                    {
                        xmlserializer.Serialize(writer, t);
                        buffer = new StringBuilder(stringWriter.ToString());
                    }


                    var line = await reader.ReadLineAsync();
                    if (!line.StartsWith("BEGIN|VERSION:1.0|"))
                    {
                        var errorMessage = $"Data must start with 'BEGIN|VERSION:1.0|'";
                        context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                        throw new Exception(errorMessage);
                    }
                    if (!line.EndsWith("|END"))
                    {
                        var errorMessage = $"Data must end with '|END'";
                        context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                        throw new Exception(errorMessage);
                    }

                    var split = line.Substring(line.IndexOf("Data:") + 5).Split(new char[] { '|' });
                    var emp = new Employee()
                    {
                        Id = Convert.ToInt32(split[0]),
                        EmployeeCode = split[1],
                        FirstName = split[2],
                        LastName = split[3]
                    };

                    return await InputFormatterResult.SuccessAsync(emp);
                }
                catch
                {
                    return await InputFormatterResult.FailureAsync();
                }
            }
        }
    }
}