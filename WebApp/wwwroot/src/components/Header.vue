<template>
    <div class="admin_component clearfix">
        <div class="fl help-img"><img src="../assets/picture/images/help.png"></div>
        <div class="fl tip-right"><img src="@/assets/picture/images/tip-tell.png"></div>
        <div class="fl line-img"></div>
        <div class="fl">
            <el-dropdown trigger="click">
                <span class="el-dropdown-link clearfix">
                    <div class="fl user-img"><img src="../assets/picture/images/user-blue.png"></div>
                    <div class="fl user-name">admin</div>
                </span>
                <el-dropdown-menu slot="dropdown">
                    <el-dropdown-item><span class="width-bd" @click="esc">退出</span></el-dropdown-item>
                </el-dropdown-menu>
            </el-dropdown>
        </div>

    </div>
</template>

<script>
    export default {
        name: "Header",
        data() {
            return {

            }
        },
        methods: {
            esc(){
                this.$router.push({path: '/'});
            }
        }
    }
</script>

<style scoped>

</style>