﻿namespace FFMediaToolkit.Helpers
{
    using System;

    /// <summary>
    /// Contains common methods for handling FFMpeg exceptions.
    /// </summary>
    internal static class ExceptionHandler
    {
        /// <summary>
        /// A delegate for error code handling.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        internal delegate void ErrorHandler(int errorCode);

        /// <summary>
        /// Checks if specified integer is error code and throws an <see cref="FFmpegException"/>.
        /// </summary>
        /// <param name="errorCode">Exit code returned by the FFMpeg method call.</param>
        /// <param name="message">The exception message.</param>
        [Obsolete]
        internal static void ThrowIfError(this int errorCode, string message)
        {
            if (errorCode >= 0)
            {
                return;
            }

            throw new FFmpegException(message, errorCode);
        }

        /// <summary>
        /// Checks if the integer is equal to the specified and excecutes the <see cref="ErrorHandler"/> method.
        /// </summary>
        /// <param name="errorCode">The exit code returned by a method.</param>
        /// <param name="handledError">The error code to handle.</param>
        /// <param name="action">The method to execute if error handled.</param>
        /// <param name="handles">If <see langword="true"/> this method after handling exception will return 0 instead of the original code.</param>
        /// <returns>Original error code or 0 if error handled and the <paramref name="handles"/> is <see langword="true"/>.</returns>
        internal static int Catch(this int errorCode, int handledError, ErrorHandler action, bool handles = true)
        {
            if (errorCode != handledError)
            {
                action(errorCode);
            }

            return handles ? 0 : errorCode;
        }

        /// <summary>
        /// Checks if the integer is equal to the <paramref name="handledError"/> and throws an <see cref="FFmpegException"/>.
        /// </summary>
        /// <param name="errorCode">The exit code returned by a method.</param>
        /// <param name="handledError">The error code to handle.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>The original error code.</returns>
        internal static int Catch(this int errorCode, int handledError, string exceptionMessage)
            => errorCode.Catch(handledError, x => throw new FFmpegException(exceptionMessage, x));

        /// <summary>
        /// Checks if specified integer is error code and throws an <see cref="FFmpegException"/>.
        /// </summary>
        /// <param name="errorCode">The exit code returned by a method.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        internal static void CatchAll(this int errorCode, string exceptionMessage)
        {
            if (errorCode >= 0)
            {
                return;
            }

            throw new FFmpegException(exceptionMessage, errorCode);
        }
    }
}
