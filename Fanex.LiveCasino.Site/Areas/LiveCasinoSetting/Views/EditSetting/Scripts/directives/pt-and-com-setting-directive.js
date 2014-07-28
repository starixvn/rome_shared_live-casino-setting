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
                            return currentPt.N3;
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
                    // PT
                    $scope.superPresetOptions = fanex.createOptionsFromRange(currentPt.S3, 0, currentPt.S4, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.superAutoPresetOptions = fanex.createOptionsFromRange(currentPt.Us3, 0, currentPt.S4, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.masterMinOptions = fanex.createOptionsFromRange(currentPt.N3, 0, currentPt.M3, fanex.liveCasinoSetting.PT_INTERVAL);
                    $scope.masterMaxOptions = fanex.createOptionsFromRange(currentPt.M3, currentPt.N3, actions.computeMaxM3(), fanex.liveCasinoSetting.PT_INTERVAL);
                    // Com
                    $scope.masterCommissionOptions = fanex.createOptionsFromRange(currentCom.Value, 0, currentCom.Upline.Value, fanex.liveCasinoSetting.COM_INTERVAL, fanex.liveCasinoSetting.COM_DECIMAL);
                    //// Onchange functions
                    $scope.updateSuperPreset = function () {
                        var maxOfMasterMax = actions.computeMaxM3();
                        if (currentPt.M3 > maxOfMasterMax) {
                            currentPt.M3 = maxOfMasterMax;
                        }

                        if (currentPt.M3 < currentPt.N3)
                        {
                            currentPt.M3 = currentPt.N3;
                        }


                        $scope.masterMaxOptions = fanex.createOptionsFromRange(currentPt.M3, currentPt.N3, maxOfMasterMax, fanex.liveCasinoSetting.PT_INTERVAL);
                        $scope.updateMasterMax();
                    };

                    $scope.updateMasterMax = function () {
                        if (currentPt.N3 > currentPt.M3) {
                            currentPt.N3 = currentPt.M3;
                        }

                        $scope.masterMinOptions = fanex.createOptionsFromRange(currentPt.N3, 0, currentPt.M3, fanex.liveCasinoSetting.PT_INTERVAL);
                    };

                }
            };
        }
    ]);
}
else {
    alert('Missing fanex.liveCasinoSetting in directive declare!');
}