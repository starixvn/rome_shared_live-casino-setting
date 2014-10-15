/* File Created: July 21, 2014 */
/*global settingContext*/
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.js" />
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        'CustomerSettingService',
        function ($scope, $http, customerSettingService) {
            'use strict';
            $scope.commandResults = [];
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
                    ptAndComScope = angular.element('[data-ng-controller="PtAndComSettingController"]').scope();

                var settingContextForUpdate = {
                    settingContext: settingContext.SettingContext,
                    custInfo: custInfoScope.getCustInfo(),
                    ptAndComSetting: ptAndComScope.ptAndComSetting
                };

                customerSettingService.updateAllSetting(settingContextForUpdate, $scope);
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