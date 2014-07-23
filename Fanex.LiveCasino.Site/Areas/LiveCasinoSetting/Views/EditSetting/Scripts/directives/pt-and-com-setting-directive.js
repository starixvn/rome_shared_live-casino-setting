/* File Created: July 23, 2014 */

if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.directive('editSuperPtAndCom', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                controller: 'PtAndComSettingController',
                templateUrl: '/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/edit-super.htm',
                link: function ($scope, $element) {
                    var currentPt = $scope.ptAndComSetting.PositionTaking;
                    $scope.superMaxOptions = fanex.createOptionsFromRange(0, currentPt.L4, 0.01);
                    $scope.superMinOptions = fanex.createOptionsFromRange(0, currentPt.S4, 0.01);
                    $scope.updateSuperMax = function () {
                        $scope.superMinOptions = fanex.createOptionsFromRange(0, currentPt.S4, 0.01);
                    };
                }
            };
        }
    ]);

}
else {
    alert('Missing fanex.liveCasinoSetting in directive declare!');
}