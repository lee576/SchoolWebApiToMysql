<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">用户权限管理></a><a href="#">添加权限</a></div>
            <div class="fl">
                <router-link to="/UserRights" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
                <div class="add-page">
                    <div class="add-page-title">添加权限</div>
                    <el-form :model="adminForm" :rules="adminrules" ref="adminForm" label-width="100px"
                             class="demo-ruleForm">
                        <div class="gray-box-part">
                            <el-form-item label="姓名" prop="username">
                                <el-input v-model="adminForm.username" placeholder="请输入姓名"></el-input>
                            </el-form-item>
                            <el-form-item label="登录账号" prop="account">
                                <el-input v-model="adminForm.account" placeholder="请输入登录账号"></el-input>
                            </el-form-item>
                            <el-form-item label="登录密码" prop="password">
                                <el-input v-model="adminForm.password" placeholder="请输入登录密码"></el-input>
                            </el-form-item>
                            <el-form-item label="确认密码" prop="dlpassword">
                                <el-input v-model="adminForm.dlpassword" placeholder="请再次输入登录密码"></el-input>
                            </el-form-item>
                        </div>
                        <div class="gray-box-part">
                            <el-form-item label="角色管理" prop="userRadio">
                                <el-radio-group v-model="adminForm.userRadio" @change="changeshow">
                                    <el-radio label=0>普通用户</el-radio>
                                    <el-radio label=1>食堂经理</el-radio>
                                    <el-radio label=2>财务</el-radio>
                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="收款分类" v-show="receipt">
                                <el-select v-model="adminForm.receivables" placeholder="请选择收款分类">
                                    <el-option
                                            v-for="item in introductions"
                                            :key="item.id"
                                            :label="item.name"
                                            :value="item.id">
                                    </el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="食堂" v-show="show">
                                <el-select v-model="adminForm.canteen" placeholder="请选择食堂">
                                    <el-option
                                            v-for="item in options"
                                            :key="item.id"
                                            :label="item.name"
                                            :value="item.id">
                                    </el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="权限管理">
                                <el-tree
                                        class="tree_data"
                                        :data="treedata"
                                        show-checkbox
                                        node-key="id"
                                        ref="tree"
                                        :props="defaultProps"
                                        style="width: 250px;height: 300px;"
                                >
                                </el-tree>
                            </el-form-item>
                            <el-form-item label="备注项" placeholder="请输入备注项">
                                <el-input type="textarea" v-model="adminForm.useDesc"></el-input>
                            </el-form-item>
                        </div>
                        <el-form-item>
                            <el-button class="btn-bg" @click="submitForm('adminForm')">创建</el-button>
                            <el-button class="btn-border" @click="resetForm('adminForm')">取消</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "UserRightsAdd",
        data() {
            return {
                adminForm: {
                    username: '',
                    account: '',
                    password: '',
                    dlpassword: '',
                    userRadio: '0',
                    receivables: '',
                    canteen: '',
                    useDesc: ''
                }, adminrules: {
                    username: [
                        {required: true, message: '请输入姓名', trigger: 'blur'}
                    ], account: [
                        {required: true, message: '请输入登录账号', trigger: 'blur'}
                    ], password: [
                        {required: true, message: '请输入登录密码', trigger: 'blur'}
                    ], dlpassword: [
                        {required: true, message: '请再次输入登录密码', trigger: 'blur'}
                    ], userRadio: [
                        {required: true, message: '请选择角色管理', trigger: 'blur'}
                    ]
                },
                show: false,
                receipt: true,
                alldata: [],//合并树形数据
                treedata: [],//树形数据
                options: [],//食堂数据
                introductions: [],//收款分类
                checked: false,
                defaultProps: {
                    children: 'children',
                    label: 'label',
                },
            }
        },
        created() {
            this.axios.get(`/api/Cashier/GetDinningHall`, {
                params: {
                    school_id: localStorage.schoolcode
                }
            }).then(res => {
                console.log(res)
                this.options = res.data.data
            })
            this.axios.get(`/api/PaymentItem/GetPaymentTypeList`, {
                params: {
                    schoolcode: localStorage.schoolcode
                }
            }).then(res => {
                console.log(res)
                this.introductions = res.data.data
            })
            this.init()
        },
        methods: {
            init() {
                this.axios.get(`/api/UserRightMange/GetMenus`).then(res => {
                    console.log(res)
                    if (res.data.code = "000000") {
                        res.data.Data.map(item => {
                            this.alldata.push(item)
                        })
                        res.data.Datas.map(list => {
                            this.alldata.push(list)
                        })
                        console.log(this.alldata)
                        this.toTreeData(this.alldata, 'id', 'p_id', 'title')
                        console.log(this.treedata)
                    }
                })
            },
            // 循环出父节点
            toTreeData(data, id, pid, name) {
                // 建立个树形结构,需要定义个最顶层的父节点，pId是1
                let leve = 0
                let parent = [];
                this.treedata = parent
                for (let i = 0; i < data.length; i++) {
                    if (data[i][pid] !== 0) {
                    } else {
                        let obj = {
                            label: data[i][name],
                            id: data[i][id],
                            parent_id: data[i][pid],
                            leve: leve,
                            children: []
                        };
                        parent.push(obj);//数组加数组值
                    }
                    // console.log(obj);
                    //  console.log(parent,"bnm");
                }
                children(parent, leve);

                // 调用子节点方法,参数为父节点的数组
                function children(parent, num) {
                    num++;
                    if (data.length !== 0 && num != 4) {
                        for (let i = 0; i < parent.length; i++) {
                            for (let j = 0; j < data.length; j++) {
                                if (parent[i].id == data[j][pid]) {
                                    let obj = {
                                        label: data[j][name],
                                        id: data[j][id],
                                        parent_id: data[j][pid],
                                        leve: num,
                                        children: []
                                    };
                                    parent[i].children.push(obj);
                                }
                            }
                            children(parent[i].children, num);
                        }
                    }
                    return num
                }

                console.log(parent, "bjil")
                return parent;
            },

            changeshow() {
                console.log(this.adminForm.userRadio)
                if (this.adminForm.userRadio == 1) {
                    this.show = true
                } else {
                    this.show = false
                }
                if (this.adminForm.userRadio == 0) {
                    this.receipt = true
                } else {
                    this.receipt = false
                }
            },

            submitForm(formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid && this.adminForm.password == this.adminForm.dlpassword) {
                        let arr = []
                        this.$refs.tree.getCheckedKeys().map(item => {
                            arr.push(item)
                        })
                        this.$refs.tree.getHalfCheckedNodes().map(list => {
                            arr.push(list.id)
                        })
                        console.log(arr)
                        const adduser = {
                            "schoolcode": localStorage.schoolcode,
                            "userName": this.adminForm.username,
                            "loginuser": this.adminForm.account,
                            "password": this.adminForm.password,
                            "roletype": this.adminForm.userRadio,
                            "receivables": this.adminForm.receivables,
                            "dining_talls": this.adminForm.canteen,
                            "menus": arr.join(','),
                            "remark": this.adminForm.useDesc
                        }
                        console.log(adduser)
                        this.axios.post(`/api/UserRightMange/AddUser`, adduser).then(res => {
                            console.log(res)
                            if (res.data.code == "000000") {
                                this.$message({
                                    message: res.data.msg,
                                    type: 'success'
                                });
                                this.$router.push({name: "UserRights"})
                            } else {
                                this.$message.error(res.data.msg);
                            }
                        })
                    } else if (valid && this.adminForm.password != this.adminForm.dlpassword) {
                        this.$message.error('两次密码不一样');
                    } else {
                        this.$message.error('表单输入有误');
                        return false;
                    }
                });
            },
            resetForm(formName) {
                this.$refs[formName].resetFields();
                this.$router.push({name: "UserRights"})
            }
        }
    }
</script>

<style scoped>
    el-tree .el-tree-node__content {
        border-bottom: 0 !important;
    }

    .tree_data {
        overflow: scroll
    }

    .tree_data::-webkit-scrollbar {
        display: none
    }

</style>