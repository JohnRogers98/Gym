using Gym.Infrastructure.Telegram;

namespace Gym.Infrastructure.Tests.Telegram
{
    public class TelegramSignatureVerifierTests
    {
        [Fact]
        public void Verify_That_Init_Data_Hash_Are_Valid()
        {
            //Given
            String escapedInitData = "tgWebAppData=user%3D%257B%2522id%2522%253A5265776755%252C%2522first_name%2522%253A%2522Andrey%2522%252C%2522last_name%2522%253A%2522Kardashian%2522%252C%2522username%2522%253A%2522john_rogers_zhuzhma%2522%252C%2522language_code%2522%253A%2522en%2522%252C%2522allows_write_to_pm%2522%253Atrue%252C%2522photo_url%2522%253A%2522https%253A%255C%252F%255C%252Ft.me%255C%252Fi%255C%252Fuserpic%255C%252F320%255C%252FBoKwKY5IpDRWz6uIog74D57Ss61WeT4Jf5zTwZZS-1eHFwRJR56ePM-6SrkBKkaV.svg%2522%257D%26chat_instance%3D4756907096746764373%26chat_type%3Dprivate%26auth_date%3D1766844573%26signature%3DeNGfNiBcoU6lw7RV9SNMKxZNVfpltKsRWBG0MwDi1mzuLHnq0z_KkCRFnThl071xnR7cyWZ_IxcS2pz80NFHBQ%26hash%3D95553035c77fe4aa746125e758e8393d89e8fd15b18ec8de1471903d4ce1d044";
            var botToken = TelegramBotToken.From(Environment.GetEnvironmentVariable("DOTNET_TG_BOT_TOKEN")!);

            TelegramSignatureVerifier sut = new TelegramSignatureVerifier(botToken);

            //When
            var result = sut.Verify(escapedInitData);

            //Then
            Assert.True(result.Success);
            Assert.Equal(5265776755, result.Data!.Id.Value);
        }

        [Fact]
        public void Verify_That_Init_Data_Hash_Are_Not_Valid_When_Not_The_Same()
        {
            //Given
            String escapedInitData = "tgWebAppData=user%3D%257B%2522id%2522%253A5265776755%252C%2522first_name%2522%253A%2522Andrey%2522%252C%2522last_name%2522%253A%2522Kardashian%2522%252C%2522username%2522%253A%2522john_rogers_zhuzhma%2522%252C%2522language_code%2522%253A%2522en%2522%252C%2522allows_write_to_pm%2522%253Atrue%252C%2522photo_url%2522%253A%2522https%253A%255C%252F%255C%252Ft.me%255C%252Fi%255C%252Fuserpic%255C%252F320%255C%252FBoKwKY5IpDRWz6uIog74D57Ss61WeT4Jf5zTwZZS-1eHFwRJR56ePM-6SrkBKkaV.svg%2522%257D%26chat_instance%3D4756907096746764373%26chat_type%3Dprivate%26auth_date%3D1766844573%26signature%3DeNGfNiBcoU6lw7RV9SNMKxZNVfpltKsRWBG0MwDi1mzuLHnq0z_KkCRFnThl071xnR7cyWZ_IxcS2pz80NFHBQ%26hash%3D95553035c33fe4aa746125e758e8393d89e8fd15b18ec8de1471903d4ce1d044";
            var botToken = TelegramBotToken.From(Environment.GetEnvironmentVariable("DOTNET_TG_BOT_TOKEN")!);

            TelegramSignatureVerifier sut = new TelegramSignatureVerifier(botToken);

            //When
            var result = sut.Verify(escapedInitData);

            //Then
            Assert.False(result.Success);
            Assert.Null(result.Data);
        }
    }
}
