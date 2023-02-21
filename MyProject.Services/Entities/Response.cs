﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Services.Entities
{
    public class Response<T>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }

        public static Response<T> GetResult(int code, string msg, T data = default(T))
        {
            return new Response<T>
            {
                Code = code,
                Msg = msg,
                Data = data
            };
        }
    }
}
