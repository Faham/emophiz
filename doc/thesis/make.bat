
set INPUT=thesis
set AUXDR=auxiliary
set INCDR=include

rm -f %INPUT%.pdf

mkdir %AUXDR%

set PDFTEX=pdflatex -quiet --include-directory=%INCDR% -aux-directory=%AUXDR% %INPUT%.tex
set BIBTEX=bibtex -quiet --include-directory=%INCDR% %AUXDR%/%INPUT%.aux

%PDFTEX%
%BIBTEX%
%PDFTEX%
%PDFTEX%
