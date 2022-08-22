namespace Catalogo.Domain.Validation
{
    public interface IValidationEntity
    {
        /// <summary>
        /// Execute to validate
        /// </summary>
        /// <exception cref="DomainExceptionValidation"/>
        void Validate();
    }
}
