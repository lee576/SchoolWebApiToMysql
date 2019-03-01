<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">收款账号管理></a><a href="#">{{id?'修改收款账号':'添加收款账号'}}</a></div>
            <div class="fl">
                <el-button class="cx-back" @click="cancel" >返回</el-button>
            </div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
               <div class="add-page">
                    <div class="add-page-title">{{id?'修改收款账号':'添加收款账号'}}</div>
                    <el-form :model="accountForm" :rules="recerules" ref="accountForm" label-width="100px" class="demo-ruleForm">
                        <div class="gray-box-part">
                            <el-form-item label="收款名称" prop="name">
                                <el-input v-model="accountForm.name" placeholder="请输入应用私钥"></el-input>
                            </el-form-item>
                            <el-form-item label="PID" prop="pid">
                                <el-input v-model="accountForm.pid" placeholder="请输入PID"></el-input>
                            </el-form-item>
                            <el-form-item label="APPID" prop="appid">
                                <el-input v-model="accountForm.appid" placeholder="请输入APPID"></el-input>
                            </el-form-item>
                        </div>
                        <div class="gray-box-part">
                            <el-form-item label="应用私钥" prop="private_key">
                                <el-input type="textarea" placeholder="请输入应用私钥" v-model="accountForm.private_key"></el-input>
                            </el-form-item>
                            <el-form-item label="应用公钥" prop="privatekey">
                                <el-input type="textarea" placeholder="请输入应用公钥" v-model="accountForm.publickey"></el-input>
                            </el-form-item>
                            <el-form-item label="支付宝公钥" prop="publickey">
                                <el-input type="textarea" placeholder="请输入支付宝公钥" v-model="accountForm.alipay_public_key"></el-input>
                            </el-form-item>
                        </div>
                        <el-form-item>
                            <el-button class="btn-bg" @click="onSubmit('accountForm')">{{id?'修改':'添加'}}</el-button>
                            <el-button class="btn-border" @click="cancel" >取消</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "ReceivingManage_Add",
        data() {
            return {
                id:'',
                flag:null,
                accountForm: {
                    name:'',
                    pid:'',
                    appid:'',
                    private_key:'',
                    publickey:'',
                    alipay_public_key:''
                },recerules:{
                    name: [
                        {required: true, message: '请输入收款名称', trigger: 'blur'}
                    ],pid: [
                        {required: true, message: '请输入PID', trigger: 'blur'}
                    ],appid: [
                        {required: true, message: '请输入APPID', trigger: 'blur'}
                    ],private_key: [
                        {required: true, message: '请输入应用私钥', trigger: 'blur'}
                    ],publickey: [
                        {required: true, message: '请输入应用公钥', trigger: 'blur'}
                    ],alipay_public_key: [
                        {required: true, message: '请输入支付宝公钥', trigger: 'blur'}
                    ]
                }

            }
        },
        created(){
            this.getParams();
            this.bandData(this.id);
        },
        methods: {
            getParams(){
                // 取到路由带过来的参数
                let self = this;
                self.id = self.$route.query.id;
                self.flag = self.$route.query.flag;
            },
            onSubmit(ruleForm) {
                var self = this;
                const json = {
                    id:self.id,
                    schoolcode: localStorage.schoolcode,
                    pid: self.accountForm.pid,
                    appid: self.accountForm.appid,
                    name: self.accountForm.name,
                    private_key: self.accountForm.private_key,
                    publickey: self.accountForm.publickey,
                    alipay_public_key: self.accountForm.alipay_public_key,
                };
                if(self.id){
                    self.$refs[ruleForm].validate((valid) => {
                            if (valid) {
                                self.axios.post('api/PaymentAR/UpdatePaymentAccount',json)
                                    .then(function (response) {
                                        if(response.data.code == '000000'){
                                            self.$router.push({name: 'ReceivingManage_List'});
                                            self.$router.push({
                                                path: '/ReceivingManage_List',
                                                query: {
                                                    flag:self.flag,
                                                    type:1
                                                }
                                            });
                                            self.$message({
                                                showClose: true,
                                                message: response.data.msg,
                                                type: 'success'
                                            });
                                        }else {
                                            self.$message({
                                                showClose: true,
                                                message: response.data.msg,
                                                type: 'warning'
                                            });
                                        }
                                    })
                                    .catch(function (error) {
                                        console.log(error);
                                    });
                            }
                    });

                }else {
                    self.$refs[ruleForm].validate((valid) => {
                        if (valid) {
                            self.axios.post('api/PaymentAR/AddPaymentAccount',json)
                                .then(function (response) {
                                    if(response.data.code == '000000'){
                                        self.$router.push({
                                            path: '/ReceivingManage_List',
                                            query: {
                                                flag:self.flag,
                                                type:1
                                            }
                                        });
                                        self.$message({
                                            showClose: true,
                                            message: response.data.msg,
                                            type: 'success'
                                        });
                                    }else {
                                        self.$message({
                                            showClose: true,
                                            message: response.data.msg,
                                            type: 'warning'
                                        });
                                    }
                                })
                                .catch(function (error) {
                                    console.log(error);
                                });
                        }
                    });
                }
            },
            cancel(){
                this.$router.push({
                    path: '/ReceivingManage_List',
                    query: {
                        type:1,
                        flag:this.flag,

                    }});
            },handleChange(value) {
                console.log(value);
            },
            bandData(id){
                 //展示数据
                let self = this;
                if(id){
                    self.axios.get('api/PaymentAR/GetPaymentAccountToid', {
                        params: {
                            schoolcode: localStorage.schoolcode,
                            id:self.id,
                        }
                    })
                        .then(function (response) {
                            console.log(response);
                            let res = response.data;
                            if(res.code == '000000'){
                                self.accountForm.pid = res.data.pid;
                                self.accountForm.appid = res.data.appid;
                                self.accountForm.name = res.data.name;
                                self.accountForm.private_key = res.data.private_key;
                                self.accountForm.publickey = res.data.publickey;
                                self.accountForm.alipay_public_key = res.data.alipay_public_key;

                            }else {
                                self.$message({
                                    showClose: true,
                                    message: '获取数据失败',
                                    type: 'warning'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }
            }
        },

    }
</script>

<style scoped>

</style>