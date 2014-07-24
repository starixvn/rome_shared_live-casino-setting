/* File Created: July 21, 2014 */
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('CustomerWinLimitController',
    [
        '$scope',
        '$http',
        function ($scope, $http) {
            $scope.custInfo = fanex.cloneObject(custSetting.CustInfo);
            $scope.memberWinLimit = fanex.cloneObject(custSetting.CustInfo.MemberWinLimit);

            $scope.maxWinLimits = [{ lable: "3X", value: 3 }, { lable: "5X", value: 5 }, { lable: "10X", value: 10 }, { lable: "Unlimited", value: 0}];

            switch ($scope.memberWinLimit.Upline.Max) {
                case 3:
                    $scope.maxWinLimits.splice(1, 3);
                    break;
                case 5:
                    $scope.maxWinLimits.splice(2, 2);
                    break;
                case 10:
                    $scope.maxWinLimits.splice(3, 1);
                    break;
            }

            var index = 0;

            switch ($scope.memberWinLimit.Max) {
                case 3:
                    index = 0;
                    break;
                case 5:
                    index = 1;
                    break;
                case 10:
                    index = 2;
                    break;
                default:
                    index = 3;
                    break;

            }

            $scope.maxWinLimitModel = {
                maxWinLimit: $scope.maxWinLimits[index],
                isDailyAutoReset: $scope.memberWinLimit.IsAutoReset
            };
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}