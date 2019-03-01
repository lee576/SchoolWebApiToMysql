<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">常规缴费查询</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn" @click="download">导出EXCEL</button>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab-box">
                <div class="nav-tab1 clearfix">
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">缴费时间</div>
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
                    </div>
                    <div class="fl on-part-search2">
                        <div class="fl search-input" style="width: 280px;" >
                            <input  type="text" placeholder="请输入订单编号、学工号、付款姓名进行搜索" class="se-input" @keyup.enter="search" v-model="inputVal">
                        </div>
                        <div class="fl search-button"><button class="btn-pro" @click="search">搜索</button></div>
                        <div class="fl search-button"><button class="btn-pro" @click="reset">重置</button></div>
                    </div>
                    <div class="high-set-on fr" style="margin-bottom: 15px;" :class="[{ active: isActive }]"  @click="toggleClass(isActive)">
                        <span  class="high-set-word">高级设置</span>
                        <span  class="high-set-img"></span>
                    </div>
                </div>
                <div class="nav-tab2 clearfix" :style="{display:(isActive?'block':'none')}">
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">缴费项</div>
                        <div class="fl form-timerange">
                            <!--multiple collapse-tags-->
                            <el-select v-model="paymentStr" placeholder="请选择缴费项" style="width:200px" @change="paymentChoose">
                                  <el-option v-for="item in paymentArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                            </el-select>

                        </div>
                    </div>
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1" style="width:42px">班级</div>
                        <div class="fl form-timerange">
                            <el-select placeholder="请选择班级" style="width:200px" v-model="classStr" @change="classChoose">
                                <el-option v-for="item in classArr" :label="item.classname" :value="item.id" :key="item.id"></el-option>
                            </el-select>

                        </div>
                    </div>
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1" style="width:59px">缴费状态</div>
                        <div class="fl form-timerange">
                            <el-select placeholder="请选择缴费状态" style="width:200px" v-model="statusStr" @change="statusChoose">
                                <el-option v-for="item in statusArr" :label="item.label" :value="item.value" :key="item.value"></el-option>
                            </el-select>

                        </div>
                    </div>
                    <ul class="choose-time-fo fr">
                        <li  v-for="(item,index) in btnArr" :class="radio==index?'active':''"><el-button  @click="changeIcon(index)" >{{item}}</el-button></li>
                    </ul>
                </div>
            </div>
            <div class="three-top-box clearfix">
                <div class="on-border-box fl">
                    <img :src="imageUrl1" class="fl on-border-img1">
                    <div class="fl on-border-img2">
                        <div class="border-word1">交易订单笔数</div>
                        <div class="border-word2">{{orderTotalNu | currency('',false) }}笔</div>
                    </div>
                </div>
                <div class="on-border-box fl">
                    <img :src="imageUrl3" class="fl on-border-img1">
                    <div class="fl on-border-img2">
                        <div class="border-word1">交易成功笔数</div>
                        <div class="border-word2">{{orderSuccessNO | currency('',false) }}笔</div>
                    </div>
                </div>
                <div class="on-border-box fl">
                    <img :src="imageUrl4" class="fl on-border-img1">
                    <div class="fl on-border-img2">
                        <div class="border-word1">待支付笔数</div>
                        <div class="border-word2">{{payoffNo | currency('',false) }}笔</div>
                    </div>
                </div>
                <div class="two-border-box fl">
                    <img :src="imageUrl2" class="fl on-border-img1">
                    <div class="fl on-border-img2">
                        <div class="border-word1">实收总金额</div>
                        <div class="border-word3">{{ orderTotalAmount| currency('￥') }}</div>
                    </div>
                </div>
            </div>
            <div class="payment-item-table">
                <div class="table-box">
                    <template>
                    <el-table :data="configTable.tableArr" stripe style="width: 100%">
                            <el-table-column prop="out_order_no" label="订单编号" >
                            </el-table-column>
                            <el-table-column prop="name" label="缴费项目">
                            </el-table-column>
                            <el-table-column prop="pay_amount" :formatter="moneyTable" label="缴费金额">
                            </el-table-column>
                            <el-table-column prop="student_id" label="学工号">
                            </el-table-column>
                            <el-table-column prop="pay_name" label="付款姓名">
                            </el-table-column>
                            <el-table-column prop="passport" label="身份证号">
                            </el-table-column>
                             <el-table-column prop="phone" label="手机号">
                            </el-table-column>
                            <el-table-column prop="pay_time" :formatter="dateTable" label="缴费时间">
                            </el-table-column>
                            <el-table-column prop="status" :formatter="feeStateTable"  label="缴费状态">
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
        </div>



    </div>
</template>

<script>
    import { currency } from './../../../util/currency'
    import { getDateType } from './../../../util/getDate'
    import { export_json_to_excel } from './../../../assets/js/Export2Excel'
    export default {
        name: "CommonSearchList",
        data() {
            return {
                statusStr:null,
                downloadLoading:false,
                radio:'0',
                dateArr:[],
                paymentArr:[],
                orderTotalNu:0,
                orderSuccessNO:0,
                payoffNo:0,
                orderTotalAmount:0,
                statusArr:[{value:null , label: '全部'},{value: '0', label: '未交费'},{value: '1', label: '已缴费'}],
                btnArr:['今天','昨天','最近七天','最近30天'],
                isActive:false,
                classArr:[],
                classStr:'',
                paymentStr:'',
                inputVal:'',
                imageUrl1: require('../../../assets/picture/images/order number.png'),
                imageUrl2: require('../../../assets/picture/images/order amount.png'),
                imageUrl3: require('../../../assets/images/success.png'),
                imageUrl4: require('../../../assets/images/tobepaid.png'),
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
        filters:{
            currency: currency,
            getDateType:getDateType,
            export_json_to_excel:export_json_to_excel

        },
        created() {
            let self=this;
            self.dateArr = [getDateType(1,0),getDateType(1,0)];
            //列表
            self.tableList();
            //缴费项列表
             self.paymentList();
            //班级列表
            self.classListA();
        },
        methods: {
            tableList(){
                let self = this,stime = '',etime = '';
                if(self.dateArr){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/PaymentItem/GetPaymentInfoSearch', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        sTime:stime,
                        eTime:etime,
                        tb_payment_sub_adminItem:self.paymentStr,
                        classID:self.classStr,
                        selectinfo:self.inputVal,
                        status:self.statusStr
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.configTable.tableArr = JSON.parse(res.data);
                            self.configTable.total = res.iTotalRecords;
                            //订单总数
                            self.orderTotalNu = res.totle.itemcount;
                            //待支付笔数
                            self.payoffNo = res.totle.djcount;
                            //交易金额
                            self.orderTotalAmount = res.totle.pay_amountTotle;
                            //成功交易笔数
                            self.orderSuccessNO = res.totle.sj_count;

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
            download(){
                let self = this,stime = '',etime = '';
                if(self.dateArr){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/PaymentItem/GetPaymentInfoSearchToExcel', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        stime:stime,
                        etime:etime,
                        id:self.id,
                        tb_payment_sub_adminItem:self.paymentStr,
                        classID:self.classStr,
                        selectinfo:self.inputVal

                    }
                })
                    .then(function (response) {
                        console.log(response)
                        let res = response.data;
                        if(res.code == '000000'){
                            self.downloadLoading = true;
                            require.ensure([], () => {
                                const tHeader = ["订单编号", "缴费项目", "缴费金额", "学工号", "付款姓名", "身份证号","手机号",'缴费时间','缴费状态']; //这个是表头名称 可以是iveiw表格中表头属性的title的数组
                                const filterVal = ["out_order_no", "name", "pay_amount", "student_id", "pay_name", "passport", "phone", "pay_time",'status']; //与表格数据配合 可以是iview表格中的key的数组
                                const list = res.data; //表格数据，iview中表单数据也是这种格式！
                                const data = self.formatJson(filterVal, list)
                                export_json_to_excel(tHeader, data, '常规缴费项明细') //列表excel  这个是导出表单的名称
                                self.downloadLoading = false;
                                self.exportVisible = false;
                            });
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
            formatJson(filterVal, jsonData) {
                return jsonData.map(v => filterVal.map(j => v[j]))
            },
            paymentChoose(val){
                this.paymentStr = val;
            },
            paymentList(){
                let self = this;
                self.axios.get('api/PaymentItem/GetPayment_itemToSchoolcode', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.paymentArr= res.data;
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
            toggleClass(e){
                if(e){
                    this.isActive = false;
                }else{
                    this.isActive = true;
                }

            },
            reset(){
                //重置
                this.orderNo = '';
                this.dateArr = [getDateType(1,0),getDateType(1,0)];
                this.paymentStr = '';
                this.inputVal = '';
                this.classStr = '';
                this.radio == '0';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
            changeIcon(icon){
                let self = this;
                self.radio = icon;
                if(self.radio == 0){
                    self.dateArr = [getDateType(1,0),getDateType(1,0)];
                }else if(self.radio == 1){
                    self.dateArr = [getDateType(1,-1),getDateType(1,0)];
                }else if(self.radio == 2){
                    self.dateArr = [getDateType(1,-7),getDateType(1,0)];
                }else if(self.radio == 3){
                    self.dateArr = [getDateType(1,-30),getDateType(1,0)];
                }
            },
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
            classListA(){
                let self = this;
                self.axios.get('api/SchoolDepartment/GetSchoolClassInfoToSchoolcode', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.classArr= res.data;
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
            classChoose(val){
                this.classStr = val;
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
            feeStateTable:function(row, column){
                //收费状态

                if(row.status == 1){
                    return "已缴费";
                }else {
                    return "未缴费";
                }

            },
            moneyTable:function(row, column){
                //金额
                return  currency(row.pay_amount,'￥');
            },
            dateTable:function (row, column) {
              return  row.pay_time.split('T')[0];
            },
            statusChoose(val){
                this.statusStr = val;
            },
            timeChoose(){
                let self = this;

                if(!self.dateArr){
                    self.radio = '6'
                }
            }
        }
    }
</script>

<style scoped>

</style>