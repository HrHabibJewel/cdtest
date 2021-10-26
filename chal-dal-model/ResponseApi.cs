using System;

namespace chal_dal_model
{
    public class ResponseApi<T>
    {
        public string Status; //(OK/FAILED)
        public string Message; //Api return message will appear here
        public T Result; //Api return Result will appear here
        public T Data; //some data reference for Result
        
    }
}
