/// <reference path="../../../../../../custSetting.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/jquery-2.1.1.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular-sanitize.js" />
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
        //// Given
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.customerLevel.Master;
        custSetting.PtAndComSetting.PositionTaking = {
            Is3: true
        };

        //// When
        $scope.ptAndComSetting.PositionTaking.Is3 = false;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Then

        expect(isEditOnMember).toEqual(true);
    });

    it('should not show popup when super change from not auto to take auto', function () {
        //// Given
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.customerLevel.Master;
        custSetting.PtAndComSetting.PositionTaking = {
            Is3: false
        };

        //// When
        $scope.ptAndComSetting.PositionTaking.Is3 = true;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Then

        expect(isEditOnMember).toEqual(false);
    });

    it('should show popup when master change from is auto preset to not', function () {
        //// Given
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.customerLevel.Agent;
        custSetting.PtAndComSetting.PositionTaking = {
            Im2: true
        };

        //// When
        $scope.ptAndComSetting.PositionTaking.Im2 = false;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Then

        expect(isEditOnMember).toEqual(true);
    });

    it('should not show popup when master change from is not auto to take auto', function () {
        //// Given
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.customerLevel.Agent;
        custSetting.PtAndComSetting.PositionTaking = {
            Im2: false
        };

        //// When
        $scope.ptAndComSetting.PositionTaking.Im2 = true;
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Then

        expect(isEditOnMember).toEqual(false);
    });

    it('should not show popup when form has no auto', function () {
        //// Given
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.customerLevel.Member;

        //// When
        var isEditOnMember = $scope.isEditPresetOnMember();
        //// Then
        expect(isEditOnMember).toEqual(false);
    });

    it('should update setting', function () {
        //// When
        $scope.updatePtAndComSetting();

        //// Then
        expect(ptAndComSettingService.updatePtAndComSetting).toHaveBeenCalled();
    });

    it('should not update setting and show popup when update setting', function () {
        //// Given
        custSetting.SettingContext.TargetRoleId = fanex.liveCasinoSetting.customerLevel.Master;
        custSetting.PtAndComSetting.PositionTaking = {
            Is3: true
        };

        //// When
        $scope.updatePtAndComSetting();

        //// Then
        expect(ptAndComSettingService.updatePtAndComSetting).not.toHaveBeenCalled();
    });

    it('should update setting after choose preset on member', function () {
        //// Given 
        $scope.$parent.isUpdateAllSetting = false;
        var isUpdate = false;

        //// When
        $scope.updatePresetOnMember(isUpdate);

        //// Then
        expect(ptAndComSettingService.updatePtAndComSetting).toHaveBeenCalled();
        expect($scope.ptAndComSetting.PositionTaking.UpdateAllMembers).toEqual(isUpdate);
    });

    it('should update all setting after choose preset on member', function () {
        //// Given 
        $scope.$parent.isUpdateAllSetting = true;
        var isUpdate = true;
        $scope.$parent.updateAllSetting = function () { return; };
        spyOn($scope.$parent, 'updateAllSetting');

        //// When
        $scope.updatePresetOnMember(isUpdate);

        //// Then
        expect(ptAndComSettingService.updatePtAndComSetting).not.toHaveBeenCalled();
        expect($scope.ptAndComSetting.PositionTaking.UpdateAllMembers).toEqual(isUpdate);
    });


    it('should not show popup when form has no auto', function () {
        //// Given
        custSetting.CustInfo.IsSyncLiveCasino = false;
        $scope.$parent.validateAndUpdateSetting = function () { return; };
        spyOn($scope.$parent, 'validateAndUpdateSetting');

        //// When
        $scope.updatePtAndComSetting();

        //// Then
        expect($scope.$parent.validateAndUpdateSetting).toHaveBeenCalled();
    });
});