using Codeflix.Catalog.Application.Exceptions;
using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.Domain.Entity;
using Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using UseCase = Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace Codeflix.Catalog.UnitTest.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture) => _fixture = fixture;

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
    public async Task UpdateCategory(Category exampleCategory, UpdateCategoryInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);


        var useCase = new UseCase.UpdateCategory(
            unitOfWorkMock.Object,
            repositoryMock.Object
            );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);

        repositoryMock.Verify(repository => repository.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );

        repositoryMock.Verify(repository => repository.Update(
                exampleCategory,
                It.IsAny<CancellationToken>()
            ), 
                Times.Once
        );

        unitOfWorkMock.Verify(uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );


    }


    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();

        repositoryMock.Setup(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"category '{input.Id}' not found."));

        var useCase = new UseCase.UpdateCategory(
            unitOfWorkMock.Object,
            repositoryMock.Object
            );



        var task = async () => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();

        repositoryMock.Verify(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }


    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
    public async Task UpdateCategoryWithoutProvidingIsActive(Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name,
                exampleInput.Description
            );

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);


        var useCase = new UseCase.UpdateCategory(
            unitOfWorkMock.Object,
            repositoryMock.Object
            );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        repositoryMock.Verify(repository => repository.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );

        repositoryMock.Verify(repository => repository.Update(
                exampleCategory,
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );

        unitOfWorkMock.Verify(uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );


    }


    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
        parameters: 10,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
    public async Task UpdateCategoryOnlyName(Category exampleCategory, UpdateCategoryInput exampleInput)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = new UpdateCategoryInput(
                exampleInput.Id,
                exampleInput.Name
            );

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);


        var useCase = new UseCase.UpdateCategory(
            unitOfWorkMock.Object,
            repositoryMock.Object
            );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be((bool)exampleCategory.IsActive!);

        repositoryMock.Verify(repository => repository.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );

        repositoryMock.Verify(repository => repository.Update(
                exampleCategory,
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );

        unitOfWorkMock.Verify(uow => uow.Commit(
                It.IsAny<CancellationToken>()
            ),
                Times.Once
        );


    }


    //-------------------------------------------------------------------------------------------------//
    //-------------------------------------------------------------------------------------------------//
    //-------------------------------------------------------------------------------------------------//
    //-------------------------------------------------------------------------------------------------//
    //-------------------------------------------------------------------------------------------------//

    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(
        nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
    public async Task ThrowWhenCantUpdateCategory(UpdateCategoryInput input, string expectedExceptionMessage)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleCategory = _fixture.GetExampleCategory();
        input.Id = exampleCategory.Id;

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);


        var useCase = new UseCase.UpdateCategory(
            unitOfWorkMock.Object,
            repositoryMock.Object
            );

        var task = async () 
            => await useCase.Handle(input, CancellationToken.None);

         await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);

        repositoryMock.Verify(repository => repository.Get(
               exampleCategory.Id,
               It.IsAny<CancellationToken>()
           ),
               Times.Once
       );
    }



}
