﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 04.03.2025 18:12:09
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

#nullable enable annotations
#nullable disable warnings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace DataStructure
{
    public partial class LOA {

        public LOA()
        {
            OnCreated();
        }

        public int ID { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public bool EndIndefinet { get; set; }

        public string Reason { get; set; }

        public DateTime LastEdit { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
