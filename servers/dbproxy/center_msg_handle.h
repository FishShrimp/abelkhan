/*
 * center_msg_handle.h
 *
 *  Created on: 2016-7-11
 *      Author: qianqians
 */
#ifndef _center_msg_handle_h
#define _center_msg_handle_h

#include "centerproxy.h"

void reg_server_sucess(boost::shared_ptr<dbproxy::centerproxy> _centerproxy) {
	_centerproxy->is_reg_sucess;
}

#endif //_center_msg_handle_h
