
set INPUT=thesis
set AUXDR=auxiliary
set INCDR=include

rm -f %INPUT%.pdf

set PDFTEX=pdflatex -qutie --include-directory=%INCDR% -aux-directory=%AUXDR% %INPUT%.tex
set BIBTEX=bibtex -qutie --include-directory=%INCDR% %AUXDR%/%INPUT%.aux

%PDFTEX%
%BIBTEX%
%PDFTEX%
%PDFTEX%
