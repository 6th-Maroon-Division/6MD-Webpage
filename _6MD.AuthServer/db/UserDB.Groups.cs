﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 16.03.2025 12:40:55
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

namespace _6MD.AuthServer.DB
{
    public partial class Groups : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private Guid _Guid;

        private string _Name;

        private string _Color;

        private IList<User> _Users;

        private IList<GroupPremission> _Premissions;

        public Groups()
        {
            this._Users = new List<User>();
            this._Premissions = new List<GroupPremission>();
            OnCreated();
        }

        public Guid Guid
        {
            get
            {
                return this._Guid;
            }
            set
            {
                if (this._Guid != value)
                {
                    this.SendPropertyChanging("Guid");
                    this._Guid = value;
                    this.SendPropertyChanged("Guid");
                }
            }
        }

        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if (this._Name != value)
                {
                    this.SendPropertyChanging("Name");
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                }
            }
        }

        public string Color
        {
            get
            {
                return this._Color;
            }
            set
            {
                if (this._Color != value)
                {
                    this.SendPropertyChanging("Color");
                    this._Color = value;
                    this.SendPropertyChanged("Color");
                }
            }
        }

        public virtual IList<User> Users
        {
            get
            {
                return this._Users;
            }
            set
            {
                this._Users = value;
            }
        }

        public virtual IList<GroupPremission> Premissions
        {
            get
            {
                return this._Premissions;
            }
            set
            {
                this._Premissions = value;
            }
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion

        public virtual event PropertyChangingEventHandler PropertyChanging;

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            var handler = this.PropertyChanging;
            if (handler != null)
                handler(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanging(System.String propertyName) 
        {
            var handler = this.PropertyChanging;
            if (handler != null)
                handler(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void SendPropertyChanged(System.String propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
