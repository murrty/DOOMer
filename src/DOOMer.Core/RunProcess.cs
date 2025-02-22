// RunProcess is a part of 'https://github.com/murrty/DOOMer.Core' doom launcher.
// Licensed via GPL-3.0, if you did not receieve a license with this file; idk figure it out.
// This code, *as-is*, should not be a part of another project; it should really only be used as reference or testing.

#nullable enable
namespace DOOMer.Core;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

public static class RunProcess {
    /// <summary>
    ///     Opens a file in a new process.
    /// </summary>
    /// <param name="fullPath">
    ///     The path of the file.
    /// </param>
    /// <param name="arguments">
    ///     The arguments to pass to the file.
    /// </param>
    public static bool OpenFile(string fullPath, string? arguments = null) {
        if (!File.Exists(fullPath)) {
            //Log.Warn($"Cannot open '{fullPath}', it does not exist.");
            //Sounds.ActionFail();
            return false;
        }
        return OpenItemCore(path: fullPath, arguments: arguments, useShellExecute: true);
    }
    /// <summary>
    ///     Opens a file in a new process.
    /// </summary>
    /// <param name="file">
    ///     The file info to open.
    /// </param>
    /// <param name="arguments">
    ///     The arguments to pass to the file.
    /// </param>
    public static bool Open(FileInfo file, string? arguments = null) {
        if (!file.Exists || file.Length < 1) {
            file.Refresh();
            if (!file.Exists || file.Length < 1) {
                //Log.Warn($"Cannot open '{file.FullName}', it does not exist or has no length.");
                //Sounds.ActionFail();
                return false;
            }
        }
        return OpenItemCore(path: file.FullName, arguments: arguments, useShellExecute: true);
    }
    /// <summary>
    ///     Shows a file in the file explorer.
    /// </summary>
    /// <param name="fullPath">
    ///     The path of the file.
    /// </param>
    public static bool ShowFile(string fullPath) {
        if (!File.Exists(fullPath)) {
            //Log.Warn($"Cannot show '{fullPath}', it does not exist.");
            //Sounds.ActionFail();
            return false;
        }
        return ShowItemCore(path: fullPath, useShellExecute: true);
    }
    /// <summary>
    ///     Shows a file in the file explorer.
    /// </summary>
    /// <param name="file">
    ///     The file info to show.
    /// </param>
    public static bool Show(FileInfo file) {
        if (!file.Exists || file.Length < 1) {
            file.Refresh();
            if (!file.Exists || file.Length < 1) {
                //Log.Warn($"Cannot show '{file.FullName}', it does not exist or has no length.");
                //Sounds.ActionFail();
                return false;
            }
        }
        return ShowItemCore(path: file.FullName, useShellExecute: true);
    }

    /// <summary>
    ///     Opens a directory in a new explorer process.
    /// </summary>
    /// <param name="fullPath">
    ///     The path of the directory.
    /// </param>
    public static bool OpenDirectory(string fullPath) {
        if (!Directory.Exists(fullPath)) {
            //Log.Warn($"Cannot open '{fullPath}', it does not exist.");
            //Sounds.ActionFail();
            return false;
        }
        return OpenItemCore(path: fullPath, arguments: null, useShellExecute: true);
    }
    /// <summary>
    ///     Opens a directory in a new explorer process.
    /// </summary>
    /// <param name="directory">
    ///     The directory info to open.
    /// </param>
    public static bool Open(DirectoryInfo directory) {
        if (!directory.Exists) {
            directory.Refresh();
            if (!directory.Exists) {
                //Log.Warn($"Cannot show '{directory.FullName}', it does not exist.");
                //Sounds.ActionFail();
                return false;
            }
        }
        return OpenItemCore(path: directory.FullName, arguments: null, useShellExecute: true);
    }
    /// <summary>
    ///     Shows a directory in the file explorer.
    /// </summary>
    /// <param name="fullPath">
    ///     The path of the directory.
    /// </param>
    public static bool ShowDirectory(string fullPath) {
        if (!Directory.Exists(fullPath)) {
            //Log.Warn($"Cannot show '{fullPath}', it does not exist.");
            //Sounds.ActionFail();
            return false;
        }
        return ShowItemCore(path: fullPath, useShellExecute: true);
    }
    /// <summary>
    ///     Shows a directory in the file explorer.
    /// </summary>
    /// <param name="directory">
    ///     The directory info to show.
    /// </param>
    public static bool Show(DirectoryInfo directory) {
        if (!Directory.Exists(directory.FullName)) {
            //Log.Warn($"Cannot show '{directory.FullName}', it does not exist.");
            //Sounds.ActionFail();
            return false;
        }
        return ShowItemCore(path: directory.FullName, useShellExecute: true);
    }

    /// <summary>
    ///     Opens a web page in the default browser.
    /// </summary>
    /// <param name="fullUrl">
    ///     The URL that will be opened.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the url was launched;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool OpenUri(string fullUrl) {
        Uri uri = new(fullUrl);
        return Open(absoluteUri: uri);
    }
    /// <summary>
    ///     Opens a web page in the default browser.
    /// </summary>
    /// <param name="absoluteUri">
    ///     The absolute uri of the web page to open.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the uri was launched;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Open(Uri absoluteUri) {
        if (!absoluteUri.IsAbsoluteUri) {
            //Log.Warn($"Could not open '{absoluteUri}', it's not an absolute uri.");
            //Sounds.ActionFail();
            return false;
        }
        return OpenItemCore(path: absoluteUri.AbsoluteUri, arguments: null, useShellExecute: true);
    }

    private static bool OpenItemCore(string path, string? arguments, bool useShellExecute) {
        ProcessStartInfo? startInfo = GetProcessStartInfo(path: path, arguments: arguments, useShellExecute: useShellExecute);

        if (startInfo is null) {
            return false;
        }

        using var process = Process.Start(startInfo);
        return true;
    }
    private static bool ShowItemCore(string path, bool useShellExecute) {
        ProcessStartInfo? startInfo = GetShowItemProcessStartInfo(path: path, useShellExecute: useShellExecute);

        if (startInfo is null) {
            return false;
        }

        using var process = Process.Start(startInfo);
        return true;
    }

    private static ProcessStartInfo? GetProcessStartInfo(string? path, string? arguments, bool useShellExecute) {
        // Check if the OS platform is windows.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            // If the process is null/empty/whitespace, just return.
            // Windows requires a process specified.
            if (path.IsNullEmptyWhitespace()) {
                return null;
            }

            // Return the new start info for the parameters.
            return new ProcessStartInfo(fileName: path, arguments: arguments?.Trim() ?? string.Empty) {
                UseShellExecute = useShellExecute,
            };
        }

        // Check if both process and arguments are null/empty/whitespace.
        // If they are, we need to return, since Linux and OS-X require a special command.
        if (path.IsNullEmptyWhitespace()) {
            if (arguments.IsNullEmptyWhitespace()) {
                return null;
            }
            // Just trim the arguments, since they will only be used.
            arguments = arguments.Trim();
        }
        else {
            // Combine the process and arguments into the arguments string.
            arguments = arguments.IsNullEmptyWhitespace() ? path.Trim() : path.Trim() + " " + arguments.Trim();
        }

        // Check if the OS platform is linux.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            // Return the new start info for the parameters.
            return new ProcessStartInfo(fileName: "xdg-open", arguments: arguments) {
                UseShellExecute = true,
            };
        }

        // Check if the OS platform is os-x.
        if (RuntimeInformation.IsOSPlatform (OSPlatform.OSX)) {
            // Return the new start info for the parameters.
            return new ProcessStartInfo(fileName: "open", arguments: arguments) {
                UseShellExecute = true,
            };
        }

        return null;
    }
    private static ProcessStartInfo? GetShowItemProcessStartInfo(string? path, bool useShellExecute) {
        //Process.Start(ExplorerPath, "/select, \"" + fullPath + "\"");
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new(ExplorerPath) {
            Arguments = $"/select, \"{path}\"",
            UseShellExecute = useShellExecute,
        } :
        RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? new("xdg-open") {
            Arguments = Path.GetDirectoryName(path),
            UseShellExecute = true,
        } :
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new("open") {
            Arguments = "-R " + path,
            UseShellExecute = true,
        } :
        null;
    }

    private static string ExplorerPath {
        get {
            return Environment.GetFolderPath(Environment.SpecialFolder.Windows) + Path.DirectorySeparatorChar + "explorer.exe";
        }
    }
}
