using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication
{
    interface ITokenRetriever<T, U>
    {
        Token<T, U> RetrieveToken();
    }
}
