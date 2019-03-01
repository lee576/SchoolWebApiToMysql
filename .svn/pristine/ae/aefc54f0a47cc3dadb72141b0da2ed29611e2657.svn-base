<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word" style="display: flex">
                <el-breadcrumb separator-class="el-icon-arrow-right" style="margin-top: 13px;margin-right: 20px">
                    <el-breadcrumb-item>宿舍管理</el-breadcrumb-item>
                    <el-breadcrumb-item>管理</el-breadcrumb-item>
                </el-breadcrumb>
                <router-link to="/dormitoryadmin">
                    <el-button plain size="mini" class="details">
                        返回
                    </el-button>
                </router-link>
            </div>


            <div class="fr batch-operation">
                <a href="#" class="operation-btn" @click="addVisible = true">添加设施</a>
            </div>
        </div>
        <div class="page-content">
            <div class="one-part-table">
                <div class="one-row clearfix">
                    <div class="fl">
                        <div class="fl search-input"><input type="text" class="se-input"
                                                            placeholder="请输入需要查询的设施名称" v-model="theinput">
                        </div>
                        <div class="fr search-button">
                            <button class="btn-pro" @click="sureTheinput">搜索</button>
                        </div>
                    </div>
                    <el-button type="primary" size="small" style="float: right;margin-right: 20px"
                               @click="openAlldelete">批量删除
                    </el-button>

                </div>
                <div class="table-box" style="border: none">
                    <template>
                        <el-table :data="publicarea" stripe style="width: 100%;border: 1px solid #ddd;"
                                  @selection-change="handleSelectionChange">
                            <el-table-column
                                    type="selection"
                                    width="55">
                            </el-table-column>
                            <el-table-column prop="building_room_no" label="设施名称">
                            </el-table-column>
                            <el-table-column prop="id" label="门禁编码" :formatter="ifendcase">
                            </el-table-column>
                            <el-table-column label="操作">
                                <template slot-scope="scope">
                                    <el-button type="text" size="small" class="edit-tr-con"
                                               @click="handleEdit(scope.$index, scope.row)">修改
                                    </el-button>
                                    <el-button type="text" size="small" class="del-tr-con"
                                               @click="handleDelete(scope.$index, scope.row)">删除
                                    </el-button>
                                </template>
                            </el-table-column>
                        </el-table>
                        <div class="page-pagination">
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
                    </template>
                </div>
            </div>
        </div>

        <el-dialog title="修改设施" :visible.sync="editVisible">
            <el-form :model="editform">
                <el-form-item label="设施名称">
                    <el-input v-model="editform.name" autocomplete="off"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="editVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureEdit">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog title="添加设施" :visible.sync="addVisible">
            <el-form :model="addform">
                <el-form-item label="设施名称">
                    <el-input v-model="addform.name" autocomplete="off"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="addVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureAdd">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog
                title="删除设施"
                :visible.sync="deleteVisible"
                width="30%">
            <span>确定是否删除</span>
            <span slot="footer" class="dialog-footer">
            <el-button @click="deleteVisible = false">取 消</el-button>
            <el-button type="primary" @click="SureDelete">确 定</el-button>
          </span>
        </el-dialog>
        <el-dialog
                title="批量删除设施"
                :visible.sync="alldeleteVisible"
                width="30%">
            <span>确定是否删除</span>
            <span slot="footer" class="dialog-footer">
            <el-button @click="alldeleteVisible = false">取 消</el-button>
            <el-button type="primary" @click="SureAllDelete">确 定</el-button>
          </span>
        </el-dialog>

    </div>
</template>

<script>
    export default {
        name: "PublicFacilities",

        data() {
            return {
                uploadData: {
                    schoolcode: localStorage.schoolcode
                },
                dateChoose: true,
                dateArr1: [],
                theinput: '',//输入设施名称
                publicarea: [{
                    building_room_no: '',
                    id: '',
                }],
                configTable: {
                    total: 0,
                    size: 10,
                    index: 0
                },
                multipleSelection: [],
                editform: {
                    name: '',
                    id: '',
                },
                addform: {
                    name: '',
                },
                deleteId: '',
                deleteAddid: [],
                editVisible: false,
                addVisible: false,
                deleteVisible: false,
                alldeleteVisible: false,
            };
        },


        created() {
            this.GetPublicArea()
        },
        methods: {
            //获取所有设施
            GetPublicArea() {
                this.axios.get(`/api/Dormitory/GetPublicArea`, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    },
                    params: {
                        sEcho: 1,
                        iDisplayStart: this.configTable.index,
                        iDisplayLength: this.configTable.size,
                        publicName: this.theinput
                    }
                }).then(res => {
                    console.log(res)
                    this.publicarea = res.data.data
                    this.configTable.total = res.data.iTotalRecords
                })
            },
            //搜索设施名称
            sureTheinput() {
                this.GetPublicArea()
            },
            // 分页
            handleSizeChange(val) {
                console.log(val)
                this.configTable.size = val
                this.GetPublicArea()
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                this.configTable.index = val
                this.GetPublicArea()
                console.log(`当前页: ${val}`);
            },
            //新增设施
            SureAdd() {
                const adddormitory = {
                    isPublic: 1,
                    name: this.addform.name,
                    pid: 0
                }
                this.axios.post(`/api/Dormitory/AddDormitory`, adddormitory, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    },
                }).then(res => {
                    console.log(res)
                    if (res.data.code == "000000") {
                        this.addVisible = false
                        this.addform.name = ""
                        this.GetPublicArea()
                    } else {
                        this.addform.name = ""
                    }
                })
            },
            handleEdit(index, row) {
                console.log(row)
                this.editform.name = row.building_room_no
                this.editform.id = row.id
                this.editVisible = true
            },
            //修改
            SureEdit() {
                const dormitory = {
                    name: this.editform.name,
                    id: this.editform.id
                }
                this.axios.post(`/api/Dormitory/UpdateDormitory`, dormitory, {
                    headers: {
                        schoolcode: localStorage.schoolcode
                    },
                }).then(res => {
                    console.log(res)
                    if (res.data.code = "000000") {
                        this.editVisible = false
                        this.GetPublicArea()
                    }
                })
            },
            handleDelete(index, row) {
                this.deleteVisible = true
                this.deleteId = row.id
            },
            //删除
            SureDelete() {
                this.axios.get(`/api/Dormitory/DeleteDormitory`, {
                    params: {
                        id: this.deleteId
                    },
                    headers: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    console.log(res)
                    if (res.data.code = "000000") {
                        this.deleteVisible = false
                        this.GetPublicArea()
                    }
                })
            },
            openAlldelete() {
                this.alldeleteVisible = true
                this.multipleSelection.map(item => {
                    this.deleteAddid.push(item.id)
                })
            },
            SureAllDelete() {
                const deleteAddid = {
                    ids:this.deleteAddid
                }
                console.log(deleteAddid)
                this.axios.post(`/api/Dormitory/DeleteDormitory`, deleteAddid).then(res => {
                    console.log(res)
                    if (res.data.code = "000000") {
                        this.alldeleteVisible = false
                        this.GetPublicArea()
                    }
                })
            },
            //选择表格
            handleSelectionChange(val) {
                this.multipleSelection = val;
                console.log(this.multipleSelection)
            },
            ifendcase(val){
                console.log(val.id)
                let id= val.id.toString().padStart(8, "0")
                return id
            }
        },


    }
</script>






