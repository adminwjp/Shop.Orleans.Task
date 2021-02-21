using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Tasks
{
    /// <summary>
    /// 任务
    /// </summary>
    [ Table("t_task")]
	[Serializable]
	public class TaskEntity:BaseEntity
    {
		/// <summary>
		/// 任务休息时间
		/// </summary>
		[Column("sleep")]
		[System.ComponentModel.DataAnnotations.StringLength(20)]
		public virtual string Sleep { get; set; }
		/// <summary>
		/// 下次工作时间
		/// </summary>
		[Column("next_work_time")]
		public virtual DateTime NextWorkTime { get; set; }
		/// <summary>
		/// 任务编码
		/// </summary>
		[Column("code")]
		[System.ComponentModel.DataAnnotations.StringLength(10)]
		public virtual string Code { get; set; }
		/// <summary>
		/// 当前任务状态
		/// </summary>
		[Column("status")]
		[System.ComponentModel.DataAnnotations.StringLength(20)]
		public virtual string Status { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		[Column("name")]
		[System.ComponentModel.DataAnnotations.StringLength(20)]
		public virtual string Name { get; set; }

	}
}
