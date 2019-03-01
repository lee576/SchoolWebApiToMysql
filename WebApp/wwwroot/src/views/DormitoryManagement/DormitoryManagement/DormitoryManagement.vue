<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">宿舍管理</a></div>
            <div class="fr batch-operation">
                <button class="operation-btn">上传宿舍</button>
            </div>
        </div>
        <div class="page-content">
            <div class="two-part-box">
                <div class="one-part-tree">
                    <div class="clearfix">
                        <div class="fl"><button class="btn">公共设施</button></div>
                        <div class="ztree-icon fr" style="margin-top: 5px;margin-bottom: 8px">
                            <span class="icon iconfont el-icon-tianjia"></span>
                            <span class="icon iconfont el-icon-icon-edit"></span>
                            <span class="icon iconfont el-icon-shanchu"></span>
                        </div>
                    </div>
                </div>
                <div class="one-part-table">
                    <div class="clearfix">
                        <div class="fl on-part-search1">
                            <div class="fl form-timerange">
                                <el-select placeholder="请选择楼号" style="width:200px" v-model="flloorStr">
                                    <el-option label="001栋" value="001"></el-option>
                                </el-select>
                            </div>
                        </div>
                        <div class="fl on-part-search1">
                            <div class="fl form-timerange">
                                <el-select placeholder="请选择房间号" style="width:200px" v-model="flloorStr">
                                    <el-option label="103" value="103"></el-option>
                                </el-select>
                            </div>
                        </div>
                        <div class="fr">
                            <div class="fl on-part-search1">
                                <div class="fl left-word-de">分配状态</div>
                                <div class="fl form-timerange">
                                    <el-select placeholder="请选择分配状态" style="width:200px" v-model="status">
                                        <el-option label="已分配" value="01"></el-option>
                                    </el-select>
                                </div>
                            </div>
                            <div class="fl on-part-search2">
                                <div class="fl left-word-de">学工号或姓名</div>
                                <div class="fl search-input">
                                    <input  type="text" placeholder="请输入学工号或姓名" class="se-input" v-model="orderNo" >
                                </div>
                                <div class="fl search-button"><button class="btn-pro">搜索</button></div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix">
                        <div class="fr right-btn-ta">
                            <button class="btn" style="margin-right: 10px"  @click="dialogVisible = true">批量分配导入</button>
                            <button class="btn-pro">批量取消分配</button>
                        </div>
                    </div>
                    <div class="table-box" style="margin-top: 15px">
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
                                        label="姓名"
                                >
                                </el-table-column>
                                <el-table-column
                                        prop="stalls"
                                        label="学工号"
                                >
                                </el-table-column>
                                <el-table-column
                                        prop="address"
                                        label="楼栋">
                                </el-table-column>
                                <el-table-column
                                        prop="address"
                                        label="宿舍号">
                                </el-table-column>
                                <el-table-column
                                        prop="address"
                                        label="门禁编码">
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
        </div>


        <!--批量导入-->
        <el-dialog title="批量导入" :visible.sync="dialogVisible" width="600px" top="20vh">
            <div class="gray-import">
                <div class="improt-word1">注意事项：</div>
                <ul class="improt-word2">
                    <li>1、必须按照指定的模板格式才能导入成功。</li>
                    <li>2、如果Excel中数据已存在，则会覆盖原有信息。</li>
                    <li>3、请按照对应的卡片导入数据。</li>
                    <li>4、请上传Excel后缀为.xls文件。</li>
                </ul>
                <div class="improt-word3">下载模板：<span >校园学生卡数据导入示例表</span></div>
            </div>
            <div class="gray-import" style="margin-top:10px;">
                <el-upload
                        class="upload-demo"
                        drag
                        action="https://jsonplaceholder.typicode.com/posts/"
                        multiple>
                    <i class="el-icon-upload"></i>
                    <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                    <div class="el-upload__tip" slot="tip">只能上传jpg/png文件，且不超过500kb</div>
                </el-upload>

            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="dialogVisible = false" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    export default {
        name: "DormitoryManagement",
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
                flloorStr:'',
                orderNo:'',
                status:'',
                dialogVisible:false,
                currentPage1:1,
                tableData: [{
                    order:'赵子靖',
                    stalls: '201809250178',
                    address:"12302"
                }, {
                    order:'赵子靖',
                    stalls: '201809250178',
                    address:"12302"
                }, {
                    order:'赵子靖',
                    stalls: '201809250178',
                    address:"12302"
                }, {
                    order:'赵子靖',
                    stalls: '201809250178',
                    address:"12302"
                }],
            }
        }
    }
</script>

<style scoped>

</style>