function getSearchParameters() {
    var prmstr = window.location.search.substr(1);
    return prmstr != null && prmstr != "" ? transformToAssocArray(prmstr) : {};
}

function transformToAssocArray( prmstr ) {

    var params = {};
    var prmarr = prmstr.split("&");
    for ( var i = 0; i < prmarr.length; i++) {
        var tmparr = prmarr[i].split("=");
        params[tmparr[0]] = tmparr[1];
    }

    return params;
}

function sendGET(port, path, data) {
    var xhr = new XMLHttpRequest();
    var url = "http://localhost:" + port + "/" + path +"/?data=" + encodeURIComponent(data);
    xhr.open("GET", url, true);
    xhr.setRequestHeader("Content-Type", "application/json");
    xhr.send();
    return new Promise(resolve => {
        setTimeout(() => {
            resolve();
        }, 100);
    });
}

function sendPOST(port, path, data) {
    var xhr = new XMLHttpRequest();
    var url = "http://localhost:" + port + "/" + path +"/";
    xhr.open("POST", url, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.send(data);

    return new Promise(resolve => {
        setTimeout(() => {
            resolve();
        }, 2000);
    });
}