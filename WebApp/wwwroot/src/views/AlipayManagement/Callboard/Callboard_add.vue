<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">公告管理></a><a href="#">{{id?'修改公告':'添加公告'}}</a></div>
            <div class="fl">
                <el-button class="cx-back" @click="cancel" >返回</el-button>
            </div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
                <div class="add-page">
                    <div class="add-page-title">{{id?'修改公告':'添加公告'}}</div>
                    <el-form :model="adminForm" :rules="adminrules" ref="ruleForm" label-width="100px" class="demo-ruleForm">
                        <div class="gray-box-part">

                            <el-form-item label="公告对象" prop="Charging">
                                <el-radio-group v-model="adminForm.feeObj" >
                                    <el-radio  v-for="(item,index) in cardArr" :label="item.ID"  border @change="limitChange(item.card_show_name)">{{item.card_show_name}}</el-radio>
                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="公告群体">
                                <el-radio-group v-model="adminForm.feeRadio"  >
                                    <div :style="{display:activeIndex == '学生卡'?'block':'none'}">
                                        <div class="classchoose">
                                            <el-radio label="0">指定班级</el-radio>
                                            <el-cascader
                                                    style="margin-left: 10px;width: 360px;"
                                                    placeholder="请选择班级"
                                                    change-on-select
                                                    :options="classArr"
                                                    :props="props"
                                                    v-model="adminForm.classStr"
                                                    @change="handleChangemove">
                                            </el-cascader>
                                        </div>
                                    </div>
                                    <div :style="{display:activeIndex == '教师卡'?'block':'none'}">
                                        <div  class="classchoose">
                                         <el-radio label="1">所有老师</el-radio>
                                        </div>
                                    </div>
                                    <div :style="{display:activeIndex == '其他卡'?'block':'none'}">
                                        <div  class="classchoose">
                                            <el-radio label="2">其他用户</el-radio>
                                        </div>
                                    </div>
                                    <div :style="{display:activeIndex == '不限制'?'block':'none'}">
                                        <div  class="classchoose">
                                            <el-radio label="3">所有用户</el-radio>
                                        </div>
                                    </div>


                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="公告标题"  prop="noticeTitle">
                                <el-input placeholder="请输入公告标题" v-model="adminForm.noticeTitle"></el-input>
                            </el-form-item>
                            <el-form-item label="公告内容" prop="noticeContent">
                                <el-input type="textarea" placeholder="请输入公告内容" :autosize="{ minRows: 3, maxRows: 6}"  v-model="adminForm.noticeContent"></el-input>
                            </el-form-item>
                        </div>

                        <el-form-item>
                            <el-button class="btn-bg" @click="onSubmit('ruleForm')">{{id?'修改':'添加'}}</el-button>
                            <el-button class="btn-border" @click="cancel">取消</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Callboard_add",
        data() {
            return {
                classArr:[],
                cardArr:[],
                props: {
                    value: 'value',
                    children: 'children',
                    label: 'label',
                },
                id:'',
                activeIndex:'学生卡',
                adminForm: {
                    nteRadio:'',
                    noticeTitle:'',
                    noticeContent:'',
                    feeRadio:'0',
                    classStr:[],
                    feeObj: '',
                },
                adminrules:{
                    noticeTitle: [
                        {required: true, message: '请输入公告标题', trigger: 'blur'}
                    ],noticeContent: [
                        {required: true, message: '请输入公告内容', trigger: 'blur'}
                    ]
                }
            }
        },
        created() {
            let self=this;
            self.cardList();
            self.getParams();
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
            },
            limitChange(name){
                let self = this;
                self.activeIndex = name;
                if(name =='学生卡' ){
                    self.adminForm.feeObj = '0';
                    self.adminForm.feeRadio = '0';
                }else if(name =='教师卡'){
                     self.adminForm.feeObj = '1';
                    self.adminForm.feeRadio = '1';
                }else if(name =='其他卡'){
                    self.adminForm.feeObj = '2';
                    self.adminForm.feeRadio = '2';
                }else  if(name == '不限制'){
                    self.adminForm.feeObj = '3';
                    self.adminForm.feeRadio = '3';
                }
            },
            cardList(){
                let self = this;
                self.axios.get(`/api/SchoolUser/GetSchoolCardList`, {
                    params: {
                        School_ID: localStorage.schoolcode
                    }
                }).then(res => {

                    var list = res.data.data;
                    for(var i=0;i<list.length;i++){
                        if(list[i].card_show_name == '学生卡'){
                            self.cardArr.push({'ID':'0','card_show_name':'学生卡'});
                        }else if(list[i].card_show_name == '教师卡'){
                            self.cardArr.push({'ID':'1','card_show_name':'教师卡'});
                        }
                    }
                    self.cardArr.push({'ID':'2','card_show_name':'其他卡'});
                    self.cardArr.push({'ID':'3','card_show_name':'不限制'});
                    self.activeIndex= self.cardArr[0].card_show_name;
                    if(self.cardArr[0].card_show_name =='学生卡' ){
                        self.adminForm.feeObj = '0';
                        self.adminForm.feeRadio ='0';
                    }else if(self.cardArr[0].card_show_name =='教师卡'){
                        self.adminForm.feeObj = '1';
                        self.adminForm.feeRadio ='1';
                    }else if(self.cardArr[0].card_show_name =='其他卡'){
                        self.adminForm.feeObj = '2';
                        self.adminForm.feeRadio ='2';
                    }else  if(self.cardArr[0].card_show_name == '不限制'){
                        self.adminForm.feeObj = '3';
                        self.adminForm.feeRadio ='3';
                    }
                })
            },
            classListA(){
                let self = this;
                self.axios.get(`/api/SchoolDepartment/GetSchoolDepartmentCascader`, {
                    params: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        self.classArr.push(res.data.data);
                        self.classArr = self.getTreeData(self.classArr);
                    }
                })
            },
            getTreeData(data) {
                // 循环遍历json数据
                for (var i = 0; i < data.length; i++) {
                    if (data[i].children.length < 1) {
                        // children若为空数组，则将children设为undefined
                        data[i].children = undefined;
                    } else {
                        // children若不为空数组，则继续 递归调用 本方法
                        this.getTreeData(data[i].children);
                    }
                }
                return data;
            },
            handleChangemove(value) {
                console.log(value);
                this.adminForm.classStr = value;
            },
            onSubmit(ruleForm) {
                let self = this;
                if(self.adminForm.feeObj=='0'){
                    if(self.adminForm.classStr.length<=0){
                        self.$message({
                            showClose: true,
                            message: '请选择班级',
                            type: 'warning'
                        });
                        return false;
                    }
                }

                let json = {
                    id:self.id,
                    schoolcode: localStorage.schoolcode,
                    group:self.adminForm.feeObj,//公告群体
                    class_id:self.adminForm.feeObj=='0'?self.adminForm.classStr.join('/'):0,//班级id
                    title:self.adminForm.noticeTitle,  //公告标题
                    content:self.adminForm.noticeContent //公告内容
                };

                self.$refs[ruleForm].validate((valid) => {
                    if (valid) {
                        self.axios.post('api/SchoolNotice/AddOrUpdateSchoolNotice',json)
                            .then(function (response) {
                                if(response.data.code == '000000'){
                                    self.$router.push({
                                        path: '/Callboard'
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

            },
            bandData(id){
                //展示数据
                let self = this;
                if(id){
                    self.axios.get('api/SchoolNotice/GetSchoolNoticeToID', {
                        params: {
                            schoolcode: localStorage.schoolcode,
                            id:id,

                        }
                    })
                        .then(function (response) {
                            let res = response.data.data;
                            if(response.data.code == '000000'){

                                if(res.group == '0'){
                                    self.activeIndex = '学生卡';
                                    self.adminForm.classStr = response.data.department.split('/');
                                    self.adminForm.feeRadio = '0';
                                }else if(res.group == '1'){
                                    self.activeIndex = '教师卡';
                                    self.adminForm.feeRadio = '1';
                                }else if(res.group == '2'){
                                    self.activeIndex = '其他卡';
                                    self.adminForm.feeRadio = '2';
                                }else  if(res.group == '3'){
                                    self.activeIndex = '不限制';
                                    self.adminForm.feeRadio = '3';
                                }
                                self.adminForm.feeObj = (res.group).toString();
                                self.adminForm.noticeTitle = res.title;
                                self.adminForm.noticeContent =res.content;
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
            },
            cancel(){
                let self = this;
                self.$router.push({
                    path: '/Callboard',
                });
            }
        }


    }
</script>

<style scoped>

</style>