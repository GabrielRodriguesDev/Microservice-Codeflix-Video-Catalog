﻿using Codeflix.Catalog.Application.UseCases.Category.Common;
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

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task UpdateCategory()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleCategory = _fixture.GetExampleCategory();

        repositoryMock.Setup(x => x.Get(
            exampleCategory.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategory);

        var input = new UseCase.UpdateCategoryInput(
            exampleCategory.Id,
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription(),
            !exampleCategory.IsActive
            );

        var useCase = new UseCase.UpdateCategory(
            unitOfWorkMock.Object,
            repositoryMock.Object
            );

        CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);

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

}
