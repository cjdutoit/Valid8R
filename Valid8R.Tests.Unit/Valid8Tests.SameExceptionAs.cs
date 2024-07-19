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
            var expectedException = new InvalidOperationException("Test message");
            var actualException = new InvalidOperationException("Test message");
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
    }
}
