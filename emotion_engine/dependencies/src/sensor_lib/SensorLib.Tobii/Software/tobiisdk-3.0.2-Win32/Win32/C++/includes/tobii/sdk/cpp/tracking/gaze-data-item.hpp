

#ifndef __TOBII_SDK_CPP_TRACKING_GAZEDATAITEM_HPP__
#define __TOBII_SDK_CPP_TRACKING_GAZEDATAITEM_HPP__

#include <string>
#include <vector>
#include <stdexcept>
#include <iostream>

#include <boost/shared_ptr.hpp>
#include <boost/shared_array.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/tracking/types.hpp>
#include <tobii/sdk/cpp/tracking/param-stack.hpp>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace tracking {

struct gaze_data_item {

public:
	typedef boost::shared_ptr<gaze_data_item> pointer;

	uint64_t time_stamp;

	point_3d left_eye_position_3d;
	point_3d right_eye_position_3d;

	uint32_t left_validity;
	uint32_t right_validity;

	point_3d left_eye_position_3d_relative;
	point_3d right_eye_position_3d_relative;

	point_3d left_gaze_point_3d;
	point_3d right_gaze_point_3d;

	point_2d left_gaze_point_2d;
	point_2d right_gaze_point_2d;

	float left_pupil_diameter;
	float right_pupil_diameter;
};


void parse_unknown_content(uint32_t& index, param_stack& stack);

inline void parse_gaze_data_column(uint32_t& index, param_stack& stack, gaze_data_item::pointer data)
{
	static const uint32_t column_id = 3001;

	static const uint32_t timestamp_id 				= 1;

	static const uint32_t left_eyepos_3d_id 		= 2;
	static const uint32_t left_eyepos_3d_rel_id 	= 3;
	static const uint32_t left_gaze_point_3d_id 	= 4;
	static const uint32_t left_gaze_point_2d_id 	= 5;
	static const uint32_t left_pupil_id 			= 6;
	static const uint32_t left_validity_id 			= 7;

	static const uint32_t right_eyepos_3d_id 		= 8;
	static const uint32_t right_eyepos_3d_rel_id 	= 9;
	static const uint32_t right_gaze_point_3d_id 	= 10;
	static const uint32_t right_gaze_point_2d_id 	= 11;
	static const uint32_t right_pupil_id 			= 12;
	static const uint32_t right_validity_id 		= 13;

	node_prolog column_prolog = stack.get_node_prolog(index++);
	if(column_prolog.get_id() != column_id)
		throw new std::runtime_error("unexpected data in param stack");
	if(column_prolog.get_length() != 2)
		throw new std::runtime_error("unexpected column data length in param stack");

	// Read column id
	uint32_t id = stack.get_uint32(index++);

	switch (id) {
		case timestamp_id:
			data->time_stamp = stack.get_int64(index++);
			break;

		case left_eyepos_3d_id:
			data->left_eye_position_3d = stack.read_point_3d(index);
			break;
		case left_eyepos_3d_rel_id:
			data->left_eye_position_3d_relative = stack.read_point_3d(index);
			break;
		case left_gaze_point_3d_id:
			data->left_gaze_point_3d = stack.read_point_3d(index);
			break;
		case left_gaze_point_2d_id:
			data->left_gaze_point_2d = stack.read_point_2d(index);
			break;
		case left_pupil_id:
			data->left_pupil_diameter = stack.get_fixed_15x16_as_float32(index++);
			break;
		case left_validity_id:
			data->left_validity = stack.get_uint32(index++);
			break;

		case right_eyepos_3d_id:
			data->right_eye_position_3d = stack.read_point_3d(index);
			break;
		case right_eyepos_3d_rel_id:
			data->right_eye_position_3d_relative = stack.read_point_3d(index);
			break;
		case right_gaze_point_3d_id:
			data->right_gaze_point_3d = stack.read_point_3d(index);
			break;
		case right_gaze_point_2d_id:
			data->right_gaze_point_2d = stack.read_point_2d(index);
			break;
		case right_pupil_id:
			data->right_pupil_diameter = stack.get_fixed_15x16_as_float32(index++);
			break;
		case right_validity_id:
			data->right_validity = stack.get_uint32(index++);
			break;

		default:
			parse_unknown_content(index, stack);
			break;
	}

}

inline void parse_unknown_content(uint32_t& index, param_stack& stack)
{
	if(stack.is_node_prolog(index))
	{
		node_prolog prolog = stack.get_node_prolog(index++);
		uint32_t num_children = prolog.get_length();
		for (uint32_t child = 0; child < num_children; ++child) {
			// Recurse further down
			parse_unknown_content(index, stack);
		}
	}
	else
	{
		// Skip item
		index++;
	}
}


inline gaze_data_item::pointer gaze_data_from_param_stack(param_stack& stack)
{
	static const uint32_t row_id = 3000;

	try
	{
		uint32_t index = 0;
		node_prolog row_prolog = stack.get_node_prolog(index++);

		gaze_data_item::pointer data(new gaze_data_item);

		if(row_prolog.get_id() != row_id)
			throw new std::runtime_error("unexpected data in param stack");

		for (uint32_t column = 0; column < row_prolog.get_length(); ++column) {
			parse_gaze_data_column(index, stack, data);
		}

		return data;
	}
	catch (const eyetracker_exception& ex) {
		std::cout << "Exception " << ex.what() << " " << ex.get_error_code() << std::endl;
		return gaze_data_item::pointer();
	}
	catch (const std::runtime_error& ex)
	{
		std::cout << "Exception " << ex.what() << std::endl;
		return gaze_data_item::pointer();
	}
}

} // namespace tracking

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TRACKING_GAZEDATAITEM_HPP__
