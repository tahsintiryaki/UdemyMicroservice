using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }
        [JsonIgnore] //Status code metoda istekte bulunulduğunda json olarak dönmeyecek. Status code zaten postmanda sağ üstte görüntüleniyor. 2. kez dönmesine gerek yok!
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessfull { get; private set; }

        public List<string> Errors { get; set; }

        //Static Factory Method
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessfull = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T> { Errors = errors, StatusCode = statusCode, IsSuccessfull = false};
        }
        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessfull = false };
        }
    }
}
