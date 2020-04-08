using api.Controllers.Dto;
using api.DAL.EfDbContext;
using api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.DAL
{
    public class TasksRepository : ITasksRepository
    {
        private readonly TasksDbContext db;
        private readonly IStatusesRepository statusesRepository;

        public TasksRepository(TasksDbContext db, IStatusesRepository statusesRepository)
        {
            this.db = db;
            this.statusesRepository = statusesRepository;
        }

        public Model.Task Delete(int taskId)
        {
            var dbRecord = db.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (dbRecord == null)
                return null;
            db.Tasks.Remove(dbRecord);
            db.SaveChanges();
            return ToModel(dbRecord);
        }

        public Model.Task FindById(int taskId)
        {
            var dbRecord = db.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (dbRecord == null)
                return null;
            return ToModel(dbRecord);
        }

        public Model.Task Insert(CreateTask value)
        {
            if (!statusesRepository.ExistsWithName(value.Status))
                statusesRepository.Insert(new CreateStatus() { Name = value.Status });

            using (var tran = db.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
            {                
                IReadOnlyCollection<Status> statuses = statusesRepository.List();
                Status status = null;
                foreach (Status s in statuses)
                {
                    if (s.Name == value.Status)
                        status = s;
                }
                var toInsert = new DbTask() { Title = value.Title, StatusId = status.Id};
                db.Tasks.Add(toInsert);
                db.SaveChanges();
                tran.Commit();

                return new Task(toInsert.Id, toInsert.Title, toInsert.Done, toInsert.Status.Name);
            }
        }

        public IReadOnlyCollection<Model.Task> List()
        {
            return db.Tasks.Select(ToModel).ToList();
        }

        public Model.Task MarkDone(int taskId)
        {
            var dbRecord = db.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (dbRecord == null)
                return null;
            dbRecord.Done = true;
            db.SaveChanges();
            return ToModel(dbRecord);
        }

        public Model.Task MoveToStatus(int taskId, string newStatusName)
        {
            var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                return null;
            var statuses = statusesRepository.List();
            Status status = null;
            foreach (Status s in statuses)
            {
                if (s.Name == newStatusName)
                {
                    status = s;
                    break;
                }
            }
            if (status == null)
                status = statusesRepository.Insert(new CreateStatus() { Name = newStatusName });
            task.StatusId = status.Id;
            db.SaveChanges();
            return ToModel(task);
        }

        private Task ToModel(DbTask value)
        {
            Status status = statusesRepository.FindById(value.StatusId);
            return new Task(value.Id, value.Title, value.Done, status.Name);
        }
    }
}
