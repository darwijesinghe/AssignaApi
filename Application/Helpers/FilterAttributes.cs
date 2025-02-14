using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Helpers
{
    /// <summary>
    /// Helper class for the filter attributes.
    /// </summary>
    public class FilterAttributes
    {
        /// <summary>
        /// Date validation filter attribute
        /// </summary>
        public class FutureDateAttribute : ValidationAttribute
        {
            /// <summary>
            /// Overrides the base class method to implement custom validation logic.
            /// </summary>
            /// <param name="value">The value being validated, typically a user input.</param>
            /// <param name="validationContext">Provides context about the object being validated.</param>
            /// <returns>
            /// Returns a ValidationResult indicating success or failure of the validation. If validation 
            /// passes, returns ValidationResult.Success; otherwise, returns an error message.
            /// </returns>
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // converts object value to date
                var date = Convert.ToDateTime(value);

                // checks task date is future date or not
                if (date.Date < DateTime.Now.Date)
                {
                    return new ValidationResult(ErrorMessage);
                }

                // returns the result
                return ValidationResult.Success;
            }
        }
    }
}
