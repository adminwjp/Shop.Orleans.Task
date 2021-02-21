using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Shop.Tasks;
using Shop.Tasks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.Application.Services.Dtos;
using Utility.Domain.Entities;
using Utility.Enums;
using Utility.Response;

namespace Shop.Orleans.Tasks.Api.Controllers
{
  
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(IResponseApi), 200)]
    public class TaskController:ControllerBase
    {
        private readonly IClusterClient _client;
        private readonly ITaskGrain _taskGrain;
        public TaskController()
        {
            _client = Startup.Client;
            _taskGrain = this._client.GetGrain<ITaskGrain>("test");
        }

        /// <summary>
        /// 添加 
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        [ProducesResponseType(typeof(ResponseApi), 200)]
        public virtual ResponseApi Insert([FromBody] TaskEntity create)
        {
            _taskGrain.Add(create);
            int res = 1;
            return ResponseApi.Create(Language.Chinese, res > 0 ? Code.AddSuccess : Code.AddFail);
        }

        /// <summary>
        /// 修改 
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [HttpPost("update")]
        [ProducesResponseType(typeof(ResponseApi), 200)]
        public virtual ResponseApi Update([FromBody] TaskEntity update)
        {
            _taskGrain.Update(update);
            int res = 1;
            return ResponseApi.Create(Language.Chinese, res > 0 ? Code.ModifySuccess : Code.ModifyFail);
        }

        [HttpGet("delete/{id}")]
        [ProducesResponseType(typeof(ResponseApi), 200)]
        public virtual ResponseApi Delete(string id)
        {
            _taskGrain.Delete(id);
            return ResponseApi.Create(Language.Chinese, Code.DeleteSuccess);
        }

        [HttpPost("delete_list")]
        [ProducesResponseType(typeof(ResponseApi), 200)]
        public virtual ResponseApi DeleteList([FromBody] DeleteEntity<string> ids)
        {
            _taskGrain.Delete(ids.Ids);
            return ResponseApi.Create(Language.Chinese, Code.DeleteSuccess);
        }

        //[HttpPost("find")]

        //public virtual ResponseApi<IList<GetOutput>> Find([FromBody] GetInput input)
        //{
        //    var res = service.Find(input);
        //    return ResponseApi<IList<GetOutput>>.Create(Language.Chinese, Code.QuerySuccess).SetData(res);
        //}

        //[HttpPost("find/{page}/{size}")]
        //public virtual ResponseApi<IList<GetOutput>> FindByPage(GetInput input, int page = 1, int size = 10)
        //{
        //    var res = service.FindByPage(input, page, size);
        //    return ResponseApi<IList<GetOutput>>.Create(Language.Chinese, Code.QuerySuccess).SetData(res);
        //}

        [HttpPost("find/{page}/{size}")]
        public virtual ResponseApi<TaskResult> FindResultDtoByPage(TaskEntity input, int page = 1, int size = 10)
        {
            var res = _taskGrain.Find(input, page, size).Result;
            return ResponseApi<TaskResult>.Create(Language.Chinese, Code.QuerySuccess).SetData(res);
        }



        //[HttpPost("count")]
        //[ProducesResponseType(typeof(int), 200)]
        //public virtual int Count(GetInput input)
        //{
        //    int res = service.Count(input);
        //    return res;
        //}
    }
}
