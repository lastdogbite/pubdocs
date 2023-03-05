/*  NOTE: Intenal Use Only
    
CreateDate: 
CreatedBy: 
Comments:

*/

using System;
using System.Text;
using System.Collections.Generic;



public class MyAppUtil
{

    public static string GetString(object obj, string type = "STRING")
    {
        type = type.ToUpper().Trim();
        if (obj == null || obj.ToString() == "")
        {
            if (type.StartsWith("DATE")) return DateTime.MinValue.ToString("MM/dd/yyyy HH:mm:ss.hhh");
            if (type.StartsWith("INT") || type.StartsWith("DEC") || type.StartsWith("FLOAT") || type.StartsWith("DOUBLE") || type.StartsWith("LONG")) return "0";
            return "";
        }


        if (type.StartsWith("DATE") && obj.ToString().StartsWith("0000"))
            return DateTime.MinValue.ToString("MM/dd/yyyy HH:mm:ss.hhh");
            
        return Convert.ToString(obj);
    }//GetString


  

    public static bool IsNumericType(Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:                
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            default:
                return false;
        }//case
    }//IsNumericType


}
