﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 09.02.2025 18:23:27
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
    public partial class Section {

        public Section()
        {
            this.Slots = [];
            OnCreated();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int OperationID { get; set; }

        public virtual Operation Operation { get; set; }

        public virtual IList<Slot> Slots { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
