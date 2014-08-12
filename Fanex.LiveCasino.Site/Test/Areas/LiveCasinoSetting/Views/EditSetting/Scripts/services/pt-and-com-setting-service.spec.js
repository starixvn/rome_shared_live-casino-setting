/// <reference path="../../../../../../custSetting.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/jquery-2.1.1.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular-sanitize.js" />
/// <reference path="../../../../../../angular-mocks.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Scripts/core/fanex-live-casino-setting.js" />
/// <reference path="../../../../Scripts/core/fanex-live-casino-setting.spec.js" />
/// <reference path="../../../../../../../Areas/LiveCasinoSetting/Views/EditSetting/Scripts/services/pt-and-com-setting-service.js" />
/*global describe, beforeEach, inject, afterEach, it, expect, spyOn*/
describe('pt-and-com-setting-service', function () {
    'use strict';
    var httpBackend, ptAndComSettingService, timeOut;

    beforeEach(module("fanex.liveCasinoSetting"));

    beforeEach(inject(function ($httpBackend, $timeout, _PtAndComSettingService_) {
        ptAndComSettingService = _PtAndComSettingService_;
        httpBackend = $httpBackend;
        timeOut = $timeout;
    }));

    // make sure no expectations were missed in your tests.
    // (e.g. expectGET or expectPOST)
    afterEach(function () {
        httpBackend.verifyNoOutstandingExpectation();
        httpBackend.verifyNoOutstandingRequest();
    });

    it('should send the msg and return the response.', function () {
        // set up some data for the http call to return and test later.
        var result = {
            CommandResultsList: [],
            IsSuccessful: false
        },
            urlRequest = 'request/url',
            ptAndComSetting = {},
            custSettingScope = {
                afterServerReturnSuccess: function () { }
            };
        spyOn(custSettingScope, 'afterServerReturnSuccess');

        // expectGET to make sure this is called once.
        httpBackend.expect('POST', urlRequest).respond(200, result);

        // make the call.
        ptAndComSettingService.updatePtAndComSetting(urlRequest, ptAndComSetting, custSettingScope);

        // flush the backend to "execute" the request to do the expectedGET assertion.
        httpBackend.flush();

        expect(custSettingScope.afterServerReturnSuccess).toHaveBeenCalled();
    });

    it('should send the msg and return the response, show result message and then hide', function () {
        // set up some data for the http call to return and test later.
        var result = {
            CommandResultsList: [
                {
                    ErrCode: 0,
                    Message: 'success'
                }
            ],
            IsSuccessful: true
        },
            urlRequest = 'request/url',
            ptAndComSetting = {},
            custSettingScope = {
                afterServerReturnSuccess: function () { }
            };
            spyOn(custSettingScope, 'afterServerReturnSuccess');

        // expectGET to make sure this is called once.
        httpBackend.expect('POST', urlRequest).respond(200, result);

        // make the call.
        ptAndComSettingService.updatePtAndComSetting(urlRequest, ptAndComSetting, custSettingScope);

        // flush the backend to "execute" the request to do the expectedGET assertion.
        httpBackend.flush();

        expect(custSettingScope.afterServerReturnSuccess).toHaveBeenCalled();
    });

    it('should send msg and return the error response, show result message', function () {
        // set up some data for the http call to return and test later.
        var urlRequest = 'request/url',
            ptAndComSetting = {},
            custSettingScope = {
                afterServerReturnSuccess: function () { }
            },
            errorMessage = 'error message';
        spyOn(custSettingScope, 'afterServerReturnSuccess');

        // expectGET to make sure this is called once.
        httpBackend.expect('POST', urlRequest).respond(403, errorMessage);

        // make the call.
        ptAndComSettingService.updatePtAndComSetting(urlRequest, ptAndComSetting, custSettingScope);

        // flush the backend to "execute" the request to do the expectedGET assertion.
        httpBackend.flush();

        expect(fanex.liveCasinoSetting.errorResult.CommandResultsList[0].Results[0].Message).toEqual(errorMessage);
    });


});