using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Core.CrossCuttingConcerns.Logging.Log4net.Layouts
{
    public class JsonLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var logEvent = new SerializableLogEvent(loggingEvent);

            var json = JsonConvert.SerializeObject(logEvent, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            string logPath = $"{ConfigurationManager.AppSettings["LogYol"]}{(json.IndexOf("EFEntityRepository") != -1 ? "context" : "global")}\\{loggingEvent.Level}\\{DateTime.Now.ToString("dd-MM-yyyy")}\\";

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            var file = $"{logPath}log{DateTime.Now.ToString("HH")}";

            var serializableLogEvents = new List<SerializableLogEvent>();
            var allJsonString = string.Empty;

            if(File.Exists(file))
            {
                using (TextReader reader = File.OpenText(file))
                {
                    allJsonString = reader.ReadToEnd();
                }

                serializableLogEvents = JsonConvert.DeserializeObject<List<SerializableLogEvent>>(allJsonString);
            }

            serializableLogEvents.Add(logEvent);

            File.WriteAllText(file, JsonConvert.SerializeObject(serializableLogEvents, Formatting.Indented), Encoding.UTF8);

            //log4net.config dosyasında belirtilen log dosyasına yazıyor.
            //writer.WriteLine(json);
        }
    }
}
