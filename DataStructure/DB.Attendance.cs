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
    public partial class Attendance {

        public Attendance()
        {
            OnCreated();
        }

        public int ID { get; set; }

        public DateTime Date { get; set; }

        public DateTime LastChanged { get; set; }

        public AttendanceStatus Status { get; set; }

        public int UserID { get; set; }

        public int AttendanceTakenByID { get; set; }

        public virtual User User { get; set; }

        public virtual Operation Operation { get; set; }

        public virtual User AttendanceTakenBy { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
