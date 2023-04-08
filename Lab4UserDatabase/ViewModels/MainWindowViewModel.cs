using System.Windows;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Managers;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Navigation;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.ViewModels
{
    class MainWindowViewModel : BaseNavigatableViewModel<MainNavigationTypes>, ILoaderOwner
    {
        private bool _isEnabled = true;
        private Visibility _loaderVisibility = Visibility.Collapsed;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Visibility LoaderVisibility
        {
            get
            {
                return _loaderVisibility;
            }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            LoaderManager.Instance.Initialize(this);
            Navigate(MainNavigationTypes.Main);
        }

        protected override INavigatable<MainNavigationTypes> CreateViewModel(MainNavigationTypes type)
        {
            switch (type)
            {
                case MainNavigationTypes.Add:
                    return new AddPersonViewModel(() => Navigate(MainNavigationTypes.Main));
                case MainNavigationTypes.Edit:
                    return new EditPersonViewModel(() => Navigate(MainNavigationTypes.Main));
                case MainNavigationTypes.Main:
                    return new MainViewModel(() => Navigate(MainNavigationTypes.Add), () => Navigate(MainNavigationTypes.Edit));
                default:
                    return null;
            }
            return null;
        }
    }
}
