namespace ConfiguringStructureMap
{
    public class UserModelValidator : IValidator<UserModel>
    {
        public bool Validate(UserModel model)
        {
            return model.Age > 18;
        }
    }
}
