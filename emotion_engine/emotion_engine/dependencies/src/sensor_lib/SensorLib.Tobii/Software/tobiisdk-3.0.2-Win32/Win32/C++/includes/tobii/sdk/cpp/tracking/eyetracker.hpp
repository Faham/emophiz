
#ifndef __TOBII_SDK_CPP_TRACKING_EYETRACKER_HPP__
#define __TOBII_SDK_CPP_TRACKING_EYETRACKER_HPP__

#include <vector>
#include <string>

#include <boost/shared_ptr.hpp>
#include <boost/scoped_ptr.hpp>
#include <boost/function.hpp>
#include <boost/signals2.hpp>
#include <boost/bind.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/async-result.hpp>
#include <tobii/sdk/cpp/tracking/param-stack.hpp>
#include <tobii/sdk/cpp/tracking/calibration.hpp>
#include <tobii/sdk/cpp/tracking/gaze-data-item.hpp>
#include <tobii/sdk/cpp/tracking/types.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace tracking {

class eyetracker {

public:
	typedef boost::shared_ptr<eyetracker> pointer;
	typedef boost::function<void (uint32_t)> async_callback;

	typedef boost::signals2::connection connection;
	typedef boost::signals2::signal<void (float)> framerate_changed_event;
	typedef boost::signals2::signal<void ()> empty_event;
	typedef boost::signals2::signal<void (gaze_data_item::pointer)> gaze_data_received_event;
	typedef boost::signals2::signal<void (const x_configuration&)> x_configuration_changed_event;
	typedef boost::signals2::signal<void (uint32_t)> connection_error_event;

private:
	typedef boost::function<void (tobii_sdk_opcode_t, tobii_sdk_error_code_t, param_stack&)> response_handler_function;
	typedef std::vector<boost::shared_ptr<tobii_sdk_callback_connection_t> > callback_connection_vector;

	boost::shared_ptr<tobii_sdk_message_passer_t> mp;
	callback_connection_vector callback_connections;

	framerate_changed_event framerate_changed_sig;
	empty_event calibration_started_sig;
	empty_event calibration_stopped_sig;
	gaze_data_received_event gaze_data_received_sig;
	empty_event headbox_changed_sig;
	x_configuration_changed_event x_configuration_changed_sig;
	connection_error_event connection_error_sig;

	struct response_handler_trampoline_data {
		response_handler_function handler;
	};

	struct empty_data{

	};

	static void response_handler_trampoline(
			tobii_sdk_opcode_t opcode,
			tobii_sdk_error_code_t error_code,
			const tobii_sdk_param_stack_t* params,
			void* user_data)
	{
		boost::scoped_ptr<response_handler_trampoline_data> trampoline_data((response_handler_trampoline_data*)user_data);

		param_stack stack(params);
		if(trampoline_data) {
			trampoline_data->handler(opcode, error_code, stack);
		}
	}

	void execute(uint32_t opcode, const param_stack& stack, const response_handler_function& handler)
	{
		response_handler_trampoline_data* trampoline_data = new response_handler_trampoline_data;
		trampoline_data->handler = handler;

		tobii_sdk_error_t* error = 0;
		tobii_sdk_message_passer_execute_request(mp.get(),
				opcode,
				stack.get_handle(),
				&eyetracker::response_handler_trampoline,
				trampoline_data,
				&error);
		eyetracker_exception::throw_on_error(error);
	}

	void handle_empty_response(
			tobii_sdk_error_code_t error,
			async_result<empty_data>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			result->set_result(empty_data());
		}
	}

	static void handle_data_callback(tobii_sdk_opcode_t opcode,
			tobii_sdk_error_code_t,
			const tobii_sdk_param_stack_t* params,
			void* user_data)
	{
		eyetracker* self = (eyetracker*)user_data;
		param_stack stack(params);

		switch (opcode) {
			case 1640:
				self->handle_framerate_changed(stack);
				break;
			case 1040:
				self->handle_calibration_started();
				break;
			case 1050:
				self->handle_calibration_stopped();
				break;
			case 1280:
				self->handle_gaze_data_received(stack);
				break;
			case 1410:
				self->handle_headbox_changed();
				break;
			case 1450:
				self->handle_xconfig_changed(stack);
				break;
			default:
				break;
		}
	}

	static void handle_error_callback(tobii_sdk_error_code_t error, void* user_data)
	{
		eyetracker* self = (eyetracker*)user_data;
		self->handle_error_event(error);
	}

	void handle_framerate_changed(param_stack& stack)
	{
		float framerate = stack.get_fixed_15x16_as_float32(0);
		framerate_changed_sig(framerate);
	}

	void handle_calibration_started()
	{
		calibration_started_sig();
	}

	void handle_calibration_stopped()
	{
		calibration_stopped_sig();
	}

	void handle_gaze_data_received(param_stack& stack)
	{
		gaze_data_item::pointer data = gaze_data_from_param_stack(stack);
		if(data)
		{
			gaze_data_received_sig(data);
		}
	}

	void handle_headbox_changed()
	{
		headbox_changed_sig();
	}

	void handle_xconfig_changed(param_stack& stack)
	{
		x_configuration config;
		uint32_t index = 0;
		config.upper_left = stack.read_point_3d(index);
		config.upper_right = stack.read_point_3d(index);
		config.lower_left = stack.read_point_3d(index);
		// Ignore tool data

		x_configuration_changed_sig(config);
	}

	void handle_error_event(uint32_t error_code)
	{
		connection_error_sig(error_code);
	}


public:

	eyetracker(tobii_sdk_message_passer_t* message_passer) :
		mp(message_passer, tobii_sdk_message_passer_destroy)
	{
		tobii_sdk_error_t* error = 0;

		boost::shared_ptr<tobii_sdk_callback_connection_t> framerate_changed_conn(
				tobii_sdk_message_passer_add_data_handler(mp.get(), 1640, &eyetracker::handle_data_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		boost::shared_ptr<tobii_sdk_callback_connection_t> calibration_started_conn(
				tobii_sdk_message_passer_add_data_handler(mp.get(), 1040, &eyetracker::handle_data_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		boost::shared_ptr<tobii_sdk_callback_connection_t> calibration_stopped_conn(
				tobii_sdk_message_passer_add_data_handler(mp.get(), 1050, &eyetracker::handle_data_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		boost::shared_ptr<tobii_sdk_callback_connection_t> gazedata_received_conn(
				tobii_sdk_message_passer_add_data_handler(mp.get(), 1280, &eyetracker::handle_data_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		boost::shared_ptr<tobii_sdk_callback_connection_t> headbox_changed_conn(
				tobii_sdk_message_passer_add_data_handler(mp.get(), 1410, &eyetracker::handle_data_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		boost::shared_ptr<tobii_sdk_callback_connection_t> xconfig_changed_conn(
				tobii_sdk_message_passer_add_data_handler(mp.get(), 1450, &eyetracker::handle_data_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		boost::shared_ptr<tobii_sdk_callback_connection_t> error_callback_conn(
				tobii_sdk_message_passer_add_error_handler(mp.get(), &eyetracker::handle_error_callback, this, &error),
				tobii_sdk_callback_connection_destroy);
		eyetracker_exception::throw_on_error(error);

		callback_connections.push_back(framerate_changed_conn);
		callback_connections.push_back(calibration_started_conn);
		callback_connections.push_back(calibration_stopped_conn);
		callback_connections.push_back(gazedata_received_conn);
		callback_connections.push_back(headbox_changed_conn);
		callback_connections.push_back(xconfig_changed_conn);
		callback_connections.push_back(error_callback_conn);
	}

	// Event subscription methods

	connection add_framerate_changed_listener(const framerate_changed_event::slot_type& listener)
	{
		return framerate_changed_sig.connect(listener);
	}

	connection add_calibration_started_listener(const empty_event::slot_type& listener)
	{
		return calibration_started_sig.connect(listener);
	}

	connection add_calibration_stopped_listener(const empty_event::slot_type& listener)
	{
		return calibration_stopped_sig.connect(listener);
	}

	connection add_gaze_data_received_listener(const gaze_data_received_event::slot_type& listener)
	{
		return gaze_data_received_sig.connect(listener);
	}

	connection add_headbox_changed_listener(const empty_event::slot_type& listener)
	{
		return headbox_changed_sig.connect(listener);
	}

	connection add_xconfig_changed_listener(const x_configuration_changed_event::slot_type& listener)
	{
		return x_configuration_changed_sig.connect(listener);
	}

	connection add_connection_error_listener(const connection_error_event::slot_type& listener)
	{
		return connection_error_sig.connect(listener);
	}


	// Eyetracker interface

	authorize_challenge get_authorize_challenge(uint32_t realm_id, const std::vector<uint32_t> algorithms)
	{
		async_result<authorize_challenge>::pointer result(new async_result<authorize_challenge>());
		param_stack stack;
		stack.push_uint32(realm_id);
		stack.push_vector(algorithms);

		execute(1900, stack, boost::bind(&eyetracker::handle_get_authorize_challenge_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_authorize_challenge_response(
				tobii_sdk_error_code_t error,
				param_stack& stack,
				async_result<authorize_challenge>::pointer result)
		{
			if(error)
			{
				result->set_error(error);
			}
			else
			{
				try {
					authorize_challenge challenge;

					challenge.realm_id = stack.get_uint32(0);
					challenge.algorithm = stack.get_uint32(1);
					challenge.challenge_data = stack.get_blob(2);

					result->set_result(challenge);
				}
				catch (eyetracker_exception& ex) {
					result->set_error(ex.get_error_code());
				}
			}
		}

public:
	void validate_challenge_response(uint32_t realm_id, uint32_t algorithm, blob& response_data)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_uint32(realm_id);
		stack.push_uint32(algorithm);
		stack.push_blob(response_data.get_data().get(), response_data.get_length());

		execute(1910, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	std::vector<float> enumerate_framerates()
	{
		async_result<std::vector<float> >::pointer result(new async_result<std::vector<float> >());
		param_stack stack;

		execute(1630, stack, boost::bind(&eyetracker::handle_enumerate_framerates_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_enumerate_framerates_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<std::vector<float> >::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				std::vector<float> framerates(stack.get_vector_float(0));
				result->set_result(framerates);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}
public:

	void set_framerate(float framerate)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		stack.push_float32_as_fixed_15x16(framerate);
		execute(1620, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	float get_framerate()
	{
		async_result<float>::pointer result(new async_result<float>());
		param_stack stack;

		execute(1610, stack, boost::bind(&eyetracker::handle_get_framerate_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

	void handle_get_framerate_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<float>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				float framerate = stack.get_fixed_15x16_as_float32(0);
				result->set_result(framerate);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

	uint32_t get_lowblink_mode()
	{
		async_result<uint32_t>::pointer result(new async_result<uint32_t>());
		param_stack stack;

		execute(1920, stack, boost::bind(&eyetracker::handle_get_lowblink_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_lowblink_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<uint32_t>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				uint32_t lowblink = stack.get_uint32(0);
				result->set_result(lowblink);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void set_lowblink_mode(uint32_t enabled)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_uint32(enabled);

		execute(1930, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void dump_images(uint32_t count, uint32_t frequency)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_uint32(count);
		stack.push_uint32(frequency);

		execute(1500, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	blob get_diagnostics_report(uint32_t include_images)
	{
		async_result<blob>::pointer result(new async_result<blob>());
		param_stack stack;
		stack.push_uint32(include_images);

		execute(1510, stack, boost::bind(&eyetracker::handle_get_diagnostics_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_diagnostics_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<blob>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				blob data = stack.get_blob(0);
				result->set_result(data);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void set_unit_name(const std::string& name)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		stack.push_string(name);
		execute(1710, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	std::string get_unit_name()
	{
		async_result<std::string>::pointer result(new async_result<std::string>());
		param_stack stack;

		execute(1700, stack, boost::bind(&eyetracker::handle_get_unit_name_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_unit_name_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<std::string>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				std::string name = stack.get_string(0);
				result->set_result(name);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	unit_info::pointer get_unit_info()
	{
		async_result<unit_info::pointer>::pointer result(new async_result<unit_info::pointer>());
		param_stack stack;

		execute(1420, stack, boost::bind(&eyetracker::handle_get_unit_info_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_unit_info_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<unit_info::pointer>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				unit_info::pointer info(new unit_info);
				info->serial_number = stack.get_string(0);
				info->model = stack.get_string(1);
				info->generation = stack.get_string(2);
				info->firmware_version = stack.get_string(3);
				result->set_result(info);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	payperuse_info get_payperuse_info()
	{
		async_result<payperuse_info>::pointer result(new async_result<payperuse_info>());
		param_stack stack;

		execute(1600, stack, boost::bind(&eyetracker::handle_get_payperuse_info_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_payperuse_info_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<payperuse_info>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				payperuse_info p_info;
				p_info.enabled = stack.get_uint32(0) > 0;
				p_info.realm = stack.get_uint32(1);
				p_info.authorized = stack.get_uint32(2) > 0;
				result->set_result(p_info);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void start_calibration()
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		execute(1010, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void stop_calibration()
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		execute(1020, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void clear_calibration()
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		execute(1060, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void add_calibration_point(const point_2d& point)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_float64_as_fixed_22x41(point.x);
		stack.push_float64_as_fixed_22x41(point.y);
		stack.push_uint32(3);

		execute(1030, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void add_calibration_point_async(const point_2d& point, const async_callback& completed_handler)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_float64_as_fixed_22x41(point.x);
		stack.push_float64_as_fixed_22x41(point.y);
		stack.push_uint32(3);

		execute(1030, stack, boost::bind(completed_handler, _2));
	}


	void remove_calibration_point(const point_2d& point)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_float64_as_fixed_22x41(point.x);
		stack.push_float64_as_fixed_22x41(point.y);
		stack.push_uint32(3);

		execute(1080, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void compute_calibration()
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		execute(1070, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void compute_calibration_async(const async_callback& completed_handler)
	{
		param_stack stack;
		execute(1070, stack, boost::bind(completed_handler, _2));
	}


	calibration get_calibration()
	{
		async_result<calibration>::pointer result(new async_result<calibration>());
		param_stack stack;

		execute(1100, stack, boost::bind(&eyetracker::handle_get_calibration_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_calibration_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<calibration>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {

				blob raw_data = stack.get_blob(0);
				calibration calib(raw_data);

				result->set_result(calib);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void set_calibration(calibration& calibration)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		blob& raw_data = calibration.get_raw_data();
		stack.push_blob(raw_data.get_data().get(), raw_data.get_length());

		execute(1110, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void start_tracking()
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		std::vector<uint32_t> empty_vec;

		param_stack stack;
		stack.push_uint32(1280); // 1280 is the gaze data stream
		stack.push_vector(empty_vec);
		execute(1220, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	void stop_tracking()
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_uint32(1280); // 1280 is the gaze data stream

		execute(1230, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	head_movement_box::pointer get_head_movement_box()
	{
		async_result<head_movement_box::pointer>::pointer result(new async_result<head_movement_box::pointer>());
		param_stack stack;

		execute(1400, stack, boost::bind(&eyetracker::handle_get_head_movement_box_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_head_movement_box_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<head_movement_box::pointer>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				head_movement_box::pointer box(new head_movement_box);
				uint32_t index = 0;

				box->point_1 = stack.read_point_3d(index);
				box->point_2 = stack.read_point_3d(index);
				box->point_3 = stack.read_point_3d(index);
				box->point_4 = stack.read_point_3d(index);

				box->point_5 = stack.read_point_3d(index);
				box->point_6 = stack.read_point_3d(index);
				box->point_7 = stack.read_point_3d(index);
				box->point_8 = stack.read_point_3d(index);

				result->set_result(box);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void enable_extension(uint32_t extension_id)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;
		stack.push_uint32(extension_id);

		execute(1800, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	extension::vector_pointer get_available_extensions()
	{
		async_result<extension::vector_pointer>::pointer result(new async_result<extension::vector_pointer>());
		param_stack stack;

		execute(1810, stack, boost::bind(&eyetracker::handle_extension_vector_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

	extension::vector_pointer get_enabled_extensions()
	{
		async_result<extension::vector_pointer>::pointer result(new async_result<extension::vector_pointer>());
		param_stack stack;

		execute(1820, stack, boost::bind(&eyetracker::handle_extension_vector_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_extension_vector_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<extension::vector_pointer>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				extension::vector_pointer vector(new std::vector<extension>());

				uint32_t index = 0;
				node_prolog prolog = stack.get_node_prolog(index);
				uint32_t length = prolog.get_length() - 1;
				index++; // Skip element type
				for(uint32_t i=0; i<length; i++)
				{
					extension ext;
					ext.protocol_version = stack.get_uint32(index++);
					ext.extension_id = stack.get_uint32(index++);
					ext.name = stack.get_string(index++);
					ext.realm = stack.get_uint32(index++);
					vector->push_back(ext);
				}


				result->set_result(vector);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void set_x_configuration(const x_configuration& configuration)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		stack.push_point_3d(configuration.upper_left);
		stack.push_point_3d(configuration.upper_right);
		stack.push_point_3d(configuration.lower_left);

		// Create list (id 256) of empty tool data (id 12345)
		node_prolog tool_data_prolog(1, 256);
		stack.push_node_prolog(tool_data_prolog);
		stack.push_uint32(12345);

		execute(1440, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	x_configuration::pointer get_x_configuration()
	{
		async_result<x_configuration::pointer>::pointer result(new async_result<x_configuration::pointer>());
		param_stack stack;

		execute(1430, stack, boost::bind(&eyetracker::handle_get_x_configuration_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_x_configuration_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<x_configuration::pointer>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				x_configuration::pointer configuration(new x_configuration);
				uint32_t index = 0;
				configuration->upper_left = stack.read_point_3d(index);
				configuration->upper_right = stack.read_point_3d(index);
				configuration->lower_left = stack.read_point_3d(index);

				result->set_result(configuration);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	std::string get_illumination_mode()
	{
		async_result<std::string>::pointer result(new async_result<std::string>());
		param_stack stack;

		execute(2010, stack, boost::bind(&eyetracker::handle_get_illumination_mode_response, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_get_illumination_mode_response(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<std::string>::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				std::string mode = stack.get_string(0);
				result->set_result(mode);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

public:
	void set_illumination_mode(const std::string& illumination_mode)
	{
		async_result<empty_data>::pointer result(new async_result<empty_data>());
		param_stack stack;

		stack.push_string(illumination_mode);
		execute(2020, stack, boost::bind(&eyetracker::handle_empty_response, this, _2, result));

		result->wait_for_result();
		result->throw_if_error();
	}

	std::vector<std::string> enumerate_illumination_modes()
	{
		async_result<std::vector<std::string> >::pointer result(new async_result<std::vector<std::string> >());
		param_stack stack;

		execute(2030, stack, boost::bind(&eyetracker::handle_enumerate_illumination_modes, this, _2, _3, result));

		result->wait_for_result();
		result->throw_if_error();

		return result->get_result();
	}

private:
	void handle_enumerate_illumination_modes(
			tobii_sdk_error_code_t error,
			param_stack& stack,
			async_result<std::vector<std::string> >::pointer result)
	{
		if(error)
		{
			result->set_error(error);
		}
		else
		{
			try {
				std::vector<std::string> modes(stack.get_vector_string(0));
				result->set_result(modes);
			}
			catch (eyetracker_exception& ex) {
				result->set_error(ex.get_error_code());
			}
		}
	}

};

} // namespace tracking

} // namespace cpp

} // namespace sdk

} // namespace tobii


#endif  // __TOBII_SDK_CPP_TRACKING_EYETRACKER_HPP__
