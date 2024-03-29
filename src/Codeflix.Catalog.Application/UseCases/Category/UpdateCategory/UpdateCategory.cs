﻿using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Domain.Repository;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
public class UpdateCategory : IUpdateCategory
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategory(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) 
        => (_categoryRepository, _unitOfWork)
            = (categoryRepository, unitOfWork);

    public async Task<CategoryModelOutput> Handle(UpdateCategoryInput request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.Get(request.Id, cancellationToken);
        category.Update(request.Name, request.Description);

        if (
            request.IsActive is not null &&
            request.IsActive != category.IsActive
            )
            if (request.IsActive!.Value) category.Activate();
            else category.Deactivate();

        await _categoryRepository.Update(category, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);

    }
}
