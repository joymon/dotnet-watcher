using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using System.Dynamic;

namespace JoymonsCode.DotNetWatcher.Core
{
    abstract public class EntityBase :DynamicObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        Dictionary<string, object> _propertyBag;
        public object this[string propertyName]
        {
            get
            {   //Code to check typed properties,if exists return that value
                return _propertyBag[propertyName];
            }
            set
            {   //Code to check typed properties,if exists set that property
                _propertyBag[propertyName] = value;
                this.OnPropertyChanged(propertyName);
            }
        }

        #region Constructor
        protected EntityBase()
        {
            this._propertyBag = new Dictionary<string, object>();

        }
        #endregion
        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> propertyExpresion)
        {
            var property = propertyExpresion.Body as MemberExpression;
            if (property == null ||!(property.NodeType==ExpressionType.MemberAccess)|| !(property.Member is PropertyInfo))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Expression must be of the form 'this.PropertyName'. Invalid expression '{0}'.",
                    propertyExpresion), "propertyBLOCKED EXPRESSION");
            }

            this.OnPropertyChanged(property.Member.Name);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {   //Code to check typed properties,if exists return that value
            return _propertyBag.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {   //Code to check typed properties,if exists set that property
            _propertyBag[binder.Name] = value;
            return true;
        }
    }
}