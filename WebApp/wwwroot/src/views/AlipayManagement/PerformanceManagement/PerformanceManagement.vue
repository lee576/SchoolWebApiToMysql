<template>
    <div class="home" v-loading="loading">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">成绩管理</a></div>
            <div class="fr batch-operation">
                <a href="#" class="operation-btn">导出成绩单</a>
            </div>
            <div class="fr batch-operation" style="margin-right: 13px">
                <a href="#" class="operation-btn">导入成绩单</a>
            </div>
        </div>
        <div class="page-content">
            <div class="row">
                <div class="col-xs-12 col-sm-12">
                    <div class="nav-tab clearfix">
                        <ul class="nav-tab-change1 fl">
                            <li :class="{active:isShowGraph}" @click="isShowGraph=true">
                                <label class="fl view-img1"></label>
                                <span class="fl">视图</span>
                            </li>
                            <li :class="{active:!isShowGraph}" @click="isShowGraph=false">
                                <label class="fl view-img2"></label>
                                <span class="fl">树状图</span>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <ul class="view-tree-box">
                <!--视图-->
                <li class="ym-view-box" :style="{display:isShowGraph?'block':'none'}">
                    <el-row :gutter="10">
                        <el-col :xs="24" :sm="24" :md="24" :lg="10" :xl="10">
                            <div class="card-box">
                                <div style="padding: 25px 35px">
                                    <span style="margin-right: 20px">考试学期</span>
                                    <span style="position: relative;top: 5px;margin-right: 20px">
                                        <el-date-picker
                                                v-model="datatime"
                                                type="daterange"
                                                range-separator="至"
                                                start-placeholder="开始日期"
                                                end-placeholder="结束日期"
                                        >
                                        </el-date-picker>
                                    </span>
                                    <span> <button class="btn-pro" @click="">搜索</button></span>
                                </div>
                                <div style="padding: 25px 35px">
                                    <span style="margin-right: 20px">考试科目</span>
                                    <span>
                                       <el-select v-model="required" filterable
                                                  @change="ChangeRequired" style="width: 120px;margin-right: 20px">
                                        <el-option
                                                v-for="required in requiredsdata"
                                                :key="required.id"
                                                :label="required.label"
                                                :value="required.id">
                                        </el-option>
                                    </el-select>
                                </span>
                                    <span>
                                    <el-select v-model="subject" filterable placeholder="请选择科目"
                                               @change="ChangeSubject">
                                        <el-options
                                                v-for="subject in subjectsdata"
                                                :key="subject.id"
                                                :label="subject.label"
                                                :value="subject.id">
                                        </el-options>
                                    </el-select>
                                </span>
                                    <!--<div class="card-outtime clearfix">-->
                                    <!--<a href="#" class="fr details-table" @click="isShowGraph=false">详情</a>-->
                                    <!--</div>-->
                                </div>
                            </div>
                        </el-col>
                        <el-col :xs="24" :sm="24" :md="24" :lg="14" :xl="14">
                            <div class="card-box">
                                <div class="card-pad clearfix">
                                    <div class="left-card-num fl">
                                        <div>考试人数</div>
                                        <div class="fa-card">123</div>
                                    </div>
                                    <div class="right-card-num fl">
                                        <el-progress :text-inside="true" :stroke-width="30"
                                        ></el-progress>
                                        <div class="card-num-totel clearfix">
                                            <div class="fl">
                                                及格人数 <span
                                                    class="has-card-num">123</span>
                                            </div>
                                            <div class="fr">
                                                不及格人数 <span class="no-card-num">123</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </el-col>
                    </el-row>

                    <el-row :gutter="10">
                        <el-col :xs="14" :sm="14" :md="14" :lg="14" :xl="14">
                            <div class="ym-body">
                                <div class="ym-body-title">成绩分布</div>
                                <div id="barGraph" style=" height:420px;"></div>
                            </div>
                        </el-col>
                        <el-col :xs="10" :sm="10" :md="10" :lg="10" :xl="10">
                            <div class="ym-body" style="position: relative">
                                <div class="ym-body-title">人员分布</div>
                                <div id="barCharts" style="height:420px;"></div>
                                <div style="position: absolute;top: 20px;right: 20px">
                                    <el-select v-model="value" placeholder="请选择">
                                        <el-option
                                                v-for="item in options"
                                                :key="item.value"
                                                :label="item.label"
                                                :value="item.value">
                                        </el-option>
                                    </el-select>
                                </div>
                            </div>
                        </el-col>
                    </el-row>
                </li>
                <!--树状图-->
                <li class="ym-tree-box" :style="{display:isShowGraph?'none':'block'}">
                    <div class="two-part-box">
                        <div class="one-part-tree">
                            <div class="ztree-icon" style="margin-top: 0;margin-bottom: 8px">
                                <span class="icon iconfont el-icon-tianjia" @click="OpenaddTree"></span>
                                <span class="icon iconfont el-icon-icon-edit" @click="OpeneditTree"></span>
                                <span class="icon iconfont el-icon-shanchu" @click="OpendeleteTree"></span>
                            </div>
                            <div class="tree_data" style="border: 1px solid #ddd;">
                                <el-tree :data="dataArr" :props="defaultProps" @node-click="handleNodeClick"
                                         accordion node-key="id" :default-expanded-keys="opentreearr" ref="treeX"
                                         style="border: none"
                                ></el-tree>
                            </div>

                        </div>
                        <div class="one-part-table">
                            <div class="one-row clearfix">
                                <div class="school-select-box1 fl">
                                    <el-select v-model="required" filterable
                                               @change="ChangeRequired">
                                        <el-option
                                                v-for="required in requiredsdata"
                                                :key="required.id"
                                                :label="required.label"
                                                :value="required.id">
                                        </el-option>
                                    </el-select>
                                </div>
                                <div class="school-xi-box1 fl">
                                    <el-select v-model="subject" filterable placeholder="请选择科目"
                                               @change="ChangeSubject">
                                        <el-options
                                                v-for="subject in subjectsdata"
                                                :key="subject.id"
                                                :label="subject.label"
                                                :value="subject.id">
                                        </el-options>
                                    </el-select>
                                </div>
                                <div class="school-xi-box1 fl">
                                    <span style="margin-right: 10px">学院班级</span>
                                    <el-select v-model="classvalue" filterable placeholder="请选择班级"
                                               @change="ChangeClass">
                                        <el-option
                                                v-for="classe in classesdata"
                                                :key="classe.id"
                                                :label="classe.label"
                                                :value="classe.classid">
                                        </el-option>
                                    </el-select>
                                </div>
                            </div>
                            <div class="one-row clearfix">
                                <div class="fl">
                                    <div class="fl search-word1">是否合格</div>
                                    <div class="fl">
                                        <el-select v-model="qualified" placeholder="请选择"
                                                   @change="ChangeQualified">
                                            <el-option
                                                    v-for="item in qualifieddata"
                                                    :key="item.value"
                                                    :label="item.label"
                                                    :value="item.value">
                                            </el-option>
                                        </el-select>
                                    </div>
                                    <div class="fl search-input" style="margin-left: 20px">
                                        <input type="text"
                                               class="se-input"
                                               placeholder="请输入需要查询的学号或姓名"
                                               v-model="theinput">
                                    </div>
                                    <div class="fr search-button">
                                        <button class="btn-pro" @click="sureTheinput" style="margin-right: 10px">搜索
                                        </button>
                                        <button class="btn-pro" @click="returnTheinput">重置</button>
                                    </div>
                                </div>
                            </div>
                            <div class="table-box" style="border: none;">
                                <template>
                                    <el-table :data="tableData" stripe style="width: 100%;border: 1px solid #ddd;"
                                              @selection-change="handleSelectionChange">
                                        <el-table-column
                                                type="selection"
                                                width="55">
                                        </el-table-column>
                                        <el-table-column prop="user_name" label="姓名">
                                        </el-table-column>
                                        <el-table-column prop="student_id" label="学号">
                                        </el-table-column>
                                        <el-table-column label="学科">
                                            <template slot-scope="scope">
                                                {{ scope.row.class_id==0 ? '必修':'选修' }}
                                            </template>
                                        </el-table-column>
                                        <el-table-column prop="student_id" label="考试时间">
                                        </el-table-column>
                                        <el-table-column prop="student_id" label="学期">
                                        </el-table-column>
                                        <el-table-column prop="student_id" label="分数">
                                        </el-table-column>
                                        <el-table-column label="是否合格">
                                            <template slot-scope="scope">
                                                {{ scope.row.welcome_flg==0 ? '合格':'不合格' }}
                                            </template>
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
                                </template>
                            </div>
                            <div class="page-pagination" style="padding: 0;">
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
                        </div>
                    </div>
                </li>
            </ul>
        </div>

        <!--批量导入-->
        <el-dialog title="批量导入" :visible.sync="dialogVisible" width="600px" top="20vh">
            <div class="gray-import">
                <div class="improt-word1">注意事项：</div>
                <ul class="improt-word2">
                    <li>1、必须按照指定的模板格式才能导入成功。</li>
                    <li>2、如果Excel中数据已存在，则会覆盖原有信息。</li>
                    <li>3、请按照对应的卡片导入数据。</li>
                    <li>4、请上传Excel后缀为.xlsx文件。</li>
                </ul>
                <div class="improt-word3">下载模板：<span @click="download">校园学生卡数据导入示例表</span></div>
            </div>
            <div class="gray-import" style="margin-top:10px;">
                <el-upload
                        class="upload-demo"
                        ref="upload"
                        drag
                        :action=uploadUrl()
                        multiple
                        :headers="uploadData"
                        method="post"
                        :on-success="handleAvatarSuccess"
                        :auto-upload="false"
                >
                    <i class="el-icon-upload"></i>
                    <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                </el-upload>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="submitUpload" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <!--批量导出-->
        <el-dialog title="批量导出" :visible.sync="exportVisible" width="600px" top="25vh">
            <div class="layer-tip-word">确定导出以下数据？</div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="exportVisible = false">取 消</el-button>
                <el-button type="primary" @click="Export" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <!--批量删除-->
        <el-dialog title="批量删除" :visible.sync="delectVisible" width="600px" top="25vh">
            <div class="layer-tip-word">删除会同时完成解绑，是否继续？( 操作不可逆 )？</div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="delectVisible = false">取 消</el-button>
                <el-button type="primary" @click=Delete class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>

        <el-dialog title="新增部门信息" :visible.sync="addtreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-input v-model="addtreeinput" placeholder="请输入内容" @change="check"></el-input>
                <div class="err-plchod" style="font-size: 13px ;color:red;padding-top: 5px">{{errname}}</div>

            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="addtreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureaddtreeVisible" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog title="编辑部门信息" :visible.sync="edittreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-input v-model="edittreeinput.name" placeholder="请输入内容"></el-input>
                <div class="err-plchod" style="font-size: 13px ;color:red;padding-top: 5px">{{errname}}</div>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="edittreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureedittreeVisible" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog title="删除部门信息" :visible.sync="deletetreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <span>确定是否删除</span>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="deletetreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SuredeletetreeVisible" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <!--导入人员错误信息-->
        <el-dialog title="错误人员信息" :visible.sync="errorVisible" width="600px" top="25vh">
            <div class="layer-tip-word">{{errpersonnelmsg}}</div>
            <div class="layer-tip-word" @click="ExportErrData" style="cursor: pointer;color:#1D8FE1">点击下载错误人员信息</div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="errorVisible = false">取 消</el-button>
                <el-button type="primary" @click="errorVisible = false" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>


    </div>
</template>

<script>
    import {currency} from './../../../util/currency';
    import {getDateType} from './../../../util/getDate';
    // 引入基本模板
    let echarts = require('echarts/lib/echarts');
    // 引入柱状图组件
    require('echarts/lib/chart/bar');
    require("echarts/lib/chart/pie");
    // 引入提示框和title组件
    require('echarts/lib/component/tooltip');
    require('echarts/lib/component/title');
    export default {
        data() {
            return {
                datatime: '',//选择日期

                uploadData: {
                    schoolcode: localStorage.schoolcode
                },
                requiredsdata: [{
                    value: '0',
                    label: '必修'
                }, {
                    value: '1',
                    label: '选修'
                }],//必修选修
                required: '',//必修选修  0.0

                subjectsdata: [],//选择科目
                classesdata: [],//班


                dateChoose: true,
                qualifieddata: [{
                    value: '',
                    label: '全部'
                }, {
                    value: '0',
                    label: '合格'
                }, {
                    value: '1',
                    label: '不合格'
                }],
                opentreearr: [""],
                errname: "",//树形验证
                addtreeinput: "",//新增内容
                edittreeinput: "",//编辑内容
                deletetreeinput: "",//删除内容
                qualified: '',//是否合格   0.0

                subject: '',//选择科目  0.0
                classvalue: '',//班级  0.0

                theinput: '',//输入学号或工号
                loading: false,
                dialogVisible: false,
                exportVisible: false,
                delectVisible: false,
                addtreeVisible: false,
                edittreeVisible: false,
                deletetreeVisible: false,
                errorVisible: false,
                tableData: [{
                    student_id: '',
                    user_name: '',
                    card_state: '',
                    welcome_flg: '',
                    class_id: '',

                }],
                isShowGraph: true,
                dataArr: [],
                configTable: {
                    total: 0,
                    size: 10,
                    index: 1
                },
                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
                errpersonneldata: '',//导入人员失败信息
                errpersonnelmsg: '',//导入人员失败提示
                errdata: [],
            };
        },
        mounted() {
            this.circle()
            this.histogram()
        },

        created() {

        },
        methods: {

            //必修选修
            ChangeRequired() {
            },
            //选择科目
            ChangeSubject() {
            },
            ChangeClass() {

            },
            //是否合格
            ChangeQualified() {
            },
            //查询按钮
            sureTheinput() {
            },
            //重置按钮
            returnTheinput() {
            },
            OpenaddTree() {

                this.addtreeVisible = true

            },
            SureaddtreeVisible() {


            },
            OpeneditTree() {
                this.edittreeVisible = true
            },
            SureedittreeVisible() {

            },
            OpendeleteTree() {
                this.deletetreeVisible = true
            },
            SuredeletetreeVisible() {


            },
            // 分页
            handleSizeChange(val) {
                console.log(val)
                this.configTable.size = val
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                this.configTable.index = val
                console.log(`当前页: ${val}`);
            },
            check: function () {
                // if (this.addtreeinput != "") {
                //     this.errname = "";
                // } else {
                //     this.errname = "用户名不能为空";
                // }
            },

            init() {
            },

            handleEdit(index, row) {
                console.log(row)
            },
            handleDelete(index, row) {
            },


            uploadUrl() {
                var url = this.axios.defaults.baseURL + "/api/Users/StudenttempletChangeJson";
                return url;
            },
            //点击上传
            submitUpload(res, file) {
                this.loading = true
                this.dialogVisible = false;
                this.$refs.upload.submit();
            },
            //上传成功
            handleAvatarSuccess(res, file) {

            },
            ExportErrData() {
                require.ensure([], () => {
                    const {export_json_to_excel} = require('../../../assets/js/Export2Excel');
                    const tHeader = ['身份证号'];
                    // 上面设置Excel的表格第一行的标题
                    const filterVal = ['id'];
                    // 上面的index、phone_Num、school_Name是tableData里对象的属性
                    const list = this.errdata;  //把data里的tableData存到list
                    const data = this.formatJson(filterVal, list);
                    export_json_to_excel(tHeader, data, '错误人员信息');
                })
                this.dialogVisible = false
                this.errorVisible = false
            },


            Export() {
                // let theinput = "", isCord = "", cardtype = "", classid = "", level = ""
                //
                // if (this.theinput) {
                //     theinput = this.theinput
                // }
                // if (this.valuedata) {
                //     classid = this.valuedata
                // } else {
                //     classid = this.initial
                // }
                // if (this.leveldata) {
                //     level = this.leveldata
                // } else {
                //     level = 0
                // }
                // if (this.value) {
                //     isCord = this.value
                // }
                // if (this.schoolcardtype) {
                //     cardtype = this.schoolcardtype
                // }

                window.open(this.axios.defaults.baseURL + '/api/SchoolUser/InduceSchoolUserInfo?schoolcode=' + localStorage.schoolcode + '' +
                    '&nameORstudentid=' + theinput + '&isCord=' + isCord + '&cardtype=' + cardtype + '&classid=' + classid + '&level=' + level);
                this.exportVisible = false
            },
            //点击表格头
            handleSelectionChange(val) {
                console.log(this.val)
            },
            //批量删除
            Delete() {
            },
            //树状图
            handleNodeClick(data) {
                console.log(data);
            },

            // 点击下载
            download() {
                window.open(this.platUrl + 'Template/StudentsTemplate.xlsx');
            },
            treeList() {
            },

            //圆环图
            circle() {
                var myChart = echarts.init(document.getElementById('barCharts'));
                let option = {
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b}: {c} ({d}%)"
                    },
                    legend: {
                        orient: 'vertical',
                        x: '2%',
                        y: '30%',
                        itemWidth: 50,
                        itemHeight: 20,
                        itemGap: 20,
                        data: ['美术', '音乐', '数学', '语文', '英语'],
                    },
                    series: [
                        {
                            name: '访问来源',
                            type: 'pie',
                            radius: ['50%', '70%'],
                            avoidLabelOverlap: false,
                            label: {
                                emphasis: {
                                    show: true,
                                    textStyle: {
                                        fontSize: '30',
                                        fontWeight: 'bold'
                                    }
                                }
                            },
                            labelLine: {
                                normal: {
                                    show: false
                                }
                            },
                            data: [
                                {
                                    value: 335, name: '美术', itemStyle: {
                                        color: 'rgba(255,216,88,1)'
                                    }
                                },
                                {
                                    value: 310, name: '音乐', itemStyle: {
                                        color: 'rgba(35,135,251,1)'
                                    }
                                },
                                {
                                    value: 234, name: '数学', itemStyle: {
                                        color: 'rgba(252,59,85,1)'
                                    }
                                },
                                {
                                    value: 135, name: '语文', itemStyle: {
                                        color: 'rgba(255,148,41,1)'
                                    }
                                },
                                {
                                    value: 1548, name: '英语', itemStyle: {
                                        color: 'rgba(24,164,14,1)'
                                    }
                                }
                            ]
                        }
                    ]
                };
                myChart.setOption(option);
            },
            //柱状图
            histogram() {
                var myChart = echarts.init(document.getElementById('barGraph'));
                let option = {
                    tooltip: {
                        trigger: 'axis'
                    },
                    legend: {
                        show:true,
                        data:['邮件营销','联盟广告','视频广告','直接访问','搜索引擎']
                    },
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true
                    },
                    toolbox: {
                        feature: {
                            saveAsImage: {}
                        }
                    },
                    xAxis: {
                        type: 'category',
                        boundaryGap: false,
                        data: ['周一','周二','周三','周四','周五','周六','周日']
                    },
                    yAxis: {
                        type: 'value'
                    },
                    series: [
                        {
                            name:'邮件营销',
                            type:'line',
                            stack: '总量',
                            data:[120, 132, 101, 134, 90, 230, 210]
                        },
                        {
                            name:'联盟广告',
                            type:'line',
                            stack: '总量',
                            data:[220, 182, 191, 234, 290, 330, 310]
                        },
                        {
                            name:'视频广告',
                            type:'line',
                            stack: '总量',
                            data:[150, 232, 201, 154, 190, 330, 410]
                        },
                        {
                            name:'直接访问',
                            type:'line',
                            stack: '总量',
                            data:[320, 332, 301, 334, 390, 330, 320]
                        },
                        {
                            name:'搜索引擎',
                            type:'line',
                            stack: '总量',
                            data:[820, 932, 901, 934, 1290, 1330, 1320]
                        }
                    ]
                };
                myChart.setOption(option);

            },
        }

    }
</script>

<style scoped>
    .tree_data {
        overflow: scroll
    }

    .tree_data::-webkit-scrollbar {
        display: none
    }

    .table-box {
        overflow: scroll
    }

    .table-box::-webkit-scrollbar {
        display: none
    }
</style>