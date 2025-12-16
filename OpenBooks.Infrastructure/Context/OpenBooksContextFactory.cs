using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OpenBooksBack.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBooks.Infrastructure.Context
{
    public class OpenBooksContextFactory : IDesignTimeDbContextFactory<OpenBooksContext>
    {
        public OpenBooksContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OpenBooksContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=openbooks_db;Username=postgres;Password=postgres");

            return new OpenBooksContext(optionsBuilder.Options);
        }
    }
}
