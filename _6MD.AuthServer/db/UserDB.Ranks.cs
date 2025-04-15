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
    public partial class Ranks : INotifyPropertyChanging, INotifyPropertyChanged {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(System.String.Empty);

        private Guid _Guid;

        private string _Name;

        private string? _ImagePath;

        private int _Color;

        private string _Prefix;

        private IList<User> _Users;

        public Ranks()
        {
            this._Users = new List<User>();
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

        public string? ImagePath
        {
            get
            {
                return this._ImagePath;
            }
            set
            {
                if (this._ImagePath != value)
                {
                    this.SendPropertyChanging("ImagePath");
                    this._ImagePath = value;
                    this.SendPropertyChanged("ImagePath");
                }
            }
        }

        public int Color
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

        public string Prefix
        {
            get
            {
                return this._Prefix;
            }
            set
            {
                if (this._Prefix != value)
                {
                    this.SendPropertyChanging("Prefix");
                    this._Prefix = value;
                    this.SendPropertyChanged("Prefix");
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
