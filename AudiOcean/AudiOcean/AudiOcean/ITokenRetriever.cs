using AudiOcean.Droid;
using System;
using System.Collections.Generic;
using System.Text;

namespace AudiOcean
{
    interface ITokenRetriever<T, U>
    {
        Token<T, U> RetrieveToken();
    }
}
