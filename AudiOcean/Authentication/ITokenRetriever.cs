using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication
{
    public interface ITokenRetriever<T, U>
    {
        Token<T, U> RetrieveToken();
    }
}
