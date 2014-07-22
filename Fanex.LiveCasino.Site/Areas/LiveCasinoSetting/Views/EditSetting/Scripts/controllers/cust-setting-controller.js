/* File Created: July 21, 2014 */

/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.js" />
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        'CustomerSettingService',
        function ($scope, $http, customerSettingService) {

            //$scope.updateLiveCasinoStatus
            $scope.updateSetting = customerSettingService.updateSetting;
            //$scope.update
            $('#tblSetting').show();
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}