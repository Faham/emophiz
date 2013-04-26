
#ifndef __TOBII_SDK_CPP_INIT_HPP__
#define __TOBII_SDK_CPP_INIT_HPP__

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

inline void init_library()
{
	tobii_sdk_error_t* error = 0;
	tobii_sdk_init(&error);
	eyetracker_exception::throw_on_error(error);
}

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif // __TOBII_SDK_CPP_INIT_HPP__
