using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.DBContexts
{
    public class SQLContext: DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
