<template>
    <div class="home">
        <div class="backpic" style="color: #fff;">
            <div style="display: flex;justify-content: space-between;padding: 20px 20px">
                <div style="color: #fff;">
                    ＜返回
                </div>
                <div style="color: #fff;">
                    食堂管理
                </div>
                <div style="width: 40px">

                </div>
            </div>
            <div class="block" style="text-align: center;margin-top: 10px">
                <el-date-picker
                        style="width: 8rem;border-radius:50%;background-color: blue"
                        v-model="time"
                        type="month"
                        :clearable="false"
                        placeholder="选择月"
                        @change="getSTime">
                </el-date-picker>
            </div>
            <div style="font-size: 36px;text-align: center;margin: 14px 0;color: #fff;">
                ￥{{price}}
            </div>
            <div style="text-align: center;color: #fff;">
                共{{alldata}}项缴费
            </div>

        </div>
        <div style="margin: 0 auto;
        width: 90%;
        border: 1px solid #d7d7d7;
        position: relative;background-color: #fff;border-radius:5px;
        top: -1.5rem" v-show="showdata">
            <div style="height: 30rem;overflow: scroll">
                <div style="padding: 16px;border-bottom:1px solid #d7d7d7" v-for="item in cashierData">
                    <div style="display: flex;justify-content: space-between;margin-bottom: 5px;color: rgba(35,135,251,1);">
                        <div style="color: rgba(35,135,251,1)">{{item.stall}}</div>
                        <div style="color: rgba(35,135,251,1)">{{item.pay_amount}}元</div>
                    </div>
                    <div style="font-size: 12px;color: rgba(112,112,112,1);">交易时间:{{item.datetime}}</div>
                </div>
            </div>
        </div>
        <div style="margin: 0 auto;
        width: 90%;
        border: 1px solid #d7d7d7;
        position: relative;background-color: #fff;border-radius:5px;
        top: -1.5rem" v-show="shownone">
            <div style="width: 100%;height: 30rem">
                <img src="../../assets/h5Canteen/none.png" alt="" style="width:130px;position: relative;top: 40%;left: 32%;">
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                time: '',
                showdata: false,
                shownone: true,
                cashierData: [],
                alldata: '',
                dayData: '',
                price: '',
            }
        },
        created() {
            this.dateFormat()
            let id = this.$route.params.stuid
            this.getCountDays(new Date())
            this.init()
        },
        methods: {
            init() {
                this.axios.get(`api/Cashier/GetFlowing`, {
                    params: {
                        iDisplayStart: '0',
                        iDisplayLength: '9999999',
                        school_id: this.$route.query.school_id,
                        user_code: this.$route.query.stuid,
                        stime: this.time + '-1',
                        etime: this.time + '-' + this.dayData
                    }
                }).then(res => {
                    console.log(res)
                    this.cashierData = res.data.data
                    if (res.data.code == "10000" && res.data.data.length == 0) {
                        this.showdata = false
                        this.shownone = true
                    } else {
                        this.showdata = true
                        this.shownone = false
                    }
                })
                this.axios.get(`api/Cashier/GetFlowing`, {
                    params: {
                        iDisplayStart: '0',
                        iDisplayLength: '-1',
                        school_id: this.$route.query.school_id,
                        user_code: this.$route.query.stuid,
                        stime: this.time + '-1',
                        etime: this.time + '-' + this.dayData
                    }
                }).then(res => {
                    this.alldata = res.data.iTotalRecords
                    console.log(res)
                })
                this.axios.get(`api/Cashier/GetFlowingSUM`,{
                    params:{
                        school_id: this.$route.query.school_id,
                        user_code: this.$route.query.stuid,
                        stime: this.time + '-1',
                        etime: this.time + '-' + this.dayData
                    }
                }).then(res=>{
                    console.log(res)
                    this.price=res.data.count
                })
            },
            getSTime(val) {
                this.time = val;
                console.log(val)
                this.parseTime(val)
                this.getCountDays(val)
                console.log(this.dayData)
                this.init()
            },
            //默认显示时间
            dateFormat() {
                var date = new Date();
                var year = date.getFullYear();
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                this.time = year + "-" + month
            },
            //标准时间转化yyyy-mm
            parseTime(str) {
                if ((str + '').indexOf('-') != -1) {
                    str = str.replace(new RegExp(/-/gm), '/')
                }
                let d = new Date(str)
                let newDateYear = d.getFullYear()
                let newDateMonth = (d.getMonth() + 1) < 10 ? '0' + (d.getMonth() + 1) : (d.getMonth() + 1)
                this.time = newDateYear + '-' + newDateMonth
            },
            //获取天数
            getCountDays(data) {
                var curDate = data;
                console.log(curDate)
                /* 获取当前月份 */
                var curMonth = curDate.getMonth();
                /*  生成实际的月份: 由于curMonth会比实际月份小1, 故需加1 */
                curDate.setMonth(curMonth + 1);
                /* 将日期设置为0 */
                curDate.setDate(0);
                /* 返回当月的天数 */
                this.dayData = curDate.getDate()
                console.log(this.dayData)
            }
        }
    }
</script>

<style scoped>
    .backpic {
        background-image: url("../../assets/h5Canteen/back.png");
        background-size: cover;
        background-repeat: no-repeat;
        height: 17rem;
    }

    .backpic img {
        height: 20rem;
    }


</style>
