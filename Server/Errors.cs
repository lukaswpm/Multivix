using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Server
{
    class Errors
    {
        public static void Manage_Errors(Exception exception)
        {
            try
            {
                switch (exception)
                {
                    default:
                        Error_Log_Generate(exception);
                        break;
                }
            }
            catch (Exception ex)
            {
                Error_Log_Generate(ex);
            }
        }
        private static void Error_Log_Generate(Exception exception)
        {
            try
            {
                var ptBR = new CultureInfo("pt-BR");
                string User = "User: " + Global.Username;
                string DateHour = "DateTime: " + DateTime.Now.ToString(ptBR);
                string Error = "Error: " + exception.Message;
                string SO = "OS: " + Global.OSVersion();
                string DateHour_error = DateTime.Now.Year.ToString()
                                                + "-" + DateTime.Now.Month.ToString()
                                                + "-" + DateTime.Now.Day.ToString()
                                                + "_" + DateTime.Now.Hour
                                                + "-" + DateTime.Now.Minute
                                                + "-" + DateTime.Now.Second;

                string Content = User + "\r\n"
                    + DateHour + "\r\n" + SO + "\r\n" + Error;
                if (Directory.Exists(Global.Log_Path))
                {
                    File.WriteAllText(Global.Log_Path + "/Error_" + DateHour_error
                                      + "_" + Global.Username + ".log", Content);
                }
                else
                {
                    Directory.CreateDirectory(Global.Log_Path);
                    File.WriteAllText(Global.Log_Path + "/Error_" + DateHour_error
                                      + "_" + Global.Username + ".log", Content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
