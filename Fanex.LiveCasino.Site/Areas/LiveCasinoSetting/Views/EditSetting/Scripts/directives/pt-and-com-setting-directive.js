/* File Created: July 23, 2014 */

if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.directive('editSuperPtAndCom', [
        function (settingService) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PtAndComSettingController',
                templateUrl: '/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/edit-super.htm',
                link: function ($scope, $element) {
                    var currentPt = $scope.ptAndComSetting.PositionTaking,
                        currentCom = $scope.ptAndComSetting.Commission;
                    $scope.superMaxOptions = fanex.createOptionsFromRange(currentPt.S4, 0, currentPt.L4, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.superMinOptions = fanex.createOptionsFromRange(currentPt.N4, 0, currentPt.S4, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.superCommissionOptions = fanex.createOptionsFromRange(currentCom.Value, 0, currentCom.Upline.Value, fanex.liveCasinoSetting.COM_INTERVAL, fanex.liveCasinoSetting.COM_DECIMAL);
                    $scope.updateSuperMax = function () {
                        if (currentPt.N4 > currentPt.S4) {
                            currentPt.N4 = currentPt.S4;
                        }
                        $scope.superMinOptions = fanex.createOptionsFromRange(currentPt.N4, 0, currentPt.S4, fanex.liveCasinoSetting.PT_INTERVAL);
                    };
                }
            };
        }
    ]);

        //// Edit master
        fanex.liveCasinoSetting.directive('editMasterPtAndCom', [
        function (settingService) {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PtAndComSettingController',
                templateUrl: '/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/edit-master.htm',
                link: function ($scope, $element) {
                    var currentPt = $scope.ptAndComSetting.PositionTaking,
                        currentCom = $scope.ptAndComSetting.Commission;
                    var actions = {
                        computeMinM3: function () {
                            if (currentPt.Im3) {
                                return fanex.roundNumber(Math.max(0, currentPt.N4 - currentPt.Us3), fanex.liveCasinoSetting.PT_DECIMAL);
                            }
                            else {
                                return fanex.roundNumber(Math.max(0, currentPt.N4 - currentPt.S3), fanex.liveCasinoSetting.PT_DECIMAL);
                            }
                        },
                        computeMaxM3: function () {
                            if (currentPt.Im3) {
                                return currentPt.S4;
                            }
                            else {
                                return fanex.roundNumber(currentPt.S4 - currentPt.S3);
                            }
                        }
                    };

                    $scope.superPresetOptions = fanex.createOptionsFromRange(currentPt.S3, 0, currentPt.S4, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.superAutoPresetOptions = fanex.createOptionsFromRange(currentPt.Us3, 0, currentPt.S4, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.masterMinOptions = fanex.createOptionsFromRange(currentPt.N3, 0, currentPt.M3, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.masterMaxOptions = fanex.createOptionsFromRange(currentPt.Us3, actions.computeMinM3(), actions.computeMaxM3(), fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.masterCommissionOptions = fanex.createOptionsFromRange(currentCom.Value, 0, currentCom.Upline.Value, fanex.liveCasinoSetting.COM_INTERVAL, fanex.liveCasinoSetting.COM_DECIMAL);
                    //// Onchange functions
                    $scope.updateSuperPreset = function () {
                    };

                    $scope.updateSuperAutoPreset = function () {
                    };

                    $scope.updateMasterMax = function () {
                    };
                }
            };
        }
    ]);
}
else {
    alert('Missing fanex.liveCasinoSetting in directive declare!');
}