﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls.Notifications;

namespace Ursa.Controls;

/// <summary>
/// A toast that can be shown in a window or by the host operating system.
/// </summary>
/// <remarks>
/// This class represents a toast that can be displayed either in a window using
/// <see cref="WindowToastManager"/> or by the host operating system (to be implemented).
/// </remarks>
public class Toast : IToast, INotifyPropertyChanged
{
    private string? _content;

    /// <summary>
    /// Initializes a new instance of the <see cref="Toast"/> class.
    /// </summary>
    /// <param name="content">The content to be displayed in the toast.</param>
    /// <param name="type">The <see cref="NotificationType"/> of the toast.</param>
    /// <param name="expiration">The expiry time at which the toast will close. 
    /// Use <see cref="TimeSpan.Zero"/> for toasts that will remain open.</param>
    /// <param name="onClick">An Action to call when the toast is clicked.</param>
    /// <param name="onClose">An Action to call when the toast is closed.</param>
    public Toast(
        string? content,
        NotificationType type = NotificationType.Information,
        TimeSpan? expiration = null,
        Action? onClick = null,
        Action? onClose = null)
    {
        Content = content;
        Type = type;
        Expiration = expiration ?? TimeSpan.FromSeconds(5);
        OnClick = onClick;
        OnClose = onClose;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Toast"/> class.
    /// </summary>
    public Toast() : this(null)
    {
    }

    /// <inheritdoc/>
    public string? Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChanged();
            }
        }
    }

    /// <inheritdoc/>
    public NotificationType Type { get; set; }

    /// <inheritdoc/>
    public TimeSpan Expiration { get; set; }

    /// <inheritdoc/>
    public Action? OnClick { get; set; }

    /// <inheritdoc/>
    public Action? OnClose { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}