﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace ToolWindow
{
    internal class QuickInfoSource : IQuickInfoSource
    {
        private QuickInfoSourceProvider m_provider;
        private ITextBuffer m_subjectBuffer;
        private Dictionary<string, string> m_dictionary;

        public QuickInfoSource(QuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
        {
            m_provider = provider;
            m_subjectBuffer = subjectBuffer;

            //these are the method names and their descriptions
            m_dictionary = new Dictionary<string, string>();
            m_dictionary.Add("StackdriverLogging", "int add(int firstInt, int secondInt)\nAdds one integer to another.");
        }

        private bool m_isDisposed;
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                GC.SuppressFinalize(this);
                m_isDisposed = true;
            }
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> qiContent, 
            out ITrackingSpan applicableToSpan)
        {
            // Map the trigger point down to our buffer.
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(m_subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                applicableToSpan = null;
                return;
            }

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            SnapshotSpan querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = m_provider.NavigatorService.GetTextStructureNavigator(m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();

            foreach (string key in m_dictionary.Keys)
            {
                int foundIndex = searchText.IndexOf(key, StringComparison.CurrentCultureIgnoreCase);
                if (foundIndex > -1)
                {
                    applicableToSpan = currentSnapshot.CreateTrackingSpan
                        (
                                                //querySpan.Start.Add(foundIndex).Position, 9, SpanTrackingMode.EdgeInclusive
                                                extent.Span.Start + foundIndex, key.Length, SpanTrackingMode.EdgeInclusive
                        );

                    string value;
                    m_dictionary.TryGetValue(key, out value);
                    if (value != null)
                        qiContent.Add(value);
                    else
                        qiContent.Add("");

                    return;
                }
            }

            applicableToSpan = null;
        }
    }
}
