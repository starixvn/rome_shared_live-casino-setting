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

describe('editMasterPtAndCom', function () {
    var $compile, $scope, $templateCache;

    // Load the myApp module, which contains the directive
    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function (_$compile_, _$rootScope_, _$templateCache_) {
        $compile = _$compile_;
        $scope = _$rootScope_;
        $templateCache = _$templateCache_;
        $templateCache.put('/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/edit-master.htm', '<template-goes-here />');
    }));

    beforeEach(function () {
        custSetting.PtAndComSetting.PositionTaking = {
            NewPresetOnMember: 0.0,
            UpdateAllMembers: false,
            S4: 0.9900,
            S3: 0.2000,
            M3: 0.7400,
            Us3: 0.1300,
            Is3: true,
            N3: 0.20
        };
        var element = $compile("<edit-master-pt-and-com></edit-master-pt-and-com>")($scope);
        $scope.$digest();
    });

    it('should change range of master max when super preset change', function () {
        //// S3 changed and M3 > S4-S3
        $scope.ptAndComSetting.PositionTaking.S3 = 0.3;
        $scope.ptAndComSetting.PositionTaking.Is3 = false;
        $scope.updateSuperPreset();

        //// Then change range of M3,  change M3=S4-S3 & change range of M3, M3 = [N3; S4-S3]
        expect($scope.ptAndComSetting.PositionTaking.M3).toEqual(0.69);
        expect($scope.masterMaxOptions.length).toEqual(50);
        expect($scope.masterMaxOptions[$scope.masterMaxOptions.length - 1].value).toEqual(0.20);
        expect($scope.masterMaxOptions[0].value).toEqual(0.69);
    });

    it('should show range of super auto preset', function () {
        //// Us3 changed
        $scope.ptAndComSetting.PositionTaking.Is3 = true;

        //// Us3 = [0: S4]
        expect($scope.superAutoPresetOptions.length).toEqual(100);
        expect($scope.superAutoPresetOptions[$scope.superAutoPresetOptions.length - 1].value).toEqual(0);
        expect($scope.superAutoPresetOptions[0].value).toEqual(0.99);
    });

    it('should change range of master min when master max change', function () {
        ////M3 changed
        $scope.ptAndComSetting.PositionTaking.M3 = 0.2;
        $scope.ptAndComSetting.PositionTaking.Is3 = false;
        $scope.updateMasterMax();

        //// Then change range of N3,  , change N3=M3 & change range of N3
        expect($scope.ptAndComSetting.PositionTaking.N3).toEqual(0.2);
        expect($scope.masterMinOptions.length).toEqual(21);
        expect($scope.masterMinOptions[$scope.masterMinOptions.length - 1].value).toEqual(0);
        expect($scope.masterMinOptions[0].value).toEqual(0.2);
    });

    it('should change range of master max when master min change', function () {
        ////N3 changed
        $scope.ptAndComSetting.PositionTaking.N3 = 0.4;
        $scope.ptAndComSetting.PositionTaking.Is3 = false;
        $scope.updateMasterMin();

        //// Then change range of M3
        expect($scope.masterMaxOptions.length).toEqual(40);
        expect($scope.masterMaxOptions[$scope.masterMaxOptions.length - 1].value).toEqual(0.4);
        expect($scope.masterMaxOptions[0].value).toEqual(0.79);
    });
});

describe('editAgentPtAndCom', function () {
    var $compile, $scope, $templateCache;

    // Load the myApp module, which contains the directive
    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function (_$compile_, _$rootScope_, _$templateCache_) {
        $compile = _$compile_;
        $scope = _$rootScope_;
        $templateCache = _$templateCache_;
        $templateCache.put('/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/edit-agent.htm', '<template-goes-here />');
    }));

    beforeEach(function () {
        custSetting.PtAndComSetting.PositionTaking = {
            IsDisableAuto: false,
            NewPresetOnMember: 0.0,
            UpdateAllMembers: false,
            M3: 0.7400,
            M2: 0.2700,
            A2: 0.3300,
            Um2: 0.0000,
            Im2: true,
            N2: 0.2000
        };
        var element = $compile("<edit-agent-pt-and-com></edit-agent-pt-and-com>")($scope);
        $scope.$digest();
    });

    it('should change range of agent max when master preset change', function () {
        //// M2 changed and A2 > M3-M2
        $scope.ptAndComSetting.PositionTaking.M2 = 0.5;
        $scope.ptAndComSetting.PositionTaking.Im2 = false;
        $scope.updateMasterPreset();

        //// Then change range of A2,  change A2=M3-M2 & change range of A2, A2 = [N2; M3-M2]
        expect($scope.ptAndComSetting.PositionTaking.A2).toEqual(0.24);
        expect($scope.agentMaxOptions.length).toEqual(5);
        expect($scope.agentMaxOptions[$scope.agentMaxOptions.length - 1].value).toEqual(0.2);
        expect($scope.agentMaxOptions[0].value).toEqual(0.24);
    });

    it('should show range of master auto preset', function () {
        //// Um2 = [0: M3]
        expect($scope.masterAutoPresetOptions.length).toEqual(75);
        expect($scope.masterAutoPresetOptions[$scope.masterAutoPresetOptions.length - 1].value).toEqual(0);
        expect($scope.masterAutoPresetOptions[0].value).toEqual(0.74);
    });

    it('should change range of agent min when agent max change', function () {
        ////A2 changed
        $scope.ptAndComSetting.PositionTaking.A2 = 0.3;
        $scope.ptAndComSetting.PositionTaking.Im2 = false;
        $scope.updateAgentMax();

        //// Then change range of N3,  , change N3=M3 & change range of N3
        expect($scope.ptAndComSetting.PositionTaking.N2).toEqual(0.2);
        expect($scope.agentMinOptions.length).toEqual(31);
        expect($scope.agentMinOptions[$scope.agentMinOptions.length - 1].value).toEqual(0);
        expect($scope.agentMinOptions[0].value).toEqual(0.3);
    });

    it('should change range of agent max when agent min change', function () {
        ////N3 changed
        $scope.ptAndComSetting.PositionTaking.N2 = 0.22;
        $scope.ptAndComSetting.PositionTaking.Im2 = false;
        $scope.updateAgentMin();

        //// Then change range of M3
        expect($scope.agentMaxOptions.length).toEqual(26);
        expect($scope.agentMaxOptions[$scope.agentMaxOptions.length - 1].value).toEqual(0.22);
        expect($scope.agentMaxOptions[0].value).toEqual(0.47);
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
            S1Min: 0.1,
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
        expect($scope.superPTOptions.length).toEqual(16);
        expect($scope.superPTOptions[0].value).toEqual(0.25);
        expect($scope.superPTOptions[$scope.superPTOptions.length - 1].value).toEqual(0.1);
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

describe('agentEditMemberPtAndCom', function () {
    var $compile, $scope, $templateCache;

    // Load the myApp module, which contains the directive
    beforeEach(module('fanex.liveCasinoSetting'));

    beforeEach(inject(function (_$compile_, _$rootScope_, _$templateCache_) {
        $compile = _$compile_;
        $scope = _$rootScope_;
        $templateCache = _$templateCache_;
        $templateCache.put('/Areas/LiveCasinoSetting/Views/EditSetting/Scripts/templates/PtAndCom/agent-edit-member.htm', '<template-goes-here />');
    }));

    beforeEach(function () {
        custSetting.PtAndComSetting.PositionTaking = {
            A2: 0.3300,
            A1: 0.3200,
            N2: 0.2500
        };

        custSetting.PtAndComSetting.Commission = {
            "Value": 0.006,
            "Upline": {
                "Value": 0.0065
            }
        };

        var element = $compile("<agent-edit-member-pt-and-com></agent-edit-member-pt-and-com>")($scope);
        $scope.$digest();
    });

    it('should assign right range', function () {
        //// Then agent on member range in [0.35:0.33]
        expect($scope.agentPresetOptions.length).toEqual(9);
        expect($scope.agentPresetOptions[0].value).toEqual(0.33);
        expect($scope.agentPresetOptions[$scope.agentPresetOptions.length - 1].value).toEqual(0.25);
        //// Then member commission in []
        expect($scope.memberCommissionOptions.length).toEqual(14);
        expect($scope.memberCommissionOptions[0].value).toEqual(0.0065);
    });
});