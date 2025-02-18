﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IG.Lib
{

    /// <summary>Classes whose instances have unique string IDs.</summary>
    public interface IStringIdentifiable
    {

        /// <summary>Unique string ID of the current object; often used in logs.</summary>
        string StringId { get; }

    }

}
