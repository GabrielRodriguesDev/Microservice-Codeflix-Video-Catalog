﻿using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;
using DomainEntity = Codeflix.Catalog.Domain.Entity;
 
namespace Codeflix.Catalog.UnitTest.Domain.Entity.Category
{
    /* Utilizando Package FluentAssertion para os testes (só para ajudar na legibilidade, não muda o resultado).*/
    /* Utilizando o Fixture (do xunit) para compartilhar informações entre os testes e entre as classes */
    /* Utilizando o Bogus para gerar os dados randomicos (utilizamos ele pois permite gerar dados em pt_BR) -> Criamos a instancia na classe BaseFixture */

    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest : IClassFixture<CategoryTestFixture>
    {
        private readonly CategoryTestFixture _categoryTestFixture;

        public CategoryTest(CategoryTestFixture categoryTestFixture) => _categoryTestFixture = categoryTestFixture;

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {

            var validCategory = _categoryTestFixture.GetValidCategory();

            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            (category.IsActive).Should().BeTrue(); 
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var datetimeBefore = DateTime.Now;

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
            var datetimeAfter = DateTime.Now.AddSeconds(1);

            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt > datetimeBefore).Should().BeTrue();
            (category.CreatedAt < datetimeAfter).Should().BeTrue();
            (category.IsActive).Should().Be(isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            Action action = () => new DomainEntity.Category(name!, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be null or empty");
            
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            Action action = () => new DomainEntity.Category(validCategory.Name, null!);
            action.Should().Throw<EntityValidationException>().WithMessage("Description should not be null");

        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 12)]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");
        }

        public static IEnumerable<Object[]> GetNamesWithLessThan3Characters(int numberOfTest = 6)
        {
            var fixture = new CategoryTestFixture();
            for (int i = 0; i < numberOfTest; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] { fixture.GetValidCategoryName().Substring(0, isOdd is true ? 1 : 2) };
            }
            //Classe estatica usada como fornecedor de dados através de uma MemberData para fornecer informações para um teste de tipo theory
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(x => "a").ToArray());
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(x => "a").ToArray());
            Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription!);
            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10000 characters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);

            category.Activate();

            (category.IsActive).Should().BeTrue();
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            var validCategory = _categoryTestFixture.GetValidCategory();

            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

            category.Deactivate();

            (category.IsActive).Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            var category = _categoryTestFixture.GetValidCategory();
            var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

            category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

            category.Name.Should().Be(categoryWithNewValues.Name);
            category.Description.Should().Be(categoryWithNewValues.Description);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            var category = _categoryTestFixture.GetValidCategory();
            var newName = _categoryTestFixture.GetValidCategoryName();
            var currentDescription = category.Description;

            category.Update(newName);

            Assert.Equal(category.Name, newName);
            Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            var category = _categoryTestFixture.GetValidCategory();
            Action action = () => category.Update(name!);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be null or empty");
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("a")]
        [InlineData("ab")]
        public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            var category = _categoryTestFixture.GetValidCategory();
            Action action = () => category.Update(invalidName);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should be at leats 3 characters long");

        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            var category = _categoryTestFixture.GetValidCategory();

            var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
            Action action = () => category.Update(invalidName);

            action.Should().Throw<EntityValidationException>().WithMessage("Name should be less or equal 255 characters long");
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            var category = _categoryTestFixture.GetValidCategory();

            var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
            while (invalidDescription.Length <= 10000)
                invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

            Action action = () => category.Update(_categoryTestFixture.GetValidCategoryName(), invalidDescription); ;

            action.Should().Throw<EntityValidationException>().WithMessage("Description should be less or equal 10000 characters long");
        } 
    }
}
