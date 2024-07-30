using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Utilities;
using Irihi.Avalonia.Shared.Helpers;
using static System.Math;

namespace Ursa.Controls;

public class ElasticWrapPanel : WrapPanel
{
    static ElasticWrapPanel()
    {
        IsFillHorizontalProperty.Changed.AddClassHandler<Control>(OnIsFillPropertyChanged);
        IsFillVerticalProperty.Changed.AddClassHandler<Control>(OnIsFillPropertyChanged);

        AffectsMeasure<ElasticWrapPanel>(IsFillHorizontalProperty, IsFillVerticalProperty);
    }

    #region AttachedProperty

    public static void SetFixToRB(Control element, bool value)
    {
        _ = element ?? throw new ArgumentNullException(nameof(element));
        element.SetValue(FixToRBProperty, value);
    }

    public static bool GetIsFixToRB(Control element)
    {
        _ = element ?? throw new ArgumentNullException(nameof(element));
        return element.GetValue(FixToRBProperty);
    }

    /// <summary>
    /// Fixed to [Right (Horizontal Mode) | Bottom (Vertical Mode)]
    /// which will cause line breaks
    /// </summary>
    public static readonly AttachedProperty<bool> FixToRBProperty =
        AvaloniaProperty.RegisterAttached<ElasticWrapPanel, Control, bool>("FixToRB");

    #endregion

    #region StyledProperty

    public bool IsFillHorizontal
    {
        get => GetValue(IsFillHorizontalProperty);
        set => SetValue(IsFillHorizontalProperty, value);
    }

    public static readonly StyledProperty<bool> IsFillHorizontalProperty =
        AvaloniaProperty.Register<ElasticWrapPanel, bool>(nameof(IsFillHorizontal));

    public bool IsFillVertical
    {
        get => GetValue(IsFillVerticalProperty);
        set => SetValue(IsFillVerticalProperty, value);
    }

    public static readonly StyledProperty<bool> IsFillVerticalProperty =
        AvaloniaProperty.Register<ElasticWrapPanel, bool>(nameof(IsFillVertical));

    private static void OnIsFillPropertyChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
    {
        (d as ElasticWrapPanel)?.InvalidateMeasure();
    }

    #endregion

    protected override Size MeasureOverride(Size constraint)
    {
        double itemWidth = ItemWidth;
        double itemHeight = ItemHeight;
        var orientation = Orientation;
        var children = Children;

        // Determine the space required for items in the same row/column based on horizontal/vertical arrangement
        var curLineSize = new UVSize(orientation);

        // Calculate the total space requirement for this ElasticWrapPanel
        var panelSize = new UVSize(orientation);

        // Measure UVSize with the given space constraint, used for measuring elements when ItemWidth and ItemHeight are not set
        var uvConstraint = new UVSize(orientation, constraint.Width, constraint.Height);
        bool itemWidthSet = !double.IsNaN(itemWidth);
        bool itemHeightSet = !double.IsNaN(itemHeight);

        var childConstraint = new Size(
            itemWidthSet ? itemWidth : constraint.Width,
            itemHeightSet ? itemHeight : constraint.Height);

        // Measurement space for elements with FixToRB=True
        Size childFixConstraint = new Size(constraint.Width, constraint.Height);
        switch (orientation)
        {
            case Orientation.Horizontal when itemHeightSet:
                childFixConstraint = new Size(constraint.Width, itemHeight);
                break;
            case Orientation.Vertical when itemWidthSet:
                childFixConstraint = new Size(itemWidth, constraint.Height);
                break;
        }

        // This is the size for non-space measurement
        UVSize itemSetSize = new UVSize(orientation,
            itemWidthSet ? itemWidth : 0,
            itemHeightSet ? itemHeight : 0);

        foreach (var child in children)
        {
            UVSize sz;
            if (GetIsFixToRB(child))
            {
                // Measure the element when it needs to be fixed to the right/bottom
                child.Measure(childFixConstraint);
                sz = new UVSize(orientation, child.DesiredSize.Width, child.DesiredSize.Height);

                // Ensure the width/height is within the constraint limits
                if (sz.U > 0 && itemSetSize.U > 0)
                {
                    if (sz.U < itemSetSize.U)
                    {
                        sz.U = itemSetSize.U;
                    }
                    else
                    {
                        sz.U = Min(sz.U, uvConstraint.U);
                    }
                }

                if (sz.V > 0 && itemSetSize.V > 0 && sz.V < itemSetSize.V)
                {
                    sz.V = itemSetSize.V;
                }

                if (MathHelpers.GreaterThan(curLineSize.U + sz.U, uvConstraint.U))
                {
                    panelSize.U = Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;
                }
                else
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Max(sz.V, curLineSize.V);
                    panelSize.U = Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                }

                curLineSize = new UVSize(orientation);
            }
            else
            {
                // Flow passes its own constraint to children
                child.Measure(childConstraint);

                // This is the size of the child in UV space
                sz = new UVSize(orientation,
                    itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    itemHeightSet ? itemHeight : child.DesiredSize.Height);

                if (MathHelpers.GreaterThan(curLineSize.U + sz.U, uvConstraint.U)) // Need to switch to another line
                {
                    panelSize.U = Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    if (MathHelpers.GreaterThan(sz.U, uvConstraint.U)) // The element is wider than the constraint - give it a separate line
                    {
                        panelSize.U = Max(sz.U, panelSize.U);
                        panelSize.V += sz.V;
                        curLineSize = new UVSize(orientation);
                    }
                }
                else // Continue to accumulate a line
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Max(sz.V, curLineSize.V);
                }
            }
        }

        // The last line size, if any should be added
        panelSize.U = Max(curLineSize.U, panelSize.U);
        panelSize.V += curLineSize.V;

        // Go from UV space to W/H space
        return new Size(panelSize.Width, panelSize.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        bool itemWidthSet = !double.IsNaN(ItemWidth);
        bool itemHeightSet = !double.IsNaN(ItemHeight);

        // This is the size for non-space measurement
        UVSize itemSetSize = new UVSize(Orientation,
            itemWidthSet ? ItemWidth : 0,
            itemHeightSet ? ItemHeight : 0);

        // Measure UVSize with the given space constraint, used for measuring elements when ItemWidth and ItemHeight are not set
        UVSize uvFinalSize = new UVSize(Orientation, finalSize.Width, finalSize.Height);

        // Collection of elements in the same direction (row/column)
        List<UVCollection> lineUVCollection = new List<UVCollection>();

        #region Get the collection of elements in the same direction

        // Current collection of elements in a row/column
        UVCollection curLineUIs = new UVCollection(Orientation, itemSetSize);

        // Iterate over the child elements
        var children = Children;
        foreach (var child in children)
        {
            UVSize sz;
            if (GetIsFixToRB(child))
            {
                // Measure the element when it needs to be fixed to the right/bottom
                sz = new UVSize(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                double lengthCount = 1;
                if (sz.U > 0 && itemSetSize.U > 0)
                {
                    if (sz.U < itemSetSize.U)
                    {
                        sz.U = itemSetSize.U;
                    }
                    else
                    {
                        lengthCount = Ceiling(sz.U / itemSetSize.U);
                        sz.U = Min(sz.U, uvFinalSize.U);
                    }
                }

                if (sz.V > 0 && itemSetSize.V > 0 && sz.V < itemSetSize.V)
                {
                    sz.V = itemSetSize.V;
                }

                if (MathHelpers.GreaterThan(curLineUIs.TotalU + sz.U, uvFinalSize.U))
                {
                    if (curLineUIs.Count > 0)
                    {
                        lineUVCollection.Add(curLineUIs);
                    }

                    curLineUIs = new UVCollection(Orientation, itemSetSize);
                    curLineUIs.Add(child, sz, Convert.ToInt32(lengthCount));
                }
                else
                {
                    curLineUIs.Add(child, sz, Convert.ToInt32(lengthCount));
                }

                lineUVCollection.Add(curLineUIs);
                curLineUIs = new UVCollection(Orientation, itemSetSize);
            }
            else
            {
                sz = new UVSize(Orientation,
                    itemWidthSet ? ItemWidth : child.DesiredSize.Width,
                    itemHeightSet ? ItemHeight : child.DesiredSize.Height);

                if (MathHelpers.GreaterThan(curLineUIs.TotalU + sz.U, uvFinalSize.U)) // Need to switch to another line
                {
                    if (curLineUIs.Count > 0)
                    {
                        lineUVCollection.Add(curLineUIs);
                    }

                    curLineUIs = new UVCollection(Orientation, itemSetSize);
                    curLineUIs.Add(child, sz);
                    if (MathHelpers.GreaterThan(sz.U, uvFinalSize.U))
                    {
                        lineUVCollection.Add(curLineUIs);
                        curLineUIs = new UVCollection(Orientation, itemSetSize);
                    }
                }
                else
                {
                    curLineUIs.Add(child, sz);
                }
            }
        }

        if (curLineUIs.Count > 0 && !lineUVCollection.Contains(curLineUIs))
        {
            lineUVCollection.Add(curLineUIs);
        }

        #endregion

        bool isFillU = false;
        bool isFillV = false;
        switch (Orientation)
        {
            case Orientation.Horizontal:
                isFillU = IsFillHorizontal;
                isFillV = IsFillVertical;
                break;

            case Orientation.Vertical:
                isFillU = IsFillVertical;
                isFillV = IsFillHorizontal;
                break;
        }

        if (lineUVCollection.Count > 0)
        {
            double accumulatedV = 0;
            double adaptULength = 0;
            bool isAdaptV = false;
            double adaptVLength = 0;
            if (isFillU)
            {
                if (itemSetSize.U > 0)
                {
                    int maxElementCount = lineUVCollection
                        .Max(uiSet => uiSet.UICollection
                            .Sum(p => p.Value.ULengthCount));
                    adaptULength = (uvFinalSize.U - maxElementCount * itemSetSize.U) / maxElementCount;
                    adaptULength = Max(adaptULength, 0);
                }
            }

            if (isFillV)
            {
                if (itemSetSize.V > 0)
                {
                    isAdaptV = true;
                    adaptVLength = uvFinalSize.V / lineUVCollection.Count;
                }
            }

            bool isHorizontal = Orientation == Orientation.Horizontal;
            foreach (var uvCollection in lineUVCollection)
            {
                double u = 0;
                var lineUIEles = uvCollection.UICollection.Keys.ToList();
                double linevV = isAdaptV ? adaptVLength : uvCollection.LineV;
                foreach (var child in lineUIEles)
                {
                    UVLengthSize childSize = uvCollection.UICollection[child];

                    double layoutSlotU = childSize.UVSize.U + childSize.ULengthCount * adaptULength;
                    double layoutSlotV = isAdaptV ? linevV : childSize.UVSize.V;
                    if (ElasticWrapPanel.GetIsFixToRB(child) == false)
                    {
                        child.Arrange(new Rect(
                            isHorizontal ? u : accumulatedV,
                            isHorizontal ? accumulatedV : u,
                            isHorizontal ? layoutSlotU : layoutSlotV,
                            isHorizontal ? layoutSlotV : layoutSlotU));
                    }
                    else
                    {
                        if (itemSetSize.U > 0)
                        {
                            layoutSlotU = childSize.ULengthCount * itemSetSize.U +
                                          childSize.ULengthCount * adaptULength;
                            double leaveULength = uvFinalSize.U - u;
                            layoutSlotU = Min(leaveULength, layoutSlotU);
                        }

                        child.Arrange(new Rect(
                            isHorizontal ? Max(0, uvFinalSize.U - layoutSlotU) : accumulatedV,
                            isHorizontal ? accumulatedV : Max(0, uvFinalSize.U - layoutSlotU),
                            isHorizontal ? layoutSlotU : layoutSlotV,
                            isHorizontal ? layoutSlotV : layoutSlotU));
                    }

                    u += layoutSlotU;
                }

                accumulatedV += linevV;
                lineUIEles.Clear();
            }
        }

        lineUVCollection.ForEach(col => col.Dispose());
        lineUVCollection.Clear();
        return finalSize;
    }

    #region Protected Methods

    private struct UVSize
    {
        internal UVSize(Orientation orientation, double width, double height)
        {
            U = V = 0d;
            _orientation = orientation;
            Width = width;
            Height = height;
        }

        internal UVSize(Orientation orientation)
        {
            U = V = 0d;
            _orientation = orientation;
        }

        internal double U;
        internal double V;
        private Orientation _orientation;

        internal double Width
        {
            get { return _orientation == Orientation.Horizontal ? U : V; }
            set
            {
                if (_orientation == Orientation.Horizontal) U = value;
                else V = value;
            }
        }

        internal double Height
        {
            get { return _orientation == Orientation.Horizontal ? V : U; }
            set
            {
                if (_orientation == Orientation.Horizontal) V = value;
                else U = value;
            }
        }
    }

    private class UVLengthSize
    {
        public UVSize UVSize { get; set; }

        public int ULengthCount { get; set; }

        public UVLengthSize(UVSize uvSize, int uLengthCount)
        {
            this.UVSize = uvSize;
            this.ULengthCount = uLengthCount;
        }
    }

    /// <summary>
    /// Elements used to store the same row/column
    /// </summary>
    private class UVCollection : IDisposable
    {
        public Dictionary<Control, UVLengthSize> UICollection { get; }

        private UVSize LineDesireUVSize;

        private UVSize ItemSetSize;

        public UVCollection(Orientation orientation, UVSize itemSetSize)
        {
            UICollection = new Dictionary<Control, UVLengthSize>();
            LineDesireUVSize = new UVSize(orientation);
            ItemSetSize = itemSetSize;
        }

        public double TotalU => LineDesireUVSize.U;

        public double LineV => LineDesireUVSize.V;

        public void Add(Control element, UVSize childSize, int itemULength = 1)
        {
            if (UICollection.ContainsKey(element))
                throw new InvalidOperationException("The element already exists and cannot be added repeatedly.");

            UICollection[element] = new UVLengthSize(childSize, itemULength);
            LineDesireUVSize.U += childSize.U;
            LineDesireUVSize.V = Max(LineDesireUVSize.V, childSize.V);
        }

        public int Count => UICollection.Count;

        public void Dispose()
        {
            UICollection.Clear();
        }
    }

    #endregion
}