/// <binding AfterBuild="build" Clean="clean" />
"use strict";

var gulp = require("gulp"),
    rimraf = require("gulp-rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass"),
    typescript = require("gulp-typescript");

var paths = {
    webroot: "./wwwroot/",
    npm: "./node_modules/",
    lib: "./wwwroot/lib/",
    scripts: "./Scripts/",
    app: "./wwwroot/app/"
};

var libs = [
    paths.npm + "angular2/bundles/angular2.dev.js",
    paths.npm + "angular2/bundles/http.dev.js",
    paths.npm + "angular2/bundles/angular2-polyfills.js",
    paths.npm + "angular2/bundles/router.dev.js",
    paths.npm + "es6-shim/es6-shim.js",
    paths.npm + "systemjs/dist/system.js",
    paths.npm + "systemjs/dist/system-polyfills.js"
];

gulp.task("rxjs", function () {
    return gulp.src(paths.npm + "rxjs/**/*.js")
        .pipe(gulp.dest(paths.lib + "rxjs/"));
});

gulp.task("libs", ["rxjs"], function () {
    return gulp.src(libs)
        .pipe(gulp.dest(paths.lib));
});

gulp.task("clean", function () {
    return gulp.src(paths.app + "**/*.*", { read: false })
        .pipe(rimraf());
});

gulp.task("html", function () {
    return gulp.src(paths.scripts + "**/*.html")
        .pipe(gulp.dest(paths.app));
});

gulp.task("js", function () {
    return gulp.src(paths.scripts + "**/*.ts")
        .pipe(typescript({
            "noImplicitAny": false,
            "noEmitOnError": true,
            "removeComments": false,
            "sourceMap": false,
            "target": "es5",
            "module": "commonjs",
            "experimentalDecorators": true,
            "emitDecoratorMetadata": true
        }))
        .pipe(gulp.dest(paths.app));
});

gulp.task("css", function () {
    return gulp.src(paths.scripts + "**/*.scss")
        .pipe(sass())
        .pipe(gulp.dest(paths.app));
});

gulp.task("build", ["libs", "html", "js", "css"]);
