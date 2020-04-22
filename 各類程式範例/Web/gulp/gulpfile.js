var debug = false;

var gulp = require("gulp");
var uglify = require("gulp-uglify");
var browserify = require("browserify");
var source = require('vinyl-source-stream');
var tsify = require("tsify");
var gutil = require("gulp-util");
var buffer = require('vinyl-buffer');
var del = require("del");
var htmlreplace = require('gulp-html-replace');
var htmlmin = require("gulp-htmlmin");
var cssmin = require("gulp-cssmin");
var rename = require('gulp-rename');

var tsProject = {
    "noImplicitAny": false,
    "noEmitOnError": true,
    "removeComments": true,
    "sourceMap": false,
    "target": "es5"
};
if (debug) {
    tsProject = {
        "noImplicitAny": false,
        "noEmitOnError": true,
        "removeComments": false,
        "sourceMap": true,
        "target": "es5"
    };
}

var files = {
    min_js: [
        "./node_modules/jquery/dist/jquery.min.js"
        , "./node_modules/jquery-ui-npm/jquery-ui.min.js"
        , "./node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"
        , "./node_modules/moment/min/moment.min.js"
        , "./node_modules/moment/min/moment-with-locales.min.js"
        , "./node_modules/moment-timezone/builds/moment-timezone.min.js"
        , "./node_modules/moment-timezone/builds/moment-timezone-with-data-1970-2030.min.js"
        , "./node_modules/@microsoft/signalr/dist/browser/signalr.min.js"
        , "./node_modules/pc-bootstrap4-datetimepicker/build/js/bootstrap-datetimepicker.min.js"
        , "./node_modules/bootstrap-table/dist/bootstrap-table.min.js"
        , "./node_modules/bootstrap-table/dist/bootstrap-table-locale-all.min.js"
        , "./node_modules/bootstrap-treeview/dist/bootstrap-treeview.min.js"
        , "./node_modules/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.js"
        , "./node_modules/vis/dist/vis.min.js"
        , "./node_modules/vis/dist/vis-graph3d.min.js"
        , "./node_modules/vis/dist/vis-timeline-graph2d.min.js"
    ],
    js: [
        "./node_modules/jquery.cookie/jquery.cookie.js"
        , "./node_modules/jquery.ui.widget/jquery.ui.widget.js"
        , "./node_modules/blueimp-file-upload/js/jquery.iframe-transport.js"
        , "./node_modules/blueimp-file-upload/js/jquery.fileupload.js"
        , "./node_modules/jquery-file-download/src/Scripts/jquery.fileDownload.js"
    ],
    min_css: [
        "./node_modules/bootstrap/dist/css/bootstrap.min.css"
        , "./node_modules/@fortawesome/fontawesome-free/css/all.min.css"
        , "./node_modules/pc-bootstrap4-datetimepicker/build/css/bootstrap-datetimepicker.min.css"
        , "./node_modules/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.css"
        , "./node_modules/bootstrap-table/dist/bootstrap-table.min.css"
        , "./node_modules/bootstrap-treeview/dist/bootstrap-treeview.min.css"
        , "./node_modules/vis/dist/vis.min.css"
        , "./node_modules/vis/dist/vis-timeline-graph2d.min.css"
    ],
    css: [
        "./src/css/chevalier.css"
        , "./node_modules/blueimp-file-upload/css/jquery.fileupload.css"
    ],
    font: [
        "./node_modules/@fortawesome/fontawesome-free/webfonts/*.*"
    ]
};
var useFiles = {
    js: [
        "/js/jquery.min.js"
        , "/js/jquery.cookie.min.js"
        , "/js/bootstrap.bundle.min.js"
        , "/js/moment.min.js"
        , "/js/moment-with-locales.min.js"
        , "/js/signalr.min.js"
        , "/js/jquery.bootstrap-touchspin.min.js"
        , "/js/bootstrap-datetimepicker.min.js"
        , "/js/jquery.ui.widget.min.js"
        , "/js/jquery.iframe-transport.min.js"
        , "/js/jquery.fileupload.min.js"
        , "/js/jquery.fileDownload.min.js"
        , "/js/bootstrap-table.min.js"
        , "/js/bootstrap-table-locale-all.min.js"
        , "/js/bootstrap-treeview.min.js"
    ]
    , css: [
        "/css/all.min.css"
        , "/css/bootstrap.min.css"
        , "/css/bootstrap-datetimepicker.min.css"
        , "/css/jquery.bootstrap-touchspin.min.css"
        , "/css/jquery.fileupload.min.css"
        , "/css/bootstrap-table.min.css"
        , "/css/bootstrap-treeview.min.css"
        , "/css/chevalier.min.css"
    ]
    , css_chart: [
        "/css/vis.min.css"
        , "/css/vis-timeline-graph2d.min.css"
    ]
    , js_chart: [
        "/js/vis.min.js"
        , "/js/vis-graph3d.min.js"
        , "/js/vis-timeline-graph2d.min.js"
    ]
};

gulp.task("copy:clean", done => {
    del(["wwwroot"]);
    done();
});
gulp.task("copy:min_js", done => {
    gulp.src(files.min_js)
        .pipe(gulp.dest("./wwwroot/js"));
    done();
});
gulp.task("copy:js", done => {
    gulp.src(files.js)
        .pipe(uglify())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest("./wwwroot/js"));
    done();
});
gulp.task("copy:min_css", done => {
    gulp.src(files.min_css)
        .pipe(gulp.dest("./wwwroot/css"));
    done();
});
gulp.task("copy:css", done => {
    gulp.src(files.css)
        .pipe(cssmin())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest('./wwwroot/css'));
    done();
});
gulp.task("copy:font", done => {
    gulp.src(files.font)
        .pipe(gulp.dest("./wwwroot/webfonts"));
    done();
});
gulp.task("copy:image", done => {
    gulp.src("./src/images/*.*")
        .pipe(gulp.dest("./wwwroot/images"));
    done();
});
gulp.task("copy", gulp.series("copy:clean", "copy:min_js", "copy:js", "copy:min_css", "copy:css", "copy:font", "copy:image"));


gulp.task("index:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/index.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("index.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot"));
});
gulp.task("index:html", done => {
    gulp.src(["./src/pages/index.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "index.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot"));
    done();
});
gulp.task("index", gulp.series(["index:js", "index:html"]));

gulp.task("machine-status:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/machine-status.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("machine-status.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("machine-status:html", done => {
    gulp.src(["./src/pages/machine-status.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "/pages/machine-status.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("machine-status", gulp.series(["machine-status:js", "machine-status:html"]));

gulp.task("system-user:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/system-user.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("system-user.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("system-user:html", done => {
    gulp.src(["./src/pages/system-user.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "/pages/system-user.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("system-user", gulp.series(["system-user:js", "system-user:html"]));

gulp.task("system-machine:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/system-machine.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("system-machine.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("system-machine:html", done => {
    gulp.src(["./src/pages/system-machine.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "/pages/system-machine.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("system-machine", gulp.series(["system-machine:js", "system-machine:html"]));

gulp.task("system-log:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/system-log.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("system-log.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("system-log:html", done => {
    gulp.src(["./src/pages/system-log.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "/pages/system-log.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("system-log", gulp.series(["system-log:js", "system-log:html"]));

gulp.task("system-parameter:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/system-parameter.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("system-parameter.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("system-parameter:html", done => {
    gulp.src(["./src/pages/system-parameter.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "/pages/system-parameter.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("system-parameter", gulp.series(["system-parameter:js", "system-parameter:html"]));

gulp.task("machine-historical_alarm:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/machine-historical_alarm.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("machine-historical_alarm.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("machine-historical_alarm:html", done => {
    gulp.src(["./src/pages/machine-historical_alarm.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "js": useFiles.js
            , "js_page": [
                "/pages/machine-historical_alarm.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("machine-historical_alarm", gulp.series(["machine-historical_alarm:js", "machine-historical_alarm:html"]));

gulp.task("machine-historical-signal:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/machine-historical-signal.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("machine-historical-signal.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("machine-historical-signal:html", done => {
    gulp.src(["./src/pages/machine-historical-signal.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "css_chart": useFiles.css_chart
            , "js": useFiles.js
            , "js_chart": useFiles.js_chart
            , "js_page": [
                "/pages/machine-historical-signal.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("machine-historical-signal", gulp.series(["machine-historical-signal:js", "machine-historical-signal:html"]));

gulp.task("machine-utilization:js", function () {
    return browserify({
        basedir: ".",
        debug: debug,
        entries: ["./src/pages/machine-utilization.ts"],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify, tsProject)
        .bundle()
        .pipe(source("machine-utilization.js"))
        .pipe(debug ? gutil.noop() : buffer())
        .pipe(debug ? gutil.noop() : uglify())
        .pipe(gulp.dest("./wwwroot/pages"));
});
gulp.task("machine-utilization:html", done => {
    gulp.src(["./src/pages/machine-utilization.html"])
        .pipe(htmlreplace({
            "css": useFiles.css
            , "css_chart": useFiles.css_chart
            , "js": useFiles.js
            , "js_chart": useFiles.js_chart
            , "js_page": [
                "/pages/machine-utilization.js"
            ]
        }))
        .pipe(debug ? gutil.noop() : htmlmin({ collapseWhitespace: true }))
        .pipe(gulp.dest("./wwwroot/pages"));
    done();
});
gulp.task("machine-utilization", gulp.series(["machine-utilization:js", "machine-utilization:html"]));

gulp.task("pages", gulp.series(["index", "machine-status", "system-user", "system-machine", "system-log", "system-parameter", "machine-historical_alarm", "machine-historical-signal", "machine-utilization"]));
