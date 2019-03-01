import Vue from 'vue'
import Router from 'vue-router'

// 登录页
import Login from './Login.vue'
import Home from './Home.vue'

// 电子校园卡
import CardIndex from './views/campusCard/cardAdmin/CardIndex.vue'
import CardAdmin from './views/campusCard/cardAdmin/CardAdmin.vue'
import CardAdmin_set from './views/campusCard/cardAdmin/CardAdmin_set.vue'

import CardPersonnel from './views/campusCard/cardPersonnel/CardPersonnel.vue'
import PersonnelAdd from './views/campusCard/cardPersonnel/PersonnelAdd.vue'
import PersonnelEdit from './views/campusCard/cardPersonnel/PersonnelEdit.vue'
import CardSurvey from './views/campusCard/cardSurvey/CardSurvey.vue'

// 缴费大厅
import PaymentSurvey from './views/paymentLobby/paymentSurvey/PaymentSurvey.vue'
import RoutineAdmin from './views/paymentLobby/paymentRoutineadmin/RoutineAdmin.vue'
import RoutineAdmin_Add from './views/paymentLobby/paymentRoutineadmin/RoutineAdmin_Add.vue'
import PaymentDetail from './views/paymentLobby/paymentRoutineadmin/PaymentDetail.vue'
import RoutineAdmin_Check from './views/paymentLobby/paymentRoutineadmin/RoutineAdmin_Check.vue'
import RoutineAdmin_Check_add from './views/paymentLobby/paymentRoutineadmin/RoutineAdmin_Check_add.vue'
import CommonSearchList from './views/paymentLobby/CommonSearch/CommonSearchList.vue'
import PayableManagement from './views/paymentLobby/PayableManagement/PayableManagement.vue'
import ManagementDetail from './views/paymentLobby/PayableManagement/ManagementDetail.vue'
import ReceivingManage_List from './views/paymentLobby/ReceivingManage/ReceivingManage_List.vue'
import ReceivingManage_Add from './views/paymentLobby/ReceivingManage/ReceivingManage_Add.vue'
import Electricitycharge_List from './views/paymentLobby/ElectricitychargeManagement/Electricitycharge_List.vue'
import Waterfee_List from './views/paymentLobby/WaterfeeManage/Waterfee_List.vue'

//宿舍管理
import DormitoryAdmin from './views/DormitoryManagement/DormitoryManagement/DormitoryAdmin.vue'
import PublicFacilities from './views/DormitoryManagement/DormitoryManagement/PublicFacilities.vue'

// 收银台
import record from './views/cashier/record/record.vue'
import Transactionflow from './views/cashier/Transactionflow/Transactionflow.vue'
import BillDownload from './views/cashier/Transactionflow/BillDownload.vue'
import CanteenManagement from './views/cashier/CanteenManagement/CanteenManagement.vue'
import FundManagement from './views/cashier/FundManagement/FundManagement.vue'

// 小程序管理
import AccessControl from './views/AlipayManagement/AccessControl/AccessControl.vue'
import AccessControlAnalysis from './views/AlipayManagement/AccessControl/AccessControlAnalysis.vue'
import EquipmentMaintenance from './views/AlipayManagement/AccessControl/EquipmentMaintenance.vue'
import BluetoothManagement from './views/AlipayManagement/BluetoothManagement/BluetoothManagement.vue'
import SignIn from './views/AlipayManagement/SignIn/SignIn.vue'
import CourseSchedule from './views/AlipayManagement/CourseSchedule/CourseSchedule.vue'


// 系统设置
import ContractManagement from './views/SystemSetup/ContractManagement/ContractManagement.vue'
import UserRights from './views/SystemSetup/UserRights/UserRights.vue'
import UserRightsAdd from './views/SystemSetup/UserRights/UserRightsAdd.vue'
import UserRightsEdit from './views/SystemSetup/UserRights/UserRightsEdit.vue'
import SchoolFunction from './views/SystemSetup/SchoolFunction/SchoolFunction.vue'

Vue.use(Router)

export default new Router({
    mode: 'history',
    base: process.env.BASE_URL,

    routes: [
        {
            path: '/',
            name: 'Login',
            component: Login,
        },
       

        {
            path: '/home',
            name: 'Home',
            component: Home,
            meta: {
                requiresAuth: true
            },
            redirect: {name: 'CardSurvey'},
            children: [

                // 电子校园卡
                {
                    path: '/cardsurvey',
                    name: 'CardSurvey',
                    component: CardSurvey,
                    // meta: {
                    //     requiresAuth: true
                    // }
                },
                {
                    path: '/cardindex',
                    name: 'CardIndex',
                    component: CardIndex,
                    redirect: {name: 'CardAdmin'},
                    children: [
                        {
                            path: '/cardadmin',
                            name: 'CardAdmin',
                            component: CardAdmin,
                        },
                        {
                            path: '/cardadmin_set:id',
                            name: 'CardAdmin_set',
                            component: CardAdmin_set,
                        },
                    ]
                },
                {
                    //收银台
                    path: '/record',
                    name: 'record',
                    component: CardIndex,
                    redirect: {name: 'record'},
                    children: [
                        {
                            path: '/record',
                            name: 'record',
                            component: record,
                        },
                        {
                            path: '/Transactionflow',
                            name: 'Transactionflow',
                            component: Transactionflow,
                        },
                        {
                            path: '/CanteenManagement',
                            name: ' CanteenManagement',
                            component:  CanteenManagement,
                        },
                        {
                            path: '/FundManagement',
                            name: ' FundManagement',
                            component:  FundManagement,
                        },
                        {
                            path: '/BillDownload',
                            name: ' BillDownload',
                            component:  BillDownload,
                        }
                    ]
                },
                {
                    //小程序管理
                    path: '/AccessControl',
                    name: 'AccessControl',
                    component: CardIndex,
                    redirect: {name: 'AccessControl'},
                    children: [
                        {
                            path: '/AccessControl',
                            name: 'AccessControl',
                            component: AccessControl,
                        },{
                            path: '/AccessControlAnalysis',
                            name: 'AccessControlAnalysis',
                            component: AccessControlAnalysis,
                        },{
                            path: '/EquipmentMaintenance',
                            name: 'EquipmentMaintenance',
                            component: EquipmentMaintenance,
                        },{
                            path: '/SignIn',
                            name: 'SignIn',
                            component: SignIn,
                        },
                        {
                            path: '/BluetoothManagement',
                            name: 'BluetoothManagement',
                            component: BluetoothManagement,
                        },
                        {
                            path: '/CourseSchedule',
                            name: 'CourseSchedule',
                            component: CourseSchedule,
                        }
                    ]
                },
                {
                    //系统设置
                    path: '/ContractManagement',
                    name: 'ContractManagement',
                    component: CardIndex,
                    redirect: {name: 'ContractManagement'},
                    children: [
                        {
                            path: '/ContractManagement',
                            name: 'ContractManagement',
                            component: ContractManagement,
                        },
                        {
                            path: '/UserRights',
                            name: 'UserRights',
                            component: UserRights,
                        },
                        {
                            path: '/UserRightsAdd',
                            name: 'UserRightsAdd',
                            component: UserRightsAdd,
                        },
                        {
                            path: '/userrightsedit:id',
                            name: 'UserRightsEdit',
                            component: UserRightsEdit,
                        },
                        {
                            path: '/SchoolFunction',
                            name: 'SchoolFunction',
                            component: SchoolFunction,
                        }
                    ]
                },
                {
                    path: '/cardpersonnel',
                    name: 'CardPersonnel',
                    component: CardPersonnel
                },

                {
                    path: '/personneladd',
                    name: 'PersonnelAdd',
                    component: PersonnelAdd
                },
                {
                    path: '/personneledit:id',
                    name: 'PersonnelEdit',
                    component: PersonnelEdit
                },
                {
                    path: '/paymentdetail',
                    name: 'PaymentDetail',
                    component: CardIndex,
                    redirect: {name: 'PaymentSurvey'},
                    children: [
                        {
                            path: '/paymentsurvey',
                            name: 'PaymentSurvey',
                            component: PaymentSurvey,
                        },
                        {
                            path: '/routineadmin',
                            name: 'RoutineAdmin',
                            component: RoutineAdmin,
                        },
                        {
                            path: '/RoutineAdmin_Add',
                            name: 'RoutineAdmin_Add',
                            component: RoutineAdmin_Add,
                        },
                        {
                            path: '/PaymentDetail',
                            name: 'PaymentDetail',
                            component: PaymentDetail,
                        },
                        {
                            path: '/RoutineAdmin_Check',
                            name: 'RoutineAdmin_Check',
                            component: RoutineAdmin_Check,
                        },
                        {
                            path: '/RoutineAdmin_Check_add',
                            name: 'RoutineAdmin_Check_add',
                            component: RoutineAdmin_Check_add,
                        },
                        {
                            path: '/commonsearchlist',
                            name: 'CommonSearchList',
                            component: CommonSearchList,
                        },
                        {
                            path: '/PayableManagement',
                            name: 'PayableManagement',
                            component: PayableManagement,
                        },
                        {
                            path: '/ManagementDetail',
                            name: 'ManagementDetail',
                            component: ManagementDetail,
                        },
                        {
                            path: '/ReceivingManage_List',
                            name: 'ReceivingManage_List',
                            component: ReceivingManage_List,
                        },
                        {
                            path: '/ReceivingManage_Add',
                            name: 'ReceivingManage_Add',
                            component: ReceivingManage_Add,
                        },
                        {
                            path: '/Electricitycharge_List',
                            name: 'Electricitycharge_List',
                            component: Electricitycharge_List,
                        },
                        {
                            path: '/Waterfee_List',
                            name: 'Waterfee_List',
                            component: Waterfee_List,
                        }
                    ]
                },
                {
                    path: '/dormitoryadmin',
                    name: 'DormitoryAdmin',
                    component: DormitoryAdmin,

                },{
                    path: '/publicfacilities',
                    name: 'PublicFacilities',
                    component: PublicFacilities,

                },
            ]
        },


        // // 电子校园卡
        // {
        //     path: '/cardsurvey',
        //     name: 'CardSurvey',
        //     component: CardSurvey,
        //     // meta: {
        //     //     requiresAuth: true
        //     // }
        // },
        // {
        //     path: '/cardindex',
        //     name: 'CardIndex',
        //     component: CardIndex,
        //     redirect: {name: 'CardAdmin'},
        //     children: [
        //         {
        //             path: '/cardadmin',
        //             name: 'CardAdmin',
        //             component: CardAdmin,
        //         },
        //         {
        //             path: '/cardadmin_set:id',
        //             name: 'CardAdmin_set',
        //             component: CardAdmin_set,
        //         },
        //     ]
        // },
        // {
        //     path: '/paymentdetail',
        //     name: 'PaymentDetail',
        //     component: CardIndex,
        //     redirect: {name: 'RoutineAdmin'},
        //     children: [
        //         {
        //             path: '/paymentsurvey',
        //             name: 'PaymentSurvey',
        //             component: PaymentSurvey,
        //         },
        //         {
        //             path: '/routineadmin',
        //             name: 'RoutineAdmin',
        //             component: RoutineAdmin,
        //         },
        //         {
        //             path: '/RoutneAdmin_Add',
        //             name: 'RoutneAdmin_Add',
        //             component: RoutneAdmin_Add,
        //         },
        //         {
        //             path: '/PaymentDetail',
        //             name: 'PaymentDetail',
        //             component: PaymentDetail,
        //         },
        //         {
        //             path: '/RoutineAdmin_Check',
        //             name: 'RoutineAdmin_Check',
        //             component: RoutineAdmin_Check,
        //         },
        //         {
        //             path: '/RoutineAdmin_Check_add',
        //             name: 'RoutineAdmin_Check_add',
        //             component: RoutineAdmin_Check_add,
        //         },
        //         {
        //             path: '/CommonSearchList',
        //             name: 'CommonSearchList',
        //             component: CommonSearchList,
        //         },
        //         {
        //             path: '/PayableManagement',
        //             name: 'PayableManagement',
        //             component: PayableManagement,
        //         },
        //         {
        //             path: '/ManagementDetail',
        //             name: 'ManagementDetail',
        //             component: ManagementDetail,
        //         },
        //         {
        //             path: '/ReceivingManage_List',
        //             name: 'ReceivingManage_List',
        //             component: ReceivingManage_List,
        //         },
        //         {
        //             path: '/ReceivingManage_Add',
        //             name: 'ReceivingManage_Add',
        //             component: ReceivingManage_Add,
        //         },
        //         {
        //             path: '/Electricitycharge_List',
        //             name: 'Electricitycharge_List',
        //             component: Electricitycharge_List,
        //         },
        //         {
        //             path: '/Waterfee_List',
        //             name: 'Waterfee_List',
        //             component: Waterfee_List,
        //         }
        //     ]
        // },
        // {
        //     path: '/cardpersonnel',
        //     name: 'CardPersonnel',
        //     component: CardPersonnel
        // },
        //
        // {
        //     path: '/campus_user_add',
        //     name: 'Campus_user_add',
        //     component: Campus_user_add
        // },


    ]
})
