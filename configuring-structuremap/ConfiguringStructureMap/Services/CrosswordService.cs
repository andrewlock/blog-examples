namespace ConfiguringStructureMap
{
    public class CrosswordService : IGamingService
    {
        private readonly IValidator<AvatarModel> _validator;  
        public CrosswordService(IValidator<AvatarModel> validator)
        {
            _validator = validator;
        }
    }
}
