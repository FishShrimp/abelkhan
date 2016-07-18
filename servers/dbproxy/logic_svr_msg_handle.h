/*
 * logic_svr_msg_handle.h
 *
 *  Created on: 2016-7-11
 *      Author: qianqians
 */
#ifndef _logic_svr_msg_handle_h
#define _logic_svr_msg_handles_h

#include <boost/shared_ptr.hpp>

#include <string>
#include <cstdint>

#include <caller/dbproxy_call_logiccaller.h>

#include "logicsvrmanager.h"
#include "mongodb_proxy.h"
#include "closehandle.h"

void reg_logic(boost::shared_ptr<dbproxy::logicsvrmanager> _logicsvrmanager, boost::shared_ptr<dbproxy::closehandle> _closehandle, std::string uuid) {
	_logicsvrmanager->reg_channel(uuid, juggle::current_ch);
	_closehandle->reg_logic();

	boost::shared_ptr<caller::dbproxy_call_logic> _caller = boost::make_shared<caller::dbproxy_call_logic>(juggle::current_ch);
	_caller->reg_logic_sucess();

	std::cout << "logic server " << uuid << "connected" << std::endl;
}

void logic_closed(boost::shared_ptr<dbproxy::closehandle> _closehandle) {
	_closehandle->logic_closed();
}

void logic_create_persisted_object(boost::shared_ptr<dbproxy::logicsvrmanager> _logicsvrmanager, boost::shared_ptr<dbproxy::mongodb_proxy> _mongodb_proxy, std::string object_info, std::string callbackid) {
	if (!_logicsvrmanager->is_logic(juggle::current_ch)) {
		return;
	}

	_mongodb_proxy->save(object_info);

	boost::shared_ptr<caller::dbproxy_call_logic> _caller = boost::make_shared<caller::dbproxy_call_logic>(juggle::current_ch);
	_caller->ack_create_persisted_object(callbackid);
}

void logic_updata_persisted_object(boost::shared_ptr<dbproxy::logicsvrmanager> _logicsvrmanager, boost::shared_ptr<dbproxy::mongodb_proxy> _mongodb_proxy, std::string objectid, std::string object_info, std::string callbackid) {
	if (!_logicsvrmanager->is_logic(juggle::current_ch)) {
		return;
	}

	_mongodb_proxy->update("{\"objectid\":\"" + objectid + "\"}", object_info);

	boost::shared_ptr<caller::dbproxy_call_logic> _caller = boost::make_shared<caller::dbproxy_call_logic>(juggle::current_ch);
	_caller->ack_updata_persisted_object(callbackid);
}

void logic_get_object_info(boost::shared_ptr<dbproxy::logicsvrmanager> _logicsvrmanager, boost::shared_ptr<dbproxy::mongodb_proxy> _mongodb_proxy, std::string objectid, std::string callbackid) {
	if (!_logicsvrmanager->is_logic(juggle::current_ch)) {
		return;
	}

	auto s = _mongodb_proxy->find(0, 0, 0, "{\"objectid\":\"" + objectid + "\"}", "");

	if (s.size() > 1) {
		std::cout << "mutli objectid:" << objectid << std::endl;
		return;
	}

	boost::shared_ptr<caller::dbproxy_call_logic> _caller = boost::make_shared<caller::dbproxy_call_logic>(juggle::current_ch);
	_caller->ack_get_object_info(callbackid, s[0]);
}

#endif //_logic_svr_msg_handles_h
