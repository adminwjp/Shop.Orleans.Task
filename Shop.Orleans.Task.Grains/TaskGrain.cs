using Microsoft.Extensions.Logging;
using Shop.Tasks.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility.Application.Services.Dtos;
using Utility.Domain.Uow;
using Utility.Extensions;

namespace Shop.Tasks.Grains
{
    public class TaskGrain : Orleans.Grain, ITaskGrain
    {
        private readonly ILogger logger;//注入不进去
        
        private IUnitWork unitWork;
        public TaskGrain(IUnitWork unitWork)
        {
            this.unitWork = unitWork;
        }

        //public TaskGrain(ILogger logger, IUnitWork unitWork)
        //{
        //    this.logger = logger;
        //    this.unitWork = unitWork;
        //}

        public void Add(TaskEntity task)
        {
            task.CreationTime = DateTime.Now;
            task.Id = Guid.NewGuid().ToString("N");
            unitWork.Insert(task);
        }

        public void Delete(string id)
        {
            unitWork.Delete(id);
        }

        public void Delete(string[] ids)
        {
            Expression<Func<TaskEntity, bool>> where = null;
            foreach (var item in ids)
            {
                where = LinqExpression.Or(where, it => it.Id == item);
            }
            unitWork.Update<TaskEntity>(where, it => new TaskEntity()
            {
                LastModificationTime = DateTime.Now,
                IsDeleted = true
            });
        }

        public Task<TaskResult> Find(TaskEntity task, int page, int size)
        {
           var data= unitWork.FindByPage<TaskEntity>(null,page,size).ToList();
            var count= unitWork.Count<TaskEntity>(null);
            // 不能用 这个
            //new Task();
            return Task.FromResult<TaskResult>(
            new TaskResult
            {
                Data = data,
                Result = new PageResultDto(page, size,
              count / size == 0 ? count / size : count / size + 1, count)
            });
        }

        public System.Threading.Tasks.Task<string> SayHello(string name)
        {
            logger.LogInformation($"SayHello message received: greeting = '{name}'");
            return System.Threading.Tasks.Task.FromResult($"You said: '{name}', I say: Hello!");
        }

        public void Update(TaskEntity task)
        {
            task.LastModificationTime = DateTime.Now;
            unitWork.Update(task);
        }
    }
}
