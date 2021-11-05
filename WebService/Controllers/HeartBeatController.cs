using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebService.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController
    {

        [HttpPost("{Message}")]
        public void Post(String Message)
        {
            
        }
    }
    
}