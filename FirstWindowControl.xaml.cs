//------------------------------------------------------------------------------
// <copyright file="FirstWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace ToolWindow
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    using System.ComponentModel.Composition;

    using GoogleCloudExtension.SolutionUtils;
    using System.Text;
    using EnvDTE;
    using System.Linq;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Editor;

    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Adornments;

    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for FirstWindowControl.
    /// </summary>
    public partial class FirstWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstWindowControl"/> class.
        /// </summary>
        public FirstWindowControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var projectItems = SolutionHelper.CurrentSolution.FindMatchingSourceFile("Startup.cs");
            _w = projectItems?[0].GotoLine(10);
            ShowTip();
        }

        private void OpenAssemblyInfo()
        {
            var projectItems = SolutionHelper.CurrentSolution.FindMatchingSourceFile("AssemblyInfo.cs");
            projectItems?[0].GotoLine(5);
        }

        //    var s = SolutionHelper.CurrentSolution._solution;

        //    foreach (Project p in s.Projects)
        //    {
        //        NavigateProject(p);
        //    }
        //}


        //private void NavigateProject(Project project )
        //{
        //    textBox.Text = $"{project.Name}\n{project.FullName}\n{project.UniqueName}";

        //    StringBuilder text = new StringBuilder();
        //    foreach (ProjectItem pi in project.ProjectItems)
        //    {
        //        string kind = DecodeProjectItemKind(pi.Kind);
        //        if (kind == "File")
        //        {
        //            text.AppendLine($"{pi.Name}, {kind}");

        //            if (pi.Name.ToLower() == "Startup.cs".ToLower())
        //            {
        //                var w = pi.Open(EnvDTE.Constants.vsViewKindPrimary);  //Constants.vsViewKindCode);
        //                _w = w;
        //                w.Visible = true;
        //                //OpenProjectItemInView(pi);
        //            }
        //        }
        //    }

        //    _projectItems.Text = text.ToString();
        //}

        //private string DecodeProjectItemKind(string kind)
        //{
        //    switch(kind)
        //    {
        //        case EnvDTE.Constants.vsProjectItemKindPhysicalFile:
        //            return "File";
        //        default:
        //            return "other";
        //    }
        //}

        private void OpenProjectItemInView(EnvDTE.ProjectItem projectItem)
        {
            //public static void OpenDocumentWithSpecificEditor(
            //    IServiceProvider provider,
            //    string fullPath,
            //    Guid editorType,
            //    Guid logicalView,
            //    out IVsUIHierarchy hierarchy,
            //    out uint itemID,
            //    out IVsWindowFrame windowFrame
            //)

            Guid guid_microsoft_visual_basic_editor = new Guid("{2C015C70-C72C-11D0-88C3-00A0C9110049}");
            Guid guid_microsoft_visual_basic_editor_with_encoding = new Guid("{6C33E1AA-1401-4536-AB67-0E21E6E569DA}");
            Guid guid_microsoft_csharp_editor = new Guid("{A6C744A8-0E4A-4FC6-886A-064283054674}");
            Guid guid_microsoft_csharp_editor_with_encoding = new Guid("{08467b34-b90f-4d91-bdca-eb8c8cf3033a}");
            // Microsoft.VisualStudio.VSConstants.GUID_TextEditorFactory
            Guid guid_source_code_text_editor = new Guid("{8B382828-6202-11d1-8870-0000F87579D2}");
            Guid guid_source_code_text_editor_with_encoding = new Guid("{C7747503-0E24-4FBE-BE4B-94180C3947D7}");

            var sp = GetGloblalServiceProvider();

            IVsUIHierarchy hierarchy;
            uint itemID;
            IVsWindowFrame windowFrame;
            VsShellUtilities.TryOpenDocument(sp, projectItem.FileNames[0], guid_microsoft_csharp_editor_with_encoding,
                out hierarchy, out itemID, out windowFrame);

        }

        private EnvDTE.Window _w;


        //private void Move()
        //{
        //    if (_w == null) return;

        //    var d = _w.Document;
        //    //TextDocument td = (EnvDTE.TextDocument)d;
        //    //var editPoint = td.StartPoint.CreateEditPoint();
        //    //editPoint.MoveToLineAndOffset(5, 0);

        //    // Perform selection
        //    TextSelection selection = d.Selection as TextSelection;
        //    selection.MoveToAbsoluteOffset(55, Extend: false);


        //    //// Show the currently selected line at the top of the editor if possible
        //    TextPoint tp = (TextPoint)selection.TopPoint;
        //    tp.TryToShow(vsPaneShowHow.vsPaneShowTop, null);

        //    selection.GotoLine(10, Select: false);

        //    //VirtualPoint objActive = selection.ActivePoint;
        //    //var editPoint = objActive.CreateEditPoint();
        //    //editPoint.MoveToLineAndOffset(10, 0);
        //}

        private void move_Click(object sender, RoutedEventArgs e)
        {
            OpenAssemblyInfo();
        }

        //[Import]
        //public IToolTipProviderFactory ToolTipProviderFactory { get; set; }

        //[Import]
        //public ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        //[Import]
        //internal IClassifierAggregatorService AggregatorService { get; set; }

        //[Import]
        //internal ITextSearchService TextSearchService { get; set; }

        //[Import]
        //internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

        public static IWpfTextView CurrentTextViewer { get; private set; }
        private IToolTipProvider _toolTipProvider;
        private ITextStructureNavigator _navigatorService;
        private IClassifier _classifier;
        private bool _isToolTipShown;

        private void ShowTip()
        {
            //IVsWindowFrame frame = _w.LinkedWindowFrame as IVsWindowFrame;
            //IVsTextView textView = VsShellUtilities.GetTextView(frame);
            IVsTextView textView  = GetIVsTextView(_w.Document.FullName);
            CurrentTextViewer = GetWpfTextView(textView);
            //_toolTipProvider = ToolTipProviderFactory.GetToolTipProvider(this._view);
            //_navigatorService = NavigatorService.GetTextStructureNavigator(_view.TextBuffer);
            //_classifier = AggregatorService.GetClassifier(_view.TextBuffer);

            //SearchMove(_view);

            if (LoggingTagger.LoggingTaggerCollection != null)
            {
                LoggingTagger loggingTagger;
                if (LoggingTagger.LoggingTaggerCollection.TryGetValue(CurrentTextViewer, out loggingTagger))
                {
                    loggingTagger.UpdateAtCaretPosition(CurrentTextViewer.Caret.ContainingTextViewLine);
                }
            }
        }

        //private void SearchMove(IWpfTextView view)
        //{
        //    var _buffer = view.TextBuffer;
        //    SnapshotSpan? spanAtMousePosition =
        //        SpanHelpers.GetSpanAtMousePosition(this._view, this._navigatorService);
        //    if (spanAtMousePosition.HasValue && !_isToolTipShown)
        //    {
        //        _isToolTipShown = true;
        //        _toolTipProvider.ClearToolTip();
        //        _toolTipProvider.ShowToolTip(
        //            spanAtMousePosition.Value.Snapshot.CreateTrackingSpan(
        //                spanAtMousePosition.Value.Span, SpanTrackingMode.EdgeExclusive),
        //            new Border
        //            {
        //                Background = new SolidColorBrush(Colors.LightGray),
        //                Padding = new Thickness(10),
        //                Child = new StackPanel
        //                {
        //                    Orientation = Orientation.Horizontal,
        //                    Children =
        //                    {
        //                        new Rectangle
        //                        {
        //                            Height = 30,
        //                            Width = 30,
        //                            Fill = new SolidColorBrush(Colors.Red)
        //                        }
        //                        //,
        //                        //new TextBlock
        //                        //{
        //                        //    Margin = new Thickness(10, 0, 0, 0),
        //                        //    Inlines =
        //                        //    {
        //                        //        new Run(xamlColor.ToString()),
        //                        //        new Run(" ("),
        //                        //        copyHexColorHyperlink,
        //                        //        new Run(")"),
        //                        //        new LineBreak(),
        //                        //        new Run(string.Format("{0}", xamlColor.ToString())),
        //                        //    }
        //                        //}
        //                    }
        //                }
        //            }, PopupStyles.PositionClosest);
        //    }        
        //}

        /// <summary>
        /// Returns an IVsTextView for the given file path, if the given file is open in Visual Studio.
        /// </summary>
        /// <param name="filePath">Full Path of the file you are looking for.</param>
        /// <returns>The IVsTextView for this file, if it is open, null otherwise.</returns>
        internal static IVsTextView GetIVsTextView(string filePath)
        {
            var sp = GetGloblalServiceProvider();


            Microsoft.VisualStudio.Shell.Interop.IVsUIHierarchy uiHierarchy;
            uint itemID;
            Microsoft.VisualStudio.Shell.Interop.IVsWindowFrame windowFrame;
            IWpfTextView wpfTextView = null;
            if (Microsoft.VisualStudio.Shell.VsShellUtilities.IsDocumentOpen(
                sp, filePath, Guid.Empty,
                out uiHierarchy, out itemID, out windowFrame))
            {
                // Get the IVsTextView from the windowFrame.
                return Microsoft.VisualStudio.Shell.VsShellUtilities.GetTextView(windowFrame);
            }

            return null;
        }

        private IWpfTextView GetWpfTextView(IVsTextView vTextView)
        {
            IWpfTextView view = null;
            IVsUserData userData = vTextView as IVsUserData;

            if (null != userData)
            {
                IWpfTextViewHost viewHost;
                object holder;
                Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
                userData.GetData(ref guidViewHost, out holder);
                viewHost = (IWpfTextViewHost)holder;
                view = viewHost.TextView;
            }

            return view;
        }


        internal static ServiceProvider GetGloblalServiceProvider()
        {
            var dte2 = (EnvDTE80.DTE2)Microsoft.VisualStudio.Shell.Package.GetGlobalService(
                typeof(Microsoft.VisualStudio.Shell.Interop.SDTE));
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp =
                (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte2;
            return new Microsoft.VisualStudio.Shell.ServiceProvider(sp);
        }

        private void _project_Click(object sender, RoutedEventArgs e)
        {
            var project = SolutionHelper.CurrentSolution.Projects?[0];

            textBox.Text += $"{project?.AssemblyName}\r\n";
            textBox.Text += $"{project?.Version}\r\n";

            ShowAllProperties();
        }

        private void ShowAllProperties()
        { 
            // Reset the TreeView to 0 items.  
            _treeView.Items.Clear();

            Projects projects = SolutionHelper.CurrentSolution.solution.Projects;
            if (projects.Count == 0)   // no project is open  
            {
                TreeViewItem item = new TreeViewItem();
                item.Name = "Projects";
                item.ItemsSource = new string[] { "no projects are open." };
                item.IsExpanded = true;
                _treeView.Items.Add(item);
                return;
            }

            Project project = projects.Item(1);
            TreeViewItem item1 = new TreeViewItem();
            item1.Header = project.Name + "Properties";
            _treeView.Items.Add(item1);

            foreach (Property property in project.Properties)
            {
                TreeViewItem item = new TreeViewItem();
                // if (property.Name.ToLower().Contains("assembly"))
                object value = null;
                try
                {
                    value = property.Value;
                }
                catch (Exception ex)
                {

                }

                {
                    item.ItemsSource = new string[] { $"{property.Name} : {value} " };
                    item.IsExpanded = true;
                    _treeView.Items.Add(item);
                }
            }
        }
    }
}