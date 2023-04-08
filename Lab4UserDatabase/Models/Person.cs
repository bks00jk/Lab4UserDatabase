using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase
{
    [Serializable]
    public class Person : INotifyPropertyChanged
    {
        #region Fields
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate;
        private bool _isAdult;
        private string _sunSign;
        private string _chineseSign;
        private bool _isBirthday;
        private int _age;
        #endregion

        #region Constructors
        [JsonConstructor]
        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            CheckAge(birthDate);

            string name_pattern = @"^[A-Za-z]+$";
            string email_pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(firstName, name_pattern))
            {
                throw new InvalidNameException($"Invalid name : {firstName}! \nThe name should consist of only uppercase and lowercase letters, \nwithout any numbers or special characters!");
            }
            if (!Regex.IsMatch(lastName, name_pattern))
            {
                throw new InvalidNameException($"Invalid surname : {lastName}!\nThe surname should consist of only uppercase and lowercase letters, \nwithout any numbers or special characters!");
            }
            if(!Regex.IsMatch(email, email_pattern))
            {
                throw new InvalidEmailException($"Invalid email : {email}!");
            }

            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _birthDate = birthDate;

            FindSunSign(birthDate);
            FindChineseSign(birthDate);

        }

        public Person(string firstName, string lastName, string email) : this(firstName, lastName, email, DateTime.Today)
        { }

        public Person(string firstName, string lastName, DateTime birthDate) : this(firstName, lastName, null, birthDate)
        { }
        #endregion

        #region Properties
        public string FirstName
        {
            get { return _firstName; }
            private set { _firstName = value;
                OnPropertyChanged();
            }

        }
        public string LastName
        {
            get { return _lastName; }
            private set { _lastName = value; OnPropertyChanged();
            }
        }

        public string? Email
        {
            get { return _email; }
            private set { _email = value; OnPropertyChanged();
            }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            private set { _birthDate = value; OnPropertyChanged();
            }
        }

        public bool IsAdult
        {
            get { return _isAdult; }
        }

        public string SunSign
        {
            get { return _sunSign; }
        }

        public string ChineseSign
        {
            get { return _chineseSign; }
        }

        public bool IsBirthday
        {
            get { return _isBirthday; }
        }

        public int Age
        {
            get { return _age; }
        }
        #endregion

        #region Methods
        private void FindChineseSign(DateTime birthDate)
        {
            string[] chineseSigns = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Sheep" };
            int zodiacIndex = birthDate.Year % 12;
            _chineseSign = chineseSigns[zodiacIndex];
        }

        private void FindSunSign(DateTime birthDate)
        {

            var day = birthDate.Day;
            var month = birthDate.Month;

            switch (month)
            {
                case 1:
                    _sunSign = (day <= 20) ? "Capricorn" : "Aquarius";
                    break;
                case 2:
                    _sunSign = (day <= 19) ? "Aquarius" : "Pisces";
                    break;
                case 3:
                    _sunSign = (day <= 20) ? "Pisces" : "Aries";
                    break;
                case 4:
                    _sunSign = (day <= 20) ? "Aries" : "Taurus";
                    break;
                case 5:
                    _sunSign = (day <= 21) ? "Taurus" : "Gemini";
                    break;
                case 6:
                    _sunSign = (day <= 21) ? "Gemini" : "Cancer";
                    break;
                case 7:
                    _sunSign = (day <= 22) ? "Cancer" : "Leo";
                    break;
                case 8:
                    _sunSign = (day <= 23) ? "Leo" : "Virgo";
                    break;
                case 9:
                    _sunSign = (day <= 23) ? "Virgo" : "Libra";
                    break;
                case 10:
                    _sunSign = (day <= 23) ? "Libra" : "Scorpio";
                    break;
                case 11:
                    _sunSign = (day <= 22) ? "Scorpio" : "Sagittarius";
                    break;
                case 12:
                    _sunSign = (day <= 21) ? "Sagittarius" : "Capricorn";
                    break;
                default:
                    throw new ArgumentException("Invalid birth month.");
            }
        }

        private void CheckAge(DateTime birthDate)
        {
            _isBirthday = (birthDate.Month == DateTime.Today.Month && birthDate.Day == DateTime.Today.Day) ? true : false;

            _age = DateTime.Today.Year - birthDate.Year;
            if (DateTime.Today.Month < birthDate.Month || (DateTime.Today.Month == birthDate.Month && DateTime.Today.Day < birthDate.Day))
            {
                _age--;
            }

            if (_age < 0)
            {
                throw new FutureBirthdayException($"Invalid birth date : {birthDate}! \nThe event has not yet occurred!");
            }
            else if (_age > 135)
            {
                throw new PastBirthdayException($"Invalid birth date : {birthDate}! \nDate is too old to be birthday!");
            }

            _isAdult = _age >= 18;
        }
        #endregion

        #region OnPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
