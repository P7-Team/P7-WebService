using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models.DTOs
{
    public class HeartbeatDTO
    {
        [FromBody, BindRequired]
        public string Status { get; set; }
        [FromBody, BindRequired]
        public string Provider { get; set; }


        public HeartBeat MapToHeartbeat()
        {
            return new HeartBeat(Status);
        }
    }
}
