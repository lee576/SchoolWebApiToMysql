<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">门禁管理></a><a href="#">分析数据信息</a></div>
            <div class="fl">
                <router-link to="/AccessControl" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab-box" style="margin-bottom: 20px">
                <div class="nav-tab1 clearfix">
                   <!-- <div class="fl on-part-search1">
                        <div class="fl left-word-on1">设备</div>
                        <div class="fl form-timerange">
                            <div class="fl form-timerange">
                                <el-select placeholder="请选择设备" style="width:180px" v-model="equipStr"  @change="equipChoose">
                                    <el-option v-for="item in equipArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                                </el-select>
                            </div>
                        </div>
                    </div>-->

                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">门禁时间</div>
                        <div class="fl form-timerange">
                            <el-date-picker
                                    v-model="dateArr"
                                    value-format="yyyy-MM-dd"
                                    type="daterange"
                                    range-separator="至"
                                    start-placeholder="开始日期"
                                    end-placeholder="结束日期" style="width:250px" :picker-options="pickerOptions2">
                            </el-date-picker>
                        </div>
                        <div class="fl on-part-search">
                            <div class="fl search-button"><button class="btn-pro" @click="search">搜索</button></div>
                            <div class="fl search-button"><button class="btn-pro" @click="reset">重置</button></div>
                        </div>
                    </div>

                </div>
            </div>
            <el-row :gutter="10" >
                <el-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16" >
                   <div class="ant-card-men" style="height: 480px">
                       <div class="echart-men-tip">门禁卡使用时间</div>
                       <div id="barChart" :style="{width: '100%', height: '420px'}" style=" background: rgb(249, 249, 249);"></div>
                   </div>
                </el-col>
                <el-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8" >
                    <div class="ant-card-men" style="height: 480px">
                        <div class="echart-men-tip">门禁卡使用对比</div>
                        <div>
                            <div id="circleChart" :style="{width: '100%', height: '420px'}"></div>
                           <!-- <div class="process-box-ab">
                                <div class="ab-on1">
                                    <div class="cx-code">付款码</div>
                                    <div class="ab-on-pro"><el-progress :text-inside="true" :stroke-width="23" :percentage="50" color="#F8991C"></el-progress></div>
                                    <div>100人/50%</div>
                                </div>
                                <div class="ab-on1">
                                    <div class="cx-code">校园码码</div>
                                    <div class="ab-on-pro"><el-progress :text-inside="true" :stroke-width="23" :percentage="80"></el-progress></div>
                                    <div>160人/80%</div>
                                </div>
                            </div>-->
                        </div>
                    </div>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                    <div class="ant-card-men">
                        <div class="rank-num">目前场景总人数：{{allPerson}}</div>
                        <div class="rank-word">排行榜</div>
                       <div :style="{display:flag?'block':'none'}">
                           <ul class="rank-list clearfix"  >
                               <li v-for="(item, index) in perArr" :key="item.id">{{index+1}}　{{item.user_name}}</li>
                           </ul>
                       </div>
                        <div class="cx-rank" :style="{display:!flag?'block':'none'}">暂无排行</div>
                    </div>
                </el-col>
            </el-row>
        </div>
    </div>
</template>

<script>
    import {currency} from './../../../util/currency';
    import {getDateType} from './../../../util/getDate';
    // 引入基本模板
    let echarts = require('echarts/lib/echarts');
    // 引入柱状图组件
    require('echarts/lib/chart/line');
    require('echarts/lib/chart/bar');
    require("echarts/lib/chart/pie");
    // 引入提示框和title组件
    require('echarts/lib/component/tooltip');
    require('echarts/lib/component/title');
    export default {
        name: "AccessControlAnalysis",
        data() {
            return {
                equipStr:'',
                flag:true,
                dateArr: [],
                dataArrRate:[],
                dataArrTime:[],
                noUseRate:'',
                useRate:'',
                allPerson:'0',
                perArr:[],
                pickerOptions2: {
                    disabledDate(time) {
                        return time.getTime() > Date.now() - 8.64e6
                    },
                },
            }
        },
        created(){
            this.getParams();
            this.dateArr = [getDateType(1, -6), getDateType(1, 0)];
            this.rankList();
        },
        mounted() {
             this.drawLine();
             this.drawCircle();

        },
        filters: {
            currency: currency,
            getDateType: getDateType
        },

        methods: {
            getParams(){
                // 取到路由带过来的参数
                this.equipStr = this.$route.query.equipStr;
            },
            //柱形图
            drawLine(stime, etime) {
                let self = this;
                // 基于准备好的dom，初始化echarts实例
                let barChart = echarts.init(document.getElementById('barChart'));

                // 绘制图表
                if (!stime) {
                    stime = self.dateArr[0]
                }
                if (!etime) {
                    etime = self.dateArr[1]
                }

                self.axios.get('api/Dormitory/Get_seriesIncomingFrequencyInfo', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        stime: stime,
                        etime: etime,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if (res.code == '00000') {
                            self.dateArr1 = res.dateArr;
                            self.dataArrRate = res.dataArrRate;
                            self.dataArrTime = res.dataArrTime;
                            // 基于准备好的dom，初始化echarts实例
                            barChart.setOption({
                                tooltip: {
                                    trigger: 'axis'
                                },
                                legend: {
                                    //data: ['进入频次', '平均停留时间']
                                    data: ['进入频次', '平均停留时间'],
                                    formatter: function (name) {
                                        console.log(name)
                                        return name;
                                    },
                                    // orient: 'vertical',
                                    align: 'right',
                                    itemWidth: 20,
                                    itemHeight: 20,
                                    // right: 0,
                                    top: '20px',
                                    textStyle: {
                                        // 图例的公用文本样式。
                                        fontWight: 'bold',
                                        fontSize: 20,
                                        color: '#707070'
                                    },
                                    symbolKeepAspect: true,
                                    itemGap: 20,
                                    // padding: [
                                    //     20,  // 上
                                    //     20, // 右
                                    //     20,  // 下
                                    //     20, // 左
                                    // ]

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
                                        data: self.dateArr1,
                                        splitLine: {
                                            show: true,
                                            lineStyle: {
                                                color: ['#ccc'],
                                                width: 1,
                                                type: 'dashed'
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
                                                type: 'dashed'
                                            }

                                        }
                                    }
                                ],
                                grid: {
                                    //show:true,
                                    //   backgroundColor: '#f9f9f9',
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
                                                color: ['#2387fb'],
                                                barBorderRadius: [8, 8, 0, 0]
                                            }
                                        },
                                        name: '进入频次',
                                        type: 'line',
                                        data: self.dataArrRate,
                                        smooth: true,
                                        markPoint: {
                                            data: [
                                                {type: 'max', name: '最大值'},
                                                {type: 'min', name: '最小值'}
                                            ]
                                        },
                                        markLine: {
                                            data: [
                                                {type: 'average', name: '平均值'}
                                            ]
                                        },
                                        barWidth: 20
                                    },
                                    {
                                        itemStyle: {
                                            normal: {
                                                color: ['#ffb758'],
                                                barBorderRadius: [8, 8, 0, 0]
                                            }
                                        },
                                        name: '平均停留时间',
                                        type: 'line',
                                        data: self.dataArrTime,
                                        /*markPoint : {
                                            data : [
                                                {name : '年最高', value : 182.2, xAxis: 7, yAxis: 183},
                                                {name : '年最低', value : 2.3, xAxis: 11, yAxis: 3}
                                            ]
                                        },*/
                                        markLine: {
                                            data: [
                                                {type: 'average', name: '平均值'}
                                            ]
                                        },
                                        barWidth: 20
                                    },
                                ],
                                show: true,

                            });
                        } else {
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
            //饼状图
            drawCircle(stime, etime) {
                let self = this;
                // 基于准备好的dom，初始化echarts实例
                let circleChart = echarts.init(document.getElementById('circleChart'));

                // 绘制图表
                if (!stime) {
                    stime = self.dateArr[0]
                }
                if (!etime) {
                    etime = self.dateArr[1]
                }

                self.axios.get('api/Dormitory/Get_dataRate', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        stime: stime,
                        etime: etime,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if (res.code == '00000') {
                           self.noUseRate = res.dataRate.split('-')[0];
                           self.useRate =  res.dataRate.split('-')[1];
                            // 基于准备好的dom，初始化echarts实例

                            circleChart.setOption({
                                title: {
                                    // text: '某站点用户访问来源',
                                    //subtext: '纯属虚构',
                                    x: '0',
                                    textStyle: {
                                        // 图例的公用文本样式。
                                        // fontWight:'bold',
                                        fontSize: 14,
                                        color: '#707070'
                                    },
                                },
                                tooltip: {
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {
                                    // top:10,
                                    bottom: 20,
                                    left: 'center',
                                    itemWidth: 20,
                                    itemHeight: 20,
                                    data: ['已使用', '未使用'],
                                    formatter: function (name) {
                                        console.log(name)
                                        return name;
                                    },
                                    itemGap: 40,
                                },
                                color: ['#ffb758', '#2387fb'],
                                series: [
                                    {
                                        name: '门禁卡使用率',
                                        type: 'pie',
                                        radius: '70%',
                                        center: ['50%', '50%'],
                                        // radius: ['50%', '70%'],  // 设置环形饼状图， 第一个百分数设置内圈大小，第二个百分数设置外圈大小
                                        // center: ['50%', '50%'],  // 设置饼状图位置，第一个百分数调水平位置，第二个百分数调垂直位置

                                        data: [
                                            {value:self.useRate, name: '已使用'},
                                            {value: self.noUseRate, name: '未使用'},

                                        ],
                                        itemStyle: {
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        },
                                        label: {
                                            normal: {
                                                show: true,
                                                position: 'inside',
                                                formatter: '{d}%',//模板变量有 {a}、{b}、{c}、{d}，分别表示系列名，数据名，数据值，百分比。{d}数据会根据value值计算百分比

                                                textStyle: {
                                                    align: 'center',
                                                    baseline: 'middle',
                                                    fontFamily: '微软雅黑',
                                                    fontSize: 15,
                                                    fontWeight: 'bolder'
                                                }
                                            },
                                        },
                                    }
                                ]
                            })
                        } else {
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
            //排行
            rankList(stime, etime){
                let self = this;
                // 绘制图表
                if (!stime) {
                    stime = self.dateArr[0]
                }
                if (!etime) {
                    etime = self.dateArr[1]
                }
                self.axios.get('api/Dormitory/Get_LibraryRanking', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        stime: stime,
                        etime: etime,
                        devicetype:self.equipStr
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '00000'){
                            if(res.count.length>0){
                                self.flag = true;
                                if(res.count.length >6){
                                    self.perArr =res.count.slice(0,6);
                                    self.allPerson = 6;
                                }else {
                                    self.perArr = res.count;
                                    self.allPerson = res.count.length;
                                }

                            }else {
                                self.flag = false;
                            }
                        }else {
                            self.$message({
                                showClose: true,
                                message: res.msg,
                                type: 'error'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            search(){
                let stime = this.dateArr[0],etime = this.dateArr[1];
                if(this.timeDif(stime,etime)>30){
                    this.$message({
                        showClose: true,
                        message: '时间差不能超过30天',
                        type: 'error'
                    });
                }else {
                    this.drawLine(stime,etime);
                    this.drawCircle(stime,etime);
                    this.rankList(stime,etime);
                }
            },
            reset(){
                this.dateArr = [getDateType(1, -6), getDateType(1, 0)];
                this.drawLine();
                this.drawCircle();
                this.rankList();
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
    .cx-rank{
        width: 800px;
        text-align: center;
        background: rgb(249, 249, 249);
        line-height: 43px;
        border-radius: 3px;
    }

</style>