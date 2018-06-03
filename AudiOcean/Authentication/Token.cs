namespace Authentication
{
    public class Token<T, U>
    {
        private U _payload;

        public virtual T Value { get; protected set; }

        public virtual U Payload { get => _payload; set => _payload = value; }

        public Token(T token)
        {
            this.Value = token;
        }
    }
}