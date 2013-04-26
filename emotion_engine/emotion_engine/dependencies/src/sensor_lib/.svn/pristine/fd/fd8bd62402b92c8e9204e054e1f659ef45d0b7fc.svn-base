
#ifndef __TOBII_SDK_CPP_EYETRACKER_EXCEPTION_HPP__
#define __TOBII_SDK_CPP_EYETRACKER_EXCEPTION_HPP__

#include <tobii/sdk/core.h>

#include <stdexcept>

namespace tobii {

namespace sdk {

namespace cpp {

typedef uint32_t error_code_t;

class eyetracker_exception : public std::runtime_error {

private:
	error_code_t error_code_;

public:
	eyetracker_exception(error_code_t code, const std::string& message) :
		std::runtime_error(message),
		error_code_(code)
	{

	}

	error_code_t get_error_code() const
	{
		return error_code_;
	}

	static void throw_on_error(tobii_sdk_error_t* error)
	{
		if(!error)
			return;

		error_code_t code = tobii_sdk_error_get_code(error);
		uint32_t error_is_failure = tobii_sdk_error_is_failure(error);



		if(error_is_failure)
		{
			const char* raw_message = tobii_sdk_error_get_message(error);
			std::string message(raw_message);
			tobii_sdk_free_string(raw_message);
			tobii_sdk_error_destroy(error);

			throw eyetracker_exception(code, message);
		}
		tobii_sdk_error_destroy(error);
	}

};


} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif // __TOBII_SDK_CPP_EYETRACKER_EXCEPTION_HPP__
