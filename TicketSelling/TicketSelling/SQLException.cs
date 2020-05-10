﻿using System;
using System.Runtime.Serialization;

namespace TicketSelling
{
    [Serializable]
    internal class SQLException : Exception
    {
        public SQLException()
        {
        }

        public SQLException(string message) : base(message)
        {
        }

        public SQLException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SQLException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public object ErrorCode { get; internal set; }
    }
}