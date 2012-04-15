

#ifndef __TOBII_SDK_CPP_TRACKING_TYPES_HPP__
#define __TOBII_SDK_CPP_TRACKING_TYPES_HPP__

#include <string>
#include <vector>
#include <iostream>

#include <boost/shared_ptr.hpp>
#include <boost/shared_array.hpp>

#include <tobii/sdk/core.h>

namespace tobii {

namespace sdk {

namespace cpp {

namespace tracking {

class blob {

private:
	boost::shared_array<uint8_t> data;
	uint32_t length;

public:
	typedef boost::shared_array<uint8_t> data_type;

	blob() :
		length(0)
	{ }

	blob(boost::shared_array<uint8_t> bytes, uint32_t byte_length) :
		data(bytes),
		length(byte_length)
	{ }

	uint32_t get_length()
	{
		return length;
	}

	boost::shared_array<uint8_t> get_data()
	{
		return data;
	}

};

struct authorize_challenge {
	uint32_t realm_id;
	uint32_t algorithm;
	blob challenge_data;
};

struct unit_info {
	typedef boost::shared_ptr<unit_info> pointer;

	std::string serial_number;
	std::string model;
	std::string generation;
	std::string firmware_version;
};


struct payperuse_info {
	bool enabled;
	bool authorized;
	uint32_t realm;
};

struct point_2d {
	double x;
	double y;

	point_2d() :
		x(0.0), y(0.0)
	{ }

	point_2d(double x_, double y_) :
		x(x_), y(y_)
	{ }
};

struct point_3d {
	double x;
	double y;
	double z;

	point_3d() :
		x(0.0), y(0.0), z(0.0)
	{ }

	point_3d(double x_, double y_, double z_) :
		x(x_), y(y_), z(z_)
	{ }
};

inline std::ostream& operator<< (std::ostream& stream, const point_2d& pt)
{
	stream << "point 2d [";
	stream << pt.x << " " << pt.y;
	stream << "]";
	return stream;
}

inline std::ostream& operator<< (std::ostream& stream, const point_3d& pt)
{
	stream << "point 3d [";
	stream << pt.x << " " << pt.y << " " << pt.z;
	stream << "]";
	return stream;
}


struct extension {
	typedef boost::shared_ptr<std::vector<extension> > vector_pointer;

	uint32_t extension_id;
	std::string name;
	uint32_t realm;
	uint32_t protocol_version;
};

struct x_configuration {
	typedef boost::shared_ptr<x_configuration> pointer;

	point_3d upper_left;
	point_3d upper_right;
	point_3d lower_left;
};

struct head_movement_box {
	typedef boost::shared_ptr<head_movement_box> pointer;

	point_3d point_1;
	point_3d point_2;
	point_3d point_3;
	point_3d point_4;
	point_3d point_5;
	point_3d point_6;
	point_3d point_7;
	point_3d point_8;
};



} // namespace tracking

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TRACKING_TYPES_HPP__
