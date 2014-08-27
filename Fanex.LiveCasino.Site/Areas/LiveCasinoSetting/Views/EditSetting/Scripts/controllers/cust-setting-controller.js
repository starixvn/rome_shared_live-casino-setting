/* File Created: July 21, 2014 */
/*global custSetting*/
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.js" />
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerSettingController',
    [
        '$scope',
        '$http',
        '$window',
        '$timeout',
        'CustomerSettingService',
        function ($scope, $http, $window, $timeout, customerSettingService) {
            'use strict';
            var updateAllSettingUrl = $('#tblSetting').data('update-href');
            $scope.mainResult = {
                CommandResultsList: []
            };
            $scope.isUpdateAllSetting = false;
            $scope.isReadOnly = custSetting.SettingContext.FormType === fanex.liveCasinoSetting.formType.ReadOnly;

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
                    gameSetting: gameSettingScope.getGameSetting()
                };

                customerSettingService.updateAllSetting(updateAllSettingUrl, custSettingForUpdate, $scope);
                $scope.isUpdateAllSetting = false;
            };

            $scope.afterServerReturnSuccess = function (mainResult, isUpdatePatial) {
                $scope.mainResult = mainResult;

                if (mainResult.IsSuccessful) {
                    $scope.isReadOnly = true;
                    $timeout(function () {
                        if ($window.parent.actionAfterUpdatedLiveCasinoSettingMultiple) {
                            $window.parent.actionAfterUpdatedLiveCasinoSettingMultiple(isUpdatePatial);
                        }
                        else {
                            if (!isUpdatePatial
                                && $window.parent.actionAfterUpdatedLiveCasinoSetting) {
                                $window.parent.actionAfterUpdatedLiveCasinoSetting(isUpdatePatial);
                            }

                            $window.location.href = $window.location.href;
                        }
                    }, fanex.liveCasinoSetting.TIME_OUT_SHOW_MESSAGE);
                }
            };

            $('#tblSetting').show();
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!');
}