<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">资金管理</a></div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
                <div class="add-page">
                    <div class="add-page-title">对账单下载</div>
                    <el-form :model="accountForm" :rules="adminrules" ref="accountForm" label-width="100px" class="demo-ruleForm">
                        <div class="gray-box-part">
                            <el-form-item label="对账平台">
                                <el-radio label="0" v-model="accountForm.payStr">支付宝</el-radio>
                            </el-form-item>
                            <el-form-item label="对账周期">
                                <el-radio-group v-model="accountForm.radioTime">
                                    <el-radio label="0"  @change="dateChange">按自然日下载</el-radio>
                                    <el-radio label="1"  @change="dateChange">按自然月下载</el-radio>
                                </el-radio-group>
                            </el-form-item>
                            <el-form-item label="收款账号" prop="account">
                                <el-select v-model="accountForm.accountStr" placeholder="请选择收款账号" @change="accountChange">
                                    <el-option v-for="item in accountForm.accountArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                                </el-select>
                            </el-form-item>
                            <el-form-item label="对账单日期" prop="dayValue" :style="{display:radioFlag?'block':'none'}">
                                <el-date-picker
                                        v-model="accountForm.dayValue"
                                        type="date"
                                        value-format="yyyy-MM-dd"
                                        :clearable=false
                                        placeholder="选择对账单日期">
                                </el-date-picker>
                            </el-form-item>
                            <el-form-item label="对账单日期" prop="monthValue" :style="{display:!radioFlag?'block':'none'}">
                                <el-date-picker
                                        v-model="accountForm.monthValue"
                                        type="month"
                                        value-format="yyyy-MM"
                                        :clearable=false
                                        placeholder="选择对账单日期">
                                </el-date-picker>
                            </el-form-item>
                            <el-form-item label="对账单类型">
                                <el-radio label="0" v-model="accountForm.type">所有交易账单(含退款)</el-radio>
                            </el-form-item>
                        </div>
                        <div class="load-tip">可下载的对账数据为已结算的订单</div>
                        <el-form-item>
                            <el-button class="btn-bg" @click="download">立即下载</el-button>
                            <el-button class="btn-border" @click="cancel">取 消</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import { export_json_to_excel } from './../../../assets/js/Export2Excel'
    import { getDateType } from './../../../util/getDate'
    export default {
        name: "BillDownload",
        data() {
            return {
                radioFlag:true,
                exportListData:[],
                accountForm: {
                    type:'0',
                    accountStr:null,
                    accountArr:[],
                    payStr:'0',
                    radioTime:'0',
                    dayValue: '',
                    monthValue:''
                },adminrules: {

                }
            }
        },
        created() {
            this.accountForm.dayValue = getDateType(1,0);
            this.accountForm.monthValue = getDateType(0,0);
            this.accountList();
        },
        filters:{
            getDateType:getDateType,
            export_json_to_excel:export_json_to_excel
        },
        methods: {
            //点击下载
            download(){
                let self = this,time;
                if(this.radioFlag){
                    time = this.accountForm.dayValue;
                }else {
                    time = this.accountForm.monthValue;
                }
                debugger;
                self.axios.get('api/Cashier/GetBillExcel', {
                    params: {
                        school_id: localStorage.schoolcode,
                        timeType:self.accountForm.radioTime,
                        datetime:time,
                        payment_accounts:self.accountForm.accountStr
                    }
                })
                    .then(function (response) {
                        console.log(response)
                        let res = response.data;
                        if(res.code == '10000'){
                            self.exportListData = res.data;
                            require.ensure([], () => {
                                const tHeader = ["支付宝交易号", "商户订单号", "业务类型", "商品名称", "创建时间", "完成时间","门店编号",'门店名称','操作员',"终端号",'对方账户','订单金额（元）','商家实收（元）','退款金额（元）','支付宝红包（元）','集分宝（元）','支付宝优惠（元）','商家优惠（元）','券核销金额（元）','券名称','商家红包消费金额（元）','卡消费金额（元）','退款批次号/请求号','服务费（元）','分润（元）','备注']; //这个是表头名称 可以是iveiw表格中表头属性的title的数组
                                const filterVal = ["alipay_order", "order", "type", "trade_name", "create_time", "finish_time", "stall",
                                    "name",'operators','terminal_number','payer_account','pay_amount','paid','refund',
                                    "alipay_red", "collection_treasure", "alipay_discount", "merchant_discount", "ticket_money", "ticket_name", "merchant_red_consumption",
                                    "card_consumption", "refund_batch_number", "service_charge", "shares_profit", "remark"];//与表格数据配合 可以是iview表格中的key的数组
                                const list = self.exportListData; //表格数据，iview中表单数据也是这种格式！
                                const data = self.formatJson(filterVal, list)
                                export_json_to_excel(tHeader, data, '交易流水') //列表excel  这个是导出表单的名称
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
            dateChange(){
                let self = this;
                if(self.radioFlag){
                    self.radioFlag=false;
                    self.accountForm.is_choosetime = '1';
                }else {
                    self.radioFlag=true;
                    self.accountForm.is_choosetime = '0';
                }

            },
            formatJson(filterVal, jsonData) {
                return jsonData.map(v => filterVal.map(j => v[j]))
            },
            accountList(){
                let self = this;
                self.axios.get('api/Cashier/GetPayment_accounts', {
                    params: {
                        school_id: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '10000'){
                            self.accountForm.accountArr = res.data;
                            self.accountForm.accountArr.unshift({id:null , name: '全部'});
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
            accountChange(val){
                this.accountForm.accountStr = val;
            },
            cancel(){
                this.$router.push({
                    path: '/Transactionflow'
                });
            }
        }
    }
</script>

<style scoped>

</style>