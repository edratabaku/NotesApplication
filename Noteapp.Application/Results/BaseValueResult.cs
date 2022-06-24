﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.Results
{
    public abstract class BaseValueResult<TValue> : BaseResult
    {
        public TValue Value { get; set; }
    }
}
