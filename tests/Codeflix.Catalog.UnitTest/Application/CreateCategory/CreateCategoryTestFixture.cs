using Codeflix.Catalog.Application.Interfaces;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Repository;
using Codeflix.Catalog.UnitTest.Common;
using Moq;
using Xunit;

namespace Codeflix.Catalog.UnitTest.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : BaseFixture
{
    public CreateCategoryTestFixture() : base() { }

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

    public CreateCategoryInput GetInput()
        => new CreateCategoryInput(
                    GetValidCategoryName(),
                    GetValidCategoryDescription(),
                    GetRandomBoolean()
               );

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidIpuntShortName = GetInput();
        invalidIpuntShortName.Name = invalidIpuntShortName.Name.Substring(0, 2);

       return invalidIpuntShortName;
    }

    public CreateCategoryInput GetInvalidInputTooLongName()
    {
        var invalidIpuntTooLongName = GetInput();

        while (invalidIpuntTooLongName.Name.Length <= 255)
            invalidIpuntTooLongName.Name = $"{invalidIpuntTooLongName.Name} {Faker.Commerce.ProductName()}";

        return invalidIpuntTooLongName;
    }

    public CreateCategoryInput GetInvalidInputNameNull()
    {
        var invalidInputNameNull = GetInput();
        invalidInputNameNull.Name = null!;

        return invalidInputNameNull;
    }

    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var invalidInputDescriptionNull = GetInput();
        invalidInputDescriptionNull.Description = null!;

        return invalidInputDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidIpuntTooLongDescription = GetInput();

        while (invalidIpuntTooLongDescription.Description.Length <= 10000)
            invalidIpuntTooLongDescription.Description = $"{invalidIpuntTooLongDescription.Description} {Faker.Commerce.ProductDescription()}";

        return invalidIpuntTooLongDescription;
    }


    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new Mock<IUnitOfWork>();

    public Mock<ICategoryRepository> GetRepositoryMock()
        => new Mock<ICategoryRepository>();
}


