
#ifndef __TOBII_SDK_CPP_TRACKING_EYETRACKER_FACTORY_HPP__
#define __TOBII_SDK_CPP_TRACKING_EYETRACKER_FACTORY_HPP__

#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/mainloop.hpp>
#include <tobii/sdk/cpp/async-result.hpp>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/discovery/factory-info.hpp>
#include <tobii/sdk/cpp/tracking/eyetracker.hpp>


namespace tobii {

namespace sdk {

namespace cpp {

namespace tracking {

void eyetracker_created_trampoline(tobii_sdk_error_t* error,
		tobii_sdk_message_passer_t* mpi,
		void* user_data);

inline eyetracker::pointer create_eyetracker(const discovery::factory_info& info,
		const mainloop& loop)
{
	async_result<tobii_sdk_message_passer_t*> result;
	tobii_sdk_error_t* error = 0;
	tobii_sdk_message_passer_get(info.get_handle(),
			loop.get_handle(),
			&eyetracker_created_trampoline,
			&result,
			&error);
	eyetracker_exception::throw_on_error(error);

	result.wait_for_result();
	result.throw_if_error();

	return eyetracker::pointer(new eyetracker(result.get_result()));
}


inline void eyetracker_created_trampoline(tobii_sdk_error_t* error,
		tobii_sdk_message_passer_t* mpi,
		void* user_data)
{
	async_result<tobii_sdk_message_passer_t*>* result_ptr = (async_result<tobii_sdk_message_passer_t*>*)user_data;

	if(tobii_sdk_error_is_failure(error))
	{
		uint32_t error_code = tobii_sdk_error_get_code(error);
		result_ptr->set_error(error_code);
	}
	else
	{
		result_ptr->set_result(mpi);
	}
}



} // namespace tracking

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TRACKING_EYETRACKER_FACTORY_HPP__
