<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">应缴款项></a><a href="#">{{ detailArr.name }}</a></div>
            <div class="fl">
                <router-link to="/PayableManagement" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="gray-box-border clearfix">

                    <div class="yin-yo1 fl">
                        <div class="ymy-on1" style="font-size: 16px;color: #323232;">缴费名称 : {{ detailArr.name }}</div>
                        <div class="ymy-on2" style="color: #9e9e9e" >缴费批号 : {{ detailArr.ARID }}</div>
                    </div>
                    <div class="yin-yo2 fl">
                        <div class="ymy-on3 fl">
                            <div class="ym-se1">应缴笔数</div>
                            <div class="ym-se2">应缴金额</div>
                        </div>
                        <div class="ymy-on4 fl">
                            <div class="ym-se3">{{ detailArr.arcount | currency('',false) }}</div>
                            <div class="ym-se4">{{ detailArr.amount| currency('￥') }}</div>
                        </div>
                    </div>
                    <div class="yin-yo3 fl">
                        <div class="ymy-on3 fl">
                            <div class="ym-se1">实缴笔数</div>
                            <div class="ym-se2">实缴金额</div>
                        </div>
                        <div class="ymy-on4 fl">
                            <div class="ym-se3">{{ detailArr.fact_count | currency('',false) }}</div>
                            <div class="ym-se4">{{ detailArr.fact_amount| currency('￥') }}</div>
                        </div>
                    </div>
            </div>


            <div class="three-top-box clearfix">
                <div class="mg-t10 clearfix">
                    <ul class="fl tog-ji">
                        <li  :class="{active:isActive}" @click="paymentTotal">已缴费统计</li>
                        <li :class="{active:!isActive}" @click="paymentTotal">未缴费统计</li>
                    </ul>
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">缴费时间</div>
                        <div class="fl form-timerange">
                            <el-date-picker
                                    v-model="dateArr"
                                    value-format="yyyy-MM-dd"
                                    type="daterange"
                                    range-separator="至"
                                    start-placeholder="开始日期"
                                    end-placeholder="结束日期"
                                    :picker-options="pickerOptions2"
                                    style="width:250px">
                            </el-date-picker>
                        </div>
                    </div>
                  <!--  <div class="fl on-part-search1">
                        <div class="fl left-word-on1">缴费项</div>
                        <div class="fl form-timerange">
                            <el-select placeholder="请选择缴费项" style="width:200px" v-model="paymentStr"  @change="paymentChoose">
                                <el-option v-for="item in paymentArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                            </el-select>
                        </div>
                    </div>-->
                </div>
                <div class="clearfix">
                    <div class="fl search-input" style="width:400px">
                        <input  type="text" :placeholder="isActive?'订单编号、学工号、付款姓名、身份证号或手机号进行搜索':'学工号、付款姓名、身份证号或手机号进行搜索'" class="se-input" v-model="inputStr"  @keyup.enter="search">
                    </div>
                    <div class="fl search-button"><button class="btn-pro"  @click="search">搜索</button></div>
                    <div class="fl search-button"><button class="btn-pro"  @click="reset">重置</button></div>
                </div>
            </div>

            <div class="payment-item-table" :style="{display:isActive?'block':'none'}">
                <div class="table-box">
                    <template>

                        <el-table :data="configTable.tableArr" stripe style="width: 100%">
                            <el-table-column   prop="out_order_no" label="订单编号" >
                            </el-table-column>
                            <el-table-column prop="name" label="缴费项目">
                            </el-table-column>
                            <el-table-column prop="amount" :formatter="moneyTable" label="缴费金额">
                            </el-table-column>
                            <el-table-column prop="fact_amount" :formatter="moneyTotalTable" label="实缴金额">
                            </el-table-column>
                            <el-table-column   label="学工号">
                                <template slot-scope="scope">
                                    {{scope.row.stundetid?(scope.row.stundetid):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column  label="付款姓名">
                                <template slot-scope="scope">
                                    {{scope.row.st_name?(scope.row.st_name):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column  label="身份证号">
                                <template slot-scope="scope">
                                    {{scope.row.passport?(scope.row.passport):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column label="收款PID">
                                <template slot-scope="scope">
                                    {{scope.row.pid?(scope.row.pid):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column    prop="pay_time" label="缴费时间" width="110px">
                            </el-table-column>
                            <el-table-column prop="status" :formatter="feeStateTable" label="缴费状态">
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
            <div class="payment-item-table" :style="{display:isActive?'none':'block'}">
                <div class="table-box" >
                    <template>
                        <el-table :data="configTable2.tableArr"  @row-click="handleRowCurrentChange" class="tb-edit"stripe style="width: 100%">
                            <el-table-column prop="name" label="缴费项目">
                            </el-table-column>
                            <el-table-column prop="amount" :formatter="moneyTable" label="缴费金额">
                            </el-table-column>
                            <el-table-column  label="学工号">
                                <template slot-scope="scope">
                                    <el-input size="small" v-model="scope.row.stundetid" placeholder="请输入学工号" @change="handleEdit(scope.$index, scope.row)"></el-input> <span> {{scope.row.stundetid?(scope.row.stundetid):'暂无'}}</span>

                                </template>
                            </el-table-column>
                            <el-table-column label="付款姓名">
                                <template slot-scope="scope">
                                    {{scope.row.st_name?(scope.row.st_name):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column label="身份证号">
                                <template slot-scope="scope">
                                    {{scope.row.passport?(scope.row.passport):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column  label="收款PID">
                                <template slot-scope="scope">
                                    {{scope.row.pid?(scope.row.pid):'暂无'}}
                                </template>
                            </el-table-column>
                            <el-table-column prop="status" :formatter="feeStateTable" label="缴费状态">
                            </el-table-column>
                            <el-table-column  label="操作" width="120">
                                <template slot-scope="scope">
                                    <el-button type="text" size="small" class="del-tr-con" @click="tableDelete(scope.row.id)">删除</el-button>
                                </template>
                            </el-table-column>
                        </el-table>
                    </template>
                </div>
                <div class="block" style="text-align: right;">
                    <el-pagination
                            @size-change="handleSizeChange2"
                            @current-change="handleCurrentChange2"
                            :current-page.sync="configTable2.currentPage"
                            :page-size="configTable2.iDisplayLength"
                            layout="total, prev, pager, next"
                            :total="configTable2.total">
                    </el-pagination>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import { currency } from './../../../util/currency'
    import { getDateType } from './../../../util/getDate'
    export default {
        name: "ManagementDetail",
        data() {
            return {
                flag:false,
                detailArr:{},
                isActive:true,
                dateArr:[],
                paymentArr:[],
                paymentStr:'',
                inputStr:'',
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },
                configTable2:{
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
        filters:{
            currency: currency,
            getDateType:getDateType
        },
        created() {
            let self=this;
            self.getParams();
            //列表
             self.tableList();
           self.payTableList();
         /*   let body = document.querySelector('body')
            body.addEventListener('click',(e)=>{
                console.log(e)
                // debugger;
                // console.log(e.target.id === 'toggler-icon')
                if(e.target.id == 'toggler'){
                    this.flag = true;
                    debugger;
                }else {
                    debugger;
                    this.flag = false;
                }
            },false)*/

        },
        methods: {
            getParams(){
                // 取到路由带过来的参数
                this.detailArr = this.$route.params.list;
            },
            //已交
            tableList(){
                let self = this,stime = '',etime = '',status = '';
                if(self.isActive){
                    status = '1';
                }else {
                    status = '0';
                }
                if(self.dateArr){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/PaymentAR/GetPayment_ARisPayment', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        sTime:stime,
                        eTime:etime,
                        ARID:self.detailArr.ARID,
                        JSstatus:status,
                        name:self.inputStr,
                        itemid:self.paymentStr,

                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.configTable.tableArr = res.data;
                            self.configTable.total = res.iTotalRecords;
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
            //未交
            payTableList(){
                let self = this,stime = '',etime = '',status = '';
                if(self.isActive){
                    status = '1';
                }else {
                    status = '0';
                }
                if(self.dateArr){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/PaymentAR/GetPayment_ARisPayment', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable2.currentPage,
                        iDisplayStart:self.configTable2.iDisplayStart,
                        iDisplayLength:self.configTable2.iDisplayLength,
                        sTime:stime,
                        eTime:etime,
                        ARID:self.detailArr.ARID,
                        JSstatus:status,
                        name:self.inputStr,
                        itemid:self.paymentStr,

                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.configTable2.tableArr = res.data;
                            self.configTable2.total = res.iTotalRecords;
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
            paymentChoose(val){
                this.paymentStr = val;
            },
            moneyTable:function(row, column){
                //金额
                return  currency(row.amount,'￥');
            },
            moneyTotalTable:function(row, column){
                //金额
                return  currency(row.fact_amount,'￥');
            },
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
                this.configTable2.iDisplayStart = 0;
                this.configTable2.currentPage = 1;
                this.payTableList();
            },
            feeStateTable:function(row, column){
                //收费状态
                if(row.status == 1){
                    return "已缴费";
                }else {
                    return "未缴费";
                }

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
                self.configTable.iDisplayStart = (val - 1)*self.configTable.iDisplayLength;
                self.tableList();
            },
            handleSizeChange2(val) {
                console.log(`每页 ${val} 条`);
                let self = this;
                self.configTable2.iDisplayLength = val;
                self.payTableList();
            },
            handleCurrentChange2(val) {
                console.log(`当前页: ${val}`);
                let self = this;
                self.configTable2.iDisplayStart = (val - 1)*self.configTable2.iDisplayLength;
                self.payTableList();
            },
            paymentTotal(){
                if(this.isActive){
                    this.isActive = false;
                    this.payTableList();
                }else {
                    this.isActive = true;
                    this.tableList();
                }

            },
            reset(){
                this.isActive = true;
                this.dateArr = [];
                this.inputStr = '';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
                this.configTable2.iDisplayStart = 0;
                this.configTable2.currentPage = 1;
                this.payTableList();
            },
            //未交费删除
            tableDelete(id){
                let self = this;
                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/PaymentAR/DeletePayment_arToid', {
                        schoolcode: localStorage.schoolcode,
                        id: id
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.payTableList();
                                self.$message({
                                    message:response.data.msg,
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
            //未交费修改
            handleEdit(index, row) {
                // console.log(index, row);
            },
            handleRowCurrentChange(row, event, column) {
                // console.log(row, event, column, event.currentTarget)

            },

        }
    }
</script>

<style scoped>
    .tb-edit .el-input {
        display: none
    }
    .tb-edit .current-row .el-input {
        display: block
    }
    .tb-edit .current-row .el-input+span {
        display: none
    }
</style>