/* File Created: July 21, 2014 */
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/libs/angular-sanitize.js" />
/// <reference path="../../../../../Areas/LiveCasinoSetting/Scripts/core/fanex-live-casino-setting.js" />
/*global describe, it, expect*/
describe('Fanex.Core', function () {
    'use strict';
    it('should be clone object.', function () {
        //// Arrange
        var needBeCloneObject = {
            propertyA: 1,
            propertyB: '1',
            subObject: {
                propertyC: 1
            }
        };
        //// Act
        var clonedObject = fanex.cloneObject(needBeCloneObject);

        //// Assert
        expect(clonedObject).toEqual(needBeCloneObject);
    });

    it('should not contains clone function', function () {
        //// Arrange
        var needBeCloneObject = {
            clone: null
        };

        //// Act
        var clonedObject = fanex.cloneObject(needBeCloneObject);

        //// Assert
        expect(clonedObject.clone).toBeUndefined();
    });

    it('should round number with 2 decimals', function () {
        //// Arrange
        var notRoundedNumber = 1.22222;

        //// Act
        var roundedNumber = fanex.roundNumber(notRoundedNumber, 2);

        //// Assert
        expect(roundedNumber).toEqual(1.22);
    });

    it('should create range from 0.1 to 0.2 with 2 decimals', function () {
        //// Arange
        var from = 0.1,
            to = 0.2,
            decimals = 2,
            current = 0.1,
            gap = 0.01;

        //// Act
        var options = fanex.createOptionsFromRange(current, from, to, gap, decimals);

        //// Assert
        expect(options.length).toEqual(11);
        expect(options[options.length - 1].value).toEqual(current);
        expect(options[0].value).toEqual(to);
    });

    it('should create range from 0.1 to 0.2 and current must lower than range', function () {
        //// Arange
        var from = 0.1,
            to = 0.2,
            decimals = 2,
            current = 0.01,
            gap = 0.01;

        //// Act
        var options = fanex.createOptionsFromRange(current, from, to, gap, decimals);

        //// Assert
        expect(options.length).toEqual(12);
        expect(options[options.length - 1].value).toEqual(current);
        expect(options[0].value).toEqual(to);
    });

    it('should create range from 0.1 to 0.2 and current must lower than range', function () {
        //// Arange
        var from = 0.1,
            to = 0.2,
            decimals = 2,
            current = 0.21,
            gap = 0.01;

        //// Act
        var options = fanex.createOptionsFromRange(current, from, to, gap, decimals);

        //// Assert
        expect(options.length).toEqual(12);
        expect(options[0].value).toEqual(current);
    });
});

