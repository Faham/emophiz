#ifndef ANYHELPER_H
#define ANYHELPER_H

#include "anyType.h"

System::Object^ toObject(const AnyType &source);
void toAny(System::Object ^source, AnyType &destination);

#endif
