
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

            $scope.isEditPresetOnMember = function () {
                var currentPt = $scope.ptAndComSetting.PositionTaking;
                switch (custSetting.SettingContext.TargetRoleId) {
                    case fanex.liveCasinoSetting.custRole.Master:
                        if (custSetting.PtAndComSetting.PositionTaking.Is3
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
                        if (custSetting.PtAndComSetting.PositionTaking.Im2
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
            $scope.ptAndComSetting = fanex.cloneObject(custSetting.PtAndComSetting);
            $scope.presetOnMemberOptions = [];
            $scope.isUpdateAllSetting = false;
            $scope.language = language;
            $scope.isReadOnly = custSetting.SettingContext.FormType === fanex.liveCasinoSetting.formType.ReadOnly;

            //// Action
            $scope.updatePtAndComSetting = function () {
                if ($scope.isEditPresetOnMember()) {
                    return;
                }

                ptAndComSettingService.updatePtAndComSetting($scope.ptAndComSetting, $scope.$parent);
            };

            $scope.updatePresetOnMember = function (isUpdate) {
                $scope.ptAndComSetting.PositionTaking.UpdateAllMembers = isUpdate;
                if ($scope.$parent.isUpdateAllSetting) {
                    $scope.$parent.updateAllSetting();
                }
                else {
                    ptAndComSettingService.updatePtAndComSetting($scope.ptAndComSetting, $scope.$parent);
                }
            };
        }
        ]);
}
else {
    alert('Missing fanex.liveCasinoSetting!')
}