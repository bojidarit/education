module.exports = {
    "env": {
        "browser": true,
        "es6": true
    },
    "extends": "eslint:recommended",
    "globals": {
        "Atomics": "readonly",
        "SharedArrayBuffer": "readonly"
    },
    "parserOptions": {
        "ecmaFeatures": {
            "jsx": true
        },
        "ecmaVersion": 2018,
        "sourceType": "module"
    },
    "plugins": [
        "react"
    ],
    "rules": {
        "no-extra-semi": "warn",
        "no-const-assign": "warn",
        "no-unreachable": "warn",
        "no-unused-vars": "warn",
        "no-undef": "warn",
        "no-const-assign": "warn",
        "no-this-before-super": "warn",
        "constructor-super": "warn",
        "valid-typeof": "warn"
    }
};