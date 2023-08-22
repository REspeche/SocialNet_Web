mainApp.controller("wallCtrl", wallCtrl);
wallCtrl.$inject = ["$rootScope", "$scope", "$timeout", "Upload", "mainService"];

mainApp.directive('previewYoutube', previewYoutube);
previewYoutube.$inject = ["$sce"];

function previewYoutube($sce) {
    return {
        restrict: 'EA',
        scope: { code:'=' },
        replace: true,
        template: '<div style="height:187px;"><iframe style="overflow:hidden;height:100%;width:100%" width="100%" height="100%" src="{{url}}" frameborder="0"></iframe></div>',
        link: function (scope) {
            scope.$watch('code', function (newVal) {
                if (newVal) {
                    scope.url = $sce.trustAsResourceUrl("http://www.youtube.com/embed/" + newVal + "?rel=0&autoplay=0&autohide=1&wmode=opaque&showinfo=0&theme=light");
                }
            });
        }
    };
};

function wallCtrl($rootScope, $scope, $timeout, Upload, mainService) {
    var oriData, oriDataComment;
    $scope._noMoreRecords = false;
    $scope.content = null;
    $scope.posts = [];
    $scope.Math = window.Math;
    $scope.imgWidth = 0;
    $scope.isAnonymous = false;
    $scope.indexEditPost = 0
    $scope.cardCommentShow = [];
    $scope.typePage = 0;

    $scope.data = {
        comment: '',
        id: 0,
        idTarget: '',
        idDd: 0,
        typeEvent: 1,
        visibility: 1,
        locationString: '',
        locationCode: '',
        latitude: '',
        longitude: '',
        externalType: 0,
        externalLink: ''
    };
    $scope.dataComment = angular.copy($scope.data);

    $scope.loadInit = function () {
        $scope.typePage = parseInt(document.getElementById("varTypePage").value);
        oriData = angular.copy($scope.data);
        oriDataComment = angular.copy($scope.dataComment);

        // dialog
        var dialogPost = document.querySelector('#dialogPost');
        if (!dialogPost.showModal) dialogPolyfill.registerDialog(dialogPost);

        dialogPost.addEventListener('close', function () {
            toastr.remove();
            setHash('');
            $scope.txtPostRequired = false;
            $scope.txtMediaRequired = false;
            document.getElementById("txtYoutube").value = "";
            $scope.isAnonymous = false;
            $scope.file = undefined;
            $scope.data = angular.copy(oriData);
            $scope.dataComment = angular.copy(oriDataComment);
            $rootScope.isDialogOpen = false;
            $scope.$applyAsync(function () {
                $scope.indexEditPost = 0;
            });
        });

        dialogPost.querySelector('.cancel').addEventListener('click', function () {
            dialogPost.close();
        });

        // add button
        $('#btnAddItem').on('click', function () {
            setHash('#post');
            $scope.data = angular.copy(oriData);       
            $scope.$applyAsync(function () {
                switch ($scope.typePage) {
                    case 2: $scope.data.typeEvent = 5; break;
                    case 3: $scope.data.typeEvent = 9; break;
                }
            });
            $rootScope.isDialogOpen = true;
            dialogPost.showModal();

        });

        $scope.getMorePost();
    };

    $scope.changeTypeEvent = function (id) {
        $scope.data.typeEvent = id;
        $scope.txtPostRequired = false;
        $scope.txtMediaRequired = false;
    }

    $scope.getMorePost = function () {
        lastId = 0;
        if ($scope.posts.length > 0) lastId = $scope.posts[$scope.posts.length - 1].postId;
        var pageService = '';
        switch ($scope.typePage) {
            case 2:
                pageService = '/photo/getphotos';
                break;
            case 3:
                pageService = '/video/getvideos';
                break;
            default:
                pageService = '/wall/getposts';
        }
        if (lastId == 0) $scope._noMoreRecords = false;
        mainService.ajax(pageService,
            {
                'lastId': lastId
            },
            function (data, callBackFunction) {
                if (data.code == 0) {
                    $rootScope.btnAddItem = true;
                    if (data.items.length == 0) {
                        callBackFunction();
                        $scope._noMoreRecords = true;
                        return;
                    }
                    var widthContent = document.getElementById("content").offsetWidth;
                    if ($scope.imgWidth == 0) $scope.imgWidth = ($rootScope.isMobile.any()) ? ((widthContent > 400) ? 400 : widthContent) : 400;
                    $scope.posts.push.apply($scope.posts, data.items);
                    callBackFunction();
                    setTimeout(function () {
                        $scope.$applyAsync(function () {
                            $rootScope.calculateDateTimePost();
                        });
                    }, 1000);
                }
            });
    };

    $scope.getMoreComment = function (index) {
        firstId = 0;
        if ($scope.posts.length > 0 && $scope.posts[index].lastComments.length > 0) firstId = $scope.posts[index].lastComments[0].postId;
        mainService.ajax('/wall/getcomments',
            { 'firstId': firstId, 'postId': $scope.posts[index].postId },
            function (data) {
                if (data.code == 0) {
                    if (data.items.length == 0)  return;
                    $scope.posts[index].lastComments.unshift.apply($scope.posts[index].lastComments, data.items);
                    $scope.refreshWall();
                    setTimeout(function () {
                        $scope.$applyAsync(function () {
                            $rootScope.calculateDateTimePost();
                        });
                    }, 1000);
                }
            });
    };

    $scope.showMore = function () {
        $scope.getMorePost();
    };
    $scope.submitPost = function () {
        switch ($scope.data.typeEvent) {
            case 1:
                if ($scope.data.comment == '') {
                    $scope.txtPostRequired = true;
                    $scope.frmPost.$invalid = true;
                    return false;
                }
                break;
            case 5:
                if ($scope.file == undefined) {
                    $scope.txtMediaRequired = true;
                    $scope.frmPost.$invalid = true;
                    return false;
                }
                break;
            case 9:
                if ($scope.data.externalLink == '') {
                    $scope.txtMediaRequired = true;
                    $scope.frmPost.$invalid = true;
                    return false;
                }
                break;
        }
        if ($scope.indexEditPost>0) {
            mainService.ajax('/wall/writepost',
                $scope.data,
                function (data) {
                    if (data.code == 0) {
                        if (data.items.length > 0) {
                            $scope.posts[$scope.indexEditPost].postText = $scope.data.comment;
                            $scope.refreshWall();
                        }
                        $scope.indexEditPost = 0;
                        if ($rootScope.isDialogOpen) dialogPost.close();
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        }
        else {
            mainService.fileUpload('/wall/writepostupload',
                $scope.file,
                $scope.data,
                function (data) {
                    if (data.code == 0) {
                        if (data.items.length > 0) {
                            $scope.posts.unshift.apply($scope.posts, data.items);
                            $scope.refreshWall();
                            $timeout(function () {
                                $rootScope.calculateDateTimePost();
                                refreshBindPreviewMultimedia();
                            }, 1000);
                        }
                        if ($rootScope.isDialogOpen) dialogPost.close();
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        }
    };
    $scope.submitComment = function (index) {
        var postId = $scope.posts[index].postId;
        var commentObj = document.getElementById("txtComment-" + postId);
        var commentVal = commentObj.value;
        commentObj.value = "";
        if (!commentVal == '') {
            // Set other parameters
            $scope.dataComment.comment = commentVal;
            $scope.dataComment.idTarget = postId;
            $scope.dataComment.idDd = $scope.posts[index].userId;
            mainService.ajax('/wall/writecomment',
                $scope.dataComment,
                function (data) {
                    if (data.code == 0) {
                        if (data.items.length > 0) {
                            $scope.dataComment = angular.copy(oriDataComment);
                            $scope.posts[index].lastComments.push.apply($scope.posts[index].lastComments, data.items);
                            $scope.posts[index].countComments++;
                            $scope.refreshWall();
                            $timeout(function () {
                                $rootScope.calculateDateTimePost();
                            }, 1000);
                        }
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        }
    };
    $scope.editPost = function (index) {
        $scope.indexEditPost = index;
        $rootScope.isDialogOpen = true;
        dialogPost.showModal();
        mainService.ajax('/wall/getpost',
            { 'postId': $scope.posts[index].postId },
            function (data) {
                if (data.code == 0) {
                    if (data.items.length == 1) {
                        document.getElementById("txtPost").focus();
                        $scope.data.id = data.items[0].postId;
                        $scope.data.typeEvent = 1;
                        $scope.data.comment = data.items[0].postText;
                        return;
                    }
                }
            });
    };
    $scope.removePost = function (index) {
        $rootScope.confirmMessage("remove", function () {
            mainService.ajax('/wall/removepost',
                { 'id': $scope.posts[index].postId },
                function (data) {
                    if (data.code == 0) {
                        if ($scope.posts.length > 0) $scope.posts.splice(index, 1);
                    }
                    else {
                        $rootScope.showAlert().notifyError(data.message);
                    }
                });
        });
    };
    $scope.removeComment = function (indexParent, index) {
        mainService.ajax('/wall/removecomment',
            { 'id': $scope.posts[indexParent].lastComments[index].postId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.posts.length > 0 && $scope.posts[indexParent].lastComments.length > 0) {
                        $scope.posts[indexParent].lastComments.splice(index, 1);
                        $scope.posts[indexParent].countComments--;
                        if ($scope.posts[indexParent].lastComments.length == 0 && $scope.posts[indexParent].countComments > 0)
                            $scope.getMoreComment(indexParent);
                        else $scope.refreshWall();
                    }
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.likePost = function (index) {
        if ($rootScope.session.typeEntity != 1) return false;
        mainService.ajax('/wall/likepost',
            { 'id': $scope.posts[index].postId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.posts.length > 0) {
                        if ($scope.posts[index].isLike) {
                            $scope.posts[index].isLike = false;
                            $scope.posts[index].countLikes--;
                        }
                        else {
                            $scope.posts[index].isLike = true;
                            $scope.posts[index].countLikes++;
                        }
                    }
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.likeComment = function (indexParent, index) {
        if ($rootScope.session.typeEntity != 1) return false;
        mainService.ajax('/wall/likepost',
            { 'id': $scope.posts[indexParent].lastComments[index].postId },
            function (data) {
                if (data.code == 0) {
                    if ($scope.posts.length > 0 && $scope.posts[indexParent].lastComments.length > 0) {
                        if ($scope.posts[indexParent].lastComments[index].isLike) {
                            $scope.posts[indexParent].lastComments[index].isLike = false;
                            $scope.posts[indexParent].lastComments[index].countLikes--;
                        }
                        else {
                            $scope.posts[indexParent].lastComments[index].isLike = true;
                            $scope.posts[indexParent].lastComments[index].countLikes++;
                        }
                    }
                }
                else {
                    $rootScope.showAlert().notifyError(data.message);
                }
            });
    };
    $scope.verifyUrl = function ($event) {
        var target = $event.target;
        try {
            var urlValid = youtube_parser(target.value);
            if (urlValid) {
                $scope.data.externalType = 2;
                $scope.data.externalLink = urlValid;
            }
        }
        catch (err) {
            $scope.data.externalLink = "";
        }
    };
    $scope.showComment = function (index) {
        var postId = $scope.posts[index].postId;
        if ($scope.posts[index].countComments > 0 && $scope.posts[index].lastComments.length == 0) $scope.getMoreComment(index);
        $scope.cardCommentShow[postId] = !$scope.cardCommentShow[postId];
        $scope.refreshWall();
    };
    $scope.refreshWall = function () {
        $rootScope.$broadcast('masonry.reload');
    }
}

function refreshBindPreviewMultimedia() {
    componentHandler.upgradeAllRegistered();
    $('.card__image').magnificPopup({
        type: 'image',
        tLoading: document.getElementById("lblPopupLoading").value,
        tClose: document.getElementById("lblPopupClose").value,
        image: {
            markup: '<div class="mfp-figure">' +
                        '<div class="mfp-close"></div>' +
                        '<div class="mfp-img"></div>' +
                        '<div class="mfp-bottom-bar">' +
                          '<div class="mfp-title"></div>' +
                          '<div class="mfp-counter"></div>' +
                        '</div>' +
                      '</div>',
            cursor: 'mfp-zoom-out-cur',
            titleSrc: function(item) {
                return item.el.attr('data-text') + '<small>por ' + item.el.attr('data-author') + '</small>';
            },
            verticalFit: true
        }
    }).removeClass("isPreviewOnPopup");
    $('.card__video > .isPreviewOnPopup').magnificPopup({
        type: 'iframe',
        tLoading: document.getElementById("lblPopupLoading").value,
        tClose: document.getElementById("lblPopupClose").value,
        iframe: {
            markup: '<div class="mfp-iframe-scaler">' +
                      '<div class="mfp-close"></div>' +
                      '<iframe class="mfp-iframe" frameborder="0" allowfullscreen></iframe>' +
                    '</div>',
            patterns: {
                youtube: {
                    index: 'youtube.com/',
                    id: 'v=',
                    src: '//www.youtube.com/embed/%id%?rel=0&autoplay=1&autohide=1&wmode=opaque&showinfo=0&theme=light'
                }
            }
        },
        disableOn: function () {
            if ($(window).width() < 600) {
                return false;
            }
            return true;
        }
    }).removeClass("isPreviewOnPopup");
}