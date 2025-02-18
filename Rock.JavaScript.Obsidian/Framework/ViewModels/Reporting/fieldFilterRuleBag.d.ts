//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

import { Guid } from "@Obsidian/Types";

/**
 * A single rule for a field filter. this defines the source to obtain the
 * left-hand value from, the right hand value, and the operator to use when
 * comparing them.
 */
export type FieldFilterRuleBag = {
    /** The unique identifier of this rule. */
    guid?: Guid | null;

    /**
     * The type of comparison to use when comparing the source value (left-hand
     * side) and  (right-hand side).
     */
    comparisonType: number;

    /** The right-hand side of the comparison to use when executing the rule. */
    value?: string | null;

    /** The source location for where to get the left-hand side value. */
    sourceType: number;

    /**
     * The attribute unique identifier to use as the left-hand side value
     * if  specifies an Attribute.
     */
    attributeGuid?: Guid | null;
};
