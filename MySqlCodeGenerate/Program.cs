using System;
using System.IO;
using System.Reflection;
using System.Text;
using SqlSugar;


namespace MySqlCodeGenerate
{
    public class Program
    {
        static void Main(string[] args)
        {
            string assemblyCodeBase =
                Assembly.GetEntryAssembly().Location;
            string dirName = Path.GetDirectoryName(assemblyCodeBase);
            var path = FindBin(dirName);
            var projectRoot = Directory.GetParent(path).FullName;

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=192.168.1.4;uid=root;pwd=root;database=alischoolcard;",
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            });
            //生成模型
            db.DbFirst.CreateClassFile(projectRoot.Replace(@"\", @"/") + "/Models/DbModels", "DbModel");


            //生成数据库IService,Service
            var tableInfoList = db.Utilities.TranslateCopy(db.DbMaintenance.GetTableInfoList());
            foreach (DbTableInfo tabInfo in tableInfoList)
            {
                var seChar = Path.DirectorySeparatorChar.ToString();
                var tableName = tabInfo.Name;

                //生成IService
                string IServiceContent =
                    @"using DbModel;
using Infrastructure.Service;
namespace IService
{
    public interface I" + tableName + @"Service" + @" : IServiceBase<" + tableName + @">" + @"
    {
	       
    }
}";
                var fileIServicePath = (projectRoot.Replace(@"\", @"/") + "/IService").TrimEnd('\\').TrimEnd('/') +
                                       string.Format(seChar + "{0}.cs", "I" + tableName + "Service");
                FileHelper.CreateFile(fileIServicePath, IServiceContent, Encoding.UTF8);

                //生成Service
                string ServiceContent =
                    @"using DbModel;
using IService;
using Infrastructure.Service;
namespace Service
{
    public class " + tableName + @"Service" + @" : GenericService<" + tableName + @">,I" + tableName + @"Service" + @"
    {

    }
}";
                var fileServicePath = (projectRoot.Replace(@"\", @"/") + "/Service").TrimEnd('\\').TrimEnd('/') +
                                      string.Format(seChar + "{0}.cs", tableName + "Service");
                FileHelper.CreateFile(fileServicePath, ServiceContent, Encoding.UTF8);
            }
        }

        //找到当前dll的Bin的父文件夹
        public static string FindBin(string path)
        {
            var root = Directory.GetParent(path);
            var rootPath = root.FullName;
            var findBin = path.Substring(path.LastIndexOf(@"\", StringComparison.Ordinal) + 1,
                path.Length - path.LastIndexOf(@"\", StringComparison.Ordinal) - 1);
            if (findBin.ToUpper() != "BIN")
            {
                FindBin(rootPath);
            }
            return Directory.GetParent(Directory.GetParent(rootPath).FullName).FullName;
        }
    }
}
