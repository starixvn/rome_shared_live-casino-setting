
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('PtAndComSettingController',
    [
        '$scope',
        function ($scope) {
            $scope.ptAndComSetting = custSetting.PtAndComSetting;

        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}