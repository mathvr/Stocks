using STOCKS.Models.Helpers;

namespace stocks.Services.Helpers;

public static class ModelValidator
{
    public static ValidateObjectResponse ValidateObject<T>(T? model) where T : class
    {
        if (model == null) return new ValidateObjectResponse{ IsValid = false, Message = "Object cannot be null." };
        
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            if (property.GetValue(model) == null)
            {
                return new ValidateObjectResponse{ IsValid = false, Message = $"{property.Name} cannot be null." };
            }
        }
        
        return new ValidateObjectResponse{ IsValid = true, Message = $"{model.GetType().Name}." };
    }
}