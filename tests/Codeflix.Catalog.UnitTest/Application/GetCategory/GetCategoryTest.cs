using Moq;
using Xunit;

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

        var input = new GetCategoryInput(exampleCategory.Id);
        var useCase = new GetCategory(repositoryMock);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(exampleCategory.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        output.Id.Should().Be(exampleCategory.Id);
        output.CreatedAt.Should().Be(exampleCategory.CreatedAt);

    }
}
