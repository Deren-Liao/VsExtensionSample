using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Adornments;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.VisualStudio.Text.Formatting;

using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Documents;
using System.Linq;


using System.Diagnostics;

namespace ToolWindow
{
    [Export(typeof(IMouseProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("text")]
    [Name("MyCustomTooltip")]
    internal sealed class CustomTooltipHandlerProvider : IMouseProcessorProvider
    {
        [Import]
        public IToolTipProviderFactory ToolTipProviderFactory { get; set; }

        [Import]
        public ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import]
        internal IClassifierAggregatorService AggregatorService { get; set; }

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            return new CustomTooltipMouseProcessor(view, this.ToolTipProviderFactory, 
                this.NavigatorService.GetTextStructureNavigator(view.TextBuffer), 
                this.AggregatorService.GetClassifier(view.TextBuffer));
        }
    }

    internal class CustomTooltipMouseProcessor : MouseProcessorBase
    {
        private readonly IWpfTextView _view;

        private readonly IToolTipProvider _toolTipProvider;
        private readonly ITextStructureNavigator _navigatorService;
        private readonly IClassifier _classifier;

        private readonly ICollection<FontFamily> _systemFonts;

        private bool IsToolTipShown { get; set; }

        internal CustomTooltipMouseProcessor(IWpfTextView view, IToolTipProviderFactory
            toolTipProviderFactory, ITextStructureNavigator navigatorService, IClassifier classifier)
        {
            this._view = view;
            this._toolTipProvider = toolTipProviderFactory.GetToolTipProvider(this._view);
            this._navigatorService = navigatorService;
            this._classifier = classifier;

            this._systemFonts = Fonts.SystemFontFamilies;

            this.IsToolTipShown = false;
        }

        public override void PreprocessMouseMove(MouseEventArgs e)
        {
            return;

            if (FirstWindowControl.CurrentTextViewer != _view)
            {
                Debug.WriteLine("FirstWindowControl.CurrentTextViewer != _view");
                return;
            }

            var colorConverter = new ColorConverter();

            SnapshotSpan? spanAtMousePosition = 
                SpanHelpers.GetSpanAtMousePosition(this._view, this._navigatorService);
            if (spanAtMousePosition.HasValue)
            {
                var textAtMousePosition = spanAtMousePosition.Value.GetText();

                if (!string.IsNullOrWhiteSpace(textAtMousePosition))
                {
                    if (!this.IsToolTipShown)
                    {
                        this.IsToolTipShown = true;

                        this._toolTipProvider.ClearToolTip();
                        this._toolTipProvider.ShowToolTip(
                            spanAtMousePosition.Value.Snapshot.CreateTrackingSpan(
                                spanAtMousePosition.Value.Span, SpanTrackingMode.EdgeExclusive),
                            new Border
                            {
                                Background = new SolidColorBrush(Colors.LightGray),
                                Padding = new Thickness(10),
                                Child = new StackPanel
                                {
                                    Orientation = Orientation.Horizontal,
                                    Children =
                                    {
                                        new Rectangle
                                        {
                                            Height = 30,
                                            Width = 30,
                                            Fill = new SolidColorBrush(Colors.Red)
                                        },
                                        new TextBlock
                                        {
                                            Margin = new Thickness(10, 0, 0, 0),
                                            Inlines =
                                            {
                                                new Run(textAtMousePosition)
                                            }
                                        }
                                    }
                                }
                            }, PopupStyles.PositionClosest);
                        return;
                    }
                }
            }

            this._toolTipProvider.ClearToolTip();
            IsToolTipShown = false;
        }
    }
}
