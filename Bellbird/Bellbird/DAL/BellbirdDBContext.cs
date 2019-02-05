using Bellbird.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bellbird.DAL
{
    public class BellbirdDbContext : DbContext
    {
        public BellbirdDbContext(DbContextOptions<BellbirdDbContext> options)
            : base(options)
        { }
        public virtual DbSet<Alarm> Alarms { get; set; }

    }
}
