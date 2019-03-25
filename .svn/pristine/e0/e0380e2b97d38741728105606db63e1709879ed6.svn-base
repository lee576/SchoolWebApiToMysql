<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">补考管理</a></div>
            <div class="fr payable-btn">
                <button class="operation-btn" @click="userOption()">导出EXCEL</button>
                <button class="operation-btn" @click="userOption()">批量导入</button>
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
                        <label class="fl view-img3"></label>
                        <span class="fl">列表</span>
                    </li>
                </ul>
                <span   style="height:20px" class="fl">
                    <el-input v-model="input" placeholder="请输入内容" style="height:20px"></el-input>
                </span>
                <span   style="height:20px" class="fl">
                    <el-input v-model="input" placeholder="请输入内容" style="height:20px"></el-input>
                </span>
            </div>
            <ul class="view-tree-box">
                <!--视图-->
                <li class="ym-view-box" :style="{display:isShowGraph?'block':'none'}">
                    <div class="no-message-tip" :style="{display:isShow?'block':'none'}">
                        <img src="../../../assets/picture/images/no-mess.png" height="100" width="100"/>
                        <div class="message-tip-word">暂无内容</div>
                    </div>
                    <el-row :gutter="10">
                        <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                            <div class="pay-card" style="color:red; ">
                                <div style="display: flex;justify-content: space-between;">
                                    <div style="opacity: 0.8">补考批号：201901098987</div>
                                    <div>补考人数 <span style="margin: 0 2px;color: black">89</span>/126</div>
                                </div>
                                <div style="font-size: 20px;margin: 8px 0;color: #717171;">英语3</div>
                                <div style="display: flex;justify-content: space-between">
                                    <div>报名金额：￥40.00</div>
                                    <div style="font-size: 20px;">
                                        总计：￥40.00
                                    </div>
                                </div>
                                <div style="display: flex;justify-content: space-between;margin-top: 5px">
                                    <div style="opacity: 0.8">补考报名截至时间：2019.3.20</div>
                                    <div style="display: flex;">
                                        <div style="color:rgba(35,135,251,1);cursor:pointer">查看</div>
                                        <div style="color:rgba(51,188,92,1);margin: 0 10px;cursor:pointer">导出</div>
                                        <div style="cursor:pointer">删除</div>
                                    </div>
                                </div>
                            </div>
                        </el-col>
                    </el-row>
                    <div class="block" style="text-align: right;">
                        <el-pagination
                                background
                                @size-change="handleSizeChange"
                                @current-change="handleCurrentChange"
                                :current-page.sync="configTable.currentPage"
                                :page-size="configTable.iDisplayLength"
                                layout="total, prev, pager, next"
                                :total="configTable.total">
                        </el-pagination>
                    </div>
                </li>
                <!--树状图-->
                <li class="ym-tree-box" :style="{display:isShowGraph?'none':'block'}">
                    <div class="payment-item-table">
                        <div class="table-box">
                            <template>
                                <el-table :data="configTable.tableArr" stripe style="width: 100%">
                                    <el-table-column prop="name" label="补考批号">
                                    </el-table-column>
                                    <el-table-column prop="appid" label="补考科目">
                                    </el-table-column>
                                    <el-table-column prop="pid" label="补考人数">
                                    </el-table-column>
                                    <el-table-column prop="accountstatus" label="已报名人数">
                                    </el-table-column>
                                    <el-table-column prop="pid" label="报名金额">
                                    </el-table-column>
                                    <el-table-column prop="pid" label="合计实收">
                                    </el-table-column>
                                    <el-table-column prop="pid" label="补考报名截至时间">
                                    </el-table-column>
                                    <el-table-column label="操作" width="120">
                                        <template slot-scope="scope">
                                            <el-button type="text" size="small" class="edit-tr-con"
                                                       @click="TableExamine(scope.row.id)">查看
                                            </el-button>
                                            <el-button type="text" size="small" class="edit-tr-con"
                                                       @click="TableExport(scope.row.id)">导出
                                            </el-button>
                                            <el-button type="text" size="small" class="del-tr-con"
                                                       @click="TableDelete(scope.row.id)">删除
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
                                    :current-page.sync="configTable.currentPage"
                                    :page-size="configTable.iDisplayLength"
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
        name: "SupplementaryExamination",
        data() {
            return {
                isShowGraph: true,
                isShow: false,
                input:"",//输入内容
                configTable: {
                    tableArr: [],
                    total: 0,
                    currentPage: 1,
                    iDisplayStart: 0,
                    iDisplayLength: 10,
                },
            }
        },
        created() {
        },
        methods: {
            //分页
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
            },
            //Table查看
            TableExamine() {

            },
            //Table导出
            TableExport() {

            },
            //Table删除
            TableDelete() {

            },

        },
    }
</script>

<style scoped>

</style>