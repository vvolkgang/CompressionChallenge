using ByteSizeLib;
using System;
using System.Collections.Generic;
using Core.Data;

namespace Core
{
    public abstract class BaseTest
    {
        public virtual bool IsEnabled { get; } = true;

        public abstract string TestName { get; }

        public virtual bool IsBaseline { get; set; }

        public abstract string Filename { get; }

        public abstract byte[] Execute(List<Contact> list);
    }
}
