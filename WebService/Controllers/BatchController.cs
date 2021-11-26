using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebService.Interfaces;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;
using WebService.Services.Repositories;
using WebService.Services.Stores;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        BatchStore _store;
        public BatchController(BatchStore store)
        {
            _store = store;
        }

        // POST api/<BatchController>
        [HttpPost]
        public void Post([FromForm] BatchDTO batchInput)
        {
            Batch batch = batchInput.MapToBatch();
            //TODO: set OwnerUsername property of the batch to a real user
            batch.OwnerUsername = getUser().Username;
            _store.Store(batch);
        }   
        private User getUser()
        {
            return new User("fakeUser", "fakePassword");
        }
    }
}
