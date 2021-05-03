using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service.Interfaces;
using System;
using System.IO;

namespace Service.Services
{
    public class WritableConfiguration<T> : IWritableConfiguration<T> where T : class, new()
    {
        private readonly IHostEnvironment _environment;
        private readonly IOptionsMonitor<T> _options;
        private readonly IConfigurationRoot _configuration;
        private readonly string _section;
        private readonly string _file;

        public WritableConfiguration(IHostEnvironment environment, IOptionsMonitor<T> options, IConfigurationRoot configuration, string section, string file)
        {
            _environment = environment;
            _options = options;
            _configuration = configuration;
            _section = section;
            _file = file;
        }

        public T Value => _options.CurrentValue;
        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;

            var jObject = fileInfo.Exists ? JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath)) : new JObject();
            var sectionObject = jObject!.TryGetValue(_section, out JToken? section) && section != null 
                ? JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject!);

            jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));

            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));

            _configuration.Reload();
        }
    }
}
