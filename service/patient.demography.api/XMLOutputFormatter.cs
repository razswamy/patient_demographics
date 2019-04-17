using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
namespace patient.demography.api
{
    public class XMLOutputFormatter : TextOutputFormatter
    {
        public XMLOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            return base.CanWriteType(type);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            FormatCsv<object>(context.Object, out StringBuilder buffer);

            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                return writer.WriteAsync(buffer.ToString());
            }
        }

        private static void FormatCsv<T>(T t, out StringBuilder buffer)
        {
            Type[] types = new Type[] { typeof(response), typeof(patient), typeof(patientphonenumber), typeof(griddata), typeof(List<string>), typeof(gendertype), typeof(phonenumbertype) };
            var xmlserializer = new XmlSerializer(typeof(T), types);
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, t);
                buffer = new StringBuilder(stringWriter.ToString());
            }
        }
    }
}