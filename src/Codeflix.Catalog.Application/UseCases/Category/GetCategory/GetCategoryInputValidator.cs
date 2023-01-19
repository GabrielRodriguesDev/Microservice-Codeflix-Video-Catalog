using FluentValidation;

namespace Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategoryInputValidator : AbstractValidator<GetCategoryInput> //Implementando o AbstractionValidator, para usar o Validate do FluentValidation
{
	public GetCategoryInputValidator()
	{
		RuleFor(x => x.Id).NotEmpty(); // Definindo regras de validação para o GetCategoryInput através do FluentValidation
	}
}
