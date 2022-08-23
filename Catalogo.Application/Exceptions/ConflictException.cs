using System.Net;

namespace Catalogo.Application.Exceptions
{
    /// <summary>
    /// Conflict on create or update
    /// </summary>
    public class ConflictException : CatalogoException
    {
        public override int StatusCode => (int)HttpStatusCode.Conflict;
    }
}