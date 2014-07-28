
if (fanex.liveCasinoSetting) {
    fanex.liveCasinoSetting.controller('PtAndComSettingController',
    [
        '$scope',
        '$window',
        'PtAndComSettingService',
        function ($scope, $window, ptAndComSettingService) {
            //// Privates
            var me = this;
            me.$presetOnMemberModal = $('#presetOnMemberModal');
            me.isEditPresetOnMember = function (ptAndComSetting) {
                var currentPt = ptAndComSetting.PositionTaking;
                switch (custSetting.SettingContext.TargetRoleId) {
                    case fanex.liveCasinoSetting.custRole.Master:
                        if (ptAndComSettingService.ptAndComSetting.PositionTaking.Is3
                            && !currentPt.Is3) {
                            currentPt.NewPresetOnMember = currentPt.S3;
                            $scope.presetOnMemberOptions = fanex.createOptionsFromRange(currentPt.S3, 0, currentPt.S3, fanex.liveCasinoSetting.PT_INTERVAL);
                            me.$presetOnMemberModal.modal({
                                backdrop: 'static',
                                show: true,
                                keyboard: false
                            });
                            return true;
                        }
                        break;
                    case fanex.liveCasinoSetting.custRole.Agent:
                        if (ptAndComSettingService.ptAndComSetting.PositionTaking.Im2
                            && !currentPt.Im2) {
                            currentPt.NewPresetOnMember = currentPt.M2;
                            $scope.presetOnMemberOptions = fanex.createOptionsFromRange(currentPt.M2, 0, currentPt.M2, fanex.liveCasinoSetting.PT_INTERVAL);
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
            }

            //// Properties
            $scope.ptAndComSetting = fanex.cloneObject(ptAndComSettingService.ptAndComSetting);
            $scope.presetOnMemberOptions = [];

            //// Action
            $scope.updatePtAndComSetting = function () {
                if (me.isEditPresetOnMember($scope.ptAndComSetting)) {
                    return;
                }

                ptAndComSettingService.updatePtAndComSetting($scope.ptAndComSetting);
            };

            $scope.updatePresetOnMember = function (isUpdate) {
                $scope.ptAndComSetting.PositionTaking.UpdateAllMembers = isUpdate;
                ptAndComSettingService.updatePtAndComSetting($scope.ptAndComSetting);
            };


        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}