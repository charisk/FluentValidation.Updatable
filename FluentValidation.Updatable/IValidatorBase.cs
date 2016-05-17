namespace FluentValidation.Updatable
{
    using Internal;
    using Results;
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a validator for a particular type but extends the standard <see cref="IValidator{T}"/>
    /// by adding support for updates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidatorBase<T> : IValidator<T>
    {
        /// <summary>
        /// Validates an update from <paramref name="original"/> to <paramref name="modified"/>. This includes
        /// validating the modified object to make sure that it's still valid based on the creation validation rules.
        /// </summary>
        /// <param name="original">
        /// The original object.
        /// </param>
        /// <param name="modified">
        /// The modified object.
        /// </param>
        /// <returns>
        /// The <see cref="ValidationResult"/>.
        /// </returns>
        ValidationResult ValidateUpdates(T original, T modified);

        /// <summary>
        /// Validates an update from <paramref name="original"/> to <paramref name="modified"/>. This includes
        /// validating the modified object to make sure that it's still valid based on the creation validation rules.
        /// </summary>
        /// <param name="original">
        /// The original object.
        /// </param>
        /// <param name="modified">
        /// The modified object.
        /// </param>
        /// <param name="cancellation">
        /// The <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// The <see cref="ValidationResult"/>.
        /// </returns>
        Task<ValidationResult> ValidateUpdatesAsync(T original, T modified, CancellationToken cancellation = new CancellationToken());

        /// <summary>
        /// Validates an update from <paramref name="original"/> to <paramref name="modified"/> and throws a
        /// <see cref="ValidationException"/> if there are any <see cref="ValidationFailure"/>s.
        /// </summary>
        /// <param name="original">
        /// The original object.
        /// </param>
        /// <param name="modified">
        /// The modified object.
        /// </param>
        void ValidateUpdatesAndThrow(T original, T modified);

        /// <summary>
        /// Validates an update from <paramref name="original"/> to <paramref name="modified"/> and throws a
        /// <see cref="ValidationException"/> if there are any <see cref="ValidationFailure"/>s.
        /// </summary>
        /// <param name="original">
        /// The original object.
        /// </param>
        /// <param name="modified">
        /// The modified object.
        /// </param>
        /// <param name="cancellation">
        /// The <see cref="CancellationToken"/>.
        /// </param>
        Task ValidateUpdatesAndThrowAsync(T original, T modified, CancellationToken cancellation = new CancellationToken());

        /// <summary>
        /// Adds an update rule for a property, receiving a delegate to use to check for validity.
        /// </summary>
        /// <typeparam name="TProperty">
        /// The type of the property being updated.
        /// </typeparam>
        /// <param name="expression">
        /// The expression to use to retrieve the property.
        /// </param>
        /// <param name="validityCheck">
        /// The function to execute to check for the validity of the rule.
        /// </param>
        /// <returns>
        /// The <see cref="IRuleBuilderOptions{T, TProperty}"/>.
        /// </returns>
        IRuleBuilderOptions<T, TProperty> UpdateRuleFor<TProperty>(Expression<Func<T, TProperty>> expression, Func<T, T, bool> validityCheck);

        /// <summary>
        /// Adds an update rule for a property. 
        /// </summary>
        /// <typeparam name="TProperty">
        /// The type of the property being updated.
        /// </typeparam>
        /// <param name="expression">
        /// The expression to use to retrieve the property.
        /// </param>
        /// <returns>
        /// The <see cref="IRuleBuilderOptions{T, TProperty}"/>.
        /// </returns>
        RuleBuilder<T, TProperty> UpdateRuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
    }
}
