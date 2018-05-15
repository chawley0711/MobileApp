using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    interface ITokenValidator<T, U>
    {
        bool ValidateToken(Token<T, U> token);
    }
}
