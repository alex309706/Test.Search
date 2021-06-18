﻿using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Search.Interfaces
{
    public interface IRequestable
    {
        public string SearchingSystemName { get;}
        public long RequestTime { get; set;}
        public async Task<string> Request (int minimalExecutionTime, int maximalExecutiontime,CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return "TIMEOUT";

            return await Task.Run(() => SyncRequest(minimalExecutionTime, maximalExecutiontime, token));
        }
        string SyncRequest(int minimalExecutionTime, int maximumExecutiontime, CancellationToken token)
        {
            Stopwatch stopwatchToGetSpentTimeForRequest = new Stopwatch();
            try
            {
                stopwatchToGetSpentTimeForRequest.Start();
                Random rnd = new Random();
                //для генерации случайного времени выполнения запроса
                int minimalExecutionTimeToSeconds = minimalExecutionTime * 1000;
                int maximumExecutiontimeToSeconds = maximumExecutiontime * 1000;

                int executionTime = rnd.Next(minimalExecutionTimeToSeconds, maximumExecutiontimeToSeconds);

                
                Thread.Sleep(executionTime);

                //проверка ожидания данных от запроса
                if (token.IsCancellationRequested)
                {
                    return "TIMEOUT";
                }
                int flagToResult = rnd.Next(0, 100);

                if (flagToResult % 2 == 0)
                {
                    return "OK";
                }
                else
                {
                    return "ERROR";
                }
            }
            finally
            {
                stopwatchToGetSpentTimeForRequest.Stop();
                RequestTime = stopwatchToGetSpentTimeForRequest.ElapsedMilliseconds;
            }
       }
    }
}
