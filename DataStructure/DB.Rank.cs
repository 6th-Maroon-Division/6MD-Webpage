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
    public partial class Rank {

        public Rank()
        {
            this.Users = new List<User>();
            OnCreated();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public long? DiscordRoleID { get; set; }

        public string abbreviation { get; set; }

        public int Color { get; set; }

        public virtual IList<User> Users { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
