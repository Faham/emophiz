
#ifndef __TOBII_SDK_CPP_DISCOVERY_FACTORY_INFO_HPP__
#define __TOBII_SDK_CPP_DISCOVERY_FACTORY_INFO_HPP__

#include <string>
#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/discovery/factory-info.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace discovery {

class factory_info {

private:
	boost::shared_ptr<tobii_sdk_factory_info_t> internal_factory_info;

public:
	typedef boost::shared_ptr<factory_info> pointer;

	factory_info(const std::string& ip_address, uint32_t tetserver_port, uint32_t sync_port)
	{
		tobii_sdk_error_t* error = 0;
		internal_factory_info.reset(
				tobii_sdk_factory_info_create_for_networked_eyetracker(ip_address.c_str(),
						tetserver_port, sync_port, &error),
				&tobii_sdk_factory_info_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	factory_info(const eyetracker_info& et_info)
	{
		tobii_sdk_error_t* error = 0;
		internal_factory_info.reset(tobii_sdk_device_info_get_factory_info(et_info.get_handle(), &error),
				&tobii_sdk_factory_info_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	tobii_sdk_factory_info_t* get_handle() const
	{
		return internal_factory_info.get();
	}

	std::string get_representation()
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<const char> c_value (tobii_sdk_factory_info_get_representation (internal_factory_info.get(), &error), &tobii_sdk_free_string);
		eyetracker_exception::throw_on_error(error);
		if (c_value == 0)
			return "";

		std::string value(c_value.get());
		return value;
	}
};

} // namespace discovery

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif // __TOBII_SDK_CPP_DISCOVERY_FACTORY_INFO_HPP__
