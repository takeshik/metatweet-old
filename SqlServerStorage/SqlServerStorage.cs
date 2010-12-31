// -*- mode: csharp; encoding: utf-8; tab-width: 4; c-basic-offset: 4; indent-tabs-mode: nil; -*-
// vim:set ft=cs fenc=utf-8 ts=4 sw=4 sts=4 et:
// $Id$
/* MetaTweet
 *   Hub system for micro-blog communication services
 * SqlServerStorage
 *   MetaTweet Storage module which is provided by Microsoft SQL Server RDBMS.
 *   Part of MetaTweet
 * Copyright © 2008-2011 Takeshi KIRIYA (aka takeshik) <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of SqlServerStorage.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XSpect.Extension;

namespace XSpect.MetaTweet.Modules
{
    public class SqlServerStorage
        : StorageModule
    {
        public String ConnectionString
        {
            get;
            set;
        }

        public String ProviderConnectionString
        {
            get
            {
                return Regex.Match(
                    this.ConnectionString,
                    "provider connection string=\"(.*)\""
                ).Groups[1].Value;
            }
        }

        public override void InitializeContext(String connectionString)
        {
            this.ConnectionString = connectionString;
            base.InitializeContext(this.ConnectionString);
        }
    }
}