/// <reference path="../../../../../../custSetting.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/jquery-2.1.1.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular-sanitize.js" />
/// <reference path="../../../../../../angular-mocks.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/core/fanex-live-casino-setting.js" />
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.spec.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/controllers/cust-info-controller.js" />
/*global describe, beforeEach, inject, it, custSetting, expect*/
describe('cust-info-controller', function () {
    'use strict';
    var $scope;

    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function ($rootScope, $controller) {
        $scope = $rootScope.$new();
        $controller('CustomerInfoController', { $scope: $scope });
    }));

    it('should enable live casino', function () {
        //// Arrange
        custSetting.CustInfo.DisableLiveCasino = true;

        //// Assert
        expect($scope.enableLiveCasino).toBe(true);
    });

    it('should be get customer information after changed', function () {
        //// Arrange
        $scope.enableLiveCasino = true;

        //// Act
        var custInfo = $scope.getCustInfo();

        //// Assert
        expect(custInfo).not.toBeNull();
        expect(custInfo.DisableLiveCasino).toEqual(false);
    });
});