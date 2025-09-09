import vueParser from "vue-eslint-parser";
import vuePlugin from "eslint-plugin-vue";
import tsParser from "@typescript-eslint/parser";
import tsPlugin from "@typescript-eslint/eslint-plugin";
import prettier from "eslint-plugin-prettier";

export default [
    {
        files: ["**/*.vue"],
        languageOptions: {
            parser: vueParser,
            parserOptions: {
                parser: tsParser, // for <script lang="ts">
                ecmaVersion: "latest",
                sourceType: "module",
            },
        },
        plugins: {
            vue: vuePlugin,
            "@typescript-eslint": tsPlugin,
            prettier,
        },
        rules: {
            "vue/multi-word-component-names": "off",
            "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
            "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
        },
    },
    {
        files: ["**/*.ts", "**/*.js"],
        languageOptions: {
            parser: tsParser,
            ecmaVersion: "latest",
            sourceType: "module",
        },
        plugins: {
            "@typescript-eslint": tsPlugin,
            prettier,
        },
        rules: {
            "no-console": process.env.NODE_ENV === "production" ? "warn" : "off",
            "no-debugger": process.env.NODE_ENV === "production" ? "warn" : "off",
        },
    },
];
