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
                0.58,
                (k, v) => v
            );

            currencies.AddOrUpdate(
                new Tuple<string, string>("USD", "EUR"),
                0.86,
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
            var findRate = currencies.ContainsKey(new Tuple<string, string>(fromCurrency, toCurrency));

            if (findRate)
            {

            }

            if (findRate)
            {

            }

            throw new ArgumentException("currency has not founded");
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
