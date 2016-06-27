namespace ConfiguringBuiltInContainer
{
    public class SudokuService : IGamingService
    {
        private readonly IValidator<AvatarModel> _validator;  
        public SudokuService(IValidator<AvatarModel> validator)
        {
            _validator = validator;
        }
    }
}
