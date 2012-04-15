
#ifndef __TOBII_SDK_CPP_TIME_SYNC_STATE_HPP__
#define __TOBII_SDK_CPP_TIME_SYNC_STATE_HPP__

#include <vector>
#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>

namespace tobii {

namespace sdk {

namespace cpp {

namespace time {


struct sync_point {

	int64_t local_midpoint;
	int64_t remote_time;
	int64_t roundtrip_time;

	sync_point() :
		local_midpoint(0),
		remote_time(0),
		roundtrip_time(0)
	{

	}
};

class sync_state {

private:
	boost::shared_ptr<tobii_sdk_sync_state_t> internal_sync_state;
	std::vector<sync_point> sync_points;
public:
	typedef boost::shared_ptr<sync_state> pointer;

	enum sync_state_flag {UNSYNCHRONIZED, STABILIZING, SYNCHRONIZED};

	sync_state(const tobii_sdk_sync_state_t* state)
	{
		// Clone sync state without taking ownership of
		// the state pointer
		tobii_sdk_error_t* error = 0;
		internal_sync_state.reset(
							tobii_sdk_sync_state_clone(state, &error),
							&tobii_sdk_sync_state_destroy);
		eyetracker_exception::throw_on_error(error);

		update_sync_points();
	}

	sync_state(boost::shared_ptr<tobii_sdk_sync_state_t> state)
	{
		internal_sync_state = state;
		update_sync_points();
	}

	sync_state_flag get_sync_state_flag()
	{
		tobii_sdk_error_t* error = 0;
		uint32_t state_flag = tobii_sdk_sync_state_get_sync_state_flag(internal_sync_state.get(), &error);
		eyetracker_exception::throw_on_error(error);
		return static_cast<sync_state_flag>(state_flag);
	}

	const std::vector<sync_point>& get_sync_points()
	{
		return sync_points;
	}

private:

	void update_sync_points()
	{
		tobii_sdk_error_t* error = 0;
		uint32_t num_points = tobii_sdk_sync_state_get_number_of_points_in_use(internal_sync_state.get(), &error);
		eyetracker_exception::throw_on_error(error);

		for (uint32_t i = 0; i < num_points; ++i) {
			sync_point pt;
			tobii_sdk_sync_state_get_point_in_use(internal_sync_state.get(), i,
					&(pt.local_midpoint), &pt.remote_time, &pt.roundtrip_time, &error);
			eyetracker_exception::throw_on_error(error);
			sync_points.push_back(pt);
		}
	}

};

} // namespace time

} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif  // __TOBII_SDK_CPP_TIME_SYNC_STATE_HPP__
