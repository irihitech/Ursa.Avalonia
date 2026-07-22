using System;

namespace Ursa.Demo.ViewModels.Controls;

public class BreadcrumbItemData(IObservable<string?> header)
{
    public IObservable<string?> Header => header;
}
