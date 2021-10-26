/// *************************************************************************************************
/// || Creation History ||
/// -------------------------------------------------------------------------------------------------
/// Author        : Md. Habibur Rahman Jewel
/// Purpose       : All the related business or validation check will be here
/// Creation Date : 24/10/2021
/// Note          : It check all the backend validation or building logic, then pass the request to
///                 Data Context for its required data.
/// ==================================================================================================
///  || Modification History ||
///  -------------------------------------------------------------------------------------------------
///  Sl No. Date:           Author:                     Ver: Area of Change:
///  01     
/// *************************************************************************************************


using chal_dal_data_context;
using chal_dal_interface;
using chal_dal_model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace chal_dal_buiseness_logic_layer
{
    public class DistBLL : IDist
    {
        private readonly DistDC _distDataContext;
        private IConfiguration _config;
        public DistBLL(DistDC distDataContext, IConfiguration configuration)
        {
            _distDataContext = distDataContext;
            _config = configuration;
        }
        public ResponseApi<dynamic> user_list(string pUserType, string pFromDate, string pToDate)
        {
            string selectedUsers = string.Empty;
            string userType = string.Empty;
            string[] userRole = new string[] { };
            int selectedUsersCount = 0;
            List<User> userList = new List<User>();
            List<UserWithMeals> userWithMealsList = new List<UserWithMeals>();
            ResponseApi<dynamic> response = new ResponseApi<dynamic>();

            //Considering all the paramter are required.
            if (!string.IsNullOrEmpty(pUserType))
            {
                if (!string.IsNullOrEmpty(pFromDate))
                {
                    if (!string.IsNullOrEmpty(pToDate))
                    {
                        response = _distDataContext.user_list(); //get all user from data folder
                        userList = response.Result;
                        
                        for (int i = 0; i < userList.Count; i++)
                        {
                            UserWithMeals objUserWithMeals = new UserWithMeals();
                            objUserWithMeals.userId = userList[i].userId;

                            DateTime specificStartDate = Convert.ToDateTime(pFromDate);
                            DateTime specificEndDate = Convert.ToDateTime(pToDate);

                            double precedingDatePeriod = (specificEndDate - specificStartDate).TotalDays;
                            DateTime precedingStartDate = specificStartDate.AddDays(-precedingDatePeriod);
                            DateTime precedingEndDate = specificStartDate.AddDays(-1);

                            for (int j = 0; j < userList[i].Calendar.dateToDayId.Count; j++)
                            {
                                var item = userList[i].Calendar.dateToDayId.ElementAt(j);

                                #region search if the individual user meal day are in the specified date range or not. if listed then count it for the user
                                if (specificStartDate <= specificEndDate)
                                {
                                    if (Convert.ToDateTime(item.Key) >= specificStartDate && Convert.ToDateTime(item.Key) <= specificEndDate)
                                    {
                                        objUserWithMeals.specificPeriodMealCount++;
                                    }
                                }
                                #endregion
                                #region search if the individual user meal day are in the preceding date range or not. if listed then count it for the user
                                if (precedingStartDate <= precedingEndDate)
                                {
                                    if (Convert.ToDateTime(item.Key) >= precedingStartDate && Convert.ToDateTime(item.Key) <= precedingEndDate)
                                    {
                                        objUserWithMeals.precedingPeriodMealCount++;
                                    }
                                }
                                #endregion

                                specificStartDate = specificStartDate.AddDays(1);
                                precedingStartDate = precedingStartDate.AddDays(1);

                            }
                            userWithMealsList.Add(objUserWithMeals);
                        }

                        if(_config.GetSection("UserType:" + pUserType).Value != null)
                        {
                            userType = _config.GetSection("UserType:" + pUserType).Value;
                        }
                        else
                        {
                            response.Status = "FAILED";
                            response.Message = "Sorry! searched user type is not listed in appsettings.json";
                            response.Result = null;
                            response.Data = null;
                            return response;
                        }

                        #region get user based on user type condition and store them to a string with comma (,) separator
                        userRole = _config.GetSection("UserType:" + pUserType + "Condition").Value.Split(",");

                        for (int i = 0; i < userWithMealsList.Count; i++)
                        {
                            switch (Convert.ToInt16(userRole[0]))
                            {
                                case 1:
                                    if (userWithMealsList[i].specificPeriodMealCount > Convert.ToInt32(userType))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 2:
                                    if (userWithMealsList[i].specificPeriodMealCount < Convert.ToInt32(userType))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 3:
                                    if (userWithMealsList[i].specificPeriodMealCount >= Convert.ToInt32(userType))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 4:
                                    if (userWithMealsList[i].specificPeriodMealCount <= Convert.ToInt32(userType))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 5:
                                    if (userWithMealsList[i].specificPeriodMealCount > Convert.ToInt32(userType) &&
                                        userWithMealsList[i].specificPeriodMealCount < Convert.ToInt32(userRole[1]))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 6:
                                    if (userWithMealsList[i].specificPeriodMealCount >= Convert.ToInt32(userType) &&
                                        userWithMealsList[i].specificPeriodMealCount < Convert.ToInt32(userRole[1]))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 7:
                                    if (userWithMealsList[i].specificPeriodMealCount > Convert.ToInt32(userType) &&
                                        userWithMealsList[i].specificPeriodMealCount <= Convert.ToInt32(userRole[1]))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                case 8:
                                    if (userWithMealsList[i].specificPeriodMealCount >= Convert.ToInt32(userType) &&
                                        userWithMealsList[i].specificPeriodMealCount <= Convert.ToInt32(userRole[1]))
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                                default:
                                    if (userWithMealsList[i].precedingPeriodMealCount >= Convert.ToInt32(userType) &&
                                        userWithMealsList[i].precedingPeriodMealCount > userWithMealsList[i].specificPeriodMealCount)
                                    {
                                        selectedUsers = ConcatUser(selectedUsers, userWithMealsList[i].userId);
                                        selectedUsersCount++;
                                    }
                                    break;
                            }
                        }
                        #endregion

                        response.Status = "OK";
                        response.Message = "Total "+selectedUsersCount + " user(s) found by the specific criteria";
                        response.Result = selectedUsers;
                        response.Data = userWithMealsList;
                        
                    }
                    else
                    {
                        response.Status = "FAILED";
                        response.Message = "Please Select To Date";
                        response.Result = null;
                    }
                }
                else
                {
                    response.Status = "FAILED";
                    response.Message = "Please Select From Date";
                    response.Result = null;
                }

            }
            else
            {
                response.Status = "FAILED";
                response.Message = "Please Select User Type";
                response.Result = null;
            }

            return response;
        }
        private string ConcatUser(string userString, string userId)
        {
            if (string.IsNullOrEmpty(userString))
                userString += userId;
            else
                userString += "," + userId;

            return userString;
        }
    }
}
