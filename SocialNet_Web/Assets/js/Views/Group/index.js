mainApp.controller("groupCtrl", groupCtrl);
groupCtrl.$inject = ["$rootScope", "$scope", "mainService"];

function groupCtrl($rootScope, $scope, mainService) {
    var oriData, oriDataAdd;
    var dialogGroup = [];
    $scope._noMoreRecords = false;
    $scope.groups = [];
    $scope.groupsSuggest = [];
    $scope.groupsFriend = [];
    $scope.groupSelected = null;
    $scope.tab = 1;
    $scope.countAll = 0;
    $scope.countReceive = 0;

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

        switch (getHash()) {
            case 'tab1': $scope.tab = 1; break;
            case 'tab2': $scope.tab = 2; break;
            case 'tab3': $scope.tab = 3; break;
        }

        // dialog search
        dialogGroup[0] = document.querySelector('#dialogGroupSearch');
        if (!dialogGroup[0].showModal) dialogPolyfill.registerDialog(dialogGroup[0]);

        dialogGroup[0].addEventListener('close', function () {
            $scope.$broadcast('angucomplete-alt:clearInput', 'txtFindGroup');
            document.getElementById("txtFindGroup_value").value = "";
            toastr.remove();
            setHash('');
            $scope.data = angular.copy(oriData);
            $rootScope.isDialogOpen = false;
            $scope.$applyAsync(function () {
                $scope.groupSelected = null;
            });
        });

        dialogGroup[0].querySelector('.cancel').addEventListener('click', function () {
            dialogGroup[0].close();
        });

        // dialog add
        dialogGroup[1] = document.querySelector('#dialogGroupAdd');
        if (!dialogGroup[1].showModal) dialogPolyfill.registerDialog(dialogGroup[1]);

        dialogGroup[1].addEventListener('close', function () {
            toastr.remove();
            setHash('');
            $scope.dataAdd = angular.copy(oriDataAdd);
            $rootScope.isDialogOpen = false;
        });

        dialogGroup[1].querySelector('.cancel').addEventListener('click', function () {
            dialogGroup[1].close();
        });

        // add button
        $('#btnAddItem').on('click', function () {
            if ($scope.tab == 1) {
                setHash('#groupAdd');
                $scope.dataAdd = angular.copy(oriDataAdd);
                $rootScope.isDialogOpen = true;
                dialogGroup[1].showModal();
            }
            else {
                setHash('#groupSearch');
                $scope.data = angular.copy(oriData);
                $rootScope.isDialogOpen = true;
                dialogGroup[0].showModal();
            }
        });

        $scope.getCounts();
        $scope.getMoreGroup();
    };

    $scope.clickTab = function (tabIndex) {
        $scope.tab = tabIndex;
        $rootScope.btnAddItem = false;
        switch ($scope.tab) {
            case 1:
                setHash('#tab1');
                $scope.groups = [];
                if ($scope.groups.length == 0) $scope.getMoreGroup();
                break;
            case 2:
                setHash('#tab2');
                $scope.groupsSuggest = [];
                if ($scope.groupsSuggest.length == 0) $scope.getMoreGroup();
                break;
            case 3:
                setHash('#tab3');
                $scope.groupsFriend = [];
                if ($scope.groupsFriend.length == 0) $scope.getMoreGroup();
                break;
        }
    };

    $scope.getCounts = function () {
        mainService.ajax('/group/getcounts',
            {},
            function (data) {
                if (data.code == 0) {
                    $scope.countAll = data.countAll;
                    $scope.countReceive = data.countReceive;
                }
            });
    };

    $scope.showMore = function () {
        $scope.getMoreGroup();
    };
    $scope.getMoreGroup = function () {
        lastId = 0;
        switch ($scope.tab) {
            case 1:
                if ($scope.groups.length > 0) lastId = $scope.groups[$scope.groups.length - 1].groupId;
                break;
            case 2:
                if ($scope.groupsSuggest.length > 0) lastId = $scope.groupsSuggest[$scope.groupsSuggest.length - 1].groupId;
                break;
            case 3:
                if ($scope.groupsFriend.length > 0) lastId = $scope.groupsFriend[$scope.groupsFriend.length - 1].groupId;
                break;
        }
        if (lastId == 0) $scope._noMoreRecords = false;
        mainService.ajax('/group/getgroups',
            {
                'lastId': lastId,
                'tab': $scope.tab
            },
            function (data) {
                if (data.code == 0) {
                    switch ($scope.tab) {
                        case 1:
                            $scope.groups.push.apply($scope.groups, data.items);
                            $rootScope.btnAddItem = true;
                            break;
                        case 2:
                            $scope.groupsSuggest.push.apply($scope.groupsSuggest, data.items);
                            break;
                        case 3:
                            $scope.groupsFriend.push.apply($scope.groupsFriend, data.items);
                            $rootScope.btnAddItem = true;
                            break;
                    }
                    if (data.items.length == 0) $scope._noMoreRecords = true;
                }
            });
    };
    $scope.followGroup = function (index) {
        mainService.ajax('/group/followGroup',
            { 'id': $scope.groups[index].joinId },
            function (data) {
                if (data.code == 0) {
                    $scope.groups[index].following = !$scope.groups[index].following;
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.removeGroup = function (index) {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/group/removegroup',
                { 'id': $scope.groups[index].groupId },
                function (data) {
                    if (data.code == 0) {
                        if ($scope.groups.length > 0) $scope.groups.splice(index, 1);
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };
    $scope.unlinkGroup = function (index) {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/group/unlinkgroup',
                { 'id': $scope.groups[index].groupId },
                function (data) {
                    if (data.code == 0) {
                        if ($scope.groups.length > 0) {
                            $scope.countAll--;
                            $scope.groups.splice(index, 1);
                        }
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };
    $scope.submitGroupSearch = function () {
        mainService.ajax('/group/joinGroup',
            {
                'idGroup': $scope.groupSelected.groupId,
                'idMember': ($rootScope.session.userId == $rootScope.session.view_userId) ? 0 : $rootScope.session.view_userId,
                'send': 0
            },
            function (data) {
                switch (data.code) {
                    case 0:
                        if ($rootScope.isDialogOpen) dialogGroup[0].close();
                        $rootScope.showAlert().notify(data.message);
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
    $scope.submitGroupAdd = function () {
        mainService.ajax('/group/addGroup',
            $scope.dataAdd,
            function (data) {
                if (data.code == 0) {
                    $scope.groups.push.apply($scope.groups, data.items);
                    if ($rootScope.isDialogOpen) dialogGroup[1].close();
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.acceptGroup = function (index) {
        mainService.ajax('/group/acceptGroup',
            { 'id': $scope.groupsSuggest[index].joinId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.tab == 2 && $scope.groupsSuggest.length > 0) {
                        $scope.groupsSuggest.splice(index, 1);
                        $scope.groups = [];
                        $scope.countAll++;
                        $scope.countReceive--;
                    }
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.rejectGroup = function (index) {
        mainService.ajax('/group/rejectgroup',
            { 'id': $scope.groupsSuggest[index].joinId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.tab == 2 && $scope.groupsSuggest.length > 0) {
                        $scope.groupsSuggest.splice(index, 1);
                        $scope.countReceive--;
                    }
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.requestInvitation = function (index, send) {
        mainService.ajax('/group/JoinGroup',
            {
                'idGroup': $scope.groupsFriend[index].groupId,
                'idMember': 0,
                'send': send
            },
            function (data) {
                switch (data.code) {
                    case 0:
                        if (data.items.length == 1) $scope.groupsFriend[index].joinId = data.items[0].joinId;
                        $rootScope.showAlert().notify(data.message);
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
    $scope.cancelJoin = function (index) {
        mainService.ajax('/group/cancelJoin',
            { 'id': $scope.groupsFriend[index].joinId },
            function (data) {
                if (data.code == 0) {
                    $scope.groupsFriend[index].joinId = null;
                    $rootScope.showAlert().notify(data.message);
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    // Find groups

    $scope.remoteUrlRequestFn = function (str) {
        return { searchstr: str };
    };

    $scope.itemRender = function (selected) {
        if (selected) $scope.groupSelected = selected.originalObject;
    };
}

function refreshBindPreviewMultimedia() {
    componentHandler.upgradeAllRegistered();
}