using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Equinox.Models
{
    public class AgeRangeAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int _minAge;
        private readonly int _maxAge;

        public AgeRangeAttribute(int minAge, int maxAge)
        {
            _minAge = minAge;
            _maxAge = maxAge;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dob)
            {
                int age = DateTime.Today.Year - dob.Year;
                if (dob > DateTime.Today.AddYears(-age)) age--;

                if (age > _minAge && age < _maxAge)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            }

            return new ValidationResult("Invalid date format.");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (!context.Attributes.ContainsKey("data-val"))
                context.Attributes.Add("data-val", "true");

            context.Attributes.Add("data-val-agerange", GetErrorMessage(context.ModelMetadata.DisplayName ?? context.ModelMetadata.Name ?? "Date"));
            context.Attributes.Add("data-val-agerange-min", _minAge.ToString());
            context.Attributes.Add("data-val-agerange-max", _maxAge.ToString());
        }

        private string GetErrorMessage(string fieldName) =>
            ErrorMessage ?? $"{fieldName} must result in an age between {_minAge + 1} and {_maxAge - 1}.";
    }
}
