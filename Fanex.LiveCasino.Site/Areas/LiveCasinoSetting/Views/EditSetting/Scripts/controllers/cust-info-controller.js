/* File Created: July 21, 2014 */
/*global custSetting*/
fanex.liveCasinoSetting.controller('CustomerInfoController',
    [
        '$scope',
        function ($scope) {
            'use strict';
            $scope.enableLiveCasino = !custSetting.Customer.DisableLiveCasino;
            $scope.disableCheckBoxLiveCasino = custSetting.Customer.UplineDisableLiveCasino;

            //// Functions
            $scope.getCustInfo = function () {
                custSetting.Customer.DisableLiveCasino = !$scope.enableLiveCasino;
                return custSetting.Customer;
            };
        }
        ]);
