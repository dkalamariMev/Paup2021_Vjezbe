using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Paup2021_Vjezbe.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BazaDbContext : DbContext
    {
        public DbSet<Student> PopisStudenata { get; set; }

        public DbSet<Kolegij> PopisKolegija { get; set; }

        public DbSet<Smjer> PopisSmjerova { get; set; }

        public DbSet<Korisnik> PopisKorisnika { get; set; }

        public DbSet<Ovlast> PopisOvlasti { get; set; }
    }
}