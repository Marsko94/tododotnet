namespace RecipeManagement.Domain.Recipes.Features;

using RecipeManagement.Domain.Recipes.Services;
using RecipeManagement.Services;
using SharedKernel.Exceptions;
using MediatR;

public static class DeleteRecipe
{
    public sealed record Command(Guid Id) : IRequest;

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
            var recordToDelete = await _recipeRepository.GetById(request.Id, cancellationToken: cancellationToken);
            _recipeRepository.Remove(recordToDelete);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}