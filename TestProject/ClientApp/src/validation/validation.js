import React from "react";
import { isEmail } from "validator";
import { Alert } from "reactstrap";

export const validateRequired = (t) => (value) => {
    if (!value) {
        return (
            <Alert color="danger" style={{ textAlign: 'center' }} className="mt-2">
                {t("This field is required!")}
            </Alert>
        );
    }
};

export const validateEmail = (t) => (value) => {
    if (!isEmail(value)) {
        return (
            <Alert color="danger" style={{ textAlign: 'center' }} className="mt-2">
                {t("This is not a valid email")}
            </Alert>
        );
    }
};

export const validateField = (t) => (value) => {
    if (value.length > 30 || value.length < 2) {
        return (
            <Alert color="danger" style={{ textAlign: 'center' }} className="mt-2">
                {t("The field must be between 2 and 30 characters")}
            </Alert>
        );
    }
};

export const validatePassword = (t) => (value) => {
    if (value.length < 8 || value.length > 18) {
        return (
            <Alert color="danger" style={{ textAlign: 'center' }} className="mt-2">
                {t("The password must be between 8 and 18 characters")}
            </Alert>
        );
    }
};