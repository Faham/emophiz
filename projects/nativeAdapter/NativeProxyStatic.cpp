#include "StdAfx.h"
#include "NativeProxyStatic.h"

namespace nativeAdapter
{

struct NativeProxyStatic::Impl
{
    Impl(const stdtstring &dllName, const stdtstring &className)
        : managedDll(dllName)
        , managedClass(className)
    {
    }
    stdtstring managedDll, managedClass;
};

NativeProxyStatic::NativeProxyStatic(const stdtstring &dllName, const stdtstring &className)
    : pImpl_(new Impl(dllName, className))
{
}

NativeProxyStatic::~NativeProxyStatic(void)
{
    delete pImpl_;
}

void NativeProxyStatic::executeManaged(const stdtstring &function)
{
    AnyTypeArray parameters;
    executeManaged(function, parameters);
}

void NativeProxyStatic::executeManaged(const stdtstring &function, AnyTypeArray &parameters)
{
    AnyType result;
    executeManaged(function, parameters, result);
}

void NativeProxyStatic::executeManaged(const stdtstring &function, AnyType &result)
{
    AnyTypeArray parameters;
    executeManaged(function, parameters, result);
}

void NativeProxyStatic::executeManaged(const stdtstring &function, AnyTypeArray &parameters, AnyType &result)
{
    executeStatic(pImpl_->managedDll, pImpl_->managedClass, function, parameters, result);
}

void NativeProxyStatic::operator()(const stdtstring &function)
{
    executeManaged(function);
}

void NativeProxyStatic::operator()(const stdtstring &function, AnyTypeArray &parameters)
{
    executeManaged(function, parameters);
}

void NativeProxyStatic::operator()(const stdtstring &function, AnyType &result)
{
    executeManaged(function, result);
}

void NativeProxyStatic::operator()(const stdtstring &function, AnyTypeArray &parameters, AnyType &result)
{
    executeManaged(function, parameters, result);
}

void NativeProxyStatic::set(const stdtstring &property, const AnyType &value)
{
    setPropertyStatic(pImpl_->managedDll, pImpl_->managedClass, property, value);
}

void NativeProxyStatic::get(const stdtstring &property, AnyType &value)
{
    getPropertyStatic(pImpl_->managedDll, pImpl_->managedClass, property, value);
}

}
