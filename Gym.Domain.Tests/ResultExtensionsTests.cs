using Gym.Application.Extensions;
using Gym.Domain._Common;
using Gym.Domain._Exceptions;

namespace Gym.Domain.Tests
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void Unwrap_When_Success()
        {
            Assert.True(this.SuccessfulTypedOperation().Unwrap());
        }

        [Fact]
        public void Unwrap_Throws_When_Fail()
        {
            Assert.Throws<DomainException>(() => this.FailedTypedOperation().Unwrap());
        }

        [Fact]
        public void Bind_Successful_Results()
        {
            var result = this.SuccessfulOperation()
                .Bind(this.SuccessfulOperation);

            Assert.True(result.Success);
        }

        [Fact]
        public void Bind_Successful_With_Failed_Results()
        {
            var result = this.SuccessfulOperation()
                .Bind(this.FailedOperation);

            Assert.False(result.Success);
        }

        [Fact]
        public void Bind_Successful_Typed_Results()
        {
            var result = this.SuccessfulTypedOperation()
                .Bind(this.SuccessfulTypedOperation);

            Assert.True(result.Success);
        }

        [Fact]
        public void Bind_Successful_Typed_With_Failed_Typed_Results()
        {
            var result = this.SuccessfulTypedOperation()
                .Bind(this.FailedTypedOperation);

            Assert.False(result.Success);
        }

        [Fact]
        public void Bind_Successful_Typed_With_No_Typed_Results()
        {
            var result = this.SuccessfulTypedOperation()
                .Bind(this.SuccessfulOperation);

            Assert.True(result.Success);
        }

        private Result<Boolean> SuccessfulTypedOperation() => Result<Boolean>.Ok(true);
        private Result SuccessfulOperation() => Result.Ok();

        private Result<Boolean> FailedTypedOperation() => Result<Boolean>.Fail(new SelfShuntingError());
        private Result FailedOperation() => Result.Fail(new SelfShuntingError());

        private class SelfShuntingError : DomainError
        {
            public SelfShuntingError() : base(nameof(SelfShuntingError)) { }
        }
    }
}
