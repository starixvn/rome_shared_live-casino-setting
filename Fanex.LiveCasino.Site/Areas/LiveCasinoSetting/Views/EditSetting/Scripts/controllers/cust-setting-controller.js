/* File Created: July 21, 2014 */
/*global custSetting*/
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.js" />
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        'CustomerSettingService',
        function ($scope, $http, customerSettingService) {
            'use strict';
            $scope.mainResult = {
                CommandResultsList: []
            };
            $scope.isUpdateAllSetting = false;
            //$scope.updateLiveCasinoStatus

            $scope.validateAndUpdateSetting = function () {
                $scope.isUpdateAllSetting = true;
                var ptAndComScope = angular.element('[data-ng-controller="PtAndComSettingController"]').scope();

                if (ptAndComScope.isEditPresetOnMember()) {
                    return;
                }

                $scope.updateAllSetting();
            };

            $scope.updateAllSetting = function () {
                var custInfoScope = angular.element('[data-ng-controller="CustomerInfoController"]').scope(),
                    ptAndComScope = angular.element('[data-ng-controller="PtAndComSettingController"]').scope(),
                    winLimitScope = angular.element('[data-ng-controller="CustomerWinLimitController"]').scope(),
                    gameSettingScope = angular.element('[data-ng-controller="CustGameSettingController"]').scope();

                var custInfo = custInfoScope.getCustInfo();
                custInfo.MemberWinLimit = winLimitScope.getWinLimit();


                var custSettingForUpdate = {
                    settingContext: custSetting.SettingContext,
                    custInfo: custInfo,
                    ptAndComSetting: ptAndComScope.ptAndComSetting,
                    gameSetting: gameSettingScope.gameSetting
                };

                customerSettingService.updateAllSetting(custSettingForUpdate, $scope);
                $scope.isUpdateAllSetting = false;
            };

            //$scope.update
            $('#tblSetting').show();
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!');
}