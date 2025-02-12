﻿using OMDb.Core.DbModels;
using OMDb.Core.Extensions;
using OMDb.Core.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.Core.Services
{
    public static class EntryService
    {
        /// <summary>
        /// 按指定排序方式获取词条
        /// 获取完自行处理分页
        /// </summary>
        /// <param name="sortType"></param>
        /// <param name="sortWay"></param>
        /// <returns></returns>
        public static List<QueryResult> QueryEntry(Enums.SortType sortType, Enums.SortWay sortWay, List<string> labelIds = null)
        {
            if (DbService.DbConfigIds?.Count == 0)
            {
                return null;
            }
            else
            {
                return sortType switch
                {
                    Enums.SortType.CreateTime => SortByCreateTime(sortWay, labelIds),
                    Enums.SortType.LastWatchTime => SortByLastWatchTime(sortWay, labelIds),
                    Enums.SortType.LastUpdateTime => SortByLastUpdateTime(sortWay, labelIds),
                    //Enums.SortType.WatchTimes => SortByWatchTimes(sortWay, labelIds),
                    Enums.SortType.MyRating => SortByMyRating(sortWay, labelIds),
                    _ => null,
                };
            }
        }
        public static async Task<List<QueryResult>> QueryEntryAsync(Enums.SortType sortType, Enums.SortWay sortWay, List<string> labelIds = null)
        {
            return await Task.Run(()=> QueryEntry(sortType, sortWay, labelIds));
        }
        private static List<QueryResult> SortByCreateTime(Enums.SortWay sortWay, List<string> labelIds = null)
        {
            List<QueryResult> queryResults = new List<QueryResult>();
            foreach (var dbid in DbService.DbConfigIds)
            {
                var db = DbService.Db.GetConnection(dbid);
                if (db != null)
                {
                    if (labelIds != null && labelIds.Count != 0)
                    {
                        var inLabelEntryIds = LabelService.GetEntrys(labelIds);
                        var ls = db.Queryable<DbModels.EntryDb>()
                            .In(inLabelEntryIds)
                            .Select(p => new { p.Id, p.CreateTime })
                            .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.CreateTime, dbid));
                        });
                    }
                    else
                    {
                        var ls = db.Queryable<DbModels.EntryDb>()
                                .Select(p => new { p.Id, p.CreateTime })
                                .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.CreateTime, dbid));
                        });
                    }
                }
            }
            if (sortWay == Enums.SortWay.Positive)
            {
                return queryResults.OrderBy(p => p.Value).ToList();
            }
            else
            {
                return queryResults.OrderByDescending(p => p.Value).ToList();
            }
        }
        private static List<QueryResult> SortByLastWatchTime(Enums.SortWay sortWay, List<string> labelIds = null)
        {
            List<QueryResult> queryResults = new List<QueryResult>();
            foreach (var dbid in DbService.DbConfigIds)
            {
                var db = DbService.Db.GetConnection(dbid);
                if (db != null)
                {
                    if (labelIds != null && labelIds.Count != 0)
                    {
                        var inLabelEntryIds = LabelService.GetEntrys(labelIds);
                        var ls = db.Queryable<DbModels.EntryDb>()
                            .In(inLabelEntryIds)
                            .Select(p => new { p.Id, p.LastWatchTime })
                            .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.LastWatchTime, dbid));
                        });
                    }
                    else
                    {
                        var ls = db.Queryable<DbModels.EntryDb>()
                        .Select(p => new { p.Id, p.LastWatchTime })
                        .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.LastWatchTime, dbid));
                        });
                    }
                }
            }
            if (sortWay == Enums.SortWay.Positive)
            {
                return queryResults.OrderBy(p => p.Value).ToList();
            }
            else
            {
                return queryResults.OrderByDescending(p => p.Value).ToList();
            }
        }
        private static List<QueryResult> SortByLastUpdateTime(Enums.SortWay sortWay, List<string> labelIds = null)
        {
            List<QueryResult> queryResults = new List<QueryResult>();
            foreach (var dbid in DbService.DbConfigIds)
            {
                var db = DbService.Db.AsTenant().GetConnection(dbid);
                if (db != null)
                {
                    if (labelIds != null && labelIds.Count != 0)
                    {
                        var inLabelEntryIds = LabelService.GetEntrys(labelIds);
                        var ls = db.Queryable<DbModels.EntryDb>()
                            .In(inLabelEntryIds)
                            .Select(p => new { p.Id, p.LastUpdateTime }).ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.LastUpdateTime, dbid));
                        });
                    }
                    else
                    {
                        var ls = db.Queryable<DbModels.EntryDb>()
                            .Select(p => new { p.Id, p.LastUpdateTime })
                            .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.LastUpdateTime, dbid));
                        });
                    }
                }
            }
            if (sortWay == Enums.SortWay.Positive)
            {
                return queryResults.OrderBy(p => p.Value).ToList();
            }
            else
            {
                return queryResults.OrderByDescending(p => p.Value).ToList();
            }
        }
        //private static List<QueryResult> SortByWatchTimes(Enums.SortWay sortWay, List<int> labelIds = null)
        //{
        //    List<QueryResult> queryResults = new List<QueryResult>();
        //    foreach (var dbid in DbService.DbConfigIds)
        //    {
        //        var db = DbService.Db.GetConnection(dbid);
        //        if (db != null)
        //        {
        //            if (labelIds != null)
        //            {
        //                var inLabelEntryIds = LabelService.GetEntrys(labelIds);
        //                var ls = db.Queryable<DbModels.EntryDb>()
        //                    .In(inLabelEntryIds)
        //                    .Select(p => new { p.Id, p.WatchTimes })
        //                    .ToList();
        //                ls.ForEach(p =>
        //                {
        //                    queryResults.Add(new QueryResult(p.Id, p.WatchTimes, dbid));
        //                });
        //            }
        //            else
        //            {
        //                var ls = db.Queryable<DbModels.EntryDb>()
        //                    .Select(p => new { p.Id, p.WatchTimes })
        //                    .ToList();
        //                ls.ForEach(p =>
        //                {
        //                    queryResults.Add(new QueryResult(p.Id, p.WatchTimes, dbid));
        //                });
        //            }
        //        }
        //    }
        //    if (sortWay == Enums.SortWay.Positive)
        //    {
        //        return queryResults.OrderBy(p => p.Value).ToList();
        //    }
        //    else
        //    {
        //        return queryResults.OrderByDescending(p => p.Value).ToList();
        //    }
        //}
        private static List<QueryResult> SortByMyRating(Enums.SortWay sortWay, List<string> labelIds = null)
        {
            List<QueryResult> queryResults = new List<QueryResult>();
            foreach (var dbid in DbService.DbConfigIds)
            {
                var db = DbService.Db.GetConnection(dbid);
                if (db != null)
                {
                    if (labelIds != null && labelIds.Count != 0)
                    {
                        var inLabelEntryIds = LabelService.GetEntrys(labelIds);
                        var ls = db.Queryable<DbModels.EntryDb>()
                            .In(inLabelEntryIds)
                            .Select(p => new { p.Id, p.MyRating })
                            .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.MyRating, dbid));
                        });
                    }
                    else
                    {
                        var ls = db.Queryable<DbModels.EntryDb>()
                            .Select(p => new { p.Id, p.MyRating })
                            .ToList();
                        ls.ForEach(p =>
                        {
                            queryResults.Add(new QueryResult(p.Id, p.MyRating, dbid));
                        });
                    }
                }
            }
            if (sortWay == Enums.SortWay.Positive)
            {
                return queryResults.OrderBy(p => p.Value).ToList();
            }
            else
            {
                return queryResults.OrderByDescending(p => p.Value).ToList();
            }
        }

        /// <summary>
        /// 查询词条
        /// </summary>
        /// <param name="queryItems"></param>
        /// <param name="withName">结果是否带默认名字</param>
        /// <returns></returns>
        public static async Task<List<Entry>> QueryEntryAsync(List<QueryItem> queryItems, bool withName = true)
        {
            if (queryItems != null)
            {
                return await Task.Run(() =>
                {
                    Dictionary<string,Entry> dic = new Dictionary<string, Entry>();
                    var group = queryItems.GroupBy(p => p.DbId);
                    foreach (var item in group)
                    {
                        var entryDbs = DbService.Db.GetConnection(item.Key).Queryable<DbModels.EntryDb>().Where(p2 => item.Select(p => p.Id).Contains(p2.Id)).ToList();
                        if (entryDbs.Any())
                        {
                            var entriesTemp = entryDbs.Select(p => Entry.Create(p, item.Key)).ToList();
                            if (withName)
                            {
                                EntryNameSerivce.SetName(entriesTemp, item.Key);
                            }
                            //entries.AddRange(entriesTemp);
                            entriesTemp.ForEach(p =>
                            {
                                dic.Add(p.Id, p);
                            });
                        }
                    }
                    List<Entry> entries = new List<Entry>();
                    foreach(var item in queryItems)
                    {
                        if(dic.TryGetValue(item.Id, out Entry entry))
                        {
                            entries.Add(entry);
                        }
                    }
                    return entries;
                });
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查询词条
        /// </summary>
        /// <param name="queryItem"></param>
        /// <param name="withName">结果是否带默认名字</param>
        /// <returns></returns>
        public static async Task<Entry> QueryEntryAsync(QueryItem queryItem, bool withName = true)
        {
            if (queryItem != null)
            {
                return await Task.Run(() =>
                {
                    var result = DbService.Db.GetConnection(queryItem.DbId).Queryable<DbModels.EntryDb>().First(p => p.Id == queryItem.DbId);
                    if (result == null)
                    {
                        var entry = Entry.Create(result, queryItem.DbId);
                        if (withName)
                        {
                            entry.Name = EntryNameSerivce.QueryName(entry.Id, queryItem.DbId);
                        }
                        return entry;
                    }
                    else
                    {
                        return null;
                    }
                });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加前需指定数据库id
        /// 若未设词条id将自动赋值
        /// </summary>
        /// <param name="entry"></param>
        public static void AddEntry(Entry entry)
        {
            if(string.IsNullOrEmpty(entry.Id))
            {
                entry.Id = Guid.NewGuid().ToString();
            }
            //EntryDb db = new EntryDb();
            //db.CopyFrom<EntryDb>(entry);
            DbService.Db.GetConnection(entry.DbId).Insertable(entry as EntryDb).ExecuteCommand();
        }

        public static void UpdateEntry(Entry entry)
        {
            DbService.Db.GetConnection(entry.DbId).Updateable(entry as EntryDb).ExecuteCommand();
        }

        /// <summary>
        /// 从数据库移除词条
        /// 不会删除文件
        /// </summary>
        /// <param name="entry"></param>
        public static void RemoveEntry(Entry entry)
        {
            var connet = DbService.Db.GetConnection(entry.DbId);
            DbService.Db.BeginTran();
            connet.Deleteable<EntryDb>().In(entry.Id).ExecuteCommand();
            connet.Deleteable<EntryNameDb>().Where(p=>p.Id == entry.Id).ExecuteCommand();
            connet.Deleteable<WatchHistoryDb>().Where(p=>p.Id == entry.Id).ExecuteCommand();
            connet.Deleteable<EntryLabelDb>().Where(p=>p.EntryId == entry.Id).ExecuteCommand();
            DbService.Db.CommitTran();
        }

        /// <summary>
        /// 从数据库移除词条
        /// 不会删除文件
        /// </summary>
        /// <param name="entries"></param>
        public static void RemoveEntry(List<Entry> entries)
        {
            entries.GroupBy(p => p.DbId).ToList().ForEach(g =>
              {
                  var ids = g.Select(p => p.Id);
                  var connet = DbService.Db.GetConnection(g.Key);
                  connet.Deleteable<EntryDb>().In(ids).ExecuteCommand();
                  connet.Deleteable<EntryNameDb>().In(ids).ExecuteCommand();
                  connet.Deleteable<WatchHistoryDb>().In(ids).ExecuteCommand();
              });
        }

        public static async Task<int> QueryEntryCountAsync(string dbId)
        {
            return await DbService.Db.GetConnection(dbId).Queryable<EntryDb>().CountAsync();
        }


    }
}
