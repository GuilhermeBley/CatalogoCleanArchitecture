using System;
using System.Net;
using System.Runtime.Serialization;

namespace Catalogo.Application.Exceptions
{
    /// <summary>
    /// Conflict on create or update
    /// </summary>
    public class ConflictException : CatalogoException
    {
        public override int StatusCode => (int)HttpStatusCode.Conflict;
        
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}