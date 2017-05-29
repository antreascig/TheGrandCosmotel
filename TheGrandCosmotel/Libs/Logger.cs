using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGames.Libs
{
    public enum LogType
    {
        INFO,
        ERROR,
        CRITICAL
    }

    public class Logger
    {

        public static void Log(string msg, LogType msgType)
        {

        }

        public static void Log(Exception exc)
        {

        }

        public static void Log(Exception exc, string msg)
        {

        }
    }
}