using Catalogo.Domain.Validation;
using System;

namespace Catalogo.Domain.Entities
{
    public sealed class Produto : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; }
        public int Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public int IdCategoria { get; set; }
        public Categoria Categoria { get; set; }
        
        public override void Validate()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Nome),
                "Nome inválido. O nome é obrigatório");

            DomainExceptionValidation.When(Nome.Length < 3,
                "O nome deve ter no mínimo 3 caracteres");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Descricao),
                "Descrição inválida. A descrição é obrigatória");

            DomainExceptionValidation.When(Descricao.Length < 5,
                "A descrição deve ter no mínimo 5 caracteres");

            DomainExceptionValidation.When(Preco < 0, "Valor do preço inválido");

            DomainExceptionValidation.When(ImagemUrl is null,
                "Necessário uma imagem para o produto.");
            
            DomainExceptionValidation.When(ImagemUrl?.Length > 250,
                "O nome da imagem não pode exceder 250 caracteres");

            DomainExceptionValidation.When(ImagemUrl?.Length > 250,
                "O nome da imagem não pode exceder 250 caracteres");

            DomainExceptionValidation.When(Estoque < 0, "Estoque inválido");
        }
    }
}
