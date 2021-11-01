using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebService.Interfaces;

namespace WebService.Models
{
    public class TaskContext : List<Task>, ITaskContext
    {
        
    }
}