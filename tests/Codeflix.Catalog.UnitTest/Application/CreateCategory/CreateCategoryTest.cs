using Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace Codeflix.Catalog.UnitTest.Application.CreateCategory;

/* Utilizando o a biblioteca Moq para gerar os Mocks */

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest : IClassFixture<CreateCategoryTestFixture>
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
        => this._fixture = fixture;

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {

        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var repositoryMock = _fixture.GetRepositoryMock();

        var useCase = new UseCase.CreateCategory(unitOfWorkMock.Object, repositoryMock.Object);

        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        /* 
         * Verify() -> Verificando se o método Insert foi de fato chamado.
         * It.IsAny<T> -> Conferindo se os parametros que foram passados foram "Categoria" e "CancellationToken"
         * Times.Once -> Se só foi chamado uma vez.
         */

        repositoryMock.Verify(
            respository => respository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        /*
         * Teste abaixo segue a mesma idéia do teste de cima.
         */

        unitOfWorkMock.Verify(
            uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
    }

    [Theory(DisplayName = nameof(ThrowWhenCanInstantiateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(
        nameof(CreateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 23, //Parametro do método GetInvalidInputs
        MemberType = typeof(CreateCategoryTestDataGenerator) // Definfinido o tipo do MemberData (Classe da onde ele está sendo chamado).
    )]
    public async void ThrowWhenCanInstantiateCategory(CreateCategoryInput input, string exceptionMessage)
    {

        var useCase = new UseCase.CreateCategory(_fixture.GetUnitOfWorkMock().Object, _fixture.GetRepositoryMock().Object);

        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);

    }
}
