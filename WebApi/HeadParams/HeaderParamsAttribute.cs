using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolWebApi.HeadParams
{
    /// <summary>
    /// 
    /// </summary>
    public class HeaderParamsAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] ParametersName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametersName"></param>
        public HeaderParamsAttribute(params string[] parametersName)
        {
            ParametersName = parametersName;
        }
    }
}
