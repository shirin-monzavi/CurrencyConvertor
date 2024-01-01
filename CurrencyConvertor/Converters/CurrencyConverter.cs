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

        #region Public Properties
        public ConcurrentDictionary<Tuple<string, string>, double> Currencies => currencies;
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
            currencies.Clear();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            double value;

            if (findRate(fromCurrency, toCurrency, out value))
                return calculator(amount, value, "*");

            if (findRate(toCurrency, fromCurrency, out value))
                return calculator(amount, value, "/");

            foreach (var currency in currencies)
            {
                if (currency.Key.Item2 == toCurrency &&
                    currencies.Any(c => c.Key.Item2 == currency.Key.Item2 && c.Key.Item2 == toCurrency)
                    )
                {
                    var findInDirectRate = findRate(currency.Key.Item1, fromCurrency, out value);

                    var newAmount = calculator(amount, value, "/");

                    return calculator(newAmount, currency.Value, "*");
                }
            }

            return 0;
        }


        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            foreach (var conversionRate in conversionRates)
            {
                currencies.AddOrUpdate(
                new Tuple<string, string>(
                    conversionRate.Item1,
                    conversionRate.Item2),
                    conversionRate.Item3,
                    (k, v) => conversionRate.Item3
                );
            }
        }
        #endregion

        #region Private Method
        private bool findRate(string fromCurrency, string toCurrency, out double value) =>
             currencies
                .TryGetValue(
                    new Tuple<string, string>(fromCurrency, toCurrency),
                    out value
                );

        private double calculator(double amount, double value, string @operator)
        {
            double rate;

            if (@operator == "*")
            {
                rate = multiply(amount, value);

                return roundRate(rate);
            }

            rate = divide(amount, value);

            return roundRate(rate);
        }

        private double roundRate(double rate) =>
             Math.Round(rate, 2, MidpointRounding.ToEven);

        private double multiply(double amount, double value) =>
             amount * value;

        private double divide(double amount, double value) =>
             amount / value;
        #endregion
    }
}
