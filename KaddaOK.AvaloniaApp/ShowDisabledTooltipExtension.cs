using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;
using System.Linq;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace KaddaOK.AvaloniaApp
{
    // per tkefauver at https://github.com/AvaloniaUI/Avalonia/issues/3847#issuecomment-1618790059
    public static class ShowDisabledTooltipExtension
    {
        #region Constructors
        static ShowDisabledTooltipExtension()
        {
            ShowOnDisabledProperty.Changed.AddClassHandler<Control>(HandleShowOnDisabledChanged);
        }

        #endregion

        #region Properties

        #region ShowOnDisabled AvaloniaProperty
        public static bool GetShowOnDisabled(AvaloniaObject obj)
        {
            return obj.GetValue(ShowOnDisabledProperty);
        }

        public static void SetShowOnDisabled(AvaloniaObject obj, bool value)
        {
            obj.SetValue(ShowOnDisabledProperty, value);
        }

        public static readonly AttachedProperty<bool> ShowOnDisabledProperty = AvaloniaProperty.RegisterAttached<object, Control, bool>("ShowOnDisabled");

        private static void HandleShowOnDisabledChanged(Control control, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is true)
            {
                control.DetachedFromVisualTree += AttachedControl_DetachedFromVisualOrExtension;
                control.AttachedToVisualTree += AttachedControl_AttachedToVisualTree;
                if (control.IsInitialized)
                {
                    // enabled after visual attached
                    AttachedControl_AttachedToVisualTree(control, null);
                }
            }
            else
            {
                AttachedControl_DetachedFromVisualOrExtension(control, null);
            }

        }

        private static void AttachedControl_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs? e)
        {
            if (sender is not Control control)
            {
                return;
            }
            var tl = TopLevel.GetTopLevel(control);
            // NOTE PointerMoved needed to be tunneled for tkefauver but we may not need to...
            tl?.AddHandler(InputElement.PointerMovedEvent, TopLevel_PointerMoved, RoutingStrategies.Tunnel);
        }


        private static void AttachedControl_DetachedFromVisualOrExtension(object? s, VisualTreeAttachmentEventArgs? e)
        {
            if (s is not Control control)
            {
                return;
            }
            control.DetachedFromVisualTree -= AttachedControl_DetachedFromVisualOrExtension;
            control.AttachedToVisualTree -= AttachedControl_AttachedToVisualTree;
            if (TopLevel.GetTopLevel(control) is not TopLevel tl)
            {
                return;
            }
            tl.RemoveHandler(InputElement.PointerMovedEvent, TopLevel_PointerMoved);
        }

        private static void TopLevel_PointerMoved(object? sender, global::Avalonia.Input.PointerEventArgs e)
        {
            if (sender is not Control tl)
            {
                return;
            }
            var attached_controls =
                tl.GetVisualDescendants().Where(GetShowOnDisabled).Cast<Control>().ToList();

            // find disabled children under pointer w/ this extension enabled
            var disabled_child_under_pointer =
                attached_controls
                    .FirstOrDefault(x =>
                        x.Bounds.Contains(e.GetPosition(x.Parent as Visual)) &&
                        x.IsEffectivelyVisible &&
                        !x.IsEffectivelyEnabled);
            if (disabled_child_under_pointer != null)
            {
                // manually show tooltip
                ToolTip.SetIsOpen(disabled_child_under_pointer, true);
            }
            var disabled_tooltips_to_hide =
                    attached_controls
                        .Where(x =>
                            ToolTip.GetIsOpen(x) &&
                            x != disabled_child_under_pointer &&
                            !x.IsEffectivelyEnabled);
            foreach (var dcst in disabled_tooltips_to_hide)
            {
                ToolTip.SetIsOpen(dcst, false);
            }
        }
        #endregion

        #endregion
    }
}
