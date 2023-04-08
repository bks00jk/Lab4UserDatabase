using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Managers;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools.Exceptions;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Navigation;
using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Services;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.ViewModels
{
    internal class AddPersonViewModel : INavigatable<MainNavigationTypes>, INotifyPropertyChanged
    {
        #region Fields
        private Person _person;
        private Action _gotoMain;
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate = DateTime.Today;
        private RelayCommand<object> _addCommand;

        public AddPersonViewModel(Action gotoMain)
        {
            _gotoMain = gotoMain;
        }
        #endregion

        #region Properties

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

        public MainNavigationTypes ViewType
        {
            get
            {
                return MainNavigationTypes.Add;
            }
        }
        #endregion


        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Email);
        }
        public RelayCommand<object> Add
        {
            get
            {
                return _addCommand ??= new RelayCommand<object>(
                    AddPersonImplementation, o => CanExecuteCommand());
            }
        }
       

        private async void AddPersonImplementation(object o)
        {
            LoaderManager.Instance.ShowLoader();
            bool isSuccess = await Task.Run(() =>
            {
                try
                {
                    _person = new Person(FirstName, LastName, Email, BirthDate);
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
                    await Task.Run(() => authService.AddPerson(_person));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Adding person failed: {ex.Message}");
                    return;
                }
                finally
                {
                    LoaderManager.Instance.HideLoader();
                }


                StationManager.mainVM.Update();
                MessageBox.Show("Person was successfully added!", "New Person", MessageBoxButton.OK);
                FirstName = "";
                LastName = "";
                Email = "";
                BirthDate = DateTime.Today;
                _gotoMain.Invoke();
            }
           

        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
    }
