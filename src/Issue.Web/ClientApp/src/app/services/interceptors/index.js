"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/* "Barrel" of Http Interceptors */
var http_1 = require("@angular/common/http");
var auth_interceptor_1 = require("./auth-interceptor");
/** Http interceptor providers in outside-in order */
exports.httpInterceptorProviders = [
    { provide: http_1.HTTP_INTERCEPTORS, useClass: auth_interceptor_1.AuthInterceptor, multi: true },
];
//# sourceMappingURL=index.js.map