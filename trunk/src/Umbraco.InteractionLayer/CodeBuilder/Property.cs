using System;

namespace Umbraco.InteractionLayer.CodeBuilder
{
    internal class Property
    {
        public string Name { get; set; }
        public Type DataType { get; set; }
        public bool Mandatory { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string ValidationRegex { get; set; }
    }
}
