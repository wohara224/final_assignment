using System;
using System.Collections.Generic;
using NLog; // ＊追加

namespace TaskAuditApp; // *ファイルスコープ

public class TaskAnalyzer
{
    private readonly Logger _logger;
    public TaskAnalyzer(Logger logger)
    {
        // ロガー
        _logger = logger; // ＊logger -> _logger
    }
    /// <summary>
    /// 期限切れの未完了タスクをチェックし、警告ログを出力する
    /// </summary>
    public void CheckOverdueTasks(List<TaskModel> tasks)
    {
        _logger.Info("期限切れタスクのチェックを開始します。");
        Console.WriteLine("期限切れタスクのチェックを開始します。"); // ＊確認用
        foreach (var task in tasks)
        {
            // ステータスが完了以外を対象とする
            if (task.STATUS != "完了")
            {
                // 期限日の判定
                if (task.DUE_DATE > DateTime.Today)
                {
                    // 警告ログ出力
                    _logger.Warn($"【警告】期限切れタスクを発見: {task.TASK_NAME} (担当: {task.ASSIGNEE}, 期限: {task.DUE_DATE:yyyy/MM/dd})"); // ＊\削除
                    Console.WriteLine($"【警告】期限切れタスクを発見: {task.TASK_NAME} (担当: {task.ASSIGNEE}, 期限: {task.DUE_DATE:yyyy/MM/dd})"); // ＊確認用
                }
            }
        }
    }
    /// <summary>
    /// 担当者ごとの未完了タスク数を集計してログ出力する
    /// </summary>
    public void LogTaskCountByAssignee(List<TaskModel> tasks)
    {
        _logger.Info("担当者ごとの未完了タスク数集計を出力します。");
        Console.WriteLine("担当者ごとの未完了タスク数集計を出力します。"); // ＊確認用
        // 担当者ごとの未完了リスト
        var assigneeList = new List<string>();
        foreach (var t in tasks)
        {
            if (!assigneeList.Contains(t.ASSIGNEE))
            {
                assigneeList.Add(t.ASSIGNEE);
            }
        }
        // 集計
        foreach (var assignee in assigneeList)
        {
            int count = 0;
            foreach (var t in tasks)
            {
                if (t.ASSIGNEE == assignee && t.STATUS != "完了")
                {
                    count++;
                }
            }
            _logger.Info($"担当者: {assignee} / 未完了タスク数: {count}件"); // ＊\削除
            Console.WriteLine($"担当者: {assignee} / 未完了タスク数: {count}件"); // ＊確認用
        }
    }
}