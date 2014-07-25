
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('PtAndComSettingController',
    [
        '$scope',
        'PtAndComSettingService',
        function ($scope, ptAndComSettingService) {
            $scope.ptAndComSetting = fanex.cloneObject(ptAndComSettingService.ptAndComSetting);

            $scope.updatePtAndComSetting = function () {
                ptAndComSettingService.updatePtAndComSetting($scope.ptAndComSetting);
            };
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}