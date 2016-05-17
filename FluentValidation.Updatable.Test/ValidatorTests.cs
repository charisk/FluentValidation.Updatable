namespace FluentValidation.Updatable.Test
{
    using NUnit.Framework;
    using System.Threading;

    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void Validate_WhenModelValid_ShouldPassValidation()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = 10 };

            var result = validator.Validate(model);
            Assert.That(result.IsValid);
        }

        [Test]
        public void Validate_WhenModelInvalid_ShouldFailValidation()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = -1 };

            var result = validator.Validate(model);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public async void ValidateAsync_WhenModelValid_ShouldPassValidation()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = 10 };

            var result = await validator.ValidateAsync(model);
            Assert.That(result.IsValid);
        }

        [Test]
        public async void ValidateAsync_WhenModelInvalid_ShouldFailValidation()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = -1 };

            var result = await validator.ValidateAsync(model);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void ValidateUpdates_WhenModelValid_ShouldPassValidation()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 11 };

            var result = validator.ValidateUpdates(original, modified);
            Assert.That(result.IsValid);
        }

        [Test]
        public void ValidateUpdates_WhenChangeIsInvalidBasedOnExplicitRule_ShouldFailValidation()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 12 };

            var result = validator.ValidateUpdates(original, modified);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void ValidateUpdates_WhenChangeIsInvalidBasedOnNotUpdatableRule_ShouldFailValidation()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bobby", Age = 10 };

            var result = validator.ValidateUpdates(original, modified);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public async void ValidateUpdatesAsync_WhenModelValid_ShouldPassValidation()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 11 };

            var result = await validator.ValidateUpdatesAsync(original, modified, CancellationToken.None);
            Assert.That(result.IsValid);
        }

        [Test]
        public async void ValidateUpdatesAsync_WhenChangeIsInvalid_ShouldFailValidation()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 12 };

            var result = await validator.ValidateUpdatesAsync(original, modified, CancellationToken.None);
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void ValidateAndThrow_WhenModelValid_ShouldNotThrowValidationException()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = 10 };

            Assert.DoesNotThrow(() => validator.ValidateAndThrow(model));
        }

        [Test]
        public void ValidateAndThrow_WhenModelInvalid_ShouldThrowValidationException()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = -1 };

            Assert.Throws<ValidationException>(() => validator.ValidateAndThrow(model));
        }

        [Test]
        public void ValidateAndThrowAsync_WhenModelValid_ShouldNotThrowValidationException()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = 10 };

            Assert.DoesNotThrow(async () => await validator.ValidateAndThrowAsync(model));
        }

        [Test]
        public void ValidateAndThrowAsync_WhenModelInvalid_ShouldThrowValidationException()
        {
            var validator = new Validator();
            var model = new Person { Name = "Bob", Age = -1 };

            Assert.Throws<ValidationException>(async () => await validator.ValidateAndThrowAsync(model));
        }

        [Test]
        public void ValidateUpdatesAndThrow_WhenModelValid_ShouldNotThrowValidationException()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 11 };

            Assert.DoesNotThrow(() => validator.ValidateUpdatesAndThrow(original, modified));
        }

        [Test]
        public void ValidateUpdatesAndThrow_WhenChangeIsInvalid_ShouldThrowValidationException()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 12 };

            Assert.Throws<ValidationException>(() => validator.ValidateUpdatesAndThrow(original, modified));
        }

        [Test]
        public void ValidateUpdatesAndThrowAsync_WhenModelValid_ShouldNotThrowValidationException()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 11 };

            Assert.DoesNotThrow(async () => await validator.ValidateUpdatesAndThrowAsync(original, modified));
        }

        [Test]
        public void ValidateUpdatesAndThrowAsync_WhenChangeIsInvalid_ShouldThrowValidationException()
        {
            var validator = new Validator();
            var original = new Person { Name = "Bob", Age = 10 };
            var modified = new Person { Name = "Bob", Age = 12 };

            Assert.Throws<ValidationException>(async () => await validator.ValidateUpdatesAndThrowAsync(original, modified));
        }

        private class Validator : ValidatorBase<Person>
        {
            public Validator()
            {
                this.RuleFor(p => p.Name).NotEmpty();
                this.RuleFor(p => p.Age).GreaterThanOrEqualTo(0);

                this.UpdateRuleFor(p => p.Name).NotUpdatable();
                this.UpdateRuleFor(p => p.Age, (current, modified) => current.Age + 1 == modified.Age);
            }
        } 

        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}