<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">用户权限管理</a></div>
            <div class="fr batch-operation">
                <router-link to="/UserRightsAdd">
                    <button class="operation-btn">添加权限</button>
                </router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab clearfix">
                <ul class="nav-tab-change1 fl">
                    <li :class="{active:isShowGraph}" @click="isShowGraph=true">
                        <label class="fl view-img1"></label>
                        <span class="fl">视图</span>
                    </li>
                    <li :class="{active:!isShowGraph}" @click="isShowGraph=false">
                        <label class="fl view-img2"></label>
                        <span class="fl">列表</span>
                    </li>
                </ul>
                <div class="fl">
                    <div class="fl search-input"><input type="text" class="se-input" v-model="searchdata"
                                                        placeholder="请输入姓名、登录账号或角色名"></div>
                    <div class="fl search-button">
                        <button class="btn-pro" @click="search(searchdata)">搜索</button>
                    </div>
                </div>
                <ul class="nav-tab-change2 add-ul-user fl" style="margin-left: 15px;">
                    <li class="active">全部</li>
                    <li>普通用户</li>
                    <li>食堂经理</li>
                    <li>财务</li>
                </ul>
            </div>
            <ul class="view-tree-box">
                <!--视图-->
                <li class="ym-view-box" :style="{display:isShowGraph?'block':'none'}">
                    <el-row :gutter="10" v-for="userdata in UserDatas">
                        <el-col :xs="24" :sm="24" :md="12" :lg="8" :xl="6">
                            <div class="gray-user-box">
                                <div class="clearfix">
                                    <div class="fl user-on-num1">姓名 : {{userdata.userName}}</div>
                                    <div class="fr user-on-num2">角色名 : <span
                                            class="user-on-cor1">{{userdata.role}}</span></div>
                                </div>
                                <div class="two-row-mg clearfix">
                                    <div class="fl user-on-num3">登录账号 :{{userdata.loginuser}}</div>
                                    <div class="fr two-row-on3">
                                        <a href="#" class="row-color2" @click="UserEdit(userdata.id)">修改</a>
                                        <a href="#" class="row-color3">删除</a>
                                    </div>
                                </div>
                            </div>
                        </el-col>
                    </el-row>
                </li>
                <!--列表-->
                <li class="ym-tree-box" :style="{display:isShowGraph?'none':'block'}">
                    <div class="payment-item-table">
                        <div class="table-box">
                            <template>
                                <el-table
                                        :data="tableData"
                                        stripe
                                        style="width: 100%">
                                    <el-table-column
                                            prop="userName"
                                            label="姓名"
                                    >
                                    </el-table-column>
                                    <el-table-column
                                            prop="loginuser"
                                            label="登录账号"
                                    >
                                    </el-table-column>
                                    <el-table-column
                                            prop="role"
                                            label="角色名">
                                    </el-table-column>
                                    <el-table-column label="操作" width="120">
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
                            </template>
                        </div>
                        <div class="block" style="text-align: right;">
                            <el-pagination
                                    @size-change="handleSizeChange"
                                    @current-change="handleCurrentChange"
                                    :current-page="configTable.index"
                                    :page-size="configTable.size"
                                    layout="total, prev, pager, next"
                                    :total="configTable.total">
                            </el-pagination>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    export default {
        name: "UserRights",
        data() {
            return {
                configTable: {
                    total: 0,
                    size: 10,
                    index: 0
                },
                isShowGraph: true,
                UserDatas: [],
                searchdata: '',//搜索输入框
                tableData: [{
                    userName: "",
                    loginuser: "",
                    role: "",
                    id: ""
                },],
            }
        },
        created() {
            this.init()
        },
        methods: {
            init() {
                this.axios.get(`/api/UserRightMange/GetUserList`, {
                    params: {
                        iDisplayStart: this.configTable.index,
                        iDisplayLength: this.configTable.size,
                        userNameOrLoginuserOrRole:this.searchdata
                    },
                    headers: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    console.log(res)
                    if (res.data.code == "000000") {
                        this.UserDatas = res.data.data
                        this.tableData = res.data.data
                        this.configTable.total = res.data.iTotalRecords
                    }
                })
            },
            handleSizeChange(val) {
                this.configTable.size = val
                this.init()
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                this.configTable.index = val
                this.init()
                console.log(`当前页: ${val}`);
            },
            //修改、删除
            UserEdit(id) {
                console.log(id)
                this.$router.push({name: 'UserRightsEdit', params: {id: id}})
            },
            handleEdit(index, row) {
                console.log(row)
                this.$router.push({name: 'UserRightsEdit', params: {id: row.id}})
            },
            handleDelete(index, row) {
                this.axios.get(`/api/UserRightMange/DeleteById`,{
                    params:{
                        id:row.id
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        this.UserDatas.splice(index, 1)
                        this.tableData.splice(index, 1)
                    }
                    console.log(res)
                })
            },
            //搜索
            search(data) {
                console.log(data)
                this.init()
            }
        },

    }
</script>

<style scoped>

</style>