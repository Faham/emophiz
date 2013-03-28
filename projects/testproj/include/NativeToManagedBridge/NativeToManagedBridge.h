#ifndef NATIVETOMANAGEDBRIDGE_H
#define NATIVETOMANAGEDBRIDGE_H

#include <string>
#include "anyType.h"
#include "stringtools.h"

#ifdef _UNICODE
#define CREATENETOBJECT "CreateNetObjectW"
#define CALLNETOBJECT "CallNetObjectW"
#define GETNETOBJECTPROPERTY "GetNetObjectPropertyW"
#define SETNETOBJECTPROPERTY "SetNetObjectPropertyW"
#define CALLNETCLASS "CallNetClassW"
#define GETNETCLASSPROPERTY "GetNetClassPropertyW"
#define SETNETCLASSPROPERTY "SetNetClassPropertyW"
#define RELEASENETOBJECT "ReleaseNetObjectW"
#else
#define CREATENETOBJECT "CreateNetObjectA"
#define CALLNETOBJECT "CallNetObjectA"
#define GETNETOBJECTPROPERTY "GetNetObjectPropertyA"
#define SETNETOBJECTPROPERTY "SetNetObjectPropertyA"
#define CALLNETCLASS "CallNetClassA"
#define GETNETCLASSPROPERTY "GetNetClassPropertyA"
#define SETNETCLASSPROPERTY "SetNetClassPropertyA"
#define RELEASENETOBJECT "ReleaseNetObjectA"
#endif 

extern "C"
{
    // create/release
    typedef void * ( *CreateNetObject ) (const stdtstring &dllFileName, const stdtstring &className, const AnyTypeArray &ctorParams, stdtstring &exceptionText);
    typedef bool ( *ReleaseNetObject ) (void *pointerToManagedObject, stdtstring &exceptionText);

    // members
    typedef bool ( *CallNetObject ) (void *pointerToManagedObject, const stdtstring &functionName, AnyTypeArray &inputParameters, AnyType &result, stdtstring &exceptionText);
    typedef bool ( *GetNetObjectProperty ) (void *pointerToManagedObject, const stdtstring &functionName, AnyType &result, stdtstring &exceptionText);
    typedef bool ( *SetNetObjectProperty ) (void *pointerToManagedObject, const stdtstring &functionName, const AnyType &value, stdtstring &exceptionText);

    // statics 
    typedef bool ( *CallNetClass ) (const stdtstring &dllFileName, const stdtstring &className, const stdtstring &functionName, AnyTypeArray &inputParameters, AnyType &result, stdtstring &exceptionText);
    typedef bool ( *GetNetClassProperty ) (const stdtstring &dllFileName, const stdtstring &className, const stdtstring &functionName, AnyType &result, stdtstring &exceptionText);
    typedef bool ( *SetNetClassProperty ) (const stdtstring &dllFileName, const stdtstring &className, const stdtstring &functionName, const AnyType &value, stdtstring &exceptionText);
}

#endif 
