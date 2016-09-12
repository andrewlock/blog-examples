using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Services
{
    public class DummyTestService : ITestService
    {
        public string GetImplementationName()
        {
            return "DummyTestService";
        }
    }
}
