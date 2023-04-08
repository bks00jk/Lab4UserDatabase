using System;

namespace KMA.ProgrammingInCSharp2023.Lab4UserDatabase.Navigation
{
    internal interface INavigatable<TObject> where TObject : Enum
    {
        TObject ViewType
        {
            get;
        }
    }
}
