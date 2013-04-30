
#ifndef __TOBII_SDK_CPP_TRACKING_CALIBRATION_HPP__
#define __TOBII_SDK_CPP_TRACKING_CALIBRATION_HPP__

#include <vector>
#include <boost/shared_ptr.hpp>

#include <tobii/sdk/cpp/eyetracker-exception.hpp>
#include <tobii/sdk/cpp/tracking/types.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

namespace tracking {

struct calibration_plot_item {
	typedef boost::shared_ptr<std::vector<calibration_plot_item> > vector_pointer;
	float true_x;
	float true_y;

	float map_left_x;
	float map_left_y;
	int32_t validity_left;

	float map_right_x;
	float map_right_y;
	int32_t validity_right;
};



class calibration {

private:
	blob raw_data;
	calibration_plot_item::vector_pointer plot_data;

	void extract_plot_data()
	{
		if(!raw_data.get_length())
			return;

		plot_data->clear();

		uint8_t* current_pos = raw_data.get_data().get();

		int32_t skip_length = *reinterpret_cast<int32_t*>(current_pos);
		current_pos += sizeof(int32_t);
		current_pos += skip_length;

		uint32_t number_of_items = *reinterpret_cast<uint32_t*>(current_pos);
		current_pos += sizeof(uint32_t);

		for (uint32_t i = 0; i < number_of_items; ++i) {
			extract_one_item(current_pos);
		}
	}

	void extract_one_item(uint8_t*& current_pos)
	{
		calibration_plot_item item;
		item.true_x = *reinterpret_cast<float*>(current_pos);
		current_pos += sizeof(float);
		item.true_y = *reinterpret_cast<float*>(current_pos);
		current_pos += sizeof(float);

		item.map_left_x = *reinterpret_cast<float*>(current_pos);
		current_pos += sizeof(float);
		item.map_left_y = *reinterpret_cast<float*>(current_pos);
		current_pos += sizeof(float);
		item.validity_left =  *reinterpret_cast<int*>(current_pos);
		current_pos += sizeof(int);
		current_pos += sizeof(float); // Skip quality value

		item.map_right_x = *reinterpret_cast<float*>(current_pos);
		current_pos += sizeof(float);
		item.map_right_y = *reinterpret_cast<float*>(current_pos);
		current_pos += sizeof(float);
		item.validity_right =  *reinterpret_cast<int*>(current_pos);
		current_pos += sizeof(int);
		current_pos += sizeof(float); // Skip quality value

		plot_data->push_back(item);
	}

public:
	calibration(const blob& data) :
		raw_data(data),
		plot_data(new std::vector<calibration_plot_item>())
	{
		extract_plot_data();
	}

	calibration() :
		plot_data(new std::vector<calibration_plot_item>())
	{

	}

	blob& get_raw_data()
	{
		return raw_data;
	}

	calibration_plot_item::vector_pointer get_plot_data()
	{
		return plot_data;
	}

};


} // namespace tracking

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TRACKING_CALIBRATION_HPP__
