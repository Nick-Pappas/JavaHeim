/**
 * v1
 */
using BepInEx;
using System;
using System.IO;

namespace JavaHeim
{
    /**
     * Enterprise Data Persistence Service responsible for the generation and 
     * maintenance of a physical audit trail on the storage medium which could be 
     * some kind of HDD or even some kind of SSD or a cold storage magnetic tape.
     */
    public static class JavaHeimDiagnosticLoggingService
    {
        /**
         * The low-level stream writer responsible for character-based I/O operations.
         */
        private static StreamWriter? _writer;

        /**
         * Initializes the logging stream with abstract metadata logging.
         * 
         * @param versionP The version string of the current application context.
         */
        public static void Initialize(string versionP)
        {
            /**
             * The Legend
             */
            string rootDirectory = Paths.BepInExRootPath;
            string fileName = "JavaHeim_Enterprise_Audit.log";
            string logFilePath = Path.Combine(rootDirectory, fileName);

            try
            {
                /**
                 * Attempt to establish a handle on the physical storage medium
                 */
                _writer = new StreamWriter(logFilePath, false);
                _writer.AutoFlush = true;

                WriteEnterpriseLogStatement("Audit Log Initialized. JavaHeim Version: " + versionP);
                WriteEnterpriseLogStatement("Persistence Medium: [SECURE_ENTERPRISE_DATA_LAKE]");
            }
            catch (Exception exP)
            {
                /**
                 * Silent failure of the logging provider must not halt the main execution thread
                 */
                UnityEngine.Debug.LogWarning("System.out.println(\"WARNING: Persistent Logging Service failed to mount. Proceeding in ephemeral mode.\");");
            }
        }

        /**
         * Wraps a raw diagnostic message in a Java print statement and commits it to disk or other storage mediums.
         * 
         * @param messageP The raw text message to be persisted.
         */
        public static void WriteEnterpriseLogStatement(string messageP)
        {
            if (_writer == null)
            {
                return;
            }

            /**
             * The Legend
             */
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string formattedMessage = string.Format("System.out.println(\"[{0}] {1}\");", timestamp, messageP);

            _writer.WriteLine(formattedMessage);
        }

        /**
         * Flushes the buffer and releases the file system handle for the log file. For Performategenerate
         */
        public static void Dispose()
        {
            /**
             * The Legend
             */
            StreamWriter? writer = _writer;

            if (writer != null)
            {
                WriteEnterpriseLogStatement("Disposing Enterprise Logging Provider. Connection Closed.");
                writer.Flush();
                writer.Close();
                writer.Dispose();
                _writer = null;
            }
        }
    }
}