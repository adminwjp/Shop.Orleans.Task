using Orleans;
using System;
using System.Threading.Tasks;
using Utility.Application.Services.Dtos;
using Utility.Domain.Repositories;

namespace Shop.Tasks.Interfaces
{
    /// <summary>
    /// Shop.Tasks.Interfaces.ITaskGrain.Find(Shop.Tasks.TaskEntity, int, int) 
    /// which returns a non-awaitable type Utility.Application.Services.
    /// Dtos.ResultDto<Shop.Tasks.TaskEntity>. All grain interface methods must 
    /// return awaitable types. Did you mean to 
    /// return Task<Utility.Application.Services.Dtos.ResultDto<Shop.Tasks.TaskEntity>>?
    /// 
    /// BinaryFormatter serialization and deserialization are disabled within 
    /// this application.
    /// See https://aka.ms/binaryformatter for more information.
    /// </summary>
    public interface ITaskGrain: IGrainWithStringKey
    {
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<string> SayHello(string name);

        void Add(TaskEntity task);

        void Update(TaskEntity task);


        void Delete(string id);


        void Delete(string[] ids);

        Task<TaskResult> Find(TaskEntity task,int page,int size);
    }
}
