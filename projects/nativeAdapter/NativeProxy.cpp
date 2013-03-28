#include "StdAfx.h"
#include "NativeProxy.h"

namespace nativeAdapter
{

struct NativeProxy::impl
{
    impl(const stdtstring &dllName, const stdtstring &className)
        : managedDll(dllName)
        , managedClass(className)
        , managedClassPtr(0)
    {
    }
    stdtstring managedDll, managedClass;
    void *managedClassPtr;
};

NativeProxy::NativeProxy(const stdtstring &dllName, const stdtstring &className)
    : pimpl_(new impl(dllName, className))
{
    pimpl_->managedClassPtr = createManagedClass(dllName, className);
}

NativeProxy::NativeProxy(const stdtstring &dllName, const stdtstring &className, const AnyTypeArray &ctorParams)
    : pimpl_(new impl(dllName, className))
{
    pimpl_->managedClassPtr = createManagedClass(dllName, className, ctorParams);
}

NativeProxy::~NativeProxy()
{
    try
    {
        destroyManagedClass(pimpl_->managedClassPtr);
    }
    catch(...)
    {
    }
    delete pimpl_;
}

void NativeProxy::executeManaged(const stdtstring &function)
{
    AnyTypeArray parameters;
    executeManaged(function, parameters);
}

void NativeProxy::executeManaged(const stdtstring &function, AnyTypeArray &parameters)
{
    AnyType result;
    executeManaged(function, parameters, result);
}

void NativeProxy::executeManaged(const stdtstring &function, AnyType &result)
{
    AnyTypeArray parameters;
    executeManaged(function, parameters, result);
}

void NativeProxy::executeManaged(const stdtstring &function, AnyTypeArray &parameters, AnyType &result)
{
    execute(pimpl_->managedClassPtr, function, parameters, result);
}

void NativeProxy::operator()(const stdtstring &function)
{
    executeManaged(function);
}

void NativeProxy::operator()(const stdtstring &function, AnyTypeArray &parameters)
{
    executeManaged(function, parameters);
}

void NativeProxy::operator()(const stdtstring &function, AnyType &result)
{
    executeManaged(function, result);
}

void NativeProxy::operator()(const stdtstring &function, AnyTypeArray &parameters, AnyType &result)
{
    executeManaged(function, parameters, result);
}

void NativeProxy::set(const stdtstring &property, const AnyType &value)
{
    setProperty(pimpl_->managedClassPtr, property, value);
}

void NativeProxy::get(const stdtstring &property, AnyType &value)
{
    getProperty(pimpl_->managedClassPtr, property, value);
}

}
