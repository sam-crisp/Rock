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
import { defineComponent } from "vue";
import UrlLinkBox from "../Elements/urlLinkBox";
import { getFieldEditorProps } from "./utils";
import { asBooleanOrNull } from "../Services/boolean";

export const enum ConfigurationValueKey
{
    ShouldRequireTrailingForwardSlash = "shouldRequireTrailingForwardSlash",
    ShouldAlwaysShowCondensed = "shouldAlwaysShowCondensed",
}

export const EditComponent = defineComponent({
    name: "UrlLinkField.Edit",

    components: {
        UrlLinkBox
    },

    props: getFieldEditorProps(),

    data() {
        return {
            internalValue: ""
        };
    },
    computed: {
        configAttributes (): Record<string, number | boolean>
        {
            const attributes: Record<string, number | boolean> = {};

            const shouldAlwaysShowCondensedConfig = this.configurationValues[ ConfigurationValueKey.ShouldAlwaysShowCondensed ];
            if ( shouldAlwaysShowCondensedConfig )
            {
                const shouldAlwaysShowCondensedValue = asBooleanOrNull( shouldAlwaysShowCondensedConfig ) || false;

                if ( shouldAlwaysShowCondensedValue )
                {
                    attributes.shouldAlwaysShowCondensed = shouldAlwaysShowCondensedValue;
                }
            }

            return attributes;
        },
        shouldRequireTrailingForwardSlash (): boolean
        {
            const shouldRequireTrailingForwardSlashConfig = this.configurationValues[ ConfigurationValueKey.ShouldRequireTrailingForwardSlash ];
            return asBooleanOrNull( shouldRequireTrailingForwardSlashConfig ) || false;
        }
    },
    watch: {
        internalValue() {
            this.$emit("update:modelValue", this.internalValue);
        },
        modelValue: {
            immediate: true,
            handler() {
                this.internalValue = this.modelValue || "";
            }
        }
    },
    template: `
<UrlLinkBox v-model="internalValue" v-bind="configAttributes" :shouldRequireTrailingForwardSlash="shouldRequireTrailingForwardSlash" />
`
});
