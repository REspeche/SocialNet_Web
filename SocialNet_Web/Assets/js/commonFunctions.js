function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function getQueryStringValue(key) {
    return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}

function setHash(value) {
    window.location.hash = '';
    window.history.pushState(null, null, (value == '') ? window.location.pathname + window.location.search : value);
}

function getHash() {
    var hash = '';
    if (window.location.hash.length > 0) hash = window.location.hash.split('?')[0].substring(1).toLowerCase().replace('/','');
    return hash;
}

function youtube_parser(url) {
    var regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*/;
    var match = url.match(regExp);
    return (match && match[7].length == 11) ? match[7] : false;
}

function executeActionMensage(action) {
    switch (action) {
        case 0: break;
        case 1: location.href = "/#refreshhome"; break;
        case 2: location.href = "/"; break;
        case 3:
            var elem = angular.element(document.querySelector('[ng-app]'));
            var injector = elem.injector();
            var $rootScope = injector.get('$rootScope');
            location.href = "/" + $rootScope.session.userCode;
            break;
    }
}

function onFinishRender($timeout, $rootScope) {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            if (scope.$last === true) {
                $timeout(function () {
                    element.ready(function () {
                        if (typeof refreshBindPreviewMultimedia=='function') refreshBindPreviewMultimedia();

                        var hash = getHash();
                        if (hash != '' && $rootScope.isLogin) {
                            switch (hash) {
                                case 'post':
                                case 'friend':
                                    $('#btnAddItem').click();
                                    break;
                            }
                        }
                    });
                }, 1000);
            }
        }
    }
}
function removeClassFromTag(className) {
    var x = document.getElementsByClassName(className);
    for (var i = 0; i < x.length; i++) {
        x[i].className = x[i].className.replace(className, '');
    }
}
function timeConverter(UNIX_timestamp, withoutTime) {
    var a = new Date(UNIX_timestamp * 1000);
    var year = a.getFullYear();
    var month = a.getMonth()+1;
    var date = a.getDate();
    var hour = a.getHours();
    var min = a.getMinutes();
    var sec = a.getSeconds();
    var time = (withoutTime) ?
        date + '/' + ((month < 10) ? '0' + month : month) + '/' + year :
        date + '/' + ((month < 10) ? '0' + month : month) + '/' + year + ' ' + ((hour < 10) ? '0' + hour : hour) + ':' + ((min < 10) ? '0' + min : min) + ':' + ((sec < 10) ? '0' + sec : sec);
    return time;
}

function whenScroll($rootScope) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            document.getElementsByTagName("main")[0].addEventListener('scroll', function () {
                if (document.getElementsByTagName("main")[0].scrollTop + document.getElementsByTagName("main")[0].offsetHeight >= document.getElementById("content").offsetHeight * .9) {
                    if (!$rootScope.isBusy && !scope._noMoreRecords) scope.$apply(attrs.whenscroll);
                }
            });
        }
    };
};