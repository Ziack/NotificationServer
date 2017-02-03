using NotificationServer.Core;
using NotificationServer.Core.Providers;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;

namespace NotificationServer.Templating.Razor
{
    public class RazorTemplateEngineService : ITemplateEngineService, IAllowConfigurationOverride
    {
        public ITemplateServiceConfiguration Config { get; set; }


        public RazorTemplateEngineService() { }

        public RazorTemplateEngineService(ITemplateServiceConfiguration config)
        {
            Config = config;
        }

        public void Dispose() { }

        public string Parse(string template, object model)
        {
            var templateContents = string.Empty;

            if (Config.CachingProvider != null)
            {
                if (!Config.CachingProvider.TryRetrieveTemplate(template, out templateContents))
                {
                    templateContents = Config.Resolver.Resolve(template);
                    Config.CachingProvider.CacheTemplate(templateContents, template);
                }
            }
            else
                templateContents = Config.Resolver.Resolve(template);

            return Engine.Razor.RunCompile(templateContents, Guid.NewGuid().ToString(), model.GetType(), model);
        }

        public IEnumerable<string> ParseMany(IEnumerable<string> templates, IEnumerable<object> models, bool parallel)
        {
            throw new NotImplementedException();
        }

        static RazorTemplateEngineService()
        {
            Engine.Razor = RazorEngineService.Create(new RazorEngine.Configuration.TemplateServiceConfiguration
            {
                EncodedStringFactory = new RawStringFactory()
            });
        }
    }
}
