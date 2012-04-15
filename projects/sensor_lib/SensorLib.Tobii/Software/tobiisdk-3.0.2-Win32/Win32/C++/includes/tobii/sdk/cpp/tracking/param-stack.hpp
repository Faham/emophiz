
#ifndef __TOBII_SDK_CPP_PARAM_STACK_HPP__
#define __TOBII_SDK_CPP_PARAM_STACK_HPP__

#include <string>
#include <cstring>
#include <vector>
#include <boost/shared_ptr.hpp>
#include <boost/shared_array.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/tracking/types.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace tracking {

static const uint32_t id_mask = 0x0000FFFF;
static const uint32_t length_mask = 0x0FFF0000;
static const int32_t length_shift = 16;

class node_prolog {
private:
	uint32_t value;

public:
	node_prolog(uint32_t val)
	{
		value = val;
	}

	node_prolog(uint32_t length, uint32_t id)
	{
		value = 0;
		value |= (id & id_mask);
		value |= (length << length_shift) & length_mask;
	}

	uint32_t get_id() const
	{
		return (value & id_mask);
	}

	uint32_t get_length() const
	{
		return (value & length_mask) >> length_shift;
	}

	uint32_t get_value() const
	{
		return value;
	}

};

class param_stack {

private:
	boost::shared_ptr<tobii_sdk_param_stack_t> stack;

public:

	param_stack()
	{
		tobii_sdk_error_t* error = 0;
		stack.reset(tobii_sdk_param_stack_create(&error), &tobii_sdk_param_stack_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	param_stack(const tobii_sdk_param_stack_t* p_stack)
	{
		tobii_sdk_error_t* error = 0;
		stack.reset(tobii_sdk_param_stack_clone(p_stack, &error), &tobii_sdk_param_stack_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	tobii_sdk_param_stack_t* get_handle() const
	{
		return stack.get();
	}

	void push_int32 (int32_t value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_int32(stack.get(), value, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_uint32 (uint32_t value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_uint32(stack.get(), value, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_int64 (int64_t value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_int64(stack.get(), value, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_uint64 (uint64_t value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_uint64(stack.get(), value, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_float32_as_fixed_15x16 (float value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_float32_as_fixed_15x16(stack.get(), value, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_float64_as_fixed_22x41 (double value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_float64_as_fixed_22x41(stack.get(), value, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_node_prolog (node_prolog value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_node_prolog(stack.get(), value.get_value(), &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_string (const std::string& value)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_string(stack.get(), value.c_str(), value.length(), &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_blob (const unsigned char* blob, uint32_t blob_length)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_stack_push_blob(stack.get(), blob, blob_length, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_point_3d(const point_3d& point)
	{
		node_prolog prolog(3, 8001);
		tobii_sdk_error_t* error = 0;

		tobii_sdk_param_stack_push_node_prolog(stack.get(), prolog.get_value(), &error);
		eyetracker_exception::throw_on_error(error);
		tobii_sdk_param_stack_push_float64_as_fixed_22x41(stack.get(), point.x, &error);
		eyetracker_exception::throw_on_error(error);
		tobii_sdk_param_stack_push_float64_as_fixed_22x41(stack.get(), point.y, &error);
		eyetracker_exception::throw_on_error(error);
		tobii_sdk_param_stack_push_float64_as_fixed_22x41(stack.get(), point.z, &error);
		eyetracker_exception::throw_on_error(error);
	}

	void push_vector(const std::vector<uint32_t>& vector)
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<tobii_sdk_vector_t> p_vec(
				tobii_sdk_vector_create(vector.size(), TOBII_SDK_PARAM_TYPE_UINT32, &error),
				&tobii_sdk_vector_destroy);
		eyetracker_exception::throw_on_error(error);

		for(size_t i=0; i<vector.size(); i++)
		{
			tobii_sdk_vector_set_uint32(p_vec.get(), i, vector[i], &error);
			eyetracker_exception::throw_on_error(error);
		}

		tobii_sdk_param_stack_push_vector(stack.get(), p_vec.get(), &error);
		eyetracker_exception::throw_on_error(error);
	}

	int32_t get_int32 (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		int32_t result =  tobii_sdk_param_stack_get_int32(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return result;
	}

	uint32_t get_uint32 (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		uint32_t result = tobii_sdk_param_stack_get_uint32(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return result;
	}

	int64_t get_int64 (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		int64_t result = tobii_sdk_param_stack_get_int64(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return result;
	}

	uint64_t get_uint64 (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		uint64_t result = tobii_sdk_param_stack_get_uint64(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return result;
	}

	float get_fixed_15x16_as_float32 (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		float result = tobii_sdk_param_stack_get_fixed_15x16_as_float32(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return result;
	}

	double get_fixed_22x41_as_float64 (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		double result = tobii_sdk_param_stack_get_fixed_22x41_as_float64(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return result;
	}

	bool is_node_prolog(uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		tobii_sdk_param_type_t type = tobii_sdk_param_stack_get_type(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return type == TOBII_SDK_PARAM_TYPE_NODE_PROLOG;
	}

	node_prolog get_node_prolog (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		uint32_t result = tobii_sdk_param_stack_get_node_prolog(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		return node_prolog(result);
	}

	std::string get_string (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		const char* c_str = tobii_sdk_param_stack_get_string(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		std::string result(c_str);
		tobii_sdk_free_string(c_str);
		return result;
	}

	blob get_blob (uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		const unsigned char* c_blob = tobii_sdk_param_stack_get_blob(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);
		uint32_t blob_length = tobii_sdk_param_stack_get_blob_length(stack.get(), index, &error);
		eyetracker_exception::throw_on_error(error);

		boost::shared_array<uint8_t> result(new uint8_t[blob_length]);
		std::memcpy(result.get(), c_blob, blob_length);
		return blob(result, blob_length);
	}

	std::vector<float> get_vector_float(uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<tobii_sdk_vector_t> vec(tobii_sdk_param_stack_get_vector(stack.get(), index, &error),
				tobii_sdk_vector_destroy);
		eyetracker_exception::throw_on_error(error);

		uint32_t length = tobii_sdk_vector_get_length(vec.get(), &error);
		eyetracker_exception::throw_on_error(error);

		std::vector<float> return_vec;
		for (uint32_t i = 0; i < length; ++i) {
			float f = tobii_sdk_vector_get_fixed_15x16_as_float32(vec.get(), i, &error);
			eyetracker_exception::throw_on_error(error);
			return_vec.push_back(f);
		}

		return return_vec;
	}

	std::vector<std::string> get_vector_string(uint32_t index)
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<tobii_sdk_vector_t> vec(tobii_sdk_param_stack_get_vector(stack.get(), index, &error),
				tobii_sdk_vector_destroy);
		eyetracker_exception::throw_on_error(error);

		uint32_t length = tobii_sdk_vector_get_length(vec.get(), &error);
		eyetracker_exception::throw_on_error(error);

		std::vector<std::string> return_vec;
		for (uint32_t i = 0; i < length; ++i) {
			const char* c_str = tobii_sdk_vector_get_string(vec.get(), i, &error);
			eyetracker_exception::throw_on_error(error);
			return_vec.push_back(std::string(c_str));
			tobii_sdk_free_string(c_str);
		}

		return return_vec;
	}


	point_3d read_point_3d(uint32_t& index)
	{
		tobii_sdk_error_t* error = 0;
		node_prolog prolog(tobii_sdk_param_stack_get_node_prolog(stack.get(), index++, &error));
		eyetracker_exception::throw_on_error(error);

		point_3d pt;
		pt.x = tobii_sdk_param_stack_get_fixed_22x41_as_float64(stack.get(), index++, &error);
		pt.y = tobii_sdk_param_stack_get_fixed_22x41_as_float64(stack.get(), index++, &error);
		pt.z = tobii_sdk_param_stack_get_fixed_22x41_as_float64(stack.get(), index++, &error);
		eyetracker_exception::throw_on_error(error);

		return pt;
	}

	point_2d read_point_2d(uint32_t& index)
	{
		tobii_sdk_error_t* error = 0;
		node_prolog prolog(tobii_sdk_param_stack_get_node_prolog(stack.get(), index++, &error));
		eyetracker_exception::throw_on_error(error);

		point_2d pt;
		pt.x = tobii_sdk_param_stack_get_fixed_22x41_as_float64(stack.get(), index++, &error);
		pt.y = tobii_sdk_param_stack_get_fixed_22x41_as_float64(stack.get(), index++, &error);
		eyetracker_exception::throw_on_error(error);

		return pt;
	}

};

} // namespace tracking

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_PARAM_STACK_HPP__
