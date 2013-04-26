
#ifndef __TOBII_SDK_CPP_UPGRADE__UPGRADE_HPP__
#define __TOBII_SDK_CPP_UPGRADE__UPGRADE_HPP__

#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/mainloop.hpp>
#include <tobii/sdk/cpp/discovery/eyetracker-info.hpp>
#include <tobii/sdk/cpp/upgrade/upgrade-package.hpp>
#include <tobii/sdk/cpp/upgrade/progress-reporter.hpp>



namespace tobii {

namespace sdk {

namespace cpp {

namespace upgrade {

inline void begin_upgrade(const mainloop& mainloop,
		const upgrade_package& package,
		const discovery::eyetracker_info& eyetracker_info,
		const progress_reporter& progress_reporter)
{
	tobii_sdk_error_t* error = 0;
	tobii_sdk_upgrade_begin(mainloop.get_handle(),
			package.get_handle(), eyetracker_info.get_handle(), progress_reporter.get_handle(), &error);
	eyetracker_exception::throw_on_error(error);
}

inline bool upgrade_package_is_compatible_with_device(
		const mainloop& mainloop,
		const upgrade_package& package,
		const discovery::eyetracker_info& eyetracker_info,
		uint32_t& error_code)
{
	tobii_sdk_error_t* error = 0;
	uint32_t compatible = tobii_sdk_upgrade_package_is_compatible_with_device(
			mainloop.get_handle(),
			package.get_handle(),
			eyetracker_info.get_handle(),
			&error);

	if(compatible)
	{
		error_code = 0;
		return true;
	}
	else
	{
		if(error)
		{
			error_code = tobii_sdk_error_get_code(error);
			tobii_sdk_error_destroy(error);
		}
		else
		{
			error_code = 0;
		}

		return false;
	}

}



} // namespace upgrade

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_UPGRADE__UPGRADE_HPP__
