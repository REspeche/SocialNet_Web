mainApp.config(['$compileProvider', function ($compileProvider) {
    $compileProvider.debugInfoEnabled(false);
}]);

mainApp.run(["$rootScope",
         function ($rootScope) {
             $rootScope.isLogin = false;
             $rootScope.isBusy = false;
             $rootScope.isDialogOpen = false;
             $rootScope.bntLogin = false; //login button
             $rootScope.btnAddItem = false; //add button
             $rootScope.badges = {
                 all: 0,
                 friend: 0,
                 group: 0,
                 member: 0,
                 follower: 0
             }; //Badges
             $rootScope.allBadges = function () {
                 return $rootScope.badges.all;
             };

             // is mobile
             $rootScope.isMobile = {
                 Android: function () {
                     return navigator.userAgent.match(/Android/i);
                 },
                 BlackBerry: function () {
                     return navigator.userAgent.match(/BlackBerry/i);
                 },
                 iOS: function () {
                     return navigator.userAgent.match(/iPhone|iPad|iPod/i);
                 },
                 Opera: function () {
                     return navigator.userAgent.match(/Opera Mini/i);
                 },
                 Windows: function () {
                     return navigator.userAgent.match(/IEMobile/i);
                 },
                 any: function () {
                     return ($rootScope.isMobile.Android() || $rootScope.isMobile.BlackBerry() || $rootScope.isMobile.iOS() || $rootScope.isMobile.Opera() || $rootScope.isMobile.Windows());
                 }
             };

             // Toast
             $rootScope.showAlert = function () {
                 var options = {
                     timeout: 3000,
                     extendedTimeOut: 1000,
                     positionClass: 'toast-top-center',
                     closeButton: false,
                     newestOnTop: true,
                     progressBar: false,
                     showEasing: 'swing',
                     hideEasing: 'linear',
                     showMethod: 'fadeIn',
                     hideMethod: 'fadeOut',
                     target: ($rootScope.isDialogOpen) ? 'dialog[open] > div.mdl-content' : 'body'
                 };
                 return {
                     notify: function (msg) {
                         toastr.success(msg, '', options);
                     },
                     notifyError: function (msg) {
                         toastr.error(msg, '', options);
                     },
                     notifyInfo: function (msg) {
                         toastr.info(msg, '', options);
                     }
                 }
             };

             //Reset var session
             $rootScope.resetVarSession = function () {
                 // User's session
                 $rootScope.setVarSession(
                     {
                         userId: 0,
                         userName: '',
                         userCode: '',
                         photoProfile: '',
                         photoCover: '',
                         typeEntity: 0,
                         locale: document.getElementsByTagName("html")[0].getAttribute("lang"),
                         view_userId: 0,
                         view_userName: '',
                         view_userCode: '',
                         view_photoProfile: '',
                         view_photoCover: '',
                         view_typeEntity: 0,
                         view_targetId: 0,
                         view_isFriend: 0,
                         view_isMember: 0,
                         view_isFollow: 0,
                         view_canPost: 0
                     }
                 );
                 $rootScope.badges = {
                     all: 0,
                     friend: 0,
                     group: 0,
                     member: 0,
                     follower: 0
                 };
             };
             $rootScope.setVarSession = function (data) {
                 $rootScope.session = {
                    userId : data.userId,
                    userName : data.userName,
                    userCode : data.userCode,
                    photoProfile: data.photoProfile,
                    photoCover: data.photoCover,
                    typeEntity: data.typeEntity,
                    locale : data.locale,
                    view_userId : data.view_userId,
                    view_userName : data.view_userName,
                    view_userCode : data.view_userCode,
                    view_photoProfile: data.view_photoProfile,
                    view_photoCover: data.view_photoCover,
                    view_typeEntity: data.view_typeEntity,
                    view_targetId: data.view_targetId,
                    view_isFriend: data.view_isFriend,
                    view_isMember: data.view_isMember,
                    view_isFollow: data.view_isFollow,
                    view_canPost: data.view_canPost
                }
             };
             $rootScope.resetVarSession();

             $rootScope.calculateDateTimePost = function () {
                 var x = document.getElementsByClassName("post_show_date");
                 var len = x.length;
                 for (var i = 0; i < len; i++) {
                     x[0].innerHTML = timeConverter(x[0].getAttribute("data-time"));
                     x[0].removeAttribute("class");
                 }
             };

             // Dialog confirm message
             var dialogMsg = document.querySelector('#dialogMessage');
             if (!dialogMsg.showModal) dialogPolyfill.registerDialog(dialogMsg);
             $rootScope.confirmMessage = function (type, yesFunction) {
                 $rootScope.yesConfirmFunction = yesFunction;
                 dialogMsg.showModal();
             };
             dialogMsg.querySelector('.accept').addEventListener('click', function () {
                 if (typeof $rootScope.yesConfirmFunction == 'function') $rootScope.yesConfirmFunction();
                 dialogMsg.close();
             });
             dialogMsg.querySelector('.cancel').addEventListener('click', function () {
                 dialogMsg.close();
             });

             // Permissions
             $rootScope.permissionCanPost = function () {
                 return $rootScope.isLogin &&
                        $rootScope.btnAddItem &&
                        (
                            ($rootScope.session.view_typeEntity == 0) || //public
                            ($rootScope.session.view_typeEntity == 1 && $rootScope.session.view_isFriend == 1) || //person
                            ($rootScope.session.view_typeEntity == 2 && $rootScope.session.view_isFollow == 1 && $rootScope.session.view_canPost == 1) || //entity
                            ($rootScope.session.view_typeEntity == 3 && $rootScope.session.view_isMember == 1) || //group
                            ($rootScope.session.userId == $rootScope.session.view_userId) //mine
                        );
             };
         }]);

mainApp.controller("mainCtrl", mainCtrl);
mainCtrl.$inject = ["$rootScope", "$scope", "$timeout", "mainService"];

mainApp.service("mainService", mainService);
mainService.$inject = ["$http", "$rootScope"];

mainApp.directive('untouch', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            untouch: '='
        },
        link: function (scope, element) {
            element.bind('focus', function () {
                scope.untouch.$setUntouched();
                scope.$apply();
            });
        }
    };
});

mainApp.directive('keyEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.myEnter);
                });
                event.preventDefault();
            }
        });
    };
});

$(function () {
    $(window).resize(windowResize);
    windowResize();
});

function windowResize() {
    var rightPos = document.getElementById("content").offsetLeft + 20;
    var btnAddItem = document.getElementById("btnAddItem");
    if (btnAddItem) btnAddItem.style.right = Math.round(rightPos) + "px";
}

/* angular functions */

function mainCtrl($rootScope, $scope, $timeout, mainService) {
    $scope.alerts = [];

    //Get badges
    $rootScope.getBadges = function () {
        mainService.ajax('/common/getbadges',
            {},
            function (data) {
                if (data.code == 0) {
                    $rootScope.badges = data
                }
            });
    };

    //Get alerts (scope)
    $scope.getAlerts = function () {
        lastId = 0;
        if ($scope.alerts.length > 0) lastId = $scope.alerts[$scope.alerts.length - 1].id;
        mainService.ajax('/common/getalerts',
            {
                'lastId': lastId
            },
            function (data) {
                if (data.code == 0) {
                    $scope.alerts.push.apply($scope.alerts, data.items);
                    $timeout(function () {
                        $rootScope.calculateDateTimePost();
                    }, 1000);
                }
            });
    };

    $scope.viewAlert = function (index) {
        if (!$scope.alerts[index].view) {
            mainService.ajax('/common/viewalert',
                {
                    'id': $scope.alerts[index].id
                },
                function (data) {
                    if (data.code == 0) {
                        $scope.alerts[index].view = true;
                        $rootScope.badges.all--;
                        switch ($scope.alerts[index].type) {
                            case 2:
                                $rootScope.badges.friend--;
                                break;
                            case 4:
                                $rootScope.badges.group--;
                                break;
                            case 5:
                                $rootScope.badges.member--;
                                break;
                            case 6:
                                $rootScope.badges.follower--;
                                break;
                        }
                    }
                });
        }
    };

    $scope.followEntity = function () {
        var routeAction;
        switch ($rootScope.session.view_typeEntity) {
            case 1:
                routeAction = '/friend/followfriend';
                break;
            case 2:
                routeAction = '/company/followcompany';
                break;
            case 3:
                routeAction = '/group/followgroup';
                break;
        }
        mainService.ajax(routeAction,
            { 'id': 0 },
            function (data) {
                if (data.code == 0) {
                    $rootScope.session.view_isFollow = ($rootScope.session.view_isFollow == 0) ? 1 : 0;
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    }
}

function mainService($http, $rootScope) {
    var factory = {};

    factory.ajax = function (url, parameters, successFunc) {
        toastr.remove();
        $rootScope.isBusy = true;
        $http({
            method: 'POST',
            url: url,
            data: parameters
        }).
        success(function (data, status, headers, config) {
            var callSuccess = function () {
                switch (successFunc.length) {
                    case 1:
                        successFunc(data);
                        $rootScope.isBusy = false;
                        executeActionMensage(data.action);
                        break;
                    case 2:
                        successFunc(data, function () {
                            $rootScope.isBusy = false;
                            executeActionMensage(data.action);
                        });
                        break;
                };
            };
            if (data.action != 0)
                $('div.mdl-layout__container').hide('slow', function () {
                    callSuccess()
                });
            else callSuccess();
        })
        .error(function (data, status, headers, config) {
            $rootScope.isBusy = false;
            $rootScope.showAlert().notifyError('Hubo un error en la petición.');
        });
    };
    factory.fileUpload = function (uploadUrl, file, data, successFunc) {
        $rootScope.isBusy = true;
        var fd = new FormData();
        fd.append('file', file);
        fd.append('data', JSON.stringify(data));
        $http.post(uploadUrl, fd, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
        .success(function (data, status, headers, config) {
            switch (successFunc.length) {
                case 1:
                    successFunc(data);
                    $rootScope.isBusy = false;
                    executeActionMensage(data.action);
                    break;
                case 2:
                    successFunc(data, function () {
                        $rootScope.isBusy = false;
                        executeActionMensage(data.action);
                    });
                    break;
            };
        })
        .error(function (data, status, headers, config) {
            $rootScope.isBusy = false;
            $rootScope.showAlert().notifyError('Hubo un error en la petición.');
        });
    };

    return factory;
}