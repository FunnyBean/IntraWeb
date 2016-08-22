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
    app: "./wwwroot/"
};

var libs = [
    paths.npm + "@angular2/**/*.js*",
    paths.npm + "systemjs/**/*.js*"
];

gulp.task("rxjs", function () {
    return gulp.src(paths.npm + "rxjs/**/*.js*")
        .pipe(gulp.dest(paths.lib + "rxjs/"));
});

gulp.task("anuglar", function () {
    return gulp.src(paths.npm + "@angular/**/*.js*")
        .pipe(gulp.dest(paths.lib + "@angular/"));
});

gulp.task("systemjs", function () {
    return gulp.src(paths.npm + "systemjs/**/*.js*")
        .pipe(gulp.dest(paths.lib + "systemjs/"));
});

gulp.task("core-js", function () {
    return gulp.src(paths.npm + "core-js/**/*.js*")
        .pipe(gulp.dest(paths.lib + "core-js/"));
});

gulp.task("reflect-metadata", function () {
    return gulp.src(paths.npm + "reflect-metadata/**/*.js*")
        .pipe(gulp.dest(paths.lib + "reflect-metadata/"));
});

gulp.task("zone.js", function () {
    return gulp.src(paths.npm + "zone.js/**/*.js")
        .pipe(gulp.dest(paths.lib + "zone.js*/"));
});


gulp.task("libs", ["rxjs", "anuglar", "systemjs", "core-js", "reflect-metadata", "zone.js"]);

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

gulp.task("images", function () {
    return gulp.src(paths.scripts + "**/*.png")
       .pipe(gulp.dest(paths.app));
});
gulp.task("javaScript", function () {
    return gulp.src(paths.scripts + "**/*.js*")
       .pipe(gulp.dest(paths.app));
});
gulp.task("CSS2", function () {
    return gulp.src(paths.scripts + "**/*.css")
       .pipe(gulp.dest(paths.app));
});
gulp.task("bootstrap", function () {
    return gulp.src(paths.scripts + "**/bootstrap/*")
       .pipe(gulp.dest(paths.app));
});

gulp.task("build", ["html", "js", "css", "images", "javaScript", "CSS2", "bootstrap"]);