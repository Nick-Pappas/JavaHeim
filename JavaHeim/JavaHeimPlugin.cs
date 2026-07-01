/**
 * v1
 * for science
 * 
 * TODO: for v2, refactor using proper MVC architecture and perhaps some kind of gamma-powered mechanical monster codebase
 * with freeway on-ramps for arms and a heart as black as coal...
 */
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace JavaHeim
{
    /**
     * The primary entry point for the JavaHeim Enterprise Edition mod.
     * This class handles the initialization of the Harmony patching engine
     * and the binding of the BepInEx configuration subsystem. 
     * It is a good news everyone type of class.
     */
    [BepInPlugin("com.nickpappas.javaheim", "JavaHeim", "1.0.0")]
    public class JavaHeimPlugin : BaseUnityPlugin
    {
        /**
         * The Harmony patching engine instance used for bytecode redirection.
         */
        private readonly Harmony _harmony = new Harmony("nickpappas.javaheim");

        /**
         * Global configuration entry for the Farnsworth-style prefix augmentation of the presentable String that henceforth shall be delivered...
         */
        public static ConfigEntry<bool> FarnsworthMode = null!;

        /**
         * Invoked by the BepInEx bootloader to initialize the plugin state.
         * Orchestrates the lifecycle of the configuration singleton and prepares the string transformation subsystem for runtime bytecode redirection.
         */
        private void Awake()
        {
            /**
             * The Legend
             */
            ConfigFile config = this.Config;
            ManualLogSource logger = this.Logger;
            string versionString = Info.Metadata.Version.ToString();

            /**
             * Initialize the Enterprise Diagnostic Logging Service first to catch all startup events
             */
            JavaHeimDiagnosticLoggingService.Initialize(versionString);

            /**
             * Should I Java-ify the strings in the config too? Perhaps if I add a config setting that Javalheimizes the config itself.
             * System.out.thinkAloud("I need to reflect on that.");
             */
            FarnsworthMode = config.Bind("General", "Professor-Farnsworth-it", false, "Prepend 'Good news everyone! This is a ' to the string.");

            /**
             * Initialize the Enterprise Config Watchdog to support live file editing, so that we can 
             * professor-farnsworth-ise on demand.
             */
            JavaHeimConfigWatchdogService.Initialize(config, logger);

            this._harmony.PatchAll();

            logger.LogInfo("System.out.println(\"JavaHeimization is commencing.\");");
            JavaHeimDiagnosticLoggingService.WriteEnterpriseLogStatement("JavaHeimization sequence initiated in Awake context.");
        }

        /**
         * Invoked when the plugin instance is being destroyed.
         * Ensures that the operating system resources allocated for file monitoring are released. PREFORMANT
         */ 
        private void OnDestroy()
        {
            JavaHeimConfigWatchdogService.Dispose();
            JavaHeimDiagnosticLoggingService.Dispose();
        }
    }
}