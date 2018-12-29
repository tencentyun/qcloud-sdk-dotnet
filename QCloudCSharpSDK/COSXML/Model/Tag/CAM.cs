using COSXML.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.CAM
{
    public sealed class CAMPolicy
    {
        //cam version, default 2.0
        private string version = "2.0";

        //principal
        private CAMPrincipal principal;

        //statemnet
        private List<CAMStatement> statements;

        public CAMPolicy()
        {
            statements = new List<CAMStatement>();
        }
        public void SetPrincipal(CAMPrincipal principal)
        {
            this.principal = principal;
        }
        public void SetStatement(CAMStatement statement)
        {
            statements.Add(statement);
        }
        public void SetVersion(string version)
        {
            this.version = version;
        }

        public string Version { get { return version; } }

        public CAMPrincipal Principal { get { return principal; } }
        public List<CAMStatement> Statement { get { return statements; } }

        public string GetPolicy()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //驼峰式
            serializerSettings.Formatting = Formatting.Indented; //json 显示风格
            string json = JsonConvert.SerializeObject(this, serializerSettings);
            QLog.D("CAM", json);
            return json;
        }
    }

    public class STSPolicy
    {
        //cam version, default 2.0
        private string version = "2.0";

        //statement
        private List<CAMStatement> statements;


        public STSPolicy()
        {
            statements = new List<CAMStatement>();
        }

        public void SetStatement(CAMStatement statement)
        {
            statements.Add(statement);
        }

        public void SetVersion(string version)
        {
            this.version = version;
        }

        public string Version { get { return version; } }

        public List<CAMStatement> Statement { get { return statements; } }

        public string GetPolicy()
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //驼峰式
            serializerSettings.Formatting = Formatting.Indented; //json 显示风格
            string json = JsonConvert.SerializeObject(this, serializerSettings);
            QLog.D("STS", json);
            return json;
        }
    }

    public class CAMPrincipal
    {
        private List<string> userList;

        public List<string> Qcs
        {
            get { return userList; }
        }

        public CAMPrincipal()
        {
            userList = new List<string>();
        }

        public void AddGranetUser(string ownUin, string subUin)
        {
            string userRecoder = String.Format("qcs::cam::uin/{0}:uin/{1}", ownUin, subUin);
            userList.Add(userRecoder);
        }

        //public void AddGrantGroupUser(string ownUin, string groupUin)
        //{
        //    string userRecoder = String.Format("qcs::cam::uin/{0}:groupid/{1}", ownUin, groupUin);
        //    qcs.Add(userRecoder);
        //}

        public void AddGrantAnonymousUser()
        {
            string userRecoder = "qcs::cam::anonymous:anonymous:anonymous:anonymous";
            userList.Add(userRecoder);
        }
    }

    public class CAMStatement
    {
        //grant actions
        private List<string> actions;

        //grant resources
        private List<string> resources;

        //grant allow or deny
        private string effect;

        public CAMStatement()
        {
            actions = new List<string>();
            resources = new List<string>();
        }

        public List<string> Action
        {
            get { return actions; }
        }

        public string Effect
        {
            get { return effect; }
        }

        public List<string> Resource
        {
            get { return resources; }
        }

        public void IsAllow(bool isAllow)
        {
            if (isAllow)
            {
                effect = "allow";
            }
            else
            {
                effect = "deny";
            }
        }

        /// <summary>
        /// 指定资源，
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="region"></param>
        /// <param name="resourcePreifix">具体的对象资源，可以是*表示所有，格式 bucket/objectKey</param>
        public void AddResource(string appid, string region, string resourcePreifix)
        {
            //qcs:project_id:service_type:region:account:resource:  project_id 被废弃
            string resourceRecoder = String.Format("qcs::cos:{0}:uid/{1}:prefix//{1}/{2}",
                region, appid, resourcePreifix);
            resources.Add(resourceRecoder);
        }

        public void AddAllResource()
        {
            resources.Clear();
            resources.Add("*");
        }
        /// <summary>
        /// 指定具体cos api
        /// </summary>
        /// <param name="action"></param>
        public void AddActions(string action)
        {
            string actionRecoder = String.Format("name/cos:{0}", action);
            actions.Add(actionRecoder);
        }
        public void AddAllActions()
        {
            actions.Clear();
            actions.Add("*");
        }
    }
}
