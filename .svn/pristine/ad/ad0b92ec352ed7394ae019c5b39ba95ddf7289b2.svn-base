//#创建http.js文件
import { baseUrl,platUrl } from "./setBaseUrl" //路径根据你的实际情况填写
import axios from 'axios'
import router from '@/router'
/*// axios 配置
axios.defaults.timeout = 5000;
axios.defaults.baseURL = 'http://192.168.1.7';

// http request 拦截器
axios.interceptors.request.use(
    config => { //将所有的axios的header里加上token_type和access_token
        // config.headers.Authorization = `${localStorage.token_type} ${localStorage.access_token}`;
        config.headers.Authorization = `${localStorage.token}`;
        return config;
    },
    err => {
        return Promise.reject(err);
    });

// http response 拦截器
axios.interceptors.response.use(
    response => {
        return response;
    },
    error => {
        // 401 清除token信息并跳转到登录页面
        if (error.response.status == 401) {
            alert('登录信息有误！请重新登录')
            router.replace({    //如果失败，跳转到登录页面
                name:'login'
            })
        }
        return Promise.reject(error.response.data)
    });*/
// 创建axios实例
axios.defaults.timeout = 500000;
// axios.defaults.baseURL = 'https://localhost:5001';
// axios.defaults.withCredentials = true;
axios.defaults.baseURL = baseUrl;
/*const $ = axios.create({
    baseURL: 'https://localhost:44322',
    timeout: 15000
});*/
// 请求拦截器
axios.interceptors.request.use((config) => {
    config.headers.Authorization = `${localStorage.token}`;
    showFullScreenLoading()
    return config
}, (error) => {
    return Promise.reject(error)
})

// 响应拦截器
axios.interceptors.response.use((response) => {
    tryHideFullScreenLoading()
    return response
}, (error) => {
    if (error.message.includes('timeout')) {
        // 判断请求异常信息中是否含有超时timeout字符串
        console.log("请求超时", error);
    }
    else if (error.response.status == 401) {
        alert('登录信息有误！请重新登录');
        router.replace({    //如果失败，跳转到登录页面
            name: 'Login'
        })
    }
    return Promise.reject(error)
})


// export default {
//     post: (url, data, config = { showLoading: true }) => $.post(url, data, config)
// }
//
// export default {
//     post: (url, data, config = { showLoading: true }) => $.post(url, data, config)
// }

export default axios;       //然后再次export出去，嘿嘿 main.js那里就得改改咯\

let needLoadingRequestCount = 0
export function showFullScreenLoading() {
    if (needLoadingRequestCount === 0) {
        startLoading()
    }
    needLoadingRequestCount++
}

export function tryHideFullScreenLoading() {
    if (needLoadingRequestCount <= 0) return
    needLoadingRequestCount--
    if (needLoadingRequestCount === 0) {
        endLoading()
    }
}
import { Loading } from 'element-ui'

let loading

function startLoading() {
    loading = Loading.service({
        lock: true,
        text: '加载中……',
        background: 'rgba(0, 0, 0, 0.7)'
    })
}

function endLoading() {
    loading.close()
}

