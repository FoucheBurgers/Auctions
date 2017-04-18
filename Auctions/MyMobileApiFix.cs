using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Serialization;

namespace Auctions
{
    /// <summary>
    /// Fixes issues with sending large amount of data to the server. 
    /// </summary>
    class MyMobileApiFix : MyMobileApi.API
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            ServicePointManager.Expect100Continue = false;
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(uri);
            webRequest.KeepAlive = false;
            return webRequest;
        }
    }

    public class senddata
    {
        [XmlElement("settings")]
        public settings _settings;
        [XmlElement("entries")]
        public List<entries> _entries;
    }

    public class settings
    {
        public bool live;
        public bool return_credits;
        public bool return_msgs_credits_used;
        public bool return_msgs_success_count;
        public bool return_msgs_failed_count;
        public bool return_entries_success_status;
        public bool return_entries_failed_status;
        public string default_senderid;
        public string default_date;
        public string default_time;
        public string default_curdate;
        public string default_curtime;
        public string default_data1;
        public string default_data2;
        public bool default_flash;
        public string default_type;
        public string default_costcentre;
        public string default_validityperiod;
    }

    public class entries
    {
        public string numto;
        public string customerid;
        public string senderid;
        public string time;
        public string data1;
        public string data2;
        public bool flash;
        public string type;
        public string costcentre;
        public string validityperiod;
    }

    public class api_result
    {
        [XmlElement("entries_failed")]
        public List<entries_failed> _entriesfailed;
        [XmlElement("entries_success")]
        public List<entries_success> _entriessuccess;
        [XmlElement("send_info")]
        public send_info _sendinfo;
        [XmlElement("call_result")]
        public call_result _callresult;
        [XmlElement("data")]
        public data _data;
    }

    public class data
    {
        public string credits;
    }

    public class entries_failed
    {
        public string numto;
        public string customerid;
        public string reason;
    }
    public class entries_success
    {
        public string numto;
        public string customerid;
    }

    public class send_info
    {
        public string eventid;
        public string credits;
        public string msgs_credits_used;
        public string msgs_success;
        public string msgs_failed;
    }

    public class call_result
    {
        public string result;
        public string error;
    }
}