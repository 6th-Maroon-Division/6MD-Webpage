﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 14.02.2025 15:51:53
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
    public partial class Deduction {

        public Deduction()
        {
            OnCreated();
        }

        public int ID { get; set; }

        public int Points { get; set; }

        public DateTime LastDeduction { get; set; }

        public int UserID { get; set; }

        public int UserID1 { get; set; }

        public virtual User User { get; set; }

        public virtual User DeductedBy { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
