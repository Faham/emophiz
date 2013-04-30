
#ifndef __TOBII_SDK_CPP_TIME_CLOCK_HPP__
#define __TOBII_SDK_CPP_TIME_CLOCK_HPP__

#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>


namespace tobii {

namespace sdk {

namespace cpp {

namespace time {

class clock {

private:
	boost::shared_ptr<tobii_sdk_clock_t> internal_clock;

public:
	typedef boost::shared_ptr<clock> pointer;

	clock()
	{
		tobii_sdk_error_t* error = 0;
		internal_clock.reset(tobii_sdk_clock_get_default(&error), &tobii_sdk_clock_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	int64_t get_time()
	{
		tobii_sdk_error_t* error = 0;
		int64_t time = tobii_sdk_clock_get_time(internal_clock.get(), &error);
		eyetracker_exception::throw_on_error(error);
		return time;
	}

	int64_t get_resolution()
	{
		tobii_sdk_error_t* error = 0;
		int64_t resolution = tobii_sdk_clock_get_resolution(internal_clock.get(), &error);
		eyetracker_exception::throw_on_error(error);
		return resolution;
	}

	tobii_sdk_clock_t* get_handle() const
	{
		return internal_clock.get();
	}

};

} // namespace time

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TIME_CLOCK_HPP__
