var env = process.env.NODE_ENV === 'development' ? 'development' :
    process.env.VUE_APP_TITLE === 'alpha' ? 'alpha' : 'production';
let baseUrl= "";   //这里是一个默认的url，可以没有
let platUrl = "";
switch (env) {
    case 'development':
        baseUrl = "http://localhost:5000/";  //这里是本地的请求url
        platUrl = "http://localhost:8080/";
        break
    case 'alpha':   // 注意这里的名字要和步骤二中设置的环境名字对应起来
        baseUrl = "http://192.168.1.4:5000/";  //这里是测试环境中的url
        platUrl = "http://192.168.1.4:88/";

        break
    case 'production':
        baseUrl = "http://apitext.newxiaoyuan.com";   //生产环境url
        platUrl = "http://text.newxiaoyuan.com";
        break
}
export  { baseUrl ,platUrl}
