using System;
using System.IO;
using System.Reflection;

namespace StrongerTogetherDAL
{
    public class Logger
    {
        // config
        private readonly string _LogPath;

        // creates variable to call on
        public Logger(string logpath)
        {
            _LogPath = logpath;
        }

        public void ErrorLogger(string className, string methodName, Exception sqlEx, string level = "Error")
        {

            try
            {
                //builds stacktrace for sql
                string stackTrace = sqlEx.StackTrace;

                // Mwrites the error
                using (StreamWriter errorWritter = new StreamWriter(_LogPath, true))
                {
                    
                    errorWritter.WriteLine(new string('-', 40));
                    errorWritter.WriteLine($"Class:{className} Method:{methodName} / {DateTime.Now.ToString()} / {level}\n{sqlEx.Message}\n{stackTrace}");
                    errorWritter.Close();
                }
            }
            catch (Exception ex)
            {
              
            }
        }
    }
}
