<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">食堂管理</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn" @click="userOption">添加食堂</button>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab clearfix">
                <div class="fl">
                    <div class="fl search-input"><input type="text" class="se-input" v-model="canteenName" @keyup.enter="search"  placeholder="请输入要查询的食堂及档口"></div>
                    <div class="fl search-button"><button class="btn-pro" @click="search()">搜索</button></div>
                </div>
            </div>
            <div class="two-part-box">
                <div class="one-part-tree">
                    <div class="ztree-icon" style="margin-top: 0;margin-bottom: 8px">
                        <span class="icon iconfont el-icon-tianjia" @click="addMessage"></span>
                        <span class="icon iconfont el-icon-icon-edit" @click="editMessage"></span>
                        <span class="icon iconfont el-icon-shanchu" @click="deleteMessage"></span>
                    </div>
                    <div class="tree_data">
                        <el-tree :data="dataArr"  accordion  :default-expanded-keys="[0]" :props="defaultProps"   @node-click="handleNodeClick"
                                 node-key="id"
                        ></el-tree>
                    </div>

                </div>
                <div class="one-part-table">
                    <div class="form-timerange">
                        <el-select placeholder="请选择消费食堂" style="width:180px" v-model="hallStr" @change="hallChange">
                            <el-option v-for="item in hallArr" :label="item.name" :value="item.id" :key="item.id"></el-option>
                        </el-select>
                    </div>
                    <div class="table-box" style="margin-top: 15px">
                        <template>
                            <el-table
                                    :data="configTable.tableArr"
                                    stripe
                                    style="width: 100%">
                                <el-table-column
                                        prop="SN"
                                        label="设备编号"
                                >
                                </el-table-column>
                                <el-table-column
                                        prop="stallName"
                                        label="所在档口"
                                >
                                </el-table-column>
                                <el-table-column
                                        prop="diningName"
                                        label="消费食堂">
                                </el-table-column>
                                <el-table-column  label="操作" width="140">
                                    <template slot-scope="scope">
                                        <el-button type="text" size="small" class="del-tr-con"  @click="tableDelete(scope.row.deviceid)">删除</el-button>
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
         <!--添加食堂-->
        <el-dialog title="添加食堂" :visible.sync="canteenVisible" width="600px" top="20vh">
            <div class="tk-gray tk-height">
                <el-form :model="hallForm" :rules="recerules" ref="hallForm" label-width="80px" class="demo-ruleForm">
                    <el-form-item label="食堂名称" prop="name">
                        <el-input v-model="hallForm.name" placeholder="请输入食堂名称"></el-input>
                    </el-form-item>
                    <el-form-item label="食堂介绍" prop="description">
                        <el-input  type="textarea" v-model="hallForm.description" placeholder="请输入食堂介绍"></el-input>
                    </el-form-item>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="canteenVisible = false">取 消</el-button>
                <el-button type="primary" @click="hallSubmit('hallForm')" class="sure-btn">添加</el-button>
            </div>
        </el-dialog>
        <!--添加档口-->
        <el-dialog title="添加档口" :visible.sync="stallsVisible" width="600px" top="20vh">
            <div class="tk-gray tk-height">
                <el-form :model="stallForm" :rules="stallrules" ref="accountForm" label-width="80px" class="demo-ruleForm">
                    <el-form-item label="档口名称" prop="name">
                        <el-input v-model="stallForm.name" placeholder="请输入档口名称"></el-input>
                    </el-form-item>
                    <el-form-item label="消费设置">
                        <el-radio-group v-model="stallForm.is_user">
                            <el-radio label="0">校园卡用户消费</el-radio>
                            <el-radio label="1">支持所有人消费</el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <el-form-item label="">
                        <div class="clearfix">
                            <div class="left-stall-on1 fl">身份类型</div>
                            <div class="left-stall-on2 fl">加价概要（不填或填写0则不允许消费）</div>
                        </div>
                        <div class="stall-input-number clearfix">
                            <div class="left-stall-on1 fl">学生卡</div>
                            <div class="left-stall-on2 fl">
                                <el-input-number v-model="stallForm.student" controls-position="right" @change="handleChange" :step="1" :min="0" :max="100"></el-input-number>
                                <p class="num-per">%</p>
                            </div>
                        </div>
                        <div class="stall-input-number clearfix">
                            <div class="left-stall-on1 fl">教师卡</div>
                            <div class="left-stall-on2 fl">
                                <el-input-number v-model="stallForm.student" controls-position="right" @change="handleChange" :min="1" :max="100"></el-input-number>
                                <p class="num-per">%</p>
                            </div>

                        </div>
                    </el-form-item>
                    <div class="high-set-on" style="margin-bottom: 15px;" :class="[{ active: isActive }]"  @click="toggleClass(isActive)">
                        <span  class="high-set-word" style="margin-left: 0">高级设置</span>
                        <span  class="high-set-img"></span>
                    </div>
                    <div :style="{display:(isActive?'block':'none')}">
                        <el-form-item label="餐补规则">
                            <el-radio-group v-model="stallForm.is_meal">
                                <el-radio label="0">无</el-radio>
                            </el-radio-group>
                        </el-form-item>
                    </div>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="stallsVisible = false">取 消</el-button>
                <el-button type="primary" @click="stallsVisible = false" class="sure-btn">添加</el-button>
            </div>
        </el-dialog>
        <!--添加设备-->
        <el-dialog title="添加设备" :visible.sync="equipmentVisible" width="600px" top="20vh">
            <div class="tk-gray tk-height">
                <el-form :model="equipmentForm" :rules="equipmentrules" ref="accountForm" label-width="80px" class="demo-ruleForm">
                    <el-form-item label="设备品牌">
                        <el-radio-group v-model="equipmentForm.is_kind">
                            <el-radio label="0">银通物联</el-radio>
                            <el-radio label="1">意锐</el-radio>
                            <el-radio label="2">鑫澳康</el-radio>
                            <el-radio label="3">禧云</el-radio>
                        </el-radio-group>
                    </el-form-item>
                    <el-form-item label="设备编号" prop="order">
                        <el-input v-model="equipmentForm.order" placeholder="请输入设备编号"></el-input>
                    </el-form-item>
                    <el-form-item label="所在档口">
                        测试档口
                    </el-form-item>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="equipmentVisible = false">取 消</el-button>
                <el-button type="primary" @click="equipmentVisible = false" class="sure-btn">添加</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    export default {
        name: "CanteenManagement",
        data() {
            return {
                dataArr:[],
                keysArr:[],
                defaultProps: {
                    children: "children",
                    label: "label"
                },
                hallStr:null,
                hallArr:[],
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },
                value1:'',
                value2:'',
                value3:'',
                nodeData:'',
                treeNode:'',
                canteenName: '',
                canteenVisible: false,
                stallsVisible:false,
                equipmentVisible:false,
                currentPage1:1,
                isActive:false,
                tableData: [{
                    order:'21235687987898',
                    address: '清真食堂',
                    stalls:"12302"
                }, {
                    order:'21235687987898',
                    address: '清真食堂',
                    stalls:"10260"
                }, {
                    order:'21235687987898',
                    address: '清真食堂',
                    stalls:"30236"
                }, {
                    order:'21235687987898',
                    address: '清真食堂',
                    stalls:"36100"
                }],
                hallForm: {
                    name:'',
                    description:'',
                },recerules: {
                    name: [
                        {required: true, message: '请输入食堂名称', trigger: 'blur'}
                    ],
                    description: [
                        {required: true, message: '请输入食堂介绍', trigger: 'blur'}
                    ],
                },
                stallForm: {
                    name:'',
                    is_user:'0',
                    student:0,
                    is_meal:'0'
                },stallrules: {
                    name: [
                        {required: true, message: '请输入档口名称', trigger: 'blur'}
                    ]
                },
                equipmentForm: {
                    is_kind:'0',
                    order:'',
                },equipmentrules: {
                    order: [
                        {required: true, message: '请输入设备编号', trigger: 'blur'}
                    ]
                }
            }
        },
        created() {
            //食堂树
            this.treeList();
            //食堂下拉列表
            this.hallList();
            //设备表
            this.tableList();
        },
        methods: {
            hallList(){
                let self = this;
                self.axios.get('api/Cashier/GetDinningHall', {
                    params: {
                        school_id: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '10000'){
                            self.hallArr = res.data;
                            self.hallArr.unshift({id:null , name: '全部'});
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
            hallChange(val){
                this.hallStr = val;
                this.tableList();
            },
            handleChange(value){
                console.log(value);
            },
            toggleClass(e){
                if(e){
                    this.isActive = false;
                }else{
                    this.isActive = true;
                }
            },
            treeList() {
                let self = this;
                self.axios.get('api/Cashier/GetDininglist', {
                    params: {
                        school_id: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        var arr = [];
                        if (res.code == '10000') {
                            arr.push(res.data);
                            self.dataArr = arr;
                            console.log(self.schoolCourts)
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
            handleNodeClick(data,Node,p) {
                let self = this;
                self.nodeData = data;
                self.treeNode = Node;
                console.log(self.nodeData);
                    console.log(Node);
                    // if(data.)
                // debugger;
            },
            tableList(){
                let self = this;
                self.axios.get('api/Cashier/Getdevice', {
                    params: {
                        school_id: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        dining_hall:'',
                        stall:'',
                        sn:'',
                        hallid:self.hallStr,
                        tallid:'',
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '10000'){
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
            search(){
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            },
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
            },
            tableDelete(id){
                this.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.axios.get('api/Cashier/DelDeldevice', {
                        params: {
                            id: id
                        }
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'success'
                                });
                                self.tableList();
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            userOption(){
                let self=this;
                self.canteenVisible = true;
            },
            hallSubmit(ruleForm){
                let self = this,url = '';
                if(self.nodeData.treeLever == '1'){
                    self.$refs[ruleForm].validate((valid) => {
                        if(valid){
                            self.axios.get('api/Cashier/Edit_dining_hall', {
                                params: {
                                    id: self.nodeData.id,
                                    name: self.hallForm.name,
                                    introduction:self.hallForm.description,
                                }
                            })
                                .then(function (response) {
                                    if(response.data.code == '000000'){
                                        self.$message({
                                            showClose: true,
                                            message: response.data.msg,
                                            type: 'success'
                                        });
                                        self.canteenVisible = false;
                                        self.tableList();
                                        self.nodeData.label = self.hallForm.name;
                                        self.nodeData.name = self.hallForm.name;
                                    }
                                })
                                .catch(function (error) {
                                    console.log(error);
                                });
                        }else {
                            return false;
                        }

                    })
                }else {
                    self.$refs[ruleForm].validate((valid) => {
                        if(valid){
                            self.axios.get('api/Cashier/Add_dining_hall', {
                                params: {
                                    school_id: localStorage.schoolcode,
                                    name: self.hallForm.name,
                                    introduction:self.hallForm.description,
                                }
                            })
                                .then(function (response) {
                                    if(response.data.code == '000000'){
                                        self.$message({
                                            showClose: true,
                                            message: response.data.msg,
                                            type: 'success'
                                        });
                                        self.canteenVisible = false;
                                        self.tableList();
                                        const newChild = { id: response.data.id, label: self.hallForm.name, children: [] };
                                        if (!self.nodeData.children) {
                                            this.$set(self.nodeData, 'children', []);
                                        }
                                        self.nodeData.children.push(newChild);
                                    }
                                })
                                .catch(function (error) {
                                    console.log(error);
                                });
                        }else {
                            return false;
                        }

                    })
                }

            },
            addMessage(){
                console.log(11111);
                console.log(this.nodeData)
                let self = this;
                if(self.nodeData.treeLever == '0'){
                    self.canteenVisible = true;
                    self.hallForm.name='';
                    self.hallForm.description='';
                }else if(self.nodeData.treeLever == '1'){
                    self.stallsVisible = true;
                }else  if(self.nodeData.treeLever == '2'){
                    self.equipmentVisible = true;
                }
            },
            editMessage(){
                let self = this;
                console.log(self.nodeData);
                if(self.nodeData.treeLever == '1'){
                    //食堂展示数据
                    self.hallData();
                    self.canteenVisible = true;
                }
            },
            deleteMessage(){
                let self = this;
                if(self.nodeData.treeLever == '1'){
                    self.hallDelete();
                }
            },
            hallData(){
                let self = this;
                self.axios.get('api/Cashier/Get_dining_hall', {
                    params: {
                       id:self.nodeData.id
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '10000'){
                            self.hallForm.name=res.data.name;
                            self.hallForm.description=res.data.introduction;
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
            hallDelete(){
                let self = this;
                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.get('api/Cashier/Del_dining_hall', {
                        params: {
                            id: self.nodeData.id
                        }
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'success'
                                });
                                const parent = self.treeNode.parent;
                                const children = parent.data.children || parent.data;
                                const index = children.findIndex(d => d.id === self.nodeData.id);
                                children.splice(index, 1);
                                self.tableList();
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            }
        },

    }
</script>

<style scoped>
    .tree_data{
        overflow: scroll;
    }
    .tree_data::-webkit-scrollbar {display:none;}

    .table-box{
        overflow: scroll;
    }
    .table-box::-webkit-scrollbar {display:none;}
</style>