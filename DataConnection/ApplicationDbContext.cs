using Microsoft.EntityFrameworkCore;

namespace IdentityProject.DataConnection
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }
        // This is where we will be adding models

    }
}
