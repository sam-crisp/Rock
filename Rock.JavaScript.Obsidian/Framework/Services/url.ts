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

/**
 * Is the value a valid email address?
 * @param val
 */
export function isUrl ( val: unknown ): boolean
{
    if ( typeof val === "string" )
    {
        const re = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]*$/;
        return re.test( val.toLowerCase() );
    }

    return false;
}

/**
 * Is the value a valid email address?
 * @param val
 */
export function isUrlWithTrailingForwardSlash ( val: unknown ): boolean
{
    if ( typeof val === "string" )
    {
        const re = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]*\/$/;
        return re.test( val.toLowerCase() );
    }

    return false;
}
