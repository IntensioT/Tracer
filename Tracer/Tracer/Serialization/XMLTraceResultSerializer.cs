using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Tracer.Serialization
{
    public class XMLTraceResultSerializer : ISerializable
    {
        public string Serialize(TraceResultStruct result)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResultStruct));

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };

            using (StringWriter textWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(textWriter, settings))
                {
                    xmlSerializer.Serialize(writer, result);
                }
                return textWriter.ToString();
            }
        }
    }
}
