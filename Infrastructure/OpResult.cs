﻿using System;
using System.Collections.Generic;
using System.Text;
using ServiceStack;

namespace Infrastructure
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public class OpResult : ExecResult<OpResultType, object>
    {
        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult() { ResultType = OpResultType.NoChanged; }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType)
            : this(resultType, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message)
            : this(resultType, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message, object data)
            : base(resultType, message, data)
        { }

        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool Successed { get { return ResultType == OpResultType.Success; } }

        /// <summary>
        /// 返回消息字段
        /// </summary>
        private string _message;

        /// <summary>
        /// 返回消息属性
        /// </summary>
        public override string Message
        {
            get { return string.IsNullOrEmpty(_message) ? ResultType.ToDescription() : _message; }
            set { _message = value; }
        }
    }

    /// <summary>
    /// 操作结果
    /// </summary>
    /// <typeparam name="TData">操作结果数据</typeparam>
    public class OpResult<TData> : ExecResult<OpResultType, TData>
    {
        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult() { ResultType = OpResultType.NoChanged; }

        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType)
            : this(resultType, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message)
            : this(resultType, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message, TData data)
            : base(resultType, message, data)
        { }

        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool Successed { get { return ResultType == OpResultType.Success; } }

        /// <summary>
        /// 返回消息字段
        /// </summary>
        private string _message;

        /// <summary>
        /// 返回消息属性
        /// </summary>
        public override string Message
        {
            get { return string.IsNullOrEmpty(_message) ? ResultType.ToDescription() : _message; }
            set { _message = value; }
        }
    }
}
