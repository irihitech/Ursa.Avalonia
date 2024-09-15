using Avalonia;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using Ursa.Controls;

namespace Ursa.ReactiveUI;

/// <summary>
/// This class has both <see cref="UrsaView"/> and <see cref="ReactiveUserControl{TViewModel}"/> functionality and is compatible with each other.
/// </summary>
/// <remarks>The code in this class is a direct port of the <see cref="ReactiveUserControl{TViewModel}"/> code, as detailed in this article: <seealso href="https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.ReactiveUI/ReactiveUserControl.cs"/></remarks>
/// <typeparam name="TViewModel">ViewModel type.</typeparam>
public class ReactiveUrsaView<TViewModel> : UrsaView, IViewFor<TViewModel> where TViewModel : class
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
	public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty
			.Register<ReactiveUrsaView<TViewModel>, TViewModel?>(nameof(ViewModel));

	/// <summary>
	/// Initializes a new instance of the <see cref="ReactiveUrsaWindow{TViewModel}"/> class.
	/// </summary>
	public ReactiveUrsaView()
	{
		this.WhenActivated(disposables => { });
	}

	/// <summary>
	/// The ViewModel.
	/// </summary>
	public TViewModel? ViewModel
	{
		get => GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	object? IViewFor.ViewModel
	{
		get => ViewModel;
		set => ViewModel = (TViewModel?)value;
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);

		if (change.Property == DataContextProperty) {
			if (ReferenceEquals(change.OldValue, ViewModel)
				&& change.NewValue is null or TViewModel) {
				SetCurrentValue(ViewModelProperty, change.NewValue);
			}
		}
		else if (change.Property == ViewModelProperty) {
			if (ReferenceEquals(change.OldValue, DataContext)) {
				SetCurrentValue(DataContextProperty, change.NewValue);
			}
		}
	}
}