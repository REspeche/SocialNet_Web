mainApp.controller("changePassCtrl", changePassCtrl);
changePassCtrl.$inject = ["$rootScope", "$scope", "$controller", "mainService"];

function changePassCtrl($rootScope, $scope, $controller, mainService) {
    $rootScope.bntLogin = false; //is loaded then show button
    $scope.IsMatch = false;
    $scope.Successfully = false;
    $scope.HashExpired = false;
    $scope.passNewR = '';

    $scope.dataRP = {
        passOld: '',
        passNew: '',
        hash: getUrlVars()['c']
    };

    $scope.submitResetPass = function () {
        if ($scope.dataRP.passNew != $scope.passNewR) {
            $scope.IsMatch = true;
            return false;
        }
        $scope.IsMatch = false;

        if ($scope.frmResetPass.$invalid) {
            angular.forEach($scope.frmResetPass.$error, function (field) {
                angular.forEach(field, function (errorField) {
                    errorField.$setTouched();
                })
            });
        } else {
            mainService.ajax('/account/SetNewPassword',
                $scope.dataRP,
                function (data) {
                    switch (data.code) {
                        case 0:
                            $scope.Successfully = true;
                            $rootScope.isLogin = true;
                            break;
                        case 4:
                            $scope.HashExpired = true;
                            break;
                        default:
                            $rootScope.showAlert().notifyError(data.message);
                            break;
                    }
                });
        }
    }

    $scope.showDialog = function () {
        $rootScope.showDialog('wantresetagain');
    };
}