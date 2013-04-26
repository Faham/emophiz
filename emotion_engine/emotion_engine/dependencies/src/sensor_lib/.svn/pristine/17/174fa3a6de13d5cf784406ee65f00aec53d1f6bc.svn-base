
#ifndef __TOBII_SDK_CPP_DISCOVERY_EYETRACKER_BROWSER_HPP__
#define __TOBII_SDK_CPP_DISCOVERY_EYETRACKER_BROWSER_HPP__

#include <string>
#include <boost/shared_ptr.hpp>
#include <boost/signals2.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/mainloop.hpp>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/discovery/eyetracker-info.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace discovery {

class eyetracker_browser {

public:
	enum event_type {TRACKER_FOUND, TRACKER_UPDATED, TRACKER_REMOVED};

	typedef boost::shared_ptr<eyetracker_browser> pointer;
	typedef boost::signals2::signal<void (eyetracker_browser::event_type, eyetracker_info::pointer)> browser_event;
	typedef boost::signals2::connection connection;

private:
	browser_event browser_sig;
	boost::shared_ptr<tobii_sdk_device_browser_t> browser;
	mainloop _mainloop;

public:
	eyetracker_browser(const mainloop& mainloop) :
		_mainloop(mainloop)
	{

	}

	void start()
	{
		if(!browser)
		{
			tobii_sdk_error_t* error = 0;
			browser.reset(tobii_sdk_device_browser_create(_mainloop.get_handle(),
						&eyetracker_browser::callback_trampoline,
						this,
						&error),
					&tobii_sdk_device_browser_destroy);
			eyetracker_exception::throw_on_error(error);
		}
	}

	void stop()
	{
		browser.reset();
	}

	connection add_browser_event_listener(const browser_event::slot_type& listener)
	{
		return browser_sig.connect(listener);
	}

private:

	void handle_eyetracker_info_event(event_type type,
			eyetracker_info::pointer et_info)
	{
		browser_sig(type, et_info);
	}


	static void callback_trampoline(tobii_sdk_on_device_browser_event_t event,
				tobii_sdk_device_info_t* device_info,
				void* user_data)
	{
		eyetracker_info::pointer et_info(new eyetracker_info(device_info));

		eyetracker_browser* self = (eyetracker_browser*)user_data;
		self->handle_eyetracker_info_event(static_cast<event_type>(event), et_info);
	}
};

} // namespace discovery

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif // __TOBII_SDK_CPP_DISCOVERY_EYETRACKER_BROWSER_HPP__
