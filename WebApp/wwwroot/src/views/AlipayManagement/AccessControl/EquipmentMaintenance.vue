<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">门禁管理></a><a href="#">门禁设备维护</a></div>
            <div class="fl">
                <router-link to="/AccessControl" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab-box">
                <div class="nav-tab1 clearfix">
                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1" style="width:50px">设备号</div>
                        <div class="fl form-timerange">
                            <div class="fl form-timerange">
                                <el-input v-model="equipNo" placeholder="请输入设备号"></el-input>
                            </div>
                        </div>
                    </div>

                    <div class="fl on-part-search1">
                        <div class="fl left-word-on1">设备名称</div>
                        <div class="fl form-timerange">
                            <el-input v-model="equipName" placeholder="请输入设备名称"></el-input>
                        </div>
                        <div class="fl on-part-search">
                            <div class="fl search-button"><button class="btn-pro" @click="search">搜索</button></div>
                            <div class="fl search-button"><button class="btn-pro" @click="reset">重置</button></div>
                        </div>
                    </div>
                    <div class="fr right-btn-ta">
                        <button class="btn" @click="batchDelete">批量删除</button>
                    </div>
                </div>
            </div>
            <div class="payment-item-table" style="margin-top: 15px">
                <div class="table-box">
                    <template>
                        <el-table
                                :data="configTable.tableArr"
                                stripe
                                style="width: 100%"
                                @selection-change="handleSelectionChange">
                            <el-table-column
                                    type="selection"
                                    width="55">
                            </el-table-column>
                            <el-table-column
                                    prop="device_id"
                                    label="设备号"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="device_name"
                                    label="设备名称"
                            >
                            </el-table-column>
                            <el-table-column  label="操作" width="120">
                                <template slot-scope="scope">
                                    <el-button type="text" size="small" class="edit-tr-con" @click="handleEdit(scope.$index, scope.row)">修改</el-button>
                                    <el-button type="text" size="small" class="del-tr-con"  @click="handleDelete(scope.$index, scope.row)">删除</el-button>
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

        <!--修改-->
        <el-dialog title="门禁设备维护" :visible.sync="editVisible" width="600px" top="20vh">
            <div class="tk-gray tk-height">
                <el-form :model="accountForm" :rules="recerules" ref="accountForm" label-width="80px" class="demo-ruleForm">
                    <el-form-item label="门店ID" prop="messageid">
                        <el-input v-model="accountForm.messageid" placeholder="请输入门店ID"></el-input>
                    </el-form-item>
                    <el-form-item label="设备名称" prop="name">
                        <el-input  type="textarea" v-model="accountForm.name" placeholder="请输入设备名称"></el-input>
                    </el-form-item>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="editVisible = false">取 消</el-button>
                <el-button type="primary" @click="onSubmit('accountForm')" class="sure-btn">确定</el-button>
            </div>
        </el-dialog>

    </div>
</template>

<script>
    export default {
        name: "EquipmentMaintenance",
        data() {
            return {
                equipNo:'',
                equipName:'',
                editVisible:false,
                rowItem:{},
                multipleSelection:[],
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },

                tableData: [{
                    order:'21235687987898',
                    name: '闸机',
                    status:'已领卡'
                }, {
                    order:'21235687987898',
                    name: '闸机',
                    status:'已领卡'
                }, {
                    order:'21235687987898',
                    name: '闸机',
                    status:'已领卡'
                }, {
                    order:'21235687987898',
                    name: '闸机',
                    status:'已领卡'
                }],
                accountForm: {
                    name:'',
                    messageid:'',
                },recerules: {
                    messageid: [
                        {required: true, message: '门店ID不能为空',trigger: 'blur'}
                    ],
                    name: [
                        {required: true,message: '设备名称不能为空', trigger: 'blur'}
                    ],
                },
            }
        },
        created() {
            this.tableList();
        },
        methods: {
            batchDelete(){
                let self = this,ids = [];
                self.multipleSelection.forEach(items => {
                    ids.push(items.id);
                });
                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/Dormitory/DeleteSchoolDevice', { id: ids})
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.$message({
                                    showClose: true,
                                    message: '删除成功',
                                    type: 'success'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    self.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            handleEdit(index, row) {
                let self=this;
                self.editVisible = true;
                self.rowItem = row;
                self.accountForm.messageid = row.device_id;
                self.accountForm.name = row.device_name;

            },
            handleDelete(index, row) {
                let self = this;
                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/Dormitory/DeleteSchoolDevice', {
                        id: row.id
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.tableList();
                                self.$message({
                                    showClose: true,
                                    message: '删除成功',
                                    type: 'success'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    self.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            tableList(){
                let self = this;
                self.axios.get('api/Dormitory/GetSchoolDevice', {
                    params: {
                        schoolCode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        deviceCode:self.equipNo,
                        deviceName:self.equipName,
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
            handleSelectionChange(val) {
                this.multipleSelection = val;
            },
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
            reset(){
                this.equipNo = '';
                this.equipName = '';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
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
            onSubmit(ruleForm){
                let self = this;
                self.$refs[ruleForm].validate((valid) => {
                    if(valid){
                        self.axios.post('api/Dormitory/EditSchoolDevice', {
                            schoolcode: localStorage.schoolcode,
                            id:self.rowItem.id,
                            deviceName:self.accountForm.name,
                            shopid:self.accountForm.messageid,
                        })
                            .then(function (response) {
                                if(response.data.code == '000000'){
                                    self.editVisible = false;
                                    self.tableList();
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
                    }else {
                        return false;
                    }
                });

            }
        },
    }
</script>

<style scoped>

</style>