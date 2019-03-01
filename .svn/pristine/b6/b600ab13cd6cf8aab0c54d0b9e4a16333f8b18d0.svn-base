<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">校园卡人员管理></a><a href="#">修改用户</a></div>
            <div class="fl">
                <router-link to="/CardPersonnel" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="clearfix">
                <div class="col-xs-12 col-sm-12 col-md-12 add-page-box">
                    <div class="add-page">
                        <div class="add-page-title">修改用户</div>
                        <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="100px"
                                 class="demo-ruleForm">
                            <div class="gray-box-part">
                                <el-form-item label="卡类型" prop="ID">
                                    <el-select v-model="ruleForm.ID" placeholder="请选择卡类型" @change="ChangeType">
                                        <el-option
                                                v-for="Type in Types"
                                                :key="Type.ID"
                                                :label="Type.card_show_name"
                                                :value="Type.ID">
                                        </el-option>
                                    </el-select>
                                </el-form-item>
                                <el-form-item label="学工号" prop="student_id" placeholder="请输入学工号">
                                    <el-input v-model="ruleForm.student_id"></el-input>
                                </el-form-item>
                                <el-form-item label="用户姓名" prop="user_name" placeholder="请输入用户姓名">
                                    <el-input v-model="ruleForm.user_name"></el-input>
                                </el-form-item>
                                <el-form-item label="身份证号" prop="passport" placeholder="请输入身份证号">
                                    <el-input v-model="ruleForm.passport"></el-input>
                                </el-form-item>
                            </div>

                            <div class="gray-box-part">
                                <el-form-item label="身份" prop="class_id">
                                    <el-radio-group v-model="ruleForm.class_id" @change="classArr">
                                        <el-radio :label=1>学生</el-radio>
                                        <el-radio :label=2>老师</el-radio>
                                    </el-radio-group>
                                </el-form-item>
                                <el-form-item label="班级" prop="department_id">
                                    <el-select v-model="ruleForm.department_id" filterable placeholder="请选择班级">
                                        <el-option
                                                v-for="Class in Classes"
                                                :key="Class.id"
                                                :label="Class.classname"
                                                :value="Class.id">
                                        </el-option>
                                    </el-select>
                                </el-form-item>
                                <el-form-item label="卡有效期" prop="card_validity">
                                    <el-date-picker
                                            v-model="ruleForm.card_validity"
                                            type="date"
                                            @change="getSTime"
                                            format="yyyy-MM-dd"
                                            value-format="yyyy-MM-dd 00:00:00"
                                            placeholder="选择日期">
                                    </el-date-picker>
                                </el-form-item>
                                <el-form-item label="是否迎新" prop="welcome_flg">
                                    <el-radio-group v-model="ruleForm.welcome_flg">
                                        <el-radio :label=0>迎新</el-radio>
                                        <el-radio :label=1>不迎新</el-radio>
                                    </el-radio-group>
                                </el-form-item>
                            </div>

                            <el-form-item>
                                <el-button class="btn-bg" @click="onSubmit('ruleForm')">修改</el-button>
                                <el-button class="btn-border" @click="cancel">取消</el-button>
                            </el-form-item>

                        </el-form>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Campus_user_add",

        data() {
            let idCard = (rule, value, callback) => {
                if (value && (!(/\d{17}[\d|x]|\d{15}/).test(value) || (value.length !== 15 && value.length !== 18))) {
                    callback(new Error('身份证号码不符合规范'))
                } else {
                    callback()
                }
            }
            return {
                Types: [],//卡类型
                Classes: [],//班级
                ruleForm: {
                    ID: '',
                    student_id: '',
                    user_name: '',
                    passport: '',
                    class_id:'',
                    department_id: '',
                    card_validity: '',
                    welcome_flg: ''
                },
                rules: {
                    user_name: [
                        {required: true, message: '请输入姓名', trigger: 'blur'},
                    ], student_id: [
                        {required: true, message: '请输入学工号', trigger: 'blur'},
                        {min: 3, max: 20, message: '长度在 3 到 20 个字符', trigger: 'blur'}
                    ], passport: [
                        {required: true, message: '请输入身份证号', trigger: 'blur'},
                        {validator: idCard, trigger: 'blur'}
                    ], ID: [
                        {required: true, message: '请选择卡类型', trigger: 'change'}
                    ], card_validity: [
                        {required: true, message: '请选择卡有限期', trigger: 'change'}
                    ], class_id: [
                        {required: true, message: '请选择身份', trigger: 'change'}
                    ], department_id: [
                        {required: true, message: '请选择班级', trigger: 'change'}
                    ], welcome_flg: [
                        {required: true, message: '请选择是否迎新', trigger: 'change'}
                    ],

                }
            }
        },
        created() {
            let id = this.$route.params.id
            this.axios.get(`/api/SchoolUser/GetSchoolUserInfoByid`, {
                params: {
                    user_id: id
                }
            }).then(res => {
                console.log(res)
                this.ruleForm = res.data.aaData
                if (this.ruleForm.ID == 1) {
                    this.axios.get(`/api/SchoolDepartment/GetSchoolClassInfoToSchoolcode`, {
                        params: {
                            schoolcode: localStorage.schoolcode
                        }
                    }).then(res => {
                        if (res.data.code == "000000") {
                            this.Classes = res.data.data
                            console.log(res)
                        }
                    })
                } else {
                    this.axios.get(`/api/SchoolDepartment/GetSchoolDeparmentInfoToSchoolcode`, {
                        params: {
                            schoolcode: localStorage.schoolcode
                        }
                    }).then(res => {
                        if (res.data.code == "000000") {
                            this.Classes = res.data.data
                            console.log(res)
                        }
                    })
                }
            })

            this.axios.get(`/api/SchoolUser/GetSchoolCardList`, {
                params: {
                    School_ID: localStorage.schoolcode
                }
            }).then(res => {
                this.Types = res.data.data
                // console.log(this.Types)
            })

        },
        methods: {
            ChangeType() {
                if (this.ruleForm.ID == 1) {
                    this.axios.get(`/api/SchoolDepartment/GetSchoolClassInfoToSchoolcode`, {
                        params: {
                            schoolcode: localStorage.schoolcode
                        }
                    }).then(res => {
                        if (res.data.code == "000000") {
                            this.Classes = res.data.data
                            console.log(res)
                        }
                    })
                } else {
                    this.axios.get(`/api/SchoolDepartment/GetSchoolDeparmentInfoToSchoolcode`, {
                        params: {
                            schoolcode: localStorage.schoolcode
                        }
                    }).then(res => {
                        if (res.data.code == "000000") {
                            this.Classes = res.data.data
                            console.log(res)
                        }
                    })
                }
            },
            getSTime(val) {
                this.ruleForm.card_validity = val;//这个sTime是在data中声明的，也就是v-model绑定的值
            },

            //身份关联班级
            classArr(){
                if (this.ruleForm.class_id == 1) {
                    this.axios.get(`/api/SchoolDepartment/GetSchoolClassInfoToSchoolcode`, {
                        params: {
                            schoolcode: localStorage.schoolcode
                        }
                    }).then(res => {
                        if (res.data.code == "000000") {
                            this.Classes = res.data.data
                            console.log(res)
                        }
                    })
                } else {
                    this.axios.get(`/api/SchoolDepartment/GetSchoolDeparmentInfoToSchoolcode`, {
                        params: {
                            schoolcode: localStorage.schoolcode
                        }
                    }).then(res => {
                        if (res.data.code == "000000") {
                            this.Classes = res.data.data
                            console.log(res)
                        }
                    })
                }
            },
            onSubmit(formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        const adddata = {
                            userId: this.$route.params.id,
                            schoolcode: localStorage.schoolcode,
                            class_id: this.ruleForm.class_id,
                            ID: this.ruleForm.ID,
                            card_id: this.ruleForm.ID,
                            student_id: this.ruleForm.student_id,
                            user_name: this.ruleForm.user_name,
                            passport: this.ruleForm.passport,
                            department_id: this.ruleForm.department_id,
                            card_validity: this.ruleForm.card_validity,
                            welcome_flg: this.ruleForm.welcome_flg
                        }
                        console.log(adddata)
                        this.axios.post(`/api/SchoolUser/AddOrUpdateSchoolUserInfo`, adddata).then(res => {
                            console.log(res)
                            if (res.data.code == "000000") {
                                this.$message({
                                    message: '修改成功',
                                    type: 'success'
                                });
                                this.$router.push({path: '/CardPersonnel'});
                            } else {
                                this.$message.error(res.data.msg);
                            }
                        })
                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
            },
            cancel() {
                this.$router.push({path: '/CardPersonnel'});
            }
        }
    }
</script>

<style lang="scss" scoped>

</style>