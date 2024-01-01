using CurrencyConvertor.Converters;
using FluentAssertions;

namespace CurrencyConvertorTest
{
    public class CurrencyConvertorTests
    {
        private readonly CurrencyConverter sut;

        public CurrencyConvertorTests()
        {
             sut = CurrencyConverter.Instance;
        }

        [Theory]
        //Multiply
        [InlineData("USD", "CAD", 10, 13.4)]
        [InlineData("CAD", "GBP", 10, 5.8)]
        [InlineData("USD", "EUR", 10, 8.6)]
        [InlineData("IR", "USD", 10, 0)]
        //Divide
        [InlineData("CAD", "USD", 11, 8.21)]
        [InlineData("GBP", "CAD", 12, 20.69)]
        [InlineData("EUR", "USD", 15, 17.44)]

        //Not Directed
        [InlineData("CAD", "EUR", 10, 6.42)]
        public void Convert_Should_Convert_Currencies(
            string from,
            string to,
            double amount,
            double expected)
        {
            //Arrenge

            //Act
            var actual = sut.Convert(from, to, amount);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ClearConfiguration_Should_Remove_All_Config()
        {
            //Arrenge

            //Act
            sut.ClearConfiguration();

            //Assert
            Assert.Empty(sut.Currencies);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void UpdateConfiguration_Should_Add_The_New_One(
            Tuple<List<Tuple<string, string, double>>, int> data)
        {
            //Arrenge

            //Act
            sut.UpdateConfiguration(data.Item1);

            //Assert
            sut.Currencies.Should().HaveCount(data.Item2);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] {
                new Tuple<List<Tuple<string, string, double>>,int>(
                    item1: new List<Tuple<string, string, double>>()
                    {
                        new Tuple<string,string,double>("USD","CAD", 1.5),
                    },
                    item2:3
                    ) 
            },

            new object[] {
                new Tuple<List<Tuple<string, string, double>>,int>(
                    item1: new List<Tuple<string, string, double>>()
                    {
                        new Tuple<string,string,double>("COP","CAD", 1.6),
                        new Tuple<string,string,double>("CLP","CAD", 1.8),
                    },
                    item2:5
                    )
            },

        };
    }
}