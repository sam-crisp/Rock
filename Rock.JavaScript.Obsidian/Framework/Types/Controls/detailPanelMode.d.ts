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

/** The current mode the detail panel is displayed in. */
export const enum DetailPanelMode {
    /** The panel is in view mode to show the current entity. */
    View = 0,

    /** The panel is in edit mode to edit existing entity. */
    Edit = 1,

    /** The panel is in add mode to add a new entity. */
    Add = 2
}
