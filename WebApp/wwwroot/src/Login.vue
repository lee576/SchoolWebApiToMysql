<template>
    <div id="id">
        <div class="logo-login"><img src="./assets/picture/images/logo-login.png"></div>
        <div class="logo-login-left"><img src="./assets/picture/images/left-bg.png"></div>
        <div class="logo-login-right"><img src="./assets/picture/images/right-bg.png"></div>
        <div class="copyright">Copyright © 2018 - 2019 若火版权所有</div>
        <div class="login-main-box">
            <div class="logoicon"><img src="./assets/picture/images/logoicon.png"></div>
            <div class="logo-word"><img src="./assets/picture/images/word.png"></div>
            <div class="welcome-login">欢迎登录</div>
            <div class="login-ul"></div>
            <el-form :model="ruleForm" :rules="rules" ref="ruleForm" class="demo-ruleForm">
                <el-form-item prop="user">
                    <el-input placeholder="请输入账号" v-model="ruleForm.user">
                        <template slot="prepend">
                            <img src="./assets/picture/images/user.png" alt="" class="img-lef-logo">
                        </template>
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input placeholder="请输入密码" type="password" v-model="ruleForm.password">
                        <template slot="prepend">
                            <img src="./assets/picture/images/password.png" alt="" class="img-lef-logo">
                        </template>
                    </el-input>
                </el-form-item>
                <el-form-item prop="coding">
                    <el-input placeholder="请输入校园编码" type="age" v-model.number="ruleForm.coding">
                        <template slot="prepend">
                            <img src="./assets/picture/images/yaoqingma.png" alt="" class="img-lef-logo">
                        </template>
                    </el-input>
                </el-form-item>
                <el-form-item class="code-box" prop="inputInfo">
                    <el-input placeholder="请输入验证码" type="text" v-model="ruleForm.inputInfo"
                              style="width:205px;" @keyup.enter.native="submitForm('ruleForm')">
                        <template slot="prepend">
                            <img src="./assets/picture/images/yanzhengma.png" alt="" class="img-lef-logo">
                        </template>
                    </el-input>
                    <span class="code-style" @click="createCode">{{verificationCode}}</span>
                    <!--<span class="confirm-botton" @click="confirmTheCode">验证</span>-->
                </el-form-item>

                <el-form-item class="loading-btn">
                    <el-button type="primary" @click="submitForm('ruleForm')"
                               @keyup.enter.native="submitForm('ruleForm')">登录
                    </el-button>
                </el-form-item>
            </el-form>
        </div>
    </div>
</template>

<script>
    import md5 from 'js-md5'

    export default {
        name: "Login",
        data() {
            return {
                ruleForm: {
                    user: '',
                    password: '',
                    coding: '',
                    inputInfo: '',//用户输入信息
                },

                verificationCode: '',  //生成的验证码

                rules: {
                    user: [
                        {required: true, message: '请输入账号', trigger: 'blur'},
                    ],
                    password: [
                        {required: true, message: '请输入密码', trigger: 'blur'},
                    ],
                    coding: [
                        {required: true, message: '请输入校园编码', trigger: 'blur'},
                        {type: 'number', message: '校园编码必须为数字值', trigger: 'blur'}
                    ],
                    inputInfo: [
                        {required: true, message: '请输入验证码', trigger: 'blur'},
                    ],
                },
                MDpassword: ""
            }

        },
        created() {
        },
        methods: {
            createCode: function () {    //通过随机数生成验证码
                this.verificationCode = '';
                var code = '';
                var codeLength = 4;     //验证码长度
                var random = new Array(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z');
                for (var i = 0; i < codeLength; i++) {
                    var index = Math.floor(Math.random() * 36);
                    code += random[index];
                }
                this.verificationCode = code
            },
            confirmTheCode: function () {      //验证函数

            },
            // CalcuMD5(pwd) {
            //     pwd = pwd.toUpperCase();
            //     pwd = md5(pwd);
            //     this.MDpassword
            // },
            submitForm(formName) {
                // this.CalcuMD5(this.ruleForm.password)
                const data = {
                    "username": this.ruleForm.user,
                    "password": md5(this.ruleForm.password),
                    "schoolCode": this.ruleForm.coding,
                }
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        var customerCode = this.ruleForm.inputInfo.toUpperCase();   //把你输入的小写转化为大写
                        if (customerCode == 0) {
                            this.createCode();
                            this.inputInfo = ''
                            rules: {
                                console.log(123)
                            }

                        } else if (customerCode != this.verificationCode) {
                            this.createCode();
                            this.inputInfo = ''
                            rules: {
                                console.log(456)
                                this.$message.error('验证码错误');
                            }

                        } else {
                            rules: {
                                console.log(789)
                                console.log(data)
                                console.log(data.password)

                                if(this.ruleForm.user=="admin"){
                                    this.axios.post('api/Authorize/Token', data, {
                                        headers: {
                                            'Content-Type': 'application/json'
                                        }
                                    }).then(res => {
                                        console.log(res)
                                        if (res.data.code == "000000") { //如果成功了
                                            localStorage.token = res.data.token; //存入localStorage
                                            localStorage.schoolcode = res.data.schoolcode; //存入localStorage
                                            this.$router.push({name: "Home"})
                                            localStorage.loginuser = res.data.loginuser

                                            localStorage.menus = res.data.menus
                                            if (res.data.dining_talls == null) {
                                                localStorage.dining_talls = ""
                                            } else {
                                                localStorage.dining_talls = res.data.dining_talls
                                            }
                                            if (res.data.receivables == null) {
                                                localStorage.receivables = ""
                                            } else {
                                                localStorage.receivables = res.data.receivables
                                            }
                                            console.log(this.ruleForm.user)
                                        } else if (res.data.code == "111111") {
                                            this.$refs[formName].resetFields();
                                            this.createCode()
                                            this.$message.error(res.data.msg);
                                        }
                                    })
                                }else{
                                    this.$alert('目前排队人员较多，请稍后再试', '提示', {
                                        confirmButtonText: '确定',
                                    });
                                    this.$refs[formName].resetFields();
                                    this.createCode()
                                }
                            }
                        }
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
        },
        mounted() {
            this.createCode()
        }
    }
</script>

<style scoped lang="scss">
    .img-lef-logo {
        padding: 7px 0;
        width: 20px;
    }

    .demo-ruleForm {
        width: 300px;
        margin: 0 auto;
    }

    .demo-ruleForm {
        .el-input__inner {
            height: 40px !important;
            line-height: 40px !important;
            padding: 0 15px !important;
        }
    }

    .code-box {
        display: flex;
    }

    .code-style {
        text-align: center;
        display: inline-block;
        width: 60px;
        border: 1px solid #2387fb;
        padding: 0 10px;
        border-radius: 5px;
        margin-left: 10px;
        height: 38px;
        line-height: 38px;
        color: #2387fb;
        font-size: 17px;
    }

    .logo-login-left img {
        position: absolute;
        bottom: 0px;
        left: 80px;
        width: 280px;
        display: block;
    }

    .logo-login-right img {
        position: absolute;
        bottom: 0px;
        right: 80px;
        width: 280px;
        display: block;
    }

    .copyright {
        position: absolute;
        left: 0px;
        bottom: 20px;
        width: 100%;
        text-align: center;
        color: rgba(112, 112, 112, 0.5);
    }

    .login-main-box {
        width: 300px;
    }

    .logoicon, .logo-word {
        text-align: center
    }

    .logoicon img {
        width: 80px;
    }

    .logo-word img {
        width: 150px;
    }

    .welcome-login {
        text-align: center;
        color: #707070;
        font-weight: bold;
        font-size: 18px;
        margin-bottom: 20px;
    }

    .logo-login {
        position: absolute;
        top: 25px;
        left: 80px;
    }

    .logo-word {
        text-align: center;
        margin-top: 60px;
        margin-bottom: 30px;
    }

    .logo-login img {
        width: 220px
    }

    .loading-btn button {
        width: 300px;
        height: 40px;
        background: #2387FB;
        color: #fff;
        border-radius: 25px;
        margin-top: 20px;
        border: none;
        font-size: 16px;
    }

    .loading-btn button:hover {
        background: #66b1ff;
    }

    @media screen and (min-height: 800px) {
        .login-main-box {
            padding: 7% 0;
            margin: 0 auto;
        }
    }

    @media screen and (max-height: 800px) {
        .login-main-box {
            padding: 2% 0;
            margin: 0 auto;
        }
    }

    .code-style {
        float: right;
        cursor: pointer
    }
</style>