mainApp.controller("friendCtrl", friendCtrl);
friendCtrl.$inject = ["$rootScope", "$scope", "mainService"];

function friendCtrl($rootScope, $scope, mainService) {
    var oriData;
    $scope._noMoreRecords = false;
    $scope.friends = [];
    $scope.friendsSend = [];
    $scope.friendsReceive = [];
    $scope.friendSelected = null;
    $scope.tab = 1;
    $scope.countAll = 0;
    $scope.countSend = 0;
    $scope.countReceive = 0;

    $scope.data = {
        id: 0,
        idDo: 0,
        idDd: 0,
        idDr: 0,
        suggested: 1,
        offered: 1
    };

    $scope.loadInit = function () {
        oriData = angular.copy($scope.data);

        switch (getHash()) {
            case 'tab1': $scope.tab = 1; break;
            case 'tab2': $scope.tab = 2; break;
            case 'tab3': $scope.tab = 3; break;
        }

        // dialog
        var dialogFriend = document.querySelector('#dialogFriend');
        if (dialogFriend) {
            if (!dialogFriend.showModal) dialogPolyfill.registerDialog(dialogFriend);

            dialogFriend.addEventListener('close', function () {
                $scope.$broadcast('angucomplete-alt:clearInput', 'txtFindFriend');
                document.getElementById("txtFindFriend_value").value = "";
                toastr.remove();
                setHash('');
                $scope.data = angular.copy(oriData);
                $rootScope.isDialogOpen = false;
                $scope.$applyAsync(function () {
                    $scope.friendSelected = null;
                });
            });

            dialogFriend.querySelector('.cancel').addEventListener('click', function () {
                dialogFriend.close();
            });
        }

        // add button
        $('#btnAddItem').on('click', function () {
            if ($rootScope.session.userId == $rootScope.session.view_userId || $rootScope.session.userId == $rootScope.session.view_targetId) {
                setHash('#friend');
                $scope.data = angular.copy(oriData);
                $rootScope.isDialogOpen = true;
                dialogFriend.showModal();
            }
            else {
                $scope.friendSelected = {
                    userId: $rootScope.session.userId
                };
                $scope.joinGroup();
            }
        });

        $scope.getCounts();
        $scope.getMoreFriend();
    };

    $scope.clickTab = function (tabIndex) {
        $scope.tab = tabIndex;
        $rootScope.btnAddItem = false;
        switch ($scope.tab) {
            case 1:
                setHash('#tab1');
                $scope.friends = [];
                if ($scope.friends.length == 0) $scope.getMoreFriend();
                break;
            case 2:
                setHash('#tab2');
                $scope.friendsSend = [];
                if ($scope.friendsSend.length == 0) $scope.getMoreFriend();
                break;
            case 3:
                setHash('#tab3');
                $scope.friendsReceive = [];
                if ($scope.friendsReceive.length == 0) $scope.getMoreFriend();
                break;
        }
    };

    $scope.getCounts = function () {
        var routeAction;
        switch ($rootScope.session.view_typeEntity) {
            case 1:
                routeAction = '/friend/getcounts';
                break;
            case 2:
                return false;
            case 3:
                routeAction = '/group/getcountsmembers';
                break;
        }
        mainService.ajax(routeAction,
            {},
            function (data) {
                if (data.code == 0) {
                    $scope.countAll = data.countAll;
                    $scope.countSend = data.countSend;
                    $scope.countReceive = data.countReceive;
                }
            });
    };

    $scope.getMoreFriend = function () {
        lastId = 0;
        switch ($scope.tab) {
            case 1:
                if ($scope.friends.length > 0) lastId = $scope.friends[$scope.friends.length - 1].friendId;
                break;
            case 2:
                if ($scope.friendsSend.length > 0) lastId = $scope.friendsSend[$scope.friendsSend.length - 1].friendId;
                break;
            case 3:
                if ($scope.friendsReceive.length > 0) lastId = $scope.friendsReceive[$scope.friendsReceive.length - 1].friendId;
                break;
        }
        var routeAction;
        switch ($rootScope.session.view_typeEntity) {
            case 1:
                routeAction = '/friend/getfriends';
                break;
            case 2:
                routeAction = '/company/getfollowers';
                break;
            case 3:
                routeAction = '/group/getmembers';
                break;
        }
        if (lastId == 0) $scope._noMoreRecords = false;
        mainService.ajax(routeAction,
            {
                'lastId': lastId,
                'tab': $scope.tab
            },
            function (data) {
                if (data.code == 0) {
                    if ($scope.tab == 1 || $scope.tab==2) $rootScope.btnAddItem = true;
                    if (data.items.length == 0) {
                        $scope._noMoreRecords = true;
                        return;
                    }
                    switch ($scope.tab) {
                        case 1:
                            $scope.friends.push.apply($scope.friends, data.items);
                            break;
                        case 2:
                            $scope.friendsSend.push.apply($scope.friendsSend, data.items);
                            break;
                        case 3:
                            $scope.friendsReceive.push.apply($scope.friendsReceive, data.items);
                            break;
                    }
                }
            });
    };
    $scope.showMore = function () {
        $scope.getMoreFriend();
    };
    $scope.followFriend = function (index) {
        mainService.ajax('/friend/followfriend',
            { 'id': $scope.friends[index].friendId },
            function (data) {
                if (data.code == 0) {
                    $scope.friends[index].following = !$scope.friends[index].following;
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.blockFriend = function (index) {
        var funcBlock = function () {
            mainService.ajax('/friend/blockfriend',
                { 'id': $scope.friends[index].friendId },
                function (data) {
                    if (data.code == 0) {
                        $scope.friends[index].blocked = !$scope.friends[index].blocked;
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        }
        if ($scope.friends[index].blocked) funcBlock();
        else {
            $rootScope.confirmMessage("block", function () {
                funcBlock();
            });
        }
    };
    $scope.removeFriend = function (index) {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/friend/removefriend',
                { 'id': $scope.friends[index].friendId },
                function (data) {
                    if (data.code == 0) {
                        if ($scope.friends.length > 0) {
                            $scope.friends.splice(index, 1);
                            $scope.countAll--;
                        }
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };
    $scope.submitFriend = function () {
        var routeAction;
        switch ($rootScope.session.view_typeEntity) {
            case 1:
                routeAction = '/friend/addFriend';
                break;
            case 2:
                routeAction = '/company/addFollower';
                break;
            case 3:
                routeAction = '/group/addMember';
                break;
        }
        mainService.ajax(routeAction,
            {
                'idD': $scope.friendSelected.userId,
                'idR': ($rootScope.session.userId == $rootScope.session.view_userId) ? 0 : $rootScope.session.view_userId
            },
            function (data) {
                switch (data.code) {
                    case 0:
                        $scope.countSend++;
                        if ($rootScope.isDialogOpen && dialogFriend) dialogFriend.close();
                        $rootScope.showAlert().notify(data.message);
                        if ($scope.tab == 2) {
                            $scope.friendsSend = [];
                            $scope.getMoreFriend();
                        }
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
    $scope.joinGroup = function () {
        mainService.ajax('/group/joinGroup',
            {
                'idGroup': 0,
                'idMember': $scope.friendSelected.userId,
                'send': 1
            },
            function (data) {
                switch (data.code) {
                    case 0:
                        $scope.countSend++;
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
    $scope.acceptFriend = function (index) {
        var routeAction;
        switch ($rootScope.session.view_typeEntity) {
            case 1:
                routeAction = '/friend/acceptFriend';
                break;
            case 2:
                routeAction = '/company/acceptFollower';
                break;
            case 3:
                routeAction = '/group/acceptMember';
                break;
        }
        mainService.ajax(routeAction,
            { 'id': $scope.friendsReceive[index].friendId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.tab == 3 && $scope.friendsReceive.length > 0) {
                        $scope.friendsReceive.splice(index, 1);
                        $scope.countReceive--;
                        $rootScope.badges.friend--;
                        $scope.countAll++;
                    }
                    $rootScope.showAlert().notify(data.message);
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.rejectFriend = function (index) {
        var routeAction;
        switch ($rootScope.session.view_typeEntity) {
            case 1:
                routeAction = '/friend/rejectfriend';
                break;
            case 2:
                routeAction = '/company/rejectfollower';
                break;
            case 3:
                routeAction = '/group/rejectmember';
                break;
        }
        mainService.ajax(routeAction,
            { 'id': $scope.friendsReceive[index].friendId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.tab == 3 && $scope.friendsReceive.length > 0) {
                        $scope.friendsReceive.splice(index, 1);
                        $scope.countReceive--;
                        $rootScope.badges.friend--;
                    }
                    $rootScope.showAlert().notify(data.message);
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.cancelRequest = function (index) {
        mainService.ajax('/friend/removefriend',
            { 'id': $scope.friendsSend[index].friendId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.tab == 2 && $scope.friendsSend.length > 0) {
                        $scope.friendsSend.splice(index, 1);
                        $scope.countSend--;
                    }
                    $rootScope.showAlert().notify(data.message);
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    // Find persons

    $scope.remoteUrlRequestFn = function (str) {
        return { searchstr: str };
    };

    $scope.itemRender = function (selected) {
        if (selected) $scope.friendSelected = selected.originalObject;
    };
}

function refreshBindPreviewMultimedia() {
    componentHandler.upgradeAllRegistered();
}