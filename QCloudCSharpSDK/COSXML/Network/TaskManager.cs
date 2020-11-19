using System;
using System.Collections.Generic;
using System.Text;

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

        public static TaskManager GetInstance()
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
