using Authentication;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace GoogleAuthentication
{
    public class GoogleToken : Token<string, Payload>
    {
        public override string Value { get => base.Value; protected set => base.Value = value ?? throw new ArgumentException("Tokens Value cannot be null."); }
        public override Payload Payload { get => base.Payload; set => base.Payload = value; }

        public GoogleToken(string token) : base(token) { }
    }
}
