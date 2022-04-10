using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Dtos
{
    public class ResponseDto<T>
    {
        public ResponseDto()
        {
            IsSuccessful = true;
        }

        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
