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
    public partial class Operation {

        public Operation()
        {
            this.Sections = new List<Section>();
            this.OPAttendances = new List<OPAttendance>();
            OnCreated();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime DateTimeOfOP { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public OpType Type { get; set; }

        public virtual User User { get; set; }

        public virtual IList<Section> Sections { get; set; }

        public virtual IList<OPAttendance> OPAttendances { get; set; }

        public virtual Attendance Attendance { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
