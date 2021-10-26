using System;
using System.Collections.Generic;
using System.Text;

namespace chal_dal_model
{
    public class UserWithMeals
    {
        public string userId { get; set; }
        public int specificPeriodMealCount { get; set; }
        public int precedingPeriodMealCount { get; set; }
    }
}
