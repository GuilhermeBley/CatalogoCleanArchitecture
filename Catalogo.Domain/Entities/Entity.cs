using Catalogo.Domain.Validation;

namespace Catalogo.Domain.Entities
{
    public abstract class Entity : IValidationEntity
    {
        public int Id { get; set; }

        public abstract void Validate();
    }
}
