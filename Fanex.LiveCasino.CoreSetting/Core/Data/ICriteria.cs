// --------------------------------------------------------------------------
// <copyright file="ICriteria.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICriteria
    {
        /// <summary>
        /// Gets store procedure parameters.
        /// </summary>
        /// <returns>Hash table SP parameters.</returns>
        Hashtable GetSpParams();

        /// <summary>
        /// Add store procedure parameters.
        /// </summary>
        /// <param name="storeProcParamName">Store procedure parameter name.</param>
        /// <returns>SP parameters.</returns>
        Hashtable AddSpParams(Hashtable storeProcParamName);

        /// <summary>
        /// Gets setting key in order to get store procedure name and connection info.
        /// </summary>
        /// <returns>Setting key.</returns>
        string GetSettingKey();

        /// <summary>
        ///  Gets result from checking the criteria rule.
        /// </summary>
        /// <returns>Check result.</returns>
        bool IsValid();
    }
}
