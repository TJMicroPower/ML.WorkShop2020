var logDebug = true

//输出js日志
function log(title, value, type, isDebugger = false) {
    var url = window.location.href;
    if (url.indexOf('htlx') > -1) {
        logDebug = false;
    }
    if (logDebug) {
        if (type == 1) {
            //字符串
            console.log(`${title}----${value}`)
        } else if(type == 2) {
            //对象
            console.log(`${title}----${JSON.stringify(value)}`)
        }

        if (isDebugger) {
            debugger;
        }
    }
}

//获取url参数
function getQuery(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}

////格式化Html
//function setViewHtmlFormat(value) {
//    return value.replace(/<[^>]+>/gim, '').replace(/\[(\w+)[^\]]*](.*?)\[\/\1]/g, '$2 ');
//}

//建设中
function construction(that) {
    that.$notify({
        title: "Success",
        message: '正在建设中',
        type: "success",
        duration: 2000
    });
}

function convertTime(date) {
    var json_date = new Date(date).toJSON();
    return new Date(new Date(json_date) + 8 * 3600 * 1000).toISOString().replace(/T/g, ' ').replace(/\.[\d]{3}Z/, '')
}

