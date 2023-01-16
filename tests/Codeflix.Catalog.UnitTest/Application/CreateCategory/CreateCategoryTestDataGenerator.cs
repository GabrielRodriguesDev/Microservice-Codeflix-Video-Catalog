namespace Codeflix.Catalog.UnitTest.Application.CreateCategory;
public class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs( int times = 23)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 5;

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

                    #region Name não pode ser null

                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidInputNameNull(),
                        $"Name should not be null or empty"
                    });

                    #endregion
                    
                    break;
                
                case 3:

                    #region Description não pode ser null

                    invalidInputList.Add(new object[]
                    {
                        fixture.GetInvalidInputDescriptionNull(),
                        $"Description should not be null"
                    });

                    #endregion

                    break;
                
                case 4:

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
