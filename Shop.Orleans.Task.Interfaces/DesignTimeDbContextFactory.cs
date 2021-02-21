using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;
using Utility.Config;
using Utility.Ef;
using Utility.Utils;

namespace Shop
{
    /// <summary>
    /// vs 升级后造成 的 环境 变动 影响
    /// System.Reflection.AmbiguousMatchException: Ambiguous match found. 什么 ef 迁移不过去 单元测试可以  不能放到 一起 要单独放
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskDbContent>
    {
        public TaskDbContent CreateDbContext(string[] args)
        {
            //Method not found: 'System.Collections.Generic.Dictionary`2<System.String,System.Object> Microsoft.Extensions.Configuration.IConfigurationBuilder.get_Properties()'.
            DbConfig.Flag = DbFlag.MySql;
            Utility.Ef.ConnectionHelper.ConnectionString = ConfigManager.GetByConsul($"ShopTask/{DbConfig.Flag}ConnectionString");
            var bulder = AbstractDesignTimeDbContextFactory<TaskDbContent>.Parse();
            return new TaskDbContent(bulder.Options);
        }
        //public static DbContextOptionsBuilder<ProductDbContext> Parse(DbFlag flag, string connectionString, DbContextOptionsBuilder<ProductDbContext> bulder) => flag switch
        //{
        //    DbFlag.MySql => bulder.UseMySql(connectionString),
        //    DbFlag.SqlServer => bulder.UseSqlServer(connectionString),
        //    DbFlag.Oracle => bulder.UseOracle(connectionString),
        //    DbFlag.Postgre => bulder.UseNpgsql(connectionString),
        //    DbFlag.Sqlite => bulder.UseSqlite(connectionString),
        //    _ => bulder
        //};
    }
}
