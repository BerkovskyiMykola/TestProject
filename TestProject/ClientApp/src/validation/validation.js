import React from "react";
import { isEmail } from "validator";

export const validateRequired = (t) => (value) => {
    if (!value) {
        return (
            <div className="alert alert-danger" role="alert">
                {t("This field is required!")}
            </div>
        );
    }
};

export const validateEmail = (t) => (value) => {
    if (!isEmail(value)) {
        return (
            <div className="alert alert-danger" role="alert">
                {t("This is not a valid email. Example: example@example.com")}
            </div>
        );
    }
};

export const validateField = (t) => (value) => {
    if (value.length > 30 || value.length < 2) {
        return (
            <div className="alert alert-danger" role="alert">
                {t("The field must be between 2 and 30 characters.")}
            </div>
        );
    }
};

export const validatePassword = (t) => (value) => {
    if (value.length < 8 || value.length > 18) {
        return (
            <div className="alert alert-danger" role="alert">
                {t("The password must be between 8 and 18 characters.")}
            </div>
        );
    }
};