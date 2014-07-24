/* File Created: July 23, 2014 */

if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.directive('editSuperPtAndCom', [ 'CustomerSettingService',
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
                        $scope.whenChange();
                    };

                    $scope.whenChange = function () {
                        var pt = settingService.custSetting.PtAndComSetting.PositionTaking,
                            com = settingService.custSetting.PtAndComSetting.Commission;
                        $scope.isNotAllowUpdate = (pt.S4 === currentPt.S4 
                                                    && pt.N4 === currentPt.N4
                                                    && com.Value === currentCom.Value);
                    }
                }
            };
        }
    ]);

}
else {
    alert('Missing fanex.liveCasinoSetting in directive declare!');
}