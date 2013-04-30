
#ifndef __TOBII_SDK_CPP_MAINLOOP_HPP__
#define __TOBII_SDK_CPP_MAINLOOP_HPP__

#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

class mainloop {

private:
	boost::shared_ptr<tobii_sdk_mainloop_t> loop;

public:
	typedef boost::shared_ptr<mainloop> pointer;

	mainloop()
	{
		tobii_sdk_error_t* error = 0;
		loop.reset(tobii_sdk_mainloop_create(&error), tobii_sdk_mainloop_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	void run()
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_mainloop_run(loop.get(), &error);
		eyetracker_exception::throw_on_error(error);
	}

	void quit()
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_mainloop_quit(loop.get(), &error);
		eyetracker_exception::throw_on_error(error);
	}

	tobii_sdk_mainloop_t* get_handle() const
	{
		return loop.get();
	}
};


} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif // __TOBII_SDK_CPP_MAINLOOP_HPP__
