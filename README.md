# Valid8R

[ADD BUILD TILE]
[![The Standard](https://img.shields.io/github/v/release/hassanhabib/The-Standard?filter=v2.10.2&style=default&label=Standard%20Version&color=2ea44f)](https://github.com/hassanhabib/The-Standard)
[![The Standard - COMPLIANT](https://img.shields.io/badge/The_Standard-COMPLIANT-2ea44f)](https://github.com/hassanhabib/The-Standard)
[![The Standard Community](https://img.shields.io/discord/934130100008538142?color=%237289da&label=The%20Standard%20Community&logo=Discord)](https://discord.gg/vdPZ7hS52X)

## Introduction

Valid8 is an extension library that provides functionality to assert and verify test data.

## Code example

Currently in the standard we do the following to verify that a method has been called with the expected parameters:

```cs
    this.loggingBrokerMock.Verify(broker =>
        broker.LogError(It.Is(
            SameExceptionAs(expectedSourceValidationException))),
                Times.Once);
```
This will verify that the `LogError` method was called with the expected exception, but it will not tell us what the reason was if the parameters were not as expected.  We would only see:
```
Moq.MockException : 
Expected invocation on the mock once, but was 0 times: broker => broker.LogError(It.Is<Xeption>(actualException => actualException.SameExceptionAs(expectedException)))
```

But thanks to a recent update to Xeptions that now exposes the reasons on a mismatch, we can now write the details to the test summary.  
To do this, we need to make the following modifications to the test:

1. Add the Valid8R nuget package to your project
2. Add `ITestOutputHelper output` to your test constructor.  This helper will allow us to write to the test summary in the Test Explorer window

```cs
    public partial class SourceServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly SourceService sourceService;
        private readonly ITestOutputHelper output;

        public SourceServiceTests(ITestOutputHelper output)
        {
            this.output = output;
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.sourceService = new SourceService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        . . .

    }
```

3. In the test we can now do the verification as follows:
   - `output` refer to the helper we registered in the constructor above 
   - `reference` is a string that can be seen in the Test Explorer window. This can be useful to visually identify the thing that failed.

```cs
    this.loggingBrokerMock.Verify(broker =>
        broker.LogError(It.Is(
            Valid8.SameExceptionAs(expectedSourceValidationException, output, "this.loggingBrokerMock.Verify"))),
                Times.Once);

```

With this modification, if the parameters are not as expected, the test summary will now show the reason why the parameters were not as expected:
```
    Moq.MockException : 
Expected invocation on the mock once, but was 0 times: broker => broker.LogError(It.Is<Exception>(actualException => Valid8.IsSameExceptionAs(actualException, expectedException, testOutputHelper, reference)))

Performed invocations:

   Mock<ILoggingBroker:1> (broker):

      ILoggingBroker.LogError(GitFyle.Core.Api.Models.Foundations.Sources.Exceptions.SourceValidationException: Some validation error occurred, fix errors and try again.
 ---> GitFyle.Core.Api.Models.Foundations.Sources.Exceptions.NullSourceException: Source is null
   at GitFyle.Core.Api.Services.Foundations.Sources.SourceService.ValidateSourceIsNotNull(Source source) in D:\Repos\STX\GitFyle.Core.Api\GitFyle.Core.Api\Services\Foundations\Sources\SourceService.Validations.cs:line 21
   at GitFyle.Core.Api.Services.Foundations.Sources.SourceService.ValidateSourceOnAdd(Source source) in D:\Repos\STX\GitFyle.Core.Api\GitFyle.Core.Api\Services\Foundations\Sources\SourceService.Validations.cs:line 14
   at GitFyle.Core.Api.Services.Foundations.Sources.SourceService.<>c__DisplayClass3_0.<<AddSourceAsync>b__0>d.MoveNext() in D:\Repos\STX\GitFyle.Core.Api\GitFyle.Core.Api\Services\Foundations\Sources\SourceService.cs:line 28
--- End of stack trace from previous location ---
   at GitFyle.Core.Api.Services.Foundations.Sources.SourceService.TryCatch(ReturningSourceFunction returningSourceFunction) in D:\Repos\STX\GitFyle.Core.Api\GitFyle.Core.Api\Services\Foundations\Sources\SourceService.Exceptions.cs:line 20
   --- End of inner exception stack trace ---)


  Stack Trace: 
Mock.Verify(Mock mock, LambdaExpression expression, Times times, String failMessage) line 331
Mock`1.Verify(Expression`1 expression, Times times) line 920
Mock`1.Verify(Expression`1 expression, Func`1 times) line 934
SourceServiceTests.ShouldThrowValidationExceptionIfSourceIsNullAndLogItAsync() line 43
--- End of stack trace from previous location ---

  Standard Output: 
this.loggingBrokerMock.Verify: Expected exception message to be "Source validation error occurred, fix errors and try again.", but found "Some validation error occurred, fix errors and try again.".
```

Valid8R also has a `SameObjectAs<T>` method that can be used to verify that an object is the same as another object.  
This can be useful when you want to verify that an object is the same as another object i.e. when you modify an object bases on some internal logic, but you never see the expected invocation.

```cs
    this.storageBrokerMock.Verify(broker =>
        broker.UpdateSourceAsync(
            It.Is(Valid8.SameObjectAs<Source>(inputSource, output, "1st this.storageBrokerMock.Verify"))),
                Times.Once);
```


## Standard-Compliance
This library was built according to The Standard. The library follows engineering principles, patterns and tooling as recommended by [The Standard](https://github.com/hassanhabib/The-Standard).

This library is also a community effort which involves many hours of pair-programming, test-driven development and in-depth exploration, research, and design discussions.

## Standard-Promise
The most important fulfillment aspect in a Standard complaint system is aimed towards contributing to people, its evolution, and principles.
An organization that systematically honors an environment of learning, training, and sharing of knowledge is an organization that learns from the past, makes calculated risks for the future, 
and brings everyone within it up to speed on the current state of things as honestly, rapidly, and efficiently as possible. 
 
We believe that everyone has the right to privacy, and will never do anything that could violate that right.
We are committed to writing ethical and responsible software, and will always strive to use our skills, coding, and systems for the good.
We believe that these beliefs will help to ensure that our software(s) are safe and secure and that it will never be used to harm or collect personal data for malicious purposes.
 
The Standard Community as a promise to you is in upholding these values.
