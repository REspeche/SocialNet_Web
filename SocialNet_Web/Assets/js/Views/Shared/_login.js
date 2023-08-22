mainApp.controller("loginCtrl", loginCtrl);
loginCtrl.$inject = ["$rootScope", "$scope", "$cookies", "mainService"];

function loginCtrl($rootScope, $scope, $cookies, mainService) {
    var dialogLogin;
    $rootScope.isLogin = false;
    $scope.showCancel = false;
    $scope.tab = 1;

    $scope.dataL = {
        email: "",
        password: ""
    };

    $scope.dataR_person = {
        firstname: "",
        lastname: "",
        email: "",
        password: ""
    };
    $scope.dataR_company = {
        companyname: "",
        email: "",
        password: "",
    };

    $scope.dataP = {
        email: ""
    };

    $scope.loadLogin = function () {
        // init vars
        $rootScope.bntLogin = true;
        $scope.formVisible = 'login';
        var oriDataL = angular.copy($scope.dataL);
        var oriDataR_person = angular.copy($scope.dataR_person);
        var oriDataR_company = angular.copy($scope.dataR_company);
        var oriDataP = angular.copy($scope.dataP);

        //Get session vars
        if (document.getElementById("SessionVars")) {
            $rootScope.setVarSession(
                {
                    userId: parseInt(document.getElementById("userId").value),
                    userName: document.getElementById("userName").value,
                    userCode: document.getElementById("userCode").value,
                    photoProfile: document.getElementById("photoProfile").value,
                    photoCover: document.getElementById("photoCover").value,
                    typeEntity: parseInt(document.getElementById("typeEntity").value),
                    locale: document.getElementById("locate").value,
                    view_userId: parseInt(document.getElementById("view_userId").value),
                    view_userName: document.getElementById("view_userName").value,
                    view_userCode: document.getElementById("view_userCode").value,
                    view_photoProfile: document.getElementById("view_photoProfile").value,
                    view_photoCover: document.getElementById("view_photoCover").value,
                    view_typeEntity: parseInt(document.getElementById("view_typeEntity").value),
                    view_targetId: parseInt(document.getElementById("view_targetId").value),
                    view_isFriend: parseInt(document.getElementById("view_isFriend").value),
                    view_isMember: parseInt(document.getElementById("view_isMember").value),
                    view_isFollow: parseInt(document.getElementById("view_isFollow").value),
                    view_canPost: parseInt(document.getElementById("view_canPost").value)
                }
            );
            var divDestroy = document.getElementById("SessionVars");
            divDestroy.parentNode.removeChild(divDestroy);
        }

        // Cookies
        if ($rootScope.session.userId == 0) {
            if (getHash() == 'refreshhome') {
                $cookies.remove('FBid');
                $cookies.remove('FBemail');
                setHash('');
            }
            else {
                var idF = $cookies.get('FBid');
                var emailF = $cookies.get('FBemail');
                if (idF && emailF) {
                    $rootScope.loginCall(
                    {
                        email: emailF,
                        id: idF,
                        firstName: null,
                        lastName: null,
                        gender: null
                    }, true);
                }
            }
        }

        // verify current session
        if ($("#loginDiv").data("login") == "1") {
            $rootScope.isLogin = true;
            $("#btnLogin").removeAttr("style");

            //Get badges
            $rootScope.getBadges();
        }

        // dialog
        dialogLogin = document.querySelector('#dialogLogin');
        if (!dialogLogin.showModal) dialogPolyfill.registerDialog(dialogLogin);

        dialogLogin.addEventListener('close', function () {
            toastr.remove();
            $rootScope.isDialogOpen = false;
            setHash('');
            $scope.formVisible = 'login';
            $scope.dataL = angular.copy(oriDataL);
            $scope.dataR_person = angular.copy(oriDataR_person);
            $scope.dataR_company = angular.copy(oriDataR_company);
            $scope.dataP = angular.copy(oriDataP);
        });

        document.querySelector('#btnLogin').addEventListener('click', function () {
            $scope.showCancel = false;
            setHash('#login');
            $rootScope.isDialogOpen = true;
            dialogLogin.showModal();
        });

        document.querySelector('#mnuItemMyProfile').addEventListener('click', function (e) {
            location.href = '/' + $rootScope.session.userCode + '/Profile';
        });
        document.querySelector('#mnuItemCloseSession').addEventListener('click', function (e) {
            e.preventDefault();
            mainService.ajax('/account/closesession',
                {},
                function (data, callBackFunction) {
                    if (data.code == 0) {
                        $cookies.remove('FBid');
                        $cookies.remove('FBemail');
                        $rootScope.isLogin = false;
                        $rootScope.resetVarSession();
                    }
                    callBackFunction();
                });
        });

        $scope.showForm = function (formName) {
            setHash('#' + formName);
            $scope.formVisible = formName;
        };

        var hash = getHash();
        if (hash != '' && !$rootScope.isLogin) {
            switch (hash) {
                case 'wantresetagain':
                    $scope.showCancel = true;
                    $scope.formVisible = hash;
                    setTimeout(function () {
                        $rootScope.isDialogOpen = true;
                        dialogLogin.showModal();
                    }, 1000);
                    break;
                case 'login':
                case 'register':
                case 'wantreset':
                    $scope.formVisible = hash;
                    setTimeout(function () {
                        $rootScope.isDialogOpen = true;
                        dialogLogin.showModal();
                    }, 1000);
                    break;
            }
        }
    };

    //submit form
    $scope.submitLoginKeyPress = function () {
        if (frmLogin.$valid) frmLogin.submit();
    };
    $scope.submitLogin = function () {
        if ($scope.frmLogin.$invalid) {
            angular.forEach($scope.frmLogin.$error, function (field) {
                angular.forEach(field, function (errorField) {
                    errorField.$setTouched();
                })
            });
        } else {
            $rootScope.loginCall($scope.dataL, false);
        }
    };
    $scope.submitRegister = function () {
        switch ($scope.tab) {
            case 1:
                if ($scope.frmRegisterPerson.$invalid) {
                    angular.forEach($scope.frmRegisterPerson.$error, function (field) {
                        angular.forEach(field, function (errorField) {
                            errorField.$setTouched();
                        })
                    });
                }
                else {
                    mainService.ajax('/account/registerPerson',
                        $scope.dataR_person,
                        function (data) {
                            if (data.code == 0) {
                                if ($rootScope.isDialogOpen) dialogLogin.close();
                                $rootScope.session.userId = data.userId;
                                $rootScope.session.userName = data.userName;
                                $rootScope.session.userCode = data.userCode;
                                $rootScope.session.photoProfile = data.photoProfile;
                                $rootScope.session.typeEntity = 1;
                                $rootScope.isLogin = true;
                            }
                            else {
                                $rootScope.showAlert().notifyError(data.message);
                            }
                        });
                }
                break;
            case 2:
                if ($scope.frmRegisterCompany.$invalid) {
                    angular.forEach($scope.frmRegisterCompany.$error, function (field) {
                        angular.forEach(field, function (errorField) {
                            errorField.$setTouched();
                        })
                    });
                }
                else {
                    mainService.ajax('/account/registerCompany',
                        $scope.dataR_company,
                        function (data) {
                            if (data.code == 0) {
                                if ($rootScope.isDialogOpen) dialogLogin.close();
                                $rootScope.session.userId = data.userId;
                                $rootScope.session.userName = data.userName;
                                $rootScope.session.userCode = data.userCode;
                                $rootScope.session.photoProfile = data.photoProfile;
                                $rootScope.session.typeEntity = 2;
                                $rootScope.isLogin = true;
                            }
                            else {
                                $rootScope.showAlert().notifyError(data.message);
                            }
                        });
                }
                break;
        }
    };
    $scope.submitSendResetMail = function () {
        if ($scope.frmPassword.$invalid) {
            angular.forEach($scope.frmPassword.$error, function (field) {
                angular.forEach(field, function (errorField) {
                    errorField.$setTouched();
                })
            });
        } else {
            mainService.ajax('/account/passwordreset',
                $scope.dataP,
                function (data) {
                    if (data.code == 0) {
                        if ($rootScope.isDialogOpen) dialogLogin.close();
                        $rootScope.showAlert().notify(data.message);
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        }
    };

    $rootScope.showDialog = function (formName) {
        $scope.showCancel = true;
        setHash('#' + formName);
        $scope.formVisible = formName;
        $rootScope.isDialogOpen = true;
        dialogLogin.showModal();
    };
    $rootScope.closeDialog = function () {
        if ($rootScope.isDialogOpen) dialogLogin.close();
    };

    $scope.clickTab = function (tabIndex) {
        $scope.tab = tabIndex;
    };

    $rootScope.loginCall = function (data, isFacebook) {
        if (isFacebook) {
            var expireDate = new Date();
            expireDate.setDate(expireDate.getDate() + 30);
            $cookies.put('FBid', data.id, { 'expires': expireDate });
            $cookies.put('FBemail', data.email, { 'expires': expireDate });
        }
        mainService.ajax('/account/login' + ((isFacebook) ? 'facebook' : ''),
            data,
            function (data) {
                if (data.code == 0) {
                    $rootScope.setVarSession(data);
                    $rootScope.isLogin = true;
                    if ($rootScope.isDialogOpen) dialogLogin.close();
                    $rootScope.getBadges();
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    }
}

//Facebook Functions
window.fbAsyncInit = function () {
    FB.init({
        appId: '600096136828258',
        xfbml: true,
        version: 'v2.7'
    });
};

// Load the SDK asynchronously
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/es_LA/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

function statusChangeCallback(response) {
    if (response.status === 'connected') autoLogin();
}

function checkLoginState() {
    var elem = angular.element(document.querySelector('[ng-app]'));
    var injector = elem.injector();
    var $rootScope = injector.get('$rootScope');
    $rootScope.$apply(function () {
        $rootScope.closeDialog();
    });
    FB.login(function (response) {
        statusChangeCallback(response)
    }, { scope: 'public_profile,email,user_birthday,user_location' });
}

function autoLogin() {
    FB.api('/me?fields=id,email,name,first_name,last_name,gender,location,birthday', function (response) {
        var elem = angular.element(document.querySelector('[ng-app]'));
        var injector = elem.injector();
        var $rootScope = injector.get('$rootScope');
        if (response.email && response.id) {
            $rootScope.$apply(function () {
                $rootScope.loginCall(
                            {
                                email: response.email,
                                id: response.id,
                                firstName: response.first_name,
                                lastName: response.last_name,
                                gender: response.gender,
                                birthday: (response.birthday)?response.birthday:"",
                                location: (response.location)?response.location.name:""
                            }, true);
            });
        }
    });
}