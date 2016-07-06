/*
 * qianqians
 * 2016-7-5
 * test_cpp_service_client.cpp
 */
#include <connectnetworkservice.h>
#include <process_.h>
#include <Ichannel.h>
#include <Icaller.h>
#include <channel.h>
#include <juggleservice.h>
#include <timerservice.h>

#include <caller/testcaller.h>

void main() {
	boost::shared_ptr<juggle::process> _process = boost::make_shared<juggle::process>();

	boost::shared_ptr<service::connectnetworkservice> _service = boost::make_shared<service::connectnetworkservice>(_process);
	boost::shared_ptr<juggle::Ichannel> ch = _service->connect("127.0.0.1", 1234);

	boost::shared_ptr<caller::test> _test = boost::make_shared<caller::test>(ch);

	boost::shared_ptr<service::juggleservice> _juggleservice = boost::make_shared<service::juggleservice>();
	_juggleservice->add_process(_process);

	_test->test_func("test", 1);

	int64_t tick = clock();
	int64_t tickcount = 0;

	while (true)
	{
		int64_t tmptick = (clock() & 0xffffffff);
		if (tmptick < tick)
		{
			tickcount += 1;
			tmptick = tmptick + tickcount * 0xffffffff;
		}
		tick = tmptick;

		_service->poll(tick);
		_juggleservice->poll(tick);

		tmptick = (clock() & 0xffffffff);
		if (tmptick < tick)
		{
			tickcount += 1;
			tmptick = tmptick + tickcount * 0xffffffff;
		}
		int64_t ticktime = (tmptick - tick);
		tick = tmptick;

		Sleep(15);
	}
}
