﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Observable;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration
{
    /// <summary>
    /// Represents a flat file configuration element that can create an instance of a flat file sink.
    /// </summary>
    internal class RollingFlatFileSinkElement : ISinkElement
    {
        private readonly XName sinkName = XName.Get("rollingFlatFileSink", Constants.Namespace);

        /// <summary>
        /// Determines whether this instance can create the specified configuration element.
        /// </summary>
        /// <param name="element">The configuration element.</param>
        /// <returns>
        ///   <c>True</c> if this instance can create the specified element; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated with Guard class")]
        public bool CanCreateSink(XElement element)
        {
            Guard.ArgumentNotNull(element, "element");

            return element.Name == this.sinkName;
        }

        /// <summary>
        /// Creates the <see cref="IObserver{EventEntry}" /> instance for this sink.
        /// </summary>
        /// <param name="element">The configuration element.</param>
        /// <returns>
        /// The sink instance.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated with Guard class")]
        public IObserver<EventEntry> CreateSink(XElement element)
        {
            Guard.ArgumentNotNull(element, "element");

            int rollSizeKB = (int?)element.Attribute("rollSizeKB") ?? default(int);
            RollFileExistsBehavior rollFileExistsBehavior = (RollFileExistsBehavior)Enum.Parse(typeof(RollFileExistsBehavior), (string)element.Attribute("rollFileExistsBehavior") ?? default(RollFileExistsBehavior).ToString());
            RollInterval rollInterval = (RollInterval)Enum.Parse(typeof(RollInterval), (string)element.Attribute("rollInterval") ?? default(RollInterval).ToString());
            int maxArchivedFiles = (int?)element.Attribute("maxArchivedFiles") ?? default(int);

            var subject = new EventEntrySubject();
            subject.LogToRollingFlatFile(
                (string)element.Attribute("fileName"),
                rollSizeKB,
                (string)element.Attribute("timeStampPattern"),
                rollFileExistsBehavior,
                rollInterval,
                FormatterElementFactory.Get(element),
                maxArchivedFiles);

            return subject;
        }
    }
}
