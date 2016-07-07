/*this caller file is codegen by juggle for c++*/
#include <sstream>
#include <tuple>
#include <string>
#include <Icaller.h>
#include <Ichannel.h>
#include <boost/any.hpp>
#include <boost/make_shared.hpp>

namespace caller
{
class dbproxy_call_logic : public juggle::Icaller {
public:
    dbproxy_call_logic(boost::shared_ptr<juggle::Ichannel> _ch) : Icaller(_ch) {
        module_name = "dbproxy_call_logic";
    }

    ~dbproxy_call_logic(){
    }

    void save_ovject_sucess(int64_t argv0){
        auto v = boost::make_shared<std::vector<boost::any> >();
        v->push_back("dbproxy_call_logic");
        v->push_back("save_ovject_sucess");
        v->push_back(boost::make_shared<std::vector<boost::any> >());
        (boost::any_cast<boost::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        ch->push(v);
    }

    void ack_find_object(int64_t argv0,boost::shared_ptr<std::unordered_map<std::string, boost::any> > argv1){
        auto v = boost::make_shared<std::vector<boost::any> >();
        v->push_back("dbproxy_call_logic");
        v->push_back("ack_find_object");
        v->push_back(boost::make_shared<std::vector<boost::any> >());
        (boost::any_cast<boost::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        (boost::any_cast<boost::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv1);
        ch->push(v);
    }

};

}
