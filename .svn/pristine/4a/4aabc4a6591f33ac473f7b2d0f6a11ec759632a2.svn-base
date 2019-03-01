<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">概览</a></div>
        </div>
        <div class="page-content">
            <div class="three-top-box clearfix">
                <div class="on-border-box fl">
                    <img :src="imageUrl1" class="fl on-border-img1">
                    <div class="fl on-border-img2">
                        <div class="border-word1">机器数</div>
                        <div class="border-word2">{{ machineNo | currency('',false) }}</div>
                    </div>
                </div>
                <div class="on-border-box fl">
                    <div class="fl on-border-img2">
                        <div class="border-word1">今日收款金额</div>
                        <div class="border-word-cor1">{{ todayMoney| currency('￥') }}</div>
                    </div>
                </div>
                <div class="two-border-box fl">
                    <div class="fl on-border-img2">
                        <div class="border-word1">昨日收款金额</div>
                        <div class="border-word-cor2">{{ yesMoney| currency('￥') }}</div>
                    </div>
                </div>
            </div>
            <div class="ym-body">
                <div class="clearfix">
                    <ul class="fl mon-data-ul">
                        <li :class="{active:isShow}" @click="tab">收费金额</li>
                        <li :class="{active:!isShow}" @click="tab">交易笔数</li>
                    </ul>
                    <div class="right-picker-time">
                        <el-date-picker
                                v-model="dateArr"
                                value-format="yyyy-MM-dd"
                                type="daterange"
                                align="right"
                                unlink-panels
                                range-separator="至"
                                start-placeholder="开始日期"
                                end-placeholder="结束日期"
                                :picker-options="pickerOptions2">
                        </el-date-picker>
                        <button class="search-in-time" @click="search()">搜索</button>
                    </div>
                    <div style="width: 100%; height: 450px;overflow: hidden;">
                        <div id="moneyBar" style="width:100%; height: 450px;background:#f9f9f9;" :style="{display: isShow ? 'block' : 'none'}" ref="companyStyle"></div>
                        <div id="amountBar" style="width:100%; height: 450px;background:#f9f9f9;" ></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    // 引入基本模板
    let echarts = require('echarts/lib/echarts');
    // 引入柱状图组件
    require('echarts/lib/chart/bar');
    // 引入提示框和title组件
    require('echarts/lib/component/tooltip');
    require('echarts/lib/component/title');
    import { currency } from './../../../util/currency'
    import { getDateType } from './../../../util/getDate'
    export default {
        name: "record",
        data() {
            return {
                imageUrl1: require('../../../assets/picture/images/machine.png'),
                dateArr:[],
                dateArr1:[],
                isShow: true,
                machineNo:'0',
                todayMoney:'0',
                yesMoney:'0',
                payArr:[],
                amountArr:[],
                barWidth:'',
                pickerOptions2: {
                    disabledDate(time) {
                        return time.getTime() > Date.now() - 8.64e6
                    },
                },
            }
        },
        created() {
            this.dateArr = [getDateType(1,-7),getDateType(1,0)];
            this.dataRecord();
        },

        mounted() {
            // console.log(this.$refs.companyStyle)
            // this.barWidth = this.$refs.companyStyle.clientWidth+'px';
            // console.log(this.barWidth)
            // debugger;
            this.barData();


        },
        filters:{
            currency: currency,
            getDateType:getDateType
        },
        methods: {
            tab(){
                // console.log(this.$refs.companyStyle)
                // this.barWidth = this.$refs.companyStyle.clientWidth;
                // debugger;
                this.isShow = !this.isShow;
            },
            barData(stime,etime){
                let self = this;
                if(!stime){
                    stime   = self.dateArr[0];
                }else{
                    stime  = stime;
                }
                if(!etime){
                    etime = self.dateArr[1];
                }else {
                    etime = etime;
                }

                self.axios.get('api/Cashier/GetCashierOrder',
                    {
                        params: {
                            school_id: localStorage.schoolcode,
                            st:stime,
                            et:etime
                        }
                    })
                    .then(res => {
                        console.log(res);
                        if(res.data.code == '10000'){
                            self.dateArr1 = res.data.array_alldays.reverse();
                            self.amountArr = res.data.ordercount.reverse();
                            self.payArr = res.data.pays.reverse();
                            self.drawMoneyBar(self.dateArr1,self.amountArr );
                            self.drawAmountBar(self.dateArr1,self.payArr);
                        }else {
                            self.$message({
                                showClose: true,
                                message: res.data.msg,
                                type: 'warning'
                            });
                        }

                    })
                    .catch(function (error) {

                        console.log(error);

                    });
            },
            drawAmountBar(dateArr,amountArr){
                // 基于准备好的dom，初始化echarts实例
                let myChart = echarts.init(document.getElementById('amountBar'));
                myChart.setOption({
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: ['笔数'],
                        formatter: function (name) {
                            return name;
                        },
                        orient: 'vertical',
                        align: 'right',
                        itemWidth: 20,
                        itemHeight: 20,
                        right: 0,
                        top: 'middle',
                        textStyle: {
                            // 图例的公用文本样式。
                            fontWight: 'bold',
                            fontSize: 20,
                            color: '#707070'
                        },
                        symbolKeepAspect: true,
                        itemGap: 20,

                    },

                    toolbox: {
                        show: false,
                        top: '0',
                        right: '120',
                        bottom: 'auto',
                        left: 'auto',
                        feature: {
                            dataView: {show: true, readOnly: false},
                            magicType: {show: true, type: ['line', 'bar']},
                            restore: {show: true},
                            saveAsImage: {show: true}
                        }
                    },
                    calculable: true,
                    xAxis: [
                        {
                            type: 'category',
                            //    boundaryGap : false,
                            data: dateArr,
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: ['#ccc'],
                                    width: 1,
                                    type: 'bar'
                                }

                            }

                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: ['#ccc'],
                                    width: 1,
                                    type: 'bar'
                                }

                            }
                        }
                    ],
                    grid: {
                        borderWidth: 2,
                        borderColor: '#ccc',
                        top: '80',
                        right: '120',
                        bottom: '40',
                        left: '30'

                    },
                    series: [
                        {
                            itemStyle: {
                                normal: {
                                    color: ['#6c7fff'],
                                    barBorderRadius: [8, 8, 0, 0]
                                }
                            },
                            name: '笔数',
                            type: 'bar',
                            data: amountArr,
                            markLine: {
                                data: [
                                    {type: 'average', name: '平均值'}
                                ]
                            },
                            barWidth: 30
                        }
                    ],
                    show: true,

                });
            },
            drawMoneyBar(dateArr,payArr){
                let myChart = echarts.init(document.getElementById('moneyBar'));
                myChart.setOption({
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        data: ['金额'],
                        formatter: function (name) {
                            return name;
                        },
                        orient: 'vertical',
                        align: 'right',
                        itemWidth: 20,
                        itemHeight: 20,
                        right: 0,
                        top: 'middle',
                        textStyle: {
                            // 图例的公用文本样式。
                            fontWight: 'bold',
                            fontSize: 20,
                            color: '#707070'
                        },
                        symbolKeepAspect: true,
                        itemGap: 20,

                    },

                    toolbox: {
                        show: false,
                        top: '0',
                        right: '120',
                        bottom: 'auto',
                        left: 'auto',
                        feature: {
                            dataView: {show: true, readOnly: false},
                            magicType: {show: true, type: ['line', 'bar']},
                            restore: {show: true},
                            saveAsImage: {show: true}
                        }
                    },
                    calculable: true,
                    xAxis: [
                        {
                            type: 'category',
                            //    boundaryGap : false,
                            data: dateArr,
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: ['#ccc'],
                                    width: 1,
                                }

                            }

                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: ['#ccc'],
                                    width: 1,
                                    type: 'value'
                                }

                            }
                        }
                    ],
                    grid: {
                        borderWidth: 2,
                        borderColor: '#ccc',
                        top: '80',
                        right: '120',
                        bottom: '40',
                        left: '30'

                    },
                    series: [
                        {
                            itemStyle: {
                                normal: {
                                    color: ['#6c7fff'],
                                    barBorderRadius: [8, 8, 0, 0]
                                }
                            },
                            name: '金额',
                            type: 'bar',
                            data: payArr,
                            markLine: {
                                data: [
                                    {type: 'average', name: '平均值'}
                                ]
                            },
                            barWidth: 30
                        }
                    ],
                    show: true,

                });
            },
            search(){
                if(!this.dateArr1){
                    this.$message({
                        showClose: true,
                        message: '请填写完整',
                        type: 'error'
                    });
                }else{
                   let stime = this.dateArr[0],etime = this.dateArr[1];;
                   if(this.timeDif(stime,etime)>30){
                       this.$message({
                           showClose: true,
                           message: '时间差不能超过30天',
                           type: 'error'
                       });
                   }else {
                       this.barData(stime,etime);
                   }


                }
            },
            dataRecord(){
                let self = this;
                self.axios.get('api/PaymentAR/GetPaymentARAccount?schoolcode=' + localStorage.schoolcode)
                    .then(response => {
                        let resp = response.data;
                        if(response.data.code == "10000"){
                            //机器数
                            sele.machineNo =resp.deviceCount;
                            //今日收款金额
                            self.todayMoney = resp.totalMoney.toFixed(2);
                            //昨日收款金额
                            self.yesMoney = resp.yesterdayMoney.toFixed(2);

                        }else {
                            self.$message({
                                showClose: true,
                                message: resp.data.msg,
                                type: 'warning'
                            });
                        }

                    })
                    .catch(function (error) {

                        console.log(error);

                    });
            },
            timeDif(sDate1,sDate2){
                var dateSpan,
                    tempDate,
                    iDays;
                sDate1 = Date.parse(sDate1);
                sDate2 = Date.parse(sDate2);
                dateSpan = sDate2 - sDate1;
                dateSpan = Math.abs(dateSpan);
                iDays = Math.floor(dateSpan / (24 * 3600 * 1000));
                return iDays
            }
        }
        
    }
</script>

<style scoped>

</style>