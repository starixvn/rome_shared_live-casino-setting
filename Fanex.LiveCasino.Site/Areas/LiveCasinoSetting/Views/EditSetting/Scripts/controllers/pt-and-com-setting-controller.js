
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('PtAndComSettingController',
    [
        '$scope',
        'CustomerSettingService',
        function ($scope, settingService) {
            $scope.ptAndComSetting = fanex.cloneObject(settingService.custSetting.PtAndComSetting);
            $scope.isNotAllowUpdate = true;

            $scope.updatePtAndComSetting = function () {
                settingService.updatePtAndComSetting($scope.ptAndComSetting);
            };
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}