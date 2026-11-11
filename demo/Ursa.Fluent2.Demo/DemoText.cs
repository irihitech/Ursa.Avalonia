namespace Ursa.Fluent2.Demo;

public sealed class DemoText(bool chinese)
{
    public string Search => chinese ? "搜索控件…" : "Search controls...";
    public string Settings => chinese ? "外观设置" : "Appearance";
    public string Theme => chinese ? "主题" : "Theme";
    public string Density => chinese ? "密度" : "Density";
    public string Language => chinese ? "语言" : "Language";
    public string Controls => chinese ? "控件" : "Controls";
    public string NoResults => chinese ? "未找到匹配的控件" : "No matching controls";
    public string NoResultsHint => chinese ? "请尝试其他名称，或清除搜索内容。" : "Try another name or clear the search.";
    public string ClearSearch => chinese ? "清除搜索" : "Clear search";
    public string CollapseNavigation => chinese ? "折叠或展开导航" : "Collapse or expand navigation";
    public string PageTitle(string id) => id == "Overview" ? chinese ? "控件总览" : "Overview" : id;
    public string[] ThemeOptions => chinese ? ["跟随系统", "浅色", "深色"] : ["System", "Light", "Dark"];
    public string[] DensityOptions => chinese ? ["普通", "紧凑"] : ["Normal", "Compact"];
    public string Category(string id) => id switch
    {
        "presentation" => chinese ? "布局与展示" : "Layout and display",
        "inputs" => chinese ? "输入与选择" : "Inputs and selection",
        "date-time" => chinese ? "日期与时间" : "Date and time",
        "navigation" => chinese ? "导航与窗口" : "Navigation and windows",
        "feedback" => chinese ? "浮层与反馈" : "Overlays and feedback",
        "specialized" => chinese ? "内容与加载" : "Content and loading",
        _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
    };
}
