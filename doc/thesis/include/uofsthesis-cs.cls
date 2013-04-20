%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% CHANGE LOG
%
% Version 1.4.16
% --------------
%
% - The table of contents was set to be turned off if the nofigurelist 
%   was used.  Now the table of contents is typeset unconditionally.  
%   Credit goes to Mohsen Soltanifar and his M.Sc. thesis in the the U of S 
%   Math Department for finding this bug.
%
% Version 1.4.15
% --------------
%
% - The default margins are now 1 inch on all sides.  This conforms to the
%   ETD formatting guidelines.  The old behaviour of leaving 4cm on the left
%   margin for bound theses can be restored by passing the 'bound' option to
%   the document class.
%
% Version 1.4.14
% --------------
% 
% - Fixed a bug where captions were set to a fixed width of 5in.  This had
%   the desired effect of indenting the margins of the caption a small amount
%   but had the undesirable effect that the captions of side-by-side figures
%   typeset using minipages would also be 5in wide, and run right over top
%   of each other.  The problem was \captionsetup{} from the caption package.
%   The option of width=5in was changed to margin=0.5cm which achieves
%   the same thing, while allowing the width of side-by-side caption to be
%   automatically adjusted.
%
% Version 1.4.13
% --------------
%
% - Fixed a bug where the page number for the references in the table of
%   contents was the last page of the references section, not the first.
%   This was due to the \addcontentsline{} command occurring after
%   \bibliography{} in the definition of \uofsbibliography in uofsthesis.cls
%
% Version 1.4.12 
% -------------- 
%
% - Fixed a small bug which caused the headings of the Abstract and
%   list of abbreviations to be formatted incorrectly when using
%   'proposal' mode.
% - Added \uofsappendix command which simplifes the setup that has to take
%   place before appendices are typeset.  Use of this command can be found
%   in the class documentation.  thesis_template.tex was updated to
%   use this command.
% - Added \uofsbibliography command which simplifies the setup of
%   the reference list when using bibtex.  
%   Use of this command can be found in the 
%   class documentation.  thesis_template.tex was update to use this
%   command.
% - Added \uofsbibliographynobibtex command which sets up the reference
%   list when using a 'thebibliography' environment and bibitems. 
%   Again, usage is explained in the documentation.
%
%
% Version 1.4.11
% --------------
%
% - Margins are now specified in the .cls file using the \geometry package.
%   This will result in more accurate margins.
%
% - The geometry package is used to force letter-size paper.  This should
%   solve the problem of some LaTeX distributions defaulting to A4-sized paper
%   when latex+dvips is used to generate docuements.  
%
%
% Version 1.4.10
% --------------
% 
% - Bug fix: Chapter headings should no longer vary in spacing and vertical 
%   position when the global line spacing is changed using document class
%   options.
%
% Version 1.4.9
% -------------
%
% - Fixed a bug relating to compatibility with the color and xcolor
%   packages.  These packages should no longer result in errors
%   when using floats.  Thanks to J. Cooper for pointing this one out.
%
%
% Version 1.4.8
% -------------
%
% - Actually this version is the same as verison 1.4.7. I just changed
%   the documentation to add some information on how to make this
%   class work with the subfigure package.  The actual .cls file has
%   not changed.
%
%
%
% Verison 1.4.7
% -------------
%
% Changes:
% 
% - Changed the page headings in the frontmatter section so that they
%   have a one-inch top margin instead of a 5 cm one.  Chapter
%   headings still have the required 5cm margin.  This is done in
%   response to Grad Studies rejecting a thesis and agrees
%   with the thesis preparation guide.
%
%
% 
%
% Version 1.4.6
% -------------
%
% Changes:
% 
% - An improved framework for appendices is now provided in the
%   thesis_template.tex file.
% - Removed the [tableposition=top] option from the included 'caption'
%   package.  It was causing issues on Mac's and wasn't actually doing
%   anything.  
% - Changed headings for most frontmatter items to use \chapter,
%   eliminating the need for hardcoded vertical spacing before the
%   page number.  As a side effect, titles in the frontmatter are now
%   all at the same vertical position on the page.
%
%
% Version 1.4.5
% -------------
%
% New Features:
%
% - The address for requesting copies of the thesis on the permission to
%   use page can now be specified in the preamble using the
%   \ptuaddress{} command.  See the documentation for details.
%   Default address is ``Head of Computer Science Department''.
%
% Changes:
% 
% - Added an option in THIS FILE to reduce spacing between headings
%   paragraphs even further.  To use it you have to uncomment the
%   lines in THIS FILE under SPACING OF HEADINGS OPTION below.
%
%
%
% Version 1.4.4 
% -------------
%
% Changes:
% 
% - Department address on permission to use  page is updated to reflect our move to THORV.
%
%
% Version 1.4.3
% -------------
%
% New Features:
% 
% - The department name that appears on the title page
%   may now be specified in the document pre-amble using 
%   \department{dept-name}.  If no department name is specified, the default
%   is 'Computer Science'.  Documentation is updated to reflect this change.
% - The type of academic unit that appears on the title page 
%   may now be specified, e.g. if you are not in a departmentalized college.  
%   \academicunit{unit-type} specifies the type of academic unit (e.g. division, college).
%   Default is 'Department'.
% - Added a 'doublespace' option.  
%
% Changes:
%
% - The class now issues only a warning (instead of error) if 'singlespace' is used 
%   without using 'proposal'.
%   So you can do single-space theses if you really want to, but it is not permitted
%   by CGSR.
%
% 
%
% Version 1.4.2
% -------------
%
% Changes:
% - Fixed a typo in a command name for the singlespace option.
% - Fixed a bug that was introduced with the last bugfix.  By changing the figure environment
%   no optional arguments like [h] were being processed.  This was resolved by changing the
%   way floats are typeset rather than the actual float environment.  At the same time I also
%   added a fix for a future bug which ensures that footnotes are typeset single-spaced.
% - I used the 'caption' package to slightly modify the appearance of table and figure captions.
%   They are now slightly indented on both the left and right, making them easier to distinguish
%   from the text body.  The figure/table number and designation are now also in bold text.
%   It is supposed to be possible with this package to force table captions to be above the table
%   but I was unable to get this to work.
% 
%
% Version 1.4.1
% -------------
%
% Changes:  
% - Fixed a small problem where when double or 1.5 spacing is used, the multi-line 
%   figure and table captions also come out double or 1.5 spaced.  I Redefined the figure and table
%   environments to ensure that captions are always single-spaced.
%
%
% Version 1.4
% -----------
%
% 2004/09/13
%
% Changes:
% 
% - Redefined the 'quote' and 'quotation' environments so that the text is always single-spaced
%   according to CGSR guidelines.  Quotation spacing is now single-spaced regardless of
%   global spacing.  Text size of quotations should match base font size of thesis.
% - Modified the thesis_template.tex file so that the bibliography is single spaced.
% - The table of contents, list of figures, list of tables, and list of
%   abbreviations is now always single-spaced.
% - Changed the bibliographystile in thesis_template.tex to ``unsrt'' since CGSR
%   requires that references appear in the order in which they are first cited in the text.
%
% New features:
%
% - Added an option called 'singlespace'.  This option may be used ONLY if the 'proposal'
%   option is used.  It allows proposals to be printed using single-spacing to save on printing
%   costs for proposal drafts.  This option does not alter spacing of the title page.
%   This option is documented in the uofsthesis-cs.tex document.
%
% Thanks to David Fourney for most of the above suggestions.
%
%
% Version 1.3
% -----------
% 
% 2004/08/16
% 
% Changes:
% 
% - The entry in the table of contents for the table of contents now reads ``Contents'' instead 
%   of ``Table of Contents'' to match the title of the table of contents page.
% - Not really a class file issue, but I updated the thesis_template.tex file to prevent an
%   empty chapter from occurring when using BibTeX.  
%   Also improved bibliography instructions in the template file.
%
%
%
% Version 1.2
% -----------
%
% 2004/07/19
% 
% Changes:
% 
% - Now passes options through to the base report package by using \LoadClassWithOptions
%   Allows options like 12pt to be used.
% - Fixed spacing for pages with title headings.  The top margin was too large
% - Fixed some spacing in the chapter headings that CGSR was having issues with. 
%
%
%
% Version 1.1
% -----------
% 
% 2004/07/02
% 
% Changes:
% 
% - Fixed \ifx statement for nofigurelist option.
% - Added warning to read the CGSR guidelines to documentation and template.
% - Added frontmatter items to Table of Contents
% - Added support for list of abbreviations and added related documentation.
% 
% 2004/06/28 -- Original Version
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{uofsthesis-cs}[2011/06/03 -- U of S Thesis Style for Computer Science (v. 1.4.16), -- Mark G. Eramian]


\DeclareOption{doublespace}{
\let\doublespace=y
}

\DeclareOption{singlespace}{
\let\singlespace=Y
}

\DeclareOption{proposal}{
\let\proposal=Y
}

\DeclareOption{notablelist}{
\let\notablelist=Y
}
\DeclareOption{nofigurelist}{
\let\nofigurelist=Y
}

\DeclareOption{bound}{
\let\bound=Y
}

\ProcessOptions

\LoadClassWithOptions{report}
\RequirePackage{tocloft}
\RequirePackage{titlesec}
\RequirePackage{caption}
\RequirePackage{geometry} 	% for margins and paper size

%2.54cm = 1in
%11in = 27.94cm
%8.5in = 21.59cm

%Set up margins - Left margin is 4cm.  Remaining margins are 2.5cm
\flushbottom

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Set the margins.

\geometry{letterpaper}   % Force letter-sized paper.

% If a bound thesis is selected, leave extra space at the left margin for binding.
\ifx\bound\undefined
	\geometry{margin={2.5cm,2.5cm}}  % default margins
\else
	\geometry{left=4cm,right=2.5cm,top=2.5cm,bottom=2.5cm}     % bound thesis margins
\fi


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Grad studies says it is permissible to have all page numbers at bottom center
% Roman numerals for frontmatter, arabic numerals beginning with Chapter 1.
% All pages are to be numbered consecutively.
\pagestyle{plain}

% Spacing should be 1.5 or 2 for a thesis.  Default is 1.5.
\newcommand{\spacemultiplier}{1.5}

% Make sure only one spacing option is selected.
\ifx\singlespace\undefined
    \relax
\else
    \ifx\doublespace\undefined
        \relax
    \else
        \ClassError{uofsthesis-cs}{Cannot use 'doublespace' and 'singlespace' options together.}
    \fi
\fi


% If the proposal option is selected, we allow the singlespace option.
% The purpose of this is to reduce printed pages for reviewing drafts.
\ifx\singlespace\undefined
    \relax
\else
    \ifx\proposal\undefined
        \ClassWarning{uofsthesis-cs}{The 'singlespace' option can only be used in conjunction with the 'proposal' option.}
    \fi
    \renewcommand{\spacemultiplier}{1}
\fi

\ifx\doublespace\undefined
    \relax
\else
    \renewcommand{\spacemultiplier}{2}
\fi

% Set the spacing to the selected multiplier.
\renewcommand{\baselinestretch}{\spacemultiplier}\normalsize


% Customize the table of contents title fonts
\renewcommand{\cfttoctitlefont}{\LARGE\sc\hfill}
\renewcommand{\cftloftitlefont}{\LARGE\sc\hfill}
\renewcommand{\cftlottitlefont}{\LARGE\sc\hfill}
\renewcommand{\cftaftertoctitle}{\hfill}
\renewcommand{\cftafterloftitle}{\hfill}
\renewcommand{\cftafterlottitle}{\hfill}
\setlength{\cftbeforetoctitleskip}{-10pt}
\setlength{\cftbeforeloftitleskip}{-10pt}
\setlength{\cftbeforelottitleskip}{-10pt}

% Font changes - chapter headings and so forth
%\allsectionsfont{\sc}
%\titleformat{\chapter}[display]{\LARGE\sc\fillast}{Chapter \thechapter}{0pt}{}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% SPACING OF HEADINGS OPTION:  For less spacing between headings and
% paragraphs, uncomment the next few lines.  These are courtesy
% of Dr. Kevin Schneider and i'm not sure of the exact effect.
%
%\titleformat{\chapter}[display]{\renewcommand{\baselinestretch}{1.5}\LAR GE\sc\fillast}{Chapter \thechapter}{0pt}{}
%\titlespacing{\chapter}{0pt}{0cm}{8pt}
%\titlespacing*{\section}{0pt}{0cm}{0pt}
%\titlespacing*{\subsection}{0pt}{0cm}{0pt}
%\titlespacing*{\subsubsection}{0pt}{0cm}{0pt}
%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


% Redefine quote and quotation environments so that they are single spaced
% according to CGSR regulations.
\renewenvironment{quotation}
               {\list{}{\listparindent 1.5em%
                        \itemindent    \listparindent
                        \rightmargin   \leftmargin
                        \parsep        \z@ \@plus\p@}%
                \item\renewcommand{\baselinestretch}{1}\normalsize\relax}
               {\endlist}
\renewenvironment{quote}
               {\list{}{\rightmargin\leftmargin}%
                \item\renewcommand{\baselinestretch}{1}\normalsize\relax}
               {\endlist}
% Increase the space between last line of text and footnote rule.
% This is a very untidy way to do it!

\skip\footins 20pt plus4pt minus4pt

% Reset baselinestretch within footnotes and floats. Originally stolen
% from Stanford thesis style.

\def\@xfloat#1[#2]{\ifhmode \@bsphack\@floatpenalty -\@Mii\else
   \@floatpenalty-\@Miii\fi\def\@captype{#1}\ifinner
      \@parmoderr\@floatpenalty\z@
    \else\@next\@currbox\@freelist{\@tempcnta\csname ftype@#1\endcsname
       \multiply\@tempcnta\@xxxii\advance\@tempcnta\sixt@@n
       \@tfor \@tempa :=#2\do
                        {\if\@tempa h\advance\@tempcnta \@ne\fi
                         \if\@tempa t\advance\@tempcnta \tw@\fi
                         \if\@tempa b\advance\@tempcnta 4\relax\fi
                         \if\@tempa p\advance\@tempcnta 8\relax\fi
         }\global\count\@currbox\@tempcnta}\@fltovf\fi

%    Replaced this line with the following line to fix
%    compatiblity with the color package.
%
%    \global\setbox\@currbox\vbox\bgroup 

    \global\setbox\@currbox\color@vbox\normalcolor\vbox\bgroup

    \def\baselinestretch{1}
    \boxmaxdepth\z@
    \hsize\columnwidth \@parboxrestore}

\long\def\@footnotetext#1{\insert\footins{\def\baselinestretch{1}\footnotesize
    \interlinepenalty\interfootnotelinepenalty 
    \splittopskip\footnotesep
    \splitmaxdepth \dp\strutbox \floatingpenalty \@MM
    \hsize\columnwidth \@parboxrestore
   \edef\@currentlabel{\csname p@footnote\endcsname\@thefnmark}\@makefntext
    {\rule{\z@}{\footnotesep}\ignorespaces
      #1\strut}}}


%Make fancier table and figure captions -- from 'caption' package
\DeclareCaptionFormat{uofscaption}{#1#2#3}
\captionsetup{format=uofscaption,labelfont=bf,margin=.5cm}



%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%5
%Things needed for the title page
%(author/title/date already exist)
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

\newcommand{\PhD}{Doctor of Philosophy}
\newcommand{\MSc}{Master of Science}

\newcommand{\degree}[1]{\gdef\@degree{#1}}
\newcommand{\convocationdate}[1]{\gdef\@convocationdate{#1}}
\newcommand{\department}[1]{\gdef\@department{#1}}
\newcommand{\academicunit}[1]{\gdef\@academicunit{#1}}

\academicunit{} 
\department{}
\author{}
\title{}
\degree{}
\convocationdate{}

%%%%%%%%%%%%%%%%%%%%%%%%%%
% MAKETITLE definition
%%%%%%%%%%%%%%%%%%%%%%%%%%

\renewcommand{\maketitle}{
%Check for needed definitions
\ifx \@author \@empty
\ClassError{uofsthesis-cs}{\protect\author{} is undefined}
\fi
\ifx \@title \@empty
\ClassError{uofsthesis-cs}{\protect\title{} is undefined}
\fi
\ifx \@degree \@empty
\ClassError{uofsthesis-cs}{\protect\degree{} is undefined.  Use \protect\degree{\protect\PhD}
or \protect\degree{\protect\MSc}}
\fi
\ifx \@convocationdate \@empty
\ClassError{uofsthesis-cs}{\protect\convocationdate{} is undefined.}
\fi
\ifx \@department \@empty
\department{Computer Science}
\fi
\ifx \@academicunit \@empty
\academicunit{Department}
\fi
\thispagestyle{empty}
{\renewcommand{\baselinestretch}{\spacemultiplier}
\LARGE
\vbox{}\vfil
\begin{center}
\ifx\proposal\undefined
        \relax
\else
        Thesis Proposal:\\
\fi
\textsc{\@title}\\
\strut\\
\Large
\ifx\proposal\undefined
A Thesis Submitted to the\\
College of Graduate Studies and Research\\
in Partial Fulfillment of the Requirements\\
for the degree of \@degree\\
in the \@academicunit\ of \@department\\
University of Saskatchewan\\
Saskatoon\\
\strut\\
\fi
By\\
\@author\\
\ifx\proposal\undefined
\strut\\
\copyright \@author, \@convocationdate.  All rights reserved.
\fi
\end{center}
\vfil
}
\newpage
}

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%Things needed for the frontmatter.
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
\renewcommand{\abstract}[1]{\gdef\@abstract{#1}}
\abstract{}
\newcommand{\acknowledgements}[1]{\gdef\@acknowledgements{#1}}
\acknowledgements{}
\newcommand{\dedication}[1]{\gdef\@dedication{#1}}
\dedication{}
\newcommand{\loa}[1]{\gdef\@loa{#1}}
\loa{}
\newcommand{\ptuaddress}[1]{\gdef\@ptuaddress{#1}}
\ptuaddress{}

% Command to add an entry to the list of abbreviations.
\newcommand{\abbrev}[2]{#1& #2\\}


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% FRONTMATTER DEFINTION
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Remove chapter numbers and reformat the chapter headings for the preamble
\titleformat{\chapter}{\renewcommand{\baselinestretch}{1.5}\LARGE\sc\fillast}{\renewcommand{\baselinestretch}{\spacemultiplier}\normalsize}{0pt plus0pt minus0pt}{}
\titlespacing{\chapter}{0pt}{-30pt plus0pt minus0pt}{20pt plus0pt minus0pt}


\newcommand{\frontmatter}{
%CHeck for required definitons
\ifx \@abstract \@empty
\ClassError{uofsthesis-cs}{\protect\abstract{} is undefined}
\fi

%Each item in the frontmatter is to be on a separate page.
\pagenumbering{roman}

% PERMISSION TO USE
\ifx\undefined\proposal
%\vbox{}\vspace{1.1cm}
%\addcontentsline{toc}{chapter}{Permission to Use}
\ifx \@ptuaddress \@empty
\ptuaddress{Head of the Department of Computer Science\\
176 Thorvaldson Building\\
110 Science Place\\
University of Saskatchewan\\
Saskatoon, Saskatchewan\\
Canada\\
S7N 5C9
}
\fi



\addcontentsline{toc}{chapter}{Permission to Use}
\chapter*{Permission to Use} 
In presenting this thesis in partial fulfilment of the requirements for a Postgraduate degree from the University of Saskatchewan, I agree that the Libraries of this University may make it freely available for inspection. I further agree that permission for copying of this thesis in any manner, in whole or in part, for scholarly purposes may be granted by the professor or professors who supervised my thesis work or, in their absence, by the Head of the Department or the Dean of the College in which my thesis work was done. It is understood that any copying or publication or use of this thesis or parts thereof for financial gain shall not be allowed without my written permission. It is also understood that due recognition shall be given to me and to the University of Saskatchewan in any scholarly use which may be made of any material in my thesis.

Requests for permission to copy or to make other use of material in this thesis in whole or part should be addressed to:\\
\begin{itemize}
\item[] \@ptuaddress
\end{itemize}
\newpage
\fi

% ABSTRACT
\addcontentsline{toc}{chapter}{Abstract}
\chapter*{Abstract}
%\strut\\
\@abstract
\newpage

%ACKNOWLEDGEMENTS (if any)
\ifx \@acknowledgements \@empty
\relax
\else
        \ifx\proposal\undefined
        \addcontentsline{toc}{chapter}{Acknowledgements}
        \chapter*{Acknowledgements}
        \@acknowledgements
        \newpage
        \fi
\fi

%DEDICATION (If any)
\ifx \@dedication \@empty
\relax
\else
        \ifx\proposal\undefined
        \vbox{}\vfil
        \@dedication
        \vfil
        \newpage
        \fi
\fi

%TABLE OF CONTENTS
\addcontentsline{toc}{chapter}{Contents}
{\renewcommand{\baselinestretch}{1}\normalsize
\tableofcontents
}
\newpage

%LIST OF TABLES
\ifx\notablelist\undefined
\addcontentsline{toc}{chapter}{List of Tables}
{\renewcommand{\baselinestretch}{1}\normalsize
\listoftables
}
\newpage
\fi

%LIST OF FIGURES
\ifx\nofigurelist\undefined
\addcontentsline{toc}{chapter}{List of Figures}
{\renewcommand{\baselinestretch}{1}\normalsize
\listoffigures
}
\newpage
\fi

%LIST OF ABBREVIATIONS
\ifx \@loa \@empty
\relax
\else
%        \addcontentsline{toc}{chapter}{List of Abbreviations}
        {
          \addcontentsline{toc}{chapter}{List of Abbreviations}
          \chapter*{List of Abbreviations}          
          \renewcommand{\baselinestretch}{1}\normalsize
          \begin{center}
            \begin{tabular}{lp{11cm}}
              \@loa   
            \end{tabular}
          \end{center}        
        }
        \newpage
\fi

\pagenumbering{arabic}

% Return to normal chapters
\titleformat{\chapter}[display]{\renewcommand{\baselinestretch}{1.5}\LARGE\sc\fillast}{Chapter
  \thechapter}{0pt}{}[\renewcommand{\baselinestretch}{\spacemultiplier}\normalsize]
\titlespacing{\chapter}{0pt}{1.4cm}{20pt}
\setcounter{chapter}{0}
}


%%%%%%%%%%%%%%%%%%%%%%%%%%
% Appendix setup
%%%%%%%%%%%%%%%%%%%%%%%%%%

% This command is defined to simplify setup for the typesetting of appendices.

\newcommand{\uofsappendix}{
% Sets chapter counter to appendix mode (counts by letters instead of numbers)
\appendix

% This next line is needed to change the appendix chapter titles from reading
% "Chapter A" to "Appendix A".  This should not be necessary but it seems that
% the titlesec package overrides the changes to the chapter headings that
% the \appendix commands make.  We use the following workround:
\titleformat{\chapter}[display]{\renewcommand{\baselinestretch}{1.5}\normalsize
\LARGE\sc\fillast}{Appendix \thechapter}{0pt}{}


% This is needed to put a one-inch top margin on the first page of 
% appendices -- this seems to be what CGSR wants.  If not, just comment 
% out the next line. 
\titlespacing{\chapter}{0pt}{-30pt}{20pt}

% Appendices need to be single-spaced.  Reset spacing to single
% regardless of global document spacing.
\renewcommand{\baselinestretch}{1}\normalsize
}


%%%%%%%%%%%%%%%%%%%%%%%%%%
% Bibliography Setup
%%%%%%%%%%%%%%%%%%%%%%%%%%

% This command simplifes setup for the typesetting of the bibliography.
% It takes 3 parameters:   1. the bibliography style name (optional), 2. the  
% name of the .bib file (minus extension),.  


\newcommand{\uofsbibliography}[2][plain]{
% We need to force single spacing for the bibliography which must be single
% spaced according to CGSR rules.  The line below changes spacing to single space.
% This command does not take effect until a new font size is selected, so we add
% the \normalsize command to make the spacing take effect.
{
\renewcommand{\baselinestretch}{1}\normalsize

% Select the title of the bibliography section by redefining the \bibname 
% command:
\renewcommand{\bibname}{References} 

% Add references to Table of Contents
\newpage
\addcontentsline{toc}{chapter}{References}

\bibliographystyle{#1}
\bibliography{#2}  
}
}

% Alternate setup for use with bibitems

\newcommand{\uofsbibliographynobibtex}{
% We need to force single spacing for the bibliography which must be single
% spaced according to CGSR rules.  The line below changes spacing to single 
% space. This command does not take effect until a new font size is selected,
% so we add the \normalsize command to make the spacing take effect.

\renewcommand{\baselinestretch}{1}\normalsize

\renewcommand{\bibname}{References} 

\newpage
% Add references to Table of Contents
\addcontentsline{toc}{chapter}{References}
}

