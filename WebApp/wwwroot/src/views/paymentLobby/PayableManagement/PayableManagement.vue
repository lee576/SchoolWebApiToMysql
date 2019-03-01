<template>
    <div class="home">
        <div class="nav-lead clearfix">
            <div class="fl nav-lead-word"><a href="#">应缴款项</a></div>
            <div class="fr payable-btn">
                <button class="operation-btn" @click="exportExcell">导出EXCEL</button>
                <button class="operation-btn" @click="dialogVisible = true">批量导入</button>
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
                <ul class="nav-tab-change2 fl">
                    <li  :class="{active:isActive}" @click="isActive = true;">缴费项信息</li>
                    <li :class="{active:!isActive}" @click="isActive = false;">学员信息</li>
                </ul>
                 <div class="fl form-timerange">
                     <el-select :style="{display:isActive?'block':'none'}" style="margin-right: 15px" v-model="stopStr" @change="stopChoose">
                         <el-option v-for="item in stopArr" :label="item.label" :value="item.value" :key="item.value"></el-option>
                     </el-select>
                 </div>
                <div class="fl">
                    <!--请输入缴费批号、应缴项目名称-->
                    <div class="fl search-input"><input type="text" class="se-input" :placeholder="isActive?'请输入缴费批号、应缴项目名称':'请输入学号、姓名或身份证号'" v-model="inputStr" @keyup.enter="search"></div>
                    <div class="fl search-button"><button class="btn-pro" @click="search">搜索</button></div>
                    <div class="fl search-button"><button class="btn-pro" @click="reset">重置</button></div>
                </div>

            </div>
            <ul class="view-tree-box">
                <!--视图-->
                <li class="ym-view-box" :style="{display:isShowGraph?'block':'none'}">
                    <div :style="{display:isActive?'block':'none'}">
                        <div class="no-message-tip" :style="{display:isShow?'block':'none'}">
                            <img src="../../../assets/picture/images/no-mess.png" height="100" width="100"/>
                            <div class="message-tip-word">暂无内容</div>
                        </div>
                        <el-row :gutter="10">
                            <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12"  v-for="(item, index) in configTable.tableArr" :key="item.id">
                                <div class="pay-card" :class="{grayCard:item.status=='1'}">
                                    <div class="pay-man-box1 clearfix">
                                        <div class="fl pay-man1">缴费批号：{{item.ARID}}</div>
                                        <div class="fr">
                                            <span class="pay-wo1">缴费人数</span>
                                            <span class="pay-wo2">{{ item.fact_count | currency('',false) }}</span>
                                            <span class="pay-wo3">/{{ item.arcount | currency('',false) }}</span>
                                        </div>
                                    </div>
                                    <div class="pay-man-box1 clearfix">
                                        <div class="fl pay-man2">
                                            <div class="pay-wo4">{{item.name}}</div>
                                            <div class="pay-wo5"><span class="pay-man1">应缴金额: </span><span class="pay-wo2">{{ item.amount | currency('￥') }}</span></div>
                                        </div>
                                        <div class="fr pay-man3">
                                            实收:{{ item.fact_amount | currency('￥') }}
                                        </div>
                                    </div>
                                    <div class="clearfix">
                                        <div class="fl pay-man4">{{item.star_date}}</div>
                                        <div class="fr two-row-on3">
                                            <el-button type="text" size="small" class="view-tr-con" @click="check(item)">查看</el-button>
                                            <el-button type="text" size="small" class="export-tr-con" @click="exportExcell(item)">导出</el-button>
                                            <el-button type="text" size="small" :style="{display:item.status==1?'none':''}" class="edit-tr-con" @click="stop(item)">终止</el-button>
                                            <el-button type="text" size="small" :style="{display:item.status==1?'none':''}" class="del-tr-con" @click="deleteData(item.ARID)">删除</el-button>
                                        </div>
                                    </div>
                                </div>
                            </el-col>
                            <!--  <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                                  <div class="pay-card">
                                      <div class="pay-man-box1 clearfix">
                                          <div class="fl pay-man1">201901098987</div>
                                          <div class="fr">
                                              <span class="pay-wo1">缴费人数</span>
                                              <span class="pay-wo2">369</span>
                                              <span class="pay-wo3">/500</span>
                                          </div>
                                      </div>
                                      <div class="pay-man-box1 clearfix">
                                          <div class="fl pay-man2">
                                              <div class="pay-wo4">医保缴费</div>
                                              <div class="pay-wo5"><span class="pay-man1">收费金额: </span><span class="pay-wo2">￥100.00</span></div>
                                          </div>
                                          <div class="fr pay-man3">
                                              总计:￥368,900.00
                                          </div>
                                      </div>
                                      <div class="clearfix">
                                          <div class="fl pay-man4">
                                              2019-01-04-21-35-59
                                          </div>
                                          <div class="fr two-row-on3">
                                              <a href="javascript:void(0);">
                                                  <router-link to="/ManagementDetail" class="row-color1">查看</router-link>
                                              </a>
                                              <a href="#" class="row-color4">导出</a>
                                              <a href="#" class="row-color2">终止</a>
                                              <a href="#" class="row-color3">删除</a>
                                          </div>
                                      </div>
                                  </div>
                              </el-col>
                              <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                                  <div class="pay-card gray-card">
                                      <div class="pay-man-box1 clearfix">
                                          <div class="fl pay-man1">201901098987</div>
                                          <div class="fr">
                                              <span class="pay-wo1">缴费人数</span>
                                              <span class="pay-wo2">369</span>
                                              <span class="pay-wo3">/500</span>
                                          </div>
                                      </div>
                                      <div class="pay-man-box1 clearfix">
                                          <div class="fl pay-man2">
                                              <div class="pay-wo4">医保缴费</div>
                                              <div class="pay-wo5"><span class="pay-man1">收费金额: </span><span class="pay-wo2">￥100.00</span></div>
                                          </div>
                                          <div class="fr pay-man3">
                                              总计:￥368,900.00
                                          </div>
                                      </div>
                                      <div class="clearfix">
                                          <div class="fl pay-man4">
                                              2019-01-04-21-35-59
                                          </div>
                                          <div class="fr two-row-on3">
                                              <a href="javascript:void(0);">
                                                  <router-link to="/ManagementDetail" class="row-color1">查看</router-link>
                                              </a>
                                              <a href="#" class="row-color4">导出</a>
                                              <a href="#" class="row-color2">终止</a>
                                              <a href="#" class="row-color3">删除</a>
                                          </div>
                                      </div>
                                  </div>
                              </el-col>-->
                        </el-row>
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
                    <div :style="{display:isActive?'none':'block'}">
                        <div class="no-message-tip" :style="{display:isShowStu?'block':'none'}">
                            <img src="../../../assets/picture/images/no-mess.png" height="100" width="100"/>
                            <div class="message-tip-word">暂无内容</div>
                        </div>
                        <el-row :gutter="10">
                            <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12"  v-for="(item1, index) in studentConfigTable.tableArr" :key="item1.id">
                                <div class="pay-card">
                                    <div class="pay-man-box1 clearfix">
                                        <div class="fl pay-man1">缴费批号 : {{item1.ARID}}</div>
                                        <div class="fr">
                                            <span class="pay-wo1">缴费名称 : {{item1.name}}</span>
                                        </div>
                                    </div>
                                    <div class="pay-man-box1 clearfix">
                                        <div class="fl pay-wo4" style="margin-top: 10px;">姓名 : {{item1.username}}</div>
                                        <div class="fr" style="margin-top: 5px;">
                                            <span class="pay-wo1">应缴金额 : {{ item1.amount | currency('￥') }}</span>
                                        </div>
                                    </div>
                                    <div class="pay-man-box1 clearfix" style="margin-bottom: 0;">
                                        <div class="fl pay-man2">
                                            <div class="pay-wo5" style="opacity: 0.8;padding-bottom: 5px;">学工号 : {{item1.studentid}}</div>
                                            <div class="pay-wo5" style="opacity: 0.8;">身份证号 : <span class="pay-wo2">{{item1.passport}}</span></div>
                                        </div>
                                        <div class="fr pay-man3">
                                            实收:{{ item1.fact_amount | currency('￥') }}
                                        </div>
                                    </div>
                                    <!-- <div class="clearfix">
                                         <div class="fl pay-man4">
                                             2019-01-04-21-35-59
                                         </div>

                                     </div>-->
                                </div>
                            </el-col>
                            <!--   <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                                   <div class="pay-card">
                                       <div class="pay-man-box1 clearfix">
                                           <div class="fl pay-man1">201901098987</div>
                                           <div class="fr">
                                               <span class="pay-wo1">缴费人数</span>
                                               <span class="pay-wo2">369</span>
                                               <span class="pay-wo3">/500</span>
                                           </div>
                                       </div>
                                       <div class="pay-man-box1 clearfix">
                                           <div class="fl pay-man2">
                                               <div class="pay-wo4">医保缴费</div>
                                               <div class="pay-wo5"><span class="pay-man1">收费金额: </span><span class="pay-wo2">￥100.00</span></div>
                                           </div>
                                           <div class="fr pay-man3">
                                               总计:￥368,900.00
                                           </div>
                                       </div>
                                       <div class="clearfix">
                                           <div class="fl pay-man4">
                                               2019-01-04-21-35-59
                                           </div>
                                           <div class="fr two-row-on3">
                                               <a href="javascript:void(0);">
                                                   <router-link to="/ManagementDetail" class="row-color1">查看</router-link>
                                               </a>
                                               <a href="#" class="row-color4">导出</a>
                                               <a href="#" class="row-color2">终止</a>
                                               <a href="#" class="row-color3">删除</a>
                                           </div>
                                       </div>
                                   </div>
                               </el-col>
                               <el-col :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
                                   <div class="pay-card gray-card">
                                       <div class="pay-man-box1 clearfix">
                                           <div class="fl pay-man1">201901098987</div>
                                           <div class="fr">
                                               <span class="pay-wo1">缴费人数</span>
                                               <span class="pay-wo2">369</span>
                                               <span class="pay-wo3">/500</span>
                                           </div>
                                       </div>
                                       <div class="pay-man-box1 clearfix">
                                           <div class="fl pay-man2">
                                               <div class="pay-wo4">医保缴费</div>
                                               <div class="pay-wo5"><span class="pay-man1">收费金额: </span><span class="pay-wo2">￥100.00</span></div>
                                           </div>
                                           <div class="fr pay-man3">
                                               总计:￥368,900.00
                                           </div>
                                       </div>
                                       <div class="clearfix">
                                           <div class="fl pay-man4">
                                               2019-01-04-21-35-59
                                           </div>
                                           <div class="fr two-row-on3">
                                               <a href="javascript:void(0);">
                                                   <router-link to="/ManagementDetail" class="row-color1">查看</router-link>
                                               </a>
                                               <a href="#" class="row-color4">导出</a>
                                               <a href="#" class="row-color2">终止</a>
                                               <a href="#" class="row-color3">删除</a>
                                           </div>
                                       </div>
                                   </div>
                               </el-col>-->
                        </el-row>
                        <div class="block" style="text-align: right;">
                            <el-pagination
                                    @size-change="handleSizeChange"
                                    @current-change="handleCurrentChange"
                                    :current-page.sync="studentConfigTable.currentPage"
                                    :page-size="studentConfigTable.iDisplayLength"
                                    layout="total, prev, pager, next"
                                    :total="studentConfigTable.total">
                            </el-pagination>
                        </div>
                    </div>
                </li>
                <!--树状图-->
                <li class="ym-tree-box" :style="{display:isShowGraph?'none':'block'}">
                    <div :style="{display:isActive?'block':'none'}">
                        <div class="payment-item-table">
                            <div class="table-box">
                                <template>
                                    <el-table :data="configTable.tableArr"  stripe style="width: 100%" id="table-content" >
                                        <el-table-column prop="ARID" label="缴费批号" >
                                        </el-table-column>
                                        <el-table-column prop="name" label="应缴项目">
                                        </el-table-column>
                                        <el-table-column prop="arcount" label="应缴人数">
                                            <template scope="scope">
                                                {{ scope.row.arcount | currency('',false) }}
                                            </template>
                                        </el-table-column>
                                        <el-table-column prop="fact_count" label="实缴人数">
                                            <template scope="scope">
                                                {{ scope.row.fact_count | currency('',false) }}
                                            </template>
                                        </el-table-column>
                                        <el-table-column prop="amount" :formatter="moneyTableProj" label="应缴金额">
                                        </el-table-column>
                                        <el-table-column prop="fact_amount" :formatter="moneyTotalTableProj" label="合计实收">
                                        </el-table-column>
                                        <el-table-column prop="status" :formatter="feeStateTable" label="缴费状态">
                                        </el-table-column>
                                        <el-table-column prop="star_date"  label="创建时间">
                                        </el-table-column>
                                        <el-table-column  label="操作" width="190">
                                            <template slot-scope="scope">
                                                <el-button type="text" size="small" class="view-tr-con" @click="check(scope.row)">查看</el-button>
                                                <el-button type="text" size="small" class="export-tr-con" @click="exportExcell(scope.row)">导出</el-button>
                                                <el-button type="text" size="small" :style="{display:scope.row.status==1?'none':''}" class="edit-tr-con" @click="stop(scope.row)">终止</el-button>
                                                <el-button type="text" size="small" :style="{display:scope.row.status==1?'none':''}" class="del-tr-con" @click="deleteData(scope.row.ARID)">删除</el-button>
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
                    </div>
                    <div :style="{display:isActive?'none':'block'}">
                        <div class="payment-item-table">
                            <div class="table-box">
                                <template>
                                    <el-table :data="studentConfigTable.tableArr" stripe style="width: 100%">
                                        <el-table-column prop="username" label="姓名" >
                                        </el-table-column>
                                        <el-table-column prop="studentid" label="学工号">
                                        </el-table-column>
                                        <el-table-column prop="passport" label="身份证号">
                                        </el-table-column>
                                        <el-table-column prop="ARID" label="缴费批号">
                                        </el-table-column>
                                        <el-table-column prop="name" label="缴费名称">
                                        </el-table-column>
                                        <el-table-column prop="amount"  :formatter="moneyTable" label="应缴金额">
                                        </el-table-column>
                                        <el-table-column prop="fact_amount"  :formatter="moneyTotalTable" label="合计实收">
                                        </el-table-column>
                                        <el-table-column prop="pay_time" label="支付时间">
                                            <template scope="scope">
                                                {{ scope.row.pay_time?scope.row.pay_time:'未支付'}}
                                            </template>
                                        </el-table-column>

                                    </el-table>
                                </template>
                            </div>
                            <div class="block" style="text-align: right;">
                                <el-pagination
                                        @size-change="handleSizeChangeStu"
                                        @current-change="handleCurrentChangeStu"
                                        :current-page.sync="studentConfigTable.currentPage"
                                        :page-size="studentConfigTable.iDisplayLength"
                                        layout="total, prev, pager, next"
                                        :total="studentConfigTable.total">
                                </el-pagination>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
        <!--导出-->
        <el-dialog title="导出数据" :visible.sync="exportVisible" width="600px" top="25vh">
            <div class="tk-gray">
                <el-date-picker
                        v-model="dateArr"
                        value-format="yyyy-MM-dd"
                        type="daterange"
                        range-separator="至"
                        start-placeholder="开始日期"
                        end-placeholder="结束日期" style="width:100%">
                </el-date-picker>
            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="exportVisible = false">取 消</el-button>
                <el-button type="primary" @click="download" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
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
                <div class="improt-word3">下载模板：<span @click="download2" class="tb-edit">应缴费数据导入示例表</span></div>
            </div>
            <div class="gray-import" style="margin-top:10px;">
                <el-upload
                        class="upload-demo"
                        ref="upload"
                        drag
                        :action="uploadUrl()"
                        :headers="{schoolcode: schoolcode}"
                        method="post"
                        :file-list="fileList"
                        :before-upload="handlePreview"
                        :auto-upload="false">
                    <i class="el-icon-upload"></i>
                    <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
                </el-upload>

            </div>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button  type="primary"  @click="submitUpload" class="sure-btn">确 定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
    import { currency } from './../../../util/currency'
    import { export_json_to_excel } from './../../../assets/js/Export2Excel'
    export default {

        name: "PayableManagement",
        data() {
            return {
                // downloadUrl: require('@/assets/应缴款项'),
                isShow:false,
                isShowStu:false,
                schoolcode:'',
                exportListData:[],
                fileList:[],
                id:'',
                dialogVisible:false,
                downloadLoading:false,
                isActive:true,
                exportVisible:false,
                isShowGraph:true,
                inputStr:'',
                btnArr:['缴费项信息','学员信息'],
                stopStr:null,
                dateArr:[],
                stopArr:[{value:null , label: '全部'},{value: '0', label: '开启'},{value: '1', label: '终止'}],
                configTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },
                studentConfigTable:{
                    tableArr:[],
                    total:0,
                    currentPage: 1,
                    iDisplayStart:0,
                    iDisplayLength:10,
                },
            }
        },
        filters:{
            currency: currency,
            export_json_to_excel:export_json_to_excel
        },
        created() {
            let self=this;
            //项目列表
            self.tableList();
            //学员列表
            self.studentTableList();
            self.schoolcode = localStorage.schoolcode;

        },
        methods: {
            handleSizeChange(val) {
                console.log(`每页 ${val} 条`);
                let self = this;
                self.configTable.iDisplayLength = val;
                self.tableList();
            },
            handleCurrentChange(val) {
                console.log(`当前页: ${val}`);
                let self = this;
                self.configTable.iDisplayStart = (val - 1)*self.configTable.iDisplayLength;
                self.tableList();
            },
            handleSizeChangeStu(val) {
                console.log(`每页 ${val} 条`);
                let self = this;
                self.studentConfigTable.iDisplayLength = val;
                self.tableList();
            },
            handleCurrentChangeStu(val) {
                console.log(`当前页: ${val}`);
                let self = this;
                self.studentConfigTable.iDisplayStart = (val - 1)*self.studentConfigTable.iDisplayLength;
                self.tableList();
            },
            formatJson(filterVal, jsonData) {
                return jsonData.map(v => filterVal.map(j => v[j]))
            },
            tableList(){
                let self = this;

                self.axios.get('api/PaymentAR/GetpaymentARList', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.configTable.currentPage,
                        iDisplayStart:self.configTable.iDisplayStart,
                        iDisplayLength:self.configTable.iDisplayLength,
                        status:self.stopStr,
                        selectinfo:self.inputStr
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){
                            if(res.data.length == 0){
                                self.isShow = true;
                            }else {
                                self.configTable.tableArr = res.data;
                                self.configTable.total = res.iTotalRecords;
                            }
                        }else {
                            self.isShow = false;
                            self.$message({
                                showClose: true,
                                message: res.msg,
                                type: 'warning'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });

            },
            studentTableList(){
                let self = this;
                self.axios.get('api/PaymentAR/GetpaymentARListToUser', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        sEcho:self.studentConfigTable.currentPage,
                        iDisplayStart:self.studentConfigTable.iDisplayStart,
                        iDisplayLength:self.studentConfigTable.iDisplayLength,
                        selectinfo:self.inputStr
                    }
                })
                    .then(function (response) {
                        let res = response.data;
                        if(res.code == '000000'){

                            if(res.data.length == 0){
                                self.isShowStu = true;
                            }else {
                                self.studentConfigTable.tableArr = res.data;
                                self.studentConfigTable.total = res.iTotalRecords;
                            }
                        }else {
                            self.isShowStu = false;
                            self.$message({
                                showClose: true,
                                message: res.msg,
                                type: 'warning'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            stopChoose(val){
                this.stopStr = val;
            },
            check(item){
                //查看
                this.$router.push({name: 'ManagementDetail', params: {list:item}});
            },
            exportExcell(item){
                this.exportVisible = true;
                if(item){
                    this.id = item.ARID;
                }
            },
            stop(item){
                //终止
                let self = this;
                self.$confirm('此操作将永久终止该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    self.axios.post('api/PaymentAR/UpdatePayMentARToARID', {
                        schoolcode: localStorage.schoolcode,
                        ARID: item.ARID
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.tableList();
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'success'
                                });

                            }else {
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'warning'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });

                }).catch(() => {
                    self.$message({
                        type: 'info',
                        message: '已取消'
                    });
                });



            },
            deleteData(id){
                let self = this;

                self.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    //删除
                    self.axios.post('api/PaymentAR/delPayMentARToARID', {
                        schoolcode: localStorage.schoolcode,
                        ARID: id
                    })
                        .then(function (response) {
                            if(response.data.code == '000000'){
                                self.tableList();
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'success'
                                });

                            }else {
                                self.$message({
                                    showClose: true,
                                    message: response.data.msg,
                                    type: 'warning'
                                });
                            }
                        })
                        .catch(function (error) {
                            console.log(error);
                        });
                }).catch(() => {
                    self.$message({
                        type: 'info',
                        message: '已取消'
                    });
                });


            },
            search(){
                if(this.isActive){
                    this.configTable.iDisplayStart = 0;
                    this.configTable.currentPage = 1;
                    this.tableList();
                }else {
                    this.studentConfigTable.iDisplayStart = 0;
                    this.studentConfigTable.currentPage = 1;
                    this.studentTableList();
                }
            },
            //点击下载
            download(){
                let self = this,stime = '',etime = '';
                if(self.dateArr){
                    stime = self.dateArr[0];
                    etime = self.dateArr[1];
                }
                self.axios.get('api/PaymentAR/GetPanymentInfoEXCELToVue', {
                    params: {
                        schoolcode: localStorage.schoolcode,
                        stime:stime,
                        etime:etime,
                        id:self.id

                    }
                })
                    .then(function (response) {
                        console.log(response)
                        let res = response.data;
                        if(res.code == '000000'){
                            self.exportListData = res.data;
                            self.downloadLoading = true;
                            require.ensure([], () => {
                                const tHeader = ["学号", "支付宝订单号", "姓名", "身份证", "缴费批号", "应缴项目名","应缴金额",'实缴金额','支付时间']; //这个是表头名称 可以是iveiw表格中表头属性的title的数组
                                const filterVal = ["studentid", "trade_no", "username", "passport", "ARID", "name", "amount", "fact_amount",'pay_time']; //与表格数据配合 可以是iview表格中的key的数组
                                const list = self.exportListData; //表格数据，iview中表单数据也是这种格式！
                                const data = self.formatJson(filterVal, list)
                                export_json_to_excel(tHeader, data, '应缴款项') //列表excel  这个是导出表单的名称
                                self.downloadLoading = false;
                                self.exportVisible = false;
                            });
                        }else {
                            self.$message({
                                showClose: true,
                                message: res.msg,
                                type: 'warning'
                            });
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });


            },
            download2(){
                window.open(this.platUrl+'Template/PayablePay.xlsx');
              /*  var a = document.createElement('a');
                var event = new MouseEvent('click');
                // 下载名字
                a.download = '应缴款项模板';
                // a.href = encodeURIComponent('http://192.168.1.4:88/Template/PayablePay.xlsx');
                console.log(1111)
                a.href = 'https://www.baidu.com';
                console.log(this.platUrl+'Template/PayablePay.xlsx')
                //合成函数，执行下载
                a.dispatchEvent(event);*/
             /*   let self = this;
               self.downloadLoading = true;
               require.ensure([], () => {
                   const tHeader = ["缴费批号", "缴费名称", "缴费金额", "收款帐号PID", "创建时间", "姓名","身份证号"]; //这个是表头名称 可以是iveiw表格中表头属性的title的数组
                   const filterVal = ["studentid", "trade_no", "username", "passport", "ARID", "name", "amount"]; //与表格数据配合 可以是iview表格中的key的数组
                   const list =[{studentid:"201503010133", trade_no:"20150301013",username: "4343", passport:"411628199102284567", ARID:"gfg",name: "2112",amount: "32"}];  //表格数据，iview中表单数据也是这种格式！
                   const data = self.formatJson(filterVal, list);
                    export_json_to_excel(tHeader, data, '应缴款项') //列表excel  这个是导出表单的名称
                   self.downloadLoading = false
               });*/

            },
            moneyTable:function(row, column){
                //金额
                return  currency(row.amount,'￥');
            },
            moneyTotalTable:function(row, column){
                //金额
                return  currency(row.fact_amount,'￥');
            },
            moneyTableProj:function(row, column){
                //金额
                return  currency(row.amount,'￥');
            },
            moneyTotalTableProj:function(row, column){
                //金额
                return  currency(row.fact_amount,'￥');
            },
            feeStateTable:function(row, column){
                //收费状态
                if(row.status == 1){
                    return '已关闭';
                }else {
                    return '已开启';
                }
            },
            uploadUrl(){
                var url = this.axios.defaults.baseURL+"/api/Check/UpFileARIDandPassportIsRepeat";
                return url;
            },
            handlePreview(file){
                var fileName=new Array()
                fileName =file.name.split('.');
                // const extension = fileName[fileName.length-1] === 'xls';
                const extension2 =  fileName[fileName.length-1]=== 'xlsx';
                // !extension &&
                if ( !extension2) {
                    this.$message({
                        message: '上传模板只能是xlsx格式!',
                        type: 'warning'
                    });
                }


            },
            handleSuccess(file){
                this.$message({
                    message: '上传成功!',
                    type: 'success'
                });
            },
            handleError(file){
                console.log(file);
                console.log("上传失败")
            },
            submitUpload(file){
                this.dialogVisible = false;
                this.$refs.upload.submit();
                this.tableList();
            },
            reset(){
                this.isActive = true;
                this.isShowGraph = true;
                this.stopStr = null;
                this.inputStr = '';
                this.configTable.iDisplayStart = 0;
                this.configTable.currentPage = 1;
                this.tableList();
            }
        }
    }
  </script>

<style scoped>

</style>