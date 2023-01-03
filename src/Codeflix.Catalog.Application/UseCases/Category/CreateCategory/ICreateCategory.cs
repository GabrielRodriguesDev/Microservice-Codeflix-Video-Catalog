namespace Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory
{
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput createCategoryInput, CancellationToken cancellationToken);
}
