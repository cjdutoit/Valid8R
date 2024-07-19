// -----------------------------------------------------
// Copyright (c)  Christo du Toit - All rights reserved.
// -----------------------------------------------------

using System;
using Moq;
using Xunit;

namespace Valid8R.Tests.Unit
{
    public partial class Valid8Tests
    {
        [Fact]
        public void SameExceptionAsMatchingExceptionsShouldReturnTrue()
        {
            // Arrange
            string randomMessage = GetRandomString();
            var expectedException = new InvalidOperationException(message: randomMessage);
            var actualException = new InvalidOperationException(message: randomMessage);
            string randomReference = GetRandomString();

            // Act
            var expression = Valid8.SameExceptionAs(
                expectedException,
                testOutputHelper: testOutputHelperMock.Object,
                reference: randomReference);

            var result = expression.Compile().Invoke(actualException);


            // Assert
            Assert.True(result);
            testOutputHelperMock.Verify(
                helper => helper.WriteLine(It.IsAny<string>()),
                Times.Never,
                "Expected no output when exceptions match"
            );
        }

        [Fact]
        public void SameExceptionAsNonMatchingExceptionsShouldReturnFalse()
        {
            // Arrange
            string expectedMessage = GetRandomString();
            string actualMessage = GetRandomString();
            var expectedException = new InvalidOperationException(message: expectedMessage);
            var actualException = new InvalidOperationException(message: actualMessage);
            string randomReference = GetRandomString();

            // Act
            var expression = Valid8.SameExceptionAs(
                expectedException,
                testOutputHelper: testOutputHelperMock.Object,
                reference: randomReference);

            var result = expression.Compile().Invoke(actualException);

            // Assert
            Assert.False(result);
            testOutputHelperMock.Verify(
                helper => helper.WriteLine(It.IsAny<string>()),
                Times.Once,
                "Expected a message to be written to the output when exceptions do not match."
            );
        }
    }
}
