/**
 * v1
 */
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.IO;

namespace JavaHeim
{
    /**
     * Enterprise Service responsible for monitoring the physical configuration file
     * and synchronizing the in-memory state upon observed modification of said file.
     */
    public static class JavaHeimConfigWatchdogService
    {
        private static FileSystemWatcher _watcher = null!;
        private static ConfigFile _config = null!;
        private static ManualLogSource _logger = null!;
        private static DateTime _lastRead = DateTime.MinValue;

        /**
         * Initializes the file system listener for the provided configuration object.
         * 
         * @param configP The BepInEx ConfigFile instance to be monitored.
         * @param loggerP The enterprise logger instance for diagnostic output.
         */
        public static void Initialize(ConfigFile configP, ManualLogSource loggerP)
        {
            /**
             * The Legend
             */
            _config = configP;
            _logger = loggerP;
            string configDirectory = Paths.ConfigPath;
            string configFileName = Path.GetFileName(_config.ConfigFilePath);

            _watcher = new FileSystemWatcher(configDirectory, configFileName);
            _watcher.NotifyFilter = NotifyFilters.LastWrite;

            _watcher.Changed += OnFileChanged;
            _watcher.Created += OnFileChanged;
            _watcher.Renamed += OnFileChanged;

            _watcher.EnableRaisingEvents = true;

            JavaHeimDiagnosticLoggingService.WriteEnterpriseLogStatement("Config Watchdog Sensor Active on [VIRTUALIZED_STORAGE_FABRIC]");
        }

        /**
         * Event handler triggered when the configuration file is modified on disk or tape or other storage media.
         * 
         * @param senderP The object that initiated the file system event.
         * @param eP The specific event arguments containing file change metadata.
         */
        private static void OnFileChanged(object senderP, FileSystemEventArgs eP)
        {
            /**
             * The Legend
             */
            DateTime currentTime = DateTime.Now;
            TimeSpan timeSinceLastRead = currentTime - _lastRead;
            TimeSpan debounceThreshold = TimeSpan.FromMilliseconds(500);

            if (timeSinceLastRead < debounceThreshold)
            {
                return;
            }

            _lastRead = currentTime;

            _logger.LogInfo("System.out.println(\"JavaHeim: Detected config change on disk. Reloading...\");");
            JavaHeimDiagnosticLoggingService.WriteEnterpriseLogStatement("Disk modification detected. Synchronization triggered.");

            _config.Reload();

            if (JavaHeimPlugin.FarnsworthMode.Value)
            {
                _logger.LogInfo("System.out.println(\"Professor Farnsworth brings good news!\");");
                JavaHeimDiagnosticLoggingService.WriteEnterpriseLogStatement("Logical Transition: Good News State Enabled.");
            }
            else
            {
                _logger.LogInfo("System.out.println(\"Professor Farnsworth went to the angry dome!\");");
                JavaHeimDiagnosticLoggingService.WriteEnterpriseLogStatement("Logical Transition: Angry Dome State Enabled.");
            }
        }

        /**
         * Releases the operating system handle for the file watcher. PERFORMANT
         */
        public static void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}