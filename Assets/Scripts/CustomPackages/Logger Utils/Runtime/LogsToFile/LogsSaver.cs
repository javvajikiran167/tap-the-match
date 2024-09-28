using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Custom.Utils
{
    public class LogsSaver : MonoBehaviour
    {
        public static LogsSaver instance;

        #region Log Object
        [Serializable]
        public struct Logs
        {
            public string condition;
            public string stackTrace;
            public string type;
            public string dateTime;

            public Logs(string condition, string stackTrace, string type, string dateTime)
            {
                this.condition = condition;
                this.stackTrace = stackTrace;
                this.type = type;
                this.dateTime = dateTime;
            }
        }

        [Serializable]
        public class LogInfo
        {
            public List<Logs> logInfoList = new List<Logs>();
        }
        #endregion

        LogInfo logs = new LogInfo();
        FileStreamWriter fileStreamWriter;
        string filePath;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        void OnEnable()
        {
            //Subscribe to Log Event
            Application.logMessageReceived += LogCallback;
        }

        /// <summary>
        /// Creates a file and starts saving logs to that file. Along with the given file name, file created time stamp is also added as suffix to the file name.
        /// </summary>
        /// <param name="fileName"></param>
        public void CreateFileAndWriteLogs(string baseFilesPath, string fileName)
        {
            SaveAnyExistingLogsAndCloseFile();
            logs = new LogInfo();
            LogUtils.Log("Called CreateFileAndWriteLogs with gameSessionId: " + fileName);
            filePath = GetFileName(baseFilesPath, fileName);
            fileStreamWriter = FileStreamWriter.FromFile(filePath, FileMode.OpenOrCreate);
            InvokeRepeating("SaveLogs", 1, 1);
            Debug.Log("Called CreateFileAndWriteLogs new file path " + filePath);
        }

        private void SaveAnyExistingLogsAndCloseFile()
        {
            try
            {
                CancelInvoke();
                //saving logs incase of any unsaved logs
                SaveLogs();
                fileStreamWriter?.CloseIfNoNull();
            }
            catch (Exception)
            {

            }
        }

        private string GetFileName(string baseFilesPath, string fileName)
        {
            string timeStamp = string.Format("{0:yyyy_MM_dd_HH_mm_ss_fff}", DateTime.Now);
            string filePath = Path.Combine(baseFilesPath, fileName + "_" + timeStamp + ".txt");
            Debug.Log("File path: " + filePath);
            return filePath;
        }

        //Called when there is an exception
        void LogCallback(string condition, string stackTrace, LogType type)
        {
            //Create new Log
            Logs logInfo = new Logs(condition, stackTrace, type.ToString(), DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            //Add it to the List
            logs.logInfoList.Add(logInfo);
        }

        private void SaveLogs()
        {
            try
            {
                if (fileStreamWriter != null && logs != null && logs.logInfoList.Count > 0)
                {
                    string logsData = JsonUtility.ToJson(logs, true);
                    logs.logInfoList.Clear();
                    fileStreamWriter.Write(logsData);
                }
            }
            catch (Exception)
            {

            }
        }

        void OnDisable()
        {
            //Un-Subscribe from Log Event
            Application.logMessageReceived -= LogCallback;
            SaveLogs();
            fileStreamWriter?.Dispose();
        }

        private void OnApplicationQuit()
        {
            SaveLogs();
        }
    }
}