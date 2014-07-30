/* File Created: July 21, 2014 */
fanex.liveCasinoSetting.controller('CustomerInfoController',
    [
        '$scope',
        '$http',
        function ($scope, $http) {
            $scope.custInfo = fanex.cloneObject(custSetting.CustInfo);
            $scope.enableLiveCasino = !custSetting.CustInfo.DisableLiveCasino;
            $scope.disableCheckBoxLiveCasino = custSetting.CustInfo.UplineDisableLiveCasino;

            //// Functions
            $scope.getCustInfo = function () {
                $scope.custInfo.DisableLiveCasino = !$scope.enableLiveCasino
                return $scope.custInfo;
            };
        }
        ]);
