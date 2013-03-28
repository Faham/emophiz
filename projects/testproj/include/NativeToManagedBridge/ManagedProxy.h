#ifndef MANAGEDPROXY_H
#define MANAGEDPROXY_H

#include "ManagedProxyBase.h"
#include "stringtools.h"
#include "anyHelper.h"

ref class ManagedProxy : public ManagedProxyBase
{
public:
    ManagedProxy(void *ptr2ManagedClass);

    static ManagedProxy^ create(const stdtstring &dllName, const stdtstring &managedTypeName, const AnyTypeArray &ctorParams);    
    void release();
    void *nativePointer();

protected:
    virtual System::Object^ invokeFunction(System::String^ functionName, cli::array<System::Object^>^ params, System::Reflection::BindingFlags bindingFlags) override;
    virtual System::Type^ getManagedType() override;

private:
    ManagedProxy(System::Object^ instance);
    System::Object^ instance();
    void *nativePointerFromManagedObject(System::Object^ instance);

private:
    void *ptr2ManagedObject_;
};

#endif
