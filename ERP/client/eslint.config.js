import js from "@eslint/js";
import globals from "globals";
import react from "eslint-plugin-react-x";
import hooks from "eslint-plugin-react-hooks";
import unusedImports from "eslint-plugin-unused-imports";

export default [
  { ignores: ["dist/**", "coverage/**"] },
  js.configs.recommended,
  {
    files: ["**/*.{js,jsx}"],
    languageOptions: {
      ecmaVersion: "latest",
      sourceType: "module",
      parserOptions: { ecmaFeatures: { jsx: true } },
      globals: { ...globals.browser, ...globals.node, ...globals.vitest },
    },
    plugins: {
      "react-x": react,
      "react-hooks": hooks,
      "unused-imports": unusedImports,
    },
    rules: {
      "react-x/no-missing-key": "error",
      "react-x/no-duplicate-key": "error",
      "react-hooks/rules-of-hooks": "error",
      "no-console": "error",
      "no-debugger": "error",
      "no-unused-vars": [
        "error",
        { args: "none", ignoreRestSiblings: true, varsIgnorePattern: "^_" },
      ],
      "unused-imports/no-unused-imports": "error",
    },
  },
];
