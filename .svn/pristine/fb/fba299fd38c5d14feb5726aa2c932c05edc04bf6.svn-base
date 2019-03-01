<template>
    <div>
        <div class="company-logo-tip clearfix">
            <img src="../assets/images/logo.png" class="fl logo-img">
            <div class="fl logo-word">校园管理后台</div>
        </div>
        <el-menu default-active="1"
                 :unique-opened="true"
                 class="el-menu-vertical-demo"
                 background-color=" #464d70"
                 text-color="#fff"
                 active-text-color="#23c0fb"
                 @open="handleOpen"
                 @close="handleClose"
                 :router="true"
                 style="border: none">
            <el-submenu index="1">
                <template slot="title">电子校园卡</template>
                <el-menu-item index="/cardsurvey">概览</el-menu-item>
                <el-menu-item index="/cardpersonnel">校园卡人员管理</el-menu-item>
                <el-menu-item index="/cardindex">校园卡管理</el-menu-item>
            </el-submenu>
            <el-submenu index="2">
                <template slot="title">缴费大厅</template>
                <el-menu-item index="/paymentsurvey">概览</el-menu-item>
                <el-menu-item index="/routineadmin">常规缴费项管理</el-menu-item>
                <el-menu-item index="/CommonSearchList">常规缴费查询</el-menu-item>
                <el-menu-item index="/PayableManagement">应缴款项</el-menu-item>
                <el-menu-item index="/ReceivingManage_List">收款账号管理</el-menu-item>
                <el-menu-item index="/Electricitycharge_List">电费管理</el-menu-item>
                <el-menu-item index="/Waterfee_List">水费管理</el-menu-item>
            </el-submenu>
            <el-submenu index="3">
                <template slot="title">收银台</template>
                <el-menu-item index="/record">概览</el-menu-item>
                <el-menu-item index="/Transactionflow">交易流水</el-menu-item>
                <el-menu-item index="/CanteenManagement">食堂管理</el-menu-item>
                <el-menu-item index="/FundManagement">资金管理</el-menu-item>
            </el-submenu>
            <el-submenu index="4">
                <template slot="title">宿舍管理</template>
                <el-menu-item index="/dormitoryadmin">宿舍管理</el-menu-item>
            </el-submenu>
            <el-submenu index="5">
                <template slot="title">小程序管理</template>
                <el-menu-item index="/AccessControl">门禁管理</el-menu-item>
                <el-menu-item index="/BluetoothManagement">蓝牙设备管理</el-menu-item>
                <el-menu-item index="/SignIn">签到管理</el-menu-item>
                <el-menu-item index="/CourseSchedule">课程表管理</el-menu-item>
            </el-submenu>
            <el-submenu index="6">
                <template slot="title">系统设置</template>
                <el-menu-item index="/ContractManagement">签约管理</el-menu-item>
                <el-menu-item index="/UserRights">用户权限管理</el-menu-item>
                <el-menu-item index="/SchoolFunction">学校功能设置</el-menu-item>
            </el-submenu>
            <el-submenu index="7">
                <template slot="title">用户指引</template>
                <el-menu-item index="/payment_item_list">选项1</el-menu-item>
                <el-menu-item index="/payment_item_list">选项2</el-menu-item>
            </el-submenu>
        </el-menu>
    </div>
</template>

<script>
    export default {
        name: "Aside",
        methods: {
            handleOpen(key, keyPath) {

            },
            handleClose(key, keyPath) {

            }
        }
    }
</script>

<style scoped>

</style>