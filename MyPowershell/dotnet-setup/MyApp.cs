/*  NOTE: Intenal Use Only
    
CreateDate: 
CreatedBy: 
Comments:

*/

using System;
using System.Data;
using Newtonsoft.Json;
using System.Collections;
using System.Configuration;
using System.Collections.Generic;





public sealed class MyApp 
{
    //singleton implmentation
    private static readonly MyApp mApp = new MyApp();
        static MyApp()
    {}
    private MyApp()
    {}
    public static  MyApp Get
    {get {return mApp;}}


    
   

    
    private string _AppExePath = null;
    /// <summary>returns folder of exe assembly, trims end of backslash</summary>
    public string AppExePath { 
        get 
        {
            if (_AppExePath == null) {
                _AppExePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                System.IO.FileInfo fInfo = new System.IO.FileInfo(_AppExePath);
                _AppExePath = fInfo.DirectoryName.TrimEnd('\\');

            }
            return _AppExePath;
        } 
    }
    



    public const string DateFormatLong = "yyyy/MM/dd HH:mm:ss.hhh";
    public const string DateFormatShort = "yyyy/MM/dd HH:mm:ss";
    public const string DateFormatDate = "yyyy/MM/dd";

    //App
    public string Env { get; private set; }
    public int AppId { get; private set; }
    public string AppName { get; private set; }
    public string AppUser { get; private set; }
    public bool AppStarted { get; private set; }

    //setting
    private Dictionary<string, string> AppSettingsDicJson = new Dictionary<string, string>();
    public string AppSettingsJsonPath { get; private set; }


    /// <summary>
    /// return config setting value by name returns null if not there.        
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public  string GetSetting(string Name)
    {
        string val = null;
        if (AppSettingsDicJson.ContainsKey(Name))
            val = AppSettingsDicJson[Name].ToString();
        return val;
    }//GetSetting

}//class 

