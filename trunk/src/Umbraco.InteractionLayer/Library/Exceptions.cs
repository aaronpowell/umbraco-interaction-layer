using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbraco.InteractionLayer.Library
{
    [global::System.Serializable]
    public class DocTypeMissMatchException : Exception
    {
        public DocTypeMissMatchException(int docTypeId, int expcectedDocTypeId) : this(docTypeId, expcectedDocTypeId, string.Empty) { }
        public DocTypeMissMatchException(int docTypeId, int expcectedDocTypeId, string message)
            : base(string.Format("DocTypeID provided did not match what was expected (provided: {0}, expected: {1}){2}{3}", docTypeId, expcectedDocTypeId, Environment.NewLine, message))
        {
            ExpectedDocTypeId = expcectedDocTypeId;
            ActualDocTypeId = docTypeId;
        }
        public DocTypeMissMatchException(int docTypeId, int expcectedDocTypeId, string message, Exception inner)
            : base(string.Format("DocTypeID provided did not match what was expected (provided: {0}, expected: {1}){2}{3}", docTypeId, expcectedDocTypeId, Environment.NewLine, message), inner)
        {
            ExpectedDocTypeId = expcectedDocTypeId;
            ActualDocTypeId = docTypeId;
        }
        protected DocTypeMissMatchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public int ExpectedDocTypeId { get; set; }
        public int ActualDocTypeId { get; set; }
    }

    [global::System.Serializable]
    public class MandatoryFailureException : Exception
    {
        public MandatoryFailureException(string propertyName) : this(propertyName, string.Empty) { }
        public MandatoryFailureException(string propertyName, string message)
            : base(string.Format("The mandatory property \"{0}\" did not have a value.{1}{2}", propertyName, Environment.NewLine, message))
        {
            PropertyName = propertyName;
        }
        public MandatoryFailureException(string propertyName, string message, Exception inner)
            : base(string.Format("The mandatory property \"{0}\" did not have a value.{1}{2}", propertyName, Environment.NewLine, message), inner)
        {
            PropertyName = propertyName;
        }
        protected MandatoryFailureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string PropertyName { get; set; }
    }
}
