

#ifndef __TOBII_SDK_CPP_UPGRADE_PACKAGE_HPP__
#define __TOBII_SDK_CPP_UPGRADE_PACKAGE_HPP__

#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/upgrade/progress-reporter.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace upgrade {

class upgrade_package {

public:
	typedef boost::shared_ptr<upgrade_package> pointer;

private:
	boost::shared_ptr<tobii_sdk_upgrade_package_t> internal_upgrade_package;

public:
	upgrade_package(const uint8_t* package_data,
			uint32_t package_size)
	{
		tobii_sdk_error_t* error = 0;
		internal_upgrade_package.reset(tobii_sdk_upgrade_package_parse(package_data, package_size, 1, &error),
				&tobii_sdk_upgrade_package_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	tobii_sdk_upgrade_package_t* get_handle() const
	{
		return internal_upgrade_package.get();
	}

};


} // namespace upgrade

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_UPGRADE_PACKAGE_HPP__
