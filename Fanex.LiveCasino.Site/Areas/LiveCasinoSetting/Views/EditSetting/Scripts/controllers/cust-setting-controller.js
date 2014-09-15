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
            var updateAllSettingUrl = $('#UpdateLiveCasinoSettingUrl').val();
            $scope.mainResult = {
                CommandResultsList: []
            };
            $scope.isUpdateAllSetting = false;

            $scope.isReadOnly = settingContext.FormType === fanex.liveCasinoSetting.formType.ReadOnly;
            $scope.isEditMultiple = settingContext.Customers && settingContext.Customers.length > 0;
            $scope.isEditDirect = (settingContext.Upline.Level - settingContext.Customer.Level === 1);
            if ($scope.isEditMultiple
                && settingContext.Customers.IsSyncLiveCasino
                && $scope.isEditDirect) {
                custSetting.CustInfo.IsSyncLiveCasino = settingContext.TargetSyncStatus.indexOf('0') === -1;
            }

            $scope.validateAndUpdateSetting = function (updateType) {
                $scope.isUpdateAllSetting = true;
                var ptAndComScope = angular.element('[data-ng-controller="PtAndComSettingController"]').scope();

                if (ptAndComScope.isEditPresetOnMember()) {
                    return;
                }

                $scope.updateAllSetting(updateType);
            };

            $scope.updateAllSetting = function (updateType) {
                updateType = updateType || fanex.liveCasinoSetting.updateType.AllSetting;
                var custInfoScope = angular.element('[data-ng-controller="CustomerInfoController"]').scope(),
                    ptAndComScope = angular.element('[data-ng-controller="PtAndComSettingController"]').scope(),
                    winLimitScope = angular.element('[data-ng-controller="CustomerWinLimitController"]').scope(),
                    gameSettingScope = angular.element('[data-ng-controller="CustGameSettingController"]').scope();

                var customer = custInfoScope.getCustInfo();
                customer.memberWinLimit = winLimitScope.getWinLimit();
                customer.positionTaking = ptAndComScope.positionTaking;
                customer.commission = ptAndComScope.commission;
                customer.gameSetting = gameSettingScope.getGameSetting();

                var custSettingForUpdate = {
                    settingContext: settingContext,
                    customer: customer,
                    isApplyPositionTakingAllMembers: ptAndComScope.uplinePTOnMember.isApplyNewUplinePositionTaking,
                    positionTakingOnMembers: ptAndComScope.uplinePTOnMember.newUplinePositionTaking,
                    updateType: updateType
                };

                customerSettingService.updateAllSetting(updateAllSettingUrl, custSettingForUpdate, $scope);
                $scope.isUpdateAllSetting = false;
            };

            $scope.afterServerReturnSuccess = function (mainResult, isUpdatePartial) {
                $scope.mainResult = mainResult;

                if (mainResult.IsSuccessful) {
                    $scope.isReadOnly = true;
                    $timeout(function () {
                        if ($scope.isEditDirect
                            && $window.parent.actionAfterUpdatedLiveCasinoSettingMultiple) {
                            $window.parent.actionAfterUpdatedLiveCasinoSettingMultiple(isUpdatePartial);
                        }
                        else {
                            if (!isUpdatePartial
                                && $window.parent.actionAfterUpdatedLiveCasinoSettingSingle) {
                                $window.parent.actionAfterUpdatedLiveCasinoSettingSingle(isUpdatePartial);
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