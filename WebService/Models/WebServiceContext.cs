using Microsoft.EntityFrameworkCore;

namespace WebService.Models
{
    public class WebServiceContext : DbContext
    {
        public WebServiceContext(DbContextOptions<WebServiceContext> options) : base(options)
        {
            
        }
        
        public DbSet<Task> Tasks { get; set; }
    }
}