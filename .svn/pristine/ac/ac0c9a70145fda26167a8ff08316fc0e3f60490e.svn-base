<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">常规缴费项管理></a><a href="#">{{id?'修改收费项':'添加收费项'}}</a></div>
            <div class="fl">
                <el-button class="cx-back" @click="cancel" >返回</el-button>
            </div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
                <div class="add-page">
                    <div class="add-page-title">{{id?'修改收费项':'添加收费项'}}</div>
                    <el-form :model="adminForm" :rules="adminrules" ref="ruleForm" label-width="100px" class="demo-ruleForm">
                        <div class="choose-logo" >
                            <img @click="dialogVisible = true" :src='"../../../assets/images/payIcon/choose-icon"+slectIcon+".png"' class="choose-logo-img">
                            <div class="choose-logo-word">点击修改收费图标</div>
                        </div>
                        <div class="gray-box-part">
                            <el-form-item label="收款账号" prop="accountStr">
                                <el-select v-model="adminForm.accountStr" placeholder="请选择收款账号" @change="accountChoose">
                                    <el-option v-for="item in accountArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="收款分类" prop="payType">
                                <el-select v-model="adminForm.payType" placeholder="请选择收款分类" @change="payTypeChoose">
                                    <el-option v-for="item in payTypeArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="收费名称" prop="feeName" >
                                <el-input v-model="adminForm.feeName"  placeholder="请输入收费名称"></el-input>
                            </el-form-item>
                            <el-form-item label="收费介绍" prop="feeDesc">
                                <el-input type="textarea" v-model="adminForm.feeDesc" placeholder="请输入收费介绍"></el-input>
                            </el-form-item>
                        </div>
                        <div class="gray-box-part">
                            <el-form-item label="收费对象" prop="Charging">
                                <el-radio-group v-model="adminForm.feeRadio">
                                    <el-radio label="1"  border @change="limitChange">校园卡</el-radio>
                                    <el-radio label="0"  border @change="limitChange">不限制</el-radio>
                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="收费群体">
                                <el-radio-group v-model="adminForm.feeObj">
                                    <div :style="{display:radioFlag?'block':'none'}">
                                        <div  class="classchoose">
                                            <el-radio label="1"  @change="adminForm.classStr='';">所有电子校园卡用户</el-radio>
                                        </div>
                                        <div class="classchoose">
                                            <el-radio label="2">指定班级</el-radio>
                                            <el-select v-bind:disabled="adminForm.feeObj==2?false:true" v-model="adminForm.classStr" filterable placeholder="请选择班级分类" @change="classChooseType">
                                                <el-option v-for="item in classArr" :label="item.classname" :value="item.id" :key="item.id"></el-option>
                                            </el-select>
                                        </div>
                                    </div>
                                    <div :style="{display:!radioFlag?'block':'none'}">
                                        <div  class="classchoose">
                                            <el-radio label="0">所有用户</el-radio>
                                        </div>
                                    </div>
                                </el-radio-group>
                            </el-form-item>
                            <!--   <el-form-item label="收费群体"  :style="{display:!radioFlag?'block':'none'}">
                                   <el-radio-group v-model="adminForm.allUser">
                                       <div  class="classchoose">
                                           <el-radio label="0">所有用户</el-radio>
                                       </div>
                                   </el-radio-group>
                               </el-form-item>-->
                            <el-form-item label="收费次数" prop="feeAccount">
                                <el-input v-model="adminForm.feeAccount" placeholder="请输入收费次数"></el-input>
                            </el-form-item>
                            <el-form-item label="收费金额" prop="feeMoney">
                                <el-radio-group v-model="adminForm.radioMoney" >
                                    <div  class="classchoose">
                                        <el-radio label="0">用户自助填写</el-radio>
                                    </div>
                                    <div class="numchoose">
                                        <el-radio label="1" >定额</el-radio>
                                        <el-input-number v-bind:disabled="adminForm.radioMoney==1?false:true" v-model="adminForm.fixedFee" controls-position="right" @change="handleChange" :min="1" :max="10"></el-input-number>
                                    </div>
                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="所需信息" prop="info">
                                <el-checkbox-group
                                        v-model="adminForm.info">
                                    <el-checkbox v-for="(item, index) in infoArr" :label="index+1" :key="index">{{item}}</el-checkbox>
                                </el-checkbox-group>
                            </el-form-item>
                            <el-form-item label="缴费日期" prop="dateArr" class="form-timerange">
                                <el-date-picker
                                        v-model="adminForm.dateArr"
                                        type="daterange"
                                        range-separator="至"
                                        start-placeholder="开始日期"
                                        end-placeholder="结束日期" :picker-options="pickerOptions0">
                                </el-date-picker>
                            </el-form-item>
                        </div>

                        <div class="high-set" :class="[{ active: isActive }]"  @click="toggleClass(isActive)">
                                <span>
                                    <span class="high-set-word">高级设置</span>
                                    <!--//class="high-set-img" -->
                                    <span class="high-set-img" ></span>
                                </span>
                        </div>
                        <div class="gray-box-part" :style="{display:(isActive?'block':'none')}" >
                            <el-form-item label="通知链接" >
                                <el-input placeholder="请输入通知链接" v-model="adminForm.noticeLink"></el-input>
                            </el-form-item>
                            <el-form-item label="通知秘钥" >
                                <el-input type="textarea"  v-model="adminForm.noticeKey" placeholder="请输入通知附言"></el-input>
                            </el-form-item>
                            <el-form-item label="通知附言">
                                <el-input type="textarea" v-model="adminForm.noticeDetail" placeholder="请输入通知附言"></el-input>
                            </el-form-item>
                            <el-form-item label="备注项">
                                <el-input type="textarea" v-model="adminForm.remark" placeholder="请输入备注项"></el-input>
                            </el-form-item>
                        </div>
                        <el-form-item>
                            <el-button class="btn-bg" @click="onSubmit('ruleForm')">{{id?'修改':'添加'}}</el-button>
                            <el-button class="btn-border" @click="cancel" >取消</el-button>
                        </el-form-item>

                    </el-form>

                </div>
            </div>
        </div>

        <el-dialog title="图标修改" :visible.sync="dialogVisible" width="600px" top="20vh">

            <div class="edit-choose-logo">
                <img :src='"../../../assets/images/payIcon/choose-icon"+imageUrl+".png"' class="edit-logo-img">
                <div class="edit-logo-word">选中图标</div>
            </div>
            <ul class="edit-logo-box clearfix">
                <li  v-for="(item,index) in iconArr" :class="radio==index?'active':''"><img :src='"../../../assets/images/payIcon/choose-icon"+item+".png"' style="cursor: pointer;"  @click="changeIcon(item)" class="edit-logo"></li>
            </ul>
            <div slot="footer" class="dialog-footer diff-button-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="sureIcon" class="sure-btn">确 定</el-button>
            </div>

        </el-dialog>
    </div>
</template>

<script>
    export default {
        name: "Payment_add_kind",
        data() {
            return {
                infoArr:['学号','姓名','身份证号','手机号'],
                id:'',
                flag:null,
                accountArr:[],
                payTypeArr:[],
                classArr:[],
                radioFlag:true,
                radio:'0',
                imageUrl:1,
                slectIcon:'1',
                dialogVisible:false,
                isActive:false,
                iconArr:22,
                adminForm: {
                    accountStr: '',
                    payType:'',
                    feeName:'',
                    classStr:'',
                    feeDesc:'',
                    feeObj: '1',
                    feeAccount:'',
                    radioMoney:'1',
                    feeRadio:'1',
                    allUser:'0',
                    fixedFee:'1',
                    info: [],
                    dateArr:[],
                    noticeLink: '',
                    noticeKey:'',
                    noticeDetail:'',
                    remark:''

                },adminrules: {
                    accountStr: [
                        {required: true, message: '请输入收款账号', trigger: 'blur'}
                    ],payType: [
                        {required: true, message: '请选择收款分类', trigger: 'change'}
                    ], classStr: [
                        {required: true, message: '请选择班级分类', trigger: 'change'}
                    ], feeName: [
                        {required: true, message: '请输入收费名称', trigger: 'change'}
                    ],feeAccount: [
                        {required: true, message: '请输入收费次数', trigger: 'change'}
                    ],
                    feeDesc:[
                        {required: true, message: '请输入收费介绍', trigger: 'blur'}
                    ],info: [
                        { type: 'array', required: true, message: '请至少选择一个所需信息', trigger: 'change' }
                    ],dateArr:[
                        { required: true, message: '请选择缴费日期', trigger: 'change'}
                    ]
                },
                pickerOptions0: {
                    disabledDate(time) {
                        return time.getTime() < Date.now() - 8.64e7;
                    }
                },
            }
        },
        created() {
            let self=this;
            self.getParams();
            //账号列表
            self.accountList();
            //收款分类列表
            self.payTypeList();
            //班级列表
            self.classListA();
            if(self.id){
                //修改页面展示数据
                self.bandData(self.id);
            }

        },
        methods: {
            getParams(){
                // 取到路由带过来的参数
                this.id = this.$route.query.id;
                this.flag = this.$route.query.flag;
            },
            limitChange(){
                let self = this;
                if(self.radioFlag){
                    self.radioFlag=false;
                    self.adminForm.feeObj = '0';
                }else {
                    self.radioFlag=true;
                    self.adminForm.feeObj = '1';
                }

            },
            toggleClass(e){
                if(e){
                    this.isActive = false;
                }else{
                    this.isActive = true;
                }
            },
            accountList(){
                let self = this;
                self.axios.get('api/PaymentItem/GetPaymentAccountList', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.accountArr= res.data;
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
            },
            accountChoose(val){
                this.adminForm.accountStr = val;
            },
            payTypeList(){
                let self = this;
                self.axios.get('api/PaymentItem/GetPaymentTypeList', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.payTypeArr= res.data;
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
            },
            payTypeChoose(val){
                this.adminForm.payType = val;
            },
            classListA(){
                let self = this;
                self.axios.get('api/SchoolDepartment/GetSchoolClassInfoToSchoolcode', {
                    params: {
                        schoolcode: '55555',
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.classArr= res.data;
                        }else {
                            self.$message({
                                showClose: true,
                                message: res.msg,
                                type: 'warning'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            classChooseType(val){
                this.adminForm.classStr = val;
            },
            onSubmit(ruleForm) {
                let self = this,stime='',etime='';
                if(self.adminForm.dateArr.length>0){
                    stime = self.timeChange(self.adminForm.dateArr[0]);
                    etime = self.timeChange(self.adminForm.dateArr[1]);
                }
                let json = {
                    id:self.id,
                    is_external:0,
                    method:1,
                    count:1,
                    can_set_count:0,
                    status:0,
                    schoolcode: localStorage.schoolcode,
                    icon:self.slectIcon,//收费图标
                    account:self.adminForm.accountStr,//收费账户
                    type:self.adminForm.payType,//收费分类
                    name:self.adminForm.feeName,//收费名字
                    introduction:self.adminForm.feeDesc,//收费介绍
                    target:self.adminForm.feeRadio,//收费对象
                    group:self.adminForm.feeObj,//收费群体
                    class_id:self.adminForm.classStr,//班级id
                    limit:self.adminForm.feeAccount,//收费次数
                    fixed:self.adminForm.radioMoney,//收费金额选择
                    money:self.adminForm.fixedFee,//定额数
                    nessary_info:self.adminForm.info.join(','),//所需信息
                    date_from:stime,//起始时间
                    date_to:etime,//结束时间
                    notify_link:self.adminForm.noticeLink,
                    notify_msg:self.adminForm.noticeDetail,
                    notify_key:self.adminForm.noticeKey,
                    remark:self.adminForm.remark,
                };
                if(self.id){
                    self.$refs[ruleForm].validate((valid) => {
                        if (valid) {
                            self.axios.post('api/PaymentItem/UpdatePaymentItem',json)
                                .then(function (response) {
                                    if(response.data.code == '000000'){
                                        self.$router.push({
                                            path: '/RoutineAdmin',
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
                            self.axios.post('api/PaymentItem/AddPaymentItem',json)
                                .then(function (response) {
                                    if(response.data.code == '000000'){
                                        self.$router.push({
                                            path: '/RoutineAdmin',
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
                let self = this;
                self.$router.push({
                    path: '/RoutineAdmin',
                    query: {
                        flag:self.flag,
                        type:1
                    }
                });
            },handleChange(value) {
                console.log(value);
            },
            changeIcon(icon){
                console.log(icon)
                this.radio = icon -1;
                this.imageUrl=icon;
            },
            timeChange(time){
                var d = new Date(time);
                var datetime=d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
                return datetime
            },
            bandData(id){
                //展示数据
                let self = this;
                if(id){
                    self.axios.get('api/PaymentItem/GetPaymentItemToID', {
                        params: {
                            schoolcode: localStorage.schoolcode,
                            id:self.id,

                        }
                    })
                        .then(function (response) {

                            let res = response.data.data[0];
                            if(response.data.code == '000000'){
                                //图标
                                self.radio = res.icon - 1;
                                self.slectIcon = res.icon;
                                self.imageUrl =res.icon;
                                self.adminForm.accountStr = res.account;
                                self.adminForm.payType = res.type;
                                self.adminForm.feeName = res.name;
                                self.adminForm.feeDesc = res.introduction;
                                self.adminForm.feeRadio = res.target.toString();
                                self.adminForm.feeObj = res.group.toString();
                                self.adminForm.classStr = res.class_id;
                                self.adminForm.feeAccount = res.limit;
                                self.adminForm.radioMoney = res.fixed.toString();
                                self.adminForm.fixedFee = res.money;
                                var arr = res.nessary_info.split(',');
                                arr.forEach(function (item) {
                                    self.adminForm.info.push(parseInt(item));
                                });
                                self.adminForm.dateArr = [res.date_from,res.date_to];
                                self.adminForm.noticeLink = res.notify_link;
                                self.adminForm.noticeDetail = res.notify_msg;
                                self.adminForm.noticeKey = res.notify_key;
                                self.adminForm.remark = res.remark;
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
            },
            sureIcon(){
                this.dialogVisible = false;
                this.slectIcon=this.imageUrl;
            }

        }
    }
</script>

<style scoped>

</style>