#include "stdafx.h"
#include "stringtools.h"
#include "windows.h"

namespace stringtools
{

std::string toAsciiString(const std::wstring &source)
{
    int sourceLen = static_cast<int>(source.size());
    int requiredCharacters = WideCharToMultiByte(GetACP(), 0, source.c_str(), sourceLen, 0, 0, 0, 0);
    char *buffer = new char[requiredCharacters + 1];
    int writtenCharacters = WideCharToMultiByte(GetACP(), 0, source.c_str(), sourceLen, buffer, requiredCharacters, 0, 0);    
    buffer[writtenCharacters] = 0;
    std::string result(buffer);  
    delete[] buffer;
    return result;
}

std::wstring toUnicodeString(const std::string &source)
{
    int sourceLen = static_cast<int>(source.size());
    int requiredCharacters = MultiByteToWideChar (GetACP(), 0, source.c_str(), sourceLen, 0, 0);
    wchar_t *buffer = new wchar_t[requiredCharacters + 1];
    int writtenCharacters = MultiByteToWideChar(GetACP(), 0, source.c_str(), sourceLen, buffer, requiredCharacters);    
    buffer[writtenCharacters] = 0;
    std::wstring result(buffer);  
    delete[] buffer;
    return result;
}

}
