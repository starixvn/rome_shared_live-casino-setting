/* File Created: July 21, 2014 */
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustGameSettingController',
    [
        '$scope',
        '$http',
        'CustomerGameSettingService',
        function ($scope, $http, customerGameSettingService) {
            $scope.gameSetting = fanex.cloneObject(custSetting.GameSetting);

            $scope.updateGameSetting = function () {
                customerGameSettingService.updateGameSetting($scope.gameSetting);
            }
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}