import gulp from 'gulp';
import project from '../aurelia.json';
export default function copyKendoFonts() {
return gulp.src(project.paths.kendoFontsInput).pipe(gulp.dest(project.paths.kendoFontsOutput));
}
