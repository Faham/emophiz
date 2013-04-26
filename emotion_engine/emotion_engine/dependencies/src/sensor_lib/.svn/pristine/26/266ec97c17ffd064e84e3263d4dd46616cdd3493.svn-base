
#ifndef __TOBII_SDK_CPP_TIME_SYNC_MANAGER_HPP__
#define __TOBII_SDK_CPP_TIME_SYNC_MANAGER_HPP__

#include <boost/shared_ptr.hpp>
#include <boost/signals2.hpp>

#include <tobii/sdk/core.h>

#include <tobii/sdk/cpp/mainloop.hpp>
#include <tobii/sdk/cpp/time/clock.hpp>
#include <tobii/sdk/cpp/time/sync-state.hpp>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/discovery/factory-info.hpp>


namespace tobii {

namespace sdk {

namespace cpp {

namespace time {

class sync_manager {

public:
	typedef boost::shared_ptr<sync_manager> pointer;
	typedef boost::signals2::signal<void (sync_state::pointer)> sync_state_changed_event;
	typedef boost::signals2::connection connection;

private:
	boost::shared_ptr<tobii_sdk_sync_manager_t> internal_sync_manager;
	sync_state_changed_event sync_state_changed_sig;

public:

	sync_manager(const mainloop& mainloop, const clock& clock, const discovery::factory_info& factory_info)
	{
		tobii_sdk_error_t* error = 0;
		internal_sync_manager.reset(
				tobii_sdk_sync_manager_create(
							clock.get_handle(),
							factory_info.get_handle(),
							mainloop.get_handle(),
							&sync_manager::sync_manager_error_handler_trampoline,
							this,
							&sync_state_changed_trampoline,
							this,
							&error
						),
				&tobii_sdk_sync_manager_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	int64_t local_to_remote(int64_t local_time)
	{
		tobii_sdk_error_t* error = 0;
		int64_t remote = tobii_sdk_sync_manager_convert_from_local_to_remote(internal_sync_manager.get(), local_time, &error);
		eyetracker_exception::throw_on_error(error);
		return remote;
	}

	int64_t remote_to_local(int64_t remote_time)
	{
		tobii_sdk_error_t* error = 0;
		int64_t local = tobii_sdk_sync_manager_convert_from_remote_to_local(internal_sync_manager.get(), remote_time, &error);
		eyetracker_exception::throw_on_error(error);
		return local;
	}

	sync_state::pointer get_sync_state()
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<tobii_sdk_sync_state_t> current_state(
				tobii_sdk_sync_manager_get_sync_state(internal_sync_manager.get(), &error),
				tobii_sdk_sync_state_destroy);
		eyetracker_exception::throw_on_error(error);

		sync_state::pointer state(new sync_state(current_state));
		return state;
	}

	connection add_sync_state_changed_listener(const sync_state_changed_event::slot_type& listener)
	{
		return sync_state_changed_sig.connect(listener);
	}


private:

	void handle_sync_state_changed(const tobii_sdk_sync_state_t* state)
	{
		sync_state::pointer wrapped_state(new sync_state(state));
		sync_state_changed_sig(wrapped_state);
	}

	static void sync_state_changed_trampoline(const tobii_sdk_sync_state_t* state, void* user_data)
	{
		sync_manager* self = (sync_manager*)user_data;
		self->handle_sync_state_changed(state);
	}

	static void sync_manager_error_handler_trampoline(tobii_sdk_error_code_t error, void* user_data)
	{
		// This handler is empty since the error event
		// never fires in the current implementation
	}
};

} // namespace time

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TIME_SYNC_MANAGER_HPP__
