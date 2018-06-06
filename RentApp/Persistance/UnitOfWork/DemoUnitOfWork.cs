using RentApp.Models.Entities;
using RepoDemo.Persistance.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Unity.Attributes;

namespace RepoDemo.Persistance.UnitOfWork
{
    public class DemoUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        [Dependency]
        public IServiceRepository Services { get; set; }
        [Dependency]
        public IRepository<AppUser, int> Users { get; set; }
        [Dependency]
        public IRepository<Comment, int> Comments { get; set; }
        [Dependency]
        public IRepository<Office, int> Offices { get; set; }
        [Dependency]
        public IRepository<PriceList, int> PriceLists { get; set; }
        [Dependency]
        public IRepository<PriceListItem, int> PriceListItems { get; set; }
        [Dependency]
        public IRepository<Vehicle, int> Vehicles { get; set; }
        [Dependency]
        public IRepository<VehicleType, int> VehicleTypes { get; set; }


        public DemoUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
