// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.EditAndContinue;
using Microsoft.CodeAnalysis.Editor.Shared.Utilities;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.LanguageServices.Implementation.ProjectSystem;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Microsoft.CodeAnalysis.Editor.Implementation.EditAndContinue
{
    internal sealed class VsReadOnlyDocumentTracker : ForegroundThreadAffinitizedObject, IDisposable
    {
        private readonly IEditAndContinueWorkspaceService _encService;
        private readonly IVsEditorAdaptersFactoryService _adapters;
        private readonly Workspace _workspace;

        private bool _isDisposed;
        private Func<DocumentId, bool> _allowsReadOnly;

        public VsReadOnlyDocumentTracker(IEditAndContinueWorkspaceService encService, IVsEditorAdaptersFactoryService adapters, Func<DocumentId, bool> allowsReadOnly)
            : base(assertIsForeground: true)
        {
            Debug.Assert(encService.DebuggingSession != null);

            _encService = encService;
            _adapters = adapters;
            _workspace = encService.DebuggingSession.InitialSolution.Workspace;
            _allowsReadOnly = allowsReadOnly;

            _workspace.DocumentOpened += OnDocumentOpened;
            UpdateWorkspaceDocuments();
        }

        public Workspace Workspace
        {
            get { return _workspace; }
        }

        private void OnDocumentOpened(object sender, DocumentEventArgs e)
        {
            InvokeBelowInputPriority(() =>
            {
                if (!_isDisposed)
                {
                    SetReadOnly(e.Document);
                }
            });
        }

        internal void UpdateWorkspaceDocuments()
        {
            foreach (var documentId in _workspace.GetOpenDocumentIds())
            {
                var document = _workspace.CurrentSolution.GetDocument(documentId);
                Debug.Assert(document != null);

                SetReadOnly(document);
            }
        }

        public void Dispose()
        {
            AssertIsForeground();

            if (_isDisposed)
            {
                return;
            }

            _workspace.DocumentOpened -= OnDocumentOpened;

            UpdateWorkspaceDocuments();

            // event handlers may be queued after the disposal - they will be a no-op
            _isDisposed = true;
        }

        private void SetReadOnly(Document document)
        {
            // Only set documents read-only if they're part of a project that supports Enc.
            var workspace = document.Project.Solution.Workspace as VisualStudioWorkspaceImpl;
            var project = workspace?.ProjectTracker?.GetProject(document.Project.Id) as AbstractEncProject;

            if (project != null)
            {
                SessionReadOnlyReason sessionReason;
                ProjectReadOnlyReason projectReason;
                SetReadOnly(document.Id, _encService.IsProjectReadOnly(document.Project.Id, out sessionReason, out projectReason));
            }
        }

        internal void SetReadOnly(DocumentId documentId, bool value)
        {
            AssertIsForeground();
            Debug.Assert(!_isDisposed);

            var textBuffer = GetTextBuffer(_workspace, documentId);
            if (textBuffer != null)
            {
                SetReadOnlyFlag(textBuffer, value && _allowsReadOnly(documentId));
            }
        }

        private void SetReadOnlyFlag(ITextBuffer buffer, bool value)
        {
            var vsBuffer = _adapters.GetBufferAdapter(buffer);

            uint oldFlags;
            uint newFlags;
            vsBuffer.GetStateFlags(out oldFlags);
            if (value)
            {
                newFlags = oldFlags | (uint)BUFFERSTATEFLAGS.BSF_USER_READONLY;
            }
            else
            {
                newFlags = oldFlags & ~(uint)BUFFERSTATEFLAGS.BSF_USER_READONLY;
            }

            if (oldFlags != newFlags)
            {
                vsBuffer.SetStateFlags(newFlags);
            }
        }

        private static ITextBuffer GetTextBuffer(Workspace workspace, DocumentId documentId)
        {
            var doc = workspace.CurrentSolution.GetDocument(documentId);
            SourceText text;
            if (!doc.TryGetText(out text))
            {
                // TODO: should not happen since the document is open (see bug 896058)
                return null;
            }

            var snapshot = text.FindCorrespondingEditorTextSnapshot();
            if (snapshot == null)
            {
                return null;
            }

            return snapshot.TextBuffer;
        }
    }
}
