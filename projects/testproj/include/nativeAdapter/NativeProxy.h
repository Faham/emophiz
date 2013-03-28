#ifndef NATIVE2MANAGEDMANAGEDWRAPPER_H
#define NATIVE2MANAGEDMANAGEDWRAPPER_H

#include "NativeProxyBase.h"

namespace nativeAdapter
{

class NativeProxy : public NativeProxyBase
{
public:
    NativeProxy(const stdtstring &dllName, const stdtstring &className);
    NativeProxy(const stdtstring &dllName, const stdtstring &className, const AnyTypeArray &ctorParams);
    virtual ~NativeProxy();

public:
    void executeManaged(const stdtstring &function);
    void executeManaged(const stdtstring &function, AnyTypeArray &parameters);
    void executeManaged(const stdtstring &function, AnyType &result);
    void executeManaged(const stdtstring &function, AnyTypeArray &parameters, AnyType &result);

    void operator()(const stdtstring &function);
    void operator()(const stdtstring &function, AnyTypeArray &parameters);
    void operator()(const stdtstring &function, AnyType &result);
    void operator()(const stdtstring &function, AnyTypeArray &parameters, AnyType &result);

    void set(const stdtstring &property, const AnyType &value);
    void get(const stdtstring &property, AnyType &value);

private:
    NativeProxy(const NativeProxy &);
    NativeProxy &operator=(const NativeProxy &);
    struct impl;
    impl *pimpl_;
};

}

#endif
