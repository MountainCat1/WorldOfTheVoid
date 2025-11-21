using WorldOfTheVoid.Domain;

namespace WorldOfTheVoid.Utilities;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

public sealed class EntityIdModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(EntityId))
            return new BinderTypeModelBinder(typeof(EntityIdModelBinder));

        return null;
    }
}
