﻿using Infrastructure.Service.Base;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// 数据库工厂
    /// </summary>
    public class DbFactory
    {
        public static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// SqlSugarClient属性
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetSqlSugarClient(string connectString = "")
        {
            string rootdir = AppContext.BaseDirectory;
            var _connectString = String.Empty;
            IConfigurationBuilder builder;
#if DEBUG
            builder = new ConfigurationBuilder().SetBasePath(rootdir).AddJsonFile("infrastructuresetting.debug.json", optional: false, reloadOnChange: true);
#else
            var runHost = NetworkInterface
                .GetAllNetworkInterfaces()
                .Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .FirstOrDefault(p =>
                    p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))
                ?.Address.ToString();

            var configSettings = (new ConfigurationBuilder().SetBasePath(rootdir)
                .AddJsonFile("dbsettings.json", optional: false, reloadOnChange: true)).Build();
            //读取正式服配置文件
            if (runHost == configSettings["ProductServer"] + "")
            {
                builder = new ConfigurationBuilder().SetBasePath(rootdir).AddJsonFile("infrastructuresetting.json", optional: false, reloadOnChange: true);
            }
            //读取测试服配置文件
            else if (runHost == configSettings["TestServer"] + "")
            {
                builder = new ConfigurationBuilder().SetBasePath(rootdir).AddJsonFile("infrastructuresetting.debug.json", optional: false, reloadOnChange: true);
            }
            else
            {
                throw new Exception("当前服务器IP " + runHost + " 未在 dbsettings.json 配置文件中找到匹配");
            }
#endif

            var config = builder.Build();
            if (string.IsNullOrEmpty(connectString))
                _connectString = config["ConnetString"];

            ICacheService redisCache = new RedisCache(config["RedisConnection"]); //读取Redis连接串
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    //DataInfoCacheService = redisCache,               //二级缓存使用Redis
                    SerializeService = new SqlSugarSerializeService()  //使用自定义的序列化器,控制时间格式等
                },
                IsShardSameThread = true,              //设为true相同线程是同一个SqlConnection
                ConnectionString = _connectString,     //必填
                DbType = DbType.MySql,                 //必填
                IsAutoCloseConnection = true,          //默认false
                InitKeyType = InitKeyType.SystemTable, //从系统表读取主键信息
                MoreSettings = new ConnMoreSettings()
                {
                    //谨慎,在MySql中要设为false,该项只在Sql Server有效
                    IsWithNoLockQuery = false,         //为true表式查询的时候默认会加上.With(SqlWith.NoLock),可以用With(SqlWith.Null)让全局的失效
                    IsAutoRemoveDataCache = true       //为true表示可以自动删除二级缓存
                }
            });
            var excuteSql = string.Empty;
            db.Ado.IsEnableLogEvent = true;
            db.Ado.LogEventStarting = (sql, pars) =>
            {
                excuteSql = pars.Aggregate(sql, (current, parameter) => current.Replace(parameter.ParameterName, parameter.Value + ""));
            };

            //记录执行时间超过1分钟的Sql
            db.Aop.OnLogExecuted = (sql, pars) =>
            {
                var ts = db.Ado.SqlExecutionTime;
                if (ts.TotalMinutes > 1)
                {
                    excuteSql = pars.Aggregate(sql, (current, parameter) => current.Replace(parameter.ParameterName, parameter.Value + ""));
                    Log.Warn("Sql执行时间超过1分钟:" + excuteSql + "\n执行时间为:" + ts.TotalSeconds + "秒");
                }
            };

            //执行SQL 错误事件
            db.Aop.OnError = (exp) =>
            {
                var errorMessages = new StringBuilder();
                errorMessages.Append("ErrorSql:" + excuteSql + "\n");
                errorMessages.Append("InnerException:" + exp.InnerException + "\n");
                errorMessages.Append("StackTrace:" + exp.StackTrace + "\n");
                errorMessages.Append("TargetSite:" + exp.TargetSite + "\n");
                errorMessages.Append("Source:" + exp.Source + "\n");
                Log.Error(errorMessages.ToString());
            };
            return db;
        }
    }
}
