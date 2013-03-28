#ifndef NATIVE2MANAGEDMANAGEDEXCEPTION_H
#define NATIVE2MANAGEDMANAGEDEXCEPTION_H

#include "anyType.h"

namespace nativeAdapter
{

class ManagedException : public std::exception
{
public:
    ManagedException(const stdtstring &message);
    virtual ~ManagedException();    
    virtual const char *what() const;

private:
    std::string message_;
};

}

#endif
