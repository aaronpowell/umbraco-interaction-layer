using System;
using System.Linq;
using System.Reflection;
using umbraco.cms.businesslogic.web;

namespace Umbraco.InteractionLayer.Library
{
    public sealed class Helper
    {
        private Helper() { }

        /// <summary>
        /// Gets the umbraco property or the default value
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="doc">The Umbraco Document to get the data from.</param>
        /// <param name="key">The property alias.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value of the Umbraco document or default if the property doesn't exist or is null</returns>
        /// <see>Aaron Powell - mailto:apowell@nextdigital.com </see>
        public static T GetPropertyValue<T>(Document doc, string key, T defaultValue)
        {
            if (doc.getProperty(key) != null)
            {
                if (doc.getProperty(key).Value == DBNull.Value)
                {
                    return defaultValue;
                }
                else
                {
                    return (T)doc.getProperty(key).Value;
                }
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the umbraco property or the default value
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="doc">The Umbraco Document to get the data from.</param>
        /// <param name="key">The property alias.</param>
        /// <returns>The value of the Umbraco property</returns>
        /// <see>Aaron Powell - mailto:apowell@nextdigital.com </see>
        public static T GetPropertyValue<T>(Document doc, string key)
        {
            return Helper.GetPropertyValue(doc, key, default(T));
        }

        /// <summary>
        /// Prebuilt function for getting the custom properites of the class
        /// </summary>
        /// <remarks>This is a Lambda function which will return all the properties of the current class which are custom DocType properties</remarks>
        public static Func<PropertyInfo, bool> CustomDocTypeProperties = p => p.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false).Count() != 0 && ((UmbracoFieldInfoAttribute)p.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false)[0]).IsCustom;

        /// <summary>
        /// Prebuild function for getting the mandatory properites of the class
        /// </summary>
        /// <remarks>This is a Lambda function which will return all the properties of the current class which are custom DocType properties and mandatory</remarks>
        public static Func<PropertyInfo, bool> MandatoryDocTypeProperties = p => p.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false).Count() != 0 && ((UmbracoFieldInfoAttribute)p.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false)[0]).IsCustom && ((UmbracoFieldInfoAttribute)p.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false)[0]).Mandatory;
    }
}
