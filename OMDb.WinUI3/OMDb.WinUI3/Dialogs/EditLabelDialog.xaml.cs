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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace OMDb.WinUI3.Dialogs
{
    public sealed partial class EditLabelDialog : Page
    {
        private Core.DbModels.LabelDb Label;
        public EditLabelDialog(Core.DbModels.LabelDb label)
        {
            this.InitializeComponent();
            if(label == null)
            {
                Label = new Core.DbModels.LabelDb();
            }
            else
            {
                Label = label;
                TextBox_Name.Text = label.Name;
                TextBox_Desc.Text = label.Description;
            }
        }
        public static async Task<Core.DbModels.LabelDb> ShowDialog(Core.DbModels.LabelDb label = null)
        {
            MyContentDialog dialog = new MyContentDialog();
            dialog.TitleTextBlock.Text = label == null ? "新建标签" : "编辑标签";
            dialog.PrimaryButton.Content = "保存";
            dialog.CancelButton.Content = "取消";
            EditLabelDialog content = new EditLabelDialog(label);
            dialog.ContentFrame.Content = content;
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                content.Label.Name = content.TextBox_Name.Text;
                content.Label.Description = content.TextBox_Desc.Text;
                return content.Label;
            }
            else
            {
                return null;
            }
        }
    }
}
