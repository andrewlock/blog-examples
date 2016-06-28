using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfiguringBuiltInContainer
{
    public class PurchasingService : IPurchasingService
    {
        private readonly IValidator<UserModel> _validator;  
        public PurchasingService(IValidator<UserModel> validator)
        {
            _validator = validator;
        }
    }
}
