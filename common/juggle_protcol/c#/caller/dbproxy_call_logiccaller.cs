/*this caller file is codegen by juggle for c#*/
using System;
using System.Collections;
using System.IO;
using MsgPack;
using MsgPack.Serialization;

namespace caller
{
    public class dbproxy_call_logic : juggle.Icaller 
    {
        public dbproxy_call_logic(juggle.Ichannel _ch) : base(_ch)
        {
            module_name = "dbproxy_call_logic";
        }

        public void save_ovject_sucess(Int64 argv0)
        {
            ArrayList _argv = new ArrayList();
            _argv.Add(argv0);
            call_module_method("save_ovject_sucess", _argv);
        }

        public void ack_find_object(Int64 argv0,Hashtable argv1)
        {
            ArrayList _argv = new ArrayList();
            _argv.Add(argv0);
            _argv.Add(argv1);
            call_module_method("ack_find_object", _argv);
        }

    }
}
