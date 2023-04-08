using KMA.ProgrammingInCSharp2023.Lab4UserDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Managers
{
    static class StationManager
    {
        internal static Person CurrentPerson { get; set; }
        internal static MainViewModel mainVM { get; set; }
    }
}
