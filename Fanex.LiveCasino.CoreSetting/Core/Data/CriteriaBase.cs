// --------------------------------------------------------------------------
// <copyright file="CriteriaBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2014.7.21">Created</change>
// </history>

namespace Fanex.LiveCasino.CoreSetting.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CriteriaBase
    {
        /// <summary>
        /// Get store procedure call parameter.
        /// </summary>
        /// <returns>Store procedure parameters.</returns>
        public virtual Hashtable GetSpParams()
        {
            var storeProcNameParams = new Hashtable();
            Type type = this.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                Type typeOfPropertyAttribute = typeof(SpParamAttribute);
                var attr = (SpParamAttribute)Attribute.GetCustomAttribute(propertyInfo, typeOfPropertyAttribute);

                if (attr == null)
                {
                    storeProcNameParams.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(this, null));
                }
                else if (attr != null && !attr.Ignored && attr.SqlParamName != null)
                {
                    storeProcNameParams.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                }
            }

            return storeProcNameParams;
        }

        /// <summary>
        /// Add store procedure call parameter.
        /// </summary>
        /// <param name="storeProcParamName">Store procedure parameter name.</param>
        /// <returns>Store procedure parameters.</returns>
        public virtual Hashtable AddSpParams(Hashtable storeProcParamName)
        {
            Type type = this.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                Type typeOfPropertyAttribute = typeof(SpParamAttribute);
                var attr = (SpParamAttribute)Attribute.GetCustomAttribute(propertyInfo, typeOfPropertyAttribute);

                if (attr == null)
                {
                    storeProcParamName.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(this, null));
                }
                else if (attr != null && !attr.Ignored && attr.SqlParamName != null)
                {
                    storeProcParamName.Add(attr.SqlParamName.ToLower(), propertyInfo.GetValue(this, null));
                }
            }

            return storeProcParamName;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public virtual string GetSettingKey()
        {
            throw new NotImplementedException();
        }
    }
}
