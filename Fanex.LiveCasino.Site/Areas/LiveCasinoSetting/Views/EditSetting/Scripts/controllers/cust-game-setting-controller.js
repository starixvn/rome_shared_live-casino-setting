/* File Created: July 21, 2014 */
/*global custSetting*/
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustGameSettingController',
    [
        '$scope',
        '$http',
        'CustomerGameSettingService',
        function ($scope, $http, customerGameSettingService) {
            'use strict';
            $scope.gameSetting = fanex.cloneObject(custSetting.GameSetting);
            $scope.isReadOnly = custSetting.SettingContext.FormType === fanex.liveCasinoSetting.formType.ReadOnly;
            $scope.updateGameSetting = function () {
                if (custSetting.SettingContext.TargetRoleId === fanex.liveCasinoSetting.custRole.Member) {
                    for (var i = 0; i < $scope.gameSetting.length; i++) {
                        var gameType = $scope.gameSetting[i];
                        for (var j = 0; j < gameType.BettingLimit.length; j++) {
                            var bettingLimit = gameType.BettingLimit[j];
                            bettingLimit.IsChecked = bettingLimit.Id === gameType.CheckedStakeLevelId;
                        }
                    }
                }

                customerGameSettingService.updateGameSetting($scope.gameSetting, $scope.$parent);
            };
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!');
}