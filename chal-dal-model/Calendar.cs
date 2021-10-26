using System.Collections.Generic;

namespace chal_dal_model
{
    public class Calendar
    {
        public Dictionary<string,string> dateToDayId { get; set; }
        public Dictionary<string,string> mealIdToDayId { get; set; }
    }
}
