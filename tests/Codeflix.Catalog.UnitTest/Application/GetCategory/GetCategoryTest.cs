using Codeflix.Catalog.Application.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using UseCases = Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace Codeflix.Catalog.UnitTest.Application.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]

public class GetCategoryTest
{
    private readonly GetCategoryTestFixture _fixture;

    public GetCategoryTest(GetCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(GetCategory))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task GetCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleCategory = _fixture.GetValidCategory();
        /*
         * Teste abaixo
         * Monta a execução do método 
         * Sempre que eu chamar um Get e passando um Guid e um CancelletionToken como parametro
         * Ele vai me retornar um exampleCategory (uma categorya valida).
         */
        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);


        var input = new UseCases.GetCategoryInput(exampleCategory.Id);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        /*
         * Teste abaixo
         * Valida se o método Get foi chamado
         * Passando dois parametros sendo eles o primeiro do tipo Guid e o segundo do tipo CancellationToken
         * Chamado pelo menos 1 vez
         */

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);



        output.Should().NotBeNull();
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Id.Should().Be(exampleCategory.Id);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }


    [Fact(DisplayName = nameof(NotFoundWhenCategoryDoesntExist))]
    [Trait("Application", "GetCategory - Use Cases")]
    public async Task NotFoundWhenCategoryDoesntExist()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var exampleGuid = Guid.NewGuid();

        /*
         * Teste abaixo
         * Monta a execução do método 
         * Sempre que eu chamar um Get e passando um Guid e um CancelletionToken como parametro
         * Ele vai me retornar um NotFoundException
         */

        repositoryMock.Setup(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            )).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found."));


        var input = new UseCases.GetCategoryInput(exampleGuid);
        var useCase = new UseCases.GetCategory(repositoryMock.Object);
        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()
            ), Times.Once);

    }

}
