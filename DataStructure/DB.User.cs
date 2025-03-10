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
    public partial class User {

        public User()
        {
            this.LOAs = new List<LOA>();
            this.Groups = new List<Groups>();
            this.Trainers = new List<Trainers>();
            this.Trainings = new List<Training>();
            this.OPAttendances = new List<OPAttendance>();
            this.Attendances = new List<Attendance>();
            this.Deductions = new List<Deduction>();
            this.DeductionsGiven = new List<Deduction>();
            this.AttendanceTaken = new List<Attendance>();
            this.Operations = new List<Operation>();
            OnCreated();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime DateJoined { get; set; }

        public bool Retired { get; set; }

        public long DiscordID { get; set; }

        public long UserPremissions { get; set; }

        public int? RankID { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public virtual Rank Rank { get; set; }

        public virtual IList<LOA> LOAs { get; set; }

        public virtual IList<Groups> Groups { get; set; }

        public virtual IList<Trainers> Trainers { get; set; }

        public virtual IList<Training> Trainings { get; set; }

        public virtual IList<OPAttendance> OPAttendances { get; set; }

        public virtual IList<Attendance> Attendances { get; set; }

        public virtual IList<Deduction> Deductions { get; set; }

        public virtual IList<Deduction> DeductionsGiven { get; set; }

        public virtual IList<Attendance> AttendanceTaken { get; set; }

        public virtual IList<Operation> Operations { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
