using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.UI.WebControls;
using ModelBindingASPNET4.Models;

namespace ModelBindingASPNET4.Controllers.api
{
    public class PersonController : ApiController
    {
        [HttpPost]
        public Person UnProtected(Person person)
        {
            return person;
        }

        [HttpPost]
        public Person Protected(Person person)
        {
            ValidateRequestHeader(Request);
            return person;
        }

        public void ValidateRequestHeader(HttpRequestMessage request)
        {
            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> token;
            if (request.Headers.TryGetValues("RequestVerificationToken", out token))
            {
                string[] tokens = token.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            AntiForgery.Validate(cookieToken, formToken);
        }

    }
}
