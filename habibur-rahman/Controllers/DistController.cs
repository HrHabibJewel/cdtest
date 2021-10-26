/// *************************************************************************************************
/// || Creation History ||
/// -------------------------------------------------------------------------------------------------
/// Author        : Md. Habibur Rahman Jewel
/// Purpose       : This is endpoint controller/api controller
/// Creation Date : 24/10/2021
/// Note          : This will receive request from front-end user and pass the call to Business Logic 
///                 Layer to check all the validation/building business through it's related interface
/// Live test     : https://habibur-rahman-cd.azurewebsites.net/swagger/
/// ==================================================================================================
///  || Modification History ||
///  -------------------------------------------------------------------------------------------------
///  Sl No. Date:           Author:                     Ver: Area of Change:
///  01     
/// *************************************************************************************************

using chal_dal_interface;
using chal_dal_model;
using Microsoft.AspNetCore.Mvc;

namespace habibur_rahman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistController : ControllerBase
    {
        IDist _distInterface;
        public DistController(IDist distInterface)
        {
            _distInterface = distInterface;
        }

        // GET api/dist/user_list
        [HttpGet]
        [Route("user_list")]
        public IActionResult user_list(string pUserType, string pFromDate, string pToDate)
        {
            ResponseApi<dynamic> response = new ResponseApi<dynamic>();
            response = _distInterface.user_list(pUserType, pFromDate, pToDate);
            
            return Ok(response);
        }

    }
}
