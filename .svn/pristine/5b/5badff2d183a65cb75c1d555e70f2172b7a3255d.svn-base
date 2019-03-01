<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">签约管理</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn" @click="contractVisible = true">新增签约</button>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab clearfix">
                <div class="fl">
                    <div class="fl search-input"><input type="text" class="se-input" v-model="feeName"   placeholder="请输入姓名、登录账号或角色名"></div>
                    <div class="fl search-button"><button class="btn-pro">搜索</button></div>
                </div>
                <div class="fr right-btn-ta">
                    <button class="btn" @click="transferVisible = true">批量转移</button>
                </div>
            </div>
            <div class="payment-item-table">
                <div class="table-box">
                    <template>
                        <el-table
                                :data="tableData"
                                stripe
                                style="width: 100%">
                            <el-table-column
                                    type="selection"
                                    width="55">
                            </el-table-column>
                            <el-table-column
                                    prop="order"
                                    label="签约人姓名"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="address"
                                    label="签约学校名称"
                            >
                            </el-table-column>
                            <el-table-column
                                    prop="stalls"
                                    label="签约时间">
                            </el-table-column>
                            <el-table-column  label="操作" width="140">
                                <template slot-scope="scope">
                                    <el-button type="text" size="small" class="edit-tr-con">修改</el-button>
                                    <el-button type="text" size="small" class="del-tr-con">转移</el-button>
                                </template>
                            </el-table-column>
                        </el-table>
                    </template>
                </div>
                <div class="block" style="text-align: right;">
                    <el-pagination
                            @size-change="handleSizeChange"
                            @current-change="handleCurrentChange"
                            :current-page.sync="currentPage1"
                            :page-size="100"
                            layout="total, prev, pager, next"
                            :total="1000">
                    </el-pagination>
                </div>
            </div>
        </div>


        <!--新增签约-->
        <el-dialog title="新增签约" :visible.sync="contractVisible" width="600px" top="25vh">
            <div class="tk-gray tk-height">
                <el-form :model="contractForm" :rules="contractrules" ref="contractrules" label-width="110px" class="demo-ruleForm">
                    <el-form-item label="签约人姓名" prop="name">
                        <el-input v-model="contractForm.name" placeholder="请输入签约人姓名"></el-input>
                    </el-form-item>
                    <el-form-item label="签约学校名称" prop="description">
                        <el-input v-model="contractForm.description" placeholder="请输入签约学校名称"></el-input>
                    </el-form-item>
                    <el-form-item label="签约时间" prop="date">
                        <el-date-picker
                                v-model="contractForm.date"
                                type="date"
                                placeholder="请选择签约时间" style="width:100%">
                        </el-date-picker>
                    </el-form-item>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="contractVisible = false">取 消</el-button>
                <el-button type="primary" @click="contractVisible = false" class="sure-btn">添加</el-button>
            </div>
        </el-dialog>

        <!--批量转移-->
        <el-dialog title="批量转移" :visible.sync="transferVisible" width="600px" top="25vh">
            <div class="tk-gray tk-height">
                <el-form :model="transferForm" :rules="transferrules" ref="transferrules" label-width="110px" class="demo-ruleForm">
                    <el-form-item label="签约人姓名" prop="name">
                        <el-select v-model="transferForm.name" placeholder="请选择签约人姓名" style="width: 100%">
                            <el-option label="张三" value="zhangsan"></el-option>
                            <el-option label="李四" value="lisi"></el-option>
                        </el-select>
                    </el-form-item>
                </el-form>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="transferVisible = false">取 消</el-button>
                <el-button type="primary" @click="transferVisible = false" class="sure-btn">添加</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    export default {
        name: "ContractManagement",
        methods: {
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
            },
        },
        data() {
            return {
                feeName:'',
                contractVisible:false,
                transferVisible:false,
                currentPage1:1,
                tableData: [{
                    order:'王小丫',
                    address: '中南民族大学',
                    stalls:"12302"
                }, {
                    order:'王小丫',
                    address: '中南民族大学',
                    stalls:"10260"
                }, {
                    order:'王小丫',
                    address: '中南民族大学',
                    stalls:"30236"
                }, {
                    order:'王小丫',
                    address: '中南民族大学',
                    stalls:"36100"
                }],
                contractForm: {
                    name:'',
                    description:'',
                    date:''
                },contractrules: {
                    name: [
                        {required: true, message: '请输入签约人姓名', trigger: 'blur'}
                    ],description: [
                        {required: true, message: '请输入签约学校名称', trigger: 'blur'}
                    ],date: [
                        {required: true, message: '请选择签约时间', trigger: 'blur'}
                   ]
                },
                transferForm: {
                    name:'',
                },transferrules: {
                    name: [
                        {required: true, message: '请输入签约人姓名', trigger: 'blur'}
                    ]
                },
            }
        }
    }
</script>

<style scoped>

</style>