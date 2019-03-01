<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">常规缴费项管理</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn" @click="userOption()">添加收费项</button>
            </div>
        </div>
        <div class="page-content">
            <el-row :gutter="10">
                <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                    <div class="nav-tab clearfix">
                        <ul class="nav-tab-change1 fl">
                            <li :class="{active:isShowGraph}" @click="isShowGraph=true">
                                <label class="fl view-img1"></label>
                                <span class="fl">视图</span>
                            </li>
                            <li :class="{active:!isShowGraph}" @click="isShowGraph=false">
                                <label class="fl view-img3"></label>
                                <span class="fl">列表</span>
                            </li>
                        </ul>
                        <div class="fl">
                            <div class="fl search-input"><input type="text" class="se-input" v-model="feeName" @keyup.enter="search"  placeholder="请输入收费名称"></div>
                            <div class="fl search-button"><button class="btn-pro" @click="search()">搜索</button></div>
                            <div class="fl search-button"><button class="btn-pro" @click="reset()">重置</button></div>
                        </div>
                        <div class="fr right-btn-ta">
                                <router-link to="/RoutineAdmin_Check"><button class="btn">管理分类</button></router-link>
                        </div>
                    </div>
                </el-col>
            </el-row>
            <ul class="view-tree-box">
                <!--视图-->
                <li class="ym-view-box" :style="{display:isShowGraph?'block':'none'}">
                    <div class="no-message-tip" :style="{display:isShow?'block':'none'}" >
                        <img src="../../../assets/picture/images/no-mess.png" height="100" width="100"/>
                        <div class="message-tip-word">暂无内容</div>
                    </div>
                    <el-row :gutter="10">
                        <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12" v-for="(item, index) in configTable.tableArr">
                            <div class="pay-card on-pa-card" :class="{grayCard:dateComp(item.status) | isStop(item.isstop)}" >
                                <div class="clearfix">
                                    <div class="fl">
                                        <div class="clearfix">
                                            <div class="fl">
                                                <img :src='"../../../assets/images/payIcon/choose-icon"+item.icon+".png"' height="40" width="40"/>
                                            </div>
                                            <div class="fl pay-logo-word">{{item.name}}</div>
                                        </div>
                                        <div class="pay-money">应缴金额 :<span class="pay-money-bold">{{ item.money | currency('￥') }}</span></div>
                                    </div>
                                    <div class="fr">
                                        <div class="pay-person">缴费人数</div>
                                        <div class="pay-person-num">{{ item.count | currency('',false) }}</div>
                                    </div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl two-row-on1">{{targetType(item.target)}}</div>
                                    <div class="fl two-row-on2">截止日期：{{item.dateto | dateSubstr() }}</div>
                                    <div class="fr two-row-on3">
                                        <el-button type="text" size="small"   class="view-tr-con" @click="viewDetail(item)">查看</el-button>
                                        <el-button type="text" size="small" :style="{display:dateComp(item.status)?'none':'inline-block'}"  class="edit-tr-con" @click="userOption(item.id)">修改</el-button>
                                        <el-button type="text" size="small" :style="{display:dateComp(item.status)?'none':'inline-block'}" class="del-tr-con" @click="tableDelete(item.id)">删除</el-button>
                                        <el-button type="text" size="small" :style="{display:dateComp(item.status)?'none':'inline-block'}" class="stop-tr-con" @click="stopUser(item.id,item.isstop)">{{!isStop(item.isstop)?'暂停':'开启'}}</el-button>
                                    </div>
                                </div>
                                <img class="cx-stop" :style="{display:dateComp(item.status)?'block':'none'}" :src='"../../../assets/images/timeout.png"' height="60" width="60"/>
                                <img class="cx-stop" :style="{display:isStop(item.isstop)?'block':'none'}" :src='"../../../assets/images/stop.png"' height="60" width="60"/>
                            </div>
                        </el-col>
                       <!-- <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                            <div class="pay-card gray-card">
                                <div class="clearfix">
                                    <div class="fl">
                                        <div class="clearfix">
                                            <div class="pay-logo-img1 fl"></div>
                                            <div class="fl pay-logo-word">爱心捐赠</div>
                                        </div>
                                        <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                                    </div>
                                    <div class="fr">
                                        <div class="pay-person">缴费人数</div>
                                        <div class="pay-person-num">3689</div>
                                    </div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl two-row-on1">校园卡</div>
                                    <div class="fl two-row-on2">截止日期：2019-09-01</div>
                                    <div class="fr two-row-on3">
                                        <a href="javascript:void(0);">
                                            <router-link to="/PaymentDetail" class="row-color1">查看</router-link>
                                        </a>
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
                                            <div class="pay-logo-img2 fl"></div>
                                            <div class="fl pay-logo-word">电费</div>
                                        </div>
                                        <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                                    </div>
                                    <div class="fr">
                                        <div class="pay-person">缴费人数</div>
                                        <div class="pay-person-num">3689</div>
                                    </div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl two-row-on1">校园卡</div>
                                    <div class="fl two-row-on2">截止日期：2019-09-01</div>
                                    <div class="fr two-row-on3">
                                        <a href="javascript:void(0);">
                                            <router-link to="/PaymentDetail" class="row-color1">查看</router-link>
                                        </a>
                                        <a href="#" class="row-color2">修改</a>
                                        <a href="#" class="row-color3">删除</a>
                                    </div>
                                </div>
                            </div>
                        </el-col>
                        <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                            <div class="pay-card gray-card">
                                <div class="clearfix">
                                    <div class="fl">
                                        <div class="clearfix">
                                            <div class="pay-logo-img2 fl"></div>
                                            <div class="fl pay-logo-word">电费</div>
                                        </div>
                                        <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                                    </div>
                                    <div class="fr">
                                        <div class="pay-person">缴费人数</div>
                                        <div class="pay-person-num">3689</div>
                                    </div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl two-row-on1">校园卡</div>
                                    <div class="fl two-row-on2">截止日期：2019-09-01</div>
                                    <div class="fr two-row-on3">
                                        <a href="javascript:void(0);">
                                            <router-link to="/PaymentDetail" class="row-color1">查看</router-link>
                                        </a>
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
                                            <div class="fl pay-logo-word">借阅费</div>
                                        </div>
                                        <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                                    </div>
                                    <div class="fr">
                                        <div class="pay-person">缴费人数</div>
                                        <div class="pay-person-num">3689</div>
                                    </div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl two-row-on1">校园卡</div>
                                    <div class="fl two-row-on2">截止日期：2019-09-01</div>
                                    <div class="fr two-row-on3">
                                        <a href="javascript:void(0);">
                                            <router-link to="/PaymentDetail" class="row-color1">查看</router-link>
                                        </a>
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
                                            <div class="pay-logo-img4 fl"></div>
                                            <div class="fl pay-logo-word">书本费</div>
                                        </div>
                                        <div class="pay-money">收费金额 :<span class="pay-money-bold">￥100.00</span></div>
                                    </div>
                                    <div class="fr">
                                        <div class="pay-person">缴费人数</div>
                                        <div class="pay-person-num">3689</div>
                                    </div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl two-row-on1">校园卡</div>
                                    <div class="fl two-row-on2">截止日期：2019-09-01</div>
                                    <div class="fr two-row-on3">
                                        <a href="javascript:void(0);">
                                            <router-link to="/PaymentDetail" class="row-color1">查看</router-link>
                                        </a>
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
                </li>
                <!--树状图-->
                <li class="ym-tree-box" :style="{display:isShowGraph?'none':'block'}">
                    <div class="payment-item-table">
                        <div class="table-box">
                            <template>
                                <el-table :data="configTable.tableArr" stripe style="width: 100%">
                                    <el-table-column prop="name" label="收费名称" >
                                    </el-table-column>
                                    <el-table-column prop="target" :formatter="targetTypeTable"  label="收费对象">
                                    </el-table-column>
                                    <el-table-column prop="money" :formatter="moneyTable" label="应缴金额">
                                    </el-table-column>
                                    <el-table-column prop="dateto" :formatter="dateSubstrTable" label="收费截止日期">
                                    </el-table-column>
                                    <el-table-column prop="tname" label="分类">
                                    </el-table-column>
                                    <el-table-column prop="account" label="收款账号">
                                    </el-table-column>
                                    <el-table-column label="收款状态">
                                        <template slot-scope="scope">
                                           {{accountStatus(scope.row)}}
                                        </template>
                                    </el-table-column>
                                    <el-table-column prop="count" :formatter="amountTable"  label="当前收款人数">
                                    </el-table-column>
                                    <el-table-column  label="操作" width="200">
                                        <template slot-scope="scope">
                                            <el-button type="text" size="small"   class="view-tr-con" @click="viewDetail(scope.row)">查看</el-button>
                                            <el-button type="text" size="small" :style="{display:dateComp(scope.row.status)?'none':'inline-block'}"  class="edit-tr-con" @click="userOption(scope.row.id)">修改</el-button>
                                            <el-button type="text" size="small" :style="{display:dateComp(scope.row.status)?'none':'inline-block'}" class="del-tr-con" @click="tableDelete(scope.row.id)">删除</el-button>
                                            <el-button type="text" size="small" :style="{display:dateComp(scope.row.status)?'none':'inline-block'}" class="stop-tr-con" @click="stopUser(scope.row.id,scope.row.isstop)">{{!isStop(scope.row.isstop)?'暂停':'开启'}}</el-button>
                                        </template>
                                    </el-table-column>
                                </el-table>
                            </template>
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

                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    import { currency } from './../../../util/currency'
    import { getDateType } from './../../../util/getDate'
    import { dateSubstr } from './../../../util/getDate'
    export default {
        name: "Payment_item_list",
        data() {
            return {
                // isStop:false,
                isShowGraph:true,
                isShow:false,
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10
                },
                feeName:''
            }
        },
        filters:{
            currency: currency,
            dateSubstr: dateSubstr,
            getDateType:getDateType,
        },
        created() {
            let self=this;
            self.getParams();
            self.paregrahList(self);
        },
        mounted(){
            console.log(2222);
        },
        methods: {
            getParams(){
                // 取到路由带过来的参数
                    let self = this;
                    if(self.$route.query.type){
                        self.isShowGraph = self.$route.query.flag;
                    }
            },
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
                let self = this;
                self.configTable.iDisplayLength = val;
                self.paregrahList(self);
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
                let self = this;
                self.configTable.iDisplayStart = (val - 1)*self.configTable.iDisplayLength;
                self.paregrahList(self);
            },
            paregrahList(self){
                self.axios.get('api/PaymentItem/GetPaymentlist', {
                    params: {
                        jfName:self.feeName,
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            if(res.data.length == 0){
                                self.isShow = true;
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
            targetType(type){
                switch (type) {
                    case '0':
                        return "不限制";
                        break;
                    case '1':
                        return "校园卡";
                        break;
                }
            },
            viewDetail(item){
                this.$router.push({name: 'PaymentDetail', params: {list:item}})
            },
            dateSubstrTable:function(row, column){
                var date = row.dateto;
                if (date == undefined) {
                    return "";
                }
                return date.split(' ')[0];
            },
            targetTypeTable:function(row, column){
                //收费对象
               return this.targetType(row.target);
            },
            feeStateTable:function(row, column){
                //收费状态
                if(row.status == '0'){
                    return "已开启";
                }else {
                    return "已关闭";
                }
            },
            amountTable:function(row, column){
                //人数
                return  currency(row.count,'',false);
            },
            moneyTable:function(row, column){
                //金额
               return  currency(row.money,'￥');
            },
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.paregrahList(this);
            },
            tableDelete(id){
                this.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.axios.post('api/PaymentItem/DeletePaymentItem', {
                        schoolcode: localStorage.schoolcode,
                        id: id
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'success'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消'
                    });
                });  //一定别忘了这个
            },
            userOption(id){

                var self = this;
                if(id){
                    self.$router.push({
                        path: '/RoutineAdmin_Add',
                        query: {
                            id: id,
                            flag:self.isShowGraph
                        }
                    });
                }else {
                    self.$router.push({
                        path: '/RoutineAdmin_Add',
                        query: {
                            flag:self.isShowGraph
                        }
                    });
                }

            },
            dateComp(val){
                if(val == '1'){
                    return true;
                }else{
                    return false;
                }
            },
            stopUser(id,isStop){
                let self = this;
                let status = 0;
                if(isStop == '0'){
                    status = 1;
                }
                self.$confirm('是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/PaymentItem/UpdatePaymentItemStatus', {
                        status: status,
                        id: id
                    })
                        .then(function (response) {
                            if(response.data.code == "000000"){
                                self.paregrahList(self);
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'success'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消'
                    });
                });
            },
            isStop(val){
                if(val == '1'){
                    return true;
                }else {
                    return false;
                }
            },
            accountStatus(row){
                if(row.status == '0'){
                    if(row.isstop == '0'){
                        return '已开启';
                    }else {
                        return '已暂停';
                    }
                }else {
                  return  '已关闭';
                }
            },
            reset(){
                this.feeName = '';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.paregrahList(this);
            }
        },
    }
</script>

<style scoped>
.fee-img{
    width: 40px;
}
</style>