<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">宿舍管理</a></div>
            <div class="fr batch-operation" @click="dialogVisible = true">
                <a href="#" class="operation-btn">上传宿舍</a>
            </div>
        </div>
        <div class="page-content">
            <div class="two-part-box">
                <div class="one-part-tree">
                    <div class="ztree-icon" style="margin-top: 0;margin-bottom: 20px">
                        <router-link to="/publicfacilities">
                            <button style="float:left;background: rgba(35,135,251,1);color: #fff;font-size: 14px;font-weight:200">
                                公共设施
                            </button>
                        </router-link>
                        <span class="icon iconfont el-icon-tianjia" @click="OpenaddTree"></span>
                        <span class="icon iconfont el-icon-icon-edit" @click="OpeneditTree"></span>
                        <span class="icon iconfont el-icon-shanchu" @click="OpendeleteTree"></span>
                    </div>
                    <div class="tree_data">
                        <el-tree :data="dataArr" :props="defaultProps" @node-click="handleNodeClick"
                                 accordion node-key="id"
                                 :default-expanded-keys="[2089]"></el-tree>
                    </div>

                </div>
                <div class="one-part-table" style="overflow: visible">
                    <div class="one-row clearfix">
                        <div class="school-select-box1 fl">
                            <el-select v-model="value1" filterable placeholder="请选择" disabled>
                                <el-option
                                        v-for="schoolCourt in schoolCourts"
                                        :key="schoolCourt.name"
                                        :label="schoolCourt.label"
                                        :value="schoolCourt.name">
                                </el-option>
                            </el-select>
                        </div>
                        <div class="school-xi-box1 fl">
                            <el-select v-model="value2" filterable placeholder="请选择"
                                       @change="changeSchooldepartment" disabled clearable>
                                <el-option
                                        v-for="schoolDepartment in schoolDepartments"
                                        :key="schoolDepartment.name"
                                        :label="schoolDepartment.label"
                                        :value="schoolDepartment.name">
                                </el-option>
                            </el-select>
                        </div>
                        <div class="fr search-one" style="margin-right: 0">
                            <div class="fl search-word1">分配状态</div>
                            <div class="fl" style="margin-right: 20px">
                                <el-select v-model="value" placeholder="请选择"
                                           @change="changecardchooseoptions">
                                    <el-option
                                            v-for="item in cardchooseoptions"
                                            :key="item.value"
                                            :label="item.label"
                                            :value="item.value">
                                    </el-option>
                                </el-select>
                            </div>
                            <div class="fl">
                                <div class="fl search-input">
                                    <input type="text" class="se-input"
                                           placeholder="请输入需要查询的学工号或姓名" v-model="theinput">
                                </div>
                                <div class="fr search-button">
                                    <button class="btn-pro" @click="sureTheinput">搜索</button>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="one-row clearfix">
                        <el-row style="text-align:right;margin-right: 5px;margin-bottom: 12px">
                            <el-button type="primary" @click="Allocation" size="small">批量分配导入</el-button>
                            <el-button type="primary" @click="CancelDistribution" size="small">批量取消分配</el-button>
                        </el-row>
                    </div>
                    <div class="table-box" style="border: none;">
                        <template>
                            <el-table :data="tableData" stripe
                                      style="width: 100%;border: 1px solid #ddd;"
                                      @selection-change="handleSelectionChange"
                                      height="480"
                                      :row-style="{height:'45px'}">
                                <el-table-column
                                        type="selection"
                                        width="55">
                                </el-table-column>
                                <el-table-column prop="user_name" label="姓名">
                                </el-table-column>
                                <el-table-column prop="student_id" label="学工号">
                                </el-table-column>
                                <el-table-column prop="floor_name" label="楼栋">
                                </el-table-column>
                                <el-table-column prop="room_name" label="宿舍号">
                                </el-table-column>
                                <el-table-column prop="access_code" label="门禁编码">
                                </el-table-column>
                            </el-table>

                        </template>
                    </div>
                    <div class="page-pagination" style="padding:0">
                        <el-pagination
                                @size-change="handleSizeChange"
                                @current-change="handleCurrentChange"
                                :current-page="configTable.index"
                                :page-size="configTable.size"
                                background
                                layout="prev, pager, next"
                                :total="configTable.total">
                        </el-pagination>
                    </div>
                </div>
            </div>
        </div>


        <!--批量导入-->
        <el-dialog title="上传宿舍" :visible.sync="dialogVisible" width="600px" top="20vh">
            <div class="gray-import">
                <div class="improt-word1">注意事项：</div>
                <ul class="improt-word2">
                    <li>1、必须按照指定的模板格式才能导入成功。</li>
                    <li>2、如果Excel中数据已存在，则会覆盖原有信息。</li>
                    <li>3、请按照对应的卡片导入数据。</li>
                    <li>4、请上传Excel后缀为.xlsx文件。</li>
                </ul>
                <div class="improt-word3">下载模板：<span @click="download"> 宿舍数据导入示例表</span></div>
            </div>
            <div class="gray-import" style="margin-top:10px;">
                <el-upload
                        class="upload-demo"
                        :drag='true'
                        :action=uploadUrl()
                        multiple
                        :headers="uploadData"
                        accept="xlsx"
                        :before-upload="beforeAvatarUpload"
                        :on-success="handleAvatarSuccess"
                >
                    <i class="el-icon-upload"></i>
                    <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                </el-upload>

            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="dialogVisible = false" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>

        <el-dialog title="宿舍人员分配导入" :visible.sync="AllocationVisible" width="600px" top="20vh">
            <div class="gray-import">
                <div class="improt-word1">注意事项：</div>
                <ul class="improt-word2">
                    <li>1、必须按照指定的模板格式才能导入成功。</li>
                    <li>2、如果Excel中数据已存在，则会覆盖原有信息。</li>
                    <li>3、请按照对应的卡片导入数据。</li>
                    <li>4、请上传Excel后缀为.xlsx文件。</li>
                </ul>
                <div class="improt-word3">下载模板：<span @click="download2"> 宿舍人员分配导入示例表</span></div>
            </div>
            <div class="gray-import" style="margin-top:10px;">
                <el-upload
                        class="upload-demo"
                        :drag='true'
                        :action=uploadSrc()
                        multiple
                        :headers="uploadData"
                        accept="xlsx"
                        :before-upload="beforeAvatarUpload"
                        :on-success="handleAvatarSuccess"
                >
                    <i class="el-icon-upload"></i>
                    <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                </el-upload>

            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="AllocationVisible = false">取 消</el-button>
                <el-button type="primary" @click="AllocationVisible = false" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>


        <el-dialog :title="addtitle" :visible.sync="addtreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-input v-model="addtreeinput" placeholder="请输入内容" @change="check"></el-input>
                <div class="err-plchod" style="font-size: 13px ;color:red;padding-top: 5px">{{errname}}</div>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="addtreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureaddtreeVisible" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog :title="edittitle" :visible.sync="edittreeVisible" width="600px" top="25vh">
            <el-form :model="editform">
                <el-form-item label="">
                    <el-input v-model="editform.name" autocomplete="off"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="edittreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureedittreeVisible">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog :title="deletetitle" :visible.sync="deletetreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <span>确定删除前，清先取消人员分配</span>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="deletetreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SuredeletetreeVisible" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    import {currency} from './../../../util/currency';
    import {getDateType} from './../../../util/getDate';

    // 引入提示框和title组件
    export default {
        name: "DormitoryAdmin",

        data() {
            return {
                uploadData: {schoolcode: localStorage.schoolcode,},
                room_no: '',
                floor_no: '',
                schoolCourts: [],//院
                schoolDepartments: [],//系
                dateChoose: true,
                stuArr: [],
                cardchooseoptions: [{
                    value: '0',
                    label: '已分配'
                }, {
                    value: '1',
                    label: '未分配'
                }],
                editform: {
                    name: "",
                    id: ""
                },

                errname: "",//树形验证
                gainaddtree: "",//获取新增父级内容
                addtreeinput: "",//新增内容
                edittreeinput: "",//编辑内容
                deletetreeinput: "",//删除内容
                addtitle: "",
                edittitle: "",
                deletetitle: "",
                isClass: '0',//新增选项
                value: '',//是否分配宿舍
                value1: '',
                value2: '',
                theinput: '',//输入学号或工号
                treeData: '',//点击获取树形图信息
                schoolcardtype: '',//类型
                dialogVisible: false,
                exportVisible: false,
                delectVisible: false,
                addtreeVisible: false,
                edittreeVisible: false,
                deletetreeVisible: false,
                AllocationVisible: false,
                tableData: [{
                    student_id: '',
                    user_name: '',
                    floor_name: '',
                    room_name: '',
                    access_code: '',
                }],
                isShowGraph: true,
                dataArr: [],
                configTable: {
                    total: 0,
                    size: 10,
                    index: 1
                },

                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
                selectedOptions: [],
                cardAccount: "",
                leaderCard: '',
                unCard: '',
                exportXlsx: [],
                multipleSelection: [],
            };
        },
        mounted() {
            /* this.drawCircle();*/
            this.getParams();
        },
        filters: {
            currency: currency,
            getDateType: getDateType
        },
        created() {
            this.cardData();
            this.init()
            this.GetDormitoryPeople()
            this.value = this.cardchooseoptions[0].value;//默认已分配
            this.axios.get(`/api/SchoolUser/InduceSchoolUserInfo`, {
                params: {
                    school_Code: localStorage.schoolcode,
                }
            }).then(res => {
                this.exportXlsx = res.data.aaData
            })


        },
        methods: {
            init() {
                this.axios.get(`/api/Dormitory/GetDormitory`, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    let item = res.data
                    if (item.code == "000000") {
                        console.log(item)
                        this.toTreeData(item.data, 'id', 'parent_id', 'building_room_no')
                    }
                })
            },
            // 循环出父节点
            toTreeData(data, id, pid, name) {
                // 建立个树形结构,需要定义个最顶层的父节点，pId是1
                let leve = 0
                let parent = [];
                this.dataArr = parent
                for (let i = 0; i < data.length; i++) {
                    if (data[i][pid] !== 0) {
                    } else {
                        let obj = {
                            label: data[i][name],
                            id: data[i][id],
                            parent_id: data[i][pid],
                            leve: leve,
                            children: []
                        };
                        parent.push(obj);//数组加数组值
                    }
                    // console.log(obj);
                    //  console.log(parent,"bnm");
                }
                children(parent, leve);

                // 调用子节点方法,参数为父节点的数组
                function children(parent, num) {
                    num++;
                    if (data.length !== 0 && num != 4) {
                        for (let i = 0; i < parent.length; i++) {
                            for (let j = 0; j < data.length; j++) {
                                if (parent[i].id == data[j][pid]) {
                                    let obj = {
                                        label: data[j][name],
                                        id: data[j][id],
                                        parent_id: data[j][pid],
                                        leve: num,
                                        children: []
                                    };
                                    parent[i].children.push(obj);
                                }
                            }
                            children(parent[i].children, num);
                        }
                    }
                    return num
                }

                console.log(parent, "bjil")
                return parent;
            },
            //获取已分配人员
            GetAllocationPeople() {
                let TableIndex = this.configTable.index * 10 - 10
                this.axios.get(`/api/Dormitory/GetBandingDormitory`, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    },
                    params: {
                        sEcho: 0,
                        iDisplayStart: TableIndex,
                        iDisplayLength: this.configTable.size,
                        floor_no: this.floor_no,
                        room_no: this.room_no,
                        studentIdentity: this.theinput,
                    }
                }).then(res => {
                    console.log(res)
                    this.tableData = res.data.aadata
                    this.configTable.total = res.data.iTotalRecords
                })
            },
            //获取未分配人员
            GetundistributedPeople() {
                let TableIndex = this.configTable.index * 10 - 10
                this.axios.get(`/api/Dormitory/GetUnBandingDormitory`, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    },
                    params: {
                        sEcho: 0,
                        iDisplayStart: TableIndex,
                        iDisplayLength: this.configTable.size,
                        floor_no: this.floor_no,
                        room_no: this.room_no,
                        studentIdentity: this.theinput,
                    }
                }).then(res => {
                    console.log(res)
                    this.tableData = res.data.aaData
                    this.configTable.total = res.data.iTotalRecords
                })
            },

            // 获取宿舍人员
            GetDormitoryPeople() {
                console.log(this.value)
                if (this.value == 0) {
                    this.GetAllocationPeople()
                } else {
                    this.GetundistributedPeople()
                }
            },

            changeSchoolcourt() {

            },
            changeSchooldepartment() {

            },
            //分配状态
            changecardchooseoptions() {
                this.GetDormitoryPeople()
            },
            sureTheinput() {
                if (this.value == 0) {
                    this.GetAllocationPeople()
                } else {
                    this.GetundistributedPeople()
                }
            },
            //批量分配导入
            Allocation() {
                this.AllocationVisible = true

            },
            //批量取消分配
            CancelDistribution() {
                this.multipleSelection.map(item => {
                    const id = {
                        id: item.id
                    }
                    this.axios.post(`/api/Dormitory/DeleteDormitory`, id).then(res => {
                        console.log(res)
                    })
                    const name = {
                        name: item.name
                    }
                    this.axios.post(`/api/Dormitory/DeleteBandingDormitory`, name).then(res => {
                        console.log(res)
                    })
                })
            },

            //打开新增树形
            OpenaddTree() {
                if (this.addtitle == "") {
                    this.$message({
                        message: '请点击进入学校新增',
                        type: 'warning'
                    });
                } else {
                    if (this.treeData.leve == 2) {
                        this.$message({
                            message: '已到最底层，无法新增',
                            type: 'warning'
                        });
                    } else {
                        this.addtreeVisible = true
                        this.gainaddtree = this.treeData
                    }

                }
            },
            //新增树形
            SureaddtreeVisible() {
                const adddata = {
                    name: this.addtreeinput,
                    pid: this.gainaddtree.id
                }
                this.axios.post(`/api/Dormitory/AddDormitory`, adddata, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        this.init()
                        this.addtreeinput = ""
                        this.addtreeVisible = false
                    }
                    console.log(res)
                })
            },
            //打开编辑树形
            OpeneditTree() {
                if (this.edittitle == "") {
                    this.$message({
                        message: '请勿更改学校名字',
                        type: 'warning'
                    });
                } else {
                    this.edittreeVisible = true
                    this.editform.id = this.treeData.id
                    this.editform.name = this.treeData.label
                }
            },
            //编辑树形
            SureedittreeVisible() {
                const editdata = {
                    id: this.editform.id,
                    name: this.editform.name
                }
                this.axios.post(`/api/Dormitory/UpdateDormitory`, editdata, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        console.log(res)
                        this.init()
                        this.edittreeVisible = false
                    }
                })
            },
            //打开删除树形
            OpendeleteTree() {
                if (this.deletetitle == "") {
                    this.$message({
                        message: '请勿删除学校',
                        type: 'warning'
                    });
                } else {
                    this.deletetreeVisible = true
                    this.deletetreeinput = this.treeData
                }
            },
            //删除树形
            SuredeletetreeVisible() {
                console.log(this.deletetreeinput)
                this.axios.get(`/api/Dormitory/DeleteDormitory`, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    },
                    params: {
                        id: this.deletetreeinput.id
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        this.init()
                        this.deletetreeVisible = false
                    }
                    console.log(res)
                })
                // this.axios.post(`/api/Dormitory/DeleteBandingDormitory`,)
            },
            // 分页
            handleSizeChange(val) {
                console.log(val)
                this.configTable.size = val
                if (this.value == 0) {
                    this.GetAllocationPeople()
                } else {
                    this.GetundistributedPeople()
                }
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                this.configTable.index = val
                if (this.value == 0) {
                    this.GetAllocationPeople()
                } else {
                    this.GetundistributedPeople()
                }
                console.log(`当前页: ${val}`);
            },

            check: function () {
                // if (this.addtreeinput != "") {
                //     this.errname = "";
                // } else {
                //     this.errname = "用户名不能为空";
                // }
            },
            uploadUrl() {
                var url = this.axios.defaults.baseURL + "/api/Dormitory/UploadDormitory";
                return url;
            },
            uploadSrc() {
                var src = this.axios.defaults.baseURL + "/api/Dormitory/UploadBandingDormitory";
                return src;
            },

            beforeAvatarUpload(file) {
                console.log(file)
            },
            handleAvatarSuccess(res, file) {
                console.log(res)
                if (res.code=="000000"){
                    this.$message({
                        message: res.data,
                        type: 'success'
                    });
                }else if (res.code=="111111"){
                    this.$message({
                        message:res.data,
                        type: 'success'
                    });
                }

            },

            //点击表格头
            handleSelectionChange(val) {
                this.multipleSelection = val;
                console.log(this.multipleSelection)
            },


            getParams() {
                // 取到路由带过来的参数
                const linkR = this.$route.query.linkR;
                // 将数据放在当前组件的数据内

                if (linkR) {
                    this.isShowGraph = false;
                }

            },

            //点击树状图
            handleNodeClick(data) {
                console.log(data);
                if (data.leve == 0) {
                    this.addtitle = "增加楼栋"
                }
                if (data.leve == 1) {
                    this.addtitle = "增加宿舍"
                    this.edittitle = "编辑楼栋"
                    this.deletetitle = "删除楼栋"
                    this.value1 = data.label
                    this.value2 = ""
                }
                if (data.leve == 2) {
                    this.edittitle = "编辑宿舍"
                    this.deletetitle = "删除宿舍"
                    this.value2 = data.label
                }
                this.treeData = data
                if (this.value == 0 && data.leve == 0) {
                    this.floor_no = ""
                    this.room_no = ""
                    this.GetAllocationPeople()
                    console.log(1)
                } else if (this.value == 1 && data.leve == 0) {
                    this.floor_no = ""
                    this.room_no = ""
                    this.GetundistributedPeople()
                    console.log(2)
                } else if (this.value == 0 && data.leve == 1) {
                    this.floor_no = data.id
                    this.room_no = ""
                    this.GetAllocationPeople()
                    console.log(3)
                } else if (this.value == 1 && data.leve == 1) {
                    this.floor_no = data.id
                    this.room_no = ""
                    this.GetundistributedPeople()
                    console.log(4)
                } else if (this.value == 0 && data.leve == 2) {
                    this.room_no = data.id
                    this.GetAllocationPeople()
                    console.log(5)
                } else if (this.value == 1 && data.leve == 2) {
                    this.room_no = data.id
                    this.GetundistributedPeople()
                    console.log(6)
                }
            },
            formatJson(filterVal, jsonData) {
                return jsonData.map(v => filterVal.map(j => v[j]))
            },
            // 点击下载
            download() {
                let self = this;
                self.downloadLoading = true;
                require.ensure([], () => {
                    const {export_json_to_excel} = require('../../../assets/js/Export2Excel');
                    const tHeader = ['宿舍信息'];
                    const filterVal = ['class_id'];
                    const list = [{
                        class_id: "3栋/101房间",
                    }];  //表格数据，iview中表单数据也是这种格式！
                    const data = self.formatJson(filterVal, list);
                    export_json_to_excel(tHeader, data, '宿舍模版') //列表excel  这个是导出表单的名称
                    self.downloadLoading = false
                });
            },
            download2() {
                let self = this;
                self.download2Loading = true;
                require.ensure([], () => {
                    const {export_json_to_excel} = require('../../../assets/js/Export2Excel');
                    const tHeader = ['学工号', '姓名', '楼栋名称', '房间名称'];
                    const filterVal = ['class_id', 'user_name', 'floor_no', 'room_no'];
                    const list = [{
                        class_id: "20180824999",
                        user_name: "江小强",
                        floor_no: "5栋",
                        room_no: "101宿舍",
                    }];  //表格数据，iview中表单数据也是这种格式！
                    const data = self.formatJson(filterVal, list);
                    export_json_to_excel(tHeader, data, '宿舍人员分配导入') //列表excel  这个是导出表单的名称
                    self.download2Loading = false
                });
            },
            cardData() {
                let self = this;
                self.axios.get('api/SchoolCodr/SchoolCardCount', {
                    params: {
                        school_id: localStorage.schoolcode
                    }
                })
                    .then(function (response) {
                        if (response.data.code == '000000') {
                            let res = response.data;
                            self.cardAccount = res.card_count;
                            self.leaderCard = res.registered_count;
                            self.unCard = res.unregistered_count;
                        } else {
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
        }
        ,
    }
</script>

<style scoped>

    .tree_data {
        overflow: scroll
    }

    .tree_data::-webkit-scrollbar {
        display: none
    }

    .table-box {
        overflow: scroll
    }

    .table-box::-webkit-scrollbar {
        display: none
    }
</style>

