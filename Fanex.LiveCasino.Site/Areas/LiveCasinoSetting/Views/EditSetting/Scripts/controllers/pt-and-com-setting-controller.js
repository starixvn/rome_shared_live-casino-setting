/*global custSetting, language*/
fanex.liveCasinoSetting.controller('PtAndComSettingController',
    [
    '$scope',
    '$window',
    'PtAndComSettingService',
    function ($scope, $window, ptAndComSettingService) {
        'use strict';
        //// Privates
        var me = this;
        me.$presetOnMemberModal = $('#presetOnMemberModal');
        $scope.uplinePTOnMember =
        {
            NewUplinePositionTaking: 0,
            IsApplyNewUplinePositionTaking: false
        };

        $scope.isEditPresetOnMember = function () {
            var currentPt = $scope.PositionTaking;
            switch (custSetting.SettingContext.Customer.Level) {
                case fanex.liveCasinoSetting.customerLevel.Master:
                    if (custSetting.PtAndComSetting.PositionTaking.Is3
                        && !currentPt.Is3) {
                        $scope.uplinePTOnMember = fanex.roundNumber(currentPt.S4 - currentPt.M3);
                        $scope.uplinePTOnMemberOptions = fanex.createOptionsFromRange(
                                                            currentPt.NewPresetOnMember,
                                                            0,
                                                            currentPt.NewPresetOnMember,
                                                            fanex.liveCasinoSetting.PT_INTERVAL);
                        me.$presetOnMemberModal.modal({
                            backdrop: 'static',
                            show: true,
                            keyboard: false
                        });
                        return true;
                    }
                    break;
                case fanex.liveCasinoSetting.customerLevel.Agent:
                    if (custSetting.PtAndComSetting.PositionTaking.Im2
                        && !currentPt.Im2) {
                        $scope.uplinePTOnMember = fanex.roundNumber(currentPt.M3 - currentPt.A2);
                        $scope.uplinePTOnMemberOptions = fanex.createOptionsFromRange(
                                                            currentPt.NewPresetOnMember,
                                                            0,
                                                            currentPt.NewPresetOnMember,
                                                            fanex.liveCasinoSetting.PT_INTERVAL);
                        me.$presetOnMemberModal.modal({
                            backdrop: 'static',
                            show: true,
                            keyboard: false
                        });
                        return true;
                    }
                    break;
            }
            return false;
        };

        //// Properties
        $scope.positionTaking = fanex.cloneObject(custSetting.Customer.LiveCasinoSetting.PositionTaking);
        $scope.commission = custSetting.Customer.LiveCasinoSetting.Commission;
        $scope.parentCommission = custSetting.Customer.Parent.LiveCasinoSetting.Commission;
        $scope.uplinePTOnMemberOptions = [];
        $scope.isUpdateAllSetting = false;
        $scope.language = language;
        me.updatePTComUrl = $('#UpdatePtComSettingUrl').val();

        //// Action
        $scope.updatePtAndComSetting = function () {
            if (!custSetting.Customer.IsSyncLiveCasino) {
                $scope.$parent.validateAndUpdateSetting(fanex.liveCasinoSetting.updateType.PositionTakingAndCommission);
                return;
            }

            if ($scope.isEditPresetOnMember()) {
                return;
            }

            ptAndComSettingService.updatePtAndComSetting(me.updatePTComUrl, $scope.commission, $scope.positionTaking, $scope.uplinePTOnMember, $scope.$parent);
        };

        $scope.updatePTOnMember = function (isUpdate) {
            $scope.ptAndComSetting.PositionTaking.UpdateAllMembers = isUpdate;
            if ($scope.$parent.isUpdateAllSetting) {
                $scope.$parent.updateAllSetting();
            }
            else {
                ptAndComSettingService.updatePtAndComSetting(
                            me.updatePTComUrl,
                            $scope.commission,
                            $scope.positionTaking,
                            $scope.uplinePTOnMember,
                            $scope.$parent);
            }
        };
    }
]);