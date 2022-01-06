using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace WebService.Services
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Reads the OS specific directory for storing files on the file system
        /// </summary>
        /// <param name="config">The configuration for the web service</param>
        /// <returns>The directory path where files are to be stored</returns>
        /// <exception cref="NotSupportedException">If OS running web service is not Windows or Linux</exception>
        public static string ReadOSFileDirFromConfiguration(IConfiguration config)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return config.GetSection("WindowsDir").Value;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return config.GetSection("LinuxDir").Value;
            }

            throw new NotSupportedException("Current OS not supported");
        }
    }
}