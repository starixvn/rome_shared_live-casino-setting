/// <reference path="../../../../../../custSetting.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/jquery-2.1.1.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../../angular-mocks.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/core/fanex-live-casino-setting.js" />
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.spec.js" />

/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/core/live-casino-lang.js" />
/// <reference path="../../../../Scripts/core/live-casino-lang.spec.js" />

/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/controllers/pt-and-com-setting-controller.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/services/pt-and-com-setting-service.js" />
/// <reference path="../controllers/pt-and-com-setting-controller.spec.js" />

/// <reference path="../services/pt-and-com-setting-service.spec.js" />

/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/directives/pt-and-com-setting-directive.js" />


describe('editSuperPtAndCom', function () {
    var $compile, $scope, $templateCache;

    // Load the myApp module, which contains the directive
    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function (_$compile_, _$rootScope_, _$templateCache_) {
        $compile = _$compile_;
        $scope = _$rootScope_;
        $templateCache = _$templateCache_;
        $templateCache.put('/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/edit-super.htm', '<template-goes-here />');
    }));

    beforeEach(function () {
        custSetting.PtAndComSetting.PositionTaking = {
            S4: 0.9900,
            L4: 1.0,
            N4: 0.3900
        };
        var element = $compile("<edit-super-pt-and-com></edit-super-pt-and-com>")($scope);
        $scope.$digest();
    });

    it('should change min range when max change greater than min', function () {
        //// When update S4 to 0.6
        $scope.ptAndComSetting.PositionTaking.S4 = 0.6;
        $scope.updateSuperMax();

        //// Then change range of S3 in [0 : 0.6]
        expect($scope.ptAndComSetting.PositionTaking.N4).toEqual(0.39);
        expect($scope.superMinOptions.length).toEqual(61);
        expect($scope.superMinOptions[0].value).toEqual(0.6);
    });

    it('should change min range and min when updata max lower than min', function () {
        //// When Update S4 to 0.3
        $scope.ptAndComSetting.PositionTaking.S4 = 0.3;
        $scope.updateSuperMax();

        //// Then S3 change to 0.3 and range of S3 in [0 - 0.3]
        expect($scope.ptAndComSetting.PositionTaking.N4).toEqual(0.3);
        expect($scope.superMinOptions.length).toEqual(31);
        expect($scope.superMinOptions[0].value).toEqual(0.3);
    });
});

describe('superEditMemberPt', function () {
    var $compile, $scope, $templateCache;

    // Load the myApp module, which contains the directive
    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function (_$compile_, _$rootScope_, _$templateCache_) {
        $compile = _$compile_;
        $scope = _$rootScope_;
        $templateCache = _$templateCache_;
        $templateCache.put('/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/super-edit-member.htm', '<template-goes-here />');
    }));

    beforeEach(function () {
        custSetting.PtAndComSetting.PositionTaking = {
            S1Min: 0.0000,
            S1Max: 0.2500,
            S4: 0.9900,
            S1: 0.1200,
            M1: 0.1300,
            A1: 0.3200,
            Us1: 0.1200,
            Is3: true,
            Is1: true
        };
        var element = $compile("<super-edit-member-pt></super-edit-member-pt>")($scope);
        $scope.$digest();
    });

    it('should assign right range', function () {
        expect($scope.superPTOptions.length).toEqual(26);
        expect($scope.superPTOptions[0].value).toEqual(0.25);
        expect($scope.superAutoPTOptions.length).toEqual(100);
        expect($scope.superAutoPTOptions[0].value).toEqual(0.99);
    });
});

describe('masterEditMemberPt', function () {
    var $compile, $scope, $templateCache;

    // Load the myApp module, which contains the directive
    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function (_$compile_, _$rootScope_, _$templateCache_) {
        $compile = _$compile_;
        $scope = _$rootScope_;
        $templateCache = _$templateCache_;
        $templateCache.put('/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/master-edit-member.htm', '<template-goes-here />');
    }));

    beforeEach(function () {
        custSetting.PtAndComSetting.PositionTaking = {
            IsDisableAuto: false,
            M1Max: 0.4200,
            M1Min: 0.1,
            M3: 0.7400,
            M1: 0.1300,
            A1: 0.3200,
            Um1: 0.1300,
            Im2: false,
            Im1: true
        };
        var element = $compile("<master-edit-member-pt></master-edit-member-pt>")($scope);
        $scope.$digest();
    });

    it('should assign right range', function () {
        //// Then master on member range in [0.1:0.42], and auto in [0:0.74] 
        expect($scope.masterPTOptions.length).toEqual(33);
        expect($scope.masterPTOptions[0].value).toEqual(0.42);
        expect($scope.masterPTOptions[$scope.masterPTOptions.length - 1].value).toEqual(0.1);
        expect($scope.masterAutoPTOptions.length).toEqual(75);
        expect($scope.masterAutoPTOptions[0].value).toEqual(0.74);
    });
});