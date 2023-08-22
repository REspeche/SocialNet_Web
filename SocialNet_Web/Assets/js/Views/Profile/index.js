mainApp.controller("profileCtrl", profileCtrl);
profileCtrl.$inject = ["$rootScope", "$scope", "mainService"];

mainApp.config(['ADMdtpProvider', function(ADMdtp) {
    ADMdtp.setOptions({
        calType: 'gregorian',
        format: 'DD/MM/YYYY',
        dtpType: 'date',
        autoClose: true,
        multiple: false,
        transition: false,
        language: document.getElementsByTagName("html")[0].getAttribute("lang")
    });
}]);

function profileCtrl($rootScope, $scope, mainService) {
    $scope.dataBD = {
        email: '',
        firstname: '',
        lastname: '',
        gender: 0,
        genderLabel: 'No Especificado',
        birthdate: '',
        language: 0,
        languageLabel: '',
        country: 0,
        countryLabel: '',
        photoProfile: '',
        photoCover: '',
        companyname: '',
        canPost: 0
    };
    $scope.dataRemove = {
        password: ''
    };
    $scope.dataProfile = angular.copy($scope.dataBD);

    $scope.isEditProfile = [];
    $scope.countryList = [];
    $scope.file = undefined;

    $scope.loadInit = function () {
        $scope.isEditProfile[0] = false;
        $scope.isEditProfile[1] = false;
        $scope.isEditProfile[2] = false;
        $scope.isEditProfile[3] = false;
        $scope.isEditProfile[4] = false;

        mainService.ajax('/profile/getprofiledata',
            {},
            function (data) {
                if (data.code == 0) $scope.dataBD = angular.copy(data.item);
            });
    };

    $scope.editProfile = function (idForm) {
        $scope.dataProfile = angular.copy($scope.dataBD);
        $scope.isEditProfile[idForm] = true;
        switch (idForm) {
            case 0:
                if ($scope.countryList.length == 0) {
                    mainService.ajax('/common/getlistcountry',
                        {},
                        function (data) {
                            if (data.code == 0) {
                                $scope.countryList = data.items;
                            }
                        });
                }
                break;
            default:
                break;
        }
    };

    $scope.removeAccount = function () {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/account/removeAccount',
                {},
                function (data) {
                    if (data.code != 0) {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };

    $scope.removeGroup = function () {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/group/removegroupfromprofile',
                {
                    'id': $rootScope.session.view_userId
                },
                function (data) {
                    if (data.code != 0) {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };

    $scope.cancelProfile = function (idForm) {
        switch (idForm) {
            case 0:
                break;
            case 1:
            case 2:
                $scope.file = undefined;
                break;
        };
        $scope.isEditProfile[idForm] = false;
    };

    $scope.submitProfile = function (idForm) {
        switch (idForm) {
            case 0:
                var routeAction, dataParam;
                switch ($rootScope.session.view_typeEntity) {
                    case 1:
                        routeAction = '/profile/saveprofile';
                        dataParam = {
                            firstname: $scope.dataProfile.firstname,
                            lastname: $scope.dataProfile.lastname,
                            gender: document.getElementById("selGender").value,
                            birthdate: $scope.dataProfile.birthdate,
                            language: document.getElementById("selLanguage").value,
                            country: document.getElementById("selCountry").value,
                        };
                        break;
                    case 2:
                        routeAction = '/company/saveprofile';
                        dataParam = {
                            companyname: $scope.dataProfile.firstname,
                            country: document.getElementById("selCountry").value,
                            language: document.getElementById("selLanguage").value
                        };
                        break;
                    case 3:
                        routeAction = '/group/saveprofile';
                        dataParam = {
                            groupname: $scope.dataProfile.firstname,
                            createdate: $scope.dataProfile.birthdate,
                            country: document.getElementById("selCountry").value,
                        };
                        break;
                }
                mainService.ajax(routeAction,
                    dataParam,
                    function (data) {
                        if (data.code == 0) {
                            $scope.dataBD.firstname = data.item.firstname;
                            $scope.dataBD.lastname = data.item.lastname;
                            $scope.dataBD.gender = data.item.gender;
                            $scope.dataBD.birthdate = data.item.birthdate;
                            $scope.dataBD.language = data.item.language;
                            $scope.dataBD.country = data.item.country;
                            $scope.dataBD.genderLabel = data.item.genderLabel;
                            $scope.dataBD.languageLabel = data.item.languageLabel;
                            $scope.dataBD.countryLabel = data.item.countryLabel;
                            $scope.dataBD.birthdateLabel = data.item.birthdateLabel;
                            if ($rootScope.session.view_typeEntity == 1) {
                                $rootScope.session.userName = data.item.firstname + " " + data.item.lastname;
                                $rootScope.session.view_userName = data.item.firstname + " " + data.item.lastname;
                            }
                            else {
                                $rootScope.session.view_userName = data.item.firstname;
                            }
                            $scope.isEditProfile[idForm] = false;
                            $rootScope.showAlert().notify(data.message);
                        }
                        else {
                            $rootScope.showAlert().notifyError(data.message);
                        }
                    });
                break;
            case 1:
            case 2:
                var routeAction;
                switch ($rootScope.session.view_typeEntity) {
                    case 1:
                        routeAction = '/profile/' + ((idForm == 1) ? 'savephoto' : 'savecover');
                        break;
                    case 2:
                        routeAction = '/company/' + ((idForm == 1) ? 'savephoto' : 'savecover');
                        break;
                    case 3:
                        routeAction = '/group/' + ((idForm == 1) ? 'savephoto' : 'savecover');
                        break;
                }
                mainService.fileUpload(routeAction,
                    $scope.file,
                    {},
                    function (data) {
                        if (data.code == 0) {
                            if (idForm == 1) {
                                if ($rootScope.session.view_typeEntity == 1 || $rootScope.session.view_typeEntity == 2) $rootScope.session.photoProfile = data.item.photoProfile;
                                $rootScope.session.view_photoProfile = data.item.photoProfile;
                                $scope.dataBD.photoProfile = data.item.photoProfile;
                            }
                            else {
                                if ($rootScope.session.view_typeEntity == 1 || $rootScope.session.view_typeEntity == 2) $rootScope.session.photoCover = data.item.photoCover;
                                $rootScope.session.view_photoCover = data.item.photoCover;
                                $scope.dataBD.photoCover = data.item.photoCover;
                            }
                            $scope.file = undefined;
                            $scope.isEditProfile[idForm] = false;
                            $rootScope.showAlert().notify(data.message);
                        }
                        else {
                            $rootScope.showAlert().notifyError(data.message);
                        }
                    });
                break;
        };
    };

    $scope.changeStateSwitch = function (objSwitch) {
        var dataSwitch = {
            name: objSwitch,
            value: 0
        }
        switch (objSwitch) {
            case 'canPost': dataSwitch.value = $scope.dataBD.canPost; break;
        }
        mainService.ajax('/company/saveSwitch',
            dataSwitch,
            function (data) {
                if (data.code == 0) {

                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    }
}