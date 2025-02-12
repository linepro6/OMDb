﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using OMDb.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.WinUI3.Models
{
    public class EntryDetail : ObservableObject
    {
        public EntryDetail() { }
        public EntryDetail(Core.Models.Entry entry)
        {
            Entry = entry.DepthClone<Core.Models.Entry>();
            FullEntryPath = Helpers.PathHelper.EntryFullPath(entry);
            FullCoverImgPath = Helpers.PathHelper.EntryCoverImgFullPath(entry);
            FullMetaDataPath = System.IO.Path.Combine(FullEntryPath, Services.ConfigService.MetadataFileNmae);
        }
        public static EntryDetail Create(Core.Models.Entry entry)
        {
            var entryDetail = entry.DepthClone<EntryDetail>();
            if(entryDetail != null)
            {
                entryDetail.FullEntryPath = Helpers.PathHelper.EntryFullPath(entry);
                entryDetail.FullCoverImgPath = Helpers.PathHelper.EntryCoverImgFullPath(entry);
                entryDetail.FullMetaDataPath = System.IO.Path.Combine(entryDetail.FullEntryPath, Services.ConfigService.MetadataFileNmae);
            }
            return entryDetail;
        }
        public static async Task<EntryDetail> CreateAsync(Core.Models.Entry entry)
        {
            var entryDetail = new EntryDetail(entry);
            await entryDetail.Init();
            return entryDetail;
        }
        public Core.Models.Entry Entry { get; set; }
        public ObservableCollection<EntryName> Names { get; set; } = new ObservableCollection<EntryName>();
        private string fullEntryPath;
        public string FullEntryPath
        {
            get => fullEntryPath;
            set=>SetProperty(ref fullEntryPath, value);
        }
        private string fullCoverImgPath;
        public string FullCoverImgPath
        {
            get => fullCoverImgPath;
            set => SetProperty(ref fullCoverImgPath, value);
        }
        private string fullMetaDataPath;
        public string FullMetaDataPath
        {
            get => fullMetaDataPath;
            set => SetProperty(ref fullMetaDataPath, value);
        }
        private Core.Models.EntryMetadata metadata;
        public Core.Models.EntryMetadata Metadata
        {
            get => metadata;
            set=>SetProperty(ref metadata,value);
        }
        public ObservableCollection<Core.Models.WatchHistory> WatchHistory { get; set; } = new ObservableCollection<Core.Models.WatchHistory>();
        public List<Core.DbModels.LabelDb> Labels { get; set; } = new List<Core.DbModels.LabelDb>();
        public ObservableCollection<string> Imgs { get; set; }
        private async Task Init()
        {
            var namesT = await Core.Services.EntryNameSerivce.QueryNamesAsync(Entry.Id, Entry.DbId);
            if (namesT != null)
            {
                Names = namesT.Select(p => new EntryName(p)).ToObservableCollection();
            }
            WatchHistory = (await Core.Services.WatchHistoryService.QueryWatchHistoriesAsync(Entry.Id, Entry.DbId)).ToObservableCollection();
            Labels = await Core.Services.LabelService.GetLabelOfEntryAsync(Entry.DbId, Entry.Id);
            if (WatchHistory == null)
            {
                WatchHistory = new ObservableCollection<Core.Models.WatchHistory>();
            }
            if (Labels == null)
            {
                Labels = new List<Core.DbModels.LabelDb>();
            }
            LoadImgs();
            LoadMetaData();
        }
        private void LoadImgs()
        {
            Imgs = new ObservableCollection<string>();
            string imgFolder = Path.Combine(FullEntryPath,Services.ConfigService.ImgFolder);
            if(Directory.Exists(imgFolder))
            {
                var files = new DirectoryInfo(imgFolder).GetFiles();
                if(files.Any())
                {
                    foreach(var file in files)
                    {
                        if(Helpers.ImgHelper.IsSupportImg(file.FullName))
                        {
                            Imgs.Add(file.FullName);
                        }
                    }
                }
            }
        }
        public void LoadMetaData()
        {
            Metadata = Core.Models.EntryMetadata.Read(Path.Combine(FullEntryPath, Services.ConfigService.MetadataFileNmae));
        }
        private void LoadVideos()
        {
            string folder = Path.Combine(FullEntryPath, Services.ConfigService.VideoFolder);
            if (Directory.Exists(folder))
            {
                var files = new DirectoryInfo(folder).GetFiles();
                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        
                    }
                }
            }
        }
        /// <summary>
        /// 修改词条存储路径
        /// </summary>
        /// <param name="newPath"></param>
        public void ChangeEntryPath(string newPath)
        {

        }
        /// <summary>
        /// 修改词条封面
        /// </summary>
        /// <param name="newImgName">新封面图带后缀名名字，必须为词条Img文件夹下的图片文件</param>
        public void ChangeCoverImg(string newImgName)
        {

        }
    }
}
