#ifndef NATIVEPROXYBASE_H
#define NATIVEPROXYBASE_H

#include <string>
#include "stringtools.h"
#include "anyType.h"

namespace nativeAdapter
{

class NativeProxyBase
{
public:
    NativeProxyBase(void);
    explicit NativeProxyBase(const stdtstring &bridgeDllName);
    virtual ~NativeProxyBase(void);

    bool isValid() const;	
    void *createManagedClass(const stdtstring &assemblyFile, const stdtstring &className);
    void *createManagedClass(const stdtstring &assemblyFile, const stdtstring &className, const AnyTypeArray &parameters);
    void destroyManagedClass(void *classPointer);
    // instance 
    void execute(void *classPointer, const stdtstring &functionName, AnyTypeArray &parameters, AnyType &result);
    void setProperty(void *classPointer, const stdtstring &propertyName, const AnyType & value);
    void getProperty(void *classPointer, const stdtstring &propertyName, AnyType &value);
    // static 
    void executeStatic(const stdtstring &assemblyFile, const stdtstring &className, const stdtstring &functionName, AnyTypeArray &parameters, AnyType &result);
    void setPropertyStatic(const stdtstring &assemblyFile, const stdtstring &className, const stdtstring &propertyName, const AnyType & value);
    void getPropertyStatic(const stdtstring &assemblyFile, const stdtstring &className, const stdtstring &propertyName, AnyType &value);

private:
    NativeProxyBase(const NativeProxyBase&);
    NativeProxyBase &operator=(const NativeProxyBase&);

    struct Impl;
    Impl *pImpl_;
};

}

#endif
