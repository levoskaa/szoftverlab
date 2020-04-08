using api.Controllers.Dto;
using api.DAL.EfDbContext;
using api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.DAL
{
    public class StatusesRepository : IStatusesRepository
    {
        private readonly TasksDbContext db;

        public StatusesRepository(TasksDbContext db)
        {
            this.db = db;
        }

        public bool ExistsWithName(string statusName)
        {
            return db.Statuses.Any(s => s.Name.ToUpperInvariant() == statusName.ToUpperInvariant());
        }

        public Status FindById(int statusId)
        {
            var dbRecord = db.Statuses.FirstOrDefault(s => s.Id == statusId);
            if (dbRecord == null)
                return null;
            return ToModel(dbRecord);
        }

        public Status FindByName(string statusName)
        {
            var dbRecord = db.Statuses.FirstOrDefault(s => s.Name == statusName);
            if (dbRecord == null)
                return null;
            return ToModel(dbRecord);
        }

        public Status Insert(CreateStatus value)
        {
            using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
            {
                if (db.Statuses.Any(s => s.Name.ToUpperInvariant() == value.Name.ToUpperInvariant()))
                    throw new ArgumentException("Name must be unique");

                var toInsert = new DbStatus() { Name = value.Name };
                db.Statuses.Add(toInsert);
                db.SaveChanges();
                tran.Commit();

                return new Status(toInsert.Id, toInsert.Name);
            }
        }

        public IReadOnlyCollection<Status> List()
        {
            return db.Statuses.Select(ToModel).ToList();
        }

        private static Status ToModel(DbStatus value)
        {
            return new Status(value.Id, value.Name);
        }
    }
}
