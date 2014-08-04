/// <reference path="../../../../../../custSetting.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/jquery-2.1.1.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/bootstrap.js" />

/// <reference path="../../../../../../angular-mocks.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/core/fanex-live-casino-setting.js" />
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.spec.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/controllers/pt-and-com-setting-controller.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/services/pt-and-com-setting-service.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/core/live-casino-lang.js" />
/// <reference path="../../../../Scripts/core/live-casino-lang.spec.js" />
/*global describe, beforeEach, inject, spyOn, it, custSetting, expect*/
describe('pt-and-com-setting-controller', function () {
    'use strict';
    var $scope, ptAndComSettingService;

    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function ($rootScope, $controller) {
        $scope = $rootScope.$new();

        ptAndComSettingService = {
            updatePtAndComSetting: null
        };

        spyOn(ptAndComSettingService, 'updatePtAndComSetting');

        $controller('PtAndComSettingController',
        {
            $scope: $scope,
            PtAndComSettingService: ptAndComSettingService
        });
    }));

    it('should show popup when super change from is auto preset to not', function () {
        //// Arrange
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.custRole.Master;
        custSetting.PtAndComSetting.PositionTaking = {
            Is3: true
        };

        //// Act
        $scope.ptAndComSetting.PositionTaking.Is3 = false;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Assert

        expect(isEditOnMember).toEqual(true);
    });

    it('should not show popup when super change from not auto to take auto', function () {
        //// Arrange
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.custRole.Master;
        custSetting.PtAndComSetting.PositionTaking = {
            Is3: false
        };

        //// Act
        $scope.ptAndComSetting.PositionTaking.Is3 = true;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Assert

        expect(isEditOnMember).toEqual(false);
    });

    it('should show popup when master change from is auto preset to not', function () {
        //// Arrange
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.custRole.Agent;
        custSetting.PtAndComSetting.PositionTaking = {
            Im2: true
        };

        //// Act
        $scope.ptAndComSetting.PositionTaking.Im2 = false;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Assert

        expect(isEditOnMember).toEqual(true);
    });

    it('should not show popup when master change from is not auto to take auto', function () {
        //// Arrange
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.custRole.Agent;
        custSetting.PtAndComSetting.PositionTaking = {
            Im2: false
        };

        //// Act
        $scope.ptAndComSetting.PositionTaking.Im2 = true;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Assert

        expect(isEditOnMember).toEqual(false);
    });

    it('should not show popup when form has no auto', function () {
        //// Arrange
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.custRole.Member;

        //// Act
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Assert
        expect(isEditOnMember).toEqual(false);
    });

    it('should update setting', function () {
        //// Act
        $scope.updatePtAndComSetting();

        //// Assert
        expect(ptAndComSettingService.updatePtAndComSetting).toHaveBeenCalled();
    });

    it('should not update setting and show popup when update setting', function () {
        //// Arrange
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.custRole.Master;
        custSetting.PtAndComSetting.PositionTaking = {
            Is3: true
        };

        //// Act
        $scope.updatePtAndComSetting();

        //// Assert
        expect(ptAndComSettingService.updatePtAndComSetting).not.toHaveBeenCalled();
    });

    it('should update setting after choose preset on member', function () {
        //// Arrange 
        $scope.$parent.isUpdateAllSetting = false;
        var isUpdate = false;

        //// Act
        $scope.updatePresetOnMember(isUpdate);

        //// Assert
        expect(ptAndComSettingService.updatePtAndComSetting).toHaveBeenCalled();
        expect($scope.ptAndComSetting.PositionTaking.UpdateAllMembers).toEqual(isUpdate);
    });

    it('should update all setting after choose preset on member', function () {
        //// Arrange 
        $scope.$parent.isUpdateAllSetting = true;
        var isUpdate = true;
        $scope.$parent.updateAllSetting = function () { return; };
        spyOn($scope.$parent, 'updateAllSetting');

        //// Act
        $scope.updatePresetOnMember(isUpdate);

        //// Assert
        expect(ptAndComSettingService.updatePtAndComSetting).not.toHaveBeenCalled();
        expect($scope.ptAndComSetting.PositionTaking.UpdateAllMembers).toEqual(isUpdate);
    });
});