namespace AudiOcean.Droid
{
    public class Token<T, U>
    {
        T Value { get; }

        U Payload { get; set; }

        public Token(T token)
        {
            this.Value = token;
        }
    }
}