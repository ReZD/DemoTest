using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace DemoTest.Controllers
{
    public class UCapiController : ApiController
    {
        Random gen = new Random();

        // GET: api/UCapi
        public IEnumerable<string> Get()
        {
            return new string[] { "TEST", "SUCCESSFUL" };
        }

        // GET: api/UCapi/5
        public string Get(string id)
        {
            return "" + id;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/getstat/testmethod")]
        [HttpPost]
        public string SomeMeth()
        {

            string mStr = "";
            var res = "Result: " + mStr;
            return res;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/getstat/listservers")]
        [HttpPost]
        public JsonResult<string[]> CalcPost([FromBody] string[] values)
        {
            ConnectionStringSettingsCollection coll = ConfigurationManager.ConnectionStrings;
            string[] ret = new string[coll.Count];

            for (int i = 0; i < coll.Count; i++)
            {
                ret[i] = coll[i].Name;
            }
            var res = Json(ret);
            return res;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/getstat/{id}")]
        [HttpPost]
        public string PostOther(string id, [FromBody] string[] values)
        {
            string r = "";
            ConnectionStringSettings mConnConfig = ConfigurationManager.ConnectionStrings["" + id];
            if (mConnConfig != null)
            {
                using (var conn = new SqlConnection(mConnConfig.ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT 'Physical (RAM)', CONVERT(DECIMAL(10, 2), 1.0 * total_physical_memory_kb / 1024) ,CONVERT(DECIMAL(10, 2), 1.0 * available_physical_memory_kb / 1024) ,CONVERT(VARCHAR(5), CONVERT(DECIMAL(10, 1), 100.0 * available_physical_memory_kb / total_physical_memory_kb)) + '%', system_low_memory_signal_state FROM sys.dm_os_sys_memory";
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    r += reader.GetValue(0).ToString() + ";" + reader.GetValue(1).ToString() + ";" + reader.GetValue(2).ToString() + ";" + reader.GetValue(3).ToString() + ";" + reader.GetValue(4).ToString();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    { r = e.Message; }
                }
            }
            else
            {
                r = "Could not find server: " + id;
            }

            return r;
        }

        //private string CalcString(string[] pStrings)
        //{
        //    string r = "";
        //    foreach (string a in pStrings)
        //    {
        //        if (a != "gogogo")
        //        {
        //            r += a + ";";
        //        }
        //    }
        //    return r.Substring(0, r.Length - 1);
        //}

        // PUT: api/UCapi/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE: api/UCapi/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
