using CurrencyConvertor.Converters;

namespace CurrencyConvertorTest
{
    public class CurrencyConvertorTests
    {
        [Fact]
        public void Convert_Should_Mupliply_Currency_When_The_First_One_Is_Greater_Than_The_Second()
        {
            //Arrenge
            var sut = new CurrencyConverter();

            //Act
            var actual = sut.Convert("USD", "EUR", 10);

            //Assert
            Assert.Equal(8.6, actual);
        }
    }
}