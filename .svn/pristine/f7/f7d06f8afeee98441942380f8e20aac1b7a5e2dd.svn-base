<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#" class="active-gray">门禁管理></a><a href="#">分析数据信息</a></div>
            <div class="fl">
                <router-link to="/AccessControl" class="cx-back">返回</router-link>
            </div>
        </div>
        <div class="page-content">
            <div class="nav-tab clearfix">
                <div class="fl">
                    <div class="fl left-word-on1">门禁时间</div>
                    <div class="fl form-timerange">
                        <el-date-picker
                                v-model="dateArr"
                                value-format="yyyy-MM-dd"
                                type="daterange"
                                range-separator="至"
                                start-placeholder="开始日期"
                                end-placeholder="结束日期" style="width:250px">
                        </el-date-picker>
                    </div>
                    <div class="fl on-part-search">
                        <div class="fl search-button"><button class="btn-pro">搜索</button></div>
                    </div>
                </div>
            </div>
            <el-row :gutter="10">
                <el-col :xs="24" :sm="24" :md="24" :lg="16" :xl="16">
                   <div class="ant-card-men">
                       <div class="echart-men-tip">门禁卡使用时间</div>
                   </div>
                </el-col>
                <el-col :xs="24" :sm="24" :md="24" :lg="8" :xl="8">
                    <div class="ant-card-men">
                        <div class="echart-men-tip">门禁卡使用对比</div>
                    </div>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                    <div class="ant-card-men">
                        <div class="rank-num">目前场景总人数：300</div>
                        <div class="rank-word">排行榜</div>
                        <ul class="rank-list clearfix">
                            <li>1　沈腾</li>
                            <li>2　马冬梅</li>
                            <li>3　沈时</li>
                            <li>4　沈费</li>
                            <li>5　沈钱</li>
                            <li>6　沈范</li>
                        </ul>
                    </div>
                </el-col>
            </el-row>
        </div>
    </div>
</template>

<script>
    export default {
        name: "AccessControlAnalysis",
        data() {
            return {
                dateArr: [],
            }
        }
    }
</script>

<style scoped>

</style>