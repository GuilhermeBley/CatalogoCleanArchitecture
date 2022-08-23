using System;
using System.Net;
using System.Runtime.Serialization;

namespace Catalogo.Application.Exceptions
{
    /// <summary>
    /// Data not found
    /// </summary>
    public class NotFoundException : CatalogoException
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;

        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}