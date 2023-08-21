namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes;
using RecipeManagement.Domain.Recipes.Dtos;
using RecipeManagement.Domain.Recipes.Services;
using RecipeManagement.Services;
using RecipeManagement.Domain.Recipes.Models;
using SharedKernel.Exceptions;
using Mappings;
using MediatR;

public static class UpdateRecipe
{
    public sealed record Command(Guid Id, RecipeForUpdateDto UpdatedRecipeData) : IRequest;

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IRecipeRepository recipeRepository, IUnitOfWork unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var recipeToUpdate = await _recipeRepository.GetById(request.Id, cancellationToken: cancellationToken);
            var recipeToAdd = request.UpdatedRecipeData.ToRecipeForUpdate();
            recipeToUpdate.Update(recipeToAdd);

            _recipeRepository.Update(recipeToUpdate);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}