import React from 'react';
import Input from "react-validation/build/input";
import { useTranslation } from "react-i18next";

const Field = ({ name, value, setValue, validations, type = "text", placeholder, minlength, maxlength, required }) => {
    const { t } = useTranslation();

    return (
        <div className="form-group">
            <label htmlFor={name}>{t(name)}</label>
            <Input
                type={type}
                className="form-control"
                name={name}
                value={value[name]}
                placeholder={placeholder}
                onChange={setValue}
                validations={validations}
                minlength={minlength}
                maxlength={maxlength}
                required={required}
            />
        </div>
    );
}

export default Field;