using System.ComponentModel;
using System.Windows;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Managers
{
    interface ILoaderOwner : INotifyPropertyChanged
    {
        public bool IsEnabled
        {
            get;
            set;
        }

        public Visibility LoaderVisibility
        {
            get;
            set;
        }
    }
}
