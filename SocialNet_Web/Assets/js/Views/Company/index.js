mainApp.controller("companyCtrl", companyCtrl);
companyCtrl.$inject = ["$rootScope", "$scope", "mainService"];

function companyCtrl($rootScope, $scope, mainService) {
    var oriData, oriDataAdd;
    var dialogCompany = [];
    $scope._noMoreRecords = false;
    $scope.companies = [];
    $scope.companySelected = null;
    $scope.tab = 1;
    $scope.countAll = 0;

    $scope.data = {
        id: 0,
        entIdO: 0,
        entIdD: 0,
        suggested: 1
    };

    $scope.dataAdd = {
        id: 0,
        name: ""
    };

    $scope.loadInit = function () {
        oriData = angular.copy($scope.data);
        oriDataAdd = angular.copy($scope.dataAdd);

        // dialog search
        dialogCompany[0] = document.querySelector('#dialogCompanySearch');
        if (!dialogCompany[0].showModal) dialogPolyfill.registerDialog(dialogCompany[0]);

        dialogCompany[0].addEventListener('close', function () {
            $scope.$broadcast('angucomplete-alt:clearInput', 'txtFindGroup');
            document.getElementById("txtFindCompany_value").value = "";
            toastr.remove();
            setHash('');
            $scope.data = angular.copy(oriData);
            $rootScope.isDialogOpen = false;
            $scope.$applyAsync(function () {
                $scope.companySelected = null;
            });
        });

        dialogCompany[0].querySelector('.cancel').addEventListener('click', function () {
            dialogCompany[0].close();
        });

        // add button
        $('#btnAddItem').on('click', function () {
            $scope.data = angular.copy(oriData);
            $rootScope.isDialogOpen = true;
            dialogCompany[0].showModal();
        });

        $scope.getCounts();
        $scope.getMoreCompanies();
    };

    $scope.clickTab = function (tabIndex) {
        $scope.tab = tabIndex;
        $rootScope.btnAddItem = false;
        switch ($scope.tab) {
            case 1:
                setHash('#tab1');
                $scope.companies = [];
                if ($scope.companies.length == 0) $scope.getMoreGroup();
                break;
        }
    };

    $scope.getCounts = function () {
        mainService.ajax('/company/getcounts',
            {},
            function (data) {
                if (data.code == 0) {
                    $scope.countAll = data.countAll;
                }
            });
    };

    $scope.getMoreCompanies = function () {
        lastId = 0;
        switch ($scope.tab) {
            case 1:
                if ($scope.companies.length > 0) lastId = $scope.companies[$scope.companies.length - 1].postId;
                break;
        }
        if (lastId == 0) $scope._noMoreRecords = false;
        mainService.ajax('/company/getcompanies',
            {
                'lastId': lastId,
                'tab': $scope.tab
            },
            function (data) {
                if (data.code == 0) {
                    switch ($scope.tab) {
                        case 1:
                            $scope.companies.push.apply($scope.companies, data.items);
                            $rootScope.btnAddItem = true;
                            break;
                    }
                    if (data.items.length == 0) $scope._noMoreRecords = true;
                }
            });
    };
    $scope.followCompany = function (index) {
        mainService.ajax('/company/followCompany',
            { 'id': $scope.companies[index].joinId },
            function (data) {
                if (data.code == 0) {
                    $scope.companies[index].following = !$scope.companies[index].following;
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.unlinkCompany = function (index) {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/company/unlinkCompany',
                { 'id': $scope.companies[index].companyId },
                function (data) {
                    if (data.code == 0) {
                        $scope.countAll--;
                        if ($scope.companies.length > 0) $scope.companies.splice(index, 1);
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };
    $scope.submitCompanySearch = function () {
        mainService.ajax('/company/joinCompany',
            {
                'idCompany': $scope.companySelected.companyId,
                'idFollower': ($rootScope.session.userId == $rootScope.session.view_userId) ? 0 : $rootScope.session.view_userId
            },
            function (data) {
                switch (data.code) {
                    case 0:
                        $scope.countAll++;
                        if ($rootScope.isDialogOpen) dialogCompany[0].close();
                        $scope.companies.push.apply($scope.companies, data.items);
                        break;
                    case 1:
                        $rootScope.showAlert().notifyInfo(data.message);
                        break;
                    default:
                        $rootScope.showAlert().notifyError(data.message);
                        break;
                }
            });
    };
    // Find companies

    $scope.remoteUrlRequestFn = function (str) {
        return { searchstr: str };
    };

    $scope.itemRender = function (selected) {
        if (selected) $scope.companySelected = selected.originalObject;
    };
}

function refreshBindPreviewMultimedia() {
    componentHandler.upgradeAllRegistered();
}