using Codeflix.Catalog.UnitTest.Common;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
using Xunit;
using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Moq;
using Codeflix.Catalog.Domain.Repository;
using Codeflix.Catalog.Application.Interfaces;

namespace Codeflix.Catalog.UnitTest.Application.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : BaseFixture
{
	public CreateCategoryTestFixture() : base() {}

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

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new Mock<IUnitOfWork>();

    public Mock<ICategoryRepository> GetRepositoryMock()
        => new Mock<ICategoryRepository>();
}


