using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataBase_Website.Models;

namespace DataBase_Website.Data
{
    public class DataBase_WebsiteContext : DbContext
    {
        public DataBase_WebsiteContext (DbContextOptions<DataBase_WebsiteContext> options)
            : base(options)
        {
            
        }

        public DbSet<DataBase_Website.Models.DataBaseModels.AccountModel> AccountModel { get; set; }
        public DbSet<DataBase_Website.Models.DataBaseModels.JobModel> JobModel { get; set; }
    }
}
