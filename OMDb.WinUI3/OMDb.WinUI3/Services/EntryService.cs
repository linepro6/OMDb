﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.WinUI3.Services
{
    public static class EntryService
    {
        ///// <summary>
        ///// 新建词条
        ///// </summary>
        ///// <returns>词条唯一id</returns>
        //public static async Task<string> AddEntryAsync()
        //{
        //    var result = await Dialogs.EditEntryDialog.ShowDialog();
        //    if (result != null && result.Item1 != null && result.Item2 != null)
        //    {
        //        var entry = result.Item1;
        //        if (Directory.Exists(entry.Path))
        //        {
        //            int i = 1;
        //            while (true)
        //            {
        //                entry.Path = $"{entry.Path}({i})";
        //                if (!Directory.Exists(entry.Path))
        //                {
        //                    break;
        //                }
        //            }
        //        }
        //        //此时的entry封面图、存储路径都是完整路径
        //        //创建文件夹、元文件、复制封面、保存数据库
        //        InitFolder(entry);
        //        InitFile(entry);
        //        await SaveToDbAsync(entry, result.Item2);
        //        //这时已经是相对路径
        //        Helpers.InfoHelper.ShowSuccess("创建成功");
        //        return entry.Id;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// 复制封面图、元数据
        ///// 会将entry.CoverImg修改为最终路径
        ///// </summary>
        ///// <param name="entry"></param>
        //private static void InitFile(Core.Models.Entry entry)
        //{
        //    string newImgCoverPath = Path.Combine(entry.Path, Services.ConfigService.ImgFolder, Path.GetFileName(entry.CoverImg));
        //    File.Copy(entry.CoverImg, newImgCoverPath, true);
        //    entry.CoverImg = newImgCoverPath;
        //    Core.Models.EntryMetadata metadata = new Core.Models.EntryMetadata()
        //    {
        //        Id = entry.Id,
        //        Name = entry.Name,
        //    };
        //    metadata.Save(Path.Combine(entry.Path, Services.ConfigService.MetadataFileNmae));
        //}
        //private static void InitFolder(Core.Models.Entry entry)
        //{
        //    Directory.CreateDirectory(entry.Path);
        //    Directory.CreateDirectory(Path.Combine(entry.Path, Services.ConfigService.SubFolder));
        //    Directory.CreateDirectory(Path.Combine(entry.Path, Services.ConfigService.ImgFolder));
        //    Directory.CreateDirectory(Path.Combine(entry.Path, Services.ConfigService.VideoFolder));
        //    Directory.CreateDirectory(Path.Combine(entry.Path, Services.ConfigService.ResourceFolder));
        //}
        //private static async Task SaveToDbAsync(Core.Models.Entry entry, List<Models.EntryName> entryNames)
        //{
        //    //恢复相对路径
        //    entry.CoverImg = Helpers.PathHelper.EntryCoverImgRelativePath(entry);
        //    entry.Path = Helpers.PathHelper.EntryRelativePath(entry);
        //    //保存至数据库
        //    Core.Services.EntryService.AddEntry(entry);
        //    List<Core.DbModels.EntryNameDb> entryNameDbs = new List<Core.DbModels.EntryNameDb>();
        //    foreach (var p in entryNames.Where(p => p.Name != null))
        //    {
        //        entryNameDbs.Add(new Core.DbModels.EntryNameDb()
        //        {
        //            Id = entry.Id,
        //            Name = p.Name,
        //            Lang = p.Lang.ToString(),
        //            IsDefault = p.IsDefault,
        //        });
        //    }
        //    await Core.Services.EntryNameSerivce.AddNamesAsync(entryNameDbs, entry.DbId);
        //    //Core.Services.LabelService.AddLabel
        //}


        ///// <summary>
        ///// 编辑词条
        ///// </summary>
        ///// <returns>词条唯一id</returns>
        //public static async Task EditEntryAsync(Core.Models.Entry entry)
        //{
        //    var result = await Dialogs.EditEntryDialog.ShowDialog(entry);
        //    if (result != null && result.Item1 != null && result.Item2 != null)
        //    {
        //        //await SaveToDbAsync(entry, result.Item2);
        //        //这时已经是相对路径
        //        Helpers.InfoHelper.ShowSuccess("已保存");
        //    }
        //}

        /// <summary>
        /// 新建词条
        /// </summary>
        /// <returns>词条唯一id</returns>
        public static async Task<string> AddEntryAsync()
        {
            var entryDetail = await Dialogs.EditEntryDialog.ShowDialog();
            if (entryDetail != null)
            {
                if (Directory.Exists(entryDetail.FullEntryPath))
                {
                    int i = 1;
                    while (true)
                    {
                        string newPath = $"{entryDetail.FullEntryPath}({i++})";
                        if (!Directory.Exists(newPath))
                        {
                            entryDetail.FullEntryPath = newPath;
                            break;
                        }
                    }
                }
                //此时的entry封面图、存储路径都是完整路径
                //创建文件夹、元文件、复制封面、保存数据库
                InitFolder(entryDetail);
                InitFile(entryDetail);
                await SaveToDbAsync(entryDetail);
                //这时已经是相对路径
                Helpers.InfoHelper.ShowSuccess("创建成功");
                return entryDetail.Entry.Id;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 复制封面图、元数据
        /// 会将entry.CoverImg修改为最终路径
        /// </summary>
        /// <param name="entry"></param>
        private static void InitFile(Models.EntryDetail entry)
        {
            string newImgCoverPath = Path.Combine(entry.FullEntryPath, Services.ConfigService.ImgFolder, Path.GetFileName(entry.FullCoverImgPath));
            if(newImgCoverPath != entry.FullCoverImgPath)
            {
                File.Copy(entry.FullCoverImgPath, newImgCoverPath, true);
            }
            string metaDateFile = Path.Combine(entry.FullEntryPath, Services.ConfigService.MetadataFileNmae);
            Core.Models.EntryMetadata metadata;
            if (System.IO.File.Exists(metaDateFile))
            {
                metadata = Core.Models.EntryMetadata.Read(metaDateFile);
                metadata.Name = entry.Entry.Name;
            }
            else
            {
                metadata = new Core.Models.EntryMetadata()
                {
                    Id = entry.Entry.Id,
                    Name = entry.Entry.Name,
                };
            }
            metadata.Save(metaDateFile);
        }
        private static void InitFolder(Models.EntryDetail entry)
        {
            Directory.CreateDirectory(entry.FullEntryPath);
            Directory.CreateDirectory(Path.Combine(entry.FullEntryPath, Services.ConfigService.SubFolder));
            Directory.CreateDirectory(Path.Combine(entry.FullEntryPath, Services.ConfigService.ImgFolder));
            Directory.CreateDirectory(Path.Combine(entry.FullEntryPath, Services.ConfigService.VideoFolder));
            Directory.CreateDirectory(Path.Combine(entry.FullEntryPath, Services.ConfigService.ResourceFolder));
        }
        /// <summary>
        /// 保存至数据库
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="entryNames"></param>
        /// <returns></returns>
        private static async Task SaveToDbAsync(Models.EntryDetail entry)
        {
            Core.Services.EntryService.AddEntry(entry.Entry);//词条
            List<Core.DbModels.EntryNameDb> entryNameDbs = new List<Core.DbModels.EntryNameDb>();
            foreach (var p in entry.Names.Where(p => p.Name != null))
            {
                entryNameDbs.Add(new Core.DbModels.EntryNameDb()
                {
                    Id = entry.Entry.Id,
                    Name = p.Name,
                    Lang = p.Lang.ToString(),
                    IsDefault = p.IsDefault,
                });
            }
            await Core.Services.EntryNameSerivce.AddNamesAsync(entryNameDbs, entry.Entry.DbId);//添加新词条名称
            if(entry.Labels?.Count!=0)
            {
                List<Core.DbModels.EntryLabelDb> entryLabelDbs = new List<Core.DbModels.EntryLabelDb>(entry.Labels.Count);
                entry.Labels.ForEach(p => entryLabelDbs.Add(new Core.DbModels.EntryLabelDb() { EntryId = entry.Entry.Id, LabelId = p.Id }));
                await Core.Services.LabelService.AddEntryLabelAsync(entryLabelDbs);//添加词条标签
            }
        }
        /// <summary>
        /// 保存至数据库
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="entryNames"></param>
        /// <returns></returns>
        private static async Task UpdateDbAsync(Models.EntryDetail entry)
        {
            Core.Services.EntryService.UpdateEntry(entry.Entry);//词条
            List<Core.DbModels.EntryNameDb> entryNameDbs = new List<Core.DbModels.EntryNameDb>();
            foreach (var p in entry.Names.Where(p => p.Name != null))
            {
                entryNameDbs.Add(new Core.DbModels.EntryNameDb()
                {
                    Id = entry.Entry.Id,
                    Name = p.Name,
                    Lang = p.Lang.ToString(),
                    IsDefault = p.IsDefault,
                });
            }
            await Core.Services.EntryNameSerivce.RemoveNamesAsync(entry.Entry.Id, entry.Entry.DbId);//删除旧词条名称
            await Core.Services.EntryNameSerivce.AddNamesAsync(entryNameDbs, entry.Entry.DbId);//添加新词条名称
            if (entry.Labels?.Count != 0)
            {
                List<Core.DbModels.EntryLabelDb> entryLabelDbs = new List<Core.DbModels.EntryLabelDb>(entry.Labels.Count);
                entry.Labels.ForEach(p => entryLabelDbs.Add(new Core.DbModels.EntryLabelDb() { EntryId = entry.Entry.Id, LabelId = p.Id }));
                await Core.Services.LabelService.ClearEntryLabelAsync(entry.Entry.Id, entry.Entry.DbId);//清空词条标签
                await Core.Services.LabelService.AddEntryLabelAsync(entryLabelDbs);//添加词条标签
            }
        }

        /// <summary>
        /// 编辑词条
        /// </summary>
        /// <returns>词条唯一id</returns>
        public static async Task EditEntryAsync(Core.Models.Entry entry)
        {
            var entryDetail = await Dialogs.EditEntryDialog.ShowDialog(entry);
            if (entryDetail != null)
            {
                InitFile(entryDetail);
                await UpdateDbAsync(entryDetail);
                Helpers.InfoHelper.ShowSuccess("已保存");
            }
        }

        public static async Task RemoveEntryAsync(Core.Models.Entry entry)
        {
            if(await Dialogs.QueryDialog.ShowDialog("删除",$"是否删除 {entry.Name} 词条?\\n\\r不会删除本地文件"))
            {
                Core.Services.EntryService.RemoveEntry(entry);
                Helpers.InfoHelper.ShowSuccess("已删除");
            }
        }
    }
}
