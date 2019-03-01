<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">电子校园卡</a></div>
            <div class="fr batch-operation">
                <a href="#" class="operation-btn">批量操作</a>
                <ul class="operation-ul">
                    <li>
                        <el-button type="text" @click="BulkImport">批量导入</el-button>
                    </li>
                    <li>
                        <el-button type="text" @click="exportVisible = true">批量导出</el-button>
                    </li>
                    <li>
                        <el-button type="text" @click="delectVisible = true">批量删除</el-button>
                    </li>
                    <li>
                        <el-button type="text" @click="moveVisible = true">批量移动</el-button>
                    </li>
                    <!--<li>-->
                        <!--<el-button type="text" @click="welcomeVisible = true">迎新状态</el-button>-->
                    <!--</li>-->
                </ul>
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
                        <ul class="nav-tab-change2 fl" @click="tabswitch">
                            <li style="cursor: pointer;" v-for="(v,i) in tabTitle" :class="isShowTab==i?'active':''" @click="cardType(i)">{{v}}
                            </li>
                        </ul>
                        <div class="fl">
                            <router-link to="/cardindex">
                                <button class="btn-mo">+新增校园卡</button>
                            </router-link>
                        </div>
                    </div>

                </div>
            </div>
            <ul class="view-tree-box">
                <!--视图-->
                <li class="ym-view-box" :style="{display:isShowGraph?'block':'none'}">

                    <el-row :gutter="10">
                        <el-col :xs="24" :sm="24" :md="24" :lg="10" :xl="10">
                            <div class="card-box">
                                <div class="school-select-box">
                                    <el-select v-model="value1" filterable placeholder="请选择学院"
                                               @change="changeSchoolcourt">
                                        <el-option
                                                v-for="schoolCourt in schoolCourts"
                                                :key="schoolCourt.name"
                                                :label="schoolCourt.label"
                                                :value="schoolCourt.name">
                                        </el-option>
                                    </el-select>
                                </div>
                                <div class="school-xi-box">
                                    <el-select v-model="value2" filterable placeholder="请选择系"
                                               @change="changeSchooldepartment">
                                        <el-option
                                                v-for="schoolDepartment in schoolDepartments"
                                                :key="schoolDepartment.name"
                                                :label="schoolDepartment.label"
                                                :value="schoolDepartment.name">
                                        </el-option>
                                    </el-select>
                                </div>
                                <div class="school-xi-box">
                                    <el-select v-model="value3" filterable placeholder="请选择班级"
                                               @change="changeschoolClass">
                                        <el-option
                                                v-for="schoolClass in schoolClasses"
                                                :key="schoolClass.name"
                                                :label="schoolClass.label"
                                                :value="schoolClass.classid">
                                        </el-option>
                                    </el-select>
                                </div>
                                <div class="card-outtime clearfix">
                                    <div class="fl">即将过期卡片 : 0</div>
                                    <a href="#" class="fr details-table" @click="isShowGraph=false">详情</a>
                                </div>
                            </div>

                        </el-col>
                        <el-col :xs="24" :sm="24" :md="24" :lg="14" :xl="14">
                            <div class="card-box">
                                <div class="card-pad clearfix">
                                    <div class="left-card-num fl">
                                        <div>发卡数量</div>
                                        <div class="fa-card">{{ cardAccount | currency('',false) }}</div>
                                    </div>
                                    <div class="right-card-num fl">
                                        <el-progress :text-inside="true" :stroke-width="30"
                                                     :percentage=Number(parseRate.amountRate)></el-progress>
                                        <div class="card-num-totel clearfix">
                                            <div class="fl">
                                                已领卡数量 <span
                                                    class="has-card-num">{{ leaderCard | currency('',false) }}</span>
                                            </div>
                                            <div class="fr">
                                                未领卡数量 <span class="no-card-num">{{ unCard | currency('',false) }}</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </el-col>
                    </el-row>

                    <el-row :gutter="10">
                        <!--<el-col :xs="24" :sm="24" :md="24" :lg="10" :xl="10">
                            <div class="ym-body">
                                <div class="ym-body-title">使用热度</div>
                                <div id="circleChart" :style="{width: '100%', height: '420px'}"></div>
                                <div class="process-box-ab">
                                    <div class="ab-on1">
                                        <div class="cx-code">付款码</div>
                                        <div class="ab-on-pro"><el-progress :text-inside="true" :stroke-width="23" :percentage="50" color="#F8991C"></el-progress></div>
                                        <div>100人/50%</div>
                                    </div>
                                    <div class="ab-on1">
                                        <div class="cx-code">校园码码</div>
                                        <div class="ab-on-pro"><el-progress :text-inside="true" :stroke-width="23" :percentage="80"></el-progress></div>
                                        <div>160人/80%</div>
                                    </div>
                                </div>
                            </div>
                        </el-col>-->
                        <el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24">
                            <div class="ym-body">
                                <div class="ym-body-title">增长情况</div>
                                <ul class="date-tab clearfix">

                                    <li :class="{active:dateChoose}" @click="dateChooseType(3)">最近3天</li>
                                    <li :class="{active:!dateChoose}" @click="dateChooseType(7)">最近7天</li>
                                </ul>
                                <div id="barChart" :style="{width: '100%', height: '420px'}"></div>
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
                            <div class="tree_data">
                                <el-tree :data="dataArr" :props="defaultProps" @node-click="handleNodeClick"
                                         accordion node-key="id"
                                ></el-tree>
                            </div>

                        </div>
                        <div class="one-part-table">
                            <div class="one-row clearfix">
                                <div class="school-select-box1 fl">
                                    <el-select v-model="value1" filterable placeholder="请选择学院"
                                               @change="changeSchoolcourt" clearable>
                                        <el-option
                                                v-for="schoolCourt in schoolCourts"
                                                :key="schoolCourt.name"
                                                :label="schoolCourt.label"
                                                :value="schoolCourt.name">
                                        </el-option>
                                    </el-select>
                                </div>
                                <div class="school-xi-box1 fl">
                                    <el-select v-model="value2" filterable placeholder="请选择系"
                                               @change="changeSchooldepartment" clearable>
                                        <el-option
                                                v-for="schoolDepartment in schoolDepartments"
                                                :key="schoolDepartment.name"
                                                :label="schoolDepartment.label"
                                                :value="schoolDepartment.name">
                                        </el-option>
                                    </el-select>
                                </div>
                                <div class="school-xi-box1 fl">
                                    <el-select v-model="value3" filterable placeholder="请选择班级"
                                               @change="changeschoolClass" clearable>
                                        <el-option
                                                v-for="schoolClass in schoolClasses"
                                                :key="schoolClass.id"
                                                :label="schoolClass.label"
                                                :value="schoolClass.classid">
                                        </el-option>
                                    </el-select>
                                </div>
                                <!--<div class="fl class-person">-->
                                <!--共{{allPerson}}人-->
                                <!--</div>-->

                            </div>
                            <div class="one-row clearfix">
                                <div class="fl">
                                    <div class="fl search-input"><input type="text" class="se-input"
                                                                        placeholder="请输入需要查询的学工号或姓名" v-model="theinput">
                                    </div>
                                    <div class="fr search-button">
                                        <button class="btn-pro" @click="sureTheinput">搜索</button>
                                    </div>
                                </div>
                                <div class="fr search-one" style="margin-right: 0">
                                    <div class="fl search-word1">是否领卡</div>
                                    <div class="fl">
                                        <el-select v-model="value" placeholder="请选择"
                                                   @change="changecardchooseoptions" clearable>
                                            <el-option
                                                    v-for="item in cardchooseoptions"
                                                    :key="item.value"
                                                    :label="item.label"
                                                    :value="item.value">
                                            </el-option>
                                        </el-select>
                                    </div>
                                    <div class="fl">
                                        <!--<router-link to="/personneladd" class="add_user_on">新增用户</router-link>-->
                                        <div  @click="AddUserOn" class="add_user_on">新增用户</div>
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
                                        <el-table-column label="卡类型">
                                            <template slot-scope="scope">
                                                {{ scope.row.class_id==1 ? '学生卡':'老师卡' }}
                                            </template>
                                        </el-table-column>
                                        <el-table-column label="是否领卡">
                                            <template slot-scope="scope">
                                                {{ scope.row.card_state==1 ? '领卡':'未领卡' }}
                                            </template>
                                        </el-table-column>
                                        <el-table-column label="是否迎新">
                                            <template slot-scope="scope">
                                                {{ scope.row.welcome_flg==1 ? '不迎新':'迎新' }}
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
                            <div class="page-pagination">
                                <el-pagination
                                        @size-change="handleSizeChange"
                                        @current-change="handleCurrentChange"
                                        :current-page="configTable.index"
                                        :page-size="configTable.size"
                                        background
                                        layout="prev, pager, next"
                                        :total="configTable.total"
                                        v-show="pageinit">
                                </el-pagination>
                                <el-pagination
                                        @size-change="treeSizeChange"
                                        @current-change="treeCurrentChange"
                                        :current-page="configTabletree.index"
                                        :page-size="configTabletree.size"
                                        background
                                        layout="prev, pager, next"
                                        :total="configTabletree.total"
                                        v-show="pagetree">
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
                        :drag='true'
                        :action=uploadUrl()
                        multiple
                        :headers="uploadData"
                        accept="xlsx"
                        :before-upload="beforeAvatarUpload"
                        :on-success="handleAvatarSuccess"
                >
                    <i class="el-icon-upload"></i>
                    <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                </el-upload>

            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="dialogVisible = false" class="sure-btn">确 定</el-button>
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
        <!--批量移动-->
        <el-dialog title="批量移动" :visible.sync="moveVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-cascader
                        :change-on-select="true"
                        :options="moveData"
                        v-model="selectedOptions"
                        @change="handleChangemove"
                        :props="props">
                </el-cascader>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="moveVisible = false">取 消</el-button>
                <el-button type="primary" @click=Move class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <!--迎新状态-->
        <el-dialog title="迎新状态" :visible.sync="welcomeVisible" width="600px" top="25vh">
            <div class="layer-tip-word">选择迎新状态</div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="welcomeVisible = false">迎新</el-button>
                <el-button type="primary" @click="welcomeVisible = false" class="sure-btn">不迎新</el-button>
            </div>
        </el-dialog>
        <el-dialog title="新增部门信息" :visible.sync="addtreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-input v-model="addtreeinput" placeholder="请输入内容" @change="check"></el-input>
                <div class="err-plchod" style="font-size: 13px ;color:red;padding-top: 5px">{{errname}}</div>
                <div v-show="radioShow">
                    <template>
                        <el-radio v-model="isClass" label="0">学生班级</el-radio>
                        <el-radio v-model="isClass" label="1">老师部门</el-radio>
                    </template>
                </div>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="addtreeVisible = false">取 消</el-button>
                <el-button type="primary" @click="SureaddtreeVisible" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
        <el-dialog title="编辑部门信息" :visible.sync="edittreeVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-input v-model="edittreeinput.name" placeholder="请输入内容" @change="check"></el-input>
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
        name: "Campus_user",

        data() {
            return {

                uploadData: {
                    schoolcode: localStorage.schoolcode
                },


                schoolCourts: [],//院
                schoolDepartments: [],//系
                schoolClasses: [],//班


                dateChoose: true,
                stuArr: [],
                teachArr: [],
                otherArr:[],
                dateArr: [],
                dateArr1: [],
                cardchooseoptions: [{
                    value: '',
                    label: '全部'
                }, {
                    value: '1',
                    label: '已领卡'
                }, {
                    value: '0',
                    label: '未领卡'
                }],
                options1: [{
                    value: '选项1',
                    label: '学院'
                }, {
                    value: '选项2',
                    label: '系院'
                }, {
                    value: '选项3',
                    label: '班级'
                }, {
                    value: '选项4',
                    label: '设计01'
                }, {
                    value: '选项5',
                    label: '设计02'
                }],
                errname: "",//树形验证
                gainaddtree: "",//获取新增父级内容
                addtreeinput: "",//新增内容
                edittreeinput: "",//编辑内容
                deletetreeinput: "",//删除内容
                isClass: '0',//新增选项
                radioShow: false,
                value: '',
                value1: '',//院
                value2: '',//系
                value3: '',//班级
                level: '',
                allPerson: "",//全部人
                pageinit: true,
                pagetree: false,
                dataid: "",
                treeIdex: "",//fenye
                theinput: '',//输入学号或工号
                treeData: '',//点击获取树形图信息
                schoolcardtype: '',//卡类型
                moveDatas: [],//班级联动
                classValue: {},//移动班级id
                moveData: [],
                dialogVisible: false,
                exportVisible: false,
                delectVisible: false,
                moveVisible: false,
                welcomeVisible: false,
                addtreeVisible: false,
                edittreeVisible: false,
                deletetreeVisible: false,
                errorVisible: false,
                select_bg: require('@/assets/picture/images/select-bg.png'),
                select_checked: require('@/assets/picture/images/select-checked.png'),
                tableData: [{
                    student_id: '',
                    user_name: '',
                    card_state: '',
                    welcome_flg: '',
                    class_id: '',

                }],
                tabTitle: ['全部', '老师卡', '学生卡','其他卡'],
                isShowTab: 0,
                isShowGraph: true,
                dataArr: [],
                configTable: {
                    total: 0,
                    size: 10,
                    index: 1
                },
                configTabletree: {
                    total: 0,
                    size: 10,
                    index: 0
                },
                props: {
                    value: 'value',
                    children: 'children',
                    label: 'label',

                },
                defaultProps: {
                    children: 'children',
                    label: 'label'
                },
                selectedOptions: [],
                cardAccount: "",
                leaderCard: '',
                unCard: '',
                exportXlsx: [],
                multipleSelection: [],
                errtree: '',//如果获取树形失败
                errpersonneldata: '',//导入人员失败信息
                errpersonnelmsg: '',//导入人员失败提示
                errdata:[],
            };
        },
        mounted() {
            this.drawLine();
            /* this.drawCircle();*/
            this.getParams();
        },
        filters: {
            currency: currency,
            getDateType: getDateType
        },
        created() {
            this.cardData(0);
            this.dateArr1 = [getDateType(1, -3), getDateType(1, 0)];
            this.treeList();
            this.init()
            this.axios.get(`/api/SchoolUser/InduceSchoolUserInfo`, {
                params: {
                    school_Code: localStorage.schoolcode,
                }
            }).then(res => {
                this.exportXlsx = res.data.aaData
            })
            // this.allPeople()
            this.moveClass()

        },
        methods: {
            moveClass() {
                this.axios.get(`/api/SchoolDepartment/GetSchoolDepartmentCascader`, {
                    params: {
                        schoolcode: localStorage.schoolcode
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        this.moveDatas.push(res.data.data)
                        this.moveData = this.getTreeData(this.moveDatas);
                        console.log(res)
                    }
                })
            },

            getTreeData(data) {
                // 循环遍历json数据
                for (var i = 0; i < data.length; i++) {
                    if (data[i].children.length < 1) {
                        // children若为空数组，则将children设为undefined
                        data[i].children = undefined;
                    } else {
                        // children若不为空数组，则继续 递归调用 本方法
                        this.getTreeData(data[i].children);
                    }
                }
                return data;
            },

            // 选项框联动
            cardType(index) {
                console.log(index)
                this.cardData(index);
                if (index == 0) {
                    this.schoolcardtype = ""
                    this.init()
                    // this.allPeople()
                } else if (index == 1) {
                    this.schoolcardtype = 2
                    this.init()
                    // this.allPeople()
                } else if (index == 2) {
                    this.schoolcardtype = 1
                    this.init()
                    // this.allPeople()
                }
            },
            changeSchoolcourt() {
                this.value2 = ""
                this.value3 = ""
                this.schoolDepartments = []
                this.schoolCourts.map(item => {
                    if (item.name == this.value1) {
                        console.log(item.children)
                        this.schoolDepartments = item.children
                    }
                })
                if (this.schoolDepartments == "") {
                    this.value2 = ""
                    this.value3 = ""
                }
                console.log(this.schoolDepartments)

            },
            changeSchooldepartment() {
                this.value3 = ""
                this.schoolClasses = []
                this.schoolDepartments.map(item => {
                    if (item.name == this.value2) {
                        this.schoolClasses = item.children
                    }
                })
                if (this.schoolClasses == "") {
                    this.value3 = ""
                }
            },
            changeschoolClass() {
                if (this.value3) {
                    this.treeIdex = 0
                    this.axios.get(`/api/SchoolUser/GetSchoolUserInfo`, {
                        params: {
                            schoolcode: localStorage.schoolcode,
                            classid: this.value3,
                            level: 3,
                            iDisplayStart: this.treeIdex,
                            iDisplayLength: this.configTable.size,
                        }
                    }).then(res => {
                        console.log(res)
                        this.tableData = res.data.data
                        this.configTable.total = res.data.iTotalRecords
                    })
                }
            },
            changecardchooseoptions() {
                this.init()
            },
            sureTheinput() {
                this.init()
            },
            OpenaddTree() {
                if (this.errtree == 0 && this.treeData == "") {
                    console.log(123)
                } else if (this.errtree == 1 && this.treeData != "") {
                    console.log(456)
                }
                this.addtreeVisible = true
                this.gainaddtree = this.treeData
            },
            SureaddtreeVisible() {
                if (this.errtree == 1) {
                    console.log()
                }
                if (this.gainaddtree.treeLever == 3) {
                    this.$message.error('不允许创建');
                } else {
                    console.log(this.gainaddtree)
                    const addtree = {
                        "isClass": this.isClass,
                        "schoolcode": localStorage.schoolcode,
                        "treeLevel": this.gainaddtree.treeLever,
                        "p_id": this.gainaddtree.id,
                        "name": this.addtreeinput
                    }
                    console.log(addtree)
                    this.axios.post(`api/SchoolDepartment/AddDepartment`, addtree).then(res => {
                        console.log(res)
                        if (res.data.code == "000000") {
                            this.addtreeVisible = false
                            this.$message({
                                message: res.data.msg,
                                type: 'success'
                            });
                            this.treeList();
                        } else if (res.data.code == "111111") {
                            this.$message.error(res.data.msg);
                        }
                    })
                }
            },
            OpeneditTree() {
                this.edittreeVisible = true
                this.edittreeinput = this.treeData
            },
            SureedittreeVisible() {
                const edittree = {
                    "id": this.edittreeinput.id,
                    "schoolcode": localStorage.schoolcode,
                    "name": this.edittreeinput.name
                }
                this.axios.post(`api/SchoolDepartment/UpdateDepartment`, edittree).then(res => {
                    if (res.data.code == "000000") {
                        this.edittreeVisible = false
                        this.$message({
                            message: res.data.msg,
                            type: 'success'
                        });
                        this.treeList();
                    } else if (res.data.code == "111111") {
                        this.$message.error(res.data.msg);
                    }
                })
            },
            OpendeleteTree() {
                this.deletetreeVisible = true
                this.deletetreeinput = this.treeData
            },
            SuredeletetreeVisible() {
                const deletetree = {
                    "id": this.deletetreeinput.id,
                    "schoolcode": localStorage.schoolcode,
                    "name": this.deletetreeinput.name
                }
                this.axios.post(`api/SchoolDepartment/DeleteDepartment`, deletetree).then(res => {
                    console.log(res)
                    if (res.data.code == "000000") {
                        this.deletetreeVisible = false
                        this.$message({
                            message: res.data.msg,
                            type: 'success'
                        });
                        this.treeList();
                    } else if (res.data.code == "111111") {
                        this.$message.error(res.data.msg);
                    }
                })
            },
            // 分页
            handleSizeChange(val) {
                console.log(val)
                this.configTable.size = val
                this.init()
                console.log(`每页 ${val} 条`);
            },
            handleCurrentChange(val) {
                this.configTable.index = val
                this.init()
                console.log(`当前页: ${val}`);
            },
            treeSizeChange(val) {
                console.log(val)
                this.configTabletree.size = val
                this.GetTreeData()
                console.log(`每页 ${val} 条`);
            },
            treeCurrentChange(val) {
                this.configTabletree.index = val
                this.treeIdex = this.configTabletree.index - 1
                this.GetTreeData()
                console.log(`当前页: ${val}`);
            },
            init() {
                let TableIndex = this.configTable.index * 10 - 10
                this.axios.get(`/api/SchoolUser/FindSchoolUserInfo`, {
                    params: {
                        school_Code: localStorage.schoolcode,
                        iDisplayStart: TableIndex,
                        iDisplayLength: this.configTable.size,
                        userNameOrId: this.theinput,
                        branchId: this.value1,
                        departmentId: this.value2,
                        department_classId: this.value3,
                        card_state: this.value,
                        classId: this.schoolcardtype,
                    }
                }).then(res => {
                    this.tableData = res.data.aaData
                    this.configTable.total = res.data.iTotalRecords
                    this.allPerson = this.configTable.total
                    this.pageinit = true
                    this.pagetree = false
                    console.log(this.allPerson)
                    console.log(this.configTable.total)
                })
            },

            handleEdit(index, row) {
                console.log(row)
                this.$router.push({name: "PersonnelEdit", params: {id: row.user_id}})
            },
            handleDelete(index, row) {
                this.axios.post(`/api/SchoolUser/DeleteSchoolUserBy?user_id=` + row.user_id).then(res => {
                    if (res.data.code == "000000") {
                        this.tableData.splice(index, 1)
                        this.$message({
                            message: '删除成功',
                            type: 'success'
                        });
                        this.init()
                    }
                    console.log(res)
                })
            },

            check: function () {
                // if (this.addtreeinput != "") {
                //     this.errname = "";
                // } else {
                //     this.errname = "用户名不能为空";
                // }
            },
            uploadUrl() {
                var url = this.axios.defaults.baseURL + "api/Users/StudenttempletChangeJson";
                return url;
            },

            beforeAvatarUpload(file) {
                console.log(file)
                // console.log(file)
            },
            handleAvatarSuccess(res, file) {
                console.log(res)
                if (res.code == "000000") {
                    const Userdata = {
                        schoolcode: localStorage.schoolcode,
                        sign: "0",
                        data: res.studentJson
                    }
                    this.axios.post(`/api/Users/AddUserInfoAndDepartmentToV2`, Userdata).then(res => {
                        console.log(res)
                        if (res.code = "000000") {
                            this.$message({
                                message: res.data.msg,
                                type: 'success'
                            });
                        } else if (res.code = "111111") {
                            this.$message.error(res.data.msg);
                        }
                    })
                } else if (res.code == "111111" && res.msg == "身份证重复请核查") {
                    this.$message.error(res.msg);
                    this.errpersonneldata=res.data
                    this.errpersonnelmsg=res.msg
                    this.errorVisible=true
                    this.errpersonneldata.split(",").map(item=>{
                        this.errdata.push({id:item})
                    })
                    console.log(this.errdata)

                } else (
                    this.$message.error(res.data)
                )
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
                this.dialogVisible=false
                this.errorVisible=false
            },
            AddUserOn(){
                this.axios.get(`/api/SchoolUser/GetSchoolCardList`, {
                    params: {
                        School_ID: localStorage.schoolcode
                    }
                }).then(res => {
                    if (res.data.data==""){
                        this.$message.error('请先添加校园卡');
                    }else{
                        this.$router.push({name: "PersonnelAdd"})
                    }
                })
            },

            BulkImport(){
                this.axios.get(`/api/SchoolUser/GetSchoolCardList`, {
                    params: {
                        School_ID: localStorage.schoolcode
                    }
                }).then(res => {
                    if (res.data.data==""){
                        this.$message.error('请先添加校园卡');
                    }else{
                        this.dialogVisible = true
                    }
                })
            },

            Export() {
                require.ensure([], () => {
                    const {export_json_to_excel} = require('../../../assets/js/Export2Excel');
                    const tHeader = ['学工号', '姓名', '身份证号', '卡类型', '部门信息', '有效期', '领卡状态', '是否迎新'];
                    // 上面设置Excel的表格第一行的标题
                    const filterVal = ['student_id', 'user_name', 'passport', 'class_id', 'department', 'create_time', 'card_state', 'welcome_flgName'];
                    // 上面的index、phone_Num、school_Name是tableData里对象的属性
                    const list = this.exportXlsx;  //把data里的tableData存到list
                    const data = this.formatJson(filterVal, list);
                    export_json_to_excel(tHeader, data, '导出信息');
                })
                this.exportVisible = false
            },
            formatJson(filterVal, jsonData) {
                return jsonData.map(v => filterVal.map(j => v[j]))
            },

            handleSelectionChange(val) {
                this.multipleSelection = val;
                console.log(this.multipleSelection)
            },
            //批量删除
            Delete() {
                let deleteuserid = []
                let deletecard = []
                let deletebiz = []
                let deleteali = []
                this.multipleSelection.map(item => {
                    deleteuserid.push(item.user_id)
                    deletecard.push(item.card_add_id)
                    deletebiz.push(item.biz_card_no)
                    deleteali.push(item.ali_user_id)
                })
                const deleteCar = {
                    card_add_id: deletecard.join(","),
                    biz_card_no: deletebiz.join(","),
                    ali_user_id: deleteali.join(","),
                }
                this.axios.post(`/api/SchoolCodr/deleteSchoolCodr`, deleteCar, {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }).then(res => {
                    if (res.data.code == "000000") {
                        const deletedata = {
                            user_id: deleteuserid.join(","),
                            schoolcode: localStorage.schoolcode
                        }
                        this.axios.post(`/api/Users/DeleteSchoolUser`, deletedata).then(res => {
                            console.log(res)
                            if (res.data.code == "000000") {
                                this.delectVisible = false
                            }
                        })
                    }
                })

            },
            //批量移动
            Move() {
                let deleteuserid = []
                this.multipleSelection.map(item => {
                    deleteuserid.push(item.user_id)
                })
                const usermoveData = {
                    schoolcode: localStorage.schoolcode,
                    depamentid: this.classValue,
                    user_id: deleteuserid.join(",")
                }
                this.axios.post(`/api/Users/UpdateSchoolUserToDepartment`, usermoveData).then(res => {
                    console.log(res)
                    if (res.data.code == "000000") {
                        this.$message({
                            message: '恭喜你，移动成功',
                            type: 'success',
                        });
                        this.moveVisible = false
                        this.init()
                    }
                })
            },


            getParams() {
                // 取到路由带过来的参数
                const linkR = this.$route.query.linkR;
                // 将数据放在当前组件的数据内

                if (linkR) {
                    this.isShowGraph = false;
                }
            },
            dateChooseType(type) {
                var self = this;
                if (type == 3) {
                    self.dateChoose = true;
                    self.drawLine(getDateType(1, -3), getDateType(1, 0));
                } else if (type == 7) {
                    self.dateChoose = false;
                    self.drawLine(getDateType(1, -7), getDateType(1, 0));
                }

            },
            //柱形图
            drawLine(stime, etime) {
                let self = this;
                // 基于准备好的dom，初始化echarts实例
                let barChart = echarts.init(document.getElementById('barChart'));

                // 绘制图表
                if (!stime) {
                    stime = self.dateArr1[0]
                }
                if (!etime) {
                    etime = self.dateArr1[1]
                }

                self.axios.get('api/SchoolCodr/SchoolCardGrowth', {
                    params: {
                        school_id: localStorage.schoolcode,
                        stime: stime,
                        etime: etime,
                    }
                })
                    .then(function (response) {
                        if (response.data.code == '000000') {
                            let res = response.data;
                            self.dateArr = res.rqs.reverse();
                            self.teachArr = res.teacherlist.reverse();
                            self.stuArr = res.stulist.reverse();
                            self.otherArr = res.qilist.reverse();
                            // 基于准备好的dom，初始化echarts实例
                            barChart.setOption({
                                tooltip: {
                                    trigger: 'axis'
                                },
                                legend: {
                                    data: ['学生卡', '教师卡','其他卡'],
                                    formatter: function (name) {
                                        console.log(name)
                                        return name;
                                    },
                                    orient: 'vertical',
                                    align: 'right',
                                    itemWidth: 20,
                                    itemHeight: 20,
                                    right: 0,
                                    top: 'middle',
                                    textStyle: {
                                        // 图例的公用文本样式。
                                        fontWight: 'bold',
                                        fontSize: 20,
                                        color: '#707070'
                                    },
                                    symbolKeepAspect: true,
                                    itemGap: 20,
                                    // padding: [
                                    //     20,  // 上
                                    //     20, // 右
                                    //     20,  // 下
                                    //     20, // 左
                                    // ]

                                },

                                toolbox: {
                                    show: true,
                                    top: '0',
                                    right: '120',
                                    bottom: 'auto',
                                    left: 'auto',
                                    feature: {
                                        dataView: {show: true, readOnly: false},
                                        magicType: {show: true, type: ['line', 'bar']},
                                        restore: {show: true},
                                        saveAsImage: {show: true}
                                    }
                                },
                                calculable: true,
                                xAxis: [
                                    {
                                        type: 'category',
                                        //    boundaryGap : false,
                                        data: self.dateArr,
                                        splitLine: {
                                            show: true,
                                            lineStyle: {
                                                color: ['#ccc'],
                                                width: 1,
                                                type: 'dashed'
                                            }

                                        }

                                    }
                                ],
                                yAxis: [
                                    {
                                        type: 'value',
                                        splitLine: {
                                            show: true,
                                            lineStyle: {
                                                color: ['#ccc'],
                                                width: 1,
                                                type: 'dashed'
                                            }

                                        }
                                    }
                                ],
                                grid: {
                                    //show:true,
                                    //   backgroundColor: '#f9f9f9',
                                    borderWidth: 2,
                                    borderColor: '#ccc',
                                    top: '80',
                                    right: '120',
                                    bottom: '40',
                                    left: '30'

                                },
                                series: [
                                    {
                                        itemStyle: {
                                            normal: {
                                                color: ['#2387fb'],
                                                barBorderRadius: [8, 8, 0, 0]
                                            }
                                        },
                                        name: '教师卡',
                                        type: 'bar',
                                        data: self.stuArr,
                                        markPoint: {
                                            data: [
                                                {type: 'max', name: '最大值'},
                                                {type: 'min', name: '最小值'}
                                            ]
                                        },
                                        markLine: {
                                            data: [
                                                {type: 'average', name: '平均值'}
                                            ]
                                        },
                                        barWidth: 20
                                    },
                                    {
                                        itemStyle: {
                                            normal: {
                                                color: ['#ffb758'],
                                                barBorderRadius: [8, 8, 0, 0]
                                            }
                                        },
                                        name: '学生卡',
                                        type: 'bar',
                                        data: self.teachArr,
                                        /*markPoint : {
                                            data : [
                                                {name : '年最高', value : 182.2, xAxis: 7, yAxis: 183},
                                                {name : '年最低', value : 2.3, xAxis: 11, yAxis: 3}
                                            ]
                                        },*/
                                        markLine: {
                                            data: [
                                                {type: 'average', name: '平均值'}
                                            ]
                                        },
                                        barWidth: 20
                                    },
                                    {
                                        itemStyle: {
                                            normal: {
                                                color: ['#FF4848'],
                                                barBorderRadius: [8, 8, 0, 0]
                                            }
                                        },
                                        name: '其他卡',
                                        type: 'bar',
                                        data: self.otherArr,
                                        /*markPoint : {
                                            data : [
                                                {name : '年最高', value : 182.2, xAxis: 7, yAxis: 183},
                                                {name : '年最低', value : 2.3, xAxis: 11, yAxis: 3}
                                            ]
                                        },*/
                                        markLine: {
                                            data: [
                                                {type: 'average', name: '平均值'}
                                            ]
                                        },
                                        barWidth: 30
                                    },
                                ],
                                show: true,

                            });
                        } else {
                            self.$message({
                                showClose: true,
                                message: '获取数据失败',
                                type: 'warning'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });


            },
            //饼状图
            drawCircle() {
                let circleChart = echarts.init(document.getElementById('circleChart'));
                circleChart.setOption({
                    title: {
                        // text: '某站点用户访问来源',
                        //subtext: '纯属虚构',
                        x: '0',
                        textStyle: {
                            // 图例的公用文本样式。
                            // fontWight:'bold',
                            fontSize: 14,
                            color: '#707070'
                        },
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        bottom: 10,
                        left: 'center',
                        itemWidth: 20,
                        itemHeight: 20,
                        data: ['直接访问', '邮件营销'],
                        formatter: function (name) {
                            console.log(name)
                            if (name == '直接访问') {
                                return name;
                            } else {
                                return name;
                            }
                        },
                        itemGap: 40,
                    },
                    color: ['#ffb758', '#2387fb'],
                    series: [
                        {
                            name: '访问来源',
                            type: 'pie',
                            radius: '70%',
                            center: ['50%', '40%'],
                            // radius: ['50%', '70%'],  // 设置环形饼状图， 第一个百分数设置内圈大小，第二个百分数设置外圈大小
                            // center: ['50%', '50%'],  // 设置饼状图位置，第一个百分数调水平位置，第二个百分数调垂直位置

                            data: [
                                {value: 310, name: '直接访问'},
                                {value: 1135, name: '邮件营销'},

                            ],
                            itemStyle: {
                                emphasis: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                }
                            },
                            label: {
                                normal: {
                                    show: true,
                                    position: 'inside',
                                    formatter: '{d}%',//模板变量有 {a}、{b}、{c}、{d}，分别表示系列名，数据名，数据值，百分比。{d}数据会根据value值计算百分比

                                    textStyle: {
                                        align: 'center',
                                        baseline: 'middle',
                                        fontFamily: '微软雅黑',
                                        fontSize: 15,
                                        fontWeight: 'bolder'
                                    }
                                },
                            },
                        }
                    ]
                })

            },
            //学生 老师 全部 tab切换
            tabswitch() {
                if (!event) return;
                let target = event.target;

                if (target.nodeName.toLowerCase() !== 'li') {
                    return;
                }
                let len = target.parentNode.children;
                for (let i = 0; i < len.length; i++) {
                    len[i].index = i;
                    len[i].removeAttribute('class');
                }
                target.setAttribute('class', 'active');
                this.isShowTab = target.index;

            },
            //树状图
            handleNodeClick(data) {
                console.log(data);
                if (data.treeLever == 1) {
                    this.value1 = data.label
                    this.value2 = ""
                    this.value3 = ""
                }
                if (data.treeLever == 2 && this.value1) {
                    this.value2 = data.label
                    this.value3 = ""
                }
                if (data.treeLever == 3 && this.value2) {
                    this.value3 = data.label
                }
                if (data.treeLever == 2) {
                    this.radioShow = true
                } else {
                    this.radioShow = false
                }
                this.level = data.treeLever
                if (data.treeLever == 3) {
                    this.dataid = data.classid
                    this.treeIdex = 0
                    this.GetTreeData()
                } else {
                    this.dataid = data.id
                    this.treeIdex = 0
                    this.GetTreeData()
                }
                this.treeData = data
            },
            GetTreeData() {
                this.axios.get(`/api/SchoolUser/GetSchoolUserInfo`, {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        classid: this.dataid,
                        level: this.level,
                        iDisplayStart: this.treeIdex,
                        iDisplayLength: this.configTabletree.size,
                    }
                }).then(res => {
                    console.log(res)
                    this.tableData = res.data.data
                    this.configTabletree.total = res.data.iTotalRecords
                    this.allPerson = res.data.iTotalRecords
                    this.pageinit = false
                    this.pagetree = true
                })
            },
            //班级级联
            handleChangemove(value) {
                console.log(value);

                value.map(item => {
                    this.classValue = item
                })

            },
            // 点击下载
            download() {
                let self = this;
                self.downloadLoading = true;
                require.ensure([], () => {
                    const {export_json_to_excel} = require('../../../assets/js/Export2Excel');
                    const tHeader = ['卡类型', '学工号', '姓名', '身份证号', '部门信息', '有效期', '是否多身份'];
                    const filterVal = ['class_id', 'student_id', 'user_name', 'pass_port', 'depart_ment', 'validity_time', 'state'];
                    const list = [{
                        class_id: "'学生",
                        student_id: "'43092319920509002",
                        username: "任馨传",
                        pass_port: "430923199205090021",
                        depart_ment: "经济管理学院/无分院/财务管理系/154财务管理四班",
                        validity_time: "2020/7/1",
                        state: "0"
                    }];  //表格数据，iview中表单数据也是这种格式！
                    const data = self.formatJson(filterVal, list);
                    export_json_to_excel(tHeader, data, '导入模板') //列表excel  这个是导出表单的名称
                    self.downloadLoading = false
                });
            },
            cardData(type) {
                let self = this;
                self.axios.get('api/SchoolCodr/SchoolCardCount', {
                    params: {
                        school_id: localStorage.schoolcode,
                        type: type
                    }
                })
                    .then(function (response) {
                        if (response.data.code == '000000') {
                            let res = response.data;
                            self.cardAccount = res.card_count;
                            self.leaderCard = res.registered_count;
                            self.unCard = res.unregistered_count;
                        } else {
                            self.$message({
                                showClose: true,
                                message: '获取数据失败',
                                type: 'warning'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            treeList() {
                let self = this;
                self.axios.get('api/SchoolDepartment/GetSchoolDepartmentTree', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        console.log(response)
                        var arr = [];
                        if (res.code == '000000') {
                            arr.push(res.data);
                            self.dataArr = arr;
                            self.defaultProps = res.defaultProps;
                            self.schoolCourts = res.data.children
                            self.errtree = 0
                            console.log(self.schoolCourts)
                        } else {
                            console.log(无数据)
                            self.errtree = 1
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },


        },
        computed: {
            parseRate: function () {
                let self = this, leaderCard = 0, cardAccount = 1, amountRate = 0;
                if (self.leaderCard != 0) {
                    leaderCard = self.leaderCard;
                }
                if (self.cardAccount != 0) {
                    cardAccount = self.cardAccount;
                }
                //领卡比例
                amountRate = (leaderCard * 100 / cardAccount).toFixed(2);

                return {amountRate: amountRate}

            },

        },

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