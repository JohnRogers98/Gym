using Gym.Domain._Common;
using Gym.Infrastructure.Scanners;

namespace Gym.Infrastructure.Tests.Scanners
{
    public partial class EventContractScannerTests
    {
        [Fact]
        public void Can_Find_Types_With_Attribute()
        {
            EventContractScanner eventSerializationScanner = EventContractScanner.ScanAssembly(this.GetType().Assembly);

            Assert.Equal(typeof(SelfShuntingDomainEvent), eventSerializationScanner.GetDomainTypeByOperationKey(nameof(SelfShuntingDomainEvent)));
            Assert.Equal(typeof(SelfShuntingDto), eventSerializationScanner.GetDtoTypeByOperationKey(nameof(SelfShuntingDomainEvent)));
            Assert.Equal(typeof(SelfShuntingDto), eventSerializationScanner.GetDtoTypeByDomainType(typeof(SelfShuntingDomainEvent)));
        }

        [Fact]
        public void Checks_Existed_Serializer_Method()
        {
            EventContractScanner.ScanAssembly(this.GetType().Assembly, serializer: typeof(SelfShuntingSerializer));
        }

        [Fact]
        public void Throws_When_Serializer_Method_Not_Exist()
        {
            Assert.Throws<NotImplementedException>(() => EventContractScanner.ScanAssembly(this.GetType().Assembly, serializer: typeof(Object)));
        }

        [Fact]
        public void Checks_Existed_Deserializer_Method()
        {
            EventContractScanner.ScanAssembly(this.GetType().Assembly, deserializer: typeof(SelfShuntingDeserializer));
        }

        [Fact]
        public void Throws_When_Deserializer_Method_Not_Exist()
        {
            Assert.Throws<NotImplementedException>(() => EventContractScanner.ScanAssembly(this.GetType().Assembly, deserializer: typeof(Object)));
        }

        [EventSerializationForm<SelfShuntingDomainEvent>]
        private class SelfShuntingDto;

        private class SelfShuntingDomainEvent : DomainEvent;

        private class SelfShuntingSerializer
        {
            public SelfShuntingDto ToDto(SelfShuntingDomainEvent domainEvent)
            {
                return new SelfShuntingDto();
            }
        }

        private class SelfShuntingDeserializer
        {
            public DomainEvent ToDomainEvent(SelfShuntingDto dto)
            {
                return new SelfShuntingDomainEvent();
            }
        }
    }
}
