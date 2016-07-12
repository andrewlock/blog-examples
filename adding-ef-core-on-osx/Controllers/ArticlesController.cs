using System;
using System.Collections.Generic;
using System.Linq;
using AddingEFCoreOnOSX.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddingEFCoreOnOSX.Controllers
{
    [Route("api/[controller]")]
    public class ArticlesController : Controller
    {
        private readonly ArticleContext _context;
        public ArticlesController(ArticleContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IEnumerable<Article> Get()
        {
            return _context.Articles.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Article Get(int id)
        {
            return _context.Articles.FirstOrDefault(x=>x.Id == id);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Article value)
        {
            _context.Articles.Add(value);
            _context.SaveChanges();
            return StatusCode(201, value);
        }
    }
}
