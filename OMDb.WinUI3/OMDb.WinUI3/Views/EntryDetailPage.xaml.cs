﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace OMDb.WinUI3.Views
{
    public sealed partial class EntryDetailPage : Page
    {
        public ViewModels.EntryDetailViewModel VM { get; set; } = new ViewModels.EntryDetailViewModel(null);
        public EntryDetailPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var entry = e.Parameter as Core.Models.Entry;
            if(entry != null)
            {
                VM.Entry = await Models.EntryDetail.CreateAsync(entry);
            }
        }
    }
}
