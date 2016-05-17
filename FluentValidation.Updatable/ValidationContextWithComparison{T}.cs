namespace FluentValidation.Updatable
{
    using Internal;

    /// <summary>
    /// A <see cref="ValidationContext{T}"/> used for comparisons between entities.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the objects being validated.
    /// </typeparam>
    public class ValidationContextWithComparison<T> : ValidationContext<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContextWithComparison{T}"/> class.
        /// </summary>
        /// <param name="instanceToValidate">
        /// The instance of the class to validate.
        /// </param>
        public ValidationContextWithComparison(T instanceToValidate) : base(instanceToValidate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationContextWithComparison{T}"/> class.
        /// </summary>
        /// <param name="instanceToValidate">
        /// The instance of the class to validate.
        /// </param>
        /// <param name="propertyChain">
        /// The <see cref="PropertyChain"/>.
        /// </param>
        /// <param name="validatorSelector">
        /// The <see cref="IValidatorSelector"/>.
        /// </param>
        public ValidationContextWithComparison(T instanceToValidate, PropertyChain propertyChain, IValidatorSelector validatorSelector) : base(instanceToValidate, propertyChain, validatorSelector)
        {
        }

        /// <summary>
        /// Gets or sets the instance to compare
        /// </summary>
        public T ToCompare { get; set; }
    }
}