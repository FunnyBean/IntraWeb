/// <binding AfterBuild="default" Clean="clean" />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var paths = {
    webroot: "./wwwroot/",
    npm: "./node_modules/",
    lib: "./wwwroot/lib/",
    scripts: "./Scripts/"
};

var libs = [
    paths.npm + "angular2/bundles/angular2.dev.js",
    paths.npm + "angular2/bundles/http.dev.js",
    paths.npm + "angular2/bundles/angular2-polyfills.js",
    paths.npm + "es6-shim/es6-shim.js",
    paths.npm + "systemjs/dist/system.js",
    paths.npm + "systemjs/dist/system-polyfills.js"
];

gulp.task("rxjs", function () {
    return gulp.src(paths.npm + "rxjs/**/*.js").pipe(gulp.dest(paths.lib + "rxjs/"));
});

gulp.task("libs", ["rxjs"], function () {
    return gulp.src(libs).pipe(gulp.dest(paths.lib));
});

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task("default", ["libs", "min"]);
