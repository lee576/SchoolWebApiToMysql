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
                    <el-form :model="adminForm" :rules="adminrules" ref="ruleForm" label-width="100px"
                             class="demo-ruleForm">
                        <div class="gray-box-part">

                            <el-form-item label="姓名" prop="username" placeholder="请输入姓名">
                                <el-input v-model="adminForm.userName"></el-input>
                            </el-form-item>
                            <el-form-item label="登录账号" prop="account">
                                <el-input v-model="adminForm.loginuser" placeholder="请输入登录账号"></el-input>
                            </el-form-item>
                            <el-form-item label="登录密码" prop="password">
                                <el-input v-model="adminForm.password" placeholder="请输入登录密码"></el-input>
                            </el-form-item>
                            <el-form-item label="登录密码" prop="dlpassword">
                                <el-input v-model="adminForm.password" placeholder="请再次输入登录密码"></el-input>
                            </el-form-item>
                        </div>
                        <div class="gray-box-part">
                            <el-form-item label="角色管理" prop="userRadio">
                                <el-radio-group v-model="adminForm.roletype">
                                    <el-radio label=0>普通用户</el-radio>
                                    <el-radio label=1>食堂经理</el-radio>
                                    <el-radio label=2>财务</el-radio>
                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="食堂">
                                <el-select v-model="adminForm.canteen" placeholder="请选择食堂">
                                    <el-option label="全部" value="totel"></el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="权限管理" prop="privilege">
                                <el-checkbox-group
                                        v-model="adminForm.privilege">
                                    <el-checkbox label="电子校园卡" name="type"></el-checkbox>
                                    <el-checkbox label="缴费大厅" name="type"></el-checkbox>
                                    <el-checkbox label="收银台" name="type"></el-checkbox>
                                    <el-checkbox label="宿舍管理" name="type"></el-checkbox>
                                </el-checkbox-group>
                            </el-form-item>
                            <el-form-item label="备注项">
                                <el-input type="textarea" v-model="adminForm.remark" placeholder="请输入备注项"></el-input>
                            </el-form-item>
                        </div>
                        <el-form-item>
                            <el-button class="btn-bg">创建</el-button>
                            <el-button class="btn-border">取消</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "UserRightsEdit",
        data() {
            return {
                adminForm: {
                    userName: '',
                    loginuser: '',
                    password: '',
                    password: '',
                    roletype: '',
                    canteen: '',
                    privilege: [],
                    remark: ''
                },
                adminrules: {
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
                    ], privilege: [
                        {required: true, message: '请选择权限管理', trigger: 'blur'}
                    ]
                }
            }
        },
        created() {
            this.init()
        },
        methods: {
            init() {
                this.axios.get(`/api/UserRightMange/GetUserById`, {
                    params: {
                        id: this.$route.params.id
                    }
                }).then(res => {
                    console.log(res)
                    if (res.data.code="000000"){
                        this.adminForm=res.data.aaData
                    }
                })
            }
        }
    }
</script>

<style scoped>

</style>