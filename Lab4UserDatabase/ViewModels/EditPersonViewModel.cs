using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Managers;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Navigation;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Services;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.ViewModels
{
    internal class EditPersonViewModel : INotifyPropertyChanged, INavigatable<MainNavigationTypes>
    {
        #region Fields
        private Person _person = StationManager.CurrentPerson;
        private Person _newPerson;
        private Action _gotoMain;
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate = DateTime.Today;
        private RelayCommand<object> _editCommand;

        public EditPersonViewModel(Action gotoMain)
        {
            _gotoMain = gotoMain;
            FirstName = _person.FirstName;
            LastName = _person.LastName;
            Email = _person.Email;
            BirthDate = _person.BirthDate;

        }
        #endregion

       

        #region Properties

        public MainNavigationTypes ViewType
        {
            get
            {
                return MainNavigationTypes.Edit;
            }
        }
        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                OnPropertyChanged();
            }
        }

        public Person NewPerson
        {
            get => _newPerson;
            set
            {
                _newPerson = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get
            {
                return _birthDate;
            }

            set
            {
                _birthDate = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public RelayCommand<object> Edit
        {
            get
            {
                return _editCommand ??= new RelayCommand<object>(
                    EditImplementation, o => CanExecuteCommand());
            }
        }

        private async void EditImplementation(object o)
        {
            LoaderManager.Instance.ShowLoader();
            bool isSuccess = await Task.Run(() =>
            {
                try
                {
                    _newPerson = new Person(FirstName, LastName, Email, BirthDate);
                }
                catch (InvalidEmailException e)
                {
                    MessageBox.Show(e.Message, "Wrong Email", MessageBoxButton.OK);
                    return false;
                }
                catch (PastBirthdayException e)
                {
                    MessageBox.Show(e.Message, "Wrong Date", MessageBoxButton.OK);
                    return false;
                }
                catch (FutureBirthdayException e)
                {
                    MessageBox.Show(e.Message, "Wrong Date", MessageBoxButton.OK);
                    return false;
                }
                catch (InvalidNameException e)
                {
                    MessageBox.Show(e.Message, "Wrong Name or Surname", MessageBoxButton.OK);
                    return false;
                }

                if (_person.IsBirthday)
                {
                    MessageBox.Show("Happy Birthday! \nWe wish you all the best!", "Congratulations", MessageBoxButton.OK);
                }
                return true;
            });

            LoaderManager.Instance.HideLoader();
            if (isSuccess)
            {
                var authService = new AuthenticationService();
                try
                {
                    LoaderManager.Instance.ShowLoader();
                    await Task.Run(() => authService.EditPerson(_newPerson, _person));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Editing person failed: {ex.Message}");
                    return;
                }
                finally
                {
                    LoaderManager.Instance.HideLoader();
                }

                StationManager.mainVM.Update();
                MessageBox.Show("Person was successfully edited!", "New Person", MessageBoxButton.OK);
                NewPerson = null;
                _person = StationManager.CurrentPerson;
                _gotoMain.Invoke();
            }
           
        }


        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email);
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
