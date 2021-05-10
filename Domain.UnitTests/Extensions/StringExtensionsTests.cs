using Domain.Extensions;
using FluentAssertions;
using System;
using Xunit;

namespace Domain.UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        public class IsValidEmail
        {
            [Fact]
            public void ValidEmail_Should_ReturnTrue()
            {
                // Arrange
                string email = "teste@teste.com";

                // Act
                var result = email.IsValidEmail();

                // Assert
                result.Should().BeTrue();
            }

            [Fact]
            public void InvalidEmail_Should_ReturnFalse()
            {
                // Arrange
                string email_1 = "@teste.com";
                string email_2 = "teste.com";

                // Act
                var result_1 = email_1.IsValidEmail();
                var result_2 = email_2.IsValidEmail();

                // Assert
                result_1.Should().BeFalse();
                result_2.Should().BeFalse();
            }

            [Fact]
            public void EmptyEmail_Should_ReturnFalse()
            {
                // Arrange
                var email = string.Empty;

                // Act
                var result = email.IsValidEmail();

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void WhitespaceEmail_Should_ReturnFalse()
            {
                // Arrange
                var email = " ";

                // Act
                var result = email.IsValidEmail();

                // Assert
                result.Should().BeFalse();
            }

            [Fact]
            public void NullEmail_Should_ReturnFalse()
            {
                // Arrange
                string? email = null;

                // Act
                var result = email.IsValidEmail();

                // Assert
                result.Should().BeFalse();
            }

        }
    }
}
