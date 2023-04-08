using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Tools.Exceptions
{
    internal class PastBirthdayException : Exception
    {
        internal PastBirthdayException(string message) : base(message) { }
    }
}
