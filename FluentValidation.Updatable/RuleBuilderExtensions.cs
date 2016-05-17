namespace FluentValidation.Updatable
{
    using FluentValidation.Internal;

    /// <summary>
    /// Extensions methods for the <see cref="RuleBuilder{T,TProperty}"/> class.
    /// </summary>
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Sets up a <see cref="PropertyValidator"/> that forbids a change in the property.
        /// </summary>
        /// The type of the object being validated.
        /// </typeparam>
        /// <typeparam name="TProperty">
        /// The type of the property being validated.
        /// </typeparam>
        /// <param name="ruleBuilder">
        /// The rule builder on which the validator should be defined.
        /// </param>
        /// <returns>
        /// The <see cref="IRuleBuilderOptions{T, TProperty}"/>.
        /// </returns>
        public static IRuleBuilderOptions<T, TProperty> NotUpdatable<T, TProperty>(this RuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.Must((item, propertyValue, propertyValidationContext) =>
            {
                var originalContext = propertyValidationContext.ParentContext as ValidationContextWithComparison<T>;

                if (originalContext == null)
                {
                    return true;
                }

                var originalValue = propertyValidationContext.Rule.PropertyFunc(originalContext.InstanceToValidate);
                var modifiedValue = propertyValidationContext.Rule.PropertyFunc(originalContext.ToCompare);

                return originalValue.Equals(modifiedValue);

            }).WithMessage($"The property '{ruleBuilder.Rule.PropertyName}' cannot be changed.");
        }
    }
}