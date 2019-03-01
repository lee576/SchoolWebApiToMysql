<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">蓝牙设备管理</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn" @click="userOption">添加设施</button>
            </div>
        </div>
        <div class="page-content">
            <div class="payment-item-table">
                <div class="clearfix" style="margin-bottom: 15px">
                    <div class="fl right-btn-ta">
                        <button class="btn" @click="batchDelete">批量删除</button>
                    </div>
                </div>
                <div class="table-box">
                    <template>
                        <el-table
                                :data="configTable.tableArr"
                                stripe
                                style="width: 100%">
                            <el-table-column
                                    type="selection"
                                    width="55">
                            </el-table-column>
                            <el-table-column
                                    prop="id"
                                    label="蓝牙设备号"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="uuid"
                                    label="蓝牙设备UUID"
                            >
                            </el-table-column>
                            <el-table-column  label="操作" width="120">
                                <template slot-scope="scope">
                                    <el-button type="text" size="small" class="edit-tr-con" @click="userOption(scope.row)">修改</el-button>
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

        <!--添加蓝牙设备-->
        <el-dialog title="添加蓝牙设备" :visible.sync="bluetoothVisible" width="600px" top="20vh">
            <div class="tk-gray tk-height">
                <el-form :model="accountForm" :rules="recerules" ref="accountForm" label-width="105px" class="demo-ruleForm">
                    <el-form-item label="蓝牙设备UUID" prop="uuid">
                        <el-input v-model="accountForm.uuid" placeholder="请输入蓝牙设备UUID"></el-input>
                    </el-form-item>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="bluetoothVisible = false">取 消</el-button>
                <el-button type="primary" @click="onSubmit('accountForm')" class="sure-btn">添加</el-button>
            </div>
        </el-dialog>

    </div>
</template>

<script>
    export default {
        name: "BluetoothManagement",
        data() {
            return {
                multipleSelection:[],
                exportListData:[],
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },
                rowItem:'',
                messagenum:'',
                messagename:'',
                bluetoothVisible:false,
                currentPage1:1,
                tableData: [{
                    order:'21235687987898',
                    name: '蓝牙设备UUID',
                }, {
                    order:'21235687987898',
                    name: '蓝牙设备UUID',
                }, {
                    order:'21235687987898',
                    name: '蓝牙设备UUID',

                }, {
                    order:'21235687987898',
                    name: '蓝牙设备UUID',

                }],
                accountForm: {
                    uuid:'',
                },recerules: {
                    uuid: [
                        {required: false, message: '蓝牙设备UUID不能为空', trigger: 'blur'}
                    ]
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
                    self.axios.post('api/Dormitory/DeleteSchoolDevice', {
                        ids: ids.split(',')
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
                    self.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            userOption(item) {
                let self=this;
                self.bluetoothVisible = true;
                if(item.id == undefined){
                    self.rowItem = '';
                    self.accountForm.uuid = '';
                }else {
                    self.rowid = item.id;
                    self.rowItem = item;
                    self.accountForm.uuid = item.uuid;
                }
            },
            handleDelete(index, row) {
                let self = this;
                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/SignIn/DeleteIBeaconInfo', {
                        id: row.id
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
                    self.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            tableList(){
                let self = this;
                self.axios.get('api/SignIn/GetIBeaconInfoToSchoolCode', {
                    params: {
                        code: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            self.configTable.tableArr = res.aaData;
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
                        if(!this.rowItem){
                            self.axios.post('api/SignIn/AddIBeaconInfo', {
                                schoolCode: localStorage.schoolcode,
                                uuid: this.accountForm.uuid
                            })
                                .then(function (response) {
                                    console.log(response);
                                    debugger;
                                    if(response.data.code == '000000'){
                                        self.editVisible = false;
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
                            self.axios.post('api/SignIn/UpdateIBeaconInfo', {
                                schoolCode: localStorage.schoolcode,
                                id:self.rowItem.id,
                                uuid: self.accountForm.uuid
                            })
                                .then(function (response) {
                                    if(response.data.code == '000000'){
                                        self.editVisible = false;
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
                        }
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