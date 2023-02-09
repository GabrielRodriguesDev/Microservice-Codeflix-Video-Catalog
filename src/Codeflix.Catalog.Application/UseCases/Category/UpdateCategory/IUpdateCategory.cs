using Codeflix.Catalog.Application.UseCases.Category.Common;
using Codeflix.Catalog.Application.UseCases.Category.GetCategory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
internal interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
{
}
