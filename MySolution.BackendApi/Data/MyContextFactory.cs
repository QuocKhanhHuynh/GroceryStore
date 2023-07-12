using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySolution.BackendApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySolution.BackendApi.Data
{
    public class MyContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationRoot Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{enviroment}.json").Build();
            var ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            var OptionsBuilder = new DbContextOptionsBuilder<MyDbContext>(); 
            OptionsBuilder.UseSqlServer(ConnectionString);
            return new MyDbContext(OptionsBuilder.Options);
        }
    }
}
