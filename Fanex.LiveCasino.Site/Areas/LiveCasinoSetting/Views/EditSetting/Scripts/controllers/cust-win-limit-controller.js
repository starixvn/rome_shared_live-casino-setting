/* File Created: July 21, 2014 */
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerWinLimitController',
    [
        '$scope',
        '$http',
        function ($scope, $http) {
            $scope.custInfo = fanex.cloneObject(custSetting.CustInfo);
            $scope.maxWins = [{ value: "3", text: "3X" }, { value: "5", text: "5X" }, { value: "10", text: "10X" }, { value: "0", text: "Unlimited"}];

        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}