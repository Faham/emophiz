#ifndef MANAGEDWRAPPERSTATIC_H
#define MANAGEDWRAPPERSTATIC_H

#include "NativeProxyBase.h"

namespace nativeAdapter
{

class NativeProxyStatic : public NativeProxyBase
{
public:
    NativeProxyStatic(const stdtstring &dllName, const stdtstring &className);
    virtual ~NativeProxyStatic(void);

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
    NativeProxyStatic(const NativeProxyStatic &);
    NativeProxyStatic &operator=(const NativeProxyStatic &);
    struct Impl;
    Impl *pImpl_;
};

}

#endif
