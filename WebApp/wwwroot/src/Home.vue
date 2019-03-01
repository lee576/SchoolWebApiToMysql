<template>
    <div id="app">
        <el-container>
            <el-aside width="250px" >
                <Aside></Aside>
            </el-aside>
            <el-container>
                <el-header  style="height: 45px ; background-color:#4c526b;">
                    <AdminHeader></AdminHeader>
                </el-header>
                <el-main style="padding: 0;">
                    <router-view/>
                </el-main>
            </el-container>
        </el-container>
    </div>
</template>

<script>
    import Aside from './components/Aside.vue'
    import AdminHeader from './components/Header.vue'

    export default {
        components: {
            AdminHeader,
            Aside,
        },
        data() {
            return {
                is_Login: true,
            }
        },
        created() {
        },
        methods: {}
    }
</script>
<style lang="scss">

    html, body {
        height: 100%;
        margin: 0;
    }

    #app {
        height: 100%;
    }

    .el-container {
        height: 100%;
    }

    .el-header, .el-footer {
        background-color: #B3C0D1;
        color: #333;
        text-align: center;
        line-height: 30px;
        display: flex;
        justify-content: flex-end;
    }

    .el-aside {
        background-color: #3f4356;
        color: #333;
        text-align: center;
        line-height: 120px;
    }

    .el-submenu__title {
        border-bottom: 1px solid rgba(255,255,255,0.1);
        text-align: left;
        padding-left: 40px!important;
        background-color: #3f4356 !important;
    }

    .header {
        display: flex;
    }

    .el-menu {
        width: 100%;

    }

    .el-main {
        background-color: #E9EEF3;
        color: #333;
    }

    body > .el-container {
        margin-bottom: 40px;
    }

    .el-container:nth-child(5) .el-aside,
    .el-container:nth-child(6) .el-aside {
        line-height: 260px;
    }

    .el-container:nth-child(7) .el-aside {
        line-height: 320px;
    }

    .title {
        background-color: #4c526b;
        height: 106px;
        text-align: center;
    }

    .title img {
        width: 25px;
        height: 25px;
    }

    .title span {
        color: #fff;
        font-size: 20px;
        line-height: 25px;
        margin-left: 5px;
    }

    .adm {
        color: #2387fb;
        font-size: 15px;
        font-weight: bold;
        margin:0 8px;
        position:relative;
        top: -5px;
    }

    .el-menu-item-group {
        background-color: #464d70;
    }

    .admin_component>span{
        margin:8px 5px;
        display: inline-block;
    }
    .admin{
        height: 30px;
        padding: 0 12px;
        border-left: 1px solid rgba(255,255,255,1);
    }
    .admin>span{
        display: inline-block;
    }
    /*侧边栏定制样式*/
    .el-menu-item{
        border-bottom: 1px solid rgba(255,255,255,0.1)
    }
    .el-menu-item:hover{
        background: none!important;
    }
    .el-menu-item:focus{
        background: none!important;

    }
    .el-submenu.is-active.is-opened .el-submenu__title{
        color:rgb(35, 135, 251)!important;
    }
    .el-submenu.is-active .el-submenu__title{
        border-bottom-color:rgba(255,255,255,0.1)!important;
    }
    .el-menu-item{border-left:5px solid rgb(70, 77, 112);}
    .el-menu-item.is-active{border-left:5px solid #409EFF;}
    .admin_component{margin-top: 8px}
    .el-submenu .el-menu-item{text-align: left;}
    .el-submenu__title{height:45px!important;line-height: 45px!important;}
    .el-submenu .el-menu-item{height:45px!important;line-height: 45px!important;}
    .el-submenu.is-opened>.el-submenu__title{color:#2387fb !important}
    .el-submenu.is-opened i{color:#2387fb !important}
    .el-button--primary span{color:#fff!important;}


</style>
