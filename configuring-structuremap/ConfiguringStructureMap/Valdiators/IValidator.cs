namespace ConfiguringStructureMap
{
    public interface IValidator<T>
    {
        bool Validate(T model);
    }
}
