using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SeamsApp.Utilities
{
    public static class Extensions
    {

        public static ModelStateDictionary ToModelStateDictionary(this ValidationResult validationResult)
        {
            var modelState = new ModelStateDictionary();

            foreach (var error in validationResult.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return modelState;
        }

    }
}
