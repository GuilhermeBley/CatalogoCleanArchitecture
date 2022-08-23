using System;
using System.Runtime.Serialization;

namespace Catalogo.Application.Exceptions
{
    /// <summary>
    /// Catalogo exception base
    /// </summary>
    public abstract class CatalogoException : Exception
    {
        /// <summary>
        /// Code identifier exception
        /// </summary>
        /// <Remarks>
        ///     <para>should be between codes <b>4XX</b></para>
        /// </Remarks>
        public abstract int StatusCode { get; }

        protected CatalogoException()
        {
        }

        protected CatalogoException(string message) : base(message)
        {
        }

        protected CatalogoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected CatalogoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}