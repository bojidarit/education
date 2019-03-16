// Source: https://www.itorian.com/2015/04/the-field-date-must-be-date-error-in.html
// Issue: Basically, this issue is related with all Webkit based web browsers. 
// Solution: The best solution for me was to override the validate date function from jquery.validate.js.

$(function () {
	$.validator.methods.date = function (value, element) {
		if ($.browser.webkit) {
			var d = new Date();
			return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
		}
		else {
			return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
		}
	};
});