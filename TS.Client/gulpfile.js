var gulp = require('gulp');

var CONFIG = {
    buildFolder : ".build",
    sourceFolder : "src",
    distributionFolder : "dist",
    declarationFileName : "empiria.core",
};

// Clean the contents of the distribution directory
gulp.task('clean', ['clean-build-folder', 'clean-distribution-folder']);

// Clean the contents of the temporary build directory
gulp.task('clean-build-folder', function () {
  var del = require('del');

  return del(CONFIG.buildFolder + "/**/*");
});

// Clean the contents of the distribution directory
gulp.task('clean-distribution-folder', function () {
  var del = require('del');

  return del(CONFIG.distributionFolder + "/**/*");
});

// TypeScript compile with source maps and typings (d.ts declaration files)
gulp.task('compile', ['clean'], function () {
  var typescript = require('gulp-typescript');
  var sourcemaps = require('gulp-sourcemaps');
  var del = require('del');

  var tscConfigData = require('./tsconfig.json');

  del(CONFIG.buildFolder);

  var tsResult = gulp.src(tscConfigData.fileGlobs)
        .pipe(sourcemaps.init())
        .pipe(typescript(tscConfigData.compilerOptions));

  tsResult.dts.pipe(gulp.dest(CONFIG.buildFolder));

  return tsResult.js
          .pipe(sourcemaps.write('.'))
          .pipe(gulp.dest(CONFIG.buildFolder));
});

// TypeScript linting.
gulp.task('tslint', function() {
  var tslintPlugin = require('gulp-tslint');
  var linter = require("tslint");

  // Look for the built-in rules at https://github.com/palantir/tslint#supported-rules
  return gulp.src(CONFIG.sourceFolder + "/**/*.ts")
          .pipe(tslintPlugin({tslint: linter}))
          .pipe(tslintPlugin.report("verbose", { emitError: false, summarizeFailureOutput: true }));
});

// Generate empiria.core.d.ts bundle file
gulp.task('tsd-bundle', ['compile'], function () {
  var dts = require('dts-bundle');

  return dts.bundle({
    name: 'Empiria',
    baseDir:  CONFIG.buildFolder,
    main: CONFIG.buildFolder + '/' + CONFIG.declarationFileName + ".d.ts",
    out: "../" + CONFIG.distributionFolder + '/' + CONFIG.declarationFileName + ".d.ts",
  });
});

// Generate javascript bundle file
gulp.task('js-bundle', ['compile'], function() {
  var concat = require('gulp-concat');

  return gulp.src(CONFIG.buildFolder + "/*.js")
    .pipe(concat(CONFIG.declarationFileName + ".js"))
    .pipe(gulp.dest(CONFIG.distributionFolder));
});

gulp.task('build', ['tslint', 'compile', 'tsd-bundle', 'js-bundle']);
gulp.task('default', ['build']);
