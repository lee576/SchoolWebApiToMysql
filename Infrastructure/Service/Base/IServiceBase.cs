using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Service
{
    /// <summary>
    /// 仓储通用接口类
    /// </summary>
    /// <typeparam name="T">泛型实体类</typeparam>
    public interface IServiceBase<T> where T : class, new()
    {
        /// <summary>
        /// 取前N条
        /// </summary>
        /// <param name="topNum">前N条</param>
        /// <returns></returns>
        IEnumerable<T> Take(int topNum);

        /// <summary>
        /// 按条件取前N条
        /// </summary>
        /// <param name="topNum">前N条</param>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        IEnumerable<T> Take(int topNum, Expression<Func<T, bool>> predicate, string orderBy = "");

        /// <summary>
        /// 按条件取前N条
        /// </summary>
        /// <param name="topNum">前N条</param>
        /// <param name="whereExpression">条件表达式树</param>
        /// <param name="orderExpression">排序表达式树</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        IEnumerable<T> Take(int topNum, Expression<Func<T, bool>> whereExpression,
            Expression<Func<T, object>> orderExpression = null, OrderByType orderType = OrderByType.Asc);

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        bool Any(Expression<Func<T, bool>> whereLambda);

        /// <summary>
        /// 获得记录条数
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int Count(Expression<Func<T, bool>> whereLambda = null);

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        T FindById(object pkValue);

        /// <summary>
        /// 查询所有数据(无分页,请慎用)
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> FindAll();

        /// <summary>
        /// 查询所有数据并排序
        /// </summary>
        /// <param name="orderLambda"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        IEnumerable<T> FindListByOrder(Expression<Func<T, object>> orderLambda, OrderByType orderType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <returns>泛型实体集合</returns>
        IEnumerable<T> FindListByClause(Expression<Func<T, bool>> whereLambda, Expression<Func<T, object>> orderLambda,
            OrderByType orderType = OrderByType.Asc);

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <returns></returns>
        T FindByClause(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        long Insert(T entity);

        /// <summary>
        /// 批量写入实体数据
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        long Insert(List<T> entities);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        bool Update(Expression<Func<T, bool>> @where);

        /// <summary>
        /// 根据主键更新指定的列
        /// </summary>
        /// <param name="entity"></param>
        /// <param name=""></param>
        /// <returns></returns>
        bool UpdateColumnsById(T entity, Expression<Func<T, object>> columns);

        /// <summary>
        /// 根据表达式中的列更新，指定列并赋值的更新
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        bool UpdateColumnsByConditon(Expression<Func<T, T>> columns, Expression<Func<T, bool>> expression);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        bool Delete(T entity);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> @where);

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteByIds(object[] ids);

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        IPagedList<T> FindPagedList(Expression<Func<T, bool>> predicate = null, string orderBy = "", int pageIndex = 1, int pageSize = 20);

        /// <summary>
        /// 根据查找条件分页数据并可以排序
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex">>当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IPagedList<T> FindPagedListOrderyType(Expression<Func<T, bool>> predicate = null, int pageIndex = 1,
            int pageSize = 20, Expression<Func<T, object>> expression = null, OrderByType type = OrderByType.Asc);
    }
}
