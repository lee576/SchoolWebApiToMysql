<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">学校功能设置</a></div>
        </div>
        <div class="page-content">
            <div class="add-page-box">
                <div class="add-page">
                    <div class="add-page-title">学校功能设置</div>
                    <el-form :model="sfunctionForm" :rules="sfunctionrules" ref="ruleForm" label-width="110px" class="demo-ruleForm">
                        <div class="gray-box-part">
                            <el-form-item label="项目编号" prop="paccount">
                                <el-input v-model="sfunctionForm.paccount"  placeholder="请输入项目编号"></el-input>
                            </el-form-item>
                            <el-form-item label="禧云编号" prop="xaccount">
                                <el-input v-model="sfunctionForm.xaccount"  placeholder="请输入禧云编号"></el-input>
                                <div class="xaccount-tip">可输入多个项目编号，用逗号隔开</div>
                            </el-form-item>

                        </div>
                        <el-form-item label="是否授权" prop="grant">
                            <el-switch v-model="sfunctionForm.grant"></el-switch>
                        </el-form-item>
                        <el-form-item label="是否分批支付" prop="batches">
                            <el-switch v-model="sfunctionForm.batches"></el-switch>
                        </el-form-item>
                        <el-form-item>
                            <el-button class="btn-bg">修改</el-button>
                            <el-button class="btn-border">取消</el-button>
                        </el-form-item>
                    </el-form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "SchoolFunction" ,
        data() {
            return {
                sfunctionForm: {
                    paccount:'',
                    xaccount:'',
                    grant:false,
                    batches:true,
                },sfunctionrules:{
                    paccount: [
                        {required: true, message: '请输入项目编号', trigger: 'blur'}
                    ],xaccount: [
                        {required: true, message: '请输入禧云编号', trigger: 'blur'}
                    ],grant: [
                        {required: true, message: '请选择是否授权', trigger: 'blur'}
                    ],batches: [
                        {required: true, message: '请选择是否分批支付', trigger: 'blur'}
                    ]
                }
            }
        }
    }
</script>

<style scoped>

</style>