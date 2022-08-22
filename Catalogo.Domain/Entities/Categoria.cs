using Catalogo.Domain.Validation;
using System.Collections.Generic;

namespace Catalogo.Domain.Entities
{
    public sealed class Categoria : Entity
    {
        public string Nome { get; private set; }
        public string ImagemUrl { get; private set; }
        public ICollection<Produto> Produtos { get; set; }

        public override void Validate()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Nome),
                "Nome inválido. O nome é obrigatório");

            DomainExceptionValidation.When(string.IsNullOrEmpty(ImagemUrl),
                "Nome da imagem inválido. O nome é obrigatório");

            DomainExceptionValidation.When(Nome.Length < 3,
               "O nome deve ter no mínimo 3 caracteres");

            DomainExceptionValidation.When(ImagemUrl.Length < 5,
                "Nome da imagem deve ter no mínimo 5 caracteres");
        }
    }
}
