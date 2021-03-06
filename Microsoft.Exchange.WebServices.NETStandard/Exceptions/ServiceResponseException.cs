/*
 * Exchange Web Services Managed API
 *
 * Copyright (c) Microsoft Corporation
 * All rights reserved.
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

namespace Microsoft.Exchange.WebServices.Data
{
    using System;

    /// <summary>
    /// Represents a remote service exception that has a single response.
    /// </summary>
    public class ServiceResponseException : ServiceRemoteException
    {
        /// <summary>
        /// Error details Value keys
        /// </summary>
        private const string ExceptionClassKey = "ExceptionClass";
        private const string ExceptionMessageKey = "ExceptionMessage";
        private const string StackTraceKey = "StackTrace";

        /// <summary>
        /// ServiceResponse when service operation failed remotely.
        /// </summary>
        private ServiceResponse response;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponseException"/> class.
        /// </summary>
        /// <param name="response">The ServiceResponse when service operation failed remotely.</param>
        internal ServiceResponseException(ServiceResponse response)
        {
            this.response = response;
        }

        /// <summary>
        /// Gets the ServiceResponse for the exception.
        /// </summary>
        public ServiceResponse Response
        {
            get { return this.response; }
        }

        /// <summary>
        /// Gets the service error code.
        /// </summary>
        public ServiceError ErrorCode
        {
            get { return this.response.ErrorCode; }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception.</returns>
        public override string Message
        {
            get
            {
                // Special case for Internal Server Error. If the server returned
                // stack trace information, include it in the exception message.
                if (this.Response.ErrorCode == ServiceError.ErrorInternalServerError)
                {
                    string exceptionClass;
                    string exceptionMessage;
                    string stackTrace;

                    if (this.Response.ErrorDetails.TryGetValue(ExceptionClassKey, out exceptionClass) &&
                        this.Response.ErrorDetails.TryGetValue(ExceptionMessageKey, out exceptionMessage) &&
                        this.Response.ErrorDetails.TryGetValue(StackTraceKey, out stackTrace))
                    {
                        return string.Format(
                            Strings.ServerErrorAndStackTraceDetails,
                            this.Response.ErrorMessage,
                            exceptionClass,
                            exceptionMessage,
                            stackTrace);
                    }
                }

                return this.Response.ErrorMessage;
            }
        }
    }
}