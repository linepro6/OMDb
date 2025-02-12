﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

namespace OMDb.WinUI3.Converters
{
    public sealed class EntryCoverImgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var entry = parameter as Core.Models.Entry;
            if (value != null)
            {
                var storage = Services.ConfigService.EnrtyStorages.FirstOrDefault(p => p.StorageName == entry.DbId);
                if(storage != null)
                {
                    return string.IsNullOrEmpty(storage.StoragePath)?null: System.IO.Path.Combine(System.IO.Path.GetDirectoryName(storage.StoragePath), entry.Path, entry.CoverImg);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        public static string Convert(Core.Models.Entry entry)
        {
            var storage = Services.ConfigService.EnrtyStorages.FirstOrDefault(p => p.StorageName == entry.DbId);
            if (storage != null)
            {
                return string.IsNullOrEmpty(storage.StoragePath) ? null : System.IO.Path.Combine(System.IO.Path.GetDirectoryName(storage.StoragePath), entry.Path, entry.CoverImg);
            }
            else
            {
                return null;
            }
        }
    }
}
