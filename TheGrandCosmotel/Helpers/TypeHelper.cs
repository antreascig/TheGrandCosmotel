using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace WebGames.Helpers
{
    public static class TypeHelper
    {
        public static void CopyObjectFrom(this object target, object source)
        {
            if (target == null || source == null) return;

            foreach (var des in target.GetType().GetProperties())
            {
                if (source is JObject)
                {
                    JObject o = source as JObject;
                    if (o[des.Name] != null)
                    {
                        if (des.SetMethod != null)
                        {
                            try
                            {
                                var val = (JValue)o[des.Name];
                                des.SetValue(target, val.Value);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else
                {
                    var src = source.GetType().GetProperty(des.Name);
                    if (src != null)
                    {
                        if (des.SetMethod != null)
                        {
                            try
                            {
                                if (des.PropertyType != src.PropertyType)
                                {
                                    try
                                    {
                                        des.SetValue(target, Convert.ChangeType(src.GetValue(source), des.PropertyType));
                                    }
                                    catch
                                    {
                                        des.SetValue(target, src.GetValue(source));
                                    }
                                }
                                else
                                {
                                    des.SetValue(target, src.GetValue(source));
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        public static void JoinWith(this string source, string delimiter, string JoinValue)
        {
            if (JoinValue == null) return;
            source = source ?? "";
            if ( source != "" ) source += delimiter;
            source += JoinValue;
        }
    }
}