#ifndef MANAGEDSTATICPROXY_H
#define MANAGEDSTATICPROXY_H

#include "ManagedProxyBase.h"
#include "stringtools.h"

#include "tchar.h"
#include <string>

ref class ManagedStaticProxy : public ManagedProxyBase
{
public:
    ManagedStaticProxy(const stdtstring &dllName, const stdtstring &managedTypeName);

protected:
    virtual System::Type^ getManagedType() override;
    virtual System::Object^ invokeFunction(System::String^ functionName, cli::array<System::Object^>^ params, System::Reflection::BindingFlags bindingFlags) override;

private:
    System::Type^ managedType_;
};

#endif
