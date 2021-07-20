using System;
using System.Collections.Generic;
namespace LocalCinema.Data.Model
{
    public class ResponseApiModel
    {
        public dynamic Meta { get; set; }
        public dynamic Data { get; set; }

        public dynamic Links { get; set; }
        public IEnumerable<OperationError> Errors { get; set; }

        public ResponseApiModel(dynamic meta = null, dynamic data = null, dynamic errors = null, dynamic links = null)
        {
            Meta = meta;
            Data = data;
            Errors = errors;
            Links = links;
        }
    }
}