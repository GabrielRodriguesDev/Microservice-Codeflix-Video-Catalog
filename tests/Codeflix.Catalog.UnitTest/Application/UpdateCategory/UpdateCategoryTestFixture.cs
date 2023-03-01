using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Repository;
using Codeflix.Catalog.UnitTest.Application.GetCategory;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection :
    ICollectionFixture<UpdateCategoryTestFixture>
{ }
public class UpdateCategoryTestFixture : BaseFixture
{
    public Mock<IUnitOfWork> GetUnitOfWorkMock()
       => new Mock<IUnitOfWork>();

    public Mock<ICategoryRepository> GetRepositoryMock()
        => new Mock<ICategoryRepository>();

    public String GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName.Substring(0, 255);
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10000) categoryDescription = categoryDescription.Substring(0, 10000);
        return categoryDescription;
    }

    public bool GetRandomBoolean()
    {
        return (new Random()).NextDouble() < 0.5;
    }


    public Category GetExampleCategory()
        => new Category(
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
            );

    public UpdateCategoryInput GetValidInput(Guid? id = null)
        => new(
            id ?? Guid.NewGuid(),
            GetValidCategoryName(),
            GetValidCategoryDescription(),
            GetRandomBoolean()
            );

    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidIpuntShortName = GetValidInput();
        invalidIpuntShortName.Name = invalidIpuntShortName.Name.Substring(0, 2);

        return invalidIpuntShortName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidIpuntTooLongName = GetValidInput();

        while (invalidIpuntTooLongName.Name.Length <= 255)
            invalidIpuntTooLongName.Name = $"{invalidIpuntTooLongName.Name} {Faker.Commerce.ProductName()}";

        return invalidIpuntTooLongName;
    }

    public UpdateCategoryInput GetInvalidInputNameNull()
    {
        var invalidInputNameNull = GetValidInput();
        invalidInputNameNull.Name = null!;

        return invalidInputNameNull;
    }


    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidIpuntTooLongDescription = GetValidInput();

        while (invalidIpuntTooLongDescription.Description!.Length <= 10000)
            invalidIpuntTooLongDescription.Description = $"{invalidIpuntTooLongDescription.Description} {Faker.Commerce.ProductDescription()}";

        return invalidIpuntTooLongDescription;
    }
}
