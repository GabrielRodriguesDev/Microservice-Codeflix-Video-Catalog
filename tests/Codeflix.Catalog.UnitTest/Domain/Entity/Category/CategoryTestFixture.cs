using Codeflix.Catalog.UnitTest.Common;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
namespace Codeflix.Catalog.UnitTest.Domain.Entity.Category;
public class CategoryTestFixture : BaseFixture
{
    public CategoryTestFixture() : base() { }

    public String GetValidCategoryName()
    {
        var categoryName = "";
        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName.Substring(0,255);
        return categoryName; 
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();
        if (categoryDescription.Length > 10000) categoryDescription = categoryDescription.Substring(0, 10000);
        return categoryDescription;
    }

    public DomainEntity.Category GetValidCategory() => new(GetValidCategoryName(), GetValidCategoryDescription());
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture> { }
