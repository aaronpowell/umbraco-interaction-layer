using System.Collections.Generic;

namespace Umbraco.InteractionLayer.CodeBuilder
{
    internal class DocType
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public IEnumerable<Property> DocTypeProperties { get; set; }
        public IEnumerable<int> ChildContentTypes { get; set; }
    }
}
