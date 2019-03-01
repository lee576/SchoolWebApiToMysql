<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">签到管理</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn" @click="download">导出Excel</button>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab-box">
                <div class="nav-tab1 clearfix">
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1" style="width:80px">群名或编号</div>
                        <div class="fl form-timerange">
                            <div class="fl form-timerange">
                                <el-input v-model="teamNameorTeamiD" placeholder="请输入群名或编号"></el-input>
                            </div>
                        </div>
                    </div>
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1" style="width:80px">姓名或学号</div>
                        <div class="fl form-timerange">
                            <div class="fl form-timerange">
                                <el-input v-model="nameorid" placeholder="请输入姓名或学号"></el-input>
                            </div>
                        </div>
                    </div>

                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">查询日期</div>
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
                        </div>
                    </div>
                </div>
            </div>
            <div class="payment-item-table" style="margin-top: 15px">
                <div class="table-box">
                    <template>
                        <el-table
                                :data="configTable.tableArr"
                                stripe
                                style="width: 100%">
                            <el-table-column
                                    prop="id"
                                    label="签到ID"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="teamID"
                                    label="签到群组ID"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="teamName"
                                    label="签到群组"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="joinUserid"
                                    label="学员ID">
                            </el-table-column>
                            <el-table-column
                                    prop="user_name"
                                    label="学员名称">
                            </el-table-column>
                            <el-table-column
                                    prop="attendanceTime"
                                    label="签到时间">
                            </el-table-column>
                            <el-table-column
                                    label="状态">
                                <template slot-scope="scope">
                                    {{ scope.row.attendanceType==0 ? '签到':'签退' }}
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
        </div>
    </div>
</template>

<script>
    import { export_json_to_excel } from './../../../assets/js/Export2Excel'
    export default {
        name: "SignIn",
        data() {
            return {
                teamNameorTeamiD:'',
                nameorid:'',
                exportListData:[],
                dateArr:[],
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
            this.tableList();
        },
        filters:{
            export_json_to_excel:export_json_to_excel
        },
        methods: {
            formatJson(filterVal, jsonData) {
                return jsonData.map(v => filterVal.map(j => v[j]))
            },
            tableList(){
                let self = this,stime = '',etime = '';
                if(self.dateArr.length>0){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/SignIn/GetAttendanceInfoToSchoolCode', {
                    params: {
                        code: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        startTime:stime,
                        endTime:etime,
                        teamNameorTeamiD:self.teamNameorTeamiD,
                        nameorid:self.nameorid
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        console.log(res)
                        if(res.code == '000000'){
                            self.configTable.tableArr =res.aaData;
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
            //点击下载
            download(){
                let self = this,stime = '',etime = '';
                if(self.dateArr.length>0){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/SignIn/GetAttendanceInfo2', {
                    params: {
                        schoolCode: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.exportListData = res.data;
                            require.ensure([], () => {
                                const tHeader = ["签到ID", "群主ID", "群主名", "学号","姓名", "签到时间", "状态"]; //这个是表头名称 可以是iveiw表格中表头属性的title的数组
                                const filterVal = ["id", "teamID", ",teamName", "joinUserid", "user_name", "attendanceTime ", "attendanceType"]; //与表格数据配合 可以是iview表格中的key的数组
                                const list = self.exportListData; //表格数据，iview中表单数据也是这种格式！
                                const data = self.formatJson(filterVal, list)
                                export_json_to_excel(tHeader, data, '签到信息') //列表excel  这个是导出表单的名称
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
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
        },
    }
</script>

<style scoped>

</style>