
#ifndef __TOBII_SDK_CPP_UPGRADE_PROGRESS_REPORTER_HPP__
#define __TOBII_SDK_CPP_UPGRADE_PROGRESS_REPORTER_HPP__

#include <boost/shared_ptr.hpp>
#include <boost/signals2.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>


namespace tobii {

namespace sdk {

namespace cpp {

namespace upgrade {

class progress_reporter {

public:

	struct progress
	{
		uint32_t current_step;
		uint32_t number_of_steps;
		uint32_t percent;

		progress() :
			current_step(0),
			number_of_steps(0),
			percent(0)
		{

		}
	};

	typedef boost::shared_ptr<progress_reporter> pointer;
	typedef boost::signals2::signal<void (uint32_t)> upgrade_completed_event;
	typedef boost::signals2::signal<void (progress)> upgrade_progress_event;
	typedef boost::signals2::signal<void (bool)> can_cancel_changed_event;
	typedef boost::signals2::connection connection;

private:
	boost::shared_ptr<tobii_sdk_upgrade_progress_reporter_t> internal_progress_reporter;
	upgrade_completed_event upgrade_completed_sig;
	upgrade_progress_event upgrade_progress_sig;
	can_cancel_changed_event can_cancel_changed_sig;

public:

	progress_reporter()
	{
		tobii_sdk_error_t* error = 0;
		internal_progress_reporter.reset(
				tobii_sdk_upgrade_progress_reporter_create(
							&progress_reporter::completion_handler_trampoline,
							&progress_reporter::progress_handler_trampoline,
							&progress_reporter::can_cancel_changed_trampoline,
							this,
							&error
						),
				&tobii_sdk_upgrade_progress_reporter_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	bool can_cancel()
	{
		tobii_sdk_error_t* error = 0;
		uint32_t can_cancel = tobii_sdk_upgrade_progress_get_can_cancel(internal_progress_reporter.get(), &error);
		eyetracker_exception::throw_on_error(error);
		return can_cancel > 0;
	}

	bool cancel()
	{
		tobii_sdk_error_t* error = 0;
		uint32_t upgrade_cancelled = tobii_sdk_upgrade_progress_cancel(internal_progress_reporter.get(), &error);
		eyetracker_exception::throw_on_error(error);
		return upgrade_cancelled > 0;
	}

	progress get_progress()
	{
		progress progress;
		tobii_sdk_error_t* error = 0;
		tobii_sdk_upgrade_progress_reporter_get_progress(internal_progress_reporter.get(),
				&progress.current_step,
				&progress.number_of_steps,
				&progress.percent,
				&error);
		eyetracker_exception::throw_on_error(error);

		return progress;
	}

	connection add_upgrade_completed_listener(const upgrade_completed_event::slot_type& listener)
	{
		return upgrade_completed_sig.connect(listener);
	}

	connection add_progress_changed_listener(const upgrade_progress_event::slot_type& listener)
	{
		return upgrade_progress_sig.connect(listener);
	}

	connection add_can_cancel_changed_listener(const can_cancel_changed_event::slot_type& listener)
	{
		return can_cancel_changed_sig.connect(listener);
	}

	tobii_sdk_upgrade_progress_reporter_t* get_handle() const
	{
		return internal_progress_reporter.get();
	}


private:

	void handle_upgrade_completed(uint32_t error)
	{
		upgrade_completed_sig(error);
	}

	void handle_progress_changed(progress& progress)
	{
		upgrade_progress_sig(progress);
	}

	void handle_can_cancel_changed(bool can_cancel)
	{
		can_cancel_changed_sig(can_cancel);
	}

	static void completion_handler_trampoline(
			tobii_sdk_upgrade_progress_reporter_t*,
			tobii_sdk_error_t* error,
			void* user_data)
	{
		progress_reporter* self = (progress_reporter*)user_data;
		uint32_t error_code = tobii_sdk_error_get_code(error);
		self->handle_upgrade_completed(error_code);
	}

	static void progress_handler_trampoline(
			tobii_sdk_upgrade_progress_reporter_t*,
			uint32_t current_step,
			uint32_t number_of_steps,
			uint32_t percentage,
			void* user_data)
	{
		progress progress;
		progress.current_step = current_step;
		progress.number_of_steps = number_of_steps;
		progress.percent = percentage;

		progress_reporter* self = (progress_reporter*)user_data;
		self->handle_progress_changed(progress);
	}

	static void can_cancel_changed_trampoline(
			tobii_sdk_upgrade_progress_reporter_t*,
			uint32_t can_cancel,
			void* user_data
		)
	{
		progress_reporter* self = (progress_reporter*)user_data;
		self->handle_can_cancel_changed(can_cancel > 0);
	}

};

} // namespace upgrade

} // namespace cpp

} // namespace sdk

} // namespace tobii


#endif // __TOBII_SDK_CPP_UPGRADE_PROGRESS_REPORTER_HPP__
