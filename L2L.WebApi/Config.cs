using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace L2L.WebApi
{
    public static class Config
    {
        public static string FbAppId
        {
            get
            {
                if (ConfigurationManager.AppSettings["fbAppId"]
                    != null)
                {
                    return ConfigurationManager
                        .AppSettings["fbAppId"].ToString();
                }

                return "447596385432215";
            }
        }

        public static string FbAppSecret
        {
            get
            {
                if (ConfigurationManager.AppSettings["fbAppSecret"]
                    != null)
                {
                    return ConfigurationManager
                        .AppSettings["fbAppSecret"].ToString();
                }

                return "632925f56091495c2284f982e2266d94";
            }
        }

        public static string FbAppTestId
        {
            get
            {
                if (ConfigurationManager.AppSettings["fbAppTestId"]
                    != null)
                {
                    return ConfigurationManager
                        .AppSettings["fbAppId"].ToString();
                }

                return "447596385432215";
            }
        }

        public static string FbAppTestSecret
        {
            get
            {
                if (ConfigurationManager.AppSettings["fbAppTestSecret"]
                    != null)
                {
                    return ConfigurationManager
                        .AppSettings["fbAppSecret"].ToString();
                }

                return "632925f56091495c2284f982e2266d94";
            }
        }
    }
}