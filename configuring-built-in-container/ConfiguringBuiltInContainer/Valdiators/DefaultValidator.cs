namespace ConfiguringBuiltInContainer
{
    public class DefaultValidator<T> : IValidator<T>
    {
        public bool Validate(T model)
        {
            return true;
        }
    }
}
