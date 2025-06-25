﻿using System;
using System.Collections.Generic;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notification = Ursa.Controls.Notification;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;

namespace Ursa.Demo.ViewModels;

public partial class NotificationDemoViewModel : ObservableObject
{
    public WindowNotificationManager? NotificationManager { get; set; }

    [ObservableProperty] private bool _showIcon = true;
    [ObservableProperty] private bool _showClose = true;

    private readonly List<Notification> _currentNotifications = new();

    [RelayCommand]
    public void ChangePosition(object obj)
    {
        if (obj is string s && NotificationManager is not null)
        {
            Enum.TryParse<NotificationPosition>(s, out var notificationPosition);
            NotificationManager.Position = notificationPosition;
        }
    }

    [RelayCommand]
    public void ShowNormal(object obj)
    {
        if (obj is not string s) return;
        Enum.TryParse<NotificationType>(s, out var notificationType);
        var notification = new Notification("Welcome", "This is message");
        _currentNotifications.Add(notification);
        NotificationManager?.Show(
            notification,
            showIcon: ShowIcon,
            showClose: ShowClose,
            type: notificationType);
    }

    [RelayCommand]
    public void ShowLight(object obj)
    {
        if (obj is not string s) return;
        Enum.TryParse<NotificationType>(s, out var notificationType);
        var notification = new Notification("Welcome", "This is message");
        _currentNotifications.Add(notification);
        NotificationManager?.Show(
            notification,
            showIcon: ShowIcon,
            showClose: ShowClose,
            type: notificationType,
            classes: ["Light"]);
    }

    [RelayCommand]
    public void CloseFirstNotification()
    {
        if (_currentNotifications.Count > 0)
        {
            var notification = _currentNotifications[0];
            NotificationManager?.Close(notification);
            _currentNotifications.RemoveAt(0);
        }
    }

    [RelayCommand]
    public void CloseAllNotifications()
    {
        NotificationManager?.CloseAll();
        _currentNotifications.Clear();
    }
}