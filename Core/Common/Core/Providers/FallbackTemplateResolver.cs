using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationServer.Core.Providers
{
    public class FallbackTemplateResolver : ITemplateResolver
    {
        public IEnumerable<string> Locations { get; set; }
        public string PathSeparator { get; set; }
        public ITemplateResolver Resolver { get; set; }



        public FallbackTemplateResolver() { }

        public FallbackTemplateResolver(IEnumerable<string> locations, ITemplateResolver resolver, string pathSeparator = "/")
        {
            Locations = locations;
            Resolver = resolver;
            PathSeparator = pathSeparator;
        }

        public string Resolve(string name, IDictionary<string, object> parameters = null)
        {
            var triedPaths = new List<string>();

            foreach (var location in Locations)
            {
                try
                {
                    var path = string.Join(PathSeparator, location, name);
                    triedPaths.Add(path);
                    var template = Resolver.Resolve(path, parameters);

                    if (template != null)
                        return template;
                }
                catch (TemplateResolvingException) { }
            }

            throw new TemplateResolvingException(
                string.Format(
                    "Could not find the template with name: {0}. Tried locations: \n{1}",
                    name,
                    string.Join("\n", triedPaths)
                )
            );
        }
    }
}
