using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/* ============================================================================== 
* Copyright 2016-2019 Tencent Cloud. All Rights Reserved.
* Auth：bradyxiao 
* Date：2019/4/4 16:48:10 
* ==============================================================================*/

namespace Tencent.QCloud.Cos.Sdk.Network
{
    public class TaskManager
    {
        private Queue<HttpTask> httpTasks;
        private static TaskManager instance;
        private Object sync;
        private TaskManager()
        {
            httpTasks = new Queue<HttpTask>(30);
            sync = new Object();
        }
        public static TaskManager getInstance()
        {
            lock (typeof(TaskManager))
            {
                if (instance == null)
                {
                    instance = new TaskManager();
                }
            }
            return instance;
        }

        public void Enqueue(HttpTask httpTask)
        {
            lock (sync)
            {
                httpTasks.Enqueue(httpTask);
            }
        }

        public HttpTask Dequeue()
        {
            HttpTask httpTask = null;
            lock (sync)
            {
                if (httpTasks.Count != 0)
                {
                    httpTask = httpTasks.Dequeue();
                }
            }
            return httpTask;
        }

    }
}
