using System;
using System.Collections.Generic;
using umbraco.cms.businesslogic.propertytype;

namespace Umbraco.InteractionLayer.CodeBuilder
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> col, Action<T> act)
        {
            if (col != null)
            {
                foreach (var item in col)
                {
                    act(item);
                }
            }
        }

        public static Type GetDotNetType(this PropertyType pt)
        {
            Type t = null;

            var dataType = pt.DataTypeDefinition.DataType;

            var dbType = ((umbraco.cms.businesslogic.datatype.BaseDataType)dataType).DBType;

            switch (dbType)
            {
                case umbraco.cms.businesslogic.datatype.DBTypes.Date:
                    t = typeof(DateTime);
                    break;
                case umbraco.cms.businesslogic.datatype.DBTypes.Integer:
                    t = typeof(int);
                    break;
                case umbraco.cms.businesslogic.datatype.DBTypes.Ntext:
                case umbraco.cms.businesslogic.datatype.DBTypes.Nvarchar:
                    t = typeof(string);
                    break;
                default:
                    t = typeof(object);
                    break;
            }

            return t;
        }
    }
}
