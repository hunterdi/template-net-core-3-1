using Business;
using FluentValidation;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Tests
{
    public class ValidatorsTest
    {
        private readonly TagInsertOrUpdateDTOValidator _tagInsertOrUpdateValidator;

        public ValidatorsTest()
        {
            this._tagInsertOrUpdateValidator = new TagInsertOrUpdateDTOValidator();
        }

        [Fact]
        public void Should_return_an_error_when_the_tag_name_has_not_been_defined()
        {
            var dto = new TagInsertOrUpdateDTO();

            _tagInsertOrUpdateValidator.ShouldHaveValidationErrorFor(v => v.Name, dto)
                .WithErrorMessage("Name is required.");
        }
    }
}
