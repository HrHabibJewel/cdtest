/// *************************************************************************************************
/// || Creation History ||
/// -------------------------------------------------------------------------------------------------
/// Author        : Md. Habibur Rahman Jewel
/// Purpose       : All the related data will get from file/database here
/// Creation Date : 24/10/2021
/// Note          : 
/// ==================================================================================================
///  || Modification History ||
///  -------------------------------------------------------------------------------------------------
///  Sl No. Date:           Author:                     Ver: Area of Change:
///  01     
/// *************************************************************************************************


using chal_dal_model;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace chal_dal_data_context
{
    public class DistDC
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public DistDC(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public ResponseApi<dynamic> user_list()
        {
            ResponseApi<dynamic> response = new ResponseApi<dynamic>();

            #region Get all users data from json files and insert to a user list
            List<User> userList = new List<User>(); //Add all user data from json files to list
            foreach (string fileName in Directory.GetFiles(_hostingEnvironment.WebRootPath+"/data", "*.json"))
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var file = Path.Combine(webRootPath, fileName);

                using (StreamReader r = new StreamReader(file))
                {
                    string json = r.ReadToEnd();
                    User objUser = JsonConvert.DeserializeObject<User>(json);
                    objUser.userId = Path.GetFileNameWithoutExtension(fileName); 
                    userList.Add(objUser);
                }
            }
            #endregion

            if(userList.Count > 0)
            {
                response.Status = "OK";
                response.Message = "data found";
                response.Result = userList;
            }
            else
            {
                response.Status = "OK";
                response.Message = "No user found";
                response.Result = null;
            }

            return response;
        }
        
    }
}
