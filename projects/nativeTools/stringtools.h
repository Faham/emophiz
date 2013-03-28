#ifndef STRINGTOOLSSTRINGTOOLS_H
#define STRINGTOOLSSTRINGTOOLS_H

#include <string>

namespace stringtools
{
    std::string toAsciiString(const std::wstring &source);
    std::wstring toUnicodeString(const std::string &source);
}

#endif 
