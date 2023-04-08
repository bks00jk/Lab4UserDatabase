using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Managers;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Navigation;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Repositories;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Services;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged, INavigatable<MainNavigationTypes>
    {
        #region Fields
        private ObservableCollection<Person> _persons;
        private Action _gotoAddView;
        private Action _gotoEditView;
        private Person _selectedPerson;
        private int _sortIndex;
        private int _filterIndex;
        private string[] _sortList = { "FirstName", "LastName", "Email", "Age", "SunSign", "ChineseSign" };
        private string[] _filterList = { "FirstName", "LastName", "Email", "SunSign", "ChineseSign" };

        public MainViewModel(Action gotoAddView, Action gotoEditView)
        {
            StationManager.mainVM = this;
            _gotoAddView = gotoAddView;
            _gotoEditView = gotoEditView;
            _persons = new ObservableCollection<Person>(new PersonService().GetAllPersons());
        }
        #endregion

        #region Commands
        private RelayCommand<object> _addPersonCommand;
        private RelayCommand<object> _editPersonCommand;
        private RelayCommand<object> _deletePersonCommand;
        private RelayCommand<object> _filterCommand;
        #endregion

       

        #region Properties

        public MainNavigationTypes ViewType
        {
            get
            {
                return MainNavigationTypes.Main;
            }
        }
        public string FilterQuery { get; set; }

        public int SortIndex
        {
            get => _sortIndex;
            set
            {
                _sortIndex = value;
                Update();
            }
        }

        public int FilterIndex
        {
            get => _filterIndex;
            set
            {
                _filterIndex = value;
                Update();
            }
        }

        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
                StationManager.CurrentPerson = _selectedPerson;
            }
        }

        public ObservableCollection<Person> PersonsList
        {
            get
            {

                IEnumerable<Person> list = _persons;
                switch (SortIndex)
                {
                    case 0: list = list.OrderBy(p => p.FirstName); break;
                    case 1: list = list.OrderBy(p => p.LastName); break;
                    case 2: list = list.OrderBy(p => p.Email); break;
                    case 3: list = list.OrderBy(p => p.BirthDate); break;
                    case 4: list = list.OrderBy(p => p.SunSign); break;
                    case 5: list = list.OrderBy(p => p.ChineseSign); break;
                }

                if (String.IsNullOrWhiteSpace(FilterQuery)) return new ObservableCollection<Person>(list);

                switch (FilterIndex)
                {
                    case 0: list = list.Where(p => p.FirstName.ToLower().Contains(FilterQuery.ToLower())); break;
                    case 1: list = list.Where(p => p.LastName.ToLower().Contains(FilterQuery.ToLower())); break;
                    case 2: list = list.Where(p => p.Email.ToLower().Contains(FilterQuery.ToLower())); break;
                    case 3: list = list.Where(p => p.SunSign.ToLower().Contains(FilterQuery.ToLower())); break;
                    case 4: list = list.Where(p => p.ChineseSign.ToLower().Contains(FilterQuery.ToLower())); break;
                }

                return new ObservableCollection<Person>(list);
            }
            private set
            {
                _persons = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> SortList => _sortList;

        public IEnumerable<string> FilterList => _filterList;

        #endregion

        #region Commands

        public RelayCommand<object> AddPersonCommand
        {
            get
            {
                return _addPersonCommand ??= new RelayCommand<object>(_ => AddPerson());
            }
        }
        public RelayCommand<object> EditPersonCommand
        {
            get
            {
                return _editPersonCommand ??= new RelayCommand<object>(_ => EditPerson(), CanExecute);
            }
        }

        public RelayCommand<object> DeletePersonCommand
        {
            get
            {
                return _deletePersonCommand ??= new RelayCommand<object>(_ => DeletePerson(), CanExecute);
            }
        }
        public RelayCommand<object> FilterCommand => _filterCommand ?? (_filterCommand = new RelayCommand<object>(o => { Update(); }));

        #endregion

        #region CommandImplementation

        private async void AddPerson()
        {
            _gotoAddView.Invoke();
        }

        private void EditPerson()
        {
            _gotoEditView.Invoke();
        }

        private async void DeletePerson()
        {
            var authService = new AuthenticationService();
            try
            {
                LoaderManager.Instance.ShowLoader();
                await authService.DeletePerson(SelectedPerson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Deleting failed: {ex.Message}");
                return;
            }
            finally
            {
                LoaderManager.Instance.HideLoader();
            }
            MessageBox.Show($"Person {_selectedPerson.FirstName} {_selectedPerson.LastName} was deleted");
            Update();
        }

        #endregion

        public bool CanExecute(object obj) => SelectedPerson != null;

        public void Update()
        {
            PersonsList = new ObservableCollection<Person>(new PersonService().GetAllPersons());
        }

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


    }
}
