// -----------------------------------------------------
// Copyright (c)  Christo du Toit - All rights reserved.
// -----------------------------------------------------

using System;
using System.Linq.Expressions;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using Xeptions;
using Xunit.Abstractions;

namespace Valid8R
{
    public static class Valid8
    {
        /// <summary>
        /// Compares two exceptions and writes a failure message to the test explorer if they are not the same.
        /// </summary>
        /// <param name="expectedException">The expected exception</param>
        /// <param name="testOutputHelper">The helper to write the failure message to the test explorer</param>
        /// <param name="reference">The reference to use that will link the failure message to your code</param>
        /// <returns>An expression to compare the actual exception with the expected exception</returns>
        public static Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException, ITestOutputHelper testOutputHelper, string reference = "") =>
        actualException => IsSameExceptionAs(actualException, expectedException, testOutputHelper, reference);

        private static bool IsSameExceptionAs(
            Exception actualException,
            Exception expectedException,
            ITestOutputHelper output,
            string prefix = "")
        {
            var result = actualException.SameExceptionAs(expectedException, out string message);

            if (!string.IsNullOrWhiteSpace(message))
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    output.WriteLine($"{prefix}: {message}");
                }
                else
                {
                    output.WriteLine(message);
                }
            }

            return result;
        }

        /// <summary>
        /// Compares two objects and writes a failure message to the test explorer if they are not the same.
        /// </summary>
        /// <param name="expectedObject">The expected object</param>
        /// <param name="testOutputHelper">The helper to write the failure message to the test explorer</param>
        /// <param name="reference">The reference to use that will link the failure message to your code</param>
        /// <returns>An expression to compare the actual object with the expected object</returns>
        public static Expression<Func<T, bool>> SameObjectAs<T>(
            object expectedObject, ITestOutputHelper testOutputHelper, string reference = "") =>
                actualObject => IsSameObjectAs(actualObject, expectedObject, testOutputHelper, reference);

        private static bool IsSameObjectAs(
            object actualObject,
            object expectedObject,
            ITestOutputHelper output,
            string reference = "")
        {
            var compareLogic = new CompareLogic();
            var result = compareLogic.Compare(expectedObject, actualObject).AreEqual;

            if (result == false)
            {
                try
                {
                    actualObject.Should().BeEquivalentTo(expectedObject);
                }
                catch (Exception exception)
                {
                    if (!string.IsNullOrEmpty(reference))
                    {
                        output.WriteLine(
                            $"{reference}: {exception.Message}.  See the stack details for more information.");
                    }
                    else
                    {
                        output.WriteLine($"{exception.Message}.  See the stack details for more information.");
                    }
                }
            }

            return result;
        }
    }
}
