<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">应缴款项></a><a href="#">123</a></div>
            <div class="fl">
                <router-link to="/PayableManagement" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="gray-box-border clearfix">

                <div class="yin-yo1 fl">
                    <div class="ymy-on1" style="font-size: 16px;color: #323232;">缴费名称 : 123</div>
                    <div class="ymy-on2" style="color: #9e9e9e">缴费批号 : 123</div>
                </div>
                <div class="yin-yo2 fl">
                    <div class="ymy-on3 fl">
                        <div class="ym-se1">应缴笔数</div>
                        <div class="ym-se2">应缴金额</div>
                    </div>
                    <div class="ymy-on4 fl">
                        <div class="ym-se3">123</div>
                        <div class="ym-se4">123</div>
                    </div>
                </div>
                <div class="yin-yo3 fl">
                    <div class="ymy-on3 fl">
                        <div class="ym-se1">实缴笔数</div>
                        <div class="ym-se2">实缴金额</div>
                    </div>
                    <div class="ymy-on4 fl">
                        <div class="ym-se3">123</div>
                        <div class="ym-se4">123</div>
                    </div>
                </div>
            </div>


            <div class="three-top-box clearfix">
                <div class="mg-t10 clearfix">
                    <ul class="fl tog-ji">
                        <li :class="{active:isActive}" @click="paymentTotal">已缴费统计</li>
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
                </div>
                <div class="clearfix">
                    <div class="fl search-input" style="width:400px">
                        <input type="text" :placeholder="isActive?'订单编号、学工号、付款姓名、身份证号或手机号进行搜索':'学工号、付款姓名、身份证号或手机号进行搜索'"
                               class="se-input" v-model="inputStr" @keyup.enter="search">
                    </div>
                    <div class="fl search-button">
                        <button class="btn-pro" @click="search">搜索</button>
                    </div>
                    <div class="fl search-button">
                        <button class="btn-pro" @click="reset">重置</button>
                    </div>
                </div>
            </div>

            <div class="payment-item-table" :style="{display:isActive?'block':'none'}">
                <div class="table-box">
                    <template>

                        <el-table :data="configTable.tableArr" stripe style="width: 100%">
                            <el-table-column prop="out_order_no" label="订单编号">
                            </el-table-column>
                            <el-table-column prop="name" label="缴费项目">
                            </el-table-column>
                            <el-table-column prop="amount" label="缴费金额">
                            </el-table-column>
                            <el-table-column prop="fact_amount" label="实缴金额">
                            </el-table-column>
                            <el-table-column label="学工号">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column label="付款姓名">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column label="身份证号">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column label="收款PID">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column prop="pay_time" label="缴费时间" width="110px">
                            </el-table-column>
                            <el-table-column prop="status" label="缴费状态">
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
                <div class="table-box">
                    <template>
                        <el-table :data="configTable2.tableArr" class="tb-edit"
                                  stripe style="width: 100%">
                            <el-table-column prop="name" label="缴费项目">
                            </el-table-column>
                            <el-table-column prop="amount" label="缴费金额">
                            </el-table-column>
                            <el-table-column label="学工号">
                                <template slot-scope="scope">
                                    <el-input size="small" v-model="scope.row.stundetid" placeholder="请输入学工号"
                                              @change="handleEdit(scope.$index, scope.row)"></el-input>
                                    <span>123</span>
                                </template>
                            </el-table-column>
                            <el-table-column label="付款姓名">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column label="身份证号">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column label="收款PID">
                                <template slot-scope="scope">
                                    123
                                </template>
                            </el-table-column>
                            <el-table-column prop="status" label="缴费状态">
                            </el-table-column>
                            <el-table-column label="操作" width="120">
                                <template slot-scope="scope">
                                    <el-button type="text" size="small" class="del-tr-con"
                                               @click="tableDelete(scope.row.id)">删除
                                    </el-button>
                                </template>
                            </el-table-column>
                        </el-table>
                    </template>
                </div>
                <div class="block" style="text-align: right;">
                    <el-pagination
                            @size-change="handleSizeChange"
                            @current-change="handleCurrentChange"
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

    export default {
        data() {
            return {
                flag: false,
                detailArr: {},
                isActive: true,
                dateArr: [],
                paymentArr: [],
                paymentStr: '',
                inputStr: '',
                configTable: {
                    tableArr: [],
                    total: 0,
                    currentPage: 1,
                    iDisplayStart: 0,
                    iDisplayLength: 10,
                },
                configTable2: {
                    tableArr: [],
                    total: 0,
                    currentPage: 1,
                    iDisplayStart: 0,
                    iDisplayLength: 10,
                },
                pickerOptions2: {
                    disabledDate(time) {
                        return time.getTime() > Date.now() - 8.64e6
                    },
                },
            }
        },

        created() {

        },
        methods: {
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
            },
            handleEdit() {

            },
            //搜索
            search() {

            },
            //重置
            reset() {

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

    .tb-edit .current-row .el-input + span {
        display: none
    }
</style>