/* File Created: July 21, 2014 */
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustGameSettingController',
    [
        '$scope',
        '$http',
        function ($scope, $http) {
            $scope.gameSettingViewModel = fanex.cloneObject(custSetting.GameSetting);

        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}