var javaBass;
(function (javaBass) {
    var Api = (function () {
        function Api() {
        }
        Api.READY = function () {
            Api.xhr = null;
            Api.Modal1 = $('#Modal1');
            Api.Modal1Title = $('#Modal1Title');
            Api.Modal1BodyP = $('#Modal1BodyP');
            Api.Modal1Ajaxloader = $('#Modal1Ajaxloader');
            Api.Modal1Button = $('#Modal1Button');
            Api.Modal1HeaderB = $('#Modal1HeaderB');
            Api.divTarget = $('#divTarget');
            Api.getPage(Api.divTarget.data("init-target"), false);
            window.onhashchange = Api.onhashchange;
        };
        Api.onhashchange = function () {
            // används ej borde var mied från början : /
            /*
            if (!Api.onhashchangeBlock)
                console.log(document.location.hash);
            else
                Api.onhashchangeBlock = false;
            */
        };
        Api.searchInit = function () {
            Api.searchTextBox = $('#searchTextBox');
            Api.searchTextForm = $('#searchTextForm');
            Api.searchTextForm.submit(Api.triggerSearch);
        };
        Api.searchSetSearchAntal = function (i) {
            javaBass.Api.searchSida = 0;
            Api.searchAntal = i;
            Api.search(null);
        };
        Api.searchSetSearchSida = function (i) {
            Api.searchSida = i;
            Api.search(null);
        };
        Api.searchSetsearchPrisSort = function () {
            Api.searchPrisSort = !Api.searchPrisSort;
            Api.search(null);
        };
        Api.triggerSearch = function (event) {
            Api.search(Api.searchTextBox.val());
            if (event != null)
                event.preventDefault();
        };
        Api.search = function (searchText_) {
            if (searchText_ != null) {
                Api.searchSida = 0;
                Api.searchTextTemp = searchText_;
            }
            else {
                searchText_ = Api.searchTextTemp;
            }
            var markers = {
                searchText: searchText_,
                searchAntal: Api.searchAntal,
                searchSida: Api.searchSida,
                searchPrisSort: Api.searchPrisSort
            };
            var searchCallbackSuccess = (function (data) {
                Api.xhr = null;
                var obj = jQuery.parseJSON(data);
                if (obj.result) {
                    Api.pageContet(obj.data, "serchResultspage");
                    Api.dialog(false);
                }
                else {
                    Api.dialogEditEX("EX");
                }
            });
            var searchCallbackError = (function (jqXHR, textStatus, errorThrown) {
                Api.xhr = null;
                Api.dialogEditEX(jqXHR + " " + textStatus + " " + errorThrown);
            });
            Api.xhr = $.ajax({
                type: "POST", url: "ajax/serchresultspage",
                success: searchCallbackSuccess,
                error: searchCallbackError,
                data: markers,
                accept: 'application/json'
            });
            Api.dialogEdit("Söker efter " + searchText_, true, null, null, null, null, false, false);
        };
        Api.searchBack = function () {
            Api.search(null);
        };
        Api.getPage = function (page, showdialog) {
            var getPageCallbackSuccess = (function (data) {
                Api.xhr = null;
                var obj = jQuery.parseJSON(data);
                if (obj.result) {
                    Api.pageContet(obj.data, Api.getPageTarget);
                    Api.dialog(false);
                }
                else {
                    Api.dialogEditEX("EX");
                }
            });
            var getPageCallbackError = (function (jqXHR, textStatus, errorThrown) {
                Api.xhr = null;
                Api.dialogEditEX(jqXHR + " " + textStatus + " " + errorThrown);
            });
            Api.getPageTarget = page;
            var addres = $(location).attr("protocol") + "//" + $(location).attr("host") + "/ajax/" + page;
            Api.xhr = $.ajax({
                type: "POST",
                url: addres,
                success: getPageCallbackSuccess,
                error: getPageCallbackError,
                data: { page: page },
                accept: 'application/json'
            });
            if (showdialog)
                Api.dialogEdit("Öppnar " + page, true, null, null, null, null, false, false);
        };
        Api.pageContet = function (data, thePageTarget) {
            if (thePageTarget === void 0) { thePageTarget = ""; }
            Api.divTarget.html(data);
            if (thePageTarget != "") {
                if (thePageTarget == "loginpage")
                    Login.init();
                if (thePageTarget == "startpage")
                    Start.init();
                if (thePageTarget == "serchResultspage")
                    SerchResults.init();
                if (thePageTarget == "productpage")
                    Product.init();
            }
        };
        Api.dialogEditEX = function (text) {
            var mef = (function () { Api.dialog(false); });
            Api.dialogEdit("EX", false, "close", mef, text, mef, true, true);
        };
        Api.logout = function () {
            var logoutCallbackSuccess = (function (data) {
                Api.xhr = null;
                var obj = jQuery.parseJSON(data);
                if (obj.result) {
                    Api.getPage("loginpage", true);
                }
                else {
                    var mef = (function () { Api.dialog(false); });
                    Api.dialogEdit("logout", false, "close", mef, "failed", mef, true, true);
                }
            });
            var logoutCallbackError = (function (jqXHR, textStatus, errorThrown) {
                Api.xhr = null;
                Api.dialogEditEX(jqXHR + " " + textStatus + " " + errorThrown);
            });
            Api.xhr = $.ajax({
                type: "POST", url: "ajax/logout",
                success: logoutCallbackSuccess,
                error: logoutCallbackError,
                accept: 'application/json'
            });
            Api.dialogEdit("logout", true, null, null, null, null, false, false);
        };
        Api.dialog = function (mode) {
            if (mode)
                Api.Modal1.modal('show');
            else
                Api.Modal1.modal('hide');
        };
        Api.dialogEdit = function (topText, loader, Modal1ButtonText, Modal1ButtonMef, Modal1BodyPText, Modal1HeaderBMef, backdrop, keyboard) {
            Api.Modal1Title.text(topText);
            if (topText != "")
                Api.Modal1Title.show();
            else
                Api.Modal1Title.hide();
            if (loader)
                Api.Modal1Ajaxloader.show();
            else
                Api.Modal1Ajaxloader.hide();
            if (Modal1ButtonText != "") {
                Api.Modal1Button.text(Modal1ButtonText);
            }
            if (Modal1ButtonMef != undefined && Modal1ButtonMef != null) {
                Api.Modal1Button.show();
                Api.Modal1Button.click(Modal1ButtonMef);
            }
            else
                Api.Modal1Button.hide();
            if (Modal1BodyPText != undefined && Modal1BodyPText != null && Modal1BodyPText != "") {
                Api.Modal1BodyP.text(Modal1BodyPText);
                Api.Modal1BodyP.show();
            }
            else
                Api.Modal1BodyP.hide();
            if (Modal1HeaderBMef != undefined && Modal1HeaderBMef != null) {
                Api.Modal1HeaderB.show();
                Api.Modal1HeaderB.click(Modal1HeaderBMef);
            }
            else
                Api.Modal1HeaderB.hide();
            Api.Modal1.modal({
                backdrop: (backdrop != undefined && backdrop != null && backdrop)
            });
            Api.Modal1.modal({
                keyboard: (keyboard != undefined && keyboard != null && keyboard)
            });
            Api.dialog(true);
        };
        Api.isinit = false;
        Api.searchAntal = 15;
        Api.searchSida = 0;
        Api.searchPrisSort = false;
        Api.searchTextTemp = "";
        Api.onhashchangeBlock = true;
        return Api;
    })();
    javaBass.Api = Api;
    var Login = (function () {
        function Login() {
        }
        Login.init = function () {
            console.log("Login.init()");
            Login.loginName = $('#loginName');
            Login.loginPw = $('#loginPw');
            Login.loginRememberMe = $('#loginRememberMe');
            $("#loginForm").submit(Login.loginonSubmit);
        };
        Login.loginonSubmit = function (event) {
            var markers = { name: Login.loginName.val(), pw: Login.loginPw.val(), rememberMe: Login.loginRememberMe.prop('checked') };
            Login.loginPw.val("");
            var addres = $(location).attr("protocol") + "//" + $(location).attr("host") + "/ajax/login";
            Api.xhr = $.ajax({
                type: "POST",
                url: addres,
                success: Login.loginCallbackSuccess,
                error: Login.loginCallbackError,
                data: markers,
                accept: 'application/json'
            });
            Api.dialogEdit("login", true, null, null, null, null, false, false);
            event.preventDefault();
        };
        Login.loginCallbackSuccess = function (data) {
            Api.xhr = null;
            var obj = jQuery.parseJSON(data);
            if (obj.result) {
                Api.getPage("startpage", true);
            }
            else {
                var mef = (function () { Api.dialog(false); });
                Api.dialogEdit("login", false, "close", mef, "failed", mef, true, true);
            }
        };
        Login.loginCallbackError = function (jqXHR, textStatus, errorThrown) {
            Api.xhr = null;
            Api.dialogEditEX(jqXHR + " " + textStatus + " " + errorThrown);
        };
        return Login;
    })();
    var Start = (function () {
        function Start() {
        }
        Start.init = function () {
            console.log("Start.init()");
            Api.searchInit();
        };
        Start.TagTriggering = function (aTagName) {
        };
        return Start;
    })();
    var SerchResults = (function () {
        function SerchResults() {
        }
        SerchResults.init = function () {
            console.log("SerchResults.init()");
            $('#SearchAntalA15').click(function (e) {
                e.preventDefault();
                javaBass.Api.searchSetSearchAntal(15);
            });
            $('#SearchAntalA24').click(function (e) {
                e.preventDefault();
                javaBass.Api.searchSetSearchAntal(24);
            });
            $('#SearchAntalA48').click(function (e) {
                e.preventDefault();
                javaBass.Api.searchSetSearchAntal(48);
            });
            $('#SearchAntalA75').click(function (e) {
                e.preventDefault();
                javaBass.Api.searchSetSearchAntal(75);
            });
            $('#SearchAntalA105').click(function (e) {
                e.preventDefault();
                javaBass.Api.searchSetSearchAntal(105);
            });
            $('#SorteraTogel').click(function (e) {
                e.preventDefault();
                javaBass.Api.searchSetsearchPrisSort();
            });
            $('#PavPagination').children().each(function () {
                $(this).children("a").each(function () {
                    $(this).click(SerchResults.PaginationLinck);
                });
            });
            Api.searchInit();
        };
        SerchResults.PaginationLinck = function (e) {
            e.preventDefault();
            var temp = $(this).data("target");
            if (temp !== "")
                javaBass.Api.searchSetSearchSida(temp);
        };
        SerchResults.openProdukt = function (name) {
            var markers = { name: name };
            Api.xhr = $.ajax({
                type: "POST", url: "ajax/productpage",
                success: SerchResults.openProduktCallbackSuccess,
                error: SerchResults.openProduktCallbackError,
                data: markers,
                accept: 'application/json'
            });
            Api.dialogEdit("open " + name, true, null, null, null, null, false, false);
        };
        SerchResults.openProduktCallbackSuccess = function (data) {
            Api.xhr = null;
            var obj = jQuery.parseJSON(data);
            if (obj.result) {
                Api.pageContet(obj.data, "productpage");
                Api.dialog(false);
            }
            else {
                Api.dialogEditEX("EX");
            }
        };
        SerchResults.openProduktCallbackError = function (jqXHR, textStatus, errorThrown) {
            Api.xhr = null;
            Api.dialogEditEX(jqXHR + " " + textStatus + " " + errorThrown);
        };
        return SerchResults;
    })();
    javaBass.SerchResults = SerchResults;
    var Product = (function () {
        function Product() {
        }
        Product.init = function () {
            console.log("Product.init()");
        };
        return Product;
    })();
})(javaBass || (javaBass = {}));
$(document).ready(javaBass.Api.READY);
//# sourceMappingURL=common.js.map