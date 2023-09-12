using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Utilities;

namespace Ursa.Controls;

public class ElasticWrapPanel : WrapPanel
{
    static ElasticWrapPanel()
    {
        IsFillHorizontalProperty.Changed.AddClassHandler<Control>(OnIsFillPropertyChanged);
        IsFillHorizontalProperty.Changed.AddClassHandler<Control>(OnIsFillPropertyChanged);

        AffectsMeasure<ElasticWrapPanel>(IsFillHorizontalProperty, IsFillHorizontalProperty);
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
        // 将元素按照水平/垂直排列的方式得出同一行/列所需的空间需求
        var curLineSize = new UVSize(orientation);
        // 计算处此 ElasticWrapPanel 所需的空间大小需求结果
        var panelSize = new UVSize(orientation);
        // 给定的空间大小测量 UVSize，用于没有 ItemWidth 和 ItemHeight 时候测量元素空间大小
        // FixToRB=True 的元素也使用这个
        var uvConstraint = new UVSize(orientation, constraint.Width, constraint.Height);
        bool itemWidthSet = !double.IsNaN(itemWidth);
        bool itemHeightSet = !double.IsNaN(itemHeight);

        var childConstraint = new Size(
            itemWidthSet ? itemWidth : constraint.Width,
            itemHeightSet ? itemHeight : constraint.Height);

        // FixToRB=True 元素使用测量需求空间 Size
        Size childFixConstraint = new Size(constraint.Width, constraint.Height);
        switch (orientation)
        {
            case Orientation.Horizontal when itemHeightSet:
                childFixConstraint = new Size(constraint.Width, itemHeight);
                break;
            case Orientation.Vertical when itemWidthSet:
                childFixConstraint = new Size(itemWidth, itemHeight);
                break;
        }

        //这个给非空间测量大小
        UVSize itemSetSize = new UVSize(orientation,
            itemWidthSet ? itemWidth : 0,
            itemHeightSet ? itemHeight : 0);

        foreach (var child in children)
        {
            UVSize sz;
            if (ElasticWrapPanel.GetIsFixToRB(child))
            {
                //此元素需要设置到固定靠右/底的型为操作，测量元素大小时需要放开
                child.Measure(childFixConstraint);
                sz = new UVSize(orientation, child.DesiredSize.Width, child.DesiredSize.Height);

                //主要是我对与这个固定住的元素的需求宽度高度按照那个标准有点头疼，干脆放开用最大控件算了
                if (sz.U > 0 && itemSetSize.U > 0)
                {
                    if (sz.U < itemSetSize.U)
                    {
                        //保持比例
                        sz.U = itemSetSize.U;
                    }
                    else
                    {
                        //设置了同方向中元素的长度，所以这里要按照比例
                        //double lengthCount = Math.Ceiling(sz.U / ItemSetSize.U);
                        //sz.U = lengthCount * ItemSetSize.U;
                        //这里防止意外
                        sz.U = Math.Min(sz.U, uvConstraint.U);
                    }
                }

                if (sz.V > 0 && itemSetSize.V > 0 && sz.V < itemSetSize.V)
                {
                    //设置了垂直方向元素长度，如果此元素空间需求小于，则按照ItemSetSize.V
                    sz.V = itemSetSize.V;
                }

                if (MathUtilities.GreaterThan(curLineSize.U + sz.U, uvConstraint.U))
                {
                    //当前同一 列/行 如果容纳 此元素空间将超出
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    //当前元素需要启1个新行
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                }
                else
                {
                    //这里是元素空间足够 填充式布局
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                }

                //下一个可能是换行元素....用于存放全新1行
                curLineSize = new UVSize(orientation);
            }
            else
            {
                // Flow passes its own constraint to children
                child.Measure(childConstraint);

                // This is the size of the child in UV space
                sz = new UVSize(Orientation,
                    itemWidthSet ? this.ItemWidth : child.DesiredSize.Width,
                    itemHeightSet ? this.ItemHeight : child.DesiredSize.Height);

                if (MathUtilities.GreaterThan(curLineSize.U + sz.U, uvConstraint.U)) // Need to switch to another line
                {
                    panelSize.U = Math.Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    if (MathUtilities.GreaterThan(sz.U, uvConstraint.U)) // The element is wider then the constraint - give it a separate line
                    {
                        panelSize.U = Math.Max(sz.U, panelSize.U);
                        panelSize.V += sz.V;
                        curLineSize = new UVSize(Orientation);
                    }
                }
                else // Continue to accumulate a line
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Math.Max(sz.V, curLineSize.V);
                }
            }
        }

        // The last line size, if any should be added
        panelSize.U = Math.Max(curLineSize.U, panelSize.U);
        panelSize.V += curLineSize.V;

        // Go from UV space to W/H space
        return new Size(panelSize.Width, panelSize.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        //是否元素宽度设置，是否元素高度设置
        bool itemWidthSet = !double.IsNaN(ItemWidth);
        bool itemHeightSet = !double.IsNaN(ItemHeight);

        //这个给非空间测量大小
        UVSize itemSetSize = new UVSize(Orientation,
            itemWidthSet ? ItemWidth : 0,
            itemHeightSet ? ItemHeight : 0);

        //给定的空间大小测量 UVSize，用于没有 ItemWidth 和 ItemHeight 时候测量元素空间大小
        //FixToRB=True 的元素也使用这个
        UVSize uvFinalSize = new UVSize(Orientation, finalSize.Width, finalSize.Height);

        //用于存放同一方向的元素列/行 集合
        List<UVCollection> lineUVCollection = new List<UVCollection>();

        #region 得到同一方向元素集合的集合

        //当前元素集合行/列
        UVCollection curLineUIs = new UVCollection(Orientation, itemSetSize);

        //遍历内部元素
        var children = Children;
        foreach (var child in children)
        {
            UVSize sz;
            if (ElasticWrapPanel.GetIsFixToRB(child))
            {
                //此元素需要设置到固定靠右/底的型为操作，测量元素大小时需要放开
                sz = new UVSize(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                double lengthCount = 1;
                if (sz.U > 0 && itemSetSize.U > 0)
                {
                    if (sz.U < itemSetSize.U)
                    {
                        //保持比例
                        sz.U = itemSetSize.U;
                    }
                    else
                    {
                        //设置了同方向中元素的长度，所以这里要按照比例
                        lengthCount = Math.Ceiling(sz.U / itemSetSize.U);
                        //sz.U = lengthCount * ItemSetSize.U;
                        sz.U = Math.Min(sz.U, uvFinalSize.U);
                    }
                }

                if (sz.V > 0 && itemSetSize.V > 0 && sz.V < itemSetSize.V)
                {
                    //设置了垂直方向元素长度，如果此元素空间需求小于，则按照ItemSetSize.V
                    sz.V = itemSetSize.V;
                }

                if (MathUtilities.GreaterThan(curLineUIs.TotalU + sz.U, uvFinalSize.U))
                {
                    //当前同一 列/行 如果容纳 此元素空间将超出
                    if (curLineUIs.Count > 0)
                    {
                        lineUVCollection.Add(curLineUIs);
                    }

                    curLineUIs = new UVCollection(Orientation, itemSetSize);
                    curLineUIs.Add(child, sz, Convert.ToInt32(lengthCount));
                }
                else
                {
                    //这里是元素空间足够
                    curLineUIs.Add(child, sz, Convert.ToInt32(lengthCount));
                }

                lineUVCollection.Add(curLineUIs);
                //下一个可能是换行元素....不管了以后闲得蛋疼再弄吧
                curLineUIs = new UVCollection(Orientation, itemSetSize);
            }
            else
            {
                sz = new UVSize(Orientation,
                    itemWidthSet ? ItemWidth : child.DesiredSize.Width,
                    itemHeightSet ? ItemHeight : child.DesiredSize.Height);

                if (MathUtilities.GreaterThan(curLineUIs.TotalU + sz.U, uvFinalSize.U)) // Need to switch to another line
                {
                    //当前同一 列/行 如果容纳 此元素空间将超出
                    if (curLineUIs.Count > 0)
                    {
                        lineUVCollection.Add(curLineUIs);
                    }

                    curLineUIs = new UVCollection(Orientation, itemSetSize);
                    curLineUIs.Add(child, sz);
                    if (MathUtilities.GreaterThan(sz.U, uvFinalSize.U))
                    {
                        lineUVCollection.Add(curLineUIs);
                        curLineUIs = new UVCollection(Orientation, itemSetSize);
                    }
                }
                else
                {
                    //空间足够
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
                    adaptULength = Math.Max(adaptULength, 0);
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
                            //说明同方向有宽度设置，这里尽量按照 ItemULength 保持
                            layoutSlotU = childSize.ULengthCount * itemSetSize.U +
                                          childSize.ULengthCount * adaptULength;
                            double leaveULength = uvFinalSize.U - u;
                            layoutSlotU = Math.Min(leaveULength, layoutSlotU);
                        }

                        child.Arrange(new Rect(
                            isHorizontal ? Math.Max(0, uvFinalSize.U - layoutSlotU) : accumulatedV,
                            isHorizontal ? accumulatedV : Math.Max(0, uvFinalSize.U - layoutSlotU),
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
            LineDesireUVSize.V = Math.Max(LineDesireUVSize.V, childSize.V);
        }

        public int Count => UICollection.Count;

        public void Dispose()
        {
            UICollection.Clear();
        }
    }

    #endregion
}