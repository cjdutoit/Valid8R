// -----------------------------------------------------
// Copyright (c)  Christo du Toit - All rights reserved.
// -----------------------------------------------------

using System;
using System.Linq.Expressions;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using Xunit.Abstractions;

namespace Valid8R
{
    public static class Valid8
    {
        /// <summary>
        /// Compares two objects and writes a failure message to the test explorer if they are not the same.
        /// </summary>
        /// <param name="expectedObject">The excpected object</param>
        /// <param name="testOutputHelper">The heplper to write the failure message to the test explorer</param>
        /// <param name="reference">The reference to use that will link the failure message to your code</param>
        /// <returns></returns>
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
