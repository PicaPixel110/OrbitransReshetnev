using OrbitransReshetnev.Utilities;
using System.Windows.Input;

namespace OrbitransReshetnev.ViewModel
{
    class NavigationMV : Utilities.ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanget(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand PaymentOderCommand { get; set; }
        public ICommand BankCommand { get; set; }
        public ICommand PredCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeMV();
        private void PaymentOder(object obj) => CurrentView = new PaymentOrderMV();
        private void Bank(object obj) => CurrentView = new BankMV();
        private void Pred(object obj) => CurrentView = new PredMV();

        public NavigationMV()
        {
            HomeCommand = new RelayComand(Home);
            PaymentOderCommand = new RelayComand(PaymentOder);
            BankCommand = new RelayComand(Bank);
            PredCommand = new RelayComand(Pred);

            CurrentView = new HomeMV();
        }
    }
}
