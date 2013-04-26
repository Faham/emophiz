
#ifndef __TOBII_SDK_CPP_DISCOVERY_EYETRACKER_INFO_HPP__
#define __TOBII_SDK_CPP_DISCOVERY_EYETRACKER_INFO_HPP__

#include <string>
#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace discovery {

class eyetracker_info {

private:
	boost::shared_ptr<tobii_sdk_device_info_t> device_info;

public:
	typedef boost::shared_ptr<eyetracker_info> pointer;

	eyetracker_info(tobii_sdk_device_info_t* dev_info)
	{
		if(!dev_info) {
			throw std::runtime_error("dev_info was null pointer");
		}

		tobii_sdk_error_t* error = 0;
		device_info.reset(tobii_sdk_device_info_clone(dev_info, &error), &tobii_sdk_device_info_destroy);
		eyetracker_exception::throw_on_error(error);
	}

	std::string get_product_id()
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<const char> c_product_id (tobii_sdk_device_info_get_product_id(device_info.get(), &error), &tobii_sdk_free_string);
		eyetracker_exception::throw_on_error(error);

		if (!c_product_id)
			return "";

		std::string product_id(c_product_id.get());
		return product_id;
	}

	std::string get_given_name()
	{
		return get_value("given-name");
	}

	std::string get_model()
	{
		return get_value("model");
	}

	std::string get_generation()
	{
		return get_value("generation");
	}

	std::string get_version()
	{
		return get_value("firmware-version");
	}

	std::string get_status()
	{
		return get_value("status");
	}

	tobii_sdk_device_info_t* get_handle() const
	{
		return device_info.get();
	}

private:
	std::string get_value(const std::string& key)
	{
		tobii_sdk_error_t* error = 0;
		boost::shared_ptr<const char> c_value (tobii_sdk_device_info_get (device_info.get(), key.c_str(), &error), &tobii_sdk_free_string);
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

#endif // __TOBII_SDK_CPP_DISCOVERY_EYETRACKER_INFO_HPP__
