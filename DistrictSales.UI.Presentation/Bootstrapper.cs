﻿using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using DistrictSales.UI.Presentation.ViewModels;

namespace DistrictSales.UI.Presentation;

public class Bootstrapper : BootstrapperBase
{
    private SimpleContainer container;

    public Bootstrapper()
    {
        Initialize();
    }

    protected override void Configure()
    {
        container = new SimpleContainer();

        container.Singleton<IWindowManager, WindowManager>();
        container.Singleton<IEventAggregator, EventAggregator>();

        container.PerRequest<MainViewModel>();
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
        DisplayRootViewFor<MainViewModel>();
    }

    protected override object GetInstance(Type service, string key)
    {
        return container.GetInstance(service, key);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
        return container.GetAllInstances(service);
    }

    protected override void BuildUp(object instance)
    {
        container.BuildUp(instance);
    }
}
