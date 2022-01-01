import React from 'react';
import Input from "react-validation/build/input";
import { useTranslation } from "react-i18next";

const Field = ({ name, value, setValue, validations, type = "text", min }) => {

    const { t } = useTranslation();

    return (
        <div className="form-group">
            <label htmlFor={name}>{t(name)}</label>
            <Input
                type={type}
                className="form-control"
                name={name}
                value={value[name]}
                min={min}
                onChange={setValue}
                validations={validations}
            />
        </div>
    );
}

export default Field;