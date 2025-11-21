using WorldOfTheVoid.Domain;

namespace WorldOfTheVoid.Utilities;

using Microsoft.AspNetCore.Mvc.ModelBinding;

public sealed class EntityIdModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var value = context.ValueProvider.GetValue(context.ModelName).FirstValue;

        if (string.IsNullOrWhiteSpace(value))
        {
            context.ModelState.TryAddModelError(context.ModelName, "EntityId is required.");
            return Task.CompletedTask;
        }

        try
        {
            var id = new EntityId(value);
            context.Result = ModelBindingResult.Success(id);
        }
        catch (Exception ex)
        {
            context.ModelState.TryAddModelError(context.ModelName, ex.Message);
        }

        return Task.CompletedTask;
    }
}
