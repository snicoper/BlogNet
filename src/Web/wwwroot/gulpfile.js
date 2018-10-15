/**
 * Load npm modules
 * Usar set NODE_ENV=production para producción en windows
 */
const babel = require('gulp-babel')
const concat = require('gulp-concat')
const gulp = require('gulp')
const imagemin = require('gulp-imagemin')
const merge = require('merge2')
const sass = require('gulp-sass')
const sourcemaps = require('gulp-sourcemaps')
const uglify = require('gulp-uglify')

const nodeEnv = process.env.NODE_ENV

/**
 * Paths.
 */
const Paths = {

  /**
     * Styles.
     */
  styles: {
    watch: ['./static/src/scss/**/*'],
    srcScss: ['./static/src/scss/main.scss'],
    cssSrc: [
      './node_modules/cookieconsent/build/cookieconsent.min.css',
      './node_modules/highlightjs/styles/atom-one-light.css',
      './node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css',
    ],
    dest: './static/dist/css/'
  },

  /**
     * Scripts.
     */
  scripts: {
    watch: ['./static/src/js/**/*.js'],
    src: ['./static/src/js/**/*.js'],
    dest: './static/dist/js/'
  },

  // Scripts de terceros.
  scriptsThird: [
    './node_modules/jquery/dist/jquery.js',
    './node_modules/popper.js/dist/umd/popper-utils.js',
    './node_modules/popper.js/dist/umd/popper.js',
    './node_modules/bootstrap/dist/js/bootstrap.js',
    './node_modules/jquery-validation/dist/jquery.validate.js',
    './node_modules/jquery-validation/dist/localization/messages_es.js',
    './node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
    './node_modules/toastr/toastr.js',
    './node_modules/cookieconsent/src/cookieconsent.js',
    './node_modules/js-cookie/src/js.cookie.js',
    './node_modules/axios/dist/axios.js',
    './node_modules/moment/min/moment.min.js',
    './node_modules/moment/locale/es.js',
    './node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js',
    './node_modules/bootstrap-datepicker/dist/locales/bootstrap-datepicker.en-GB.min.js',
    './node_modules/bootstrap-datepicker/dist/locales/bootstrap-datepicker.es.min.js',
    './node_modules/select2/dist/js/select2.min.js',
    './node_modules/select2/dist/js/i18n/es.js',
    './node_modules/highlightjs/highlight.pack.js',
    './node_modules/es6-promise/dist/es6-promise.min.js',
    './node_modules/es6-promise/dist/es6-promise.auto.min.js'
  ].concat(
    nodeEnv === 'production' ?
      './node_modules/vue/dist/vue.min.js' :
      './node_modules/vue/dist/vue.js'
  ).concat(
    'node_modules/vuejs-paginate/dist/index.js'
  ),

  /**
     * Images.
     */
  images: {
    watch: ['./static/src/img/**/*'],
    src: ['./static/src/img/**/*'],
    dest: './static/dist/img',
  }
}

/******************************************************************************
 * Tareas Copy files.
 *****************************************************************************/
gulp.task('copy', () => {
  /**
     * Fuentes.
     *
     * Copia archivos de node_modules u otros sitios a src/static/dist/xx
     */
  // Font-Awesome 5.
  gulp.src(['./node_modules/@fortawesome/fontawesome-free-webfonts/webfonts/**/*'])
    .pipe(gulp.dest('./static/dist/fonts/font-awesome'))

  // Roboto fonts.
  gulp.src(['./node_modules/roboto-fontface/fonts/roboto/**/*'])
    .pipe(gulp.dest('./static/dist/fonts/roboto'))

    /**
     * Imágenes.
     *
     * Copiar imágenes que no requieran compresión.
     * Si requieren de compresión, usar gulp.task('images', () => {})
     */
    // ...
})

/******************************************************************************
 * Tareas styles dev/prod.
 *****************************************************************************/
/**
 * CSS desarrollo.
 */
gulp.task('styles:dev', () => {
  merge(
    gulp.src(Paths.styles.srcScss)
      .pipe(sass().on('error', sass.logError)),
    gulp.src(Paths.styles.cssSrc)
  )
    .pipe(sourcemaps.init())
    .pipe(concat('main.css'))
    .pipe(sourcemaps.write('./'))
    .pipe(gulp.dest(Paths.styles.dest))
})

/**
 * CSS producción.
 */
gulp.task('styles:prod', () => {
  merge(
    gulp.src(Paths.styles.srcScss)
      .pipe(sass({ outputStyle: 'compressed' }).on('error', sass.logError)),
    gulp.src(Paths.styles.cssSrc)
  )
    .pipe(concat('main.min.css'))
    .pipe(gulp.dest(Paths.styles.dest))
})

/*****************************************************************************
 * Tareas Javascript.
 *****************************************************************************/

/**
 * Javascript locales, desarrollo.
 */
gulp.task('scripts:dev', () => {
  gulp.src(Paths.scripts.src)
    .pipe(concat('main.js'))
    .pipe(sourcemaps.init())
    .pipe(babel({
      presets: ['es2015']
    }))
    .pipe(sourcemaps.write('./'))
    .pipe(gulp.dest(Paths.scripts.dest))
})

/**
 * Javascript locales, producción.
 */
gulp.task('scripts:prod', () => {
  gulp.src(Paths.scripts.src)
    .pipe(concat('main.min.js'))
    .pipe(babel({
      presets: ['es2015']
    }))
    .pipe(uglify())
    .pipe(gulp.dest(Paths.scripts.dest))
})

/**
 * Javascript de terceros.
 */
gulp.task('scripts:third', () => {
  gulp.src(Paths.scriptsThird)
    .pipe(concat('third.js'))
    .pipe(uglify({
      output: {
        // eslint-disable-next-line
        max_line_len: 100000
      }
    }))
    .pipe(gulp.dest(Paths.scripts.dest))
})

/******************************************************************************
 * Tareas images.
 *****************************************************************************/

gulp.task('images', () => {
  gulp.src(Paths.images.src)
    .pipe(imagemin())
    .pipe(gulp.dest(Paths.images.dest))
})

/******************************************************************************
 * Watches.
 *
 * Solo son para archivos locales en desarrollo.
 *****************************************************************************/

// Watch styles.
gulp.task('watch:styles', () => {
  gulp.watch(Paths.styles.watch, ['styles:dev'])
})

// Watch scripts.
gulp.task('watch:scripts', () => {
  gulp.watch(Paths.scripts.watch, ['scripts:dev'])
})

// Watch images.
gulp.task('watch:images', () => {
  gulp.watch(Paths.images.watch, ['images'])
})

// Watches
gulp.task('watch', () => {
  gulp.watch(Paths.styles.watch, ['styles:dev'])
  gulp.watch(Paths.scripts.watch, ['scripts:dev'])
  gulp.watch(Paths.images.watch, ['images'])
})

/******************************************************************************
 * Commands.
 *****************************************************************************/
/**
 * Genera archivos para desarrollo y producción.
 * copy no tiene watch, por lo que se ha de generar al menos una vez.
 */

gulp.task('production', [
  'copy',
  'images',
  'styles:prod',
  'scripts:third',
  'scripts:prod',
])

gulp.task('development', [
  'copy',
  'images',
  'styles:dev',
  'scripts:third',
  'scripts:dev'
])

gulp.task('default', ['production', 'development'])
