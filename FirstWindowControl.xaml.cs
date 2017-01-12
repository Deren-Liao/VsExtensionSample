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

    using GoogleCloudExtension.SolutionUtils;
    using System.Text;
    using EnvDTE;
    using System.Linq;
    //using Microsoft.VisualStudio.Shell.Interop;
    //using Microsoft.VisualStudio;
    //using Microsoft.VisualStudio.OLE.Interop;
    //using Microsoft.VisualStudio.Shell;

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
            var s = SolutionHelper.CurrentSolution._solution;
            
            foreach (Project p in s.Projects)
            {
                NavigateProject(p);
            }
        }


        private void NavigateProject(Project project )
        {
            textBox.Text = $"{project.Name}\n{project.FullName}\n{project.UniqueName}";

            StringBuilder text = new StringBuilder();
            foreach (ProjectItem pi in project.ProjectItems)
            {
                string kind = DecodeProjectItemKind(pi.Kind);
                if (kind == "File")
                {
                    text.AppendLine($"{pi.Name}, {kind}");

                    if (pi.Name.ToLower() == "Startup.cs".ToLower())
                    {
                        var w = pi.Open(Constants.vsViewKindPrimary);  //Constants.vsViewKindCode);
                        _w = w;
                        w.Visible = true;
                        //OpenProjectItemInView(pi);
                    }
                }
            }

            _projectItems.Text = text.ToString();
        }

        private string DecodeProjectItemKind(string kind)
        {
            switch(kind)
            {
                case EnvDTE.Constants.vsProjectItemKindPhysicalFile:
                    return "File";
                default:
                    return "other";
            }
        }

        private void OpenProjectItemInView(EnvDTE.ProjectItem projectItem)
        {
            //string fullPath;
            //IVsWindowFrame windowFrame;

            //fullPath = projectItem.get_FileNames(0);
            
            //IVsUIShellOpenDocument shellOpenDocument = (IVsUIShellOpenDocument)FirstWindowPackage.GetGlobalService(typeof(IVsUIShellOpenDocument));

            //windowFrame = VsShellUtilities.TryOpenDocument()

            //if (windowFrame != null)
            //{
            //    windowFrame.Show();
            //}
        }

        private EnvDTE.Window _w;

        private void Move()
        {
            if (_w == null) return;

            var d = _w.Document;
            //TextDocument td = (EnvDTE.TextDocument)d;
            //var editPoint = td.StartPoint.CreateEditPoint();
            //editPoint.MoveToLineAndOffset(5, 0);

            // Perform selection
            TextSelection selection = d.Selection as TextSelection;
            selection.MoveToAbsoluteOffset(55, Extend: false);


            //// Show the currently selected line at the top of the editor if possible
            TextPoint tp = (TextPoint)selection.TopPoint;
            tp.TryToShow(vsPaneShowHow.vsPaneShowTop, null);

            selection.GotoLine(10, Select: false);

            //VirtualPoint objActive = selection.ActivePoint;
            //var editPoint = objActive.CreateEditPoint();
            //editPoint.MoveToLineAndOffset(10, 0);
        }

        private void move_Click(object sender, RoutedEventArgs e)
        {
            Move();
        }
    }
}