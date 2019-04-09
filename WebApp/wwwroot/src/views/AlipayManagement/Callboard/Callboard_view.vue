<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">公告管理></a><a href="#">公告详情</a></div>
            <div class="fl">
                <el-button class="cx-back" @click="cancel" >返回</el-button>
            </div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
                <div class="add-page">
                    <div >

                    </div>
                    <div class="add-page-title">公告详情</div>
                    <div class="gray-box-part" style="padding-bottom: 10px">
                        <div class="call-li clearfix">
                            <div class="fl left-label">公告对象：</div>
                            <div class="fl right-cont cx-blue"  v-if="group=='0'">学生卡</div>
                            <div class="fl right-cont cx-blue"  v-else-if="group=='1'">教师卡</div>
                            <div class="fl right-cont cx-blue"  v-else-if="group=='2'">其他卡</div>
                            <div class="fl right-cont cx-blue"  v-else>不限制</div>
                        </div>
                        <div class="call-li clearfix">
                            <div class="fl left-label">公告群体：</div>
                            <div class="fl right-cont" v-if="group=='0'">班级 : {{class_id}} </div>
                            <div class="fl right-cont"  v-else-if="group=='1'">所有教师卡用户</div>
                            <div class="fl right-cont"  v-else-if="group=='2'">其他卡用户</div>
                            <div class="fl right-cont"  v-else>所有人</div>
                        </div>
                        <div class="call-li clearfix">
                            <div class="fl left-label">公告标题：</div>
                            <div class="fl right-cont">{{noticeTitle}}</div>
                        </div>
                        <div class="call-li clearfix">
                            <div class="fl left-label">公告内容：</div>
                            <div class="fl right-cont">{{noticeContent}}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Callboard_view",
        data(){
            return {
                group:'',//公告群体
                class_id:'',//班级id
                noticeTitle:'',  //公告标题
                noticeContent:'' //公告内容
            }
        },
        created() {
            let self=this;
            //会传参数
            self.getParams();

        },
        methods: {
            getParams(){
                // 取到路由带过来的参数
                let self = this;
                self.bandData(self.$route.query.parm.id);
            },
            handleChangemove(value) {
                console.log(value);
                this.classStr = value;
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
                                self.group = res.group;
                                self.class_id = response.data.departmentName;
                                self.noticeTitle = res.title;
                                self.noticeContent =res.content;
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
        },
    }
</script>

<style scoped>
.cx-blue{
    color:#2387fb;
}
</style>