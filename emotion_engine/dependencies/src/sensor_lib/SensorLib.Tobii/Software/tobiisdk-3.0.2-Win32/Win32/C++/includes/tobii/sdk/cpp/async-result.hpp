

#ifndef __TOBII_SDK_CPP_ASYNC_RESULT_HPP__
#define __TOBII_SDK_CPP_ASYNC_RESULT_HPP__

#include <boost/thread/mutex.hpp>
#include <boost/thread/condition_variable.hpp>
#include <boost/shared_ptr.hpp>

#include <tobii/sdk/core.h>
#include <tobii/sdk/cpp/eyetracker-exception.hpp>

namespace tobii {

namespace sdk {

namespace cpp {

template <class T>
class async_result {

private:
	T result;
	uint32_t error_code;
	bool result_set;
	boost::mutex mutex;
	boost::condition_variable result_condition;

public:
	typedef boost::shared_ptr<async_result<T> > pointer;

	async_result() :
		result_set(false)
	{

	}

	static pointer create()
	{
		pointer ptr(new async_result<T>());
		return ptr;
	}

	void set_result(T res)
	{
		boost::unique_lock<boost::mutex> lock(mutex);
		if(!result_set)
		{
			result = res;
			error_code = 0;
			result_set = true;
			result_condition.notify_all();
		}
	}

	void set_error(uint32_t error)
	{
		boost::unique_lock<boost::mutex> lock(mutex);

		if(!result_set)
		{
			error_code = error;
			result_set = true;
			result_condition.notify_all();
		}
	}

	void wait_for_result()
	{
		boost::unique_lock<boost::mutex> lock(mutex);
		while(!result_set)
			result_condition.wait(lock);
	}

	void throw_if_error()
	{
		if(error_code)
		{
			throw eyetracker_exception(error_code, "Result contains an error");
		}

	}

	T get_result()
	{
		return result;
	}

	uint32_t get_error_code()
	{
		return error_code;
	}

};


} // namespace cpp

} // namespace sdk

} // namespace tobii

#endif // __TOBII_SDK_CPP_ASYNC_RESULT_HPP__
