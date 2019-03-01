<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">常规缴费项目管理></a><a href="#">管理分类</a></div>
            <div class="fl">
                <router-link to="/RoutineAdmin" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab clearfix">
                <div class="fl">
                    <div class="fl search-input"><input type="text" v-model="payStr" class="se-input" placeholder="请输入收费项目"></div>
                    <div class="fl search-button"><button class="btn-pro" @click="search">搜索</button></div>
                    <div class="fl search-button"><button class="btn-pro" @click="reset">重置</button></div>
                </div>
                <div class="fr right-btn-ta">
                    <button class="btn" @click="userOption()">添加分类</button>
                </div>
            </div>
            <el-row :gutter="10">
                <div class="no-message-tip" :style="{display:isShowData?'block':'none'}">
                    <img src="../../../assets/picture/images/no-mess.png" height="100" width="100"/>
                    <div class="message-tip-word">暂无内容</div>
                </div>
                <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12"  v-for="(item, index) in configTable.tableArr">
                    <div class="pay-card">
                        <div class="clearfix">
                            <div class="fl">
                                <div class="clearfix">
                                    <div class=" fl">
                                        <img :src='"../../../assets/images/payIcon/choose-icon"+(item.icon?item.icon:1)+".png"' class="choose-logo-img">
                                    </div>
                                    <div class="fl pay-logo-word">{{item.name}}</div>
                                </div>
                                <div class="pay-money">{{item.introduction}}</div>
                            </div>
                            <div class="fr">
                                <div class="pay-person">前台显示状态</div>
                                <div class="pay-person-num">{{isShow(item.is_display)}}</div>
                            </div>
                        </div>
                        <div class="two-row-mg clearfix">
                            <div class="fl two-row-on1">校园卡</div>
                            <div class="fl two-row-on2">截止日期：{{item.create_time}}</div>
                            <div class="fr two-row-on3">
                                <el-button type="text" size="small"  class="edit-tr-con" @click="userOption(item.id)">修改</el-button>
                                <el-button type="text" size="small"  class="del-tr-con" @click="tableDelete(item.id)">删除</el-button>
                            </div>
                        </div>
                    </div>
                </el-col>
              <!--  <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                    <div class="pay-card">
                        <div class="clearfix">
                            <div class="fl">
                                <div class="clearfix">
                                    <div class="pay-logo-img2 fl"></div>
                                    <div class="fl pay-logo-word">水电费</div>
                                </div>
                                <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                            </div>
                            <div class="fr">
                                <div class="pay-person">前台显示状态</div>
                                <div class="pay-person-num">显示</div>
                            </div>
                        </div>
                        <div class="two-row-mg clearfix">
                            <div class="fl two-row-on1">校园卡</div>
                            <div class="fl two-row-on2">截止日期：2019-09-01</div>
                            <div class="fr two-row-on3">
                                <a href="#" class="row-color2">修改</a>
                                <a href="#" class="row-color3">删除</a>
                            </div>
                        </div>
                    </div>
                </el-col>
                <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                    <div class="pay-card">
                        <div class="clearfix">
                            <div class="fl">
                                <div class="clearfix">
                                    <div class="pay-logo-img3 fl"></div>
                                    <div class="fl pay-logo-word">图书馆</div>
                                </div>
                                <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                            </div>
                            <div class="fr">
                                <div class="pay-person">前台显示状态</div>
                                <div class="pay-person-num">不显示</div>
                            </div>
                        </div>
                        <div class="two-row-mg clearfix">
                            <div class="fl two-row-on1">校园卡</div>
                            <div class="fl two-row-on2">截止日期：2019-09-01</div>
                            <div class="fr two-row-on3">
                                <a href="#" class="row-color2">修改</a>
                                <a href="#" class="row-color3">删除</a>
                            </div>
                        </div>
                    </div>
                </el-col>-->
            </el-row>
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
        name: "RoutneAdmin_Check",
        data() {
            return {
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10
                },
                payStr:'',
                isShowData:false,
            }
        },
        created() {
            let self=this;
            self.tableList(self);
        },
        methods:{
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
                let self = this;
                self.configTable.iDisplayLength = val;
                self.tableList(self);
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
                let self = this;
                self.configTable.iDisplayStart = (val - 1)*self.configTable.iDisplayLength;
                self.tableList(self);
            },
            tableList(self){
                self.axios.get('api/PaymentItem/GetPaymentTypePageList', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        pgName:self.payStr
                    }
                })
                    .then(function (response) {

                        let res = response;
                        if(res.data.code == '000000'){
                            console.log(response);

                            if(res.data.aaData.length == 0){
                                self.isShowData = true;
                            }else {
                                self.isShowData = false;
                                self.configTable.tableArr = res.data.aaData;
                                self.configTable.total = res.data.iTotalRecords;
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
            isShow(type){
                switch (type) {
                    case 0:
                        return "显示";
                        break;
                    case 1:
                        return "不显示";
                        break;
                }
            },
            tableDelete(id){
                let self = this;

                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('/api/PaymentItem/DeletePaymentType', {
                        schoolcode: localStorage.schoolcode,
                        id: id
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.tableList(self);
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
                }).catch(() => {
                    self.$message({
                        type: 'info',
                        message: '已取消'
                    });
                });


            },
            userOption(id){
                var self = this;
                if(id){
                    self.$router.push({
                        path: '/RoutineAdmin_Check_add',
                        query: {
                            id: id
                        }
                    });
                }else {
                    self.$router.push({
                        path: '/RoutineAdmin_Check_add',
                    });
                }

            },
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList(this);
            },
            reset(){
                this.payStr = '';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList(this);
            }
        }
    }
</script>

<style scoped>

</style>