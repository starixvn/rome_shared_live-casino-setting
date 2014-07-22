/* File Created: July 21, 2014 */
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerInfoController',
    [
        '$scope',
        '$http',
        function ($scope, $http) {
            $scope.custInfo = fanex.cloneObject(custSetting.CustInfo);
            $scope.enableLiveCasino = !custSetting.CustInfo.DisableLiveCasino;
            $scope.isChanged = false;
            $scope.disableCheckBoxLiveCasino = custSetting.CustInfo.UplineDisableLiveCasino;

            //// Functions
            $scope.updateStatus = function () {
                $scope.isChanged = $scope.enableLiveCasino === custSetting.CustInfo.DisableLiveCasino;
            };
            $scope.getCustInfo = function () {
                if ($scope.isChanged) {
                    return {
                        DisableLiveCasino: !$scope.enableLiveCasino
                    };
                }

                return null;
            };
            //$scope.updateLiveCasinoStatus
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}