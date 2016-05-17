namespace FluentValidation.Updatable
{
    using Internal;
    using Results;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for all validators.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object being validated.
    /// </typeparam>
    public class ValidatorBase<T> : AbstractValidator<T>, IValidatorBase<T>
    {
        /// <inheritdoc />  
        public ValidationResult ValidateUpdates(T original, T modified)
        {
            var validationResult = this.Validate(new ValidationContext<T>(modified));

            var comparisonContext = new ValidationContextWithComparison<T>(original) { ToCompare = modified };
            var updateValidationResult = this.Validate(comparisonContext);

            return new ValidationResult(validationResult.Errors.Concat(updateValidationResult.Errors));
        }

        /// <inheritdoc />  
        public void ValidateUpdatesAndThrow(T original, T modified)
        {
            var validationResult = this.ValidateUpdates(original, modified);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        /// <inheritdoc />  
        public async Task<ValidationResult> ValidateUpdatesAsync(T original, T modified, CancellationToken cancellation = default(CancellationToken))
        {
            var validationResult = await this.ValidateAsync(new ValidationContext<T>(modified), cancellation);

            var comparisonContext = new ValidationContextWithComparison<T>(original) { ToCompare = modified };
            var updateValidationResult = await((AbstractValidator<T>)this).ValidateAsync(comparisonContext, cancellation);

            return new ValidationResult(validationResult.Errors.Concat(updateValidationResult.Errors));
        }

        /// <inheritdoc />  
        public async Task ValidateUpdatesAndThrowAsync(T original, T modified, CancellationToken cancellation = default(CancellationToken))
        {
            var validationResult = await this.ValidateUpdatesAsync(original, modified, cancellation);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        /// <inheritdoc />  
        public RuleBuilder<T, TProperty> UpdateRuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            // We need a RuleBuilder for chaining update rules
            return this.RuleFor(expression) as RuleBuilder<T, TProperty>;

        }

        /// <inheritdoc />  
        public IRuleBuilderOptions<T, TProperty> UpdateRuleFor<TProperty>(Expression<Func<T, TProperty>> expression, Func<T, T, bool> validityCheck)
        {
            return this.RuleFor(expression).Must((item, propertyValue, propertyValidationContext) => {
                var originalContext = propertyValidationContext.ParentContext as ValidationContextWithComparison<T>;
                return originalContext == null || validityCheck(originalContext.InstanceToValidate, originalContext.ToCompare);
            });
        }
    }
}