import * as Axios from "axios";
import { DateTime } from "luxon/src/luxon";
import * as Mitt from "mitt";
import * as Vue from "vue";
import * as VueSelect from "vue-select";
import * as TSLib from "tslib";

// This shrinks the bundle by 11KB over just importing all of luxon.
const Luxon = {
    DateTime
};

export {
    Axios, // 13.7KB
    Luxon, // 60KB
    Mitt, // 374b
    Vue, // 127.2KB
    VueSelect, // 22KB
    TSLib, // 6.8KB
};
