var mainApp = angular.module('mainApp',
    [
        'wu.masonry',
        'ngFileUpload',
        'angucomplete-alt',
        'ADM-dateTimePicker',
        'ngCookies',
        'ngSanitize'
    ]);

mainApp.directive('whenscroll', whenScroll);
whenScroll.$inject = ["$rootScope"];

mainApp.directive('onFinishRender', onFinishRender);
onFinishRender.$inject = ["$timeout", "$rootScope"];