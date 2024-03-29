﻿using Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using Codeflix.Catalog.UnitTest.Application.CreateCategory;

namespace Codeflix.Catalog.UnitTest.Application.UpdateCategory;
public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for(int index =0; index < times; index++)
        {
            var exampleCategory = fixture.GetExampleCategory();
            var exampleInput = fixture.GetValidInput(exampleCategory.Id);

            yield return new object[] { 
                exampleCategory,
                exampleInput
            };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 23)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < times; index++)
        {
            switch (index / totalInvalidCases)
            {
                case 0:

                    #region Name não pode ser menor que 3 caracteres

                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidInputShortName(),
                        $"Name should be at leats 3 characters long"
                    });
                    #endregion

                    break;

                case 1:

                    #region Name não pode ser maior do que 255 caracteres

                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidInputTooLongName(),
                        $"Name should be less or equal 255 characters long"
                    });

                    #endregion

                    break;

                case 2:

                    #region Description não pode maior que 10.000 caracteres

                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidInputTooLongDescription(),
                        $"Description should be less or equal 10000 characters long"
                    });
                    #endregion

                    break;

                default:
                    break;
            }
        }

        return invalidInputList;
    }
}
