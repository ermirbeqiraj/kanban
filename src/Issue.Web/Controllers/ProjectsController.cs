using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Issue.Web.Controllers
{
    public class ProjectsController : ControllerBase
    {

        public ProjectsController()
        {

        }

        [Authorize]
        public IActionResult List()
        {
            return Ok();
        }
    }
}