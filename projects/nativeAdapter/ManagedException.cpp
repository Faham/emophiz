#include "stdafx.h"
#include "ManagedException.h"
#include "stringtools.h"

namespace nativeAdapter
{

ManagedException::ManagedException(const stdtstring &message)
{
#ifdef _UNICODE
    message_ = stringtools::toAsciiString(message);
#else
    message_ = message;
#endif
}

ManagedException::~ManagedException()
{
}

const char *ManagedException::what() const
{
    return message_.c_str();
}

}
