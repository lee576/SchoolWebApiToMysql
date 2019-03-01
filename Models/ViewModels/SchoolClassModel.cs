using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class SchoolClassModel
    {
        public int id { get; set; }
        public string schoolcode{ get; set; }
        public string Branchname { get; set; }
        public string Departmentname { get; set; }
        public string classname { get; set; }
    }
    public class SchoolDeparmentModel
    {
        public int id { get; set; }
        public string schoolcode { get; set; }
        public string BranchName { get; set; }
        public string Departmentname { get; set; }
        public string Jobname { get; set; }
    }
    public class DepartmentTree
    {
        public DepartmentTree(){
            children = new List<DepartmentTree>();
        }
        public string label { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int? p_id { get; set; }
        public string schoolcode { get; set; }
        public int? treeLever { get; set; }
        public bool? isType { get; set; }
        public List<DepartmentTree> children { get; set; }
        public int? classid { get; set; }
    }
    public class DepartmentTreeNode
    {
        public string label { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int? p_id { get; set; }
        public string schoolcode { get; set; }
        public int? treeLever { get; set; }
        public bool? isType { get; set; }
    }
    public class DepartmentCascader
    {
        public DepartmentCascader()
        {
            children = new List<DepartmentCascader>();
        }
        public int id { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public List<DepartmentCascader> children { get; set; }
    }
    public class DepartmentCascaderNode
    {
        public string label { get; set; }
        public string value { get; set; }
    }
    public class DepartmentCascaderNode_last
    {
        public int id { get; set; }
        public string label { get; set; }
        public string value { get; set; }
    }

}
