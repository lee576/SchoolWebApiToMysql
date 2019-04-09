<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">公告管理</a></div>
            <div class="fr payable-btn">
                <button class="operation-btn" @click="userOption()">添加公告</button>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab-box">
                <div class="nav-tab1 clearfix">
                   <!-- <div class="fl on-part-search1">
                        <div class="fl left-word-on1">发布时间</div>
                        <div class="fl form-timerange">
                            <el-date-picker
                                    v-model="dateArr"
                                    value-format="yyyy-MM-dd"
                                    type="daterange"
                                    range-separator="至"
                                    start-placeholder="开始日期"
                                    end-placeholder="结束日期" style="width:250px"
                                    :picker-options="pickerOptions2"
                                    @change="timeChoose">
                            </el-date-picker>
                        </div>
                    </div>-->
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">卡类型</div>
                        <div class="fl form-timerange">
                            <div class="fl form-timerange">
                                <el-select placeholder="请选择卡类型" style="width:180px" v-model="cardStr" @change="cardChoose">
                                    <el-option v-for="item in cardArr" :label="item.card_show_name" :value="item.ID" :key="item.ID" ></el-option>
                                </el-select>
                            </div>
                        </div>
                    </div>
                    <div class="fl on-part-search2">
                        <div class="fl search-input" style="width: 280px;" >
                            <input  type="text" placeholder="请输入标题" class="se-input" @keyup.enter="search" v-model="inputVal">
                        </div>
                        <div class="fl search-button"><button class="btn-pro" @click="search">搜索</button></div>
                        <div class="fl search-button"><button class="btn-pro" @click="reset">重置</button></div>
                    </div>
                </div>
            </div>
            <div style="margin-top: 10px">
                <div class="no-message-tip" :style="{display:isShow?'block':'none'}">
                    <img src="../../../assets/picture/images/no-mess.png" height="100" width="100"/>
                    <div class="message-tip-word">暂无内容</div>
                </div>
                <el-row :gutter="10">
                    <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12" v-for="(item, index) in configTable.tableArr">
                        <div class="pay-card">
                            <div class="pay-man-tell1">
                                公告标题 : {{item.title}}
                            </div>
                            <div class="pay-man-tell2">
                               公告内容 : {{item.content}}
                            </div>
                            <div class="clearfix">
                                <div class="fl pay-man-totel" style="font-size: 14px;">发布时间 : {{item.releasetime}}
                                </div>
                                <div class="fr two-row-on3">
                                    <el-button type="text" size="small" class="view-tr-con" @click="check(item)">查看</el-button>
                                    <el-button type="text" size="small" class="edit-tr-con" @click="userOption(item.id)">修改</el-button>
                                    <el-button type="text" size="small" class="del-tr-con" @click="deleteTr(item.id)">删除</el-button>
                                </div>
                            </div>

                        </div>
                    </el-col>
                </el-row>
            </div>
            <div class="block" style="text-align: right;">
                <el-pagination
                        @size-change="handleSizeChange"
                        @current-change="handleCurrentChange"
                        :current-page.sync="configTable.currentPage"
                        :page-size="configTable.iDisplayLength"
                        layout="total, prev, pager, next"
                        :total="configTable.total">
                </el-pagination>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Callboard",
        data() {
            return {
                isShow:false,
                dateArr:[],
                inputVal:'',
                cardStr:'',
                cardArr:[],
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },
                pickerOptions2: {
                    disabledDate(time) {
                        return time.getTime() > Date.now() - 8.64e6
                    },
                },
            }
        },
        created() {
            let self=this;
            //公告列表页
            self.tableList();
            //卡类型
            self.cardList();
        },
        methods: {
            tableList(){
                let self = this;
                self.axios.get('api/SchoolNotice/GetSchoolNotice', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        groupid:self.cardStr,
                        title:self.inputVal
                    }
                })
                    .then(function (response) {

                        let res = response.data;
                        console.log(res)
                        if(res.code == '000000'){
                            if(res.data.length == 0){
                                self.isShow = true;
                                self.configTable.tableArr = [];
                                self.configTable.total = 0;
                            }else {
                                self.isShow = false;
                                self.configTable.tableArr = res.data;
                                self.configTable.total = res.iTotalRecords;
                            }
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

                })
            },
            cardChoose(val){
                this.cardStr = val;
            },
            search(){
            //  搜索
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
            reset(){
            //    重置
                this.dateArr = [];
                this.cardStr = '';
                this.inputVal = '';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
                let self = this;
                self.configTable.iDisplayLength = val;
                self.tableList();
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
                let self = this;
                self.configTable.iDisplayStart = val;
                self.tableList();
            },
            deleteTr(id){
                let self = this;
                self.$confirm('此操作将永久删除该公告, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/SchoolNotice/DeleteSchoolNoticeToID', {
                        schoolcode: localStorage.schoolcode,
                        id: id
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.tableList();
                                self.$message({
                                    message:response.data.msg,
                                    type: 'success'
                                });

                                console.log(8888)
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
                }).catch(() => {
                    self.$message({
                        type: 'info',
                        message: '已取消'
                    });
                });
            },
            timeChoose(){

            },
            check(item){
                var self = this;
                self.$router.push({
                    path: '/Callboard_view',
                    query: {
                        parm: item,
                    }
                });

            },
            userOption(id){
                var self = this;
                if(id){
                    self.$router.push({
                        path: '/Callboard_add',
                        query: {
                            id: id,
                        }
                    });
                }else {
                    self.$router.push({
                        path: '/Callboard_add',
                    });
                }
            }
        }
    }
</script>

<style scoped>
.pay-man-tell1{margin-bottom: 10px;font-size: 18px;color: #707070;width:100%;overflow: hidden;text-overflow:ellipsis;white-space: nowrap;}
.pay-man-tell2{margin-bottom: 10px;font-size: 16px;color: #707070;width:100%;overflow: hidden;text-overflow:ellipsis;white-space: nowrap;}
</style>