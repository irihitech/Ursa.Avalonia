using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Ursa.Demo.Dialogs;

public class PlainDialogViewModel: ObservableObject
{
    private DateTime? _date;

    public DateTime? Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private string? _text;
    public string? Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }

    public PlainDialogViewModel()
    {
        Text =
            "千古悠悠 有多少冤魂嗟叹 空怅望人寰无限 丛生哀怨 泣血蝇虫笑苍天 孤帆叠影锁白链 残月升骤起烈烈风 尽吹散 尽吹散 尽吹散 滂沱雨无底涧 涉激流登彼岸 奋力拨云间消得雾患 社稷安抚臣子心 长驱鬼魅不休战 看斜阳照大地阡陌 从头转";
    }
}