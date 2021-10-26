/// *************************************************************************************************
/// || Creation History ||
/// -------------------------------------------------------------------------------------------------
/// Author        : Md. Habibur Rahman Jewel
/// Purpose       : Request passing medium or interface
/// Creation Date : 24/10/2021
/// Note          : This interface is implemented in Business Logic Layer. So that business logic layer
///                 has the implementation of these methods which are define here.
/// ==================================================================================================
///  || Modification History ||
///  -------------------------------------------------------------------------------------------------
///  Sl No. Date:           Author:                     Ver: Area of Change:
///  01     
/// *************************************************************************************************

using chal_dal_model;

namespace chal_dal_interface
{
    public interface IDist
    {
        ResponseApi<dynamic> user_list(string pUserType, string pFromDate, string pToDate);
    }
}
