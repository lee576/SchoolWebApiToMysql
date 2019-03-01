//日期获取
export function getDateType(type,add){
    var now = new Date();
    now.setDate(now.getDate() + add);
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var h = now.getHours();
    var m = now.getMinutes();
    var s = now.getSeconds();
    if (type == 1) {
        return year + '-' + (month >= 10 ? month : '0' + month) + '-' + (day >= 10 ? day : '0' + day);
    } else {
        return year + '-' + (month >= 10 ? month : '0' + month) + '-' + (day >= 10 ? day : '0' + day) + ' ' + (h >= 10 ? h : '0' + h) + ':' + (m >= 10 ? m : '0' + m) + ':' + (s >= 10 ? s : '0' + s);
    }
}
export function dateSubstr(date){
    return date.split(' ')[0]
}