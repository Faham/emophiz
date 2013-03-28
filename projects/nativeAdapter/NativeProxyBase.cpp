#include "StdAfx.h"
#include "NativeProxyBase.h"
#include "ManagedException.h"
#include "NativeToManagedBridge.h"

#include "windows.h"

#include <sstream>

namespace 
{
const TCHAR BridgeDll[] = _T("NativeToManagedBridge.dll");
const TCHAR ModuleSubDirectory[] = _T("netmodules");
}

namespace nativeAdapter
{

struct NativeProxyBase::Impl
{
    Impl(const stdtstring &dllfilename);
    ~Impl();

    void retrieveFunctionAddresses();
    void checkFunctionAddresses();
    void loadLibrary();
    void init();
    template<class FunctionPointer>
    FunctionPointer getAddress(const char * const functionName);

    stdtstring bridgeDllName;
    HMODULE bridgeDllHandle;
    CreateNetObject createNetObject;
    ReleaseNetObject releaseNetObject;
    CallNetObject callNetObject;
    GetNetObjectProperty getNetObjectProperty;
    SetNetObjectProperty setNetObjectProperty;
    CallNetClass callNetClass;
    GetNetClassProperty getNetClassProperty;
    SetNetClassProperty setNetClassProperty;
    bool valid;
};

NativeProxyBase::Impl::Impl(const stdtstring &dllfilename) 
    : bridgeDllName(dllfilename)
    , bridgeDllHandle(0)
    , createNetObject(0)
    , releaseNetObject(0)
    , callNetObject(0)
    , getNetObjectProperty(0)
    , setNetObjectProperty(0)
    , callNetClass(0)
    , getNetClassProperty(0)
    , setNetClassProperty(0)
    , valid(false)
{
    init();
}

NativeProxyBase::Impl::~Impl()
{
    if (bridgeDllHandle != 0)
        FreeLibrary(bridgeDllHandle);
}

void NativeProxyBase::Impl::retrieveFunctionAddresses()
{
    createNetObject = getAddress < CreateNetObject > ( CREATENETOBJECT );
    releaseNetObject = getAddress < ReleaseNetObject > ( RELEASENETOBJECT );    	
    callNetObject = getAddress < CallNetObject > ( CALLNETOBJECT );
    getNetObjectProperty = getAddress < GetNetObjectProperty > ( GETNETOBJECTPROPERTY );
    setNetObjectProperty = getAddress < SetNetObjectProperty > ( SETNETOBJECTPROPERTY );
    callNetClass = getAddress < CallNetClass > ( CALLNETCLASS );
    getNetClassProperty = getAddress < GetNetClassProperty > ( GETNETCLASSPROPERTY );
    setNetClassProperty = getAddress < SetNetClassProperty > ( SETNETCLASSPROPERTY );
}

template<class FunctionPointer>
FunctionPointer NativeProxyBase::Impl::getAddress(const char * const functionName)
{
    return reinterpret_cast<FunctionPointer>(GetProcAddress(bridgeDllHandle, functionName));
}

void NativeProxyBase::Impl::checkFunctionAddresses()
{
        if (!createNetObject 
                || !releaseNetObject
                || !callNetObject
                || !getNetObjectProperty
                || !setNetObjectProperty
                || !callNetClass
                || !getNetClassProperty
                || !setNetClassProperty
            )
    {
	    FreeLibrary(bridgeDllHandle);
        throw std::invalid_argument("Bridgefunction not found!");
    }
}

void NativeProxyBase::Impl::loadLibrary()
{
    if (bridgeDllHandle != 0)
        throw std::invalid_argument("Bridge library alread loaded!");
    
    stdtstring fileName(ModuleSubDirectory);

    fileName += _T("\\");
    fileName += bridgeDllName;

    bridgeDllHandle = ::LoadLibrary(fileName.c_str());
    if (!bridgeDllHandle)
        bridgeDllHandle = ::LoadLibrary(bridgeDllName.c_str());
	
    if (bridgeDllHandle == 0)
        throw std::invalid_argument("Cannot load bridge library!");
}

void NativeProxyBase::Impl::init()
{
    loadLibrary();
    retrieveFunctionAddresses();
    checkFunctionAddresses();
    valid = true;
}

NativeProxyBase::NativeProxyBase(void)
    : pImpl_(new Impl(BridgeDll))
{
}

NativeProxyBase::NativeProxyBase(const stdtstring &bridgeDllName)
    : pImpl_(new Impl(bridgeDllName))
{
}

NativeProxyBase::~NativeProxyBase(void)
{
    delete pImpl_;
}

bool NativeProxyBase::isValid() const
{
    return pImpl_->valid;
}

void *NativeProxyBase::createManagedClass(const stdtstring &assemblyFile, const stdtstring &className)
{
    AnyTypeArray args;
    return createManagedClass(assemblyFile, className, args);
}

void *NativeProxyBase::createManagedClass(const stdtstring &assemblyFile, const stdtstring &className, const AnyTypeArray &parameters)
{
    if (isValid())
    {
        std::wstring exceptionText;
        if (void *classPointer = pImpl_->createNetObject(assemblyFile, className, parameters, exceptionText))
            return classPointer;
        else
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::destroyManagedClass(void *classPointer)
{
    if (isValid())
    {
        std::wstring exceptionText;
        if (!pImpl_->releaseNetObject(classPointer, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::execute(void *classPointer, const stdtstring &functionName, AnyTypeArray &parameters, AnyType &result)
{
    if (isValid() && classPointer)
    {
        std::wstring exceptionText;
        if (!pImpl_->callNetObject(classPointer, functionName, parameters, result, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::setProperty(void *classPointer, const stdtstring &propertyName, const AnyType & value)
{
    if (isValid() && classPointer)
    {
        std::wstring exceptionText;
        if (!pImpl_->setNetObjectProperty(classPointer, propertyName, value, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::getProperty(void *classPointer, const stdtstring &propertyName, AnyType & value)
{
    if (isValid() && classPointer)
    {
        std::wstring exceptionText;
        if (!pImpl_->getNetObjectProperty(classPointer, propertyName, value, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::executeStatic(const stdtstring &assemblyFile, const stdtstring &className, const stdtstring &functionName, AnyTypeArray &parameters, AnyType &result)
{
    if (isValid())
    {
        std::wstring exceptionText;
        if (!pImpl_->callNetClass(assemblyFile, className, functionName, parameters, result, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::setPropertyStatic(const stdtstring &assemblyFile, const stdtstring &className, const stdtstring &propertyName, const AnyType & value)
{
    if (isValid())
    {
        std::wstring exceptionText;
        if (!pImpl_->setNetClassProperty(assemblyFile, className, propertyName, value, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

void NativeProxyBase::getPropertyStatic(const stdtstring &assemblyFile, const stdtstring &className, const stdtstring &propertyName, AnyType &value)
{
    if (isValid())
    {
        std::wstring exceptionText;
        if (!pImpl_->getNetClassProperty(assemblyFile, className, propertyName, value, exceptionText))
            throw ManagedException(exceptionText);
    }
    else
        throw std::invalid_argument("Not initialized!");
}

}
