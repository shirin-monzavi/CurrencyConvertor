using CurrencyConvertor.Converters;

namespace CurrencyConvertorTest
{
    public class CurrencyConvertorTests
    {
        [Theory]

        //Multiply
        [InlineData("USD", "CAD", 10,13.4)]
        [InlineData("CAD", "GBP", 10,5.8)]
        [InlineData("USD", "EUR", 10,8.6)]
        [InlineData("IR", "USD", 10, 0)]

        //Divide
        [InlineData("CAD", "USD", 11, 8.21)]
        [InlineData("GBP", "CAD", 12, 20.69)]
        [InlineData("EUR", "USD", 15, 17.44)]
        public void Convert_Should_Mupliply_Currency_When_The_First_One_Is_Greater_Than_The_Second(
            string from,
            string to,
            double amount,
            double expected)
        {
            //Arrenge
            var sut =  CurrencyConverter.Instance;

            //Act
            var actual = sut.Convert(from, to, amount);

            //Assert
            Assert.Equal(expected, actual);
        }

    }
}