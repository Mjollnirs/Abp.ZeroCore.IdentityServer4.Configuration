// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: TableConfiguration.cs
// 
// Copyright 2018 Mjollnir<mjollnir@59k.org>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Abp.IdentityServer4vNext.Options
{
    /// <summary>
    /// Class to control a table's name and schema.
    /// </summary>
    public class TableConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TableConfiguration(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableConfiguration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="schema">The schema.</param>
        public TableConfiguration(string name, string schema)
        {
            Name = name;
            Schema = schema;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the schema.
        /// </summary>
        /// <value>
        /// The schema.
        /// </value>
        public string Schema { get; set; }
    }
}
