/*this caller file is codegen by juggle for c#*/
using System;
using System.Collections;
using System.IO;
using MsgPack;
using MsgPack.Serialization;

namespace caller
{
    public class dbproxy_call_hub : juggle.Icaller 
    {
        public dbproxy_call_hub(juggle.Ichannel _ch) : base(_ch)
        {
            module_name = "dbproxy_call_hub";
        }

        public void reg_hub_sucess()
        {
            ArrayList _argv = new ArrayList();
            call_module_method("reg_hub_sucess", _argv);
        }

        public void ack_updata_persisted_object(Int64 argv0)
        {
            ArrayList _argv = new ArrayList();
            _argv.Add(argv0);
            call_module_method("ack_updata_persisted_object", _argv);
        }

        public void ack_get_object_info(Int64 argv0,String argv1)
        {
            ArrayList _argv = new ArrayList();
            _argv.Add(argv0);
            _argv.Add(argv1);
            call_module_method("ack_get_object_info", _argv);
        }

        public void ack_get_object_info_end(Int64 argv0)
        {
            ArrayList _argv = new ArrayList();
            _argv.Add(argv0);
            call_module_method("ack_get_object_info_end", _argv);
        }

    }
}
