using System.Net;

namespace Catalogo.Application.Exceptions
{
    /// <summary>
    /// Data not found
    /// </summary>
    public class NotFoundException : CatalogoException
    {
        public override int StatusCode => (int)HttpStatusCode.NotFound;
    }
}