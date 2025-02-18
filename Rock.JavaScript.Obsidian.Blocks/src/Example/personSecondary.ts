﻿// <copyright>
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
import bus from "@Obsidian/Utility/bus";
import Block from "@Obsidian/Templates/block";
import SecondaryBlock from "@Obsidian/Controls/secondaryBlock";
import RockButton from "@Obsidian/Controls/rockButton";
import TextBox from "@Obsidian/Controls/textBox";
import { defineComponent } from "vue";
import { useStore } from "@Obsidian/PageState";
import { Person } from "@Obsidian/ViewModels/Entities/person";

const store = useStore();

export default defineComponent({
    name: "Example.PersonSecondary",
    components: {
        Block,
        SecondaryBlock,
        TextBox,
        RockButton
    },
    data() {
        return {
            messageToPublish: "",
            receivedMessage: ""
        };
    },
    methods: {
        receiveMessage(message: string): void {
            this.receivedMessage = message;
        },
        doPublish(): void {
            bus.publish("PersonSecondary:Message", this.messageToPublish);
            this.messageToPublish = "";

        },
        doThrowError(): void {
            throw new Error("This is an uncaught error");
        }
    },
    computed: {
        currentPerson(): Person | null {
            return store.state.currentPerson;
        },
        currentPersonName(): string {
            return this.currentPerson?.fullName || "anonymous";
        },
        imageUrl(): string {
            return this.currentPerson?.photoUrl || "/Assets/Images/person-no-photo-unknown.svg";
        },
        photoElementStyle(): string {
            return `background-image: url("${this.imageUrl}"); background-size: cover; background-repeat: no-repeat;`;
        }
    },
    created() {
        bus.subscribe<string>("PersonDetail:Message", this.receiveMessage);
    },
    template:
`<SecondaryBlock>
    <Block title="Secondary Block">
        <template #default>
            <div class="row">
                <div class="col-sm-6">
                    <p>
                        Hi, {{currentPersonName}}!
                        <div class="photo-icon photo-round photo-round-sm" :style="photoElementStyle"></div>
                    </p>
                    <p>This is a secondary block. It respects the store's value indicating if secondary blocks are visible.</p>
                    <RockButton btnType="danger" btnSize="sm" @click="doThrowError">Throw Error</RockButton>
                </div>
                <div class="col-sm-6">
                    <div class="well">
                        <TextBox label="Message" v-model="messageToPublish" />
                        <RockButton btnType="primary" btnSize="sm" @click="doPublish">Publish</RockButton>
                    </div>
                    <p>
                        <strong>Detail block says:</strong>
                        {{receivedMessage}}
                    </p>
                </div>
            </div>
        </template>
    </Block>
</SecondaryBlock>`
});
