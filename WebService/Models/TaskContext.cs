using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebService.Interfaces;
using WebService.Services.Repositories;

namespace WebService.Models
{
    public class TaskContext : List<Task>, ITaskContext
    {
        private readonly ResultRepository _resultRepository;

        public TaskContext(ResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        public void SaveResult(Result result)
        {
            _resultRepository.Create(result);
        }
    }
}