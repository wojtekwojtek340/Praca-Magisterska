﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryServer.Measures
{
    public interface IMeasureProvider
    {
        void MeasureExecute();
    }
}