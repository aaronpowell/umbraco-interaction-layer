using System;

namespace Umbraco.InteractionLayer.Library
{
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class UmbracoFieldInfoAttribute : Attribute
    {
        public UmbracoFieldInfoAttribute() { }

        public string DisplayName { get; set; }
        public string Alias { get; set; }
        public bool Mandatory { get; set; }
        public bool IsCustom { get; set; }
    }

    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class UmbracoDocTypeInfoAttribute : Attribute
    {
        public UmbracoDocTypeInfoAttribute() { }

        public string Alias { get; set; }
        /// <summary>
        /// Gets or sets the DocumentTypeId
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }
    }
}
