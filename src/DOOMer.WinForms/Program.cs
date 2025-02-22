namespace DOOMer.WinForms;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using DOOMer.Core;

internal static class Program {
    /// <summary>
    /// The absolute name of the application, with extension.
    /// </summary>
    internal static string ApplicationName { get; }
    /// <summary>
    /// The absolute path of the application.
    /// </summary>
    internal static string FullApplicationPath { get; }
    /// <summary>
    /// The path of the application.
    /// </summary>
    internal static string ApplicationDirectory { get; }

    /// <summary>
    /// The current exit code relating to the program.
    /// </summary>
    public static int ExitCode { get; internal set; }
    /// <summary>
    /// Whether the program is debugging.
    /// </summary>
    internal static bool DebugMode { get; }
    /// <summary>
    /// Whether the program has ran as administrator.
    /// </summary>
    internal static bool IsAdmin { get; }
    /// <summary>
    /// Gets a string-representation of the current common language runtime version.
    /// </summary>
    public static string CLR { get; }

    static Program() {
        // An array of possible application paths.
        string?[] Paths = [
#if NET6_0_OR_GREATER
            // Base directory of the app context.
            AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar
                + AppDomain.CurrentDomain.FriendlyName + ".exe",
#else
            // The executing assembly location, most likely to work on all CLR versions.
            Assembly.GetExecutingAssembly().Location,
#endif

            // Uses the App-Domain values for cross-platform support, if it becomes possible.
            (AppDomain.CurrentDomain.BaseDirectory
            + Path.DirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + AppDomain.CurrentDomain.FriendlyName + ".exe",

            // Gets the main file name from the executing process.
            Process.GetCurrentProcess().MainModule?.FileName ];

        // Get the correct path. It may be null, but it's okay since it won't launch in that case.
        // The reason its non-nullable is because the application should KNOW it's not null to run.
        FullApplicationPath = Paths.First(x => !x.IsNullEmptyWhitespace())!.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        ApplicationDirectory = (File.Exists(FullApplicationPath) ? Path.GetDirectoryName(FullApplicationPath) : null)!;

        // Set the set application name, which can be different based on users preference.
        ApplicationName = AppDomain.CurrentDomain.FriendlyName;

        // Set the common language runtime string
        CLR = RuntimeInformation.FrameworkDescription.UnlessNullEmptyWhitespace("unknown framework")
#if NET7_0_OR_GREATER
                + "-" + RuntimeInformation.RuntimeIdentifier.UnlessNullEmptyWhitespace("unknown runtime")
#endif
                + (Environment.Is64BitProcess ? " (64-bit)" : " (32-bit)");

        // Check admin.
        IsAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

        // Debug stuff.
#if DEBUG
        DebugMode = true;
        InternalWad.EnsureNoDuplicates();
#else
        if (!FullApplicationPath.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)) {
            FullApplicationPath = FullApplicationPath.SubstringBeforeLastChar('.') + ".exe";
        }
#endif
    }

    [STAThread]
    static int Main() {
        if (FullApplicationPath.IsNullEmptyWhitespace()) {
            throw new ApplicationException("Could not get the full application path.");
        }

        if (!Environment.CurrentDirectory.Equals(ApplicationDirectory, StringComparison.InvariantCultureIgnoreCase)) {
            Environment.CurrentDirectory = ApplicationDirectory;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new frmMain());
        return ExitCode;
    }
}