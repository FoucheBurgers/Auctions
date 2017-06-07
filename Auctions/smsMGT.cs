﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Auctions
{
    public class smsMGT
    {
        MyMobileApiFix apiCall;
        private string XmlSerialize<T>(T entity) where T : class
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);
                xsSubmit.Serialize(writer, entity, xmlns);
                return sw.ToString();
            }
        }

        public string SendSingleSMS(string reference, string number, string message)
        {
            var myxml = "";
            senddata sd = new senddata();
            settings set = new settings();
            entries ent = new entries();
            List<entries> le = new List<entries>();

            set.live = true;
            set.return_credits = true;
            set.return_entries_failed_status = true;
            set.return_entries_success_status = true;
            set.default_date = "04/04/2017";
            set.default_time = "10:45";
            set.default_flash = false;
            set.default_type = "SMS";
            sd._settings = set;

            ent.numto = number.Trim();// "083449343";
            ent.customerid = reference;   //"123456";
            ent.senderid = "BMC";
            //ent.time = "10:45";
            ent.data1 = message;   //"This is my test message to one";
            //ent.flash = false;
            //ent.type = "SMS";
            //ent.costcentre = "";
            ent.validityperiod = "0";
            le.Add(ent);

            //ent = new entries();
            //ent.numto = "0836780";
            //ent.customerid = "123456";
            //ent.senderid = "terence123";
            ////ent.time = "10:45";
            //ent.data1 = "This is my test message to two";
            ////ent.flash = false;
            ////ent.type = "SMS";
            ////ent.costcentre = "";
            //ent.validityperiod = "0";
            //le.Add(ent);
            sd._entries = le;

            var result = XmlSerialize(sd);
            using (apiCall = new MyMobileApiFix())
            {
                string retresult = apiCall.Send_STR_STR("sky_bmc", "skybmc123", result); // sky_bmc

                var serializer = new XmlSerializer(typeof(api_result));
                api_result theresult;

                using (TextReader reader = new StringReader(retresult))
                {
                    theresult = (api_result)serializer.Deserialize(reader);
                }
                if (theresult._entriesfailed.Count > 0)
                    foreach(entries_failed entr in theresult._entriesfailed)
                    {
                        return entr.reason;
                    }
            }
            return "success";
        }
        public string SMSStatus()
        {
 //           var myxml = "";
            senddata sd = new senddata();
            settings set = new settings();
            entries ent = new entries();
            List<entries> le = new List<entries>();

            set.live = true;
            set.return_credits = true;
            set.return_entries_failed_status = true;
            set.return_entries_success_status = true;
            set.default_date = "04/04/2017";
            set.default_time = "10:45";
            set.default_flash = false;
            set.default_type = "SMS";
            sd._settings = set;

            ent.numto = "083449343";
            ent.customerid = "123456";
            ent.senderid = "BMC";
            ent.data1 = "Get Balance";   //"This is my test message to one";
            ent.validityperiod = "0";
            le.Add(ent);

            sd._entries = le;


            var result = XmlSerialize(sd);
            using (apiCall = new MyMobileApiFix())
            {
                string retresult = apiCall.Send_STR_STR("sky_bmc", "skybmc123", result); // sky_bmc
                return retresult;
            }
        }

    }

}