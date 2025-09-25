using Gtk;

namespace e3d
{
    public static class Config
    {
        // Application settings
        public static string PathSvg = "/home/martin/estru3dc/data/svg/";
        //string PathPng = "/home/martin/estru3dc/data/png/";
        public static int ButtonSize = 32;
        public static string AppName { get; set; } = "Estru3D";
        public static string Version { get; set; } = "1.0.0";
        
        // Window references
        public static ApplicationWindow? MainWindow { get; set; }
        
        // Application state
        public static bool IsDarkTheme { get; set; }
        public static string CurrentFilePath { get; set; } = "";
        
        // Configuration
        public static int DefaultWindowWidth { get; set; } = 800;
        public static int DefaultWindowHeight { get; set; } = 600;
        
        // Initialize global variables
        public static void Initialize()
        {
            // You can set initial values here
            IsDarkTheme = false;
            // Add any other initialization logic
        }
    }
}