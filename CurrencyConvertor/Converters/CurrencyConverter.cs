using System.Collections.Concurrent;

namespace CurrencyConvertor.Converters
{
    public class CurrencyConverter : ICurrencyConverter
    {
        #region Private
        private readonly ConcurrentDictionary<Tuple<string, string>, double> currencies;
        private static CurrencyConverter _instance;
        private static readonly object synclock = new object();
        #endregion

        #region Constructor
        private CurrencyConverter()
        {
            currencies = new ConcurrentDictionary<Tuple<string, string>, double>();

            currencies.AddOrUpdate(
                new Tuple<string, string>("USD", "CAD"),
                1.34D,
                (k, v) => v
                );

            currencies.AddOrUpdate(
                new Tuple<string, string>("CAD", "GBP"),
                0.58D,
                (k, v) => v
            );

            currencies.AddOrUpdate(
                new Tuple<string, string>("USD", "EUR"),
                0.86D,
                (k, v) => v
            );
        }
        #endregion

        #region Public
        public static CurrencyConverter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (synclock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CurrencyConverter();
                        }
                    }
                }
                return _instance;
            }
        }

        public void ClearConfiguration()
        {
            throw new NotImplementedException();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            double value, rate;

            var findRate = currencies.TryGetValue(new Tuple<string, string>(fromCurrency, toCurrency), out value);

            if (findRate)
            {
                return calculator(amount, value, "*", out rate);
            }

            findRate = currencies.TryGetValue(new Tuple<string, string>(toCurrency, fromCurrency), out value);

            if (findRate)
            {
                return calculator(amount, value, "/", out rate);
            }

            return 0;
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Method

        private double calculator(double amount, double value, string @operator, out double rate)
        {
            if (@operator == "*")
            {
                rate = multiply(amount, value);

                return roundRate(rate);
            }

            rate = divide(amount, value);

            return roundRate(rate);
        }

        private double roundRate(double rate)
        {
            return Math.Round(rate, 2, MidpointRounding.ToEven);
        }

        private double multiply(double amount, double value) =>
             amount * value;

        private double divide(double amount, double value) =>
             amount / value;
        #endregion

    }
}
