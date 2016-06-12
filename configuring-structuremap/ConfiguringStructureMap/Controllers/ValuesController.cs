using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ConfiguringStructureMap.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IEnumerable<IGamingService> _gamingService;
        private readonly IPurchasingService _purchasingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaderboard<UserModel> _leaderboard;
        private readonly ConcreteService _concreteService;

        public ValuesController(IEnumerable<IGamingService> gamingServices, IPurchasingService purchasingService, IUnitOfWork unitOfWork, ILeaderboard<UserModel> leaderboard, ConcreteService concreteService)
        {
            _gamingService = gamingServices;
            _purchasingService = purchasingService;
            _unitOfWork = unitOfWork;
            _leaderboard = leaderboard;
            _concreteService = concreteService;
        }
        
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
