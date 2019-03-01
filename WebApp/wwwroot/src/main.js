import Vue from 'vue'
import VCharts from 'v-charts'
import App from './App.vue'
import router from './router'
import store from './store'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import './assets/css/main.css'
import './assets/font/iconfont.css'
import {ContainerMixin, ElementMixin} from 'vue-slicksort';
import axios from './http';
import draggable from 'vuedraggable';
import { baseUrl,platUrl } from "./setBaseUrl" //路径根据你的实际情况填写


Vue.config.productionTip = false
Vue.prototype.axios=axios

Vue.use(ElementUI);
Vue.use(VCharts)


router.beforeEach((to, from, next) => {
    if (to.path === '/login') {
        next()
    }
    else {
        if (to.meta.requiresAuth && !sessionStorage.getItem('access_token')) {
            next({path: '/login'})
        }
        else {
            next();
        }
    }
})
new Vue({
    el: '#app',
    router,
    store,
    render: h => h(App)
}).$mount('#app')

Vue.prototype.platUrl = platUrl;
console.log('process')
// console.log(global_);
// console.log(baseUrl);
// console.log(platUrl)
console.log('000000')