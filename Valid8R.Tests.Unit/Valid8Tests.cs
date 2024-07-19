// -----------------------------------------------------
// Copyright (c)  Christo du Toit - All rights reserved.
// -----------------------------------------------------

using Moq;
using Tynamix.ObjectFiller;
using Xunit.Abstractions;

namespace Valid8R.Tests.Unit
{
    public partial class Valid8Tests
    {
        private readonly Mock<ITestOutputHelper> testOutputHelperMock;

        public Valid8Tests()
        {
            testOutputHelperMock = new Mock<ITestOutputHelper>();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
