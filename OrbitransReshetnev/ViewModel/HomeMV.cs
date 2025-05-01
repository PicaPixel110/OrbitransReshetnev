using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrbitransReshetnev.Model;
using System.Windows.Threading;
using System.Net.Http;
using OrbitransReshetnev.Utilities;


namespace OrbitransReshetnev.ViewModel
{
    class HomeMV : Utilities.ViewModelBase
    {

        // Как я понял здесь работа с данными //

        private readonly MainPage _pageModel;
        private readonly HttpClient _httpClient;
        private readonly DispatcherTimer _timer;

        private string _displayDate;
        public string DisplayDate
        {
            get { return _displayDate; }
            set { _displayDate = value; OnPropertyChanget(); }
        }

        private decimal _usdRate;
        public decimal UsdRate
        {
            get { return _usdRate; }
            set { _usdRate = value; OnPropertyChanget(); OnPropertyChanget(nameof(CurrencyRates)); }
        }

        private decimal _eurRate;
        public decimal EurRate
        {
            get { return _eurRate; }
            set { _eurRate = value; OnPropertyChanget(); OnPropertyChanget(nameof(CurrencyRates)); }
        }

        public string CurrencyRates
        {
            get { return $"USD: {UsdRate:F2} RUB | EUR: {EurRate:F2} RUB"; }
        }

        public HomeMV()
        {
            _pageModel = new MainPage();
            CultureInfo ruCulture = new CultureInfo("ru-RU");
            DisplayDate = DateTime.Now.ToString("dd MMMM, yyyy", ruCulture);

            // Инициализация HttpClient
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://open.er-api.com/");

            // Установка начальных значений (на случай, если API недоступен)
            UsdRate = 92.50m;
            EurRate = 98.30m;

            // Загрузка курсов валют при старте
            LoadCurrencyRatesAsync();

            // Таймер для обновления курсов каждые 60 минут
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(60)
            };
            _timer.Tick += async (s, e) => await LoadCurrencyRatesAsync();
            _timer.Start();
        }

        private async Task LoadCurrencyRatesAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("v6/latest/USD");
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(jsonResponse);

                // Проверяем, успешен ли запрос
                string result = data["result"]?.ToString();
                if (result != "success")
                {
                    throw new Exception("API request failed.");
                }

                // Извлекаем курсы валют
                JObject rates = (JObject)data["rates"];
                UsdRate = rates["RUB"]?.Value<decimal>() ?? UsdRate;
                decimal usdToEur = rates["EUR"]?.Value<decimal>() ?? 1m;
                EurRate = (usdToEur != 0) ? (rates["RUB"]?.Value<decimal>() ?? EurRate) / usdToEur : EurRate;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load currency rates: {ex.Message}");
            }
        }
    }
}
