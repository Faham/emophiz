#/usr/bin/bash

INPUT=thesis
AUXDR=auxiliary
INCDR=include

rm -fr $AUXDR
rm -f $INPUT.pdf

mkdir $AUXDR

PDFTEX="pdflatex -quiet --shell-escape --include-directory=${INCDR} -aux-directory=${AUXDR} ${INPUT}.tex"
BIBTEX="bibtex -quiet --include-directory=${INCDR} ${AUXDR}/${INPUT}.aux"

$PDFTEX
$BIBTEX
$PDFTEX
$PDFTEX
